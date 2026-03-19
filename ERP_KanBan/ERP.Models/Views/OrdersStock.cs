using System;

namespace ERP.Models.Views
{
    public class OrdersStock
    {
        public decimal LocaleId { get; set; }
        public decimal OrderId { get; set; }
        public decimal CTNLabelId { get; set; }
        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public decimal BrandCodeId { get; set; }
        public string BrandCode { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string Customer { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public decimal OrderQty { get; set; }
        public decimal PackingQty { get; set; }
        public int CartonCount { get; set; }
        public decimal StockInCTNS { get; set; }
        public decimal StockOutCTNS { get; set; }
        public decimal StockBalanceCTNS { get; set; }
        public decimal StockBalanceQty { get; set; }
        public DateTime? FirstStockInTime { get; set; }
        public DateTime? LastStockInTime { get; set; }
        public DateTime? FirstStockOutTime { get; set; }
        public DateTime? LastStockOutTime { get; set; }
        public int? DiffDays { get; set; }
    }
}