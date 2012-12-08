
namespace StorageMonster.Web.Services
{
    public enum ItemType
    {
        Folder,
        File
    }
    public interface IIconProvider
    {
        void Init();
        string GetIconPath(string fileName, ItemType type);
        string GetImagePath(string name);
    }
}