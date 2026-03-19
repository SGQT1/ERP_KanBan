using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class FactoryShippingVessel
    {
        public FactoryShippingVessel()
        {
            FactoryShippingContainer = new HashSet<FactoryShippingContainer>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string Vessel { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? OBDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public decimal? ShippingPortId { get; set; }
        public decimal? TargetPortId { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public decimal? FeeAmount { get; set; }
        public string DollarNameTw { get; set; }
        public int? IsPay { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int TransitType { get; set; }
        public int? IsOutstand { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<FactoryShippingContainer> FactoryShippingContainer { get; set; }
    }
}
