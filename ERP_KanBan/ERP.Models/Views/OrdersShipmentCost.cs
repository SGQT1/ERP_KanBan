using System;

namespace ERP.Models.Views
{
    public class OrdersShipmentCost
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }

        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public string Brand { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal AvgPrice { get; set; }
        public string Currency { get; set; }
        public decimal OrderQty { get; set; }
        public decimal? ShippingQty { get; set; }
        public decimal? ShortageQty { get; set; }

        public DateTime? ShippingDate { get; set; }
        public string ShippingMonth { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? OPD { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? OWD { get; set; }
        public DateTime? OWRD { get; set; }
        public DateTime? RSD { get; set; }
        public string Season { get; set; }
        public string Last { get; set; }
        public string Outsole { get; set; }

        public decimal? ShippingAmount { get; set; }
        public decimal FeedbackFund { get; set; } //出貨調整應收 ChargeAdj
        public decimal OtherCharge { get; set; } //出貨其他應收1 OtherCharge1
        public decimal CLB { get; set; } //出貨其他應收2 OtherCharge2
        public decimal? LessCharge { get; set; }
        public decimal OtherCost { get; set; } //發票其他應收
        public decimal? OutsolePrice { get; set; }
        public decimal? MidsolePrice { get; set; }
        public decimal? ToolingOtherPrice { get; set; }
        public decimal? ToolingTotalPrice { get; set; }
        
        public decimal? ARTotal { get; set; }
        public decimal? ARSubTotal { get; set; } // 出貨小計 Shipment sub total 出貨小計 + Invoice other cost 發票其他加價

        public decimal? SMCostRate { get; set; }
        public decimal? PMCostRate { get; set; }
        public decimal? SMCost { get; set; }
        public decimal? PMCost { get; set; }
        public DateTime? CostDate { get; set; }
        public string InvoiceNo { get; set; }
        public string RefOrderNo { get; set; }
        public string RefStyle { get; set; }
    }
}