using CloudBin.Core.Utilities;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;

namespace CloudBin.Data.NHibernate.Types
{
    internal sealed class UnixDateTimeOffsetType : IUserType
    {
        SqlType[] IUserType.SqlTypes
        {
            get { return new[] { SqlTypeFactory.Int64 }; }
        }
        Type IUserType.ReturnedType
        {
            get { return typeof(DateTimeOffset); }
        }

        bool IUserType.IsMutable
        {
            get { return false; }
        }

        int IUserType.GetHashCode(object x)
        {
            if (x == null)
                return 0;
            return x.GetHashCode();
        }

        object IUserType.NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            Int64 obj = (Int64)NHibernateUtil.Int64.NullSafeGet(rs, names[0]);
            return obj.ConvertFromUnixTimestamp();
        }

        void IUserType.NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.Int64.NullSafeSet(cmd, null, index);
                return;
            }

            var dtValue = (DateTimeOffset)value;
            ((IDataParameter)cmd.Parameters[index]).Value = dtValue.ConvertToUnixTimestamp();
        }

        object IUserType.DeepCopy(object value)
        {
            return value;
        }

        object IUserType.Replace(object original, object target, object owner)
        {
            return original;
        }

        object IUserType.Assemble(object cached, object owner)
        {
            return cached;
        }

        object IUserType.Disassemble(object value)
        {
            return value;
        }

        bool IUserType.Equals(object x, object y)
        {
            return Equals(x, y);
        }
    }
}
