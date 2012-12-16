using FluentNHibernate.Mapping;
using StorageMonster.Database.Nhibernate.Types;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Mappings
{
    public class StoragePluginDescriptorMap : ClassMap<StoragePluginDescriptor>
    {
        public StoragePluginDescriptorMap()
        {
            Table("storage_plugins");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.ClassPath).Column("classpath").Not.Nullable().Length(100).UniqueKey("un_classpath");
            Map(x => x.Status).Column("status").Not.Nullable().Default("0").CustomType<StoragePluginStatusType>();
        }
    }
}