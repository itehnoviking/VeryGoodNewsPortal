namespace VeryGoodNewsPortal.Data.Entities
{
    public class Comment : BaseEntities
    {
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }

    }
}