using System;

namespace CloudBin.Core.Domain
{
	public class StoragePluginDescriptor
	{
		public virtual int Id { get; set; }
		public virtual string ClassPath { get; set; }
		public virtual StoragePluginStatus Status { get; set; }
	}
}
