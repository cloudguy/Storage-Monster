using StorageMonster.Plugin.WebDav;
using StorageMonster.Services;
using StorageMonster.Services.Security;

namespace StorageMonster.Plugin.YandexDisk
{
    public class YandexDiskPlugin : WebDavPlugin
    {
        //public YandexDiskPlugin(IStorageAccountService accountService, ICryptoService cryptoService, ISecurityConfiguration securityConfiguration)
        //    : base(accountService, cryptoService, securityConfiguration)
        //{
        //}
        public override string Name { get { return "Yandex Disk"; } }
    }
}
