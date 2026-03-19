using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BatchMaterialCostSize
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public decimal SemiGoods { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string MaterialNameTw { get; set; }
        public string UnitNameTw { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal StandardUsage { get; set; }
        public decimal? CostUnitPrice { get; set; }
        public decimal? StandardCostAmount { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
