using System;
using NHibernate.UserTypes;
using NHibernate.SqlTypes;
using System.Data;
using NHibernate;
using CloudBin.Core.Domain;

namespace CloudBin.Data.NHibernate.Types
{
    internal sealed class StoragePluginStatusType : IUserType
    {
        SqlType[] IUserType.SqlTypes
        {
            get { return new[] {SqlTypeFactory.Int32}; }
        }

        Type IUserType.ReturnedType
        {
            get { return typeof (StoragePluginStatus); }
        }

        bool IUserType.IsMutable
        {
            get { return false; }
        }

        int IUserType.GetHashCode(object x)
        {
            if (x == null)
            {
                return 0;
            }
            return x.GetHashCode();
        }

        object IUserType.NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            Int32 obj = (Int32) NHibernateUtil.Int32.NullSafeGet(rs, names[0]);
            return (StoragePluginStatus) obj;
        }

        void IUserType.NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.Int32.NullSafeSet(cmd, null, index);
                return;
            }
            StoragePluginStatus statusValue = (StoragePluginStatus) value;
            ((IDataParameter) cmd.Parameters[index]).Value = (int) statusValue;
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
