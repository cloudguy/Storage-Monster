using System.IO;

namespace StorageMonster.Services.Facade
{
    public class StreamFactory : IStreamFactory
    {
        public MonsterStream CreateDownloadStream(Stream inputStream)
        {
            return MonsterDownloadStream.Create(inputStream);
        }
    }
}
