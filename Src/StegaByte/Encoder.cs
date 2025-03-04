using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace StegaByte
{
    /// <summary>
    /// Holds functionality for the StegaByte image encoder.
    /// </summary>
    public class Encoder
    {
        /// <summary>
        /// Encodes <paramref name="obj"/> into a .PNG format image file at the specified <paramref name="filePath"/> by serializing its data and embedding type information.
        /// </summary>
        /// <remarks>
        /// Uses dynamic multithreading (at max half the available threads).
        /// </remarks>
        /// <param name="obj">The object to encode.</param>
        /// <param name="filePath">The file path where the PNG image will be saved.</param>
        /// <exception cref="ArgumentNullException">Thrown if the object is null.</exception>
        public static void Encode(object obj, string filePath)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

            string jsonData = JsonSerializer.Serialize(obj);
            byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);
            string typeName = obj.GetType().AssemblyQualifiedName;
            byte[] typeBytes = Encoding.UTF8.GetBytes(typeName);
            int typeLength = typeBytes.Length;

            byte[] combinedBytes = new byte[4 + typeLength + dataBytes.Length];
            BitConverter.GetBytes(typeLength).CopyTo(combinedBytes, 0);
            typeBytes.CopyTo(combinedBytes, 4);
            dataBytes.CopyTo(combinedBytes, 4 + typeLength);

            int width = (int)Math.Ceiling(Math.Sqrt(combinedBytes.Length / 3.0));
            int height = width;
            int maxThreads = Math.Max(1, Math.Min(Environment.ProcessorCount / 2, Environment.ProcessorCount));

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = maxThreads }, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = (y * width + x) * 3;
                        byte r = index < combinedBytes.Length ? combinedBytes[index] : (byte)0;
                        byte g = index + 1 < combinedBytes.Length ? combinedBytes[index + 1] : (byte)0;
                        byte b = index + 2 < combinedBytes.Length ? combinedBytes[index + 2] : (byte)0;
                        lock (bitmap)
                        {
                            bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }
                });
                bitmap.Save(filePath, ImageFormat.Png);
            }
        }
    }
}