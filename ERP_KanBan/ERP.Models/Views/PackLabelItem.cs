using System;

namespace ERP.Models.Views
{
    public class PackLabelItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CTNLabelId { get; set; }
        public int GroupBy { get; set; }
        public int PackingType { get; set; }
        public string LabelCode { get; set; }
        public string MinRefDisplaySize { get; set; }
        public string MaxRefDisplaySize { get; set; }
        public decimal SubPackingQty { get; set; }
        public decimal? SubNetWeight { get; set; }
        public decimal? SubGrossWeight { get; set; }
        public decimal? SubMEAS { get; set; }
        public decimal? SubCBM { get; set; }
        public int SeqNo { get; set; }
        public string SubLabelCode { get; set; }
        public string DeptNo { get; set; }

        public decimal OrderId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public DateTime ExFactoryDate { get; set; }

        public string Edition { get; set; }
        public int? IsPrint { get; set; }
        public decimal PackingQty { get; set;}
        public decimal CompanyId { get; set; }
        public decimal? BrandCodeId { get; set; }
        public bool HasStockIn { get; set; }
        public decimal? StockInGrossWeight { get; set; }
        public DateTime? StockInTime { get; set; }

        public bool HasStockOut { get; set; }
        public decimal? StockOutGrossWeight { get; set; }
        public DateTime? StockOutTime { get; set; }
        public string LastLabelCode { get; set; }
    }
}