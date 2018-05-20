using LibZopfliSharp.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibZopfliSharp
{
    /// <summary>
    /// Zopfli Compression Algorithm is a new zlib (gzip, deflate) compatible compressor. This compressor takes more time (~100x slower), but compresses around 5% better than zlib and better than any other zlib-compatible compressor we have found.
    /// </summary>
    public static class Zopfli
    {
        /// <summary>
        /// Internal convert method to convert byte array to compressed byte array
        /// </summary>
        /// <param name="data_in">Uncompressed data array</param>
        /// <returns>Compressed data array</returns>
        public static byte[] compress(byte[] data_in)
        {
            return compress(data_in, ZopfliFormat.ZOPFLI_FORMAT_DEFLATE, new ZopfliOptions());
        }

        /// <summary>
        /// Internal convert method to convert byte array to compressed byte array
        /// </summary>
        /// <param name="data_in">Uncompressed data array</param>
        /// <param name="type">Format type, DEFLATE, GZIP, ZLIB</param>
        /// <returns>Compressed data array</returns>
        public static byte[] compress(byte[] data_in, ZopfliFormat type)
        {
            return compress(data_in, type, new ZopfliOptions());
        }
        
        /// <summary>
        /// Internal convert method to convert byte array to compressed byte array
        /// </summary>
        /// <param name="data_in">Uncompressed data array</param>
        /// <param name="type">Format type, DEFLATE, GZIP, ZLIB</param>
        /// <param name="options">Compression options</param>
        /// <returns>Compressed data array</returns>
        public static byte[] compress(byte[] data_in, ZopfliFormat type, ZopfliOptions options)
        {
            IntPtr result = IntPtr.Zero;
            UIntPtr result_size = UIntPtr.Zero;

            try
            {
                // Get image data length
                UIntPtr data_size = (UIntPtr)data_in.Length;

                // Compress the data via native methods
                if (Environment.Is64BitProcess)
                    ZopfliCompressor64.ZopfliCompress(ref options, type, data_in, data_in.Length, ref result, ref result_size);
                else
                    ZopfliCompressor32.ZopfliCompress(ref options, type, data_in, data_in.Length, ref result, ref result_size);

                // Copy data back to managed memory and return
                return NativeUtilities.GetDataFromUnmanagedMemory(result, (int)result_size);
            }
            catch
            {
                throw;
            }
            finally
            {
                // Free unmanaged memory
                if (Environment.Is64BitProcess)
                    ZopfliCompressor64.ZopfliFree(result);
                else
                    ZopfliCompressor32.ZopfliFree(result);
            }
        }
    }
}
