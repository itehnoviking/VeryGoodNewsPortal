﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface IHtmlParserService
    {
        Task<string> GetArticleContentFromUrlAsync(string url);
        //string DeleteAllHtmlTegsInText(string text);
    }
}
