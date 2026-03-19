using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class users_dept_bas
    {
        public decimal? DEP_NO { get; set; }
        public string DEP_NAME { get; set; }
        public string DEP_CHIEF { get; set; }
        public decimal? PARENT_NO { get; set; }
    }
}
