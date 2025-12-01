namespace AdventOfCode24

open System.Text.RegularExpressions
open DotNumerics.LinearAlgebra

type Acc = (Matrix option)*float

type Day13() =
    // to reuse code in both stars
    static let mutable _scale = false
    
    // input requires some slack in solving equations
    static member private slack = 0.0005
    static member private linAlg = new LinearEquations()
    static member scale
        with get() = _scale
        and set(s) = _scale <- s

    // find necessary button presses, if possible, for machine
    static member private solveEquation (A : Matrix) (B : Vector) : (float*float) option =
        let vec = Day13.linAlg.Solve(A, B)
        let (y1,y2) = (vec.[0], vec.[1])
        let (r1,r2) = (round y1, round y2)

        if (abs (y1-r1) < Day13.slack && abs (y2-r2) < Day13.slack) then Some (r1, r2) else None

    // monetize button presses with specified tokens
    static member private usedTokens ((a,b) : float*float) : float = a*3. + b*1.

    // parse input to machine data
    static member handleMachines ((eq, tokens) : Acc) (line : string) : Acc =
        if line = "" then (eq,tokens)
        else
            let mB = Regex.Match(line, "^Button.*X.(\d+).*Y.(\d+)$")
            let mP = Regex.Match(line, "^Prize.*X.(\d+).*Y.(\d+)$")
            match eq with
            | None -> // first line of machine input
                let m = new Matrix(2,2)
                m.[0,0] <- mB.Groups.[1].Value |> float
                m.[1,0] <- mB.Groups.[2].Value |> float

                ((Some m), tokens)
            | Some m ->
                if mB.Success then // second line of machine input
                    m.[0,1] <- mB.Groups.[1].Value |> float
                    m.[1,1] <- mB.Groups.[2].Value |> float
                    (Some m, tokens)
                else // third line of machine input, all required data present
                    let v = new Vector([|mP.Groups.[1].Value |> float; mP.Groups.[2].Value |> float|])
                    if Day13.scale then v.AddInplace (new Vector([|10000000000000.; 10000000000000.|]))
                    match Day13.solveEquation m v with
                    | None -> (None, tokens)
                    | Some r -> (None, tokens + (Day13.usedTokens r))

    static member Star1 : string[] -> string =
        string << snd << Array.fold Day13.handleMachines (None, 0.)

    static member Star2 : string[] -> string =
        Day13.scale <- true; Day13.Star1
