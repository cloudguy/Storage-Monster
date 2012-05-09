namespace StorageMonster.Web.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public int StorageId { get; set; }
        public string AccountLogin { get; set; }
        public string TotalSpace { get; set; }
        public string UsedSpace { get; set; }
        public string FreeSpace { get; set; }
    }
}