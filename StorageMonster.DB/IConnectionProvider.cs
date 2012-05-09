using System.Data;

namespace StorageMonster.DB
{
	public interface IConnectionProvider
	{
		IDbConnection CreateConnection();
	    IDbConnection CurrentConnection { get; }
	    void CloseCurentConnection();
	}
}
