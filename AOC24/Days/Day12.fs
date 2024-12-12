namespace AdventOfCode24

type D12Pos = int*int
type D12Map = (char*bool)[,]

type Day12() =
    // finds a region from current 'root' plot
    static member private findRegion (s : char) ((r,c) : D12Pos) ((rL,cL) : D12Pos) (map : D12Map) : D12Pos list =
        if r < 0 || c < 0 || r >= rL || c >= cL then []
        else match map.[r,c] with
             | (s',m) when s' <> s || m-> []
             | (s',_) -> map.[r,c] <- (s',true)
                         (r,c) :: ([(1,0); (-1,0); (0,1); (0,-1)] |> List.fold (fun acc (i,j) ->
                                    acc @ (Day12.findRegion s (r+i, c+j) (rL, cL) map)) [])

    // computes area*fences for region
    static member private handleRegion (fencer : D12Map -> D12Pos list -> int) ((r,c) : D12Pos) ((rL,cL) : D12Pos) (map : D12Map) : int =
        let plots = Day12.findRegion (fst map.[r,c]) (r,c) (rL,cL) map
        (plots.Length) * (fencer map plots)

    // walks across the map, computing each new region found
    static member private walker (totalPrice : int) (fencer : D12Map -> D12Pos list -> int) ((r,c) : D12Pos) ((rL,cL) : D12Pos) (map : D12Map) : int =
        if c >= cL then Day12.walker totalPrice fencer (r+1, 0) (rL, cL) map
        elif r >= rL then totalPrice
        else match map.[r,c] with
             | (_,m) when m -> Day12.walker totalPrice fencer (r, c+1) (rL, cL) map
             | _ -> Day12.walker (totalPrice + (Day12.handleRegion fencer (r,c) (rL,cL) map)) fencer (r, c+1) (rL, cL) map

    // perimeter fences should be placed at each 'empty' surrounding plot of region
    static member Star1 (input : string[]) : string =
        let findPerimeters (map : D12Map) : D12Pos list -> int =
            let perimeters (s : char) ((r,c) : D12Pos) (map : D12Map) : int =
                [(1,0); (-1,0); (0,1); (0,-1)]
                |> List.fold (fun acc (i,j) ->
                    acc + try if (fst map.[r+i, c+j] <> s) then 1 else 0 with _ -> 1) 0

            List.fold (fun tmp (rP, cP) -> tmp + (perimeters (fst map.[rP,cP]) (rP,cP) map)) 0
                    
        let map = input |> array2D |> Array2D.map (fun s -> (s, false)) in
        map |> (string << Day12.walker 0 findPerimeters (0,0) (Array2D.length1 map, Array2D.length2 map))

    // side fences are equal to corners of the region
    static member Star2 (input : string[]) : string =
        let findSides (map : D12Map) : D12Pos list -> int =
            let sPos (s : char) ((r,c) : D12Pos) : bool =
                try (fst map[r,c]) = s with _ -> false

            let iPos (s : char) ((r,c) : D12Pos) : int =
                if sPos s (r,c) then 1 else 0

            let sides (s : char) ((r,c) : D12Pos) (map : D12Map) : int =
                let mask = [(-1,0); (1,0); (0,-1); (0,1)] |> List.map (fun (i,j) -> sPos s (r+i, c+j))
                
                match mask with
                // + shape
                | [true; true; true; true] ->
                    [(-1,-1); (-1,1); (1,-1); (1,1)] |> List.fold (fun acc (i,j) -> acc - iPos s (r+i, c+j)) 4
                // T shape
                | [true; true; true; false] -> [(-1,-1); (1,-1)] |> List.fold (fun acc (i,j) -> acc - iPos s (r+i, c+j)) 2
                | [true; true; false; true] -> [(-1,1); (1,1)] |> List.fold (fun acc (i,j) -> acc - iPos s (r+i, c+j)) 2
                | [true; false; true; true] -> [(-1,-1); (-1,1)] |> List.fold (fun acc (i,j) -> acc - iPos s (r+i, c+j)) 2
                | [false; true; true; true] -> [(1,-1); (1,1)] |> List.fold (fun acc (i,j) -> acc - iPos s (r+i, c+j)) 2
                // L shape
                | [true; false; true; false] -> 2 - (iPos s (r-1,c-1))
                | [true; false; false; true] -> 2 - (iPos s (r-1,c+1))
                | [false; true; true; false] -> 2 - (iPos s (r+1,c-1))
                | [false; true; false; true] -> 2 - (iPos s (r+1,c+1))
                // connecting plot
                | [true; true; false; false]
                | [false; false; true; true] -> 0
                // end of - shape
                | [true; false; false; false] -> 2
                | [false; true; false; false] -> 2
                | [false; false; true; false] -> 2
                | [false; false; false; true] -> 2
                // 0 surrounding plots
                | [false; false; false; false] -> 4
                // Misc
                | _ -> 0

            List.fold (fun tmp (rP, cP) -> tmp + (sides (fst map.[rP,cP]) (rP,cP) map)) 0
                    
        let map = input |> array2D |> Array2D.map (fun s -> (s, false)) in
        map |> (string << Day12.walker 0 findSides (0,0) (Array2D.length1 map, Array2D.length2 map))
