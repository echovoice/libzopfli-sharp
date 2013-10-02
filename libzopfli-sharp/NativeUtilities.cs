using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibZopfliSharp
{
    /// <summary>
    /// Access to native lib from c#
    /// </summary>
    public static class NativeUtilities
    {
        /// <summary>
        /// Copy data from managed to unmanaged memory
        /// </summary>
        /// <param name="data">The data you want to copy</param>
        /// <returns>Pointer to the location of the unmanaged data</returns>
        public static IntPtr CopyDataToUnmanagedMemory(byte[] data)
        {
            // Initialize unmanged memory to hold the array
            int size = Marshal.SizeOf(data[0]) * data.Length;
            IntPtr pnt = Marshal.AllocHGlobal(size);
            // Copy the array to unmanaged memory
            Marshal.Copy(data, 0, pnt, data.Length);
            return pnt;
        }

        /// <summary>
        /// Get data from unmanaged memory back to managed memory
        /// </summary>
        /// <param name="source">A Pointer where the data lifes</param>
        /// <param name="length">How many bytes you want to copy</param>
        /// <returns></returns>
        public static byte[] GetDataFromUnmanagedMemory(IntPtr source, int length)
        {
            // Initialize managed memory to hold the array
            byte[] data = new byte[length];
            // Copy the array back to managed memory
            Marshal.Copy(source, data, 0, length);
            return data;
        }
    }
}
