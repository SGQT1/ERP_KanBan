using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class FactoryShippingContainerItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ContainerId { get; set; }
        public decimal RefLocaleId { get; set; }
        public string OrderNo { get; set; }
        public int SeqId { get; set; }
        public decimal ExportQty { get; set; }
        public decimal? MEAS { get; set; }
        public DateTime? PlanFinishDate { get; set; }
        public DateTime? PlanInspectDate { get; set; }
        public DateTime? PlanShippingDate { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual FactoryShippingContainer FactoryShippingContainer { get; set; }
    }
}
