using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("users");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Name).Column("name").Not.Nullable().Length(100);
            Map(x => x.Email).Column("email").Not.Nullable().Length(100).Unique();
            Map(x => x.Password).Column("password").Not.Nullable().Length(200);
            Map(x => x.Locale).Column("locale").Not.Nullable().Length(10).Default("en");
            Map(x => x.TimeZone).Column("timezone").Not.Nullable().Default("0");

           
            //Version()
        }

      
    }


}
