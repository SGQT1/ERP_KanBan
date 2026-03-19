using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class InspectLogSizeItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal ReceivedLogId { get; set; }
        public string DisplaySize { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal IQCGetQty { get; set; }
        public decimal StockQty { get; set; }
        public decimal TransferQty { get; set; }
        public int? SeqNo { get; set; }
        public double? ShoeInnerSize { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
