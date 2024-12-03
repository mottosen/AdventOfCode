namespace AdventOfCode

open System
open System.Text.RegularExpressions

type Day2() =
    // validity of difference, only checking range
    static member private validChange (n : int) : bool = 0 < n && n < 4

    // convert line to integer array
    static member private line2Nums (line : string) : int list =
        Regex.Matches(line, "\d+")
        |> Seq.map (fun n -> Int32.Parse n.Value)
        |> Seq.toList

    // signed operation with no tolerance
    static member private safeNoTolerance (incr : bool) : int list -> bool = function
        | e1::e2::rem -> Day2.validChange (if incr then e2-e1 else e1-e2) && Day2.safeNoTolerance incr (e2::rem)
        | _ -> true

    static member Star1 : string[] -> string =
        // get sign of operation, check rest with no tolerance, according to sign
        let safeLineInit : int list -> bool = function
            | e1::e2::rem -> Day2.validChange (abs (e1-e2)) && Day2.safeNoTolerance (e2-e1 > 0) (e2::rem)
            | _ -> failwith "should not happen"
        
        Array.Parallel.map (fun line -> Day2.line2Nums line |> safeLineInit)
        >> Array.fold (fun acc safe -> if safe then acc + 1 else acc) 0
        >> string

    static member Star2 : string[] -> string =
        // signed operation, continues with or without tolerance
        let rec safeLineRem incr : int list -> bool = function
            | e1::e2::e3::rem ->
                let (d1,d2,d3) = ((if incr then e2-e1 else e1-e2), (if incr then e3-e2 else e2-e3), (if incr then e3-e1 else e1-e3))

                match (Day2.validChange d1, Day2.validChange d2, Day2.validChange d3) with
                // all bad
                | (false, false, false) -> false
                // all good
                | (true, true, _) -> safeLineRem incr (e2::e3::rem)
                // remove e1
                | (false, true, false) -> Day2.safeNoTolerance incr (e2::e3::rem)
                // remove e2
                | (false, false, true) -> Day2.safeNoTolerance incr (e1::e3::rem)
                // remove e3
                | (true, false, false) -> Day2.safeNoTolerance incr (e1::e2::rem)
                // remove e1 or e2
                | (false, true, true) -> Day2.safeNoTolerance incr (e2::e3::rem) || Day2.safeNoTolerance incr (e1::e3::rem)
                // remove e2 or e3
                | (true, false, true) -> Day2.safeNoTolerance incr (e1::e3::rem) || Day2.safeNoTolerance incr (e1::e2::rem)
            | e1::e2::[] ->
                Day2.validChange <| if incr then e2-e1 else e1-e2
            | _ -> failwith "should not happen"

        // initial check of validity and sign of operation, continues with or without tolerance
        let safeLineInit : int list -> bool = function
            | e1::e2::e3::rem ->
                let (d1,d2,d3) = (e2-e1, e3-e2, e3-e1)

                match (Day2.validChange (abs d1), Day2.validChange (abs d2), Day2.validChange (abs d3)) with
                // all bad
                | (false, false, false) -> false
                // all in range
                | (true, true, _) ->
                    if (sign d1 = sign d2) then
                        safeLineRem (d1 > 0) (e2::e3::rem)
                    else
                        // remove e1 or e3
                        Day2.safeNoTolerance (d2 > 0) (e2::e3::rem) || Day2.safeNoTolerance (d1 > 0) (e1::e2::rem)
                // remove e1
                | (false, true, false) -> Day2.safeNoTolerance (d2 > 0) (e2::e3::rem)
                // remove e2
                | (false, false, true) -> Day2.safeNoTolerance (d3 > 0) (e1::e3::rem)
                // remove e3
                | (true, false, false) -> Day2.safeNoTolerance (d1 > 0) (e1::e2::rem)
                // remove e1 or e2
                | (false, true, true) -> Day2.safeNoTolerance (d2 > 0) (e2::e3::rem) || Day2.safeNoTolerance (d1 > 0) (e1::e3::rem)
                // remove e2 or e3
                | (true, false, true) -> Day2.safeNoTolerance (d1 > 0) (e1::e3::rem) || Day2.safeNoTolerance (d1 > 0) (e1::e2::rem)
            | _ -> failwith "should not happen"

        Array.Parallel.map (fun line -> Day2.line2Nums line |> safeLineInit)
        >> Array.fold (fun acc safe -> if safe then acc + 1 else acc) 0
        >> string
