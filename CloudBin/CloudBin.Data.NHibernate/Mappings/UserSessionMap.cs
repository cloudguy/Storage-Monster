using CloudBin.Core.Domain;
using CloudBin.Data.NHibernate.Types;
using FluentNHibernate.Mapping;

namespace CloudBin.Data.NHibernate.Mappings
{
    internal sealed class UserSessionMap : ClassMap<UserSession>
    {
        public UserSessionMap()
        {
            Table("user_sessions");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Token).Column("session_token").Not.Nullable().Length(32).UniqueKey("un_user_sessions_session_token");
            Map(x => x.IsPersistent).Column("is_persistent").Not.Nullable();
            Map(x => x.UserAgent).Column("user_agent").Not.Nullable().Length(200);
            Map(x => x.Expires).Column("expires").Not.Nullable().CustomType<UnixDateTimeOffsetType>();
            Map(x => x.SignedIn).Column("signed_in").Not.Nullable().CustomType<UnixDateTimeOffsetType>();
            Map(x => x.IPAddress).Column("ip_address").Not.Nullable().Length(100);
            References(x => x.User).Column("user_id").LazyLoad(Laziness.Proxy).Not.Nullable().ForeignKey("fk_user_sessions_users").Cascade.Delete().Cascade.SaveUpdate();
        }
    }
}
