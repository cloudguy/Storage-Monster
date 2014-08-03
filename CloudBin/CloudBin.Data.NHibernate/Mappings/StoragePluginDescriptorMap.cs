using CloudBin.Core.Domain;
using CloudBin.Data.NHibernate.Types;
using FluentNHibernate.Mapping;

namespace CloudBin.Data.NHibernate.Mappings
{
    internal sealed class StoragePluginDescriptorMap : ClassMap<StoragePluginDescriptor>
    {
        internal StoragePluginDescriptorMap()
        {
            Table("storage_plugin_descriptors");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.ClassPath).Column("classpath").Not.Nullable().Length(100).UniqueKey("un_storage_plugin_descriptors_classpath");
            Map(x => x.Status).Column("status").Not.Nullable().Default("0").CustomType<StoragePluginStatusType>();
        }
    }
}
