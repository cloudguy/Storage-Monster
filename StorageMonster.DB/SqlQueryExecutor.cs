using System;
using System.Data.Common;

namespace StorageMonster.DB
{
    public static class SqlQueryExecutor
    {
        public static T Exceute<T>(Func<T> @delegate)
        {
            try
            {
                return @delegate();
            }
            catch(DbException dbException)
            {
                throw new StorageMonsterDbException("Database exception", dbException);
            }
        }

        public static void Exceute(Action @delegate)
        {
            try
            {
                @delegate();
            }
            catch (DbException dbException)
            {
                throw new StorageMonsterDbException("Database exception", dbException);
            }
        }
    }
}
