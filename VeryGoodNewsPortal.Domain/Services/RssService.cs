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
        private readonly IHtmlParserService _htmlParserService;

        public RssService(IMapper mapper, ILogger<RssService> logger, ISourceService sourceService, IArticleService articleService, IHtmlParserService htmlParserService)
        {
            _mapper = mapper;
            _logger = logger;
            _sourceService = sourceService;
            _articleService = articleService;
            _htmlParserService = htmlParserService;
        }

        public async Task<int> AggregateArticleDataFromRssSources()
        {
            var rssUrls = await _sourceService.GetRssUrlsAsync();

            var concurrentDictionary = new ConcurrentDictionary<string, RssArticleDto>();

            Parallel.ForEach(rssUrls, dto =>
            {
                GetArticlesInfoFromRss(dto.RssUrl, dto.SourceId)
                      .AsParallel()
                      .ForAll(articleDto => concurrentDictionary.TryAdd(articleDto.Url, articleDto));
            });

            var extArticlesUrls = await _articleService.GetAllExistingArticleUrls();

            var resultDtos = Parallel.ForEach(extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)),
                s => concurrentDictionary.Remove(s, out var dto));

            var articleDtos = concurrentDictionary.Values.Select(dto => _mapper.Map<ArticleDto>(dto)).ToArray();


            foreach (var dto in articleDtos)
            {
                dto.Description = await GetDescriptionTextFromRssTextOnlinerAsync(dto.Description);
                dto.Body = await _htmlParserService.GetArticleContentFromUrlAsync(dto.SourceUrl);
            }

            return await _articleService.InsertArticles(articleDtos);
        }



        public IEnumerable<RssArticleDto> GetArticlesInfoFromRss(string rssUrl, Guid sourceId)
        {
            try
            {
                using (var reader = XmlReader.Create(rssUrl))
                {
                    var feed = SyndicationFeed.Load(reader);

                    var result = feed.Items
                        .Select(item => _mapper.Map<RssArticleDto>(item)).
                        ToList();

                    foreach (var dto in result)
                    {
                        dto.SourceId = sourceId;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }

        private async Task<string> GetDescriptionTextFromRssTextOnlinerAsync(string text)
        {
            var subs = text.Split("<p>");

            var result = subs[2];

            return result.Replace("</p>", "");
        }
    }
}
