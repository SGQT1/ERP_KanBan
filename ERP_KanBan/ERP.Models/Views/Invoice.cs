using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Invoice
    {
        public int Lock { get; set; }
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal CustomerId { get; set; }
        public string Customer { get; set; } // CustomerNameTw
        public decimal PayDollarCodeId { get; set; }
        public string PayDollarCodeDesc { get; set; }
        public DateTime? CHODate { get; set; } //交倉日 
        public DateTime? DocumentDispatchDate { get; set; } //寄件日
        public decimal OtherCost { get; set; }
        public decimal ExportQty { get; set; } // SaleQty Total
        public DateTime OBDate { get; set; }
        public DateTime? PaymentDueDate { get; set; } //到期日
        public decimal ARTotal { get; set; }
        public decimal ARId { get; set; }
        public decimal? ARReceived { get; set; }
        public string Remark { get; set; }
        public decimal? CompanyId { get; set; }
        public string Company { get; set; }
        public decimal? BrandId { get; set; }
        public string Brand { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
        // public decimal ShippingPortId { get; set; }
        // public string ShippingPortName { get; set; }
        // public decimal TargetPortId { get; set; }
        // public string TargetPortName { get; set; }
        // public DateTime? ArrivalDate { get; set; }
        // public int PayType { get; set; }
        // public string PayTypeDesc { get; set; }
        // public DateTime? ARReceivedDate { get; set; }
        // public int? TransitType { get; set; }
        // public string Vessel { get; set; }
        // public string ProductType { get; set; }
        // public string FOBType { get; set; }
        // public string HeadInfo { get; set; }
        // public DateTime? DeliveryDate { get; set; }
        // public string TailInfo { get; set; }
        // public string TitleInfo { get; set; }
        // public string BankInfo { get; set; }
        // public string PaymentRemark { get; set; }
    }
}