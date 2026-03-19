using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Outsole
    {
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
        public string OutsoleDesc { get; set; }
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

        public string OwnerCustomer { get; set; }
        public string MoneyCode { get; set; }
    }
}
