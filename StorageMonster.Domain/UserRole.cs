namespace StorageMonster.Domain
{
    public class UserRole
    {
        public const string RoleUser = "ROLE_USER";
        public const string RoleAdmin = "ROLE_ADMIN";

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
