using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class POItemForVendor
    {
        public string Vendor { get; set; }
        public int? PaymentPoint { get; set; }
        public int DayOfMonth { get; set; }
        public string RefPONo { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEng { get; set; }
        public DateTime PODate { get; set; }
        public decimal? PurUnitPrice { get; set; }
        public decimal? PurSubTotalPrice { get; set; }
        public string PurDollarNameTw { get; set; }
        public decimal PurDollarCodeId { get; set; }
        public string PurUnitNameTw { get; set; }
        public decimal? PlanQty { get; set; }
        public decimal? PurPlanQty { get; set; }
        public decimal? PurQty { get; set; }

        public int Status { get; set; }
        public int? POType { get; set; }
        public decimal? PayQty { get; set; }
        public decimal? FreeQty { get; set; }
        public decimal? PayTTL { get; set; }
        public decimal? PayRate { get; set; }

        public decimal? OrdersId { get; set; }
        public string OrderNo { get; set; }

        public DateTime? VendorETD { get; set; }
        public decimal? CompanyId { get; set; }
        public decimal ReceivedLocaleId { get; set; } //收貨地
        public decimal PaymentLocaleId { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal LocaleId { get; set; }
        public string PONo { get; set; }
        public string Purchaser { get; set; }
        public string CloseMonth { get; set; }
        public decimal POItemId { get; set; }
        public string ReceivedBarcode { get; set; }

        public string? StyleNo { get; set; }
        public string? ShoeName { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? SubTotalPrice { get; set; }
        public decimal? IQCGetQty { get; set; }
        public decimal? ReceivedQty { get; set; }
        public string Confirmer { get; set; }
        public decimal? ReceivedId { get; set; }
        public int QCResult { get; set; }


    }
}
