using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class FactoryShippingContainer
    {
        public FactoryShippingContainer()
        {
            FactoryShippingContainerItem = new HashSet<FactoryShippingContainerItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string SONo { get; set; }
        public string ContainerNo { get; set; }
        public decimal? VesselId { get; set; }
        public string VesselTypeDesc { get; set; }
        public DateTime? ExFactoryDate { get; set; }
        public decimal? ExportQty { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public decimal? FeeAmount { get; set; }
        public string DollarNameTw { get; set; }
        public int? IsPay { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual FactoryShippingVessel FactoryShippingVessel { get; set; }
        public virtual ICollection<FactoryShippingContainerItem> FactoryShippingContainerItem { get; set; }
    }
}
