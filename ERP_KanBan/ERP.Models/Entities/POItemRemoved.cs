using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class POItemRemoved
    {
        public POItemRemoved()
        {
            POItemSizeRemoved = new HashSet<POItemSizeRemoved>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POItemId { get; set; }
        public DateTime PODate { get; set; }
        public string PONo { get; set; }
        public string VendorNameTw { get; set; }
        public int IsShowSizeRun { get; set; }
        public DateTime VendorETD { get; set; }
        public int IsAllowPartial { get; set; }
        public string PORemark { get; set; }
        public string PhotoURLDescTw { get; set; }
        public string PhotoURL { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? CompanyId { get; set; }
        public int POType { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal UnitPrice { get; set; }
        public string DollarNameTw { get; set; }
        public decimal Qty { get; set; }
        public string UnitNameTw { get; set; }
        public int PayCodeId { get; set; }
        public decimal ReceivingLocaleId { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public string PaymentNameTw { get; set; }
        public int PaymentPoint { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public int IsOverQty { get; set; }
        public int SamplingMethod { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int Status { get; set; }
        public DateTime FactoryETD { get; set; }
        public string Remark { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<POItemSizeRemoved> POItemSizeRemoved { get; set; }
    }
}
