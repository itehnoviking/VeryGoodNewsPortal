namespace VeryGoodNewsPortal.Models
{
    public class ArticleListItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public float Rate { get; set; }
    }
}
