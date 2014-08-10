using System;

namespace CloudBin.Web.Core.Configuration
{
    public interface IAuthenticationConfiguration
    {
        string CookieName { get; }
        string SignInUrl { get; }
        string EncryptionAlgorithm { get; }
        string ValidationAlgorithm { get; }
        byte[] EncryptionKey { get; }
        byte[] ValidationKey { get; }
        bool SlideExpire { get; }
        bool AllowMultipleSessions { get; }
// ReSharper disable InconsistentNaming
        bool RequireSSL { get; }
// ReSharper restore InconsistentNaming
        bool DoNotAuthenticateScriptAndContent { get; }
        TimeSpan SessionTimeout { get; }
    }
}
