using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IEnumerable<Domain.User> List()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public User Insert(Domain.User user)
        {
            return SessionManager.CurrentSession.Save(user) as User;
        }

        public UpdateResult Update(Domain.User user)
        {
            throw new NotImplementedException();
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
