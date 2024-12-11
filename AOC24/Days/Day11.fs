namespace AdventOfCode24

open System
open System.Text.RegularExpressions

type Day11() =
    static let mutable _cache = Map.empty<(int64*int),int64> // key: (num,count), value: res

    static member private cache
        with get() = _cache
        and set(m) = _cache <- m

    // if stone already handled get result, otherwise compute and cache result
    static member private handleNum (count : int) (n : int64) : int64 =
        if count = 0 then 1
        elif Day11.cache.ContainsKey (n,count) then Day11.cache.[(n,count)]
        else
            let str = string n
            let len = str.Length

            let res = 
                // rules from problem
                if n = 0 then Day11.handleNum (count-1) 1L
                elif len % 2 = 0 then
                    (   Day11.handleNum (count-1) (str.[0..(len/2)-1] |> Int64.Parse))
                    + ( Day11.handleNum (count-1) (str.[(len/2)..]    |> Int64.Parse))
                else Day11.handleNum (count-1) (n*2024L)

            Day11.cache <- Day11.cache.Add ((n, count), res)
            res

    static member Star1 (input : string[]) : string =
        Regex.Matches(input.[0], "\d+") |> Seq.map (fun m -> m.Value |> Int64.Parse) |> Seq.toArray
        |> Array.fold (fun acc n -> acc + (Day11.handleNum 25 n)) 0L |> string

    static member Star2 (input : string[]) : string =
        Regex.Matches(input.[0], "\d+") |> Seq.map (fun m -> m.Value |> Int64.Parse) |> Seq.toArray
        |> Array.fold (fun acc n -> acc + (Day11.handleNum 75 n)) 0L |> string
