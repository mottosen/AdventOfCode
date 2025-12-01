namespace AdventOfCode24

open System.Text.RegularExpressions

type Day7() =
    // check validity of line, accumulating valid test results
    static member private handleLine (helper : float -> float -> string list -> float) (count : float) (line : string) : float =
        let nums = Regex.Matches(line, "\d+") |> Seq.map (fun m -> m.Value) |> Seq.toList
        count + (helper (nums.Head |> float) 0. (nums.Tail))

    static member Star1 : string[] -> string =
        // tries different possible combinations, picking one (of potentially more) correct
        let rec helper (res : float) (acc : float) : string list -> float = function
            | n::ns -> (helper res (acc + (n |> float)) ns) |> max <| (helper res ((max acc 1.) * (n |> float)) ns)
            | [] -> if res = acc then res else 0.

        string << Array.fold (fun acc line -> Day7.handleLine helper acc line) 0.

    static member Star2 : string[] -> string =
        // extended Star1 logic with concat operator
        let rec helper (res : float) (acc : float) : string list -> float = function
            | n::ns -> (helper res (acc + (n |> float)) ns) |> max <| (helper res ((max acc 1.) * (n |> float)) ns) |> max <| (helper res (((string acc) + n) |> float) ns)
            | [] -> if res = acc then res else 0.

        string << Array.fold (fun acc line -> Day7.handleLine helper acc line) 0.
