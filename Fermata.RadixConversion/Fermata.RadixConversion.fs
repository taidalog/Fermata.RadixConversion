// Fermata.RadixConversion Version 2.0.0
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
    | Dec of int
    static member op_Explicit(Dec v) : int = v

type Bin =
    | Bin of string
    static member op_Explicit(Bin v) : int = System.Convert.ToInt32(v, 2)

type Hex =
    | Hex of string
    static member op_Explicit(Hex v) : int = System.Convert.ToInt32(v, 16)

type Arb =
    | Valid of radix: int * symbols: seq<char> * value: string
    | Invalid of exn

[<AutoOpen>]
module Core =
    let inline dec (x: ^T) : Dec = int x |> Dec
    let inline bin (x: ^T) : Bin = System.Convert.ToString(int x, 2) |> Bin
    let inline hex (x: ^T) : Hex = System.Convert.ToString(int x, 16) |> Hex

[<RequireQualifiedAccess>]
module Dec =
    let validate (input: string) : Result<Dec, exn> =
        input
        |> Int32.validate
        |> Result.map Dec

    let toBin (dec: Dec) : Bin =
        let (Dec v) = dec
        System.Convert.ToString(v, 2) |> Bin

    let toHex (dec: Dec) : Hex =
        let (Dec v) = dec
        System.Convert.ToString(v, 16) |> fun (x: string) -> x.ToLower() |> Hex

[<RequireQualifiedAccess>]
module Bin =
    let validate (input: string) : Result<Bin, exn> =
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
        |> Result.map Bin

    let toDec (bin: Bin) : Dec =
        let (Bin v) = bin
        System.Convert.ToInt32(v, 2) |> Dec

[<RequireQualifiedAccess>]
module Hex =
    let validate (input: string) : Result<Hex, exn> =
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
        |> Result.map Hex

    let toDec (hex :Hex) : Dec =
        let (Hex v) = hex
        System.Convert.ToInt32(v, 16) |> Dec

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
