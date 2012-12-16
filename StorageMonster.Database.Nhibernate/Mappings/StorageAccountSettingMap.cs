using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Mappings
{
    public class StorageAccountSettingMap : ClassMap<StorageAccountSetting>
    {
        public StorageAccountSettingMap()
        {
            Table("storage_accounts_settings");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.SettingName).Column("setting_name").Not.Nullable().Length(45).UniqueKey("un_saccountid_settingname");
            Map(x => x.SettingValue).Column("setting_value").Not.Nullable().Length(300);
            References(x => x.StorageAccount).Column("storage_account_id").LazyLoad(Laziness.Proxy).Not.Nullable().Cascade.All().UniqueKey("un_saccountid_settingname");
        }
    }
}
