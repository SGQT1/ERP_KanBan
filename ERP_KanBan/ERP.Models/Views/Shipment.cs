using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Shipment
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal OrdersId { get; set; }
        public decimal SaleQty { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal ToolingCost { get; set; }
        public decimal OtherCharge { get; set; }
        public string OtherChargeDesc { get; set; }
        public bool IsCFM { get; set; }
        public decimal? ARId { get; set; }
        public DateTime? ARDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal FeedbackFund { get; set; }
        public decimal InvoiceId { get; set; } // Shipping
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? APTFundId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public decimal OrdersSubId { get; set; }
        public decimal? LessCharge { get; set; }
        public decimal CLB { get; set; }
        public decimal? APTFundF1Id { get; set; }
        public decimal? APTFundVId { get; set; }
        public string Season { get; set; }
        public int IsPriceBySeason { get; set; }
        public decimal? CustomerId { get; set; }
        public string Customer { get; set; }
        public string OrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public decimal? CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public decimal? BrandId { get; set; }
        public string Brand { get; set; }
        public decimal? Price { get; set; }
        public decimal? QuotationId { get; set; }
        public decimal? OutsolePrice { get; set; }
        public decimal? MidsolePrice { get; set; }
        public decimal? ToolingOtherPrice { get; set; }
        public decimal? ToolingTotalPrice { get; set; }
        public decimal? ToolFundIntel { get; set; }
        public decimal? FactoryPrice { get; set; }
        public decimal? InvoicePrice { get; set; }
        public DateTime? EffectiveDate { get; set; }
        
        public string RefCurrency { get; set; }
        public decimal? RefCurrencyId { get; set; }
        public decimal? RefProductTypeId { get; set; }
        public string RefProductType { get; set; }
        public decimal RefArticleId { get; set; }
        public string RefArticleNo { get; set; }
        public decimal RefStyleId { get; set; }
        public string RefStyleNo { get; set; }

        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}