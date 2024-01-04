// Fermata.RadixConversion Version 1.0.0
// https://github.com/taidalog/Fermata.RadixConversion
// Copyright (c) 2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/Fermata.RadixConversion/blob/main/LICENSE
module Fermata.RadixConversion.Tests

open System
open Xunit
open Fermata
open Fermata.RadixConversion

let testDecToBeError (expected: Dec) (actual: Dec) : unit =
    match expected with
    | Dec.Valid _ -> Assert.Fail "'expected' was a Dec.Valid."
    | Dec.Invalid e1 ->
        match actual with
        | Dec.Valid _ -> Assert.Fail "'actual' was a Dec.Valid."
        | Dec.Invalid e2 ->
            if e2.GetType().Name = e1.GetType().Name then
                Assert.Equal(e1.Message, e2.Message)
            else
                Assert.Fail "Error types didn't match."

let testBinToBeError (expected: Bin) (actual: Bin) : unit =
    match expected with
    | Bin.Valid _ -> Assert.Fail "'expected' was a Bin.Valid."
    | Bin.Invalid e1 ->
        match actual with
        | Bin.Valid _ -> Assert.Fail "'actual' was a Bin.Valid."
        | Bin.Invalid e2 ->
            if e2.GetType().Name = e1.GetType().Name then
                Assert.Equal(e1.Message, e2.Message)
            else
                Assert.Fail "Error types didn't match."

let testHexToBeError (expected: Hex) (actual: Hex) : unit =
    match expected with
    | Hex.Valid _ -> Assert.Fail "'expected' was a Hex.Valid."
    | Hex.Invalid e1 ->
        match actual with
        | Hex.Valid _ -> Assert.Fail "'actual' was a Hex.Valid."
        | Hex.Invalid e2 ->
            if e2.GetType().Name = e1.GetType().Name then
                Assert.Equal(e1.Message, e2.Message)
            else
                Assert.Fail "Error types didn't match."

let testArbToBeError (expected: Arb) (actual: Arb) : unit =
    match expected with
    | Arb.Valid _ -> Assert.Fail "'expected' was a Arb.Valid."
    | Arb.Invalid e1 ->
        match actual with
        | Arb.Valid _ -> Assert.Fail "'actual' was a Arb.Valid."
        | Arb.Invalid e2 ->
            if e2.GetType().Name = e1.GetType().Name then
                Assert.Equal(e1.Message, e2.Message)
            else
                Assert.Fail "Error types didn't match."

let testResultToBeError (expected: Result<'T, exn>) (actual: Result<'T, exn>) : unit =
    match expected with
    | Ok _ -> Assert.Fail "'expected' was an Ok value."
    | Error e1 ->
        match actual with
        | Ok _ -> Assert.Fail "'actual' was an Ok value."
        | Error e2 ->
            if e2.GetType().Name = e1.GetType().Name then
                Assert.Equal(e1.Message, e2.Message)
            else
                Assert.Fail "Error messages didn't match."

[<Fact>]
let ``Dec.validate 1`` () =
    let actual = "42" |> Dec.validate
    let expected = Dec.Valid 42
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.validate 2`` () =
    let actual = "FF" |> Dec.validate

    let expected =
        Dec.Invalid(FormatException "The input string 'FF' was not in a correct format.")

    testDecToBeError expected actual

[<Fact>]
let ``Dec.validate 3`` () =
    let actual = "2147483648" |> Dec.validate

    let expected =
        Dec.Invalid(OverflowException "Value was either too large or too small for an Int32.")

    testDecToBeError expected actual

[<Fact>]
let ``Dec.toBin 1`` () =
    let actual = "42" |> Dec.validate |> Dec.toBin
    let expected = Bin.Valid "101010"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.toBin 2`` () =
    let actual = "42." |> Dec.validate |> Dec.toBin

    let expected =
        Bin.Invalid(FormatException "The input string '42.' was not in a correct format.")

    testBinToBeError expected actual

[<Fact>]
let ``Dec.toHex 1`` () =
    let actual = "42" |> Dec.validate |> Dec.toHex
    let expected = Hex.Valid "2a"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.toHex 2`` () =
    let actual = "42." |> Dec.validate |> Dec.toHex

    let expected =
        Hex.Invalid(FormatException "The input string '42.' was not in a correct format.")

    testHexToBeError expected actual

[<Fact>]
let ``Bin.validate 1`` () =
    let actual = "101010" |> Bin.validate
    let expected = Bin.Valid "101010"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.validate 2`` () =
    let actual = "FF" |> Bin.validate

    let expected =
        Bin.Invalid(FormatException "The input string 'FF' was not in a correct format.")

    testBinToBeError expected actual

[<Fact>]
let ``Bin.validate 3`` () =
    let actual = "100000000000000000000000000000000" |> Bin.validate

    let expected =
        Bin.Invalid(OverflowException "Value is too long. Value must be shorter or equal to 32")

    testBinToBeError expected actual

[<Fact>]
let ``Bin.toDec 1`` () =
    let actual = "101010" |> Bin.validate |> Bin.toDec
    let expected = Dec.Valid 42
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.toDec 2`` () =
    let actual = "XX" |> Bin.validate |> Bin.toDec

    let expected =
        Dec.Invalid(FormatException "The input string 'XX' was not in a correct format.")

    testDecToBeError expected actual

[<Fact>]
let ``Hex.validate 1`` () =
    let actual = "FF" |> Hex.validate
    let expected = Hex.Valid "FF"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.validate 2`` () =
    let actual = "XX" |> Hex.validate

    let expected =
        Hex.Invalid(FormatException "The input string 'XX' was not in a correct format.")

    testHexToBeError expected actual

[<Fact>]
let ``Hex.validate 3`` () =
    let actual = "FFFFFFFFF" |> Hex.validate

    let expected =
        Hex.Invalid(OverflowException "Value is too long. Value must be shorter or equal to 8")

    testHexToBeError expected actual

[<Fact>]
let ``Hex.toDec 1`` () =
    let actual = "ff" |> Hex.validate |> Hex.toDec
    let expected = Dec.Valid 255
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.toDec 2`` () =
    let actual = "XX" |> Hex.validate |> Hex.toDec

    let expected =
        Dec.Invalid(FormatException "The input string 'XX' was not in a correct format.")

    testDecToBeError expected actual

[<Fact>]
let ``Arb.ofInt 1`` () =
    let actual = Arb.ofInt 2 "01" 42
    let expected = Arb.Valid(2, "01", "101010")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Arb.ofInt 2`` () =
    let actual = Arb.ofInt 5 "01234" 42
    let expected = Arb.Valid(5, "01234", "132")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Arb.ofInt 3`` () =
    let actual = Arb.ofInt 5 "HMNPY" 42
    let expected = Arb.Valid(5, "HMNPY", "MPN")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Arb.ofInt 4`` () =
    let actual = Arb.ofInt 1 "0" 42
    let expected = Arb.Invalid(ArgumentException "Radix must be greater than 1.")
    testArbToBeError expected actual

[<Fact>]
let ``Arb.ofInt 5`` () =
    let actual = Arb.ofInt 16 "" 42
    let expected = Arb.Invalid(ArgumentException "Symbols were not specified.")
    testArbToBeError expected actual

[<Fact>]
let ``Arb.ofInt 6`` () =
    let actual = Arb.ofInt 16 "01" 42

    let expected =
        Arb.Invalid(ArgumentException "The number of the symbols and the radix didn't match.")

    testArbToBeError expected actual

[<Fact>]
let ``Arb.toInt 1`` () =
    let actual = Arb.toInt (Arb.Valid(2, "01", "101010"))
    let expected = Ok 42
    Assert.Equal(expected, actual)

[<Fact>]
let ``Arb.toInt 2`` () =
    let actual = Arb.toInt (Arb.Valid(5, "01234", "132"))
    let expected = Ok 42
    Assert.Equal(expected, actual)

[<Fact>]
let ``Arb.toInt 3`` () =
    let actual = Arb.toInt (Arb.Valid(5, "HMNPY", "MPN"))
    let expected = Ok 42
    Assert.Equal(expected, actual)

[<Fact>]
let ``Arb.toInt 4`` () =
    let actual = Arb.toInt (Arb.Valid(1, "0", "0"))

    let expected: Result<int, exn> =
        Error(ArgumentException "Radix must be greater than 1.")

    testResultToBeError expected actual

[<Fact>]
let ``Arb.toInt 5`` () =
    let actual = Arb.toInt (Arb.Valid(16, "", "2a"))

    let expected: Result<int, exn> =
        Error(ArgumentException "Symbols were not specified.")

    testResultToBeError expected actual

[<Fact>]
let ``Arb.toInt 6`` () =
    let actual = Arb.toInt (Arb.Valid(16, "01", "2a"))

    let expected: Result<int, exn> =
        Error(ArgumentException "The number of the symbols and the radix didn't match.")

    testResultToBeError expected actual

[<Fact>]
let ``Arb.toInt 7`` () =
    let actual = Arb.toInt (Arb.Valid(16, "0123456789abcdef", "7ffffffff")) // over `Int32.MaxValue`.

    let expected: Result<int, exn> =
        Error(OverflowException "Arithmetic operation resulted in an overflow.")

    testResultToBeError expected actual
