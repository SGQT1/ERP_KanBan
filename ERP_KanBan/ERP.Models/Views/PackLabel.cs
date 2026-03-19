using System;

namespace ERP.Models.Views
{
    public class PackLabel
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? PLLocaleId { get; set; }
        public string OrderNo { get; set; }
        public DateTime ExFactoryDate { get; set; }
        public int TransitType { get; set; }
        public string TargetPort { get; set; }
        public string Customer { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public int? ProductType { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public string CustomerOrderNo { get; set; }
        public string ColorDesc { get; set; }
        public string OutsoleColorDescTW { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal PackingQty { get; set; }
        public int CTNS { get; set; }
        public DateTime? CloseDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int? IsPrint { get; set; }
        public int CNoFrom { get; set; }

        public decimal OrderId { get; set; }
        public string Edition { get; set; }
        public decimal? CompanyId { get; set; }
        public decimal? BrandCodeId { get; set; }
        public bool IsStockOut { get; set; }
        public string Company { get; set; }
        public string PLLocale { get; set; }
        public string Transit { get; set; }
        public string BrandCode { get; set; }
    }
}