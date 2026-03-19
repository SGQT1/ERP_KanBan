using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PayableItemForPay
    {
        public decimal Id { get; set; }
        public decimal APMonthId { get; set; }
        public string CloseMonth { get; set; }
        public string APYM { get; set; }
        public string Vendor { get; set; }
        public string PayDollar { get; set; }
        public decimal? PaymentLocaleId { get; set; } //付款地
        public string PaymentLocale { get; set; }
        public decimal? PurLocaleId { get; set; } //下單地
        public string PurLocale { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? CompanyId { get; set; } // Orders Company
        public string Company { get; set; }
        public string WarehouseNo { get; set; }
        

        public decimal? APTTL { get; set; }
        public decimal? APGet { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? APTax { get; set; }
        public decimal? PlanPayTTL { get; set; }
        public decimal? PurRate { get; set; }
        public decimal? SubPurRate { get; set; }
        public decimal? Discount { get; set; }
        public decimal? AP { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string PONo { get; set; }
        public string Material { get; set; }
        public string PurUnit { get; set; }
        public decimal PayQty { get; set; }
        public decimal PlanQty { get; set; }
        public decimal PayQtyTTL { get; set; }
        public decimal UnitPrice { get; set; }
        public string OrderNo { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? OrderDate { get; set; }
        public int IsGet { get; set; }
        public decimal APAmount { get; set; }
        public string StyleNo { get; set; }
        public DateTime? PODate { get; set; }
        public string Purchaser { get; set; }
        

    }
}
