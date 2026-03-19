using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class TransferItem
    {
        public TransferItem()
        {
            TransferSizeItem = new HashSet<TransferSizeItem>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal TransferId { get; set; }
        public decimal ReceivedLogId { get; set; }
        public decimal TransferQty { get; set; }
        public decimal TargetCompanyId { get; set; }
        public string MaterialNameTwCust { get; set; }
        public string MaterialNameEnCust { get; set; }
        public decimal TransferQtyCust { get; set; }
        public string UnitCodeNameTwCust { get; set; }
        public string DollarCodeNameTwCust { get; set; }
        public decimal UnitPriceCust { get; set; }
        public decimal TaxRateCust { get; set; }
        public decimal AmountCust { get; set; }
        public string WeiUnitCodeNameTwCust { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public int SubCount { get; set; }
        public string Mark { get; set; }
        public string Remark { get; set; }
        public int TransferType { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual Transfer Transfer { get; set; }
        public virtual ICollection<TransferSizeItem> TransferSizeItem { get; set; }
    }
}
