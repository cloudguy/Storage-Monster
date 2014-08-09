using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudBin.Core.Domain;

namespace CloudBin.Data.NHibernate
{
    public sealed class UserSessionRepository : Repository<UserSession, long>, IUserSessionRepository
    {
        UserSession IUserSessionRepository.GetSessionByToken(Guid token, bool fetchUser)
        {
            var query = CurrentSession.QueryOver<UserSession>()
                .Where(s => s.Token == token);
            if (fetchUser)
            {
                query = query.Fetch(s => s.User).Eager;
            }
            return query.Take(1).SingleOrDefault();
        }
    }
}
