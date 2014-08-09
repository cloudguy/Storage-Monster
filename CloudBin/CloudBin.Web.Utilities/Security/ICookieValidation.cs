using System;

namespace CloudBin.Web.Utilities.Security
{
    internal interface ICookieValidation : IDisposable
    {
        bool Validate(byte[] signedMessage);
        byte[] Sign(byte[] data);
        byte[] StripSignature(byte[] signedMessage);
    }
}
