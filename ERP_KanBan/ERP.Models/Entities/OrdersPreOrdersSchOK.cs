using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersPreOrdersSchOK
    {
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public DateTime? BOMOKDate { get; set; }
        public DateTime? ClrSWOKDate { get; set; }
        public DateTime? PurOKDate { get; set; }
        public DateTime? CfmShoeOKDate { get; set; }
        public DateTime? SpecOKDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
