using FluentNHibernate.Mapping;
using StorageMonster.Database.Nhibernate.Types;
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
            Map(x => x.Email).Column("email").Not.Nullable().Length(100).UniqueKey("un_email");
            Map(x => x.Password).Column("password").Not.Nullable().Length(200);
            Map(x => x.Locale).Column("locale").Not.Nullable().Length(10).Default("'en'");
            Map(x => x.TimeZone).Column("timezone").Not.Nullable().Default("0");
            Map(x => x.UserRole).Column("role").Not.Nullable().Default("0").CustomType<UserRoleType>();
            HasMany(x => x.StorageAccounts).LazyLoad().Inverse().KeyColumn("user_id");
            Version(x => x.Version).Column("version").Default(0).Not.Nullable();
        }
    }
}
