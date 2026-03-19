using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class NBReport
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string PONo { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public string PODate { get; set; }
        public string FreightTerms { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentMethod { get; set; }
        public string Via { get; set; }
        public string FactoryCode { get; set; }
        public string FactoryAddress1 { get; set; }
        public string FactoryAddress2 { get; set; }
        public string FactoryAddress3 { get; set; }
        public string FactoryAddress4 { get; set; }
        public string FactoryPhone { get; set; }
        public string FactoryFax { get; set; }
        public string BillToCompanyName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToAddress3 { get; set; }
        public string BillToAddress4 { get; set; }
        public string BillToPhone { get; set; }
        public string BillToFax { get; set; }
        public string ConsigneeCompanyName { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeAddress3 { get; set; }
        public string ConsigneeAddress4 { get; set; }
        public string ConsigneePhone { get; set; }
        public string ConsigneeFax { get; set; }
        public string AgentCompanyName { get; set; }
        public string AgentAddress1 { get; set; }
        public string AgentAddress2 { get; set; }
        public string AgentAddress3 { get; set; }
        public string AgentAddress4 { get; set; }
        public string AgentPhone { get; set; }
        public string AgentFax { get; set; }
        public string AgentAttn { get; set; }
        public string ShipToCompanyName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToAddress4 { get; set; }
        public string ShipToPhone { get; set; }
        public string ShipToFax { get; set; }
        public string ShipFromLocation { get; set; }
        public string NotifyPartyCompanyName { get; set; }
        public string NotifyPartyAddress1 { get; set; }
        public string NotifyPartyAddress2 { get; set; }
        public string NotifyPartyAddress3 { get; set; }
        public string NotifyPartyAddress4 { get; set; }
        public string NotifyPartyPhone { get; set; }
        public string NotifyPartyFax { get; set; }
        public string NotifyPartyAttn { get; set; }
        public string NBPartNo { get; set; }
        public string NBColorKey { get; set; }
        public string SupplierLineItemNo { get; set; }
        public string SupplierMaterialNo { get; set; }
        public string NBMaterialDescription { get; set; }
        public string SupplierMaterialDescription { get; set; }
        public string UOM { get; set; }
        public decimal? Quantity { get; set; }
        public string RXFD { get; set; }
        public string RETA { get; set; }
        public string ShipMode { get; set; }
        public string SpecialTreatment { get; set; }
        public string AdditionInstruction { get; set; }
        public string StyleNo { get; set; }
        public string CustomerPONo { get; set; }
        public decimal? ReceiptQuantity { get; set; }
        public string ReceiptDate { get; set; }
        public decimal? RejectedQuantity { get; set; }
        public decimal? RejectQuantityForColorIssues { get; set; }
        public decimal? RejectQuantityForCosmeticIssues { get; set; }
        public string FactoryPOPrintFileName { get; set; }
        public decimal? POItemId { get; set; }
        public decimal? POItemLocaleId { get; set; }
        public decimal? ReceiptQuantityQC { get; set; }
    }
}
