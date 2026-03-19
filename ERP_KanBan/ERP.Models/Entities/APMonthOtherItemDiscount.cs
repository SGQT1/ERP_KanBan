using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class APMonthOtherItemDiscount
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal APMonthOtherId { get; set; }
        public int? IsGet { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ReceiveRefNo { get; set; }
        public decimal? PayQty { get; set; }
        public string DollarCodeName { get; set; }
        public decimal? BankingRate { get; set; }
        public decimal? APAmount { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? APTax { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? PreAPGet { get; set; }
        public decimal? APTTL { get; set; }
        public decimal? APGet { get; set; }
        public string UniInvoiceNo { get; set; }
        public string Remark { get; set; }
        public string APYM { get; set; }
        public decimal? Discount { get; set; }
        public string PUnit { get; set; }
        public string TypeNo { get; set; }
        public string VendorMaterialNo { get; set; }
        public string Spec { get; set; }
        public string WONo { get; set; }
        public string WarehouseNo { get; set; }
        public string SeqNo { get; set; }
        public int? IsDraft { get; set; }
        public string VoucherNo { get; set; }
        public decimal? PurLocaleId { get; set; }
        public string PurUserName { get; set; }
        public decimal? POItemId { get; set; }
        public decimal? ReceivedLocaleId { get; set; }
        public decimal? ReceivedLogId { get; set; }
    }
}
