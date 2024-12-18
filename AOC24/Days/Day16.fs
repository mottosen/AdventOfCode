namespace AdventOfCode24
// https://www.reddit.com/r/adventofcode/comments/1hfhgl1/2024_day_16_part_1_alternate_test_case/

open System

type D16Pos = int*int
type D16Dir = Up | Right | Down | Left

type D16Node = {MinDist : int; Dir : D16Dir option; Paths : D16Pos list list}

type Day16() =
    static let mutable _sPos = (0,0)
    static let mutable _ePos = (0,0)

    static member private SPos
        with get() = _sPos
        and set(p) = _sPos <- p

    static member private EPos
        with get() = _ePos
        and set(p) = _ePos <- p
    
    static member private nextPos ((r,c) : D16Pos) : D16Dir -> D16Pos = function
        | Up -> (r-1,c) | Right -> (r,c+1) | Down -> (r+1,c) | Left -> (r,c-1)
    
    static member private connections ((r,c) : D16Pos) (blocked : bool) (dir : D16Dir) : (D16Pos*D16Dir*int)[] =
        let dst = if blocked then 1 else 1001
        match dir with
        | Up ->    [|((r,c-1), Left,  dst); ((r-1,c), Up,    1); ((r,c+1), Right, dst)|]
        | Right -> [|((r-1,c), Up,    dst); ((r,c+1), Right, 1); ((r+1,c), Down,  dst)|]
        | Down ->  [|((r,c+1), Right, dst); ((r+1,c), Down,  1); ((r,c-1), Left,  dst)|]
        | Left ->  [|((r+1,c), Down,  dst); ((r,c-1), Left,  1); ((r-1,c), Up,    dst)|]

    // unvisited : unvisited nodes
    // queue : nodes to be handled, indexed by distance
    // graph : nodes
    // map : input map
    static member private path (unvisited : D16Pos list) (map : char[,]) (queue : Map<int, D16Pos list>) (graph : Map<D16Pos, D16Node>) : Map<D16Pos, D16Node> =
        if queue.IsEmpty then graph // no more to handle
        else
            let (dst, nodePos::rest) = queue |> Map.minKeyValue
            let (x,y) = nodePos
            let queue = if rest.IsEmpty then queue |> Map.remove dst else queue.Add (dst, rest)

            if not (List.contains nodePos unvisited) then Day16.path unvisited map queue graph
            else // position is unvisited
                let node = graph.[nodePos]
                let unvisited = unvisited |> List.filter (fun p -> p <> nodePos)

                let blocked = let (i,j) = Day16.nextPos nodePos node.Dir.Value in map.[i,j] = '#'

                let (queue,graph) =
                    Day16.connections nodePos blocked node.Dir.Value
                    |> Array.map (fun (nbPos, nbDir, nbD) ->
                        let (r,c) = nbPos
                        if map.[r,c] = '#' then None
                        else
                            let nbD = let (i,j) = Day16.nextPos nbPos nbDir in if (r,c) <> Day16.EPos && map.[i,j] = '#' then nbD+1000 else nbD
                            (nbPos, nbDir, node.MinDist+nbD) |> Some)
                    |> Array.fold (fun (queue : Map<int, D16Pos list>, graph : Map<D16Pos, D16Node>) (o : (D16Pos*D16Dir*int) option) ->
                        if o.IsNone then (queue,graph)
                        else
                            let (nbPos,nbDir,nbND) = o.Value
                            let queue = queue.Add (nbND, nbPos::(if (queue.ContainsKey nbND) then queue.[nbND] else []))
                            let graph =
                                let nb = graph.[nbPos]
                                if nbND <= nb.MinDist then
                                    let updatedPaths = nb.Paths @ (node.Paths |> List.map (fun path -> nbPos::path))
                                    graph.Add (nbPos, {MinDist = nbND; Dir = Some nbDir; Paths = updatedPaths})
                                else graph

                            (queue,graph)) (queue,graph)
                
                Day16.path unvisited map queue graph

    static member Star1 (input : string[]) : string =
        let parsed = input |> array2D
        let mutable graph = Map.empty<D16Pos,D16Node>
        let mutable unvisited : D16Pos list = []
        let mutable queue = Map.empty<int,D16Pos list>

        parsed |> Array2D.iteri (fun r c s ->
            if s = 'E' then
                Day16.SPos <- (r,c)
                let (i,j) = Day16.nextPos (r,c) Left
                let dist = if parsed.[i,j] = '#' then 1000 else 0
                graph <- graph.Add ((r,c), {MinDist = dist; Dir = Some Left; Paths = [[(r,c)]]})
                queue <- queue.Add (dist, [(r,c)])
                unvisited <- (r,c)::unvisited
            elif s = 'S' then
                Day16.EPos <- (r,c)
                graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue; Dir = None; Paths = []})
                unvisited <- (r,c)::unvisited
            elif s = '.' then
                graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue; Dir = None; Paths = []})
                unvisited <- (r,c)::unvisited)

        let graph = Day16.path unvisited parsed queue graph

        if false then
            Day16.mapGraph graph parsed
        else
            for (i,j) in (graph.[Day16.EPos].Paths |> List.concat |> List.distinct) do
                parsed.[i,j] <- 'X'
            parsed |> Day16.printMap
        
        graph.[Day16.EPos].Paths |> List.concat |> List.distinct |> List.length |> printfn "%A"
        graph.[Day16.EPos].MinDist |> string

    static member Star2 (input : string[]) : string = // 442
        ""

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