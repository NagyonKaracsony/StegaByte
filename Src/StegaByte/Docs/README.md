# StegaByte

#### StegaByte is a lightweight C# library that allows you to store any type of data by encoding it directly into PNG images. Unlike traditional text-based formats such as JSON or XML, StegaByte compresses the data during encoding, resulting in significantly smaller storage sizes (typically 10-80% smaller, depending on the content).

> #### While inspired by steganography techniques, StegaByte is not designed for secure data storage as decoding images made by StegaByte is not too difficult and should not be used for sensitive information.
> StegaByte is best classified as a steganography-based data obfuscation tool, suitable for applications where data readability and tamper-resistance are desired without relying on heavy encryption.

## Quick Start

**Add StegaByte to your project via NuGet:**
```Bash
dotnet add package StegaByte
```
**Or clone the Github repo and add a project reference:**
```Bash
git clone https://github.com/NagyonKaracsony/StegaByte.git
```

```csharp
using StegaByte;

// Example object
string helloText = "Hello, World!";

// Encode data to PNG
Encoder.Encode(helloText, "hello.png");

// Decode back
string decodedText = (string)Decoder.Decode("hello.png");

Console.WriteLine(decodedText); // Outputs: Hello, World!
```

## Features
- **Data-to-Image Encoding:** Encode virtually any object or dataset into PNG format images.
- **Data Compression:** Achieves compression efficiency of 10% to 90%, confidently outperforming plain text or text-based formats such as JSON or XML as storage methods.
- **Lossless Decoding:** Ensures exact reconstruction of original data.
- **Flexible Data Types:** Supports simple variables, complex structures, and serialized objects.
- **Multithreaded Encoding/Decoding:** Dynamically uses up to **half of the available CPU threads** for faster processing, adjusting intelligently based on hardware.

## Usage Scenarios
- **Game Data Storage:** Store configuration files, procedural generation seeds, or other non-sensitive game data in compressed image format.
- **Lightweight Anti-Cheat / Anti-Tamper Systems:** Obfuscate game config or metadata files.
- **Preventing Casual Reverse-Engineering:** Especially useful in modding tools, experimental storage, or internal apps.
- **Efficient Storage:** Thanks to compression, significant amounts of data can be stored using relatively little disk space (especially compared to JSON/XML).

## Supported Runtimes
- **.NET 6.0**
- **.NET 7.0**
- **.NET 8.0**
- **.NET 9.0 (Preview)**

# Important Note About Casting
> ### When decoding, the result is returned as a generic object.
> ### You MUST cast it to the original type to use type-specific methods!

## Examples:

```Csharp
// This works fine:
var decodedText = (string)Decoder.Decode(filePath);
Console.WriteLine(decodedText.Length); // Type methods accessible.

// This DOES NOT work:
var decodedText = Decoder.Decode(filePath);
Console.WriteLine(decodedText.Length); // ERROR:'object' does not contain a definition for 'Length'...
```

### Alternatively:

```Csharp
// Using ToString() for simple types:
var decoded = Decoder.Decode(filePath);
Console.WriteLine(decoded.ToString().ToUpper()); // Type methods accessible thanks to casting.
```

# Limitations
### Not Encryption-Based:
**StegaByte does not encrypt data**. Anyone familiar with the format can decode it. **Do NOT use for passwords, personal user data, or sensitive information**.
### File Size Constraints:
**Due to disk I/O limitations and the nature of image encoding/decoding, it is not recommended to store datasets exceeding around 30 or 50 MBs in a single image as performance degradation may occur beyond this size, disk I/O and image encoding/decoding times may increase significantly (up to a minute or more even on fast hardware).**