using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PUR_ORDER_2
    {
        public decimal Id { get; set; }
        public string OrderNo { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrderQty { get; set; }
        public decimal CompanyId { get; set; }
        public decimal StyleId { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public DateTime CSD { get; set; }
        public DateTime ETD { get; set; }
        public int FilterId { get; set; }
        public int Status { get; set; }
        public int ProductType { get; set; }
        public int OrderType { get; set; }
        public int OrderVersion { get; set; }
        public decimal? ARLocaleId { get; set; }
        public decimal? ParentOrdersId { get; set; }
        public decimal? RefOrdersLocaleId { get; set; }
        public string ArticleNo { get; set; }
    }
}
