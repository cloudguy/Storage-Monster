using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using StorageMonster.Utilities;
using System;
using System.Data;

namespace StorageMonster.Database.Nhibernate.Types
{
    public class UnixDateTimeOffset : IUserType
    {
        public SqlType[] SqlTypes
        {
            get { return new[] { SqlTypeFactory.Int64 }; }
        }
        public Type ReturnedType
        {
            get { return typeof(DateTimeOffset); }
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public int GetHashCode(object x)
        {
            if (x == null)
                return 0;
            return x.GetHashCode();
        }
        
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            Int64 obj = (Int64)NHibernateUtil.Int64.NullSafeGet(rs, names[0]);
            return obj.ConvertFromUnixTimestamp();
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.Int64.NullSafeSet(cmd, null, index);
                return;
            }

            var dtValue = (DateTimeOffset)value;
            Int64 intValue = dtValue.ConvertToUnixTimestamp();
            ((IDataParameter)cmd.Parameters[index]).Value = dtValue.ConvertToUnixTimestamp();
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        bool IUserType.Equals(object x, object y)
        {
            return Equals(x, y);
        }
    } 
}
