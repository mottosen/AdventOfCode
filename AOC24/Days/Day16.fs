namespace AdventOfCode24
// https://www.reddit.com/r/adventofcode/comments/1hfhgl1/2024_day_16_part_1_alternate_test_case/

open System

type D16Pos = int*int
type D16Dir = Up | Right | Down | Left
type D16Map = (char*(D16Pos list)*int)[,]

type D16Node = {Weight:int; Dir:D16Dir option; Trail : D16Pos list; Trailed : bool; Blocked : bool}

type Day16() =
    static let mutable _sPos = (0,0)
    static let mutable _ePos = (0,0)

    static let mutable _graph = Map.empty<D16Pos,D16Node>
    static member Graph
        with get() = _graph
        and set(g) = _graph <- g

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

    static member private nextPos ((r,c) : D16Pos) : D16Dir -> D16Pos = function
        | Up -> (r-1,c) | Right -> (r,c+1) | Down -> (r+1,c) | Left -> (r,c-1)

    static member path (map : char[,]) (graph : Map<D16Pos,D16Node>) : int*(D16Pos list) =
        let mutable minWeight = Int32.MaxValue
        graph |> Map.iter (fun k v -> let w = v.Weight in if w < minWeight && not v.Trailed then minWeight <- w)

        let (r,c) = graph |> Map.findKey (fun k v -> v.Weight = minWeight && not v.Trailed)

        if (r,c) = Day16.EPos then
            Day16.Graph <- graph
            let tmp = graph.[r,c]
            (tmp.Weight, (r,c)::tmp.Trail)
        else
            let node = graph.[r,c]
            let node = {Weight = node.Weight; Dir = node.Dir; Trail = node.Trail; Trailed = true; Blocked = node.Blocked}
            let graph = graph.Add ((r,c), node)

            let nodeDir = node.Dir.Value
            let nodeWeight = node.Weight
            let nbs = Day16.connections (r,c) nodeDir |> Array.map (fun (i,j) -> if map.[i,j] = '#' then None else Some (i,j))
            if nbs |> Array.forall (fun o -> o.IsNone) then
                let o = graph.[r,c]
                Day16.path map (graph.Add ((r,c), {Weight = o.Weight; Dir = o.Dir; Trail = o.Trail; Trailed = true; Blocked = o.Blocked}))
            else
                let foo = nbs |> Array.mapi (fun dD po ->
                    if po.IsSome then
                        let (i,j) = po.Value
                        let n = graph.[i,j]

                        let nDir = Day16.changeDir dD nodeDir

                        let (x,y) = Day16.nextPos (i,j) nDir
                        let blocked = map.[x,y] = '#' && not ((i,j) = Day16.EPos)

                        let nodeWeight =
                            if blocked then nodeWeight+1000
                            else nodeWeight

                        let newWeight =
                            if node.Blocked || blocked || dD = 1 then nodeWeight+1
                            else nodeWeight+1001
                        if newWeight <= n.Weight then
                            [((i,j), {Weight = newWeight; Dir = Some nDir; Trail = ((r,c)::node.Trail @ n.Trail) |> List.distinct; Trailed = false; Blocked = blocked})]
                        else []
                    else [])

                foo
                |> Array.fold (fun (g : Map<D16Pos,D16Node>) f ->
                    f |> List.fold (fun acc pair -> acc.Add pair) g) graph
                |> Day16.path map

    static member Star1 (input : string[]) : string =
        let parsed = input |> array2D
        let mutable graph = Map.empty<D16Pos,D16Node>

        parsed |> Array2D.iteri (fun r c s ->
            if s = 'S' then
                Day16.SPos <- (r,c)
                let (i,j) = Day16.nextPos (r,c) Right
                let blocked = parsed.[i,j] = '#'
                graph <- graph.Add ((r,c), {Weight = (if blocked then 1000 else 0); Dir = Some Right; Trail = []; Trailed = false; Blocked = blocked})
            elif s = 'E' then
                Day16.EPos <- (r,c)
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false; Blocked = false})
            elif s = '.' then
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false; Blocked = false}))

        Day16.path parsed graph
        |> fst |> string

    static member Star2 (input : string[]) : string = // 432 too low
        let parsed = input |> array2D
        let mutable graph = Map.empty<D16Pos,D16Node>

        parsed |> Array2D.iteri (fun r c s ->
            if s = 'S' then
                Day16.SPos <- (r,c)
                let (i,j) = Day16.nextPos (r,c) Right
                graph <- graph.Add ((r,c), {Weight = 0; Dir = Some Right; Trail = []; Trailed = false; Blocked = parsed.[i,j] = '#'})
            elif s = 'E' then
                Day16.EPos <- (r,c)
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false; Blocked = false})
            elif s = '.' then
                graph <- graph.Add ((r,c), {Weight = Int32.MaxValue; Dir = None; Trail = []; Trailed = false; Blocked = false}))

        let res = Day16.path parsed graph

        //for (i,j) in (snd res) do
        //    parsed.[i,j] <- 'X'
        //parsed |> Day16.printMap
        //parsed |> Day16.mapGraph Day16.Graph

        // off by 10 because of edge case with eg. >^> being charged for double rotation, wrongly
        let rec backtracker (s : char) (sc : int) ((r,c) : D16Pos) (map : char[,]) : D16Pos list =
            if (r,c) = Day16.SPos then [(r,c)]
            elif map.[r,c] = '#' then []
            else
                let runs =
                    if s = '^' then
                        if [map.[r,c-1]; map.[r,c+1]] |> List.forall (fun s' -> s' = '>') then [Some (r,c-1)]
                        elif [map.[r,c-1]; map.[r,c+1]] |> List.forall (fun s' -> s' = '<') then [Some (r,c+1)]
                        else
                            [
                                let (i,j) = (r,c-1) in if map.[i,j] = '>' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                                let (i,j) = (r+1,c) in if map.[i,j] <> '#' && Day16.Graph.[i,j].Weight < sc then Some (i,j) else None
                                let (i,j) = (r,c+1) in if map.[i,j] = '<' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                            ]
                    elif s = '>' then
                        if [map.[r-1,c]; map.[r+1,c]] |> List.forall (fun s' -> s' = 'v') then [Some (r-1,c)]
                        elif [map.[r-1,c]; map.[r+1,c]] |> List.forall (fun s' -> s' = '^') then [Some (r+1,c)]
                        else
                            [
                                let (i,j) = (r-1,c) in if map.[i,j] = 'v' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                                let (i,j) = (r,c-1) in if map.[i,j] <> '#' && Day16.Graph.[i,j].Weight < sc then Some (i,j) else None
                                let (i,j) = (r+1,c) in if map.[i,j] = '^' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                            ]

                    elif s = 'v' then
                        if [map.[r,c-1]; map.[r,c+1]] |> List.forall (fun s' -> s' = '>') then [Some (r,c-1)]
                        elif [map.[r,c-1]; map.[r,c+1]] |> List.forall (fun s' -> s' = '<') then [Some (r,c+1)]
                        else
                            [
                                let (i,j) = (r,c+1) in if map.[i,j] = '<' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                                let (i,j) = (r-1,c) in if map.[i,j] <> '#' && Day16.Graph.[i,j].Weight < sc then Some (i,j) else None
                                let (i,j) = (r,c-1) in if map.[i,j] = '>' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                            ]
                    else // '<'
                        if [map.[r-1,c]; map.[r+1,c]] |> List.forall (fun s' -> s' = 'v') then [Some (r-1,c)]
                        elif [map.[r-1,c]; map.[r+1,c]] |> List.forall (fun s' -> s' = '^') then [Some (r+1,c)]
                        else
                            [
                                let (i,j) = (r-1,c) in if map.[i,j] = 'v' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                                let (i,j) = (r,c+1) in if map.[i,j] <> '#' && Day16.Graph.[i,j].Weight < sc then Some (i,j) else None
                                let (i,j) = (r+1,c) in if map.[i,j] = '^' && Day16.Graph.[i,j].Weight < sc - 1000 then Some (i,j) else None
                            ]

                (r,c) :: (runs |> List.filter (fun o -> o.IsSome)
                |> List.map (fun o -> let (i,j) = o.Value in backtracker map.[i,j] Day16.Graph.[i,j].Weight (i,j) map)
                |> List.concat) |> List.distinct

        let (i,j) = Day16.EPos
        let bam = backtracker parsed.[i,j] (res |> fst) (i,j) parsed
        printfn "%A" bam
        bam |> List.length |> printfn "%A"

        
        snd res |> List.length |> string

    static member private mapGraph (graph : Map<D16Pos,D16Node>) (map : char[,]) : unit =
        let helper : D16Dir -> char = function
            | Up -> '^' | Right -> '>' | Down -> 'v' | Left -> '<'

        graph |> Map.iter (fun (r,c) node -> if node.Dir.IsSome then map.[r,c] <- helper (node.Dir.Value))
        map |> Day16.printMap

    static member private printMap (map : char[,]) : unit =
        for i in [0..(Array2D.length1 map)-1] do
            for j in [0..(Array2D.length2 map)-1] do
                printf "%c" map.[i,j]
            printfn ""
