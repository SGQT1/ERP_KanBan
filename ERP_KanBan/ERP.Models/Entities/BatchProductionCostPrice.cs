using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BatchProductionCostPrice
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal RefLocaleId { get; set; }
        public string OrderNo { get; set; }
        public string DisplaySize { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SaleQty { get; set; }
        public decimal? ArticleInnerSize { get; set; }
        public decimal Revenue { get; set; }
        public decimal MCostRate { get; set; }
        public decimal LCostRate { get; set; }
        public decimal MExpensesRate { get; set; }
        public decimal GrProfitRate { get; set; }
        public decimal NProfitRate { get; set; }
    }
}
