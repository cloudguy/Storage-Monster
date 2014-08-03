using CloudBin.Core.Utilities;

namespace CloudBin.Core
{
    public static class DependencyContainer
    {
        public static IDependencyContainer Current { get; private set; }

        public static void Initialize(IDependencyContainer container)
        {
            Verify.NotNull(() => container);
            Current = container;
        }

        public static void ShutDown()
        {
            if (Current != null)
            {
                Current.Dispose();
            }
        }
    }
}
