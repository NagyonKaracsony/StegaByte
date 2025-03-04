# StegaByte
StegaByte is a lightweight C# library that allows you to store any type of data by encoding it directly into PNG images. Unlike traditional text-based formats such as JSON or XML, StegaByte compresses the data during encoding, resulting in significantly smaller storage sizes (typically 10-80% smaller, depending on the content). 

> #### While inspired by steganography techniques, StegaByte is not designed for secure data storage as decoding images made by StegaByte is not too difficult and should not be used for sensitive information.
> StegaByte is best classified as a steganography-based data obfuscation tool, suitable for applications where data readability and tamper-resistance are desired without relying on heavy encryption.

## Features
- Data-to-Image Encoding: Encode virtually any object or dataset into a PNG format image.
- Data Compression: Achieves compression efficiency of 10% to 80%, often outperforming plain text or binary storage methods.
- Lossless Decoding: Ensures exact reconstruction of the original data.
- Flexible Data Types: Supports simple variables, complex structures, and serialized objects.
- Multithreaded Encoding/Decoding: Utilizes multiple CPU threads (up to half of available threads) for faster processing.

## Usage Scenarios
- Game Data Storage: Store configuration files, procedural generation seeds, or other non-sensitive game data in a compressed image format.
- Lightweight anti-cheat or anti-tamper systems
- Preventing casual reverse-engineering of configuration or metadata.
- Non-sensitive Obfuscation: Hide data from plain sight without requiring it to be secure (e.g., for modding tools, experimental data storage, or internal tools).
- Due to it's compression capabilites, you can store significant ammounts if data on a relatively small storage space.

# Examples
For a simple C# console app Encoding/Decoding goes like this:
```Csharp
// Define the data you wish to encode.
string stringToEncode = "Hello, world!";
// define the path where you wish to save the encoded PNG image.
string filePath = "helloWorld.png";

// Encode your variable's data and type onto the image that will be created or overwritten at [filePath].
// the type is extracted during the encoding process, so all you have to do is provide a variable of tpye <object> and a path where the image will be saved.
Encoder.Encode(stringToEncode, filePath);

// Decoding the image
string readStringText = (string)Decoder.Decode(stringTextImagePath);

// Outputs "Hello, world!" to the console.
Console.WriteLine(readStringText);
```
### Important note:
### when Decoding an image you need to cast it to it's original type, or you won't be able to use it's type methods!
#### You can still access the variable's data, but can't use it's type methods without explicitly casting it to a type first, before operations:
```Csharp
// This should work without issues!
var readStringText = Decoder.Decode(filePath);

// Outputs "Hello, world!" to the console.
Console.WriteLine(readStringText);
```
```Csharp
// This will NOT work!
var readStringText = Decoder.Decode(filePath);

foreach(var character in readStringText) { // CS1579 ERROR: Can't operate on type 'object'...
    Console.WriteLine(character);
}
```
```Csharp
// This will NOT work!
var readStringText = Decoder.Decode(filePath);

string reversedReadStringText = readStringText.Reverse(); // CS1061 ERROR: 'object' does not contain a definition for 'Reverse'...
```
#### Correct ways to cast decoded data:
```Csharp
// This should work without issues!
var readStringText = (string)Decoder.Decode(filePath);

foreach(var character in readStringText) {
    Console.WriteLine(character);
}
```
```Csharp
// This should work without issues!
string readStringText = (string)Decoder.Decode(filePath);

foreach(var character in readStringText) {
    Console.WriteLine(character);
}
```
```Csharp
// This should work without issues!
var readStringText = Decoder.Decode(filePath);

string reversedReadStringText = readStringText.ToString().Reverse();
```
## For more examples you can check out the [StegaByte tests](./Src/StegaByte.Tests/UnitTest1.cs).

## Limitations
### Not Encryption-Based:
#### StegaByte does not encrypt data. The images can easily be decoded by anyone familiar with the format. Avoid using it for passwords, sensitive user data, or private information.
### File Size Constraints:
#### Due to disk I/O limitations and the nature of image encoding/decoding, it is not recommended to store datasets exceeding around 30 or 50 MBs in a single image. Performance degradation may occur beyond this point, and reading/ writing could take over a minute even on a high end device with fast NVME SSD storage.

Perfect for hiding data in plain sight.