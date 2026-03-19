using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectItem
    {
        public decimal Id { get; set; }
        public decimal ProjectId { get; set; }
        public decimal ShoeSize { get; set; }
        public string Suffix { get; set; }
        public decimal InnerSize { get; set; }
        public decimal LeftQty { get; set; }
        public decimal RightQty { get; set; }
        public DateTime ExpectedShippingDate { get; set; }
        public DateTime? PlanShippingDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string SampleWONo { get; set; }
        public string DisplaySize { get; set; }
        public decimal LocaleId { get; set; }
        public string WorkOrderNo { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Project Project { get; set; }
    }
}
