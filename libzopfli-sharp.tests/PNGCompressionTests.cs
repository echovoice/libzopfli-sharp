using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;

namespace LibZopfliSharp.Tests
{
    // Test files thanks to Echovoice, Copyright 2013 Echovoice LLC

    [TestClass]
    public class PNGCompressionTests
    {
        [TestMethod]
        public void testPNGCompression()
        {
            // itterate over all test images
            string[] files = new string[] { "files/ev.png", "files/test01.png", "files/test02.png" };
            for (int i = 0; i < files.Length; i++)
			{
                // make sure compression works, file should be smaller
                byte[] uncompressed = File.ReadAllBytes(files[i]);
                int before = uncompressed.Length;
                byte[] compressed = ZopfliPNG.compress(uncompressed);
                int after = compressed.Length;
                before.Should().BeGreaterOrEqualTo(after);
            }
        }

        [TestMethod]
        public void testImageObjectPNGCompression()
        {
            // capture initial file size
            FileInfo fi = new FileInfo("files/ev.png");
            long before = fi.Length;

            // load file into image object
            Image testImage = Image.FromFile("files/ev.png");

            // compress image
            string tempFile = Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".png";
            testImage.SaveAsPNG(tempFile);

            // capture compressed file size
            FileInfo fi2 = new FileInfo(tempFile);
            long after = fi2.Length;

            // remove file
            File.Delete(tempFile);

            // verify
            before.Should().BeGreaterOrEqualTo(after);
        }

        [TestMethod]
        public void testPNGCompressionbyFilePath()
        {
            // copy a test file
            string tempFile = Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".png";
            File.Copy("files/ev.png", tempFile);

            // capture initial file size
            FileInfo fi = new FileInfo(tempFile);
            long before = fi.Length;

            // compress image
            ZopfliPNG.compress(tempFile);

            // capture compressed file size
            FileInfo fi2 = new FileInfo(tempFile);
            long after = fi2.Length;

            // remove file
            File.Delete(tempFile);

            // verify
            before.Should().BeGreaterOrEqualTo(after);
        }

        [TestMethod]
        public void testPNGStreamCompression()
        {
            // make sure compression works, file should be smaller
            byte[] uncompressed = File.ReadAllBytes("files/ev.png");
            int before = uncompressed.Length;
            byte[] compressed;
            int after = 0;

            // test png stream compression code
            using (MemoryStream compressStream = new MemoryStream())
            using (ZopfliPNGStream compressor = new ZopfliPNGStream(compressStream))
            {
                compressor.Write(uncompressed, 0, before);
                compressor.Close();
                compressed = compressStream.ToArray();
                after = compressed.Length;
            }

            before.Should().BeGreaterThan(after);
        }

        [TestMethod]
        public void testPNGEmpty()
        {
            Action action = () => { byte[] compressed = ZopfliPNG.compress(File.ReadAllBytes("files/empty.png")); };
            action.ShouldThrow<ZopfliPNGException>().WithMessage("empty input or file doesn't exist");
        }

        [TestMethod]
        public void testPNGCorrupt()
        {
            Action action = () => { byte[] compressed = ZopfliPNG.compress(File.ReadAllBytes("files/corrupt.png")); };
            action.ShouldThrow<ZopfliPNGException>().WithMessage("incorrect PNG signature, it's no PNG or corrupted");
        }

        [TestMethod]
        public void testPNGSmallHeader()
        {
            Action action = () => { byte[] compressed = ZopfliPNG.compress(File.ReadAllBytes("files/small-header.png")); };
            action.ShouldThrow<ZopfliPNGException>().WithMessage("PNG file is smaller than a PNG header");
        }
    }
}