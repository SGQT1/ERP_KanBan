using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class AwayFactory
    {
        public AwayFactory()
        {
            AwayFactoryItem = new HashSet<AwayFactoryItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int Type { get; set; }
        public string FormNo { get; set; }
        public DateTime AwayDate { get; set; }
        public string OrgUnitName { get; set; }
        public string CustomerName { get; set; }
        public string Reason { get; set; }
        public int IsReturn { get; set; }
        public int ReturnCFM { get; set; }
        public DateTime? PlanReturnDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string ReturnUserName { get; set; }
        public DateTime? ReturnUpdateTime { get; set; }
        public string ReturnMark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int IsGuard { get; set; }
        public string GuardUserName { get; set; }
        public DateTime? GuardUpdateTime { get; set; }
        public string GuardMark { get; set; }

        public virtual ICollection<AwayFactoryItem> AwayFactoryItem { get; set; }
    }
}
