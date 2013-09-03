using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibZopfliSharp
{
    public enum ZopfliPNGFilterStrategy
    {
        kStrategyZero = 0,
        kStrategyOne = 1,
        kStrategyTwo = 2,
        kStrategyThree = 3,
        kStrategyFour = 4,
        kStrategyMinSum,
        kStrategyEntropy,
        kStrategyPredefined,
        kStrategyBruteForce,
        kNumFilterStrategies // Not a strategy but used for the size of this enum
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ZopfliPNGOptions
    {
        // Allow altering hidden colors of fully transparent pixels
        public Boolean lossy_transparent;

        // Convert 16-bit per channel images to 8-bit per channel
        public Boolean lossy_8bit;

        // Filter strategies to try
        public ZopfliPNGFilterStrategy[] filter_strategies;

        // Automatically choose filter strategy using less good compression
        public Boolean auto_filter_strategy;

        // PNG chunks to keep
        // chunks to literally copy over from the original PNG to the resulting one
        public String[] keepchunks;

        // Use Zopfli deflate compression
        public Boolean use_zopfli;

        // Zopfli number of iterations
        public Int32 num_iterations;

        // Zopfli number of iterations on large images
        public Int32 num_iterations_large;

        // 0=none, 1=first, 2=last, 3=both
        public Int32 block_split_strategy;

        /// <summary>
        /// Initializes options used throughout the program with default values.
        /// </summary>
        public ZopfliPNGOptions()
        {
            lossy_transparent = false;
            lossy_8bit = false;
            filter_strategies = new ZopfliPNGFilterStrategy[0];
            auto_filter_strategy = true;
            keepchunks = new String[0];
            use_zopfli = true;
            num_iterations = 15;
            num_iterations_large = 5;
            block_split_strategy = 1;
        }
    }
}
