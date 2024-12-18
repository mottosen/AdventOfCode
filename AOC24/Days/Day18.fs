namespace AdventOfCode24

open System
open System.Text.RegularExpressions

type D18Pos = int*int
type D18Node = {MinDist : int}

type Day18() =
    static let mutable _bounds = (0,0)
    static let mutable _reads = 0
    static let mutable _end = (0,0)

    static member private Bounds
        with get() = _bounds
        and set(b) = _bounds <- b

    static member private Reads
        with get() = _reads
        and set(r) = _reads <- r-1

    static member private EPos
        with get() = _end
        and set(e) = _end <- e

    static member private connections ((r,c) : D18Pos) : D18Pos list = 
        [(1,0); (0,1); (-1,0); (0,-1)] |> List.map (fun (i,j) -> (r+i,c+j))

    static member private path (unvisited : D18Pos list) (map : char[,]) (queue : Map<int, D18Pos list>) (graph : Map<D18Pos, D18Node>) : Map<D18Pos, D18Node> =
        if queue.IsEmpty then graph
        else
            let (dst, nodePos::rest) = queue |> Map.minKeyValue
            let queue = if rest.IsEmpty then queue |> Map.remove dst else queue.Add (dst, rest)

            if not (List.contains nodePos unvisited) then Day18.path unvisited map queue graph
            else
                let node = graph.[nodePos]
                let unvisited = unvisited |> List.filter (fun p -> p <> nodePos)

                let (queue,graph) =
                    Day18.connections nodePos |> List.filter (fun (i,j) -> Day18.inBounds (i,j) && map.[i,j] <> '#')
                    |> List.fold (fun (queue : Map<int, D18Pos list>, graph : Map<D18Pos, D18Node>) (i,j) ->
                            let nbND = node.MinDist+1
                            let queue = queue.Add (nbND, (i,j)::(if (queue.ContainsKey nbND) then queue.[nbND] else []))
                            let graph =
                                let nb = graph.[i,j]
                                if nbND <= nb.MinDist then
                                    graph.Add ((i,j), {MinDist = nbND})
                                else graph
                            (queue,graph)) (queue,graph)

                Day18.path unvisited map queue graph

    static member inBounds ((r,c) : D18Pos) : bool =
        let (rL,cL) = Day18.Bounds
        r >= 0 && c >= 0 && r < rL && c < cL

    static member Star1 (input : string[]) : string =
        Day18.Bounds <- (71,71)
        Day18.Reads <- 1024
        Day18.EPos <- (70,70)
        
        let mutable graph = Map.empty
        let mutable unvisited : D18Pos list = []
        let mutable queue = Map.ofList [(0, [(0,0)])]

        let fails = input.[..Day18.Reads] |> Array.map (fun line -> let m = Regex.Matches(line, "\d+") in (m.[0].Value |> Int32.Parse, m.[1].Value |> Int32.Parse))
        let map = let (r,c) = Day18.Bounds in Array2D.init r c (fun c r ->
            graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue})
            unvisited <- (r,c)::unvisited
            if Array.contains (r,c) fails then '#' else '.')

        graph <- graph.Add ((0,0), {MinDist = 0})

        let res = Day18.path unvisited map queue graph
        res.[Day18.EPos].MinDist |> string

    static member Star2 (input : string[]) : string =
        Day18.Bounds <- (71,71)
        Day18.EPos <- (70,70)

        let fails = input |> Array.map (fun line -> let m = Regex.Matches(line, "\d+") in (m.[1].Value |> Int32.Parse, m.[0].Value |> Int32.Parse))

        let mutable graph = Map.empty
        let mutable unvisited : D18Pos list = []
        let mutable queue = Map.ofList [(0, [(0,0)])]

        let map = let (r,c) = Day18.Bounds in Array2D.init r c (fun r c ->
            graph <- graph.Add ((r,c), {MinDist = Int32.MaxValue})
            unvisited <- (r,c)::unvisited
            '.')

        graph <- graph.Add ((0,0), {MinDist = 0})
                
        let (_, o) = fails.[1025..] |> Array.fold (fun (i, (final : int option)) (r,c) ->
            if final.IsSome then (i, final)
            else 
                map.[r,c] <- '#'
                let res = Day18.path unvisited map queue graph

                if res.[Day18.EPos].MinDist = Int32.MaxValue then (i, Some i) else (i+1, final)) (0, None)

        printfn "%A" o
        if o.IsNone then "no problem" else fails.[o.Value] |> string
