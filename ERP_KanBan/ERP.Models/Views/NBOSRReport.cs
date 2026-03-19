using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class NBOSRReport
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public decimal? VendorId { get; set; }
        public string Vendor { get; set; }
        public decimal Qty { get; set; }
        public decimal? MaterialId { get; set; }
        public string MaterialNameEng { get; set; }
        public string SupplierCode { get; set; }
        public string MaterialCode { get; set; }
        public string PurUnitNameTw { get; set; }
        public string MaterialColor { get; set; }
        public DateTime? VendorETD { get; set; }


        public decimal? CompanyId { get; set; }
        public string? OrderNo { get; set; }
        public string? CustomerOrderNo { get; set; }
        public string? StyleNo { get; set; }
        public string? ShoeName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? OBDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public decimal? ReceivedQty { get; set; }
        public int? LeadTime { get; set; }
        public int? FactoryShipTime { get; set; }


    }
}
