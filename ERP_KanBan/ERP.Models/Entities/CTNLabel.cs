using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CTNLabel
    {
        public CTNLabel()
        {
            CTNLabelItem = new HashSet<CTNLabelItem>();
            CTNOrders = new HashSet<CTNOrders>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? PLLocaleId { get; set; }
        public string OrderNo { get; set; }
        public DateTime ExFactoryDate { get; set; }
        public int TransitType { get; set; }
        public string TargetPort { get; set; }
        public string Customer { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public int? ProductType { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public string CustomerOrderNo { get; set; }
        public string ColorDesc { get; set; }
        public string OutsoleColorDescTW { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal PackingQty { get; set; }
        public int CTNS { get; set; }
        public DateTime? CloseDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int? IsPrint { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int CNoFrom { get; set; }

        public virtual ICollection<CTNLabelItem> CTNLabelItem { get; set; }
        public virtual ICollection<CTNOrders> CTNOrders { get; set; }
    }
}
