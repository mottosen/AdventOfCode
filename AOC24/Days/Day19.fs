namespace AdventOfCode24

open System.Text.RegularExpressions

type Day19() =
    static let mutable _cache = Map.empty<string,int64>
    static member private Cache
        with get() = _cache
        and set(c) = _cache <- c

    static member Star1 (input : string[]) : string =
        let m = Regex.Replace (input.[0], ", ", "|")
        let reg = new Regex($"^({m})+$")
        input.[2..] |> Array.fold (fun count line -> if reg.Match(line).Success then count+1 else count) 0
        |> string

    static member Star2 (input : string[]) : string =
        let rec handleLine (rs : Regex list) (line : string) : int64 =
            if Day19.Cache.ContainsKey line then Day19.Cache.[line]
            else
                let ms = rs |> List.map (fun reg -> reg.Split(line)) |> List.filter (fun s -> s.Length > 1)
                if ms.Length = 0 then
                    if line.Length = 0 then 1L else 0L
                else
                    let res = ms |> List.map (fun s -> handleLine rs s.[2]) |> List.fold (+) 0L
                    Day19.Cache <- Day19.Cache.Add (line, res)
                    res

        let rs =
            Regex.Matches(input.[0], "\w+") |> Seq.toList |> List.groupBy (fun m -> m.Length)
            |> List.map (fun (s,ms) -> ms.[1..] |> List.fold (fun acc m -> acc+"|"+m.Value) ms.[0].Value)
            |> List.map (fun s -> new Regex($"^({s})"))

        input.[2..] |> Array.map (handleLine rs) |> Array.fold (+) 0L |> string
