using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CodeItem
    {
        public decimal Id { get; set; }
        public string CodeType { get; set; }
        public string CodeNo { get; set; }
        public string NameTW { get; set; }
        public string NameEng { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ReferenceCodeNo { get; set; }
        public decimal LocaleId { get; set; }
        public int Disable { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
