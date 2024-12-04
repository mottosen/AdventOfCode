namespace AdventOfCode24

open System
open System.Text.RegularExpressions

type Day3() =
    static member private reg : Regex = new Regex("(mul\((\d{1,3}),(\d{1,3})\))|(do\(\))|(don't\(\))")

    // multiply numbers, contained in regex match, add to acc
    static member private mulAcc (acc : int) : Match -> int = function
        | m when not m.Groups.[1].Success -> acc
        | m -> acc + ((m.Groups.[2].Value |> Int32.Parse) * (m.Groups.[3].Value |> Int32.Parse))
    
    // extract multiplications with regex, sum across lines in input
    static member Star1 : string[] -> string =
        string << Array.fold (+) 0 << Array.Parallel.map
            (Seq.fold Day3.mulAcc 0 << Day3.reg.Matches)

    // extend Star1 with 'enable switch', state carries over between lines in input
    static member Star2 : string[] -> string =
        let matchHandler (res : int, enabled : bool) : Match -> int*bool = function
            | m when m.Groups.[4].Success -> (res, true)                    // do
            | m when m.Groups.[5].Success -> (res, false)                   // don't
            | m -> ((if enabled then Day3.mulAcc res m else res), enabled)  // mul

        string << fst << Array.fold (fun s ->
            Seq.fold matchHandler s << Day3.reg.Matches) (0, true)
