namespace AdventOfCode24
// https://www.reddit.com/r/adventofcode/comments/1hfhgl1/2024_day_16_part_1_alternate_test_case/

open System

type D16Pos = int*int
type D16Dir = Up | Right | Down | Left
type D16Map = (char*(D16Dir option)*int)[,]

type Day16() =
    static let mutable _sPos = (0,0)
    static let mutable _ePos = (0,0)

    static member private SPos
        with get() = _sPos
        and set(p) = _sPos <- p

    static member private EPos
        with get() = _ePos
        and set(p) = _ePos <- p

    static member nextPos ((r,c) : D16Pos) : D16Dir -> D16Pos = function
        | Up -> (r-1,c) | Right -> (r,c+1) | Down -> (r+1,c) | Left -> (r,c-1)

    static member addMap ((k,v) : int*int) (map : Map<int,int>) : Map<int,int> =
        if map.ContainsKey k then map.Add (k, map.[k]+v)
        else map.Add (k, v)

    static member neighborsToHandle ((r,c) : D16Pos) : D16Dir -> (D16Pos*D16Dir)[] = function
        | Up ->     [|((r,c-1), Left); ((r-1,c), Up); ((r,c+1), Right)|]
        | Right ->  [|((r-1,c), Up); ((r,c+1), Right); ((r+1,c), Down)|]
        | Down ->   [|((r,c+1), Right); ((r+1,c), Down); ((r,c-1), Left)|]
        | Left ->   [|((r+1,c), Down); ((r,c-1), Left); ((r-1,c), Up)|]

    static member Star1 (input : string[]) : string =
        //let map = input |> array2D |> Array2D.mapi (fun r c s ->
        //    if s = 'S' then Day16.SPos <- (r,c); (s, Some Right, 0) else (s, None, Int32.MaxValue))

        //let mutable queue = [Day16.SPos]
        //let mutable scores = Map.empty<int,int>

        //while not queue.IsEmpty do
        //    let (r,c) = queue.Head
        //    queue <- queue.Tail
        //    let (s,d,sc) = map.[r,c]
        //    if s = 'E' then
        //        scores <- Day16.addMap (sc, 1) scores
        //    else
        //        Day16.neighborsToHandle (r,c) sc d.Value
        //        |> Array.iter (fun ((rN,cN), dN, scN) ->
        //            let (sO, dO, scO) = map.[rN,cN]
        //            if sO = '#' then ()
        //            else
        //                if dO.IsSome && scN >= scO then ()
        //                else queue <- (rN,cN)::queue
        //                     map.[rN,cN] <- (sO, Some dN, scN))

        //Map.minKeyValue scores |> fst |> string
        ""

    static member Star2 (input : string[]) : string = // 432 too low
        let map = input |> array2D |> Array2D.mapi (fun r c s ->
            if s = 'S' then Day16.SPos <- (r,c); (s, Some Right, 0)
            else
                if s = 'E' then Day16.EPos <- (r,c)
                (s, None, Int32.MaxValue))

        let mutable queue = [Day16.SPos]
        let mutable scores = Map.empty<int,int>

        while not queue.IsEmpty do
            let (r,c) = queue.Head
            queue <- queue.Tail
            let (s,d,sc) = map.[r,c]
            if s = 'E' then
                scores <- Day16.addMap (sc, 1) scores
            else
                let nbs = Day16.neighborsToHandle (r,c) d.Value

                let (rF,cF) = fst nbs.[1]
                match map.[rF,cF] with ('#', _, _) -> map.[r,c] <- (s, d, sc+1000) | _ -> ()

                nbs|> Array.iter (fun ((rN,cN), dN) ->
                    let (sO, dO, scO) = map.[rN,cN]
                    if sO = '#' then ()
                    else
                        let scN =
                            if dN <> d.Value then sc+1001
                            else sc+1

                        if dO.IsSome && scN >= scO then ()
                        else queue <- (rN,cN)::queue
                             map.[rN,cN] <- (sO, Some dN, scN))

        let rec helper ((r,c) : D16Pos) (map : D16Map) : unit =
            let (s,d,sc) = map.[r,c]
            if s = 'S' then map.[r,c] <- ('X', d, sc)
            elif s = 'X' then ()
            else
                map.[r,c] <- ('X', d, sc)

                let nbs =
                    [(-1,0); (0,1); (1,0); (0,-1)]
                    |> List.fold (fun acc (i,j) ->
                        let (sT,dT,scT) = map.[r+i,c+j]
                        if sT = '#' then acc
                        else
                            let scT =
                                if List.contains d.Value [Up;Down] && List.contains dT.Value [Up;Down] && j <> 0 then scT+1000
                                elif List.contains d.Value [Right;Left] && List.contains dT.Value [Right;Left] && i <> 0 then scT+1000
                                else scT

                            if scT < sc then (r+i,c+j)::acc else acc) []

                nbs |> List.iter (fun p -> helper p map)
                    
                    
                    //Array.map (fun (i,j) -> ((r+i,c+j), map.[r+i,c+j]))

                //let groups = nbs |> Array.groupBy (fun (p, (_,_,sc)) -> sc) |> Map.ofArray

                //let foo = Map.keys groups |> Seq.filter (fun n -> n < sc) |> Seq.toArray

                //foo |> Array.iter (fun n -> groups.[n] |> Array.iter (fun (p,_) -> helper p map))
        
        helper Day16.EPos map

        let foo = map |> Array2D.map (fun (s,_,_) -> s)
        foo |> Day16.printMap

        let rec counter ((r,c) : D16Pos) ((rL,cL) : D16Pos) (acc : int) (map : D16Map) =
            if r >= rL then acc
            elif c >= cL then counter (r+1, 0) (rL,cL) acc map
            else
                let (s, _, _) = map.[r,c]
                counter (r, c+1) (rL,cL) (if s = 'X' then acc+1 else acc) map

        counter (0,0) (Array2D.length1 map, Array2D.length2 map) 0 map |> string

    static member private printMap (map : char[,]) : unit =
        for i in [0..(Array2D.length1 map)-1] do
            for j in [0..(Array2D.length2 map)-1] do
                printf "%c" map.[i,j]
            printfn ""
