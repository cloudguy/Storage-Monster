using System.Data;

namespace StorageMonster.Database
{
	public interface IConnectionProvider
	{
		IDbConnection CreateConnection();
	    IDbConnection CurrentConnection { get; }
	    void CloseCurrentConnection();
	}
}
