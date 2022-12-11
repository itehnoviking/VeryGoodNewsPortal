using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceNameAndIdDto>> GetSourceNameAndId();

        Task<IEnumerable<RssUrlsFromSourceDto>> GetRssUrlsAsync();
        Task<Guid> GetSourceByUrl(string url);
    }
}
