using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;

namespace VeryGoodNewsPortal.Domain.Services
{
    public class PositivityGradeService : IPositivityGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IArticleService _articleService;
        private readonly ILogger<PositivityGradeService> _logger;
        private readonly IHtmlParserService _htmlParserService;

        private readonly string _affinJsonPath;


        public PositivityGradeService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IArticleService articleService, ILogger<PositivityGradeService> logger, IHtmlParserService htmlParserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _articleService = articleService;
            _logger = logger;
            _htmlParserService = htmlParserService;

            _affinJsonPath = "AFFIN.json";
        }

        private async Task<string> GetJsonFromIspras(string title)
        {
            try
            {

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                    var request = new HttpRequestMessage(HttpMethod.Post, $"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey={_configuration["ApplicationVariables:IsprasKey"]}")
                    {
                        Content = new StringContent("[{\"text\":\"" + title + "\"}]",

                            Encoding.UTF8,
                            "application/json")
                    };
                    var response = await httpClient.SendAsync(request);

                    var json = await response.Content.ReadAsStringAsync();

                    return json;
                }

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private Dictionary<string, int> GetDictionaryFromJsonAfinn()
        {
            string json;

            using (StreamReader streamReader = new StreamReader(_affinJsonPath, Encoding.UTF8))
            {
                json = streamReader.ReadToEnd();
            }

            var dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            return dict;
        }

        private async Task<Dictionary<string, int>> GetDictionaryUniqueWordsFromArticleTitle(string title)
        {
            var jsonFromArticleTitle = await GetJsonFromIspras(title);

            return DeserializeJsonIsprasInDictionary(jsonFromArticleTitle);
        }

        private Dictionary<string, int> DeserializeJsonIsprasInDictionary(string json)
        {
            var jsonArray = JArray.Parse(json);

            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (var item in jsonArray)
            {
                foreach (var lemma in item["annotations"]["lemma"])
                {
                    if (!dict.ContainsKey(lemma["value"].ToString()) & lemma["value"].ToString() != "")
                    {
                        dict.Add(lemma["value"].ToString(), 1);
                    }
                    else if (dict.ContainsKey(lemma["value"].ToString()))
                    {
                        dict[lemma["value"].ToString()]++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return dict;
        }

        public async Task GetAndSavingPositivityGrade()
        {
            var article = await _articleService.GetArticleTitleWithoutPositivityGrade();

            var dictionaryFromIsprasJson = await GetDictionaryUniqueWordsFromArticleTitle(article.Title);
            var dictionaryFromAffinJson = GetDictionaryFromJsonAfinn();

            double positivityGrade = 1;

            var sumAllValuesInIsprasDictionary = (double)(dictionaryFromIsprasJson.Values.Sum());

            foreach (var kvp in dictionaryFromIsprasJson)
            {
                if (dictionaryFromAffinJson.ContainsKey(kvp.Key))
                {
                    positivityGrade = positivityGrade + (dictionaryFromAffinJson[kvp.Key] * kvp.Value);

                    if (positivityGrade == 0)
                    {
                        positivityGrade++;
                    }
                }

                else
                {
                    continue;
                }
            }

            positivityGrade = (int)((positivityGrade / sumAllValuesInIsprasDictionary) * 100);

            await _articleService.UpdatePositivityGradeArticle(article.Id, (int)positivityGrade);
        }
    }
}
