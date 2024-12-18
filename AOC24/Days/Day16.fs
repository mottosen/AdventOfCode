namespace AdventOfCode24

open System

type D16Pos = int*int
type D16Dir = Up | Right | Down | Left
type D16Node = {MinDist : int; Dir : D16Dir option; Prev : D16Pos; Paths : D16Pos list list}

type Day16() =
    static let mutable _sPos = (0,0)
    static let mutable _ePos = (0,0)

    static member private SPos
        with get() = _sPos
        and set(p) = _sPos <- p

    static member private EPos
        with get() = _ePos
        and set(p) = _ePos <- p
    
    static member private connections ((r,c) : D16Pos) : D16Dir -> (D16Pos*D16Dir*int)[] = function
        | Up ->    [|((r,c-1), Left,  1001); ((r-1,c), Up,    1); ((r,c+1), Right, 1001)|]
        | Right -> [|((r-1,c), Up,    1001); ((r,c+1), Right, 1); ((r+1,c), Down,  1001)|]
        | Down ->  [|((r,c+1), Right, 1001); ((r+1,c), Down,  1); ((r,c-1), Left,  1001)|]
        | Left ->  [|((r+1,c), Down,  1001); ((r,c-1), Left,  1); ((r-1,c), Up,    1001)|]

    static member private path (unvisited : D16Pos list) (map : char[,]) (queue : Map<int, D16Pos list>) (graph : Map<D16Pos, D16Node>) : Map<D16Pos, D16Node> =
        if queue.IsEmpty then graph
        else
            let (dst, nodePos::rest) = queue |> Map.minKeyValue
            let queue = if rest.IsEmpty then queue |> Map.remove dst else queue.Add (dst, rest)

            if not (List.contains nodePos unvisited) then Day16.path unvisited map queue graph
            else
                let node = graph.[nodePos]
                let unvisited = unvisited |> List.filter (fun p -> p <> nodePos)

                let paths = graph.[node.Prev].Paths |> List.map (fun path -> nodePos::path)

                let (queue,graph) =
                    Day16.connections nodePos node.Dir.Value |> Array.filter (fun ((r,c), _, _) -> map.[r,c] <> '#')
                    |> Array.fold (fun (queue : Map<int, D16Pos list>, graph : Map<D16Pos, D16Node>) (nbPos,nbDir,nbND) ->
                            let nbND = node.MinDist+nbND
                            let queue = queue.Add (nbND, nbPos::(if (queue.ContainsKey nbND) then queue.[nbND] else []))
                            let graph =
                                let nb = graph.[nbPos]
                                if nbND <= nb.MinDist then
                                    let mergedPaths = nb.Paths @ (paths |> List.map (fun path -> nbPos::path))
                                    graph.Add (nbPos, {MinDist = nbND; Dir = Some nbDir; Prev = nodePos; Paths = mergedPaths})
                                else graph
                            (queue,graph)) (queue,graph)

                let graph =
                    if nodePos = Day16.EPos then graph
                    else graph.Add (nodePos, {MinDist = node.MinDist+1000; Dir = node.Dir; Prev = node.Prev; Paths = node.Paths})
                
                Day16.path unvisited map queue graph

    static member Star1 (input : string[]) : string =
        let parsed = input |> array2D
        let mutable graph = Map.empty<D16Pos,D16Node>
        let mutable unvisited : D16Pos list = []
        let mutable queue = Map.empty<int,D16Pos list>

        parsed |> Array2D.iteri (fun r c s ->
            if s = 'S' then
                Day16.SPos <- (r,c)
                graph <- graph.Add ((r,c), {MinDist = 0; Dir = Some Right; Prev = (r,c); Paths = [[(r,c)]]})
                queue <- queue.Add (0, [(r,c)])
                unvisited <- (r,c)::unvisited
            elif s = 'E' then
                Day16.EPos <- (r,c)
                graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue; Dir = None; Prev = (r,c); Paths = []})
                unvisited <- (r,c)::unvisited
            elif s = '.' then
                graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue; Dir = None; Prev = (r,c); Paths = []})
                unvisited <- (r,c)::unvisited)

        let graph = Day16.path unvisited parsed queue graph
        
        graph.[Day16.EPos].MinDist |> string

    static member Star2 (input : string[]) : string =
        let parsed = input |> array2D
        let mutable graph = Map.empty<D16Pos,D16Node>
        let mutable unvisited : D16Pos list = []
        let mutable queue = Map.empty<int,D16Pos list>

        parsed |> Array2D.iteri (fun r c s ->
            if s = 'S' then
                Day16.SPos <- (r,c)
                graph <- graph.Add ((r,c), {MinDist = 0; Dir = Some Right; Prev = (r,c); Paths = [[(r,c)]]})
                queue <- queue.Add (0, [(r,c)])
                unvisited <- (r,c)::unvisited
            elif s = 'E' then
                Day16.EPos <- (r,c)
                graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue; Dir = None; Prev = (r,c); Paths = []})
                unvisited <- (r,c)::unvisited
            elif s = '.' then
                graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue; Dir = None; Prev = (r,c); Paths = []})
                unvisited <- (r,c)::unvisited)

        let graph = Day16.path unvisited parsed queue graph
        
        graph.[Day16.EPos].Paths |> List.concat |> List.distinct |> List.length |> string
