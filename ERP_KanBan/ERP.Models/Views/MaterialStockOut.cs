using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialStockOut
    {
        public decimal POItemId { get; set; }
        public decimal? Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal WarehouseId { get; set; }
        public string WarehouseNo { get; set; }
        public string OrderNo { get; set; }
        public decimal PCLUnitCodeId { get; set; }
        public string PCLUnitNameTw { get; set; }
        public string PCLUnitNameEn { get; set; }
        public decimal PCLPlanQty { get; set; }
        public decimal PCLQty { get; set; }
        public decimal PurQty { get; set; }
        public decimal ParentMaterialId { get; set; }
        public string DailyNo { get; set; }
        public decimal? MPSDailyId { get; set; }
        public decimal? MPSDailyLocaleId { get; set; }
        public int? SemiGoods { get; set; }     
    }
}
