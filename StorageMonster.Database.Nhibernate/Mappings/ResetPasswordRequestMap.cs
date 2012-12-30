using FluentNHibernate.Mapping;
using StorageMonster.Database.Nhibernate.Types;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Mappings
{
    public class ResetPasswordRequestMap : ClassMap<ResetPasswordRequest>
    {
        public ResetPasswordRequestMap()
        {
            Table("reset_password_requests");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Token).Column("token").Not.Nullable().Length(100).Unique();
            Map(x => x.Expires).Column("expires").Not.Nullable().CustomType<UnixDateTimeOffset>();
            References(x => x.User).Column("user_id").LazyLoad(Laziness.Proxy).Not.Nullable().Cascade.All();
        }
    }
}
