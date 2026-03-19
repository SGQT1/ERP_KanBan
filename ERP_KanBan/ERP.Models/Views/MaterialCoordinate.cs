using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialCoordinate
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public decimal CompanyId { get; set; }
        public decimal MaterialId { get; set; }
        public string Material { get; set; }
        public string MaterialEn { get; set; }
        public decimal UnitCodeId { get; set; }
        public string Unit { get; set; }
        public int? SemiGoods { get; set; }
        public int SecMat { get; set; }
        public decimal? ParentMaterialId { get; set; }

        public decimal? PlanQty { get; set; }
        public decimal? PurPlanQty { get; set; }
        public decimal? POQty { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? StockInQty { get; set; }
        public decimal? StockOutQty { get; set; }

        public decimal? UsageRate { get; set; }
        public decimal? LossRate { get; set; }
        public string Vendor { get; set; }
        public decimal? VendorId { get; set; }
        public DateTime? PODate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? ReceivedQty { get; set; }
    }
}
