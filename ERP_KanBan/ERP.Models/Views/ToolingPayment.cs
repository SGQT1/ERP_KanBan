using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class ToolingPayment
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BrandTw { get; set; }
        public decimal CustomerId { get; set; }
        public string CustomerNameTw { get; set; }
        public string DebitNoteNo { get; set; }
        public DateTime DebitDate { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal DebitTFAmount { get; set; }
        public decimal APTFAmount { get; set; }
        public decimal? TFDiscount { get; set; }
        public decimal? PayTFAmount { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? PaymentCodeId { get; set; }
        public string VoucherNo { get; set; }
        public string BankDesc { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? APId { get; set; }
    }
}
