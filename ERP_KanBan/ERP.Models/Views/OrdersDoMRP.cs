using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class OrdersDoMRP
    {
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public int MRPType { get; set; }
        public decimal ArticleId { get; set; }
        public string ArticleNo { get; set; }
        public decimal StyleId { get; set; }
        public string StyleNo { get; set; }
        public int OrderType { get; set; }
        public int ProductType { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CompanyId { get; set; }
        public int Status { get; set; }
        public int doMRP { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? OWD { get; set; }
        public DateTime ETC { get; set; }
        public decimal OrderQty { get; set; }
        public int CurrentOrderVersion { get; set; }
        public int? MRPOrderVersion { get; set; }
        public string ShoeName { get; set; }
        public int IsApproved { get; set; }
        public decimal? ARLocaleId { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Season { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
