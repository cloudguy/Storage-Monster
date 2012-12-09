using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using StorageMonster.Domain;
using StorageMonster.Utilities;

namespace StorageMonster.Database.Nhibernate.Types
{
    public class UserRoleType : IUserType
    {
        public SqlType[] SqlTypes
        {
            get { return new[] { SqlTypeFactory.Int32 }; }
        }
        public Type ReturnedType
        {
            get { return typeof(UserRole); }
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
            Int32 obj = (Int32)NHibernateUtil.Int32.NullSafeGet(rs, names[0]);
            return (UserRole)obj;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.Int32.NullSafeSet(cmd, null, index);
                return;
            }
            var roleValue = (UserRole)value;
            ((IDataParameter)cmd.Parameters[index]).Value = (int)roleValue;
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
