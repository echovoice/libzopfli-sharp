using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZopfliSharp
{
    public class ZopfliStream : Stream
    {
        private Stream _innerStream;
        private bool _CanWrite = true;
        private ZopfliFormat type;
        private ZopfliOptions options;
        private bool leaveOpen;

        public ZopfliStream(Stream inner)
        {
            _innerStream = inner;
            type = ZopfliFormat.ZOPFLI_FORMAT_DEFLATE;
            options = new ZopfliOptions();
            this.leaveOpen = false;
        }

        public ZopfliStream(Stream inner, ZopfliFormat type)
        {
            _innerStream = inner;
            this.type = type;
            options = new ZopfliOptions();
            this.leaveOpen = false;
        }

        public ZopfliStream(Stream inner, ZopfliFormat type, bool leaveOpen)
        {
            _innerStream = inner;
            this.type = type;
            options = new ZopfliOptions();
            this.leaveOpen = leaveOpen;
        }

        public ZopfliStream(Stream inner, ZopfliFormat type, ZopfliOptions options)
        {
            _innerStream = inner;
            this.type = type;
            this.options = options;
            this.leaveOpen = false;
        }

        public ZopfliStream(Stream inner, ZopfliFormat type, ZopfliOptions options, bool leaveOpen)
        {
            _innerStream = inner;
            this.type = type;
            this.options = options;
            this.leaveOpen = leaveOpen;
        }

        public override bool CanRead
        {
            get { return _innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _innerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _CanWrite; }
        }

        public override void Flush()
        {
            _innerStream.Flush();
        }

        public override long Length
        {
            get { return _innerStream.Length; }
        }

        public override long Position
        {
            get { return _innerStream.Position; }
            set { _innerStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _innerStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_CanWrite)
            {
                byte[] data = Zopfli.compress(buffer, type, options);
                _innerStream.Write(data, offset, data.Length);
                _CanWrite = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!leaveOpen)
                _innerStream.Dispose();
            options = null;
        }
    }
}