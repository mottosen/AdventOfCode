namespace AdventOfCode24

open System

type D6Pos = int*int
type D6Dir = Up | Right | Down | Left
type D6CharMap = char[,]

type Day6() =
    // finds the starting point on map
    static member private findStart (map : D6CharMap) : D6Pos =
        let rec helper ((r,c) : D6Pos) ((rL, cL) : D6Pos) : D6Pos =
            if r < 0 || c < 0 || r >= rL || c >= cL then failwith "no start position"
            else
                if (List.contains map.[r, c] ['^';'>';'v';'<']) then (r,c)
                elif c >= cL-1 then helper (r+1, 0) (rL, cL)
                else helper (r, c+1) (rL, cL)

        helper (0,0) (Array2D.length1 map, Array2D.length2 map)

    static member private char2Dir : char -> D6Dir = function
        | '^' -> Up | '>' -> Right | 'v' -> Down | '<' -> Left

    static member private dir2Char : D6Dir -> char = function
        | Up -> '^' | Right -> '>' | Down -> 'v' | Left -> '<'

    static member private nextDir : D6Dir -> D6Dir = function
        | Up -> Right | Right -> Down | Down -> Left | Left -> Up

    static member private nextPos ((r,c) : D6Pos) : D6Dir -> D6Pos = function
        | Up -> (r-1, c) | Right -> (r, c+1) | Down -> (r+1, c) | Left -> (r, c-1)

    // updates map according to guard walking
    static member private drawMap ((r,c) : D6Pos) (map : D6CharMap) (dir : D6Dir) : unit =
        let m = map.[r, c]
        if m = (Day6.dir2Char dir) then failwith "loop"
        elif not (m = '.') then ()
        else map.[r, c] <- Day6.dir2Char dir

    // simulates walking the guard, until he exits map or goes in a loop
    static member private walkGuard (initPos : D6Pos) ((rL,cL) : D6Pos) (initDir : D6Dir) (map : D6CharMap) : int =
        let mutable res = 0
        let mutable walk = true
        let mutable pos = initPos
        let mutable dir = initDir

        while walk do
            let (r,c) = pos
            try
                Day6.drawMap (r, c) map dir
                match dir with
                | Up -> if r <= 0 then walk <- false else
                            if map.[r-1, c] = '#' then dir <- Day6.nextDir dir
                            else pos <- Day6.nextPos (r,c) dir
                | Right -> if c >= cL-1 then walk <- false else
                            if map.[r, c+1] = '#' then dir <- Day6.nextDir dir
                            else pos <- Day6.nextPos (r,c) dir
                | Down -> if r >= rL-1 then walk <- false else
                            if map.[r+1, c] = '#' then dir <- Day6.nextDir dir
                            else pos <- Day6.nextPos (r,c) dir
                | Left -> if c <= 0 then walk <- false else
                            if map.[r, c-1] = '#' then dir <- Day6.nextDir dir
                            else pos <- Day6.nextPos (r,c) dir
            with _ -> res <- 1 ; walk <- false

        res

    // simulate guard walking, count unique points in path
    static member Star1 (input : string[]) : string =
        let map = input |> array2D
        let (r,c) = map |> Day6.findStart

        let dir = Day6.char2Dir map.[r, c]
        map.[r, c] <- 'X'
        let (rL, cL) = (Array2D.length1 map, Array2D.length2 map)

        Day6.walkGuard (r, c) (rL, cL) dir map |> ignore

        let mutable count = 0

        Array2D.iter (fun c -> if List.contains c ['^';'>';'v';'<';'X'] then count <- count + 1 else ()) map
        
        count |> string

    // simulate guard walking, place new block in each relevant position
    // simulate guard walking, for each new possible block
    static member Star2 (input : string[]) : string =
        let map = input |> array2D
        let (r,c) = map |> Day6.findStart

        let dir = Day6.char2Dir map.[r, c]
        let (rL, cL) = (Array2D.length1 map, Array2D.length2 map)
        
        map.[r, c] <- 'X'

        let tmpMap = Array2D.copy map
        Day6.walkGuard (r, c) (rL, cL) dir tmpMap |> ignore

        let mutable changes = []

        Array2D.iteri (fun i j c -> if List.contains c ['^';'>';'v';'<'] then changes <- (i,j) :: changes else ()) tmpMap

        let mutable res = 0

        changes |> List.iter (fun (i,j) -> 
            let mapCopy = Array2D.copy map
            mapCopy.[i, j] <- '#'
            res <- res + (Day6.walkGuard (r, c) (rL, cL) dir mapCopy))

        res |> string
