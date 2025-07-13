# Fermata.RadixConversion

F# library for operations related to radix conversion. Compatible with Fable.

Version 1.2.0

## Features

- Provides functions for working with radix conversion in F#.
- Works in a Fable project.

## Target Frameworks

- .NET Standard 2.0
- .NET 7
- .NET 8
- .NET 9

## Modules

- Dec  
   Contains helper functions for base 10 radix conversion (.NET wrapper functions).
- Bin  
   Contains helper functions for base 2 radix conversion (.NET wrapper functions).
- Hex  
   Contains helper functions for base 16 radix conversion (.NET wrapper functions).
- Arb  
   Contains helper functions for radix conversion with an **Arb**itrary base.

For more information, see the signature file (`.fsi`).

## Installation

.NET CLI,

```
dotnet add package Fermata.RadixConversion --Version 1.2.0
```

F# Intaractive,

```
#r "nuget: Fermata.RadixConversion, 1.2.0"
```

For more information, please see [Fermata on NuGet Gallery](https://www.nuget.org/packages/Fermata.RadixConversion).

## Notes

-

## Known Issue

-

## Release Notes

[Releases on GitHub](https://github.com/taidalog/Fermata.RadixConversion/releases)

## Breaking Changes

### 1.2.0

- `Hex.validate` returns the input hexadecimal representation in **lower case**, while it used to return the input value in **upper case or in lower case** according to the input.
- .NET 6.0 is no longer supported.

### 1.1.0

- `Bin.validate` and `Hex.validate` return the input binary number representation or hexadecimal representation **without** leading zeros, while they used to return the input value **with** leading zeros.

### 1.0.0

- Functions in the module contains the built-in exceptions on failure, while they used to contain `Fermata.Exceptions`.

## Links

- [Repository on GitHub](https://github.com/taidalog/Fermata.RadixConversion)
- [NuGet Gallery](https://www.nuget.org/packages/Fermata.RadixConversion)

## License

This product is licensed under the [MIT license](https://github.com/taidalog/Fermata.RadixConversion/blob/main/LICENSE).
