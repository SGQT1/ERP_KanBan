using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SysGroup
    {
        public int Id { get; set; }
        public int? ParentGroupId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool Validate { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public string GroupCode { get; set; }
    }
}
