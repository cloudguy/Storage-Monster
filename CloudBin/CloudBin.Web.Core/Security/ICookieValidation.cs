using System;

namespace CloudBin.Web.Core.Security
{
    internal interface ICookieValidation : IDisposable
    {
        bool Validate(byte[] signedMessage);
        byte[] Sign(byte[] data);
        byte[] StripSignature(byte[] signedMessage);
    }
}
