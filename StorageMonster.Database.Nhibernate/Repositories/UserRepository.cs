using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Database.Repositories;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Domain.User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.User> List()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public Domain.User Insert(Domain.User user)
        {
            throw new NotImplementedException();
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
