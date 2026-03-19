using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSStyle
    {
        public decimal Id { get; set; }
        public decimal MpsArticleId { get; set; }
        public string StyleNo { get; set; }
        public string ColorDesc { get; set; }
        public decimal LocaleId { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string RefOrderNo { get; set; }
        public int DoUsage { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? UnitRelaxTime { get; set; }
        public decimal? UnitStandardTime { get; set; }
        public decimal? UnitLaborCost { get; set; }
        public string RefOrderNoOfMaterial { get; set; }

        public string ArticleNo { get; set; }
        public decimal HasProcedure { get; set; }
        public string Brand { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        
    }
}
