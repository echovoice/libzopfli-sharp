using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibZopfliSharp.Native
{
    /// <summary>
    /// x86 Zopfli Compressor Hooks
    /// </summary>
    public class ZopfliCompressor32
    {
        /// <summary>
        /// Compresses according to the given output format and appends the result to the output.
        /// </summary>
        /// <param name="options">Zopfli program options</param>
        /// <param name="output_type">The output format to use</param>
        /// <param name="data">Pointer to the data</param>
        /// <param name="data_size">This is the size of the memory block pointed to by data</param>
        /// <param name="data_out">Pointer to the dynamic output array to which the result is appended</param>
        /// <param name="data_out_size">This is the size of the memory block pointed to by the dynamic output array size</param>
        [DllImport("x86\\zopfli.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZopfliCompress(ref ZopfliOptions options, ZopfliFormat output_type, byte[] data, int data_size, ref IntPtr data_out, ref UIntPtr data_out_size);

        /// <summary>
        /// Frees memory allocated by the native Zopfli library.
        /// </summary>
        /// <param name="mem">Pointer to the unmanaged memory to free</param>
        [DllImport("x86\\zopfli.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZopfliFree(IntPtr mem);
    }

    /// <summary>
    /// x64 Zopfli Compressor Hooks
    /// </summary>
    public class ZopfliCompressor64
    {
        /// <summary>
        /// Compresses according to the given output format and appends the result to the output.
        /// </summary>
        /// <param name="options">Zopfli program options</param>
        /// <param name="output_type">The output format to use</param>
        /// <param name="data">Pointer to the data</param>
        /// <param name="data_size">This is the size of the memory block pointed to by data</param>
        /// <param name="data_out">Pointer to the dynamic output array to which the result is appended</param>
        /// <param name="data_out_size">This is the size of the memory block pointed to by the dynamic output array size</param>
        [DllImport("amd64\\zopfli.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZopfliCompress(ref ZopfliOptions options, ZopfliFormat output_type, byte[] data, int data_size, ref IntPtr data_out, ref UIntPtr data_out_size);

        /// <summary>
        /// Frees memory allocated by the native Zopfli library.
        /// </summary>
        /// <param name="mem">Pointer to the unmanaged memory to free</param>
        [DllImport("amd64\\zopfli.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZopfliFree(IntPtr mem);
    }
}
