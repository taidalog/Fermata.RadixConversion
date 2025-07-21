// Fermata.RadixConversion Version 1.2.1
// https://github.com/taidalog/Fermata.RadixConversion
// Copyright (c) 2024-2025 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/Fermata.RadixConversion/blob/main/LICENSE
namespace Fermata.RadixConversion

type Dec =
    | Dec of int
    static member op_Explicit: Dec -> int

type Bin =
    | Bin of string
    static member op_Explicit: Bin -> int

type Hex =
    | Hex of string
    static member op_Explicit: Hex -> int

type Arb =
    | Valid of radix: int * symbols: seq<char> * value: string
    | Invalid of exn

[<AutoOpen>]
module Core =

    /// <summary>Returns the equivalent <c>Dec</c> representation of the input value.</summary>
    ///
    /// <param name="x">The input value.</param>
    ///
    /// <typeparam name="'T">Requires static member <c>op_Explicit</c></typeparam>
    ///
    /// <returns><c>Dec v</c>.</returns>
    ///
    /// <example id="dec-1">
    /// <code lang="fsharp">
    /// dec 42
    /// </code>
    /// Evaluates to <c>Dec 42</c>
    /// </example>
    ///
    /// <example id="dec-2">
    /// <code lang="fsharp">
    /// dec (Bin "101010")
    /// </code>
    /// Evaluates to <c>Dec 42</c>
    /// </example>
    ///
    /// <example id="dec-3">
    /// <code lang="fsharp">
    /// dec (Hex "2a")
    /// </code>
    /// Evaluates to <c>Dec 42</c>
    /// </example>
    ///
    /// <example id="dec-4">
    /// <code lang="fsharp">
    /// dec (Dec 42)
    /// </code>
    /// Evaluates to <c>Dec 42</c>
    /// </example>
    val inline dec: x: ^T -> Dec when ^T: (static member op_Explicit: ^T -> int)

    /// <summary>Returns the equivalent <c>Bin</c> representation of the input value.</summary>
    ///
    /// <param name="x">The input value.</param>
    ///
    /// <typeparam name="'T">Requires static member <c>op_Explicit</c></typeparam>
    ///
    /// <returns><c>Bin v</c>.</returns>
    ///
    /// <example id="bin-1">
    /// <code lang="fsharp">
    /// bin 42
    /// </code>
    /// Evaluates to <c>Bin "101010"</c>
    /// </example>
    ///
    /// <example id="bin-2">
    /// <code lang="fsharp">
    /// bin (Dec 42)
    /// </code>
    /// Evaluates to <c>Bin "101010"</c>
    /// </example>
    ///
    /// <example id="bin-3">
    /// <code lang="fsharp">
    /// bin (Hex "2a")
    /// </code>
    /// Evaluates to <c>Bin "101010"</c>
    /// </example>
    ///
    /// <example id="bin-4">
    /// <code lang="fsharp">
    /// bin (Bin "00101010")
    /// </code>
    /// Evaluates to <c>Bin "101010"</c>
    /// </example>
    val inline bin: x: ^T -> Bin when ^T: (static member op_Explicit: ^T -> int)

    /// <summary>Returns the equivalent <c>Hex</c> representation of the input value.</summary>
    ///
    /// <param name="x">The input value.</param>
    ///
    /// <typeparam name="'T">Requires static member <c>op_Explicit</c></typeparam>
    ///
    /// <returns><c>Hex v</c>.</returns>
    ///
    /// <example id="hex-1">
    /// <code lang="fsharp">
    /// hex 42
    /// </code>
    /// Evaluates to <c>Hex "2a"</c>
    /// </example>
    ///
    /// <example id="hex-2">
    /// <code lang="fsharp">
    /// hex (Dec 42)
    /// </code>
    /// Evaluates to <c>Hex "2a"</c>
    /// </example>
    ///
    /// <example id="hex-3">
    /// <code lang="fsharp">
    /// hex (Bin "101010")
    /// </code>
    /// Evaluates to <c>Hex "2a"</c>
    /// </example>
    ///
    /// <example id="hex-4">
    /// <code lang="fsharp">
    /// hex (Hex "002a")
    /// </code>
    /// Evaluates to <c>Hex "2a"</c>
    /// </example>
    val inline hex: x: ^T -> Hex when ^T: (static member op_Explicit: ^T -> int)

[<RequireQualifiedAccess>]
module Dec =

    /// <summary>Returns <c>Ok(Dec v)</c> if the input string can be parsed as a decimal number, otherwise <c>Error e</c>.</summary>
    ///
    /// <param name="input">The input string.</param>
    ///
    /// <returns><c>Ok(Dec v)</c> if the input string can be parsed as a decimal number, otherwise <c>Error e</c>.</returns>
    ///
    /// <example id="Dec.validate-1">
    /// <code lang="fsharp">
    /// "2" |> Dec.validate
    /// </code>
    /// Evaluates to <c>Ok(Dec 2)</c>
    /// </example>
    ///
    /// <example id="Dec.validate-2">
    /// <code lang="fsharp">
    /// "0" |> Dec.validate
    /// </code>
    /// Evaluates to <c>Ok(Dec 0)</c>
    /// </example>
    ///
    /// <example id="Dec.validate-3">
    /// <code lang="fsharp">
    /// "0000" |> Dec.validate
    /// </code>
    /// Evaluates to <c>Ok(Dec 0)</c>
    /// </example>
    ///
    /// <example id="Dec.validate-4">
    /// <code lang="fsharp">
    /// "42" |> Dec.validate
    /// </code>
    /// Evaluates to <c>Ok(Dec 42)</c>
    /// </example>
    ///
    /// <example id="Dec.validate-5">
    /// <code lang="fsharp">
    /// "0042" |> Dec.validate
    /// </code>
    /// Evaluates to <c>Ok(Dec 42)</c>
    /// </example>
    ///
    /// <example id="Dec.validate-6">
    /// <code lang="fsharp">
    /// "FF" |> Dec.validate
    /// </code>
    /// Evaluates to <c>Error(FormatException "The input string 'FF' was not in a correct format.")</c>
    /// </example>
    ///
    /// <example id="Dec.validate-7">
    /// <code lang="fsharp">
    /// "2147483648" |> Dec.validate
    /// </code>
    /// Evaluates to <c>Error(OverflowException "Value was either too large or too small for an Int32.")</c>
    /// </example>
    val validate: input: string -> Result<Dec, exn>

    /// <summary>Returns the equivalent <c>Bin</c> representation of the input <c>Dec</c> value.</summary>
    ///
    /// <param name="dec">The input <c>Dec</c>.</param>
    ///
    /// <returns>The equivalent <c>Bin</c> representation of the input <c>Dec</c> value.</returns>
    ///
    /// <example id="Dec.toBin-1">
    /// <code lang="fsharp">
    /// Dec "42" |> Dec.toBin
    /// </code>
    /// Evaluates to <c>Bin "101010"</c>
    /// </example>
    ///
    /// <example id="Dec.toBin-2">
    /// <code lang="fsharp">
    /// "42." |> Dec.validate |> Dec.toBin
    /// </code>
    /// Evaluates to <c>Error(FormatException "The input string '42.' was not in a correct format.")</c>
    /// </example>
    val toBin: Dec -> Bin

    /// <summary>Returns the equivalent <c>Hex</c> representation of the input <c>Dec</c> value.</summary>
    ///
    /// <param name="dec">The input <c>Dec</c>.</param>
    ///
    /// <returns>The equivalent <c>Hex</c> representation of the input <c>Dec</c> value.</returns>
    ///
    /// <example id="Dec.toHex-1">
    /// <code lang="fsharp">
    /// Dec 42 |> Dec.toHex
    /// </code>
    /// Evaluates to <c>Hex "2a"</c>
    /// </example>
    ///
    /// <example id="Dec.toHex-2">
    /// <code lang="fsharp">
    /// "42." |> Dec.validate |> Dec.toHex
    /// </code>
    /// Evaluates to <c>Error(FormatException "The input string '42.' was not in a correct format.")</c>
    /// </example>
    val toHex: Dec -> Hex

[<RequireQualifiedAccess>]
module Bin =

    /// <summary>Returns <c>Ok(Bin v)</c> if the input string can be parsed as a binary number, otherwise <c>Error e</c>.</summary>
    ///
    /// <param name="input">The input string.</param>
    ///
    /// <returns><c>Ok(Bin v)</c> if the input string can be parsed as a binary number, otherwise <c>Error e</c>.</returns>
    ///
    /// <example id="Bin.validate-1">
    /// <code lang="fsharp">
    /// "1" |> Bin.validate
    /// </code>
    /// Evaluates to <c>Ok(Bin "1")</c>
    /// </example>
    ///
    /// <example id="Bin.validate-2">
    /// <code lang="fsharp">
    /// "0" |> Bin.validate
    /// </code>
    /// Evaluates to <c>Ok (Bin "0")</c>
    /// </example>
    ///
    /// <example id="Bin.validate-3">
    /// <code lang="fsharp">
    /// "0000" |> Bin.validate
    /// </code>
    /// Evaluates to <c>Ok(Bin "0")</c>
    /// </example>
    ///
    /// <example id="Bin.validate-4">
    /// <code lang="fsharp">
    /// "101010" |> Bin.validate
    /// </code>
    /// Evaluates to <c>Ok(Bin "101010")</c>
    /// </example>
    ///
    /// <example id="Bin.validate-5">
    /// <code lang="fsharp">
    /// Bin.validate "0000101010"
    /// </code>
    /// Evaluates to <c>Ok(Bin "101010")</c>
    /// </example>
    ///
    /// <example id="Bin.validate-6">
    /// <code lang="fsharp">
    /// "FF" |> Bin.validate
    /// </code>
    /// Evaluates to <c>Error(FormatException "The input string 'FF' was not in a correct format.")</c>
    /// </example>
    ///
    /// <example id="Bin.validate-7">
    /// <code lang="fsharp">
    /// "100000000000000000000000000000000" |> Bin.validate
    /// </code>
    /// Evaluates to <c>Error(OverflowException "Value is too long. Value must be shorter or equal to 32")</c>
    /// </example>
    val validate: input: string -> Result<Bin, exn>

    /// <summary>Returns the equivalent <c>Dec</c> representation of the input <c>Bin</c> value.</summary>
    ///
    /// <param name="bin">The input <c>Bin</c>.</param>
    ///
    /// <returns>The equivalent <c>Dec</c> representation of the input <c>Bin</c> value.</returns>
    ///
    /// <example id="Bin.toDec-1">
    /// <code lang="fsharp">
    /// Bin "101010" |> Bin.toDec
    /// </code>
    /// Evaluates to <c>Dec 42</c>
    /// </example>
    ///
    /// <example id="Bin.toDec-2">
    /// <code lang="fsharp">
    /// "XX" |> Bin.validate |> Result.map Bin.toDec
    /// </code>
    /// Evaluates to <c>Error(FormatException "The input string 'XX' was not in a correct format.")</c>
    /// </example>
    val toDec: Bin -> Dec

[<RequireQualifiedAccess>]
module Hex =

    /// <summary>Returns <c>Ok(Hex v)</c> if the input string can be parsed as a hexadecimal number, otherwise <c>Error e</c>.</summary>
    ///
    /// <param name="input">The input string.</param>
    ///
    /// <returns><c>Ok(Hex v)</c> if the input string can be parsed as a hexadecimal number, otherwise <c>Error e</c>.</returns>
    ///
    /// <example id="Hex.validate-1">
    /// <code lang="fsharp">
    /// "a" |> Hex.validate
    /// </code>
    /// Evaluates to <c>Ok(Hex "a")</c>
    /// </example>
    ///
    /// <example id="Hex.validate-2">
    /// <code lang="fsharp">
    /// "0" |> Hex.validate
    /// </code>
    /// Evaluates to <c>Ok(Hex "0")</c>
    /// </example>
    ///
    /// <example id="Hex.validate-3">
    /// <code lang="fsharp">
    /// "0000" |> Hex.validate
    /// </code>
    /// Evaluates to <c>Ok(Hex "0")</c>
    /// </example>
    ///
    /// <example id="Hex.validate-4">
    /// <code lang="fsharp">
    /// "FF" |> Hex.validate
    /// </code>
    /// Evaluates to <c>Ok(Hex "ff")</c>
    /// </example>
    ///
    /// <example id="Hex.validate-5">
    /// <code lang="fsharp">
    /// Hex.validate "00FF"
    /// </code>
    /// Evaluates to <c>Ok(Hex "ff")</c>
    /// </example>
    ///
    /// <example id="Hex.validate-6">
    /// <code lang="fsharp">
    /// "XX" |> Hex.validate
    /// </code>
    /// Evaluates to <c>Error(FormatException "The input string 'XX' was not in a correct format.")</c>
    /// </example>
    ///
    /// <example id="Hex.validate-7">
    /// <code lang="fsharp">
    /// "FFFFFFFFF" |> Hex.validate
    /// </code>
    /// Evaluates to <c>Error(OverflowException "Value is too long. Value must be shorter or equal to 8")</c>
    /// </example>
    val validate: input: string -> Result<Hex, exn>

    /// <summary>Returns the equivalent <c>Dec</c> representation of the input <c>Hex</c> value.</summary>
    ///
    /// <param name="hex">The input <c>Hex</c>.</param>
    ///
    /// <returns>The equivalent <c>Dec</c> representation of the input <c>Hex</c> value.</returns>
    ///
    /// <example id="Hex.toDec-1">
    /// <code lang="fsharp">
    /// Hex "A1" |> Hex.toDec
    /// </code>
    /// Evaluates to <c>Dec 161</c>
    /// </example>
    ///
    /// <example id="Hex.toDec-2">
    /// <code lang="fsharp">
    /// Hex "ff" |> Hex.toDec
    /// </code>
    /// Evaluates to <c>Dec 255</c>
    /// </example>
    ///
    /// <example id="Hex.toDec-3">
    /// <code lang="fsharp">
    /// "XX" |> Hex.validate |> Result.map Hex.toDec
    /// </code>
    /// Evaluates to <c>Error(FormatException "The input string 'XX' was not in a correct format.")</c>
    /// </example>
    val toDec: Hex -> Dec

[<RequireQualifiedAccess>]
module Arb =
    /// <summary>Returns <c>Arb.Valid</c> if the radix converstion succeeded, otherwise <c>Arb.Invalid</c>.
    /// <c>Arb</c> is two-case discriminated union that conatins Valid(radix, symbols to represent a number, and the result value), or Invalid.</summary>
    ///
    /// <param name="radix">The radix to convert the number with.</param>
    ///
    /// <param name="symbols">The symbols to represent a number with.</param>
    ///
    /// <param name="number">The input number to convert.</param>
    ///
    /// <returns><c>Arb.Valid</c> if the radix converstion succeeded, otherwise <c>Arb.Invalid</c>.</returns>
    ///
    /// <example id="Arb.ofInt-1">
    /// <code lang="fsharp">
    /// Arb.ofInt 2 "01" 42
    /// </code>
    /// Evaluates to <c>Arb.Valid(2, "01", "101010")</c>
    /// </example>
    ///
    /// <example id="Arb.ofInt-2">
    /// <code lang="fsharp">
    /// Arb.ofInt 5 "01234" 42
    /// </code>
    /// Evaluates to <c>Arb.Valid(5, "01234", "132")</c>
    /// </example>
    ///
    /// <example id="Arb.ofInt-3">
    /// <code lang="fsharp">
    /// Arb.ofInt 5 "HMNPY" 42
    /// </code>
    /// Evaluates to <c>Arb.Valid(5, "HMNPY", "MPN")</c>
    /// </example>
    ///
    /// <example id="Arb.ofInt-4">
    /// <code lang="fsharp">
    /// Arb.ofInt 1 "0" 42
    /// </code>
    /// Evaluates to <c>Arb.Invalid(ArgumentException "Radix must be greater than 1.")</c>
    /// </example>
    ///
    /// <example id="Arb.ofInt-5">
    /// <code lang="fsharp">
    /// Arb.ofInt 16 "" 42
    /// </code>
    /// Evaluates to <c>Arb.Invalid(ArgumentException "Symbols were not specified.")</c>
    /// </example>
    ///
    /// <example id="Arb.ofInt-6">
    /// <code lang="fsharp">
    /// Arb.ofInt 16 "01" 42
    /// </code>
    /// Evaluates to <c>Arb.Invalid(ArgumentException "The number of the symbols and the radix didn't match.")</c>
    /// </example>
    val ofInt: radix: int -> symbols: seq<char> -> number: int -> Arb

    /// <summary>Returns <c>Ok</c> if the radix converstion succeeded, otherwise <c>Error</c>.
    /// <c>Arb</c> is two-case discriminated union that conatins Valid(radix, symbols to represent a number, and the result value), or Invalid.</summary>
    ///
    /// <param name="arb">The input <c>Arb</c>.</param>
    ///
    /// <returns><c>Ok</c> if the radix converstion succeeded, otherwise <c>Error</c>.</returns>
    ///
    /// <example id="Arb.toInt-1">
    /// <code lang="fsharp">
    /// Arb.toInt (Arb.Valid(2, "01", "101010"))
    /// </code>
    /// Evaluates to <c>Ok 42</c>
    /// </example>
    ///
    /// <example id="Arb.toInt-2">
    /// <code lang="fsharp">
    /// Arb.toInt (Arb.Valid(5, "01234", "132"))
    /// </code>
    /// Evaluates to <c>Ok 42</c>
    /// </example>
    ///
    /// <example id="Arb.toInt-3">
    /// <code lang="fsharp">
    /// Arb.toInt (Arb.Valid(5, "HMNPY", "MPN"))
    /// </code>
    /// Evaluates to <c>Ok 42</c>
    /// </example>
    ///
    /// <example id="Arb.toInt-4">
    /// <code lang="fsharp">
    /// Arb.toInt (Arb.Valid(1, "0", "0"))
    /// </code>
    /// Evaluates to <c>Error(ArgumentException "Radix must be greater than 1.")</c>
    /// </example>
    ///
    /// <example id="Arb.toInt-5">
    /// <code lang="fsharp">
    /// Arb.toInt (Arb.Valid(16, "", "2a"))
    /// </code>
    /// Evaluates to <c>Error(ArgumentException "Symbols were not specified.")</c>
    /// </example>
    ///
    /// <example id="Arb.toInt-6">
    /// <code lang="fsharp">
    /// Arb.toInt (Arb.Valid(16, "01", "2a"))
    /// </code>
    /// Evaluates to <c>Error(ArgumentException "The number of the symbols and the radix didn't match.")</c>
    /// </example>
    ///
    /// <example id="Arb.toInt-7">
    /// <code lang="fsharp">
    /// Arb.toInt (Arb.Valid(16, "0123456789abcdef", "7ffffffff")) // over `Int32.MaxValue`.
    /// </code>
    /// Evaluates to <c>Error(OverflowException "Arithmetic operation resulted in an overflow.")</c>
    /// </example>
    val toInt: arb: Arb -> Result<int, exn>
