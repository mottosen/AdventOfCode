namespace AdventOfCode24

module Program =
    open System
    
    [<EntryPoint>]
    let main (args : string[]) : int =
        try
            let mutable day = 0
            let timer = System.Diagnostics.Stopwatch()

            if (args.Length <> 1 || not (Int32.TryParse(args[0], &day))) then
                raise (new ArgumentException("Program must take exactly one argument of the day to run."))

            if day = 1 then
                let input = InputLoader.GetInputFromFile "inputs_test/day01_1.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day01_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day01.txt"

                timer.Start()
                input |> Day1.Star1 |> printfn "Day 1, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day1.Star2 |> printfn "Day 1, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 2 then
                let input = InputLoader.GetInputFromFile "inputs_test/day02_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day02_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day02.txt"

                timer.Start()
                input |> Day2.Star1 |> printfn "Day 2, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day2.Star2 |> printfn "Day 2, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 3 then
                let input = InputLoader.GetInputFromFile "inputs_test/day03_3.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day03_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day03.txt"

                timer.Start()
                input |> Day3.Star1 |> printfn "Day 3, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day3.Star2 |> printfn "Day 3, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 4 then
                let input = InputLoader.GetInputFromFile "inputs_test/day04_4.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day04_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day04.txt"

                timer.Start()
                input |> Day4.Star1 |> printfn "Day 4, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day4.Star2 |> printfn "Day 4, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 5 then
                let input = InputLoader.GetInputFromFile "inputs_test/day05_5.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day05_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day05.txt"

                timer.Start()
                input |> Day5.Star1 |> printfn "Day 5, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day5.Star2 |> printfn "Day 5, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 6 then
                let input = InputLoader.GetInputFromFile "inputs_test/day06_6.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day06_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day06.txt"

                timer.Start()
                input |> Day6.Star1 |> printfn "Day 6, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day6.Star2 |> printfn "Day 6, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 7 then
                let input = InputLoader.GetInputFromFile "inputs_test/day07_7.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day07_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day07.txt"

                timer.Start()
                input |> Day7.Star1 |> printfn "Day 7, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day7.Star2 |> printfn "Day 7, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 8 then
                let input = InputLoader.GetInputFromFile "inputs_test/day08_8.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day08_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day08.txt"

                timer.Start()
                input |> Day8.Star1 |> printfn "Day 8, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day8.Star2 |> printfn "Day 8, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 9 then
                let input = InputLoader.GetInputFromFile "inputs_test/day09_9.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day09_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day09.txt"

                timer.Start()
                input |> Day9.Star1 |> printfn "Day 9, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day9.Star2 |> printfn "Day 9, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 10 then
                let input = InputLoader.GetInputFromFile "inputs_test/day10_1.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day10_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day10.txt"

                timer.Start()
                input |> Day10.Star1 |> printfn "Day 10, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day10.Star2 |> printfn "Day 10, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 11 then
                let input = InputLoader.GetInputFromFile "inputs_test/day11_1.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day11_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day11.txt"

                timer.Start()
                input |> Day11.Star1 |> printfn "Day 11, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day11.Star2 |> printfn "Day 11, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 12 then
                let input = InputLoader.GetInputFromFile "inputs_test/day12_1.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day12_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day12.txt"

                timer.Start()
                input |> Day12.Star1 |> printfn "Day 12, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day12.Star2 |> printfn "Day 12, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 13 then
                let input = InputLoader.GetInputFromFile "inputs_test/day13_1.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day13_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day13.txt"

                timer.Start()
                input |> Day13.Star1 |> printfn "Day 13, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day13.Star2 |> printfn "Day 13, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 14 then
                let input = InputLoader.GetInputFromFile "inputs_test/day14_1.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day14_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day14.txt"

                timer.Start()
                input |> Day14.Star1 |> printfn "Day 14, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day14.Star2 |> printfn "Day 14, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            elif day = 15 then
                let input = InputLoader.GetInputFromFile "inputs_test/day15_1.txt"
                //let input = InputLoader.GetInputFromFile "inputs_test/day15_2.txt"
                //let input = InputLoader.GetInputFromFile "inputs_real/day15.txt"

                timer.Start()
                input |> Day15.Star1 |> printfn "Day 15, Star 1: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)
                
                timer.Restart()
                input |> Day15.Star2 |> printfn "Day 15, Star 2: %s"
                printfn "\ttime: %i ms" (timer.ElapsedMilliseconds)

            else
                printfn "Day not solved yet."
        
            timer.Stop()
            0
        with
        | FileException m -> eprintfn "%s" m; 1
        | :? ArgumentException as exn -> eprintf $"Argument Invalid: {exn.Message}"; 1
        | :? NotImplementedException as exn -> eprintf $"Not Implemented: {exn.Message}"; 1
        | :? Exception as exn -> eprintf $"Unknown Error: {exn.Message}"; 1
        | _ -> eprintf "Unknown Exception."; 1
