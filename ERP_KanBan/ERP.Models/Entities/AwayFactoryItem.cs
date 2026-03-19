using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class AwayFactoryItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string FormNo { get; set; }
        public int? SeqNo { get; set; }
        public string ObjectName { get; set; }
        public string Color { get; set; }
        public string Spec { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string OrderNo { get; set; }
        public string Remark { get; set; }

        public virtual AwayFactory AwayFactory { get; set; }
    }
}
