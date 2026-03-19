using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Shell
    {
        public Shell()
        {
            Project = new HashSet<Project>();
            ShellItem = new HashSet<ShellItem>();
            Style = new HashSet<Style>();
        }

        public decimal Id { get; set; }
        public string ShellNo { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? OwnerCompanyId { get; set; }
        public decimal? OwnerCustomerId { get; set; }
        public string StoragePlace { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? MoneyCodeId { get; set; }
        public string FishGoodsPhotoURL { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Company Locale { get; set; }
        public virtual Company OwnerCompany { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ShellItem> ShellItem { get; set; }
        public virtual ICollection<Style> Style { get; set; }
    }
}
