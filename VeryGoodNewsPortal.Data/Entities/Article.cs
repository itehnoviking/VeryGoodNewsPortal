namespace VeryGoodNewsPortal.Data.Entities
{
    public class Article : BaseEntities 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public int PositivityGrade { get; set; }
        public string SourceUrl { get; set; }

        public Guid SourceId { get; set; }
        public virtual Source Source { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}