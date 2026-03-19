using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class View_MpsDailyMaterialSize
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string DailyNo { get; set; }
        public DateTime DailyDate { get; set; }
        public string OrderNo { get; set; }
        public decimal MpsLiveItemId { get; set; }
        public decimal MpsStyleItemId { get; set; }
        public decimal MpsOrdersItemId { get; set; }
        public decimal SubQty { get; set; }
    }
}
