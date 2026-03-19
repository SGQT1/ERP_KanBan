using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class BondMRPItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public int IsAdh { get; set; }
        public int IsSub { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string MaterialNameTw { get; set; }
        public string UnitNameTw { get; set; }
        public decimal Total { get; set; }
        public decimal? WeightEachUnit { get; set; }
        public decimal? Weight { get; set; }
        public string BondMaterialName { get; set; }
        public string VendorShortNameTw { get; set; }
        public int SeqNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string PurDollarNameTw { get; set; }
        public string PayDollarNameTw { get; set; }
        public string ExDollarNameTw { get; set; }
        public string ParentBy { get; set; }

        public string StyleNo { get; set; }
        public string BondProductName { get; set; }
        public string BondNo { get; set; }
        public int IsClose { get; set; }
        public decimal BOMLocaleId { get; set; }
        public DateTime? SalesDate { get; set; }
        public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? CSD { get; set; }
        public decimal OrderQty { get; set; }
    } 
}
