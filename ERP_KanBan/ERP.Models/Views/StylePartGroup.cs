using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class StylePartGroup
    {
        public Models.Views.Style Style { get; set; }
        public IEnumerable<Models.Views.StylePart> StylePart { get; set; }
        public IEnumerable<Models.Views.StyleSizeRunUsage> StyleSizeRunUsage { get; set; }
        public IEnumerable<Models.Views.ArticleSizeRun> ArticleSizeRun { get; set; }
    }
    public class CopyStylePartGroup
    {
        public Models.Views.Style Style { get; set; }
        public IEnumerable<Models.Views.ArticleSizeRun> ArticleSizeRun { get; set; }

        public decimal RefLocaleId {get; set;}
        public string RefStyleNo {get; set;}
    }
}
