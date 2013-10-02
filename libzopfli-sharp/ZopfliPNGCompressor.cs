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
    /// x86 Native Hooks
    /// </summary>
    public class ZopfliPNGCompressor32
    {
        /// <summary>
        /// Library to recompress and optimize PNG images. Uses Zopfli as the compression backend, chooses optimal PNG color model, and tries out several PNG filter strategies.
        /// </summary>
        /// <param name="datain">Binary array to the PNG data</param>
        /// <param name="datainsize">Size of binary data in.</param>
        /// <param name="dataout">Binary array to which the result is appended</param>
        /// <returns>Returns data size on success, error code otherwise.</returns>
        [DllImport("x86\\zopfli.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ZopfliPNGExternalOptimize(byte[] datain, int datainsize, ref IntPtr dataout);
    }

    /// <summary>
    /// x64 Native Hooks
    /// </summary>
    public class ZopfliPNGCompressor64
    {
        /// <summary>
        /// Library to recompress and optimize PNG images. Uses Zopfli as the compression backend, chooses optimal PNG color model, and tries out several PNG filter strategies.
        /// </summary>
        /// <param name="datain">Binary array to the PNG data</param>
        /// <param name="datainsize">Size of binary data in.</param>
        /// <param name="dataout">Binary array to which the result is appended</param>
        /// <returns>Returns data size on success, error code otherwise.</returns>
        [DllImport("amd64\\zopfli.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ZopfliPNGExternalOptimize(byte[] datain, int datainsize, ref IntPtr dataout);
    }
}
