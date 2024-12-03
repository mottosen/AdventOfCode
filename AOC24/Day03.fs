namespace AdventOfCode

open System
open System.Text.RegularExpressions

type Day3() =
    // multiply numbers, contained in regex match, add to acc
    static member private mulAcc (acc : int) (m : Match) : int =
        acc + ((m.Groups.[1].Value |> Int32.Parse) * (m.Groups.[2].Value |> Int32.Parse))

    // extract multiplications with regex, sum across lines in input
    static member Star1 : string[] -> string =
        Array.Parallel.map (fun l ->
            Regex.Matches(l, "mul\((\d{1,3}),(\d{1,3})\)") |> Seq.fold Day3.mulAcc 0)
        >> Array.fold (+) 0 >> string

    // extend Star1 with 'enable switch', state carries over between lines in input
    static member Star2 : string[] -> string =
        let lineRes (state : int*bool) (line : string) : int*bool =
            let rec helper (res : int, enabled : bool) : Match list -> int*bool = function
                | m::ms ->
                    // regex allows assumptions here
                    if (m.Groups.[0].Value = "do()") then helper (res, true) ms
                    elif (m.Groups.[0].Value = "don't()") then helper (res, false) ms
                    else
                        if enabled then helper ((Day3.mulAcc res m), enabled) ms
                        else helper (res, enabled) ms
                | _ -> (res, enabled)

            Regex.Matches(line, "mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)")
            |> Seq.toList |> helper state

        Array.fold lineRes (0, true) >> fst >> string
