using System;

namespace ERP.Models.Views
{
    public class OrdersStockItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrderId { get; set; }
        public decimal CTNLabelId { get; set; }
        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public decimal BrandCodeId { get; set; }
        public string BrandCode { get; set; }
        public string OrderNo { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string Customer { get; set; }
        public int SeqNo { get; set; }
        public int CartonCount { get; set; }
        public int StockInCTNS { get; set; }
        public string LabelCode { get; set; }
        public string MinOrderSize { get; set; }
        public string MaxOrderSize { get; set; }
        public decimal OrderQty { get; set; }
        public decimal PackingQty { get; set; }
        public decimal SubPackingQty { get; set; }
        public decimal? SubNetWeight { get; set; }
        public decimal? SubGrossWeight { get; set; }
        public decimal? SubMEAS { get; set; }
        public decimal? SubCBM { get; set; }
        public decimal? StockInGrossWeight { get; set; }
        public DateTime? StockInTime { get; set; }
        public decimal? StockOutGrossWeight { get; set; }
        public DateTime? StockOutTime { get; set; }

        public string GBSPOReferenceNo { get; set; }
        public double? SubGrossUpperWeight { get; set; }
        public double? SubGrossLowerWeight { get; set; }
        public string SubLabelCode { get; set; }
        public string CustomerOrderNo { get; set; }
    }
}