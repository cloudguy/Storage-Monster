using System.Configuration;
using System.Globalization;
using System.Text;
using StorageMonster.Services.Security;


namespace StorageMonster.Services.Facade
{
    public class SecurityConfiguration : ISecurityConfiguration
    {
        public const string EncryptionKeyConfigKey = "EncryptionKey";
        public const string EncryptionSaltConfigKey = "EncryptionSalt";

        private object _initObject;
        private readonly object _lock = new object();


        private string _encryptionKey;
        private byte[] _encryptionSalt;

        public string EncryptionKey { get { return SafeGet(ref _encryptionKey); } }
        public byte[] EncryptionSalt { get { return SafeGet(ref _encryptionSalt); } }

        private T SafeGet<T>(ref T value)
        {
            if (_initObject == null)
            {
                lock (_lock)
                {
                    if (_initObject == null)
                        Initialize();
                }
            }
            return value;
        }



        private static string ParseString(string configSection)
        {
            string sValue = ConfigurationManager.AppSettings[configSection];
            if (string.IsNullOrEmpty(sValue))
                throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Configuration option {0} is missing or invalid", configSection));                    

            return sValue;
        }

        public void Initialize()
        {
            _encryptionKey = ParseString(EncryptionKeyConfigKey);
            string stringSalt = ParseString(EncryptionSaltConfigKey);
            _encryptionSalt = Encoding.ASCII.GetBytes(stringSalt);
            _initObject = new object();
        }
    }
}
