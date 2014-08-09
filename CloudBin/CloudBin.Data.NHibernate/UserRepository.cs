using CloudBin.Core.Domain;

namespace CloudBin.Data.NHibernate
{
    public sealed class UserRepository : Repository<User, int>, IUserRepository
    {
    }
}
