namespace AdventOfCode24

type Day4() =
    static member private bool2int (b : bool) : int = if b then 1 else 0

    // applies a folder to a 2d char array, for specified chars
    static member private foldMap (f : int -> int*int -> char[,] -> int) (chars : char list) (map : char[,]) : int =
        let rec helper acc (rL,cL) : int*int -> int = function
            | r,_ when r >= rL -> acc
            | r,c when c >= cL -> helper acc (rL, cL) (r+1, 0)
            | r,c when (List.contains map.[r, c] chars) -> (helper (f acc (r, c) map) (rL, cL) (r, c+1))
            | r,c -> (helper acc (rL, cL) (r, c+1))

        helper 0 (Array2D.length1 map, Array2D.length2 map) (0, 0)

    static member Star1 : string[] -> string =
        // checks if entry is start/end of "XMAS"
        let checkFind (acc : int) (r, c) (map : char[,]) : int =
            acc +
            if map.[r, c] = 'X' then // "XMAS"
                  try (map.[r+1, c-1] = 'M' && map.[r+2, c-2] = 'A' && map.[r+3, c-3] = 'S') |> Day4.bool2int with _ -> 0
                + try (map.[r+1, c]   = 'M' && map.[r+2, c]   = 'A' && map.[r+3, c]   = 'S') |> Day4.bool2int with _ -> 0
                + try (map.[r+1, c+1] = 'M' && map.[r+2, c+2] = 'A' && map.[r+3, c+3] = 'S') |> Day4.bool2int with _ -> 0
                + try (map.[r, c+1]   = 'M' && map.[r, c+2]   = 'A' && map.[r, c+3]   = 'S') |> Day4.bool2int with _ -> 0
            else // "SAMX"
                  try (map.[r+1, c-1] = 'A' && map.[r+2, c-2] = 'M' && map.[r+3, c-3] = 'X') |> Day4.bool2int with _ -> 0
                + try (map.[r+1, c]   = 'A' && map.[r+2, c]   = 'M' && map.[r+3, c]   = 'X') |> Day4.bool2int with _ -> 0
                + try (map.[r+1, c+1] = 'A' && map.[r+2, c+2] = 'M' && map.[r+3, c+3] = 'X') |> Day4.bool2int with _ -> 0
                + try (map.[r, c+1]   = 'A' && map.[r, c+2]   = 'M' && map.[r, c+3]   = 'X') |> Day4.bool2int with _ -> 0

        string << Day4.foldMap checkFind ['X';'S'] << array2D

    static member Star2 : string[] -> string =
        // checks if entry is middle of X "MAS"
        let checkFind (acc : int) (r, c) (map : char[,]) : int =
            ((( try (map.[r-1, c-1] = 'M' && map.[r+1, c+1] = 'S') with _ -> false
            ||  try (map.[r+1, c+1] = 'M' && map.[r-1, c-1] = 'S') with _ -> false)
            &&
            (   try (map.[r-1, c+1] = 'M' && map.[r+1, c-1] = 'S') with _ -> false
            ||  try (map.[r+1, c-1] = 'M' && map.[r-1, c+1] = 'S') with _ -> false))
            |> Day4.bool2int) + acc

        string << Day4.foldMap checkFind ['A'] << array2D
