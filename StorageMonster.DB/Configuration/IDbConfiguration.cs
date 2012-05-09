using System;

namespace StorageMonster.DB.Configuration
{
    public interface IDbConfiguration
    {
        String ConnectionString { get; }
    }
}
