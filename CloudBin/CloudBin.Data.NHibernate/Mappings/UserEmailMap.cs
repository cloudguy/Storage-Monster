using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudBin.Core.Domain;
using FluentNHibernate.Mapping;

namespace CloudBin.Data.NHibernate.Mappings
{
    internal sealed class UserEmailMap:ClassMap<UserEmail>
    {
        public UserEmailMap()
        {
            Table("users_emails");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Email).Column("email").Not.Nullable().Length(100).UniqueKey("un_users_emails_email");
            References(x => x.User).Column("user_id").LazyLoad(Laziness.Proxy).Not.Nullable().ForeignKey("fk_users_emails_users").Cascade.Delete().Cascade.SaveUpdate();
        }
    }
}