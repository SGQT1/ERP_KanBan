using System;

namespace ERP.Models.Views.Setting
{
    public class ProjectOutsole
    {
        public int Id { get; set; }
        public string OutsoleNo { get; set; }
        public string Remark { get; set; }
        public string OutsolePhotoURL { get; set; }
        public int? OutsolePhotoFileId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? OwnerCompanyId { get; set; }
        public decimal? OwnerCustomerId { get; set; }
        public string StoragePlace { get; set; }
        public double? TotalValue { get; set; }
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
        public int? MDPhotoFileId { get; set; }
        public string EVAPhotoURL { get; set; }
        public int? EVAPhotoFileId { get; set; }
        public string TrademarkPhotoURL { get; set; }
        public int? TrademarkPhotoFileId { get; set; }
        public int? TCQty { get; set; }
        public double? TC { get; set; }
        public double? TCTTL { get; set; }
        public int? MDQty { get; set; }
        public double? MDTC { get; set; }
        public double? MDTCTTL { get; set; }
        public int? EVAQty { get; set; }
        public double? EVATC { get; set; }
        public double? EVATCTTL { get; set; }
        public double? CBDTC { get; set; }
        public double? CBDMDTC { get; set; }
        public double? CBDEVATC { get; set; }
        public int BrandId { get; set; }
    }
}