namespace StorageMonster.Services.Security
{
    public interface ISecurityConfiguration
    {
        string EncryptionKey { get; }
        byte[] EncryptionSalt { get; }
    }
}
