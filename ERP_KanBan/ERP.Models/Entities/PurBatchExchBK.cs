using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PurBatchExchBK
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public int ActionType { get; set; }
        public string ComNo { get; set; }
        public string ComNameTw { get; set; }
        public string ComNameEn { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public string ParentMaterialNameEn { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public decimal PurLocaleId { get; set; }
        public string PurCompNo { get; set; }
        public string PurCompNameTw { get; set; }
        public string PurCompNameEn { get; set; }
        public decimal PlanQty { get; set; }
        public decimal OnHandQty { get; set; }
        public decimal UnitCodeId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public decimal PurBatchItemId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal CompanyId { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
