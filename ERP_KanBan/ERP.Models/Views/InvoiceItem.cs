using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class InvoiceItem
    {
        public decimal Id { get; set; } // ShipmentItemId
        public decimal InvoiceId { get; set; } // ShippingId
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
        public decimal? ARId { get; set; }
        public DateTime? ARDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal FeedbackFund { get; set; }
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

        public string OrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public string Currency { get; set; }
        public decimal? CurrencyId { get; set; }
        public string Brand { get; set; }
        public decimal? BrandId { get; set; }
        public string Company { get; set; }
        public decimal? CompanyId { get; set; }
        public string Customer { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public decimal ArticleId { get; set; }
        public string ArticleNo { get; set; }
        public decimal StyleId { get; set; }
        public string StyleNo { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal SubTotal { get; set; }

        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}
