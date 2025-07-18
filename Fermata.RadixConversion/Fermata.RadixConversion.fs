﻿// Fermata.RadixConversion Version 1.2.1
// https://github.com/taidalog/Fermata.RadixConversion
// Copyright (c) 2024-2025 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/Fermata.RadixConversion/blob/main/LICENSE
namespace Fermata.RadixConversion

open System
open System.Text.RegularExpressions
open Fermata
open Fermata.Validators

type Dec =
    | Valid of int
    | Invalid of exn

type Bin =
    | Valid of string
    | Invalid of exn

type Hex =
    | Valid of string
    | Invalid of exn

type Arb =
    | Valid of radix: int * symbols: seq<char> * value: string
    | Invalid of exn

[<RequireQualifiedAccess>]
module Dec =
    let validate (input: string) : Dec =
        input
        |> Int32.validate
        |> function
            | Ok x -> Dec.Valid x
            | Error e -> Dec.Invalid e

    let toBin (dec: Dec) : Bin =
        dec
        |> function
            | Dec.Valid x -> System.Convert.ToString(x, 2) |> Bin.Valid
            | Dec.Invalid e -> Bin.Invalid e

    let toHex (dec: Dec) : Hex =
        dec
        |> function
            | Dec.Valid x -> System.Convert.ToString(x, 16) |> fun (x: string) -> x.ToLower() |> Hex.Valid
            | Dec.Invalid e -> Hex.Invalid e

[<RequireQualifiedAccess>]
module Bin =
    let validate (input: string) : Bin =
        let removeLeadingZeros (input: string) : Result<string, exn> =
            try
                let m = Regex.Match(input, "^0*([01]+)$")
                Ok(m.Groups[1].Value)
            with (e: exn) ->
                Error e

        Ok input
        |> Result.bind validateNotEmptyString
        |> Result.bind (validateFormat "^[01]+$")
        |> Result.bind (validateMaxLength String.length 32)
        |> Result.bind removeLeadingZeros
        |> function
            | Ok x -> Bin.Valid x
            | Error e -> Bin.Invalid e

    let toDec (bin: Bin) : Dec =
        bin
        |> function
            | Bin.Valid x -> System.Convert.ToInt32(x, 2) |> Dec.Valid
            | Bin.Invalid e -> Dec.Invalid e

[<RequireQualifiedAccess>]
module Hex =
    let validate (input: string) : Hex =
        let removeLeadingZeros (input: string) : Result<string, exn> =
            try
                let m = Regex.Match(input, "^0*([0-9A-Fa-f]+)$")
                Ok(m.Groups[1].Value)
            with (e: exn) ->
                Error e

        Ok input
        |> Result.bind validateNotEmptyString
        |> Result.bind (validateFormat "^[0-9A-Fa-f]+$")
        |> Result.bind (validateMaxLength String.length 8)
        |> Result.bind removeLeadingZeros
        |> Result.map (fun (x: string) -> x.ToLower())
        |> function
            | Ok x -> Hex.Valid x
            | Error e -> Hex.Invalid e

    let toDec (hex: Hex) : Dec =
        hex
        |> function
            | Hex.Valid x -> System.Convert.ToInt32(x, 16) |> Dec.Valid
            | Hex.Invalid e -> Dec.Invalid e

[<RequireQualifiedAccess>]
module Arb =
    let rec private divideTill (number: int) (dividend: int) (divisor: int) (acc: int list) : int list =
        let quotient, remainder = System.Math.DivRem(dividend, divisor)
        let acc' = remainder :: acc

        if quotient = 0 then acc'
        else if quotient < number then quotient :: acc'
        else divideTill number quotient divisor acc'

    let ofInt (radix: int) (symbols: seq<char>) (number: int) : Arb =
        if radix < 2 then
            Arb.Invalid(ArgumentException "Radix must be greater than 1.")
        else if Seq.isEmpty symbols then
            Arb.Invalid(ArgumentException "Symbols were not specified.")
        else if Seq.length symbols <> radix then
            Arb.Invalid(ArgumentException "The number of the symbols and the radix didn't match.")
        else
            divideTill radix number radix []
            |> List.map (fun x -> Seq.item x symbols |> string)
            |> String.concat ""
            |> fun x -> Arb.Valid(radix, symbols, x)

    let toInt (arb: Arb) : Result<int, exn> =
        match arb with
        | Arb.Invalid e -> Error e
        | Arb.Valid(radix, symbols, value) ->
            if radix < 2 then
                Error(ArgumentException "Radix must be greater than 1.")
            else if Seq.isEmpty symbols then
                Error(ArgumentException "Symbols were not specified.")
            else if Seq.length symbols <> radix then
                Error(ArgumentException "The number of the symbols and the radix didn't match.")
            else
                try
                    let weights = List.init (String.length value) ((pown) radix) |> List.rev

                    let indexes =
                        value |> Seq.map (fun x -> Seq.findIndex ((=) x) symbols) |> Seq.toList

                    List.zip weights indexes
                    |> List.map (fun (x, y) -> x * y)
                    |> List.fold (+) 0
                    |> Ok
                with
                | :? OverflowException -> Error(OverflowException "Arithmetic operation resulted in an overflow.")
                | _ as e -> Error(ArgumentException e.Message)
