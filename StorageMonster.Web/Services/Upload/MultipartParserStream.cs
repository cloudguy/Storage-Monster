using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace StorageMonster.Web.Services.Upload
{
    public class MultipartParserStream : Stream
    {
        private Stream _requestStream;
        private long _currentPosition;
        private const int _maxPreccedingBytesBeforeFile = 8196;
        private byte[] _currentBuffer = new byte[_maxPreccedingBytesBeforeFile];
        private byte[] _nextBuffer = new byte[_maxPreccedingBytesBeforeFile];
        private int _currentBufferLength;
        private int _nextBufferLength;
        private byte[] _delimiterBytes;
        private bool _endReached;
        private int _currentBufferPosition;
        private bool _startFileFound;
        private Encoding _encoding;
        private long _total;
        private long _received;
        private byte[] _endFileMarkerBytes;
        private bool _endIndexFound;

        public string ContentType
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            private set;
        }

        public static MultipartParserStream CreateStream(Stream requestStream, Encoding encoding, byte[] delimiterBytes)
        {
            if (requestStream == null)
                throw new ArgumentNullException("requestStream");

            if (delimiterBytes == null)
                throw new ArgumentNullException("delimiterBytes");

            if (encoding == null)
                throw new ArgumentNullException("encoding");

            return new MultipartParserStream(requestStream, encoding, delimiterBytes, requestStream.Length);
        }

        private MultipartParserStream(Stream requestStream, Encoding encoding, byte[] delimiterBytes, long totalStreamLength)
        {
            _requestStream = requestStream;
            _encoding = encoding;
            _delimiterBytes = delimiterBytes;
            _total = totalStreamLength;

            List<byte> tmpList = new List<byte>();
            tmpList.AddRange(encoding.GetBytes("\r\n"));
            tmpList.AddRange(delimiterBytes);

            _endFileMarkerBytes = tmpList.ToArray();
        }

        public void ParseFileNameAndContentType()
        {
            Read(_currentBuffer, 0, 0);//file name and content type would be parsed and filled to properties of current instance
        }

        private int SearchForFileStart()
        {
            _currentBufferLength = _requestStream.Read(_currentBuffer, 0, _currentBuffer.Length);

            // Copy to a string for header parsing
            string content = _encoding.GetString(_currentBuffer);

            // Look for Content-Type
            Regex re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
            Match contentTypeMatch = re.Match(content);

            // Look for filename
            re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
            Match filenameMatch = re.Match(content);

            // Did we find the required values?
            if (contentTypeMatch.Success && filenameMatch.Success)
            {
                // Set properties
                this.ContentType = contentTypeMatch.Value.Trim();
                this.Filename = Path.GetFileName(filenameMatch.Value.Trim()); //IE returns full path

                // Get the start index of the file contents
                int startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                if (startIndex > _requestStream.Length)
                    return -1;

                return startIndex;
            }
            return -1;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (offset < 0 || count < 0)
                throw new ArgumentException("Offset or count or both are less than zero");

            if (buffer.Length < offset + count)
                throw new OverflowException("Count and offset exceed buffer length");           

            if (_endReached)
                return 0;

            if (!_startFileFound)
            {
                _currentBufferPosition = SearchForFileStart();
                if (_currentBufferPosition < 0)
                {
#warning throw invalid request
                    _endReached = true;
                    return 0;
                }
                _startFileFound = true;
            }

            if (count == 0)
                return 0;

            int totalRead = 0;
            while (true)
            {
               if (!_endIndexFound)
                {
                    int endIndex = GetFileEndIndex();
                    if (endIndex > 0)
                    {
                        _currentBufferLength = endIndex;
                        _endIndexFound = true;
                    }
                }
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

                if (_endIndexFound)
                {
                    _endReached = true;
                    return totalRead;
                }

                _currentBufferPosition = 0;

                int loaded;
                if (_nextBufferLength > 0)
                {
                    Buffer.BlockCopy(_nextBuffer, 0, _currentBuffer, 0, _nextBufferLength);                    
                    loaded = _nextBufferLength;
                    _nextBufferLength = 0;
                    continue;
                }
                else
                {
                    loaded = _requestStream.Read(_currentBuffer, 0, _currentBuffer.Length);
                    if (loaded == 0)
                    {
                        _endReached = true;
                        return totalRead;
                    }
                }
                _currentBufferLength = loaded;
                _received += loaded;
            }
        }

        private static bool CheckArrayEqueality(byte[] src, byte[] check, int srcStartIndex, int checkStartIndex ,int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (src[srcStartIndex + i] != check[checkStartIndex + i])
                    return false;
            }
            return true;
        }

        private int GetFileEndIndex()
        {
            int startPos = _currentBufferPosition;
            while (true)
            {
                if (startPos >= _currentBuffer.Length)
                    return -1;
                startPos = Array.IndexOf(_currentBuffer, _endFileMarkerBytes[0], startPos);
                if (startPos < 0)
                    return -1;

                int checkLength = Math.Min(_currentBufferLength - startPos, _endFileMarkerBytes.Length);

                if (!CheckArrayEqueality(_currentBuffer, _endFileMarkerBytes, startPos, 0, checkLength))
                {
                    startPos++;
                    continue;
                }                    

                if (checkLength >= _endFileMarkerBytes.Length)
                    return startPos;

                if (_nextBufferLength <= 0)
                {
                    _nextBufferLength = _requestStream.Read(_nextBuffer, 0, _nextBuffer.Length);
                }

                if (CheckArrayEqueality(_nextBuffer, _endFileMarkerBytes, 0, checkLength, Math.Min(_nextBufferLength, _endFileMarkerBytes.Length - checkLength)))
                    return startPos;
                else
                    return -1;
            }
        }


        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf<byte>(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }

            return -1;
        }
        public override bool CanRead
        {
            get { return _requestStream.CanRead; }
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
            get { throw new NotSupportedException("Length query not supported"); }
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