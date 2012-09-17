using System;
using System.Data.Common;

namespace StorageMonster.Database
{
    public static class SqlQueryExecutor
    {
        public static T Execute<T>(Func<T> @delegate)
        {
            if (@delegate == null)
                throw new ArgumentNullException("delegate");
            try
            {
                return @delegate(); 
            }
            catch(DbException dbException)
            {
                throw new MonsterDbException("Database exception", dbException);
            }
        }

        public static void Execute(Action @delegate)
        {
            if (@delegate == null)
                throw new ArgumentNullException("delegate");

            try
            {
                @delegate();
            }
            catch (DbException dbException)
            {
                throw new MonsterDbException("Database exception", dbException);
            }
        }
    }
}
