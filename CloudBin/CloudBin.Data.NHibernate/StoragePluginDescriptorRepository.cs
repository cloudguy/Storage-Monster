using CloudBin.Core.Domain;

namespace CloudBin.Data.NHibernate
{
    public sealed class StoragePluginDescriptorRepository : Repository<StoragePluginDescriptor, int>, IStoragePluginDescriptorRepository
    {
    }
}
