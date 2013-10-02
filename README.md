zopfli-sharp
============

A C# wrapper for the Zopfli Compression Algorithm

This library is available on Nuget as [libzopfli-sharp](https://www.nuget.org/packages/libzopfli-sharp).

What is Zopfli?
============================
Zopfli Compression Algorithm is a new zlib (gzip, deflate) compatible compressor. This compressor takes more time (~100x slower), but compresses around 5% better than zlib and better than any other zlib-compatible compressor we have found [1].

[1]: https://code.google.com/p/zopfli/

PNG Compression Usage
============================
If you are working with .Net Image objects simply call ```SaveAsPNG()``` Image extension method.

```csharp
Image testImage = Image.FromFile("files/ev.png");
testImage.SaveAsPNG(path_to_save_compressed_PNG);
```

You can compress *.PNG files directly using the ```ZopfliPNG.compress()``` method.

```csharp
string path = "files/ev.png";
ZopfliPNG.compress(path);
```

We also implemented a derived class of Stream called ```ZopfliPNGStream```

```csharp
byte[] uncompressed = File.ReadAllBytes("files/ev.png");
int before = uncompressed.Length;
byte[] compressed;
int after = 0;

using (MemoryStream compressStream = new MemoryStream())
using (ZopfliPNGStream compressor = new ZopfliPNGStream(compressStream))
{
    compressor.Write(uncompressed, 0, before);
    compressor.Close();
    compressed = compressStream.ToArray();
    after = compressed.Length;
}
```

In addition to using the default compression options Zopfli exposes some additional options to fine tune compression.
We extended this in the ```ZopfliPNGOptions``` object.

```csharp
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
}
```

Gzip, Deflate and Zlib Compression Usage
============================

For all 3 compression types we implemented a derived class of Stream ```ZopfliStream```

```csharp
byte[] uncompressed = File.ReadAllBytes("files/fp.log");
int before = uncompressed.Length;
byte[] compressed;
int after = 0;

using (MemoryStream compressStream = new MemoryStream())
using (ZopfliStream compressor = new ZopfliStream(compressStream, ZopfliFormat.ZOPFLI_FORMAT_DEFLATE))
{
    compressor.Write(uncompressed, 0, before);
    compressor.Close();
    compressed = compressStream.ToArray();
    after = compressed.Length;
}
```

The second parameter for our derived Stream class is the type of compression to use. 

```csharp
public enum ZopfliFormat
{
    ZOPFLI_FORMAT_GZIP,
    ZOPFLI_FORMAT_ZLIB,
    ZOPFLI_FORMAT_DEFLATE
};
```

In addition to using the default options Zopfli exposes some additional options used to fine tune compression.
We extended this in the ```ZopfliOptions``` object which can also be passed into the Stream.

```csharp
public class ZopfliOptions
{
    // Whether to print output
    public Int32 verbose;
        
    // Whether to print more detailed output
    public Int32 verbose_more;

    // Maximum amount of times to rerun forward and backward pass to optimize LZ77
    // compression cost. Good values: 10, 15 for small files, 5 for files over
    // several MB in size or it will be too slow.
    public Int32 numiterations;

    // If true, splits the data in multiple deflate blocks with optimal choice
    // for the block boundaries. Block splitting gives better compression. Default:
    // true (1).
    public Int32 blocksplitting;

    // If true, chooses the optimal block split points only after doing the iterative
    // LZ77 compression. If false, chooses the block split points first, then does
    // iterative LZ77 on each individual block. Depending on the file, either first
    // or last gives the best compression. Default: false (0).
    public Int32 blocksplittinglast;

    // Maximum amount of blocks to split into (0 for unlimited, but this can give
    // extreme results that hurt compression on some files). Default value: 15.
    public Int32 blocksplittingmax;
}
```
