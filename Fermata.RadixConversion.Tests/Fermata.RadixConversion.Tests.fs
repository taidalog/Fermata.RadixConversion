// Fermata.RadixConversion Version 1.2.1
// https://github.com/taidalog/Fermata.RadixConversion
// Copyright (c) 2024-2025 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/Fermata.RadixConversion/blob/main/LICENSE
module Fermata.RadixConversion.Tests

open System
open Xunit
open Fermata.RadixConversion

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

[<Theory>]
[<InlineData(0, 0)>]
[<InlineData(1, 1)>]
[<InlineData(42, 42)>]
let ``int Dec`` (x: int) (y: int) :unit=
    let actual = int (Dec x)
    let expected = y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData(0, 0)>]
[<InlineData(1, 1)>]
[<InlineData(42, 42)>]
let ``dec 1`` (x: int) (y: int) :unit=
    let actual = dec x
    let expected = Dec y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData("0", 0)>]
[<InlineData("1", 1)>]
[<InlineData("101010", 42)>]
let ``dec 2`` (x: string) (y: int) :unit=
    let actual = dec (Bin x)
    let expected = Dec y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData("0", 0)>]
[<InlineData("1", 1)>]
[<InlineData("2a", 42)>]
let ``dec 3`` (x: string) (y: int) :unit=
    let actual = dec (Hex x)
    let expected = Dec y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData(0, 0)>]
[<InlineData(1, 1)>]
[<InlineData(42, 42)>]
let ``dec 4`` (x: int) (y: int) :unit=
    let actual = dec (Dec x)
    let expected = Dec y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData('0', 48)>]
[<InlineData('A', 65)>]
[<InlineData('あ', 12354)>]
let ``dec 5`` (x: char) (y: int) :unit=
    let actual = dec (int x)
    let expected = Dec y
    Assert.Equal(expected, actual)

[<Fact>]
let ``dec-error`` () =
    Assert.Throws<System.FormatException> (Action(fun _ -> dec "a" |> ignore))

[<Fact>]
let ``Dec.validate 1`` () =
    let actual = "2" |> Dec.validate
    let expected = Ok(Dec 2)
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.validate 2`` () =
    let actual = "0" |> Dec.validate
    let expected = Ok(Dec 0)
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.validate 3`` () =
    let actual = "0000" |> Dec.validate
    let expected = Ok(Dec 0)
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.validate 4`` () =
    let actual = "42" |> Dec.validate
    let expected = Ok(Dec 42)
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.validate 5`` () =
    let actual = "0042" |> Dec.validate
    let expected = Ok(Dec 42)
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.validate 6`` () =
    let actual = "FF" |> Dec.validate

    let expected: Result<Dec,exn> =
        Error(FormatException "The input string 'FF' was not in a correct format.")

    testResultToBeError expected actual

[<Fact>]
let ``Dec.validate 7`` () =
    let actual = "2147483648" |> Dec.validate

    let expected: Result<Dec,exn> =
        Error(OverflowException "Value was either too large or too small for an Int32.")

    testResultToBeError expected actual

[<Fact>]
let ``Dec.toBin 1`` () =
    let actual = Dec 42 |> Dec.toBin
    let expected = Bin "101010"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.toBin 2`` () =
    let actual = "42." |> Dec.validate |> Result.map Dec.toBin

    let expected: Result<Bin,exn> =
        Error(FormatException "The input string '42.' was not in a correct format.")

    testResultToBeError expected actual

[<Fact>]
let ``Dec.toHex 1`` () =
    let actual = Dec 42 |> Dec.toHex
    let expected = Hex "2a"
    Assert.Equal(expected, actual)

[<Fact>]
let ``Dec.toHex 2`` () =
    let actual = "42." |> Dec.validate |> Result.map Dec.toHex

    let expected: Result<Hex,exn> =
        Error(FormatException "The input string '42.' was not in a correct format.")

    testResultToBeError expected actual

[<Theory>]
[<InlineData("0", 0)>]
[<InlineData("1", 1)>]
[<InlineData("00", 0)>]
[<InlineData("01", 1)>]
[<InlineData("101010", 42)>]
let ``int Bin`` (x: string) (y: int):unit =
    let actual = int (Bin x)
    let expected = y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData(0, "0")>]
[<InlineData(1, "1")>]
[<InlineData(42, "101010")>]
let ``bin 1`` (x: int) (y: string) :unit=
    let actual = bin x
    let expected = Bin y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData(0, "0")>]
[<InlineData(1, "1")>]
[<InlineData(42, "101010")>]
let ``bin 2`` (x: int) (y: string) :unit=
    let actual = bin (Dec x)
    let expected = Bin y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData("0", "0")>]
[<InlineData("1", "1")>]
[<InlineData("2a", "101010")>]
let ``bin 3`` (x: string) (y: string) :unit=
    let actual = bin (Hex x)
    let expected = Bin y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData("00000000", "0")>]
[<InlineData("00000001", "1")>]
[<InlineData("00101010", "101010")>]
let ``bin 4`` (x: string) (y: string) :unit=
    let actual = bin (Bin x)
    let expected = Bin y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData('0', "110000")>]
[<InlineData('A', "1000001")>]
[<InlineData('あ', "11000001000010")>]
let ``bin 5`` (x: char) (y: string) :unit=
    let actual = bin (int x)
    let expected = Bin y
    Assert.Equal(expected, actual)

[<Fact>]
let ``bin-error`` () =
    Assert.Throws<System.FormatException> (Action(fun _ -> bin "a" |> ignore))

[<Fact>]
let ``Bin.validate 1`` () =
    let actual = "1" |> Bin.validate
    let expected = Ok(Bin "1")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.validate 2`` () =
    let actual = "0" |> Bin.validate
    let expected = Ok (Bin "0")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.validate 3`` () =
    let actual = "0000" |> Bin.validate
    let expected = Ok(Bin "0")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.validate 4`` () =
    let actual = "101010" |> Bin.validate
    let expected = Ok(Bin "101010")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.validate 5`` () =
    let actual = Bin.validate "0000101010"
    let expected = Ok(Bin "101010")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.validate 6`` () =
    let actual = "FF" |> Bin.validate

    let expected: Result<Bin,exn> =
        Error(FormatException "The input string 'FF' was not in a correct format.")

    testResultToBeError expected actual

[<Fact>]
let ``Bin.validate 7`` () =
    let actual = "100000000000000000000000000000000" |> Bin.validate

    let expected: Result<Bin,exn> =
        Error(OverflowException "Value is too long. Value must be shorter or equal to 32")

    testResultToBeError expected actual

[<Fact>]
let ``Bin.toDec 1`` () =
    let actual = Bin "101010" |> Bin.toDec
    let expected = Dec 42
    Assert.Equal(expected, actual)

[<Fact>]
let ``Bin.toDec 2`` () =
    let actual = "XX" |> Bin.validate |> Result.map Bin.toDec

    let expected: Result<Dec,exn> =
        Error(FormatException "The input string 'XX' was not in a correct format.")

    testResultToBeError expected actual

[<Theory>]
[<InlineData("0", 0)>]
[<InlineData("1", 1)>]
[<InlineData("00", 0)>]
[<InlineData("01", 1)>]
[<InlineData("2a", 42)>]
[<InlineData("02a", 42)>]
let ``int Hex`` (x: string) (y: int):unit =
    let actual = int (Hex x)
    let expected = y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData(0, "0")>]
[<InlineData(1, "1")>]
[<InlineData(42, "2a")>]
let ``hex 1`` (x: int) (y: string) :unit=
    let actual = hex x
    let expected = Hex y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData(0, "0")>]
[<InlineData(1, "1")>]
[<InlineData(42, "2a")>]
let ``hex 2`` (x: int) (y: string) :unit=
    let actual = hex (Dec x)
    let expected = Hex y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData("0", "0")>]
[<InlineData("1", "1")>]
[<InlineData("101010", "2a")>]
let ``hex 3`` (x: string) (y: string) :unit=
    let actual = hex (Bin x)
    let expected = Hex y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData("00000000", "0")>]
[<InlineData("00000001", "1")>]
[<InlineData("0000002a", "2a")>]
let ``hex 4`` (x: string) (y: string) :unit=
    let actual = hex (Hex x)
    let expected = Hex y
    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData('0', "30")>]
[<InlineData('A', "41")>]
[<InlineData('あ', "3042")>]
let ``hex 5`` (x: char) (y: string) :unit=
    let actual = hex (int x)
    let expected = Hex y
    Assert.Equal(expected, actual)

[<Fact>]
let ``hex-error`` () =
    Assert.Throws<System.FormatException> (Action(fun _ -> hex "a" |> ignore))

[<Fact>]
let ``Hex.validate 1`` () =
    let actual = "a" |> Hex.validate
    let expected = Ok(Hex "a")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.validate 2`` () =
    let actual = "0" |> Hex.validate
    let expected = Ok(Hex "0")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.validate 3`` () =
    let actual = "0000" |> Hex.validate
    let expected = Ok(Hex "0")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.validate 4`` () =
    let actual = "FF" |> Hex.validate
    let expected = Ok(Hex "ff")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.validate 5`` () =
    let actual = Hex.validate "00FF"
    let expected = Ok(Hex "ff")
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.validate 6`` () =
    let actual = "XX" |> Hex.validate

    let expected: Result<Hex,exn> =
        Error(FormatException "The input string 'XX' was not in a correct format.")

    testResultToBeError expected actual

[<Fact>]
let ``Hex.validate 7`` () =
    let actual = "FFFFFFFFF" |> Hex.validate

    let expected: Result<Hex,exn> =
        Error(OverflowException "Value is too long. Value must be shorter or equal to 8")

    testResultToBeError expected actual

[<Fact>]
let ``Hex.toDec 1`` () =
    let actual = Hex "A1" |> Hex.toDec
    let expected = Dec 161
    Assert.Equal(expected, actual)
[<Fact>]
let ``Hex.toDec 2`` () =
    let actual = Hex "ff" |> Hex.toDec
    let expected = Dec 255
    Assert.Equal(expected, actual)

[<Fact>]
let ``Hex.toDec 3`` () =
    let actual = "XX" |> Hex.validate |> Result.map Hex.toDec

    let expected: Result<Dec,exn> =
        Error(FormatException "The input string 'XX' was not in a correct format.")

    testResultToBeError expected actual

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
