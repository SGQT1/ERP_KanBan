using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class BatchOrdersCost
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
        public decimal RefLocaleId { get; set; }
        public DateTime CostDate { get; set; }
        public decimal? OrderQty { get; set; }
        public DateTime? ShipmentDate { get; set; }     // public DateTime? CloseDate { get; set; }
        public decimal ShipmentQty { get; set; }        // public decimal ShippingQty { get; set; }
        public decimal ShipmentUnitPrice { get; set; }  // public decimal UnitPrice { get; set; }
        public string ShipmentCurrency { get; set; }    // public string DollarNameTw { get; set; }
        public decimal ExchangeRate { get; set; }
        public string CostCurrency { get; set; }    // public string CostDollarNameTw { get; set; }      
        public decimal? ToolingFund { get; set; }
        public decimal? ToolingCost { get; set; }
        public decimal MaterialStandardCost { get; set; }
        public decimal MaterialActualCost { get; set; }
        public decimal? AvgMaterialStandardCost { get; set; }
        public decimal? AvgMaterialActualCost { get; set; }

        public int Status { get; set; }
        public DateTime? ExchangeDate { get; set; }
        public decimal? PMCostRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public decimal BatchMaterialOrdersId { get; set; }
        public decimal BatchMaterialOrdersLocaleId { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleNo { get; set; }
        public DateTime? CSD { get; set; }
        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public string Brand { get; set; }
        public string Customer { get; set; }
        public DateTime ETD { get; set; }
        public int ProductType { get; set; }
        public int OrderType { get; set; }
        public int IsMRP { get; set; }
        public int IsCost { get; set; }

        public decimal? NetPrice { get; set; }
        public decimal? CostFee1 { get; set; }
        public decimal? CostFee2 { get; set; }
        public decimal? CostFee3 { get; set; }
        public decimal? SMCostRate { get; set; }
        public string DisplaySize { get; set; }
        public decimal? RMBExchange { get; set; }

        // public decimal SemiProcessedExpenses { get; set; }
        // public decimal PersonalLaborCost { get; set; }
        // public decimal GroupLaborCost { get; set; }
        // public decimal SemiProcessedLaborCost { get; set; }
        // public decimal DirectManufacturingExpenses { get; set; }
        // public decimal IndirectManufacturingExpenses { get; set; }
        // public decimal FixedSAExpenses { get; set; }
        // public decimal VariableSAExpenses { get; set; }
        // public decimal? PersonalLaborCost1 { get; set; }
        // public decimal? GroupLaborCost1 { get; set; }
        // public decimal? SemiProcessedLaborCost1 { get; set; }
        // public decimal? DirectManufacturingExpenses1 { get; set; }
        // public decimal? IndirectManufacturingExpenses1 { get; set; }
        // public decimal? FixedSAExpenses1 { get; set; }
        // public decimal? VariableSAExpenses1 { get; set; }       
        // public decimal TargetOutput { get; set; }

    }
}
