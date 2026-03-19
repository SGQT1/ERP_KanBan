using System;

namespace ERP.Models.Views.Setting
{
    public class ProjectLast
    {
        public int Id { get; set; }
        public string LastNo { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? OwnerCompanyId { get; set; }
        public decimal? OwnerCustomerId { get; set; }
        public string StoragePlace { get; set; }
        public double? TotalValue { get; set; }
        public decimal? MoneyCodeId { get; set; }
        public string FishGoodsPhotoURL { get; set; }
        public int? FishGoodsPhotoFileId { get; set; }
        public decimal LocaleId { get; set; }
    }
}