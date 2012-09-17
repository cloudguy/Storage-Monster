namespace StorageMonster.Web.Models.User
{
    public class UserAccountsModel
    {
        public StorageAccountsCollection AccountsCollection { get; set; }
        public bool CanAddAcounts { get; set; }
        public bool CanEditAcounts { get; set; }
        public bool CanDeleteAcounts { get; set; }
    }
}