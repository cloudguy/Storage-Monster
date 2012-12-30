using FluentNHibernate.Mapping;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Mappings
{
    public class StorageAccountMap : ClassMap<StorageAccount>
    {
        public StorageAccountMap()
        {
            Table("storage_accounts");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.AccountName).Column("account_name").Not.Nullable().Length(100).UniqueKey("un_saccountlogin_spluginid");
            References(x => x.User).Column("user_id").LazyLoad(Laziness.Proxy).Not.Nullable().Cascade.All().UniqueKey("un_saccountlogin_spluginid");
            References(x => x.StoragePlugin).Column("storage_plugin_id").Not.Nullable().LazyLoad(Laziness.Proxy).Cascade.SaveUpdate().UniqueKey("un_saccountlogin_spluginid");
            HasMany(x => x.Settings).LazyLoad().Inverse().KeyColumn("storage_account_id");
            Version(x => x.Version).Column("version").Default(0).Generated.Always().Not.Nullable();
        }
    }
}