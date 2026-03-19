using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackArticleGroup
    {
        public PackArticle PackArticle { get; set; }
        public IEnumerable<PackArticleItem> PackArticleItem { get; set; }
    }
}