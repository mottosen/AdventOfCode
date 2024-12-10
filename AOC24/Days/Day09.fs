namespace AdventOfCode24

open System
open System.Text.RegularExpressions

type Day9() =
    static member private line2SpaceArray (line : string) : char[] =
        let rec writeChars (idx : int) (count : int) (c : char) (arr : char[]) : int =
            if count <= 0 then idx
            else arr.[idx] <- c; writeChars (idx+1) (count-1) c arr

        let nums = Regex.Matches(line, "\d") |> Seq.map (fun m -> m.Value |> Int32.Parse)
        let res = Array.init (nums |> Seq.sum) (fun _ -> '.')

        let mutable idx = 0
        let mutable id = 0
        nums |> Seq.iteri (fun i n ->
            if (i%2 = 0) then
                idx <- writeChars idx n ((char)(48+id)) res
                id <- id + 1
            else
                idx <- writeChars idx n ((char)(46)) res)

        res

    static member private getChecksum : char[] -> int64 =
        snd << ((0L,0L) |> Array.fold (fun (idx, checksum) c ->
            if c = '.' then (idx+1L, checksum)
            else (idx+1L, checksum + idx*((int64)c - 48L))))
    
    static member Star1 (input : string[]) : string =
        // parse input to spaced char array
        let res = Day9.line2SpaceArray input.[0]

        // moved blocks around
        let rec swap (lI : int) (rI : int) (arr : char[]) : unit =
            if rI <= lI then ()
            else
                if arr.[lI] <> '.' then swap (lI+1) rI arr
                elif arr.[rI] = '.' then swap lI (rI-1) arr
                else
                    arr.[lI] <- arr.[rI]; arr.[rI] <- '.'
                    swap (lI+1) (rI-1) arr

        swap 0 (res.Length - 1) res

        // calculate checksum
        Day9.getChecksum res |> string

    static member Star2 (input : string[]) : string =
        // parse input to spaced char array
        let res = Day9.line2SpaceArray input.[0]

        // moved files around
        let rec findFile (lI : int) (rI : int) (c : char) (arr : char[]) : int =
            if lI < 0 || arr.[lI] <> c then lI+1
            else findFile (lI-1) rI c arr

        let moveFile (lI : int) (size : int) (arr : char[]) : int option =
            let rec findSpace (sI : int) : int option =
                let rec helper (tI : int) (rem : int) : int option =
                    if rem = 0 then None
                    elif arr.[tI] <> '.' then Some tI
                    else helper (tI+1) (rem-1)

                if lI <= sI then None
                elif arr.[sI] <> '.' then findSpace (sI+1)
                else
                    match helper sI size with
                    | None -> Some sI
                    | Some nI -> findSpace (nI+1)

            let rec mover (src : int) (tgt : int) (rem : int) : int =
                if rem = 0 then src
                else arr.[tgt] <- arr.[src]; arr.[src] <- '.'; mover (src+1) (tgt+1) (rem-1)

            let space = findSpace 0
            match space with
            | None -> None
            | Some mI ->
                mover lI mI size |> Some

        let rec swap (rI : int) (arr : char[]) : unit =
            if rI < 0 then ()
            elif arr.[rI] = '.' then swap (rI-1) arr
            else
                let lF = findFile rI rI (arr.[rI]) arr
                match moveFile lF (rI-lF+1) arr with
                | None -> swap (lF-1) arr
                | Some lS -> if lS > 0 then swap (lS-1) arr

        swap (res.Length - 1) res

        // calculate checksum
        Day9.getChecksum res |> string
