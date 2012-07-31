using System;

namespace StorageMonster.Database
{
    public interface IDbConfiguration
    {
        String ConnectionString { get; }
    }
}
