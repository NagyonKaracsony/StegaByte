using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
namespace StegaByte
{
    /// <summary>
    /// Holds utility functionality for the <seealso cref="StegaByte.Encoder"/> and <seealso cref="StegaByte.Decoder"/> classes.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Represents data size units for storage.
        /// </summary>
        public enum DataSizeUnit
        {
            /// <summary>1 byte.</summary>
            B,
            /// <summary>1024 bytes (1 kilobyte).</summary>
            KB,
            /// <summary>1024 kilobytes (1 megabyte).</summary>
            MB,
            /// <summary>1024 megabytes (1 gigabyte).</summary>
            GB,
            /// <summary>1024 gigabytes (1 terabyte).</summary>
            TB,
        }
        /// <summary>
        /// Calculates the byte size of <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object to get the size of</param>
        /// <param name="precision">The number of digit precision to round the byte size before it is returned.</param>
        /// <param name="TargetUnit">The unit of storage to convert the size of <paramref name="obj"/> to.</param>
        /// <returns>The number of bytes <paramref name="obj"/> takes up, or returns the size as converted to <paramref name="TargetUnit"/> data unit.</returns>
        public static float GetObjectSize(object obj, DataSizeUnit TargetUnit, int precision)
        {
            int devider;
            switch (TargetUnit)
            {
                case DataSizeUnit.B:
                    devider = 1;
                    break;
                case DataSizeUnit.KB:
                    devider = 1024;
                    break;
                case DataSizeUnit.MB:
                    devider = 1024 * 1024;
                    break;
                case DataSizeUnit.GB:
                    devider = 1024 * 1024 * 1024;
                    break;
                case DataSizeUnit.TB:
                    devider = 1024 * 1024 * 1024;
                    break;
                default:
                    devider = 1;
                    break;
            }

            // Not sure if this is foolproof enough.
            if (precision > 15) precision = 15;
            else if (precision < 0) precision = 0;

            string json = JsonSerializer.Serialize(obj);
            if (precision == 0 && devider != 0) return (float)Encoding.UTF8.GetBytes(json).Length / devider;
            else return (float)Math.Round((double)Encoding.UTF8.GetBytes(json).Length / devider, precision);
        }
        /// <summary>
        /// Generates the largest and least uncompressible string that StegaByte can Encode/Decode without pixel-color repetition (all rgb values possible)
        /// </summary>
        /// <remarks>
        /// Used for StegaByte debugging and performance testing purposes only.
        /// </remarks>
        /// <returns>A string type variable containing all the possible chars that can be made with rgb values.</returns>
        public static string GenerateAllCharacters()
        {
            StringBuilder sb = new StringBuilder();

            for (int r = 0; r < 256; r++)
            {
                for (int g = 0; g < 256; g++)
                {
                    for (int b = 0; b < 256; b++)
                    {
                        sb.Append((char)r);
                        sb.Append((char)g);
                        sb.Append((char)b);
                        sb.Append((char)255);  // Alpha always full
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Performance profiles an object with StegaByte's <seealso cref="Encoder"/> and <seealso cref="Decoder"/> using the <seealso cref="System.Diagnostics.Stopwatch"/> class.
        /// </summary>
        /// <remarks>
        /// Used for StegaByte debugging and performance testing purposes only.
        /// </remarks>
        /// <returns>A <see cref="StegaByteProfile"/> type object containing the results of the performance profiling</returns>
        public static StegaByteProfile Profile(object objToProfile)
        {
            Stopwatch mainStopwatch = new Stopwatch();
            Stopwatch stopwatch = new Stopwatch();
            mainStopwatch.Start();
            StegaByteProfile profile = new StegaByteProfile();
            string tempFilePath = Path.Combine(Path.GetTempPath(), "StegabyteProfiler.png");

            stopwatch.Start();
            Encoder.Encode(objToProfile, tempFilePath);
            stopwatch.Stop();
            profile.EncodingTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var readObj = Decoder.Decode(tempFilePath);
            stopwatch.Stop();
            profile.DecodingTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            Type targetType = objToProfile.GetType();
            object castedValue = Convert.ChangeType(readObj, targetType);
            stopwatch.Stop();

            mainStopwatch.Stop();
            profile.TotalTime = mainStopwatch.ElapsedMilliseconds;
            return profile;
        }
    }
    /// <summary>
    /// Holds performance results for the <seealso cref="Utility.Profile"/> method
    /// </summary>
    /// <remarks>
    /// Used for StegaByte debugging and performance testing purposes only.
    /// </remarks>
    public class StegaByteProfile
    {
        /// <summary>Returns the time it took to encode the obj in milliseconds.</summary>
        public long EncodingTime { get; set; }
        /// <summary>Returns the time it took to decode the obj in milliseconds.</summary>
        public long DecodingTime { get; set; }
        /// <summary>Returns the time it took to cast the obj to it's original type in milliseconds.</summary>
        public long CastingTime { get; set; }
        /// <summary>Returns the time it took to delete the image generated by the encoder in milliseconds.</summary>
        public long CleanupTime { get; set; }
        /// <summary>Returns the time it took in total to encode, decode, cast, and cleanup the image the encoder created for obj in milliseconds.</summary>
        public long TotalTime { get; set; }
    }
}