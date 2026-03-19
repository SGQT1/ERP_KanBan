using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class PurBatchItemRemoved
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BatchNo { get; set; }
        public DateTime? BatchDate { get; set; }
        public string OrderNo { get; set; }
        public string MaterialNameTw { get; set; }
        public string PlanUnitNameTw { get; set; }
        public decimal PlanQty { get; set; }
        public string VendorNameTw { get; set; }
        public decimal PurLocaleId { get; set; }
        public decimal? POItemId { get; set; }
        public decimal OnHandQty { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal RefLocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
