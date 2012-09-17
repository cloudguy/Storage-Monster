using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Services
{
    public enum ItemType
    {
        Folder,
        File
    }
    public interface IIconProvider
    {
        void Initizlize();
        string GetIconPath(string fileName, ItemType type);
        string GetImagePath(string name);
    }
}