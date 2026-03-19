using System;
using System.Collections.Generic;
using System.Text;
using ERP.Models.Views.View;

namespace ERP.Models.Views
{
    public class ArticleSizeRunGroup
    {
        public Article Article { get;set; }
        public IEnumerable<Models.Views.ArticleSizeRun> ArticleSizeRun { get;set; }
    }
}