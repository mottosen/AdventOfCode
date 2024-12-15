namespace AdventOfCode24

open System

type D15Pos = int*int
type D15Map = char[,]
type D15Dir = Up | Right | Down | Left

type Day15() =
    static let mutable _mapLimits = (0,0)
    static let mutable _moveLimits = (0,0)

    static member private MapLimits
        with get() = _mapLimits
        and set(l) = _mapLimits <- l

    static member private MoveLimits 
        with get() = _moveLimits
        and set(l) = _moveLimits <- l

    static member private RL_Map = fst _mapLimits
    static member private CL_Map = snd _mapLimits

    static member private RL_Move = fst _moveLimits
    static member private CL_Move = snd _moveLimits

    static member private char2Dir : Char -> D15Dir = function
        | '^' -> Up | '>' -> Right | 'v' -> Down | '<' -> Left

    static member private nextPos ((r,c) : D15Pos) : D15Dir -> D15Pos = function
        | Up -> (r-1, c) | Right -> (r, c+1) | Down -> (r+1, c) | Left -> (r, c-1)

    // find initial position for robot
    static member private findStart ((r,c) : D15Pos) (map : D15Map) : D15Pos =
        if r >= Day15.RL_Map then failwith "should not happen"
        elif c >= Day15.CL_Map then Day15.findStart (r+1, 0) map
        elif map.[r,c] = '@' then (r,c)
        else Day15.findStart (r,c+1) map

    // move an object on the map
    static member private mapDrawer ((rS,cS) : D15Pos) ((rD,cD) : D15Pos) (map : D15Map) : unit =
        map.[rD,cD] <- map.[rS,cS]; map.[rS,cS] <- '.'

    // check if (chain of) move(s) is/are valid
    static member private validMove ((r,c) : D15Pos) (dir : D15Dir) (map : D15Map) : bool =
        if map.[r,c] = '#' then false
        elif map.[r,c] = '.' then true
        elif List.contains dir [Left; Right] || List.contains map.[r,c] ['@'; 'O'] then
            Day15.validMove (Day15.nextPos (r,c) dir) dir map
        else
            let (p1,p2) = if map.[r,c] = '[' then
                              if dir = Up then ((r-1,c), (r-1, c+1))
                              else ((r+1,c), (r+1,c+1))
                          else
                              if dir = Up then ((r-1,c-1), (r-1, c))
                              else ((r+1,c-1), (r+1,c))

            Day15.validMove p1 dir map && Day15.validMove p2 dir map

    // chain move objects, assumed to be all possible moves
    static member private moveChain ((r,c) : D15Pos) (dir : D15Dir) (map : D15Map) : unit =
        if List.contains map.[r,c] ['#'; '.'] then ()
        elif map.[r,c] = '[' && List.contains dir [Up;Down] then
            let (p1,p2) = if dir = Up then ((r-1,c), (r-1, c+1))
                          else ((r+1,c), (r+1,c+1))

            Day15.moveChain p1 dir map; Day15.moveChain p2 dir map
            Day15.mapDrawer (r,c) p1 map; Day15.mapDrawer (r,c+1) p2 map
        elif map.[r,c] = ']' && List.contains dir [Up;Down] then
            let (p1,p2) =
                if dir = Up then ((r-1,c-1), (r-1, c))
                else ((r+1,c-1), (r+1,c))

            Day15.moveChain p1 dir map; Day15.moveChain p2 dir map
            Day15.mapDrawer (r,c-1) p1 map; Day15.mapDrawer (r,c) p2 map
        else
            let (rN,cN) = Day15.nextPos (r,c) dir
            Day15.moveChain (rN,cN) dir map
            Day15.mapDrawer (r,c) (rN,cN) map

    // move object(s) if possible
    static member private handleMove ((r,c) : D15Pos) (dir : D15Dir) (map : D15Map) : bool =
        let valid = Day15.validMove (r,c) dir map 
        if valid then Day15.moveChain (r,c) dir map
        valid

    // handles the moves from the input, parse and (if possible) move
    static member private doMoves ((rR,cR) : D15Pos) ((rM,cM) : D15Pos) (map : D15Map) (moves : D15Map) : unit =
        if rM >= Day15.RL_Move then ()
        elif cM >= Day15.CL_Move then Day15.doMoves (rR, cR) (rM+1, 0) map moves
        else
            let dir = moves.[rM,cM] |> Day15.char2Dir
            Day15.doMoves
                (if Day15.handleMove (rR, cR) dir map then Day15.nextPos (rR, cR) dir else (rR, cR))
                (rM, cM+1) map moves

    // sums the coordinates of boxes in a map
    static member sumCoords (sum : int) (s : Char) ((r,c) : D15Pos) (map : D15Map) : int =
        if r >= Day15.RL_Map then sum
        elif c >= Day15.CL_Map then Day15.sumCoords sum s (r+1, 0) map
        else Day15.sumCoords (if map.[r,c] = s then sum+(100*r + c) else sum) s (r, c+1) map

    static member Star1 (input : string[]) : string =
        let split = Array.findIndex (fun line -> line = "") input

        let map = input.[0..split-1] |> array2D
        let moves = input.[split+1..] |> array2D

        Day15.MapLimits <- (Array2D.length1 map, Array2D.length2 map)
        Day15.MoveLimits <- (Array2D.length1 moves, Array2D.length2 moves)

        Day15.doMoves (map |> Day15.findStart (0,0)) (0,0) map moves
        Day15.sumCoords 0 'O' (0,0) map |> string

    static member Star2 (input : string[]) : string =
        // extending the input map, double size
        let extender (input : string[]) : (Char seq)[] =
            let rec helper : Char list -> Char list = function
            | c::cs ->
                if c = 'O' then '[' :: ']' :: helper cs
                elif c = '@' then c :: '.' :: helper cs
                else c :: c :: helper cs
            | _ -> []
            
            input |> Array.Parallel.map (Seq.ofList << helper << Seq.toList)

        let split = Array.findIndex (fun line -> line = "") input

        let map = input.[0..split-1] |> extender |> array2D
        let moves = input.[split+1..] |> array2D

        Day15.MapLimits <- (Array2D.length1 map, Array2D.length2 map)
        Day15.MoveLimits <- (Array2D.length1 moves, Array2D.length2 moves)

        Day15.doMoves (map |> Day15.findStart (0,0)) (0,0) map moves
        Day15.sumCoords 0 '[' (0,0) map |> string
