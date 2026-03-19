using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BatchProductionCostFirst
    {
        public BatchProductionCostFirst()
        {
            BatchMaterialCostFirst = new HashSet<BatchMaterialCostFirst>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string StyleNo { get; set; }
        public decimal? RefLocaleId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? CostDate { get; set; }
        public decimal? ShippingQty { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? ToolingFund { get; set; }
        public decimal? ToolingCost { get; set; }
        public string CostDollarNameTw { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? MaterialStandardCost { get; set; }
        public decimal? MaterialActualCost { get; set; }
        public decimal? SemiProcessedExpenses { get; set; }
        public decimal? PersonalLaborCost { get; set; }
        public decimal? GroupLaborCost { get; set; }
        public decimal? SemiProcessedLaborCost { get; set; }
        public decimal? DirectManufacturingExpenses { get; set; }
        public decimal? IndirectManufacturingExpenses { get; set; }
        public decimal? FixedSAExpenses { get; set; }
        public decimal? VariableSAExpenses { get; set; }
        public int? Status { get; set; }
        public DateTime? CloseDate { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleNo { get; set; }
        public DateTime? CSD { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? TargetOutput { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal? PersonalLaborCost1 { get; set; }
        public decimal? GroupLaborCost1 { get; set; }
        public decimal? SemiProcessedLaborCost1 { get; set; }
        public decimal? DirectManufacturingExpenses1 { get; set; }
        public decimal? IndirectManufacturingExpenses1 { get; set; }
        public decimal? FixedSAExpenses1 { get; set; }
        public decimal? VariableSAExpenses1 { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal CBDMaterialCost { get; set; }
        public decimal CBDLOH { get; set; }
        public decimal CBDProfit { get; set; }
        public string Season { get; set; }

        public virtual ICollection<BatchMaterialCostFirst> BatchMaterialCostFirst { get; set; }
    }
}
