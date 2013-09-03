using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using FluentAssertions;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Reflection;
using Ionic.Zlib;
using System.Threading;

namespace LibZopfliSharp.Tests
{
    // Test files thanks to http://www.maximumcompression.com/data/files/

    [TestFixture]
    public class IntegrationTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void testDeflateStream()
        {
            // make sure compression works, file should be smaller
            byte[] uncompressed = File.ReadAllBytes("files/fp.log");
            int before = uncompressed.Length;
            byte[] compressed;
            int after = 0;

            // test deflate stream compression code
            using (MemoryStream compressStream = new MemoryStream())
            using (ZopfliStream compressor = new ZopfliStream(compressStream, ZopfliFormat.ZOPFLI_FORMAT_DEFLATE))
            {
                compressor.Write(uncompressed, 0, before);
                compressor.Close();
                compressed = compressStream.ToArray();
                after = compressed.Length;
            }

            before.Should().BeGreaterThan(after);

            // make sure we can decompress the file using built-in .net
            byte[] decompressedBytes = new byte[before];
            using (System.IO.Compression.DeflateStream decompressionStream = new System.IO.Compression.DeflateStream(new MemoryStream(compressed), System.IO.Compression.CompressionMode.Decompress))
            {
                decompressionStream.Read(decompressedBytes, 0, before);
            }
            uncompressed.Should().Equal(decompressedBytes);

            // use built-in .net compression and make sure zopfil is smaller
            int after_builtin = 0;
            using (MemoryStream compressStream = new MemoryStream())
            using (System.IO.Compression.DeflateStream compressor = new System.IO.Compression.DeflateStream(compressStream, System.IO.Compression.CompressionLevel.Optimal))
            {
                compressor.Write(uncompressed, 0, before);
                compressor.Close();
                after_builtin = compressStream.ToArray().Length;
            }

            after_builtin.Should().BeGreaterThan(after);
        }

        [Test]
        public void testGzipStream()
        {
            // make sure compression works, file should be smaller
            byte[] uncompressed = File.ReadAllBytes("files/fp.log");
            int before = uncompressed.Length;
            byte[] compressed;
            int after = 0;

            // test gzip stream compression code
            using (MemoryStream compressStream = new MemoryStream())
            using (ZopfliStream compressor = new ZopfliStream(compressStream, ZopfliFormat.ZOPFLI_FORMAT_GZIP))
            {
                compressor.Write(uncompressed, 0, before);
                compressor.Close();
                compressed = compressStream.ToArray();
                after = compressed.Length;
            }

            before.Should().BeGreaterThan(after);

            // make sure we can decompress the file using built-in .net
            byte[] decompressedBytes = new byte[before];
            using (MemoryStream tempStream = new MemoryStream(compressed))
            using (System.IO.Compression.GZipStream decompressionStream = new System.IO.Compression.GZipStream(tempStream, System.IO.Compression.CompressionMode.Decompress))
            {
                decompressionStream.Read(decompressedBytes, 0, before);
            }
            uncompressed.Should().Equal(decompressedBytes);

            // use built-in .net compression and make sure zopfil is smaller
            int after_builtin = 0;
            using (MemoryStream compressStream = new MemoryStream())
            using (System.IO.Compression.GZipStream compressor = new System.IO.Compression.GZipStream(compressStream, System.IO.Compression.CompressionLevel.Optimal))
            {
                compressor.Write(uncompressed, 0, before);
                compressor.Close();
                after_builtin = compressStream.ToArray().Length;
            }

            after_builtin.Should().BeGreaterThan(after);
        }

        [Test]
        public void testZlibStream()
        {
            // make sure compression works, file should be smaller
            byte[] uncompressed = File.ReadAllBytes("files/fp.log");
            int before = uncompressed.Length;
            byte[] compressed;
            int after = 0;

            // test zlib stream compression code
            using (MemoryStream compressStream = new MemoryStream())
            using (ZopfliStream compressor = new ZopfliStream(compressStream, ZopfliFormat.ZOPFLI_FORMAT_ZLIB))
            {
                compressor.Write(uncompressed, 0, before);
                compressor.Close();
                compressed = compressStream.ToArray();
                after = compressed.Length;
            }

            before.Should().BeGreaterThan(after);

            // make sure we can decompress the file using built-in .net
            byte[] decompressedBytes = new byte[before];
            using (ZlibStream decompressionStream = new ZlibStream(new MemoryStream(compressed), Ionic.Zlib.CompressionMode.Decompress))
            {
                decompressionStream.Read(decompressedBytes, 0, before);
            }
            uncompressed.Should().Equal(decompressedBytes);

            // use built-in .net compression and make sure zopfil is smaller
            int after_builtin = 0;
            using (MemoryStream compressStream = new MemoryStream())
            using (ZlibStream compressor = new ZlibStream(compressStream, Ionic.Zlib.CompressionMode.Compress))
            {
                compressor.Write(uncompressed, 0, before);
                compressor.Close();
                after_builtin = compressStream.ToArray().Length;
            }

            after_builtin.Should().BeGreaterThan(after);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}