using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace CloudBin.Core.Utilities
{
    public static class StringExtensions
    {
        public static byte[] GetByteArrayFromHexString(this string s)
        {
            return SoapHexBinary.Parse(s).Value;
        }
    }
}
