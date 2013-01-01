using FluentNHibernate.Mapping;
using StorageMonster.Database.Nhibernate.Types;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Mappings
{
    public class SessionMap : ClassMap<Session>
    {
        public SessionMap()
        {
            Table("sessions");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Token).Column("session_token").Not.Nullable().Length(32).UniqueKey("un_session_token");
            Map(x => x.Expires).Column("expires").Not.Nullable().CustomType<UnixDateTimeOffset>();
            References(x => x.User).Column("user_id").LazyLoad(Laziness.Proxy).Not.Nullable();
        }
    }
}
