using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class PurOrdersItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal PurBatchId { get; set; }
        public decimal OrdersId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Brand { get; set; }

        public string BatchNo { get; set; }
        public DateTime BatchDate { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderQty { get; set; }
        public decimal CompanyId { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? OWD { get; set; } 
        public decimal OrderType { get; set; }
        public decimal ProductType { get; set; }
        public decimal RefLocaleId { get; set; } // Orders LocaleId
        public decimal? Status { get; set; }
        public decimal? doMRP { get; set; } // Orders LocaleId
        public string CustomerOrderNo { get; set; }
        public string Customer { get; set; }
        public string ShoeName { get; set; }
    }
}
