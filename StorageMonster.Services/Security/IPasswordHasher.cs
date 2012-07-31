namespace StorageMonster.Services.Security
{
    public interface IPasswordHasher
    {
        string EncryptPassword(string password);
        string EncryptPassword(string password, string salt);
        string GetSaltFromHash(string hash);
    }
}
