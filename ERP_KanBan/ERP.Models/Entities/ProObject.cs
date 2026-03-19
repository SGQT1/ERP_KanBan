using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProObject
    {
        public ProObject()
        {
            ProObjectQuot = new HashSet<ProObjectQuot>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ObjectNo { get; set; }
        public string ObjectNameTw { get; set; }
        public string ObjectNameEn { get; set; }
        public string SpecDesc { get; set; }
        public string ObjecType { get; set; }
        public string UnitName { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int? SamplingMethod { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<ProObjectQuot> ProObjectQuot { get; set; }
    }
}
