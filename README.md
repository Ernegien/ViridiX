# ViridiX

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

ViridiX is a collection of libraries and applications designed to interface with a debug/modified Xbox (original) over a network. The name is a combination of the Latin word for green, *viridis*, and X for Xbox. The main purpose is to facilitate the public documentation of Xbox internals and provide those interested with the tools necessary to dig deeper.

The solution's project names are all loosely artisan-based, currently composed of the following:

  - Janitor - General utility/extension library; where all the shit piles up.
  - Mason - Common core library; the basic building blocks.
  - Translator - Assembler/disassembler to be used for remote code injection.
  - Linguist - The main communication library and xbdm.dll replacement.
  - Explorer - An Xbox Neighborhood replacement compatible with the latest Windows versions.
  - Ventriloquist - Provides the ability to remotely control and view an Xbox.

All libraries should be developed with CLS compliancy in mind to allow for easier VB.NET consumption using their baby datatypes.

#### Requirements

[.NET Framework 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42643)
[Visual Studio Community 2015](https://www.microsoft.com/en-us/download/details.aspx?id=48146)
