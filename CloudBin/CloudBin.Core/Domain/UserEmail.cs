namespace CloudBin.Core.Domain
{
    public class UserEmail
    {
        public virtual int Id { get; set; }
        public virtual string Email { get; set; }
        public virtual User User { get; set; }
    }
}
