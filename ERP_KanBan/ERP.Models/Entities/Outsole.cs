using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Outsole
    {
        public Outsole()
        {
            OutsoleItem = new HashSet<OutsoleItem>();
            Project = new HashSet<Project>();
            Quotation = new HashSet<Quotation>();
            Style = new HashSet<Style>();
        }

        public decimal Id { get; set; }
        public string OutsoleNo { get; set; }
        public string Remark { get; set; }
        public string OutsolePhotoURL { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? OwnerCompanyId { get; set; }
        public decimal? OwnerCustomerId { get; set; }
        public string StoragePlace { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? MoneyCodeId { get; set; }
        public string SpecCharacter { get; set; }
        public decimal LocaleId { get; set; }
        public string OtherDesc { get; set; }
        public decimal? VendorId { get; set; }
        public string MDNo { get; set; }
        public string MDDesc { get; set; }
        public decimal? MDVendorId { get; set; }
        public string EVANo { get; set; }
        public string EVADesc { get; set; }
        public decimal? EVAVendorId { get; set; }
        public string MDPhotoURL { get; set; }
        public string EVAPhotoURL { get; set; }
        public string TrademarkPhotoURL { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal? TCQty { get; set; }
        public decimal? TC { get; set; }
        public decimal? TCTTL { get; set; }
        public decimal? MDQty { get; set; }
        public decimal? MDTC { get; set; }
        public decimal? MDTCTTL { get; set; }
        public decimal? EVAQty { get; set; }
        public decimal? EVATC { get; set; }
        public decimal? EVATCTTL { get; set; }
        public decimal? CBDTC { get; set; }
        public decimal? CBDMDTC { get; set; }
        public decimal? CBDEVATC { get; set; }

        public virtual Company Locale { get; set; }
        public virtual Company OwnerCompany { get; set; }
        public virtual ICollection<OutsoleItem> OutsoleItem { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<Quotation> Quotation { get; set; }
        public virtual ICollection<Style> Style { get; set; }
    }
}
