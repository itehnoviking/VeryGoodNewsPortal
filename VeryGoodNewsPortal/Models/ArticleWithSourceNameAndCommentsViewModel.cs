using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Models
{
    public class ArticleWithSourceNameAndCommentsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid SourceId { get; set; }
        public Source Source { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
