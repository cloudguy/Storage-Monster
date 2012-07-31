using System.Collections.Generic;
using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
	public interface IUserRepository
	{
	    User GetUserByEmail(string email);
	    IEnumerable<User> List();
	    void DeleteAll();
	    User Insert(User user);
        User Update(User user);
        User Load(User user);
        User Load(int id);
	    User GetUserBySessionToken(Session session);
        User GetUserBySessionToken(string token);
	}
}
