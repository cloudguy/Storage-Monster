using System;

namespace StorageMonster.Domain
{
    public class StorageAccountSetting
    {
        public virtual int Id { get; set; }
        public virtual StorageAccount StorageAccount { get; set; }
        public virtual string SettingName { get; set; }
        public virtual string SettingValue { get; set; }
    }
}
