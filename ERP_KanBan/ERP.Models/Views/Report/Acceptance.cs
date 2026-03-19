using System;
using System.Collections.Generic;

namespace ERP.Models.Views.Report
{
    public partial class Acceptance
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string PONo { get; set; }
        public string Vendor { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEng { get; set; }
        public string UnitName { get; set; }
        public decimal IQCGetQty { get; set; }
        public decimal? IQCTestQty { get; set; }
        public decimal IQCPassQty { get; set; }
        public decimal StockQty { get; set; }
        public string OrderNo { get; set; }
        public decimal NetWeight { get; set; }
        public string WarehouseNo { get; set; }
        public DateTime? ETD { get; set; }
        public decimal TransferInId { get; set; }
        public decimal PaymentLocaleId { get; set; }

    }
    public partial class AcceptanceForm
    {
        public string No { get; set; }
        public string Id { get; set; }
        public string LocaleId { get; set; }
        public string ReceivedDate { get; set; }
        public string PONo { get; set; }
        public string Vendor { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEng { get; set; }
        public string UnitName { get; set; }
        public string IQCGetQty { get; set; }
        public string IQCTestQty { get; set; }
        public string IQCPassQty { get; set; }
        public string StockQty { get; set; }
        public string OrderNo { get; set; }
        public string NetWeight { get; set; }
        public string WarehouseNo { get; set; }
        public string ETD { get; set; }
        public string TransferInId { get; set; }

    }
}
