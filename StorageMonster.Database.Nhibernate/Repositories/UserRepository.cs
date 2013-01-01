using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetUserByEmail(string email)
        {
            return SessionManager.CurrentSession.QueryOver<User>()
                          .Where(u => u.Email == email).SingleOrDefault();
        }

        public IEnumerable<User> List()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public User Insert(User user)
        {
            SessionManager.CurrentSession.Save(user);
            return user;
        }

        public UpdateResult Update(User user)
        {
            try
            {
                SessionManager.CurrentSession.Update(user);
                SessionManager.CurrentSession.Flush();
                return UpdateResult.Success;
            }
            catch (StaleStateException)
            {
                return UpdateResult.Stalled;
            }
        }

        public Domain.User Load(Domain.User user)
        {
            throw new NotImplementedException();
        }

        public Domain.User Load(int id)
        {
            throw new NotImplementedException();
        }

        public Domain.User GetUserBySessionToken(Domain.Session session)
        {
            throw new NotImplementedException();
        }

        public Domain.User GetUserBySessionToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
