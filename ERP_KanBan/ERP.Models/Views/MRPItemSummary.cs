using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MRPItemSummary
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal PartId { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public decimal? MaterialId { get; set; }
        public string? MaterialNameTw { get; set; }
        public string? MaterialNameEn { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public decimal UnitTotal { get; set; }
        public decimal Total { get; set; }
        public int SizeDivision { get; set; }
        public string SizeDivisionDescTw { get; set; }
        public string SizeDivisionDescEn { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public int Version { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? OWD { get; set; }
        public DateTime OrderDate { get; set; }
        public string Customer { get; set; }
        public string ShoeName { get; set; }
        public decimal OrderQty { get; set; }
        public string LocaleNo { get; set; }
         public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public string Brand { get; set; }
        public decimal? BrandCodeId { get; set; }
    }
}