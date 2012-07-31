using System;

namespace StorageMonster.Domain
{
    public class StorageAccountSetting
    {
        public int Id { get; set; }
        public int StorageAccountId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public DateTime Stamp { get; set; }
    }
}
