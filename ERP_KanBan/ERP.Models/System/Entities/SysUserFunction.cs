using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class SysUserFunction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FunctionId { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
    }
}
