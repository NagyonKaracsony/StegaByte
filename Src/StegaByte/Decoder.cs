using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace StegaByte
{
    /// <summary>
    /// Holds functionality for the StegaByte image decoder.
    /// </summary>
    public class Decoder
    {
        /// <summary>
        /// Decodes an object from a .PNG format image file located at the specified <paramref name="filePath"/> by extracting and deserializing embedded data.
        /// </summary>
        /// <param name="filePath">The file path of the PNG image containing encoded data.</param>
        /// <returns>The decoded object.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the <paramref name="filePath"/> file does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the decoder couldn't extract the type encoded to the image, or if the deserialization fails.</exception>
        public static object Decode(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("Image file not found.", filePath);

            byte[] combinedBytes;
            using (Bitmap bitmap = new Bitmap(filePath))
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                combinedBytes = new byte[width * height * 3];

                Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, Math.Min(Environment.ProcessorCount / 2, Environment.ProcessorCount)) }, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = (y * width + x) * 3;
                        Color color;
                        lock (bitmap)
                        {
                            color = bitmap.GetPixel(x, y);
                        }
                        if (index < combinedBytes.Length) combinedBytes[index] = color.R;
                        if (index + 1 < combinedBytes.Length) combinedBytes[index + 1] = color.G;
                        if (index + 2 < combinedBytes.Length) combinedBytes[index + 2] = color.B;
                    }
                });
            }

            int typeLength = BitConverter.ToInt32(combinedBytes, 0);
            byte[] typeBytes = new byte[typeLength];
            byte[] dataBytes = new byte[combinedBytes.Length - 4 - typeLength];

            Array.Copy(combinedBytes, 4, typeBytes, 0, typeLength);
            Array.Copy(combinedBytes, 4 + typeLength, dataBytes, 0, dataBytes.Length);

            string typeName = Encoding.UTF8.GetString(typeBytes);
            Type type = Type.GetType(typeName) ?? throw new InvalidOperationException("Unable to resolve type.");
            string jsonData = Encoding.UTF8.GetString(dataBytes).TrimEnd('\0');

            return JsonSerializer.Deserialize(jsonData, type) ?? throw new InvalidOperationException("Failed to deserialize object.");
        }
    }
}