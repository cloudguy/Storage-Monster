using FluentNHibernate.Mapping;
using StorageMonster.Database.Nhibernate.Types;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Mappings
{
    public class StoragePluginMap : ClassMap<StoragePlugin>
    {
        public StoragePluginMap()
        {
            Table("storage_plugins");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.ClassPath).Column("classpath").Not.Nullable().Length(100).UniqueKey("un_classpath");
            Map(x => x.Status).Column("status").Not.Nullable().Default("0").CustomType<StoragePluginStatusType>();
            Version(x => x.Version).Column("version").Default(0).Not.Nullable();
        }
    }
}