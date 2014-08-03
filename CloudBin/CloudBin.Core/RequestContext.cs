using CloudBin.Core.Utilities;

namespace CloudBin.Core
{
    public static class RequestContext
    {
        private static IRequestContextProvider _contextProvider;

        public static IRequestContextProvider Current
        {
            get { return _contextProvider; }
        }

        public static void SetProvider(IRequestContextProvider contextProvider)
        {
            Verify.NotNull(()=>contextProvider);
            _contextProvider = contextProvider;
        }
    }
}
