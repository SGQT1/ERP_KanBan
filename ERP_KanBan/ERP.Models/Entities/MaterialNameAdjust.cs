using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MaterialNameAdjust
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string PreMaterialName { get; set; }
        public string RevMaterialName { get; set; }
        public int? Confirmed { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public Guid? msrepl_tran_version { get; set; }
    }
}
