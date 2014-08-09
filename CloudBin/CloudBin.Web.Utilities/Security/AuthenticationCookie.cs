using System;
using System.IO;

namespace CloudBin.Web.Utilities.Security
{
    public sealed class AuthenticationCookie
    {
        private readonly Guid _sessionId;
        private readonly byte[] _tag;

        public Guid SessionId { get { return _sessionId; } }

        public AuthenticationCookie(Guid sessionId, byte[] tag = null)
        {
            _sessionId = sessionId;
            _tag = tag;
        }

        private AuthenticationCookie(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    _sessionId = new Guid(binaryReader.ReadBytes(16));
                    var tagLength = binaryReader.ReadInt16();
                    _tag = tagLength <= 0 ? null : _tag = binaryReader.ReadBytes(tagLength);
                }
            }
        }

        public static AuthenticationCookie Deserialize(byte[] data)
        {
            return new AuthenticationCookie(data);
        }

        public byte[] Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(_sessionId.ToByteArray());
                    if (_tag == null)
                    {
                        binaryWriter.Write((short)0);
                    }
                    else
                    {
                        binaryWriter.Write((short)_tag.Length);
                        binaryWriter.Write(_tag);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
