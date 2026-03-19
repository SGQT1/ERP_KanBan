using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStockItemPO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POId { get; set; }
        public decimal POItemId { get; set; }
        public decimal StockIOId { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal StockDollarCodeId { get; set; }
        public string OrderNo { get; set; }
        public decimal? OPCount { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public string ParentMaterialNameEng { get; set; }
        public decimal? PurQty { get; set; }
        public string PONo { get; set; }
        public int? POType { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? StockOutQty { get; set; }
    }
}
