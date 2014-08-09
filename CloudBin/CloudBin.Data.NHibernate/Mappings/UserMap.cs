using CloudBin.Core.Domain;
using FluentNHibernate.Mapping;

namespace CloudBin.Data.NHibernate.Mappings
{
    internal sealed class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("users");
            Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Name).Column("name").Not.Nullable().Length(100);
            HasMany(x => x.Emails).LazyLoad().Inverse().KeyColumn("user_id");
            Map(x => x.Password).Column("password").Not.Nullable().Length(200);
            Map(x => x.Locale).Column("locale").Not.Nullable().Length(10).Default("en-US");
            Map(x => x.TimeZone).Column("timezone").Not.Nullable().Default("0");
            if (ConfigurationContext.Current.Configuration.UseOptimisticLockForUsers)
            {
                Version(x => x.Version).Column("version").Default(0).Not.Nullable();
            }
        }
    }
}
