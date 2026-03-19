using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersStockGroup
    {
        public OrdersStock OrdersStock { get; set; }
        public IEnumerable<OrdersStockItem> OrdersStockItem { get; set; }
    }
}