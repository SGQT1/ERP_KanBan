using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProjectPOAP
    {
        public ProjectPOAP()
        {
            ProjectPOAPItem = new HashSet<ProjectPOAPItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string APMonth { get; set; }
        public decimal PaymentLocaleId { get; set; }
        public decimal RefVendorId { get; set; }
        public decimal RefLocaleId { get; set; }
        public int PayCodeId { get; set; }
        public decimal TotalAPAmount { get; set; }
        public decimal AdjustAPAmount { get; set; }
        public decimal PayTotalAmount { get; set; }
        public string PayDollarNameTw { get; set; }
        public int PayConfirmed { get; set; }
        public string PayCfmUserName { get; set; }
        public DateTime? PayCfmUpdateTim { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string BillCopyURL1 { get; set; }
        public string BillCopyURL2 { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<ProjectPOAPItem> ProjectPOAPItem { get; set; }
    }
}
