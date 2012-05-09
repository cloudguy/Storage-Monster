using System.Collections.Generic;
using StorageMonster.DB.Domain;

namespace StorageMonster.DB.Repositories
{
	public interface IUserRepository
	{
	    User GetByEmail(string email);
	    IEnumerable<User> List();
	    void DeleteAll();
	    User Insert(User user);
        User Update(User user);
        User Select(User user);
        User Select(int id);
	    User GetUserBySessionToken(Session session);
        User GetUserBySessionToken(string token);
	}
}
