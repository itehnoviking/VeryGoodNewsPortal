namespace VeryGoodNewsPortal.Data.Entities
{
    public class Source : BaseEntities
    {
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public string RssUrl { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}