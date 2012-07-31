using System;

namespace StorageMonster.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public abstract class MonsterDisplayAttribute : Attribute
	{
		public int ShowOrder { get; set; }		
	}
}
