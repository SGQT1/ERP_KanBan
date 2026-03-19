using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSDailyMaterialItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsDailyMaterialId { get; set; }
        public decimal SubQty { get; set; }
        public decimal UnitUsage { get; set; }
        public decimal SubUsage { get; set; }
        public decimal PreSubUsage { get; set; }
        public decimal MpsOrdersItemId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string ArticeSize { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public decimal TotalSubQty { get; set; }
        public decimal PlanSubQty { get; set; }
    }
}
