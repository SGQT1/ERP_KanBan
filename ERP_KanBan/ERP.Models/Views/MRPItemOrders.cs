using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MRPItemOrders
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal PartId { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public decimal UnitTotal { get; set; }
        public decimal Total { get; set; }
        public int SizeDivision { get; set; }
        public string SizeDivisionDescTw { get; set; }
        public string SizeDivisionDescEn { get; set; }
        public int OrderVersion { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int? SemiGoods { get; set; }
        public string SizeUsage { get; set; }
        public int? MRPVersion { get; set; }
        public decimal? CategoryCodeId { get; set; }
    }
}
