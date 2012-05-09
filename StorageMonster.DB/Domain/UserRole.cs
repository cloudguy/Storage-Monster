namespace StorageMonster.DB.Domain
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
