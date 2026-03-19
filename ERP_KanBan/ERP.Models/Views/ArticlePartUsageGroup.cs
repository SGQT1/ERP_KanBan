using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class ArticlePartUsageGroup: BaseEntity
    {
        public Article Article { get; set; }
        public IEnumerable<ArticlePartUsage> ArticlePartUsage { get; set; }
    }
}
