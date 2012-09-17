using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StorageMonster.Plugin
{
    public class StorageFileStreamResult : StorageQueryResult
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
    }
}
