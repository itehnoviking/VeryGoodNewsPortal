using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Core.DTOs
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public int PositivityGrade { get; set; }

        public string SourceUrl { get; set; }

        public Guid SourceId { get; set; }

        public IEnumerable<CommentDto> CommentDtos { get; set; }
    }
}
