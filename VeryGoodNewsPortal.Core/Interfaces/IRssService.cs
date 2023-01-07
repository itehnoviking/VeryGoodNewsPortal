using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface IRssService
    {
        IEnumerable<RssArticleDto> GetArticlesInfoFromRss(string rssUrl, Guid sourceId);

        Task<int> AggregateArticleDataFromRssSources();
    }
}