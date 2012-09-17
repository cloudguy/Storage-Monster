using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StorageMonster.Services.Facade
{
    public class MonsterDownloadStream : MonsterStream
    {
        private Stream _underlyingStream;

        public static MonsterDownloadStream Create(Stream inputStream)
        {
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");

            return new MonsterDownloadStream(inputStream);
        }

        private MonsterDownloadStream(Stream inputStream)
        {
            _underlyingStream = inputStream;
        }

        public override bool CanRead
        {
            get { return _underlyingStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _underlyingStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _underlyingStream.CanWrite; }
        }

        public override void Flush()
        {
            _underlyingStream.Flush();
        }

        public override long Length
        {
            get { return _underlyingStream.Length; }
        }

        public override long Position
        {
            get
            {
                return _underlyingStream.Position;
            }
            set
            {
                _underlyingStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _underlyingStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _underlyingStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _underlyingStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _underlyingStream.Write(buffer, offset, count);
        }
    }
}
