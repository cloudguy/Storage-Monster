using System;

namespace StorageMonster.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MonsterPasswordBoxAttribute : MonsterDisplayAttribute
    {
    }
}
