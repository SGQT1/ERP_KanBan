using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class POItemProcess
    {
        public string Vendor { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public DateTime PODate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? QCDate { get; set; }
        public DateTime? StockInDate { get; set; }
        public DateTime? StockOutDate { get; set; }
        public decimal? PurUnitCodeId { get; set; }
        public string PurUnitCode { get; set; }
        public decimal? PlanQty { get; set; }
        public decimal? PurQty { get; set; }
        public decimal? IQCGetQty { get; set; }
        public decimal? ReceivedQty { get; set; }
        public decimal? MaterialStockQty { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? StockInQty { get; set; }
        public decimal? StockOutQty { get; set; }
 
        public decimal? OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public DateTime? VendorETD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? CSD { get; set; }
        public decimal? CompanyId { get; set; }
        public decimal ReceivedLocaleId { get; set; } //收貨地
        public decimal PaymentLocaleId { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal LocaleId { get; set; }
        public string PONo { get; set; }
        public decimal POItemId { get; set; }
        public DateTime? ShipmentDate { get; set; }
    }
}
