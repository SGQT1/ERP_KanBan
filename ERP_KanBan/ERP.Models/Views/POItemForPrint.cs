using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class POItemForPrint
    {
        public decimal POId { get; set; }
        public decimal POItemId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal VendorId { get; set; }
        public string Vendor { get; set; }
        public string PONo { get; set; }
        public int POType { get; set; }
        public int Status { get; set; }

        public DateTime PODate { get; set; }
        public string PurDollarNameTw { get; set; }
        public string PurUnitNameTw { get; set; }
        public decimal? PurQty { get; set; }
        
        public decimal? OrdersId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public DateTime? POVendorETD { get; set; }
        public DateTime? VendorETD { get; set; }
        public decimal? CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal ReceivedLocaleId { get; set; }
        public string PaymentLocale { get; set; }
        public string PurLocale { get; set; }
        public string ReceivedLocale { get; set; }
        public int PrintCount { get; set; }
        public DateTime? PrintTime { get; set; }
        public string ModifyUserName { get; set;}
        public DateTime? LastUpdateTime { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? DollarId { get; set; }
        public DateTime? LCSD { get; set; }

        public string PurLocaleTitle { get; set; }
        public string PurLocaleAddress { get; set; }
        public string PaymentLocaleTitle { get; set; }
        public string PaymentLocaleAddress { get; set; }
        public string ReceivedBarcode { get; set; }
        public string Customer { get; set; }

    }
}
