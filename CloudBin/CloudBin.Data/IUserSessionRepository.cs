using System;
using CloudBin.Core.Domain;

namespace CloudBin.Data
{
    public interface IUserSessionRepository : IRepository<UserSession, long>
    {
        UserSession GetSessionByToken(Guid token, bool fetchUser);
    }
}
