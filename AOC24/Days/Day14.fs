namespace AdventOfCode24

open System
open System.Text.RegularExpressions

type Day14() =
    static let _gridSize = (101,103)

    static member private xLim = (fst _gridSize)
    static member private xMid = Day14.xLim / 2

    static member private yLim = (snd _gridSize)
    static member private yMid = Day14.yLim / 2

    static member private cycle (c : int) (l : int) : int =
        if 0 <= c && c < l then c
        elif l <= c then c-l
        else l+c

    // move each robot n times, then score quadrants
    static member Star1 : string[] -> string =
        string << fun (q1,q2,q3,q4) -> q1*q2*q3*q4
        << Array.fold (fun (q1,q2,q3,q4) (px,py) ->
            if px = Day14.xMid || py = Day14.yMid then (q1,q2,q3,q4)
            elif px < Day14.xMid then
                if py < Day14.yMid then (q1,q2+1L,q3,q4)
                else (q1,q2,q3+1L,q4)
            else
                if py < Day14.yMid then (q1+1L,q2,q3,q4)
                else (q1,q2,q3,q4+1L)
            ) (0L,0L,0L,0L)
        << Array.Parallel.map (fun line ->
            let nums = Regex.Matches(line, "(-?\d+)") |> Seq.map (fun m -> m.Value |> Int32.Parse) |> Seq.toArray

            [1..100] |> List.fold (fun (x,y) _ ->
                (Day14.cycle (x + nums.[2]) Day14.xLim, Day14.cycle (y + nums.[3]) Day14.yLim))
                (nums.[0], nums.[1]))

    // iteratively move one robot at a time
    // looking for smallest security score, as that indicates robots close to each other (in same quadrant)
    static member Star2 : string[] -> string =
        let rec treeIteration (sec : int) (count : int) (max : float) (res : int) (lines : int[][]) : int =
            if sec = count then res
            else
                lines |> Array.iter (fun nums ->
                    nums.[0] <- Day14.cycle (nums.[0] + nums.[2]) Day14.xLim
                    nums.[1] <- Day14.cycle (nums.[1] + nums.[3]) Day14.yLim
                )

                let (q1,q2,q3,q4) = lines |> Array.fold (fun (q1,q2,q3,q4) nums ->
                                        let (px,py) = (nums.[0], nums.[1])
                                        if px = Day14.xMid || py = Day14.yMid then (q1,q2,q3,q4)
                                        elif px < Day14.xMid then
                                            if py < Day14.yMid then (q1,q2+1.,q3,q4)
                                            else (q1,q2,q3+1.,q4)
                                        else
                                            if py < Day14.yMid then (q1+1.,q2,q3,q4)
                                            else (q1,q2,q3,q4+1.)
                                        ) (0.,0.,0.,0.)

                let tmp = q1*q2*q3*q4
                if tmp < max then treeIteration (sec+1) count tmp sec lines
                else treeIteration (sec+1) count max res lines

        string << treeIteration 1 10000 infinity -1
        << Array.map (fun line ->
            Regex.Matches(line, "(-?\d+)") |> Seq.map (fun m -> m.Value |> Int32.Parse) |> Seq.toArray)
