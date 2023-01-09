using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Domain.Services;

namespace FirstMvcApp.Domain.Services
{
    public class HtmlParserService : IHtmlParserService
    {
        private readonly ISourceService _sourceService;

        public HtmlParserService(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }
        public async Task<string> GetArticleContentFromUrlAsync(string url)
        {
            var sourceId = await _sourceService.GetSourceByUrl(url);

            switch (sourceId.ToString("D").ToUpperInvariant())
            {
                case "33DF34C2-7DFD-4E72-8730-67B4075B83B8":
                    return await ParseOnlinerArticle(url);

                default:
                    break;
            }

            return null;
        }

        private async Task<string> ParseOnlinerArticle(string url)
        {
            //todo algoryth of web scrab


            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");

            var scriptNode = node.SelectSingleNode("//div[@class='news-text']/script");
            if (scriptNode != null)
            {
                node.RemoveChild(scriptNode);
            }

            var refNode = node.SelectSingleNode("//div[@class='news-text']/div[@class='news-reference']");
            if (refNode != null)
            {
                node.RemoveChild(refNode);
            }

            var widget = node.SelectSingleNode("//div[@class='news-text']/div[contains(@class, 'news-widget')]");
            if (widget != null)
            {
                node.RemoveChild(widget);
            }

            var telegramLinks = node.SelectNodes("//div[@class='news-text']/p[@style='text-align: right;']");
            if (telegramLinks != null)
            {
                node.RemoveChildren(telegramLinks);
            }

            var specialLinks = node.SelectNodes("//div[@class='news-text']/a[@target='_blank']");
            if (specialLinks != null)
            {
                node.RemoveChildren(specialLinks);
            }

            var articleText = node.InnerHtml.Trim();

            return articleText;
        }

        private async Task<string> ParseLentaRuArticle(string url)
        {
            //todo algoryth of web scrab 

            return null;
        }

        private async Task<string> Parse4PdaArticle(string url)
        {
            //todo algoryth of web scrab 

            return null;
        }
    }
}
