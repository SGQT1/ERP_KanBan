using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSReceivedLogForAP
    {
        // Select doPay,A.PriceType,AK.WarehouseNo,Convert(varchar(10),ReceivedDate,112) ReceivedDate, PayMonth, NameTw,OrderNo,NULL, ChargeQty, FreeQty, AddFreeQty, QCBackQty, ReceivedQty, PurUnitName,UnitPrice,DiscountRate,Convert(Decimal(20,2),0),Convert(Decimal(20,2),ChargeQty*UnitPrice),DollarNameTw,'',PONo,StyleNo,NULL,
        // A.PaymentLocaleId,A.Id,AK.Id,AK.LocaleId,DayOfMonth From MpsReceivedLog AK  INNER JOIN MpsProcedurePO A ON AK.MpsProcedurePOId=A.Id and AK.RefLocaleId=A.LocaleId INNER JOIN MpsProcedureVendor B ON A.MpsProcedureVendorId=B.Id and A.LocaleId=B.LocaleId Where PayMonth='202502' and OrderNo like 'CENB00125%' and AK.Id not in (Select MpsReceivedLogId From MpsAP Where LocaleId=AK.LocaleId) and AK.LocaleId=10 Order by ReceivedDate, NameTw, PONo 2 rows 219 ms#

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal ChargeQty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal QCBackQty { get; set; }

        public int DoPay { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Remark { get; set; }
        public string PayMonth { get; set; }
        public decimal AddFreeQty { get; set; }
        public decimal DiscountRate { get; set; }
        public string WarehouseNo { get; set; }
        public decimal? DayOfMonth { get; set; }
        public int? PriceType { get; set; }
        public decimal? VendorId { get; set; }
        public string OrderNo { get; set; }
        public string PurUnitName { get; set; }
        public decimal? UnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public string PONo { get; set; }
        public string StyleNo { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public decimal POId { get; set; }
        public DateTime? CSD { get; set; }
        public string ProcedureNameTw { get; set; }
        public decimal? Amount { get; set; }
        public string Vendor { get; set; }
        public string DoPayUserName { get; set; }
        
    }
}
