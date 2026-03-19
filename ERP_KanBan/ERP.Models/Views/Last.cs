using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Last
    {
        public decimal Id { get; set; }
        public string LastNo { get; set; }
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

        public string OwnerCustomer { get; set; }
        public string MoneyCode { get; set; }
    }
}
