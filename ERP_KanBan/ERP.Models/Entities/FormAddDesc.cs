using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class FormAddDesc
    {
        public decimal Id { get; set; }
        public string DatName { get; set; }
        public string FUNC_NAME { get; set; }
        public string VerifyFlowDesc { get; set; }
        public string PublishDesc { get; set; }
        public string ReserveDesc { get; set; }
        public string ISONo { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
