using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Bond
    {
        public Bond()
        {
            BondItemProduct = new HashSet<BondItemProduct>();
            BondItemProductUpdate = new HashSet<BondItemProductUpdate>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BizOrgName { get; set; }
        public string TradeTypeName { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string ReceiptBizOrgName { get; set; }
        public string ArrivalRegion { get; set; }
        public string ReceiptOrgCodeNo { get; set; }
        public string ForeignOrgName { get; set; }
        public string TaxationType { get; set; }
        public string ApprovedNo { get; set; }
        public string AgreementNo { get; set; }
        public string ImportBondNo { get; set; }
        public string ExportBondNo { get; set; }
        public decimal DollarCodeId { get; set; }
        public string ExportPortName { get; set; }
        public string ExportPortName2 { get; set; }
        public string ExportPortName3 { get; set; }
        public string ExportPortName4 { get; set; }
        public string ExportPortName5 { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public decimal? ImportValue { get; set; }
        public decimal? ExportValue { get; set; }
        public string ManualNo { get; set; }
        public string ImportantMark { get; set; }
        public decimal? SupervisionFee { get; set; }
        public string Approver { get; set; }
        public string BondType { get; set; }
        public string Remark { get; set; }
        public decimal InOutDollarCodeId { get; set; }
        public int Status { get; set; }
        public string InnerReferNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<BondItemProduct> BondItemProduct { get; set; }
        public virtual ICollection<BondItemProductUpdate> BondItemProductUpdate { get; set; }
    }
}
