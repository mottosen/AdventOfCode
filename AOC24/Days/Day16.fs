namespace AdventOfCode24
// https://www.reddit.com/r/adventofcode/comments/1hfhgl1/2024_day_16_part_1_alternate_test_case/

open System

type D16Pos = int*int
type D16Dir = Up | Right | Down | Left
type D16Map = (char*(D16Pos list)*int)[,]

type D16Node = {Weight:int; Dir:D16Dir option; Trail : D16Pos list; Trailed : bool}

type Day16() =
    static let mutable _sPos = (0,0)
    static let mutable _ePos = (0,0)

    static member private SPos
        with get() = _sPos
        and set(p) = _sPos <- p

    static member private EPos
        with get() = _ePos
        and set(p) = _ePos <- p

    static member private connections ((r,c) : D16Pos) : D16Dir -> D16Pos[] = function
        | Up ->    [|(r,c-1); (r-1,c); (r,c+1)|]
        | Right -> [|(r-1,c); (r,c+1); (r+1,c)|]
        | Down ->  [|(r,c+1); (r+1,c); (r,c-1)|]
        | Left ->  [|(r+1,c); (r,c-1); (r-1,c)|]

    static member private changeDir (c : int) : D16Dir -> D16Dir = function
        | Up -> if c = 0 then Left elif c = 1 then Up elif c = 2 then Right else failwith "should not happen"
        | Right -> if c = 0 then Up elif c = 1 then Right elif c = 2 then Down else failwith "should not happen"
        | Down -> if c = 0 then Right elif c = 1 then Down elif c = 2 then Left else failwith "should not happen"
        | Left -> if c = 0 then Down elif c = 1 then Left elif c = 2 then Up else failwith "should not happen"

    static member path (map : char[,]) (graph : Map<D16Pos,D16Node>) : int*(D16Pos list) =
        let mutable minWeight = Int32.MaxValue
        graph |> Map.iter (fun k v -> let w = v.Weight in if w < minWeight && not v.Trailed then minWeight <- w)

        let (r,c) = graph |> Map.findKey (fun k v -> v.Weight = minWeight && not v.Trailed)

        if (r,c) = Day16.EPos then
            let tmp = graph.[r,c]
            (tmp.Weight, (r,c)::tmp.Trail)
        else
            let node = graph.[r,c]
            let node = {Weight = node.Weight; Dir = node.Dir; Trail = node.Trail; Trailed = true}
            let graph = graph.Add ((r,c), node)

            let nodeDir = node.Dir.Value
            let nbs = Day16.connections (r,c) nodeDir |> Array.map (fun p -> Some p)
            if nbs |> Array.forall (fun o -> o.IsNone) then
                let o = graph.[r,c]
                Day16.path map (graph.Add ((r,c), {Weight = o.Weight; Dir = o.Dir; Trail = o.Trail; Trailed = true}))
            else
                let foo = nbs |> Array.mapi (fun dD po ->
                    if po.IsSome then
                        let (i,j) = po.Value
                        if map.[i,j] = '#' then
                            ((r,c), {Weight = node.Weight+1000; Dir = node.Dir; Trail = node.Trail; Trailed = node.Trailed}) |> Some
                        else
                            let n = graph.[i,j]

                            let nDir = Day16.changeDir dD nodeDir

                            let newWeight =
                                if dD = 1 then node.Weight+1
                                else node.Weight+1001
                                
                            if newWeight <= n.Weight then
                                ((i,j), {Weight = newWeight; Dir = Some nDir; Trail = ((r,c)::node.Trail @ n.Trail) |> List.distinct; Trailed = false}) |> Some
                            else None
                    else None)

                foo
                |> Array.filter (fun o -> o.IsSome)
                |> Array.fold (fun (g : Map<D16Pos,D16Node>) f ->
                    let (k,v) = f.Value
                    g.Add (k,v)) graph
                |> Day16.path map

    static member Star1 (input : string[]) : string =
        let parsed = input |> array2D
        let mutable graph = Map.empty<D16Pos,D16Node>

        parsed |> Array2D.iteri (fun r c s ->
            if s = 'S' then
                Day16.SPos <- (r,c)
                graph <- graph.Add ((r,c), {Weight = 0; Dir = Some Right; Trail = []; Trailed = false})
            elif s = 'E' then
                Day16.EPos <- (r,c)
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false})
            elif s = '.' then
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false}))

        Day16.path parsed graph
        |> fst |> string

    static member Star2 (input : string[]) : string = // 432 too low
        let parsed = input |> array2D
        let mutable graph = Map.empty<D16Pos,D16Node>

        parsed |> Array2D.iteri (fun r c s ->
            if s = 'S' then
                Day16.SPos <- (r,c)
                graph <- graph.Add ((r,c), {Weight = 0; Dir = Some Right; Trail = []; Trailed = false})
            elif s = 'E' then
                Day16.EPos <- (r,c)
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false})
            elif s = '.' then
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false}))

        let res = Day16.path parsed graph

        for (i,j) in (snd res) do
            parsed.[i,j] <- 'X'
        parsed |> Day16.printMap

        snd res |> List.length |> string

    static member private printMap (map : char[,]) : unit =
        for i in [0..(Array2D.length1 map)-1] do
            for j in [0..(Array2D.length2 map)-1] do
                printf "%c" map.[i,j]
            printfn ""
