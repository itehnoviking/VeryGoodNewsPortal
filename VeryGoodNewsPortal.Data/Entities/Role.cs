namespace VeryGoodNewsPortal.Data.Entities
{
    public class Role : BaseEntities
    {
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}