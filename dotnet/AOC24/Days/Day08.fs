namespace AdventOfCode24

open System.Text.RegularExpressions

type D8Pos = int*int

type Day8() =
    static member inBounds ((rL,cL) : D8Pos) ((r,c) : D8Pos) : bool =
        r >= 0 && c >= 0 && r < rL && c < cL

    // add (key,value) to map, checking if key already exists
    static member mapAdd ((k,v) : char*(D8Pos list)) (map : Map<char, D8Pos list>) : Map<char, D8Pos list> =
        if map.ContainsKey k then map.Add (k, v @ map.[k])
        else map.Add (k,v)

    // merge two maps, merging duplicate keys
    static member mapMerge (m1 : Map<char,D8Pos list>) (m2 : Map<char,D8Pos list>) : Map<char,D8Pos list> =
        Map.fold (fun acc k v -> Day8.mapAdd (k,v) acc) m1 m2

    // checks new antinodes for two points in space, either stopped by out of bounds or scaling
    static member antinodes (scale : int) ((rL,cL) : D8Pos) ((r1,c1) : D8Pos) ((r2,c2) : D8Pos) : D8Pos list =
        let rec helper (factor : int) ((rI,cI) : bool*bool) ((rD,cD) : D8Pos) (r,c) : D8Pos list =
            let np = ((if rI then r+rD else r-rD), (if cI then c+cD else c-cD))

            if factor = 0 || not (Day8.inBounds (rL,cL) np) then []
            else np :: (helper (factor-1) (rI,cI) (rD,cD) np)

        let (rD,cD) = (abs (r1-r2), abs (c1-c2))

        if (r1 < r2) then (helper scale (true, c1 < c2) (rD,cD) (r1,c1)) @ (helper scale (false, c1 >= c2) (rD,cD) (r2,c2))
        else (helper scale (false, c1 < c2) (rD,cD) (r2,c2)) @ (helper scale (true, c1 >= c2) (rD,cD) (r1,c1))

    // finds combinations of antinodes within a list of positions, all with same frequency
    static member checkAntinodes ((rL,cL) : D8Pos) (scale : int) (antinodes : D8Pos list) : D8Pos list -> D8Pos list = function
        | p1::ps -> Day8.checkAntinodes (rL,cL) scale (ps |> List.fold (fun acc p2 -> (Day8.antinodes scale (rL,cL) p1 p2) @ acc) antinodes) ps
        | _ -> antinodes

    // create map of positions with same frequency and find antinodes, scaling at most 1
    static member Star1 (input : string[]) : string =
        (string << List.length << List.distinct
        << Map.fold (fun acc _ positions -> Day8.checkAntinodes (input.Length, input.[0].Length) 1 acc positions) []
        << Array.fold Day8.mapMerge Map.empty<char,D8Pos list>
        << Array.Parallel.mapi (fun r line ->
            Regex.Matches(line, "[a-zA-Z0-9]") |> Seq.fold (fun map m ->
                Day8.mapAdd (m.Value.[0], [(r, m.Index)]) map) Map.empty<char,D8Pos list>)) input

    // same as Star1, but no scaling and including positions as antinodes
    static member Star2 (input : string[]) : string =
        (string << List.length << List.distinct
        << Map.fold (fun acc _ positions -> positions @ Day8.checkAntinodes (input.Length, input.[0].Length) -1 acc positions) []
        << Array.fold Day8.mapMerge Map.empty<char,D8Pos list>
        << Array.Parallel.mapi (fun r line ->
            Regex.Matches(line, "[a-zA-Z0-9]") |> Seq.fold (fun map m ->
                Day8.mapAdd (m.Value.[0], [(r, m.Index)]) map) Map.empty<char,D8Pos list>)) input
