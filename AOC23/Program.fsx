﻿open AdventOfCode

open System

[<EntryPoint>]
let main (args : string[]) : int =
    try
        let mutable day = 0

        if (args.Length <> 1 || not (Int32.TryParse(args[0], &day))) then
            raise (new ArgumentException("Program must take exactly one argument of the day to run."))

        if day = 1 then
            Day1.Star1 (InputLoader.GetInputFromFile "inputs_test/day01_1.txt")
            //Day1.Star1 (InputLoader.GetInputFromFile "inputs_real/day01.txt")
            |> printfn "Day 1, Star 1: %s"
        
            Day1.Star2 (InputLoader.GetInputFromFile "inputs_test/day01_2.txt")
            //Day1.Star2 (InputLoader.GetInputFromFile "inputs_real/day01.txt")
            |> printfn "Day 1, Star 2: %s"

        elif day = 2 then
            Day2.Star1 (InputLoader.GetInputFromFile "inputs_test/day02_1.txt")
            //Day2.Star1 (InputLoader.GetInputFromFile "inputs_real/day02.txt")
            |> printfn "Day 2, Star 1: %s"
        
            Day2.Star2 (InputLoader.GetInputFromFile "inputs_test/day02_2.txt")
            //Day2.Star2 (InputLoader.GetInputFromFile "inputs_real/day02.txt")
            |> printfn "Day 2, Star 2: %s"

        elif day = 3 then
            Day3.Star1 (InputLoader.GetInputFromFile "inputs_test/day03_1.txt")
            //Day3.Star1 (InputLoader.GetInputFromFile "inputs_real/day03.txt")
            |> printfn "Day 3, Star 1: %s"
        
            Day3.Star2 (InputLoader.GetInputFromFile "inputs_test/day03_2.txt")
            //Day3.Star2 (InputLoader.GetInputFromFile "inputs_real/day03.txt")
            |> printfn "Day 3, Star 2: %s"

        elif day = 4 then
            Day4.Star1 (InputLoader.GetInputFromFile "inputs_test/day04_1.txt")
            //Day4.Star1 (InputLoader.GetInputFromFile "inputs_real/day04.txt")
            |> printfn "Day 4, Star 1: %s"
        
            Day4.Star2 (InputLoader.GetInputFromFile "inputs_test/day04_2.txt")
            //Day4.Star2 (InputLoader.GetInputFromFile "inputs_real/day04.txt")
            |> printfn "Day 4, Star 2: %s"

        elif day = 5 then
            Day5.Star1 (InputLoader.GetInputFromFile "inputs_test/day05_1.txt")
            //Day5.Star1 (InputLoader.GetInputFromFile "inputs_real/day05.txt")
            |> printfn "Day 5, Star 1: %s"
        
            Day5.Star2 (InputLoader.GetInputFromFile "inputs_test/day05_2.txt")
            //Day5.Star2 (InputLoader.GetInputFromFile "inputs_real/day05.txt")
            |> printfn "Day 5, Star 2: %s"

        elif day = 6 then
            Day6.Star1 (InputLoader.GetInputFromFile "inputs_test/day06_1.txt")
            //Day6.Star1 (InputLoader.GetInputFromFile "inputs_real/day06.txt")
            |> printfn "Day 6, Star 1: %s"
        
            Day6.Star2 (InputLoader.GetInputFromFile "inputs_test/day06_2.txt")
            //Day6.Star2 (InputLoader.GetInputFromFile "inputs_real/day06.txt")
            |> printfn "Day 6, Star 2: %s"

        elif day = 7 then
            Day7.Star1 (InputLoader.GetInputFromFile "inputs_test/day07_1.txt")
            //Day7.Star1 (InputLoader.GetInputFromFile "inputs_real/day07.txt")
            |> printfn "Day 7, Star 1: %s"
        
            Day7.Star2 (InputLoader.GetInputFromFile "inputs_test/day07_2.txt")
            //Day7.Star2 (InputLoader.GetInputFromFile "inputs_real/day07.txt")
            |> printfn "Day 7, Star 2: %s"

        elif day = 8 then
            Day8.Star1 (InputLoader.GetInputFromFile "inputs_test/day08_1.txt")
            //Day8.Star1 (InputLoader.GetInputFromFile "inputs_real/day08.txt")
            |> printfn "Day 8, Star 1: %s"
        
            Day8.Star2 (InputLoader.GetInputFromFile "inputs_test/day08_2.txt")
            //Day8.Star2 (InputLoader.GetInputFromFile "inputs_real/day08.txt")
            |> printfn "Day 8, Star 2: %s"

        elif day = 9 then
            Day9.Star1 (InputLoader.GetInputFromFile "inputs_test/day09_1.txt")
            //Day9.Star1 (InputLoader.GetInputFromFile "inputs_real/day09.txt")
            |> printfn "Day 9, Star 1: %s"
        
            Day9.Star2 (InputLoader.GetInputFromFile "inputs_test/day09_2.txt")
            //Day9.Star2 (InputLoader.GetInputFromFile "inputs_real/day09.txt")
            |> printfn "Day 9, Star 2: %s"

        elif day = 10 then
            //Day10.Star1 (InputLoader.GetInputFromFile "inputs_test/day10_1.txt")
            Day10.Star1 (InputLoader.GetInputFromFile "inputs_real/day10.txt")
            |> printfn "Day 10, Star 1: %s"
        
            Day10.Star2 (InputLoader.GetInputFromFile "inputs_test/day10_2.txt")
            //Day10.Star2 (InputLoader.GetInputFromFile "inputs_real/day10.txt")
            |> printfn "Day 10, Star 2: %s"

        else
            printfn "Day not solved yet."
        
        0
    with
    | FileException m -> eprintfn "%s" m; 1
    | :? ArgumentException as exn -> eprintf $"Argument Invalid: {exn.Message}"; 1
    | :? NotImplementedException as exn -> eprintf $"Not Implemented: {exn.Message}"; 1
    | :? Exception as exn -> eprintf $"Unknown Error: {exn.Message}"; 1
    | _ -> eprintf "Unknown Exception."; 1
