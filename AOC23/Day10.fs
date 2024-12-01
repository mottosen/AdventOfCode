namespace AdventOfCode

open System

type Day10() =
    static member Star1 (input : string[]) : string =
        let rec findStart ((r,c) : int*int) (arr2d : char[,]) (pos : int*int) : int*int =
            if c = arr2d.GetLength(1) then findStart (r+1, 0) arr2d pos
            elif arr2d[r,c] = 'S' then (r,c)
            else findStart (r, c+1) arr2d pos

        let getNext (prev : int*int) ((r,c) : int*int) (arr2d : char[,]) : int*int =
            match arr2d[r,c] with
            | 'S' ->
                [
                    ((r-1,c), ['|'; '7'; 'F'])
                    ((r+1,c), ['|'; 'L'; 'J'])
                    ((r,c-1), ['-'; 'L'; 'F'])
                    ((r,c+1), ['-'; '7'; 'J'])
                ]
            | '|' ->
                [
                    ((r-1,c), ['|'; '7'; 'F'; 'S'])
                    ((r+1,c), ['|'; 'L'; 'J'; 'S'])
                ]
            | '-' ->
                [
                    ((r,c-1), ['-'; 'L'; 'F'; 'S'])
                    ((r,c+1), ['-'; '7'; 'J'; 'S'])
                ]
            | '7' ->
                [
                    ((r+1,c), ['|'; 'L'; 'J'; 'S'])
                    ((r,c-1), ['-'; 'L'; 'F'; 'S'])
                ]
            | 'F' ->
                [
                    ((r+1,c), ['|'; 'L'; 'J'; 'S'])
                    ((r,c+1), ['-'; '7'; 'J'; 'S'])
                ]
            | 'L' ->
                [
                    ((r-1,c), ['|'; '7'; 'F'; 'S'])
                    ((r,c+1), ['-'; '7'; 'J'; 'S'])
                ]
            | 'J' ->
                [
                    ((r-1,c), ['|'; '7'; 'F'; 'S'])
                    ((r,c-1), ['-'; 'L'; 'F'; 'S'])
                ]
            |> List.filter (fun ((i,j),_) -> -1<i && i<arr2d.GetLength(0) && -1<j && j<arr2d.GetLength(1))
            |> List.map (fun ((i,j), lst) -> if (List.contains arr2d[i, j] lst) then (i,j) else (-1,-1))
            |> List.except [(-1,-1); prev] |> List.head

        let rec loopLength (prev : int*int) (arr2d : char[,]) (res : (int*int) list) ((r,c) : int*int) : (int*int) list =
            if arr2d[r,c] = 'S' then (r,c) :: res
            else
                getNext prev (r,c) arr2d |> loopLength (r,c) arr2d ((r,c) :: res)

        let full_loop (prev : int*int) (arr2d : char[,]) ((r,c) : int*int) : (int*int) list =
            let mutable foo = prev
            let mutable bar = (r,c)
            let mutable baz = []

            let mutable tmp = true
            while tmp do
                if arr2d[r,c] = 'S' then
                    baz <- (r,c) :: baz
                    tmp <- false
                else
                    prev <- (r,c)
                    (r,c) <- getNext prev (r,c) arr2d
                    
                    loopLength (r,c) arr2d ((r,c) :: res)

            baz

        let parsed_input = input |> Array.map (fun line -> line |> Seq.toArray)
        let parsed_map = Array2D.init input.Length parsed_input[0].Length (fun r c -> parsed_input[r][c])

        let start_pos = findStart (0,0) parsed_map (-1,-1)
        
        let loopLength = getNext (-1,-1) start_pos parsed_map
                         |> loopLength start_pos parsed_map []

        float loopLength.Length / 2.0 |> floor |> int |> string

    static member Star2 (input : string[]) : string =
        raise (new NotImplementedException("Day10.Star2"))
