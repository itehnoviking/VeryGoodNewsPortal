namespace VeryGoodNewsPortal.Data.Entities
{
    public class UserRole : BaseEntities
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}