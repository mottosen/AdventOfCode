namespace AdventOfCode24

type Day10Pos = int*int
type Day10Map = (int*int)[,]

type Day10() =
    static member private inBounds ((r,c) : Day10Pos) ((rL,cL) : Day10Pos) : bool =
        r >= 0 && c >= 0 && r < rL && c < cL

    static member private findTrails ((r,c) : Day10Pos) ((rL,cL) : Day10Pos) (map : Day10Map) : bool -> int = function
        | false -> map |> Day10.possibleTrails -1 (r,c) (rL,cL) |> List.length
        | true -> map |> Day10.possibleTrails -1 (r,c) (rL,cL) |> List.distinct |> List.length

    static member private possibleTrails (level : int) ((r,c) : Day10Pos) ((rL,cL) : Day10Pos) (map : Day10Map) : (int*int) list =
        if not (Day10.inBounds (r,c) (rL,cL)) then []
        else
            let (pS, _) = map.[r,c]
            if not (pS = level+1) then [] // level must rise exactly one per step
            elif pS = 9 then [(r,c)]
            else [(-1, 0); (0, 1); (1, 0); (0, -1)] |> (snd
                << List.mapFold (fun acc (i,j) -> let trails = Day10.possibleTrails pS (r+i, c+j) (rL, cL) map in (0, trails@acc)) [])

    static member private mapCounter (acc : int) ((r,c) : Day10Pos) ((rL,cL) : Day10Pos) (map : int[,]) : int =
        if r = rL then acc
        elif c = cL then Day10.mapCounter acc (r+1, 0) (rL, cL) map
        else Day10.mapCounter (acc + map.[r,c]) (r, c+1) (rL, cL) map

    static member Star1 (input : string[]) : string =
        let map = input |> array2D |> Array2D.map (fun c -> (((int)c) - 48, 0))
        in map |> (string
            << Day10.mapCounter 0 (0,0) (Array2D.length1 map, Array2D.length2 map)
            << Array2D.mapi (fun r c (pS, pC) ->
                if pS = 0 then Day10.findTrails (r, c) (Array2D.length1 map, Array2D.length2 map) map true else 0))

    static member Star2 (input : string[]) : string =
        let map = input |> array2D |> Array2D.map (fun c -> (((int)c) - 48, 0))
        in map |> (string
            << Day10.mapCounter 0 (0,0) (Array2D.length1 map, Array2D.length2 map)
            << Array2D.mapi (fun r c (pS, pC) ->
                if pS = 0 then Day10.findTrails (r, c) (Array2D.length1 map, Array2D.length2 map) map false else 0))
