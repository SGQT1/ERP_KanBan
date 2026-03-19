using System;

namespace ERP.Models.Views
{
    public class OrdersStockTaking
    {
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public decimal StockQty { get; set; }
        public int CartonCount { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? ETD { get; set; }
        public decimal? OrderQty { get; set; }
    }
}