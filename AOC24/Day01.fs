namespace AdventOfCode

open System
open System.Text.RegularExpressions

type Day1() =
    static member private split2Columns : string[] -> int[]*int[] =
        // get two nums with regex, return as tuple
        let helper (line : string) : int*int =
            let m = Regex.Match(line, "^(\d+)\s+(\d+)$")
            (m.Groups[1].Value |> Int32.Parse, m.Groups[2].Value |> Int32.Parse)

        // split every line, unzip into column arrays
        Array.unzip << Array.map helper

    static member Star1 (input : string[]) : string =
        // split and sort input
        let (lCol, rCol) = Day1.split2Columns input
        Array.Sort lCol; Array.Sort rCol

        // fold both arrays, acc distance between columns
        (0, lCol, rCol)
        |||> Array.fold2 (fun acc e1 e2 -> (+) acc << abs <| e1 - e2)
        |> string

    static member Star2 (input : string[]) : string =
        // split input, map right column of elm appearance count
        let (lCol, rCol) = Day1.split2Columns input
        let rCol = rCol |> Array.countBy id |> Map.ofArray
        
        // update acc, similarity score across columns
        let helper (acc : int) (elm : int) =
            if not (rCol.ContainsKey elm) then acc
            else acc + (elm * rCol.[elm])

        // fold left column, acc total similarity score
        (0, lCol)
        ||> Array.fold helper
        |> string
