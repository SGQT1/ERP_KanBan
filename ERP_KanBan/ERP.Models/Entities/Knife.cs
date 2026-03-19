using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Knife
    {
        public Knife()
        {
            KnifeItem = new HashSet<KnifeItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string KnifeNo { get; set; }
        public string Remark { get; set; }
        public string KnifePhotoURL { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? OwnerCompanyId { get; set; }
        public decimal? OwnerCustomerId { get; set; }
        public string StoragePlace { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? MoneyCodeId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<KnifeItem> KnifeItem { get; set; }
    }
}
