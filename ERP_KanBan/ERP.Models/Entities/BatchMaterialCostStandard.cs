using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BatchMaterialCostStandard
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BPCSId { get; set; }
        public decimal SemiGoods { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string MaterialNameTw { get; set; }
        public string UnitNameTw { get; set; }
        public decimal? StandardUsage { get; set; }
        public decimal? ActualUsage { get; set; }
        public string PurUnitNameTw { get; set; }
        public string PurDollarNameTw { get; set; }
        public decimal? PurUnitPrice { get; set; }
        public decimal? TransRate { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string CostDollarNameTw { get; set; }
        public decimal? CostUnitPrice { get; set; }
        public decimal? StandardCostAmount { get; set; }
        public decimal? ActualCostAmount { get; set; }
        public Guid? msrepl_tran_version { get; set; }
        public string VendorShortNameTw { get; set; }

        public virtual BatchProductionCostStandard BatchProductionCostStandard { get; set; }
    }
}
