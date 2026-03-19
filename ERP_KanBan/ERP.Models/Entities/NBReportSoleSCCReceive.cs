using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class NBReportSoleSCCReceive
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string PONo { get; set; }
        public string Status { get; set; }
        public string POStatus { get; set; }
        public string POType { get; set; }
        public string OrderType { get; set; }
        public string Season { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string FactoryCode { get; set; }
        public string AgentCompanyName { get; set; }
        public string ShipFromLocation { get; set; }
        public string NBPartNo { get; set; }
        public string NBColorKey { get; set; }
        public string SupplierLineItemNo { get; set; }
        public string UOM { get; set; }
        public decimal? Quantity { get; set; }
        public string Length { get; set; }
        public string RXFD { get; set; }
        public string RETA { get; set; }
        public string ShipMode { get; set; }
        public string PackType { get; set; }
        public string FactoryPOPrintFileName { get; set; }
        public string NBMaterialDescription { get; set; }
        public string SupplierMaterialDescription { get; set; }
        public string StyleNo { get; set; }
        public string CustomerPONo { get; set; }
        public decimal? ReceiptQuantity { get; set; }
        public string ReceiptDate { get; set; }
        public decimal? ReceiptQuantityQC { get; set; }
        public decimal? RejectedQuantity { get; set; }
        public string AdditionInstruction { get; set; }
        public string NBWidth { get; set; }
        public string ShoesDestination { get; set; }
        public string Q1 { get; set; }
        public string Q1_5 { get; set; }
        public string Q2 { get; set; }
        public string Q2_5 { get; set; }
        public string Q3 { get; set; }
        public string Q3_5 { get; set; }
        public string Q4 { get; set; }
        public string Q4_5 { get; set; }
        public string Q5 { get; set; }
        public string Q5_5 { get; set; }
        public string Q6 { get; set; }
        public string Q6_5 { get; set; }
        public string Q7 { get; set; }
        public string Q7_5 { get; set; }
        public string Q8 { get; set; }
        public string Q8_5 { get; set; }
        public string Q9 { get; set; }
        public string Q9_5 { get; set; }
        public string Q10 { get; set; }
        public string Q10_5 { get; set; }
        public string Q11 { get; set; }
        public string Q11_5 { get; set; }
        public string Q12 { get; set; }
        public string Q12_5 { get; set; }
        public string Q13 { get; set; }
        public string Q13_5 { get; set; }
        public string Q14 { get; set; }
        public string Q14_5 { get; set; }
        public string Q15 { get; set; }
        public string Q15_5 { get; set; }
        public string Q16 { get; set; }
        public string Q16_5 { get; set; }
        public string Q17 { get; set; }
        public string Q17_5 { get; set; }
        public string Q18 { get; set; }
        public string Q18_5 { get; set; }
        public string Q19 { get; set; }
        public string Q19_5 { get; set; }
        public string Q20 { get; set; }
        public string PODate { get; set; }
        public string SupplieRemark { get; set; }
        public string FactoryRemark { get; set; }
        public decimal? POItemId { get; set; }
        public decimal? POItemLocaleId { get; set; }
    }
}
