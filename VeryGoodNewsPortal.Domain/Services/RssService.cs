using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public class RssService : IRssService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RssService> _logger;
        private readonly ISourceService _sourceService;
        private readonly IArticleService _articleService;

        public RssService(IMapper mapper, ILogger<RssService> logger, ISourceService sourceService, IArticleService articleService)
        {
            _mapper = mapper;
            _logger = logger;
            _sourceService = sourceService;
            _articleService = articleService;
        }

        public async Task<int> AggregateArticleDataFromRssSources()
        {
            var rssUrls = await _sourceService.GetRssUrlsAsync();

            var concurrentDictionary = new ConcurrentDictionary<string, RssArticleDto>();

            var result = Parallel.ForEach(rssUrls, dto =>
            {
                GetArticlesInfoFromRss(dto.RssUrl)
                      .AsParallel()
                      .ForAll(articleDto => concurrentDictionary.TryAdd(articleDto.Url, articleDto));
            });

            var extArticlesUrls = await _articleService.GetAllExistingArticleUrls();

            Parallel.ForEach(extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)),
                s => concurrentDictionary.Remove(s, out var dto));

            var articleDtos = concurrentDictionary.Values.Select(dto => _mapper.Map<ArticleDto>(dto)).ToArray();


            return await _articleService.InsertArticles(articleDtos);
        }



        public IEnumerable<RssArticleDto> GetArticlesInfoFromRss(string rssUrl)
        {
            try
            {
                using (var reader = XmlReader.Create(rssUrl))
                {
                    var feed = SyndicationFeed.Load(reader);

                    var result = feed.Items
                        .Select(item => _mapper.Map<RssArticleDto>(item)).
                        ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
    }
}
