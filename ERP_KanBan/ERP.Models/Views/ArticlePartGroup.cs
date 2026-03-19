using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class ArticlePartGroup: BaseEntity
    {
        public Models.Views.Article Article { get; set; }
        public IEnumerable<Models.Views.ArticlePart> ArticlePart { get; set; }
        public IEnumerable<Models.Views.ArticleSizeRunUsage> ArticleSizeRunUsage { get; set; }
    }
}
