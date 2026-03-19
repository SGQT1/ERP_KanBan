using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialForPO
    {
        public decimal Id { get; set; }
        public decimal MaterialId { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public decimal LocaleId { get; set; }
        public string Material { get; set; }
        public string MaterialEng { get; set; }
        public decimal? SamplingMethod { get; set; }
        public decimal? TextureCodeId { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public decimal? SemiGoods { get; set; }
        public int HasQuot { get; set; }
        public decimal? VendorId { get; set; }
        public string VendorShortNameTw { get; set; }
        public decimal? DollarCodeId { get; set;}
        public decimal? UnitPrice { get; set; }
        public decimal? UnitCodeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public decimal? PayCodeId { get; set; }
        public decimal? ProcessMethod { get; set; }
        public decimal? MaterialQuotId { get; set; }
        public decimal? PaymentPoint { get; set; }
        public decimal? OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? Usage { get; set; }
        public decimal? PurQty { get; set; }
        public decimal? POQty { get; set; }

        public int? AlternateType { get; set; }
        public int? IsShowSizeRun { get; set; }

        // for PurBatchItem
        public decimal? PurPayCodeId { get; set; }
        public decimal? PurUnitCodeId  { get; set; }
        public decimal? PurUnitPrice { get; set; }
        public decimal? PurDollarCodeId { get; set; }
        public decimal? PurPayDollarCodeId { get; set; }
        public decimal? PurPaymentCodeId { get; set; }
        public decimal? PurPaymentPoint { get; set; }
        public decimal? PurSamplingMethod { get; set; }
        public decimal? PurVendorId { get; set; }
        public string PurVendorNameTw { get; set; }
        public DateTime? LastPODate { get; set; }
        public string Customer { get; set; }
    }
}
