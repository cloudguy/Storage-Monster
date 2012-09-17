using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StorageMonster.Services.Facade
{
    public class StreamFactory : IStreamFactory
    {
        public MonsterStream MakeDownloadStream(Stream inputStream)
        {
            return MonsterDownloadStream.Create(inputStream);
        }
    }
}
