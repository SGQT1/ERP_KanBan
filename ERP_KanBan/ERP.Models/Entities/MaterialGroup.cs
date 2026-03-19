using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MaterialGroup
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string OtherName { get; set; }
        public string ThicknessCodeId { get; set; }
        public string ColorCodeId { get; set; }
        public string SpecCodeId { get; set; }
        public string AddOnDesc { get; set; }
    }
}
