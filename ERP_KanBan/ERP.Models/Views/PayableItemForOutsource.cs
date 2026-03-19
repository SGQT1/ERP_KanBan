using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PayableItemForOutsource
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedurePOId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal PayQty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal QCBackQty { get; set; }
        public int doPay { get; set; }
        public string Remark { get; set; }
        public string PayMonth { get; set; }
        public decimal AddFreeQty { get; set; }
        public decimal DiscountRate { get; set; }
        public string WarehouseNo { get; set; }

        public string Vendor { get; set; }
        public decimal VendorId { get; set; }
        public string PONo { get; set; }
        public decimal PlanQty { get; set;}
        public string Unit { get; set; }
        public decimal PurQty { get; set; }

        public decimal PayQtyTTL { get; set; }
        public decimal? SubAmount { get; set; }
        public decimal? PayDollarCodeId { get; set; }
        public string PayDollar { get; set; }
        public string CloseMonth { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? VendorETD { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string MPSProcedure { get; set; }

        public decimal? ReceivedLocaleId { get; set; } //收貨地
        public decimal? PaymentLocaleId { get; set; } //付款地
        public decimal? PurLocaleId { get; set; } //下單地
        public decimal? POLocaleId { get; set; } // PO LocaelId
        public decimal? CompanyId { get; set; } // Orders Company
        public int? PriceType { get; set; }
    }
}
