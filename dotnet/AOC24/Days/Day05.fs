namespace AdventOfCode24

open System
open System.Text.RegularExpressions

type Day5() =
    // creates map where key is a number and value is all numbers that must precede key
    static member private extractOrders (lines : string list) : Map<int, int list> * (string list) =
        let rec helper (map : Map<int, int list>) : string list -> Map<int, int list> * (string list) = function
            | line::lines when line = "" -> (map,lines) // input has empty line between orders and updates
            | line::lines ->
                let m = Regex.Match(line, "(\d+)\|(\d+)")
                let (n1,n2) = (m.Groups.[1].Value |> Int32.Parse, m.Groups.[2].Value |> Int32.Parse)
                lines |> helper (map.Add (n2, n1::(if (map.ContainsKey n2) then map.[n2] else [])))
            | _ -> (map,[])

        helper (Map.empty<int, int list>) lines

    // checks if line is valid, given order constraints
    static member private handleUpdates ((acc, errs, map) : int*(string list)*Map<int, int list>) (line : string)
        : int*(string list)*Map<int, int list> =
            let rec lineValid (s : bool) (ban : int list) : int list -> bool = function
                | n::ns ->
                    if (List.contains n ban) then false
                    elif (map.ContainsKey n) then lineValid s (map.[n] @ ban) ns
                    else lineValid s ban ns
                | [] -> true

            let ms = Regex.Matches(line, "\d+") |> Seq.map (fun m -> m.Value |> Int32.Parse)

            if (lineValid true [] (ms |> Seq.toList)) then (acc + (Seq.item (Seq.length ms / 2) ms), errs, map)
            else (acc, line::errs, map)

    static member Star1 (input : string[]) : string =
        let (orders, updates) = input |> Array.toList |> Day5.extractOrders
        updates |> List.fold Day5.handleUpdates (0, [], orders) |> fun (res,_,_) -> res |> string

    static member Star2 (input : string[]) : string =
        let (orders, updates) = input |> Array.toList |> Day5.extractOrders
        let (_,errs,_) = updates |> List.fold Day5.handleUpdates (0, [], orders)

        // we sort the invalid lines using order constraint map
        (0,errs) ||> List.fold (fun acc line ->
            let sorted =
                Regex.Matches(line, "\d+") |> Seq.map (fun m -> m.Value |> Int32.Parse)
                |> Seq.sortWith (fun n1 n2 ->
                    if (orders.ContainsKey n2) && (List.contains n1 orders.[n2]) then -1 else 1)
            acc + (Seq.item (Seq.length sorted / 2) sorted)
        ) |> string
