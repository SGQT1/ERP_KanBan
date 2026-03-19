using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERS_SIZE
    {
        public decimal? OrdersId { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? MinArticleInnerSize { get; set; }
        public decimal? MaxArticleInnerSize { get; set; }
        public string MinDisplaySize { get; set; }
        public string MaxDisplaySize { get; set; }
    }
}
