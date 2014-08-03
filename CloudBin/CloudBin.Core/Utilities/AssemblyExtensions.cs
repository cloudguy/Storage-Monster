using System;
using System.IO;
using System.Reflection;

namespace CloudBin.Core.Utilities
{
    public static class AssemblyExtensions
    {
        public static string Directory(this Assembly assembly)
        {
            Verify.NotNull(() => assembly);
            var codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
