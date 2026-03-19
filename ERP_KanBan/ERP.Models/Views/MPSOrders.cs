using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
//拖外訂單
namespace ERP.Models.Views
{
    public partial class MPSOrders
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderQty { get; set; }
        public decimal Qty { get; set; }
        public decimal MpsArticleId { get; set; }
        public decimal ProcessSetId { get; set; }
        public string StyleNo { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public decimal IncreaseRate { get; set; }
        public DateTime ETD { get; set; }
        public DateTime CSD { get; set; }
        public int BaseOn { get; set; }
        public string CustomerNameTw { get; set; }
        public int ProcessType { get; set; }

        public decimal ArticleId { get; set; }
        public string ArticleNo { get; set; }
        public decimal OrdersId { get; set; }
        public int HasPlan { get; set; }
    }
}
