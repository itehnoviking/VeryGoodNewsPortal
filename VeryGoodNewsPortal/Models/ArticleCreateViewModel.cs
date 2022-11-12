using Microsoft.AspNetCore.Mvc.Rendering;

namespace VeryGoodNewsPortal.Models
{
    public class ArticleCreateViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public Guid SourceId { get; set; }

        public DateTime CreationDate { get; set; }
        public IEnumerable<SelectListItem> SourceNameAndIdModels { get; set; }
    }
}
