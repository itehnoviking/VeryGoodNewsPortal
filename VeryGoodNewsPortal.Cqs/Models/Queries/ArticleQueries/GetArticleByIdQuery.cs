using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.ArticleQueries
{
    public class GetArticleByIdQuery : IRequest<ArticleDto>
    {
        public GetArticleByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
