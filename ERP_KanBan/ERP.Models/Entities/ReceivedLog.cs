using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ReceivedLog
    {
        public ReceivedLog()
        {
            MaterialStockReject = new HashSet<MaterialStockReject>();
            ReceivedLogSizeItem = new HashSet<ReceivedLogSizeItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ShippingListNo { get; set; }
        public decimal ShippingListVendorId { get; set; }
        public decimal POItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal SubTotalPrice { get; set; }
        public string Remark { get; set; }
        public decimal APId { get; set; }
        public DateTime? QCDate { get; set; }
        public decimal IQCFlag { get; set; }
        public decimal IQCGetQty { get; set; }
        public decimal? IQCTestQty { get; set; }
        public decimal IQCPassQty { get; set; }
        public decimal IQCRejectionQty { get; set; }
        public int IQCResult { get; set; }
        public string IQCMen { get; set; }
        public string IQCRemark { get; set; }
        public int SamplingMethod { get; set; }
        public decimal WarehouseId { get; set; }
        public decimal StockQty { get; set; }
        public string OrderNo { get; set; }
        public int IsAccounting { get; set; }
        public decimal TransferInId { get; set; }
        public decimal TransferInLocaleId { get; set; }
        public string TaiwanInvoiceNo { get; set; }
        public decimal TransferQty { get; set; }
        public decimal WeightUnitCodeId { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int? ReceivedCount { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ReceivedLogAdd ReceivedLogAdd { get; set; }
        public virtual ICollection<MaterialStockReject> MaterialStockReject { get; set; }
        public virtual ICollection<ReceivedLogSizeItem> ReceivedLogSizeItem { get; set; }
    }
}
