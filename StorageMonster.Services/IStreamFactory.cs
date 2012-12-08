using System.IO;

namespace StorageMonster.Services
{
    public interface IStreamFactory
    {
        MonsterStream CreateDownloadStream(Stream inputStream);
    }
}
