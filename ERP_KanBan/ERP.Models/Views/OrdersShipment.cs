using System;

namespace ERP.Models.Views
{
    public class OrdersShipment
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string Customer { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public string Brand { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal CurrencyId { get; set; }
        public string Currency { get; set; }
        public decimal OrderQty { get; set; }
        public decimal? ShippingQty { get; set; }
        public decimal? ShortageQty { get; set; }
        public decimal? ShippingAmount { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string ShippingMonth { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? OBDate { get; set; }
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
        public string ProductType { get; set; }
        public decimal ProductTypeId { get; set; }

        public bool IsCFM { get; set; }
        public decimal? ARId { get; set; }
        public decimal Discount { get; set; }
        public decimal ToolingCost { get; set; }
        public decimal? OutsolePrice { get; set; }
        public decimal? MidsolePrice { get; set; }
        public decimal? ToolingOtherPrice { get; set; }
        public decimal? ToolingTotalPrice { get; set; }
        public decimal OtherCharge { get; set; } //出貨其他應收1 OtherCharge1
        public decimal CLB { get; set; } //出貨其他應收2 OtherCharge2
        public decimal FeedbackFund { get; set; } //出貨調整應收 ChargeAdj
        public decimal OtherCost { get; set; } //發票其他應收
        public decimal? LessCharge { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? ARSubTotal { get; set; } // 出貨小計 Shipment sub total 出貨小計 + Invoice other cost 發票其他加價
        public decimal? ARTotal { get; set; }
        public decimal? ARReceived { get; set; }
        public decimal AROff { get; set; }
        public decimal ARPaid { get; set; }
        public decimal? FactoryPriceIntel { get; set; }
        public decimal? FactoryAmount { get; set; }
        public decimal? InvoicePriceIntel { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public DateTime? PaidDate { get; set; }
        public string InvoiceRemark { get; set; } // Remark
        public string PaymentDiffDesc { get; set; }
        public decimal RequestLocaleId { get; set; }

        public decimal ShipmentId { get; set; }
        public decimal InvoiceId { get; set; }
        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }

        public decimal? TransitTypeId { get; set; }
        public string TransitType { get; set; }
    }
}