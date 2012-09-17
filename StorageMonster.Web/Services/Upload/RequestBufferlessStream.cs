using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace StorageMonster.Web.Services.Upload
{
    public class RequestBufferlessStream : Stream
    {
        public static RequestBufferlessStream CreateStream(HttpWorkerRequest workerRequest)
        {
            if (workerRequest == null)
                throw new ArgumentNullException("workerRequest");

            long totalBytes = (long)workerRequest.GetTotalEntityBodyLength();
            return new RequestBufferlessStream(workerRequest, totalBytes);
        }

        private HttpWorkerRequest _workerRequest;
        private byte[] _currentBuffer = new byte[8192];
        private long _currentPosition;
        private int _currentBufferPosition;
        private bool _endReached;        
        private long _total;        
        private int _currentBufferLength;
        private long _received;

        private RequestBufferlessStream(HttpWorkerRequest workerRequest, long totalBytes)
        {
            _workerRequest = workerRequest;
            _total = totalBytes;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotSupportedException("Writing not supported");
        }

        public override long Length
        {
            get { return _total; }
        }

        public override long Position
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                throw new NotSupportedException("Seek not supported");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (offset < 0 || count < 0)
                throw new ArgumentException("Offset or count or both are less than zero");

            if (buffer.Length < offset + count)
                throw new OverflowException("Count and offset exceed buffer length");

            if (count == 0)
                return 0;

            if (_endReached)
                return 0;


            int totalRead = 0;
            if (_currentPosition == 0)
            {
                int preloaded = _workerRequest.GetPreloadedEntityBodyLength();                
                var preloadedBuffer = _workerRequest.GetPreloadedEntityBody();                
                if (preloadedBuffer == null) // IE normally does not preload                
                    preloaded = _workerRequest.ReadEntityBody(_currentBuffer, _currentBuffer.Length);           
                else                
                    Buffer.BlockCopy(preloadedBuffer, 0, _currentBuffer, 0, preloaded);               
                _currentBufferLength = preloaded;
                _received = preloaded;
                _currentBufferPosition = 0;                    
            }


            while (true)
            {                
                int toCopy = Math.Min(_currentBufferLength - _currentBufferPosition, count - totalRead);
                if (toCopy > 0)
                {
                    Buffer.BlockCopy(_currentBuffer, _currentBufferPosition, buffer, offset + totalRead, toCopy);
                    _currentBufferPosition += toCopy;
                    totalRead += toCopy;
                    _currentPosition += toCopy;
                }

                if (_currentPosition >= _total)
                {
                    _endReached = true;
                    return totalRead;
                }

                if (totalRead >= count)
                    return totalRead;              
                
                _currentBufferPosition = 0;

                int loaded = _workerRequest.ReadEntityBody(_currentBuffer, _currentBuffer.Length);
                if (loaded == 0)
                {
                    _endReached = true;
                    return totalRead;
                }
                _currentBufferLength = loaded;
                _received += loaded;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("Seek not supported");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Writing not supported");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Writing not supported");
        }
    }
}