using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_BATCHPRODUCTIONCOSTFirst
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? CostDate { get; set; }
        public decimal? ShippingQty { get; set; }
        public decimal? OrderQty { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? InvoiceUnitPrice { get; set; }
        public decimal? ToolingFund { get; set; }
        public decimal? ToolingCost { get; set; }
        public decimal? FactoryUnitPrice { get; set; }
        public string CostDollarNameTw { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? SemiProcessedExpenses { get; set; }
        public decimal? MaterialStandardCost { get; set; }
        public decimal? PersonalLaborCost { get; set; }
        public decimal? GroupLaborCost { get; set; }
        public decimal? SemiProcessedLaborCost { get; set; }
        public decimal? DirectManufacturingExpenses { get; set; }
        public decimal? IndirectManufacturingExpenses { get; set; }
        public decimal? FixedSAExpenses { get; set; }
        public decimal? VariableSAExpenses { get; set; }
        public string CloseDate { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleNo { get; set; }
        public decimal? NetRevenue { get; set; }
        public decimal? MaterialCostRate { get; set; }
        public decimal? MaterialStandardCostRate { get; set; }
        public decimal? OLEP { get; set; }
        public decimal? UnitOLEP { get; set; }
        public decimal? GrossProfit { get; set; }
        public decimal? UnitGrossProfit { get; set; }
        public decimal? UnitLEP { get; set; }
        public decimal? NetProfit { get; set; }
        public decimal? UnitNetProfit { get; set; }
        public decimal? GrossProfitRate { get; set; }
        public decimal? NetProfitRate { get; set; }
        public int? Status { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public DateTime? CSD { get; set; }
        public string ArticleNo { get; set; }
        public decimal? TargetOutput { get; set; }
        public decimal CBDMaterialCost { get; set; }
        public decimal CBDLOH { get; set; }
        public decimal CBDProfit { get; set; }
        public string BrandTw { get; set; }
        public string Season { get; set; }
    }
}
