using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StorageMonster.Services
{
    public interface IStreamFactory
    {
        MonsterStream MakeDownloadStream(Stream inputStream);
    }
}
