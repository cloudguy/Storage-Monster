using System;

namespace StorageMonster.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MonsterInputBoxAttribute : MonsterDisplayAttribute
	{
		public bool Multiline { get; set; }
	}
}
