using System;
using System.Collections.Generic;

namespace ERP.Models.KanBan.Views
{
    public class OrdersStockStyleGroup
    {
        public List<OrdersStockStyleTaking> OrdersStockStyleTaking { get; set; }
        public List<OrdersStockStyleByOrderRate> OrdersStockStyleByOrderRate { get; set; }
        public List<OrdersStockStyleByOrderQtyRate> OrdersStockStyleByOrderQtyRate { get; set; }
        public List<OrdersStockStyleByStockQtyRate> OrdersStockStyleByStockQtyRate { get; set; }
    }
    public class OrdersStockStyleTaking
    {
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }

        public decimal OrderCount { get; set; }
        public decimal TotalOrderCount { get; set; }
        public decimal OrderCountRate { get; set; }

        public decimal? OrderQty { get; set; }
        public decimal TotalOrderQty { get; set; }
        public decimal OrderQtyRate { get; set; }

        public decimal StockQty { get; set; }
        public decimal TotalStockQty { get; set; }
        public decimal StockQtyRate { get; set; }
    }
    public class OrdersStockStyleByOrderRate
    {
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public decimal OrderCount { get; set; }
        public decimal TotalOrderCount { get; set; }
        public decimal OrderCountRate { get; set; }
    }
    public class OrdersStockStyleByOrderQtyRate
    {
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public decimal OrderQty { get; set; }
        public decimal TotalOrderQty { get; set; }
        public decimal OrderQtyRate { get; set; }
    }
    public class OrdersStockStyleByStockQtyRate
    {
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public decimal StockQty { get; set; }
        public decimal TotalStockQty { get; set; }
        public decimal StockQtyRate { get; set; }
    }
}