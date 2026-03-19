using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Shipping
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal CustomerId { get; set; }
        public string CustomerNameTw { get; set; }
        public decimal? ShippingPortId { get; set; }
        public string ShippingPortName { get; set; }
        public decimal? TargetPortId { get; set; }
        public string TargetPortName { get; set; }
        public DateTime OBDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public decimal ExportQty { get; set; }
        public int? PayType { get; set; }
        public string PayTypeDesc { get; set; }
        public decimal PayDollarCodeId { get; set; }
        public string PayDollarCodeDesc { get; set; }
        public decimal OtherCost { get; set; }
        public decimal ARTotal { get; set; }
        public decimal ARId { get; set; }
        public string FileDesc1 { get; set; }
        public string FileURL1 { get; set; }
        public string FileDesc2 { get; set; }
        public string FileURL2 { get; set; }
        public string FileDesc3 { get; set; }
        public string FileURL3 { get; set; }
        public string FileDesc4 { get; set; }
        public string FileURL4 { get; set; }
        public string FileDesc5 { get; set; }
        public string FileURL5 { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public DateTime? ARReceivedDate { get; set; }
        public int? TransitType { get; set; }
        public decimal? ARReceived { get; set; }
        public int? Lock { get; set; }
        public string Vessel { get; set; }
        public string ProductType { get; set; }
        public string FOBType { get; set; }
        public string HeadInfo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string TailInfo { get; set; }
        public string TitleInfo { get; set; }
        public string BankInfo { get; set; }
        public string PaymentRemark { get; set; }
        public DateTime? DocumentDispatchDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public DateTime? CHODate { get; set; }
        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}
