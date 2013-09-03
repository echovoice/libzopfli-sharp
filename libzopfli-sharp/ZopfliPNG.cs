using LibZopfliSharp.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibZopfliSharp
{
    public static class ZopfliPNG
    {
        /// <summary>
        /// Save Image as PNG, compressed with Zopfli Algorithm
        /// </summary>
        /// <param name="image">Image object to compress</param>
        /// <param name="filename">File path to output the PNG image to</param>
        public static void SaveAsPNG(this Image image, string filename)
        {
            SaveAsPNG(image, filename, new ZopfliPNGOptions());
        }

        /// <summary>
        /// Save Image as PNG, compressed with Zopfli Algorithm
        /// </summary>
        /// <param name="image">Image object to compress</param>
        /// <param name="filename">File path to output the PNG image to</param>
        /// <param name="options">Zopfli PNG compression options</param>
        public static void SaveAsPNG(this Image image, string filename, ZopfliPNGOptions options)
        {
            string tempfile = Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".png";
            image.Save(tempfile, ImageFormat.Png);
            compress(tempfile, filename, options);

            if (File.Exists(tempfile))
                File.Delete(tempfile);
        }  

        /// <summary>
        /// Compress PNG file
        /// </summary>
        /// <param name="pathIn">Path to input PNG file</param>
        public static void compress(string pathIn)
        {
            compress(pathIn, new ZopfliPNGOptions());
        }

        /// <summary>
        /// Compress PNG file
        /// </summary>
        /// <param name="pathIn">Path to input PNG file</param>
        /// <param name="options">Zopfli PNG compression options</param>
        public static void compress(string pathIn, ZopfliPNGOptions options)
        {
            string tempfile = Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".png";
            compress(pathIn, tempfile, options);
            File.Replace(tempfile, pathIn, Path.GetTempPath() + Guid.NewGuid().ToString("N"));

            if (File.Exists(tempfile))
                File.Delete(tempfile);
        }

        /// <summary>
        /// Compress PNG file
        /// </summary>
        /// <param name="pathIn">Path to input PNG file</param>
        /// <param name="pathOut">Output path to compressed PNG file</param>
        public static void compress(string pathIn, string pathOut)
        {
            compress(pathIn, pathOut, new ZopfliPNGOptions());
        }

        /// <summary>
        /// Compress PNG file
        /// </summary>
        /// <param name="pathIn">Path to input PNG file</param>
        /// <param name="pathOut">Output path to compressed PNG file</param>
        /// <param name="options">Zopfli PNG compression options</param>
        public static void compress(string pathIn, string pathOut, ZopfliPNGOptions options)
        {
            // path in and out cannot be the same
            if (pathIn == pathOut)
                throw new Exception("Do not use the same path for input and output.");

            // make sure PNG input file exists
            if (!File.Exists(pathIn))
                throw new FileNotFoundException(pathIn + " does not exist.");

            // make sure output file does not exist
            if (File.Exists(pathOut))
                throw new Exception(pathOut + " already exists.");
            
            // load file into memory
            byte[] uncompressed = File.ReadAllBytes(pathIn);

            // test PNG stream compression code
            using (FileStream fileStream = new FileStream(pathOut, FileMode.Create, FileAccess.Write))
            using (MemoryStream compressStream = new MemoryStream())
            using (ZopfliPNGStream compressor = new ZopfliPNGStream(compressStream, options))
            {
                compressor.Write(uncompressed, 0, uncompressed.Length);
                compressor.Close();

                byte[] data = compressStream.ToArray();
                fileStream.Write(data, 0, data.Length);
                compressStream.Close();
            }
        }

        /// <summary>
        /// Internal convert method to convert byte array to compressed byte array
        /// </summary>
        /// <param name="data_in">Uncompressed data array</param>
        /// <returns></returns>
        public static byte[] compress(byte[] data_in)
        {
            return compress(data_in, new ZopfliPNGOptions());
        }

        /// <summary>
        /// Internal convert method to convert byte array to compressed byte array
        /// </summary>
        /// <param name="data_in">Uncompressed data array</param>
        /// <param name="options">Zopfli PNG compression options</param>
        /// <returns></returns>
        public static byte[] compress(byte[] data_in, ZopfliPNGOptions options)
        {
            IntPtr result = IntPtr.Zero;

            int error = 0;

            try
            {
                // Get image data length
                UIntPtr data_size = (UIntPtr)data_in.Length;

                // Compress the data via native methods
                if(Environment.Is64BitProcess)
                    error = ZopfliPNGCompressor64.ZopfliPNGExternalOptimize(data_in, data_in.Length, ref result);
                else
                    error = ZopfliPNGCompressor32.ZopfliPNGExternalOptimize(data_in, data_in.Length, ref result);

                // Copy data back to managed memory and return
                return NativeUtilities.GetDataFromUnmanagedMemory(result, error);
            }
            catch
            {
                if (result == IntPtr.Zero && error > 0)
                    throw new ZopfliPNGException(error);

                throw;
            }
            finally
            {
                // Free unmanaged memory
                Marshal.FreeHGlobal(result);
            }
        }
    }
}
