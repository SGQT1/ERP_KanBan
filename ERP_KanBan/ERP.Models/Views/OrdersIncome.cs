using System;

namespace ERP.Models.Views
{
    public class OrdersIncome
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string Brand { get; set; }
        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public string OrderNo { get; set; }
        public string Customer { get; set; }
        public string CustomerOrderNo { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal OrderQty { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? OPD { get; set; }
        public DateTime? OWD { get; set; }
        public string Season { get; set; }
        public string ProductType { get; set; }
        public decimal ProductTypeId { get; set; }
        public string OrderType { get; set; }
        public decimal OrderTypeId { get; set; }
        public string Currency { get; set; }
        public decimal? InvoicePrice { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? FactoryPrice { get; set; }
        public decimal? FactoryAmount { get; set; }
    }
}