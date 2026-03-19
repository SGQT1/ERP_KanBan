using System.Linq;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities.View
{
    public class BatchProductionCostService : BusinessService
    {
        private Services.Entities.BatchProductionCostService BatchProductionCost { get; }

        public BatchProductionCostService(Services.Entities.BatchProductionCostService batchProductionCostService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.BatchProductionCost = batchProductionCostService;
        }
        public IQueryable<ERP.Models.Views.View.BatchProductionCost> Get()
        {
            return BatchProductionCost.Get().Select(i => new ERP.Models.Views.View.BatchProductionCost
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                RefLocaleId = i.RefLocaleId,
                OrderNo = i.OrderNo,
                CostDate = i.CostDate,
                ShippingQty = i.ShippingQty,
                DollarNameTw = i.DollarNameTw,
                // InvoiceUnitPrice = i.InvoiceUnitPrice,
                ToolingFund = i.ToolingFund,
                ToolingCost = i.ToolingCost,
                // FactoryUnitPrice = i.FactoryUnitPrice,
                CostDollarNameTw = i.CostDollarNameTw,
                ExchangeRate = i.ExchangeRate,
                SemiProcessedExpenses = i.SemiProcessedExpenses,
                MaterialStandardCost = i.MaterialStandardCost,
                MaterialActualCost = i.MaterialActualCost,
                PersonalLaborCost = i.PersonalLaborCost,
                GroupLaborCost = i.GroupLaborCost,
                SemiProcessedLaborCost = i.SemiProcessedLaborCost,
                DirectManufacturingExpenses = i.DirectManufacturingExpenses,
                IndirectManufacturingExpenses = i.IndirectManufacturingExpenses,
                FixedSAExpenses = i.FixedSAExpenses,
                VariableSAExpenses = i.VariableSAExpenses,
                // CloseDate = i.CloseDate,
                StyleNo = i.StyleNo,
                ShoeName = i.ShoeName,
                OutsoleNo = i.OutsoleNo,
                // NetRevenue = i.NetRevenue,
                // MaterialCostRate = i.MaterialCostRate,
                // MaterialStandardCostRate = i.MaterialStandardCostRate,
                // OLEP = i.OLEP,
                // UnitOLEP = i.UnitOLEP,
                // GrossProfit = i.GrossProfit,
                // UnitGrossProfit = i.UnitGrossProfit,
                // UnitLEP = i.UnitLEP,
                // NetProfit = i.NetProfit,
                // UnitNetProfit = i.UnitNetProfit,
                // GrossProfitRate = i.GrossProfitRate,
                // NetProfitRate = i.NetProfitRate,
                Status = i.Status,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                CSD = i.CSD,
                OrderQty = i.OrderQty,
                TargetOutput = i.TargetOutput,
                // CompanyId = i.CompanyId,
                // BrandNameTw = i.BrandNameTw,
                PersonalLaborCost1 = i.PersonalLaborCost1,
                GroupLaborCost1 = i.GroupLaborCost1,
                SemiProcessedLaborCost1 = i.SemiProcessedLaborCost1,
                DirectManufacturingExpenses1 = i.DirectManufacturingExpenses1,
                IndirectManufacturingExpenses1 = i.IndirectManufacturingExpenses1,
                FixedSAExpenses1 = i.FixedSAExpenses1,
                VariableSAExpenses1 = i.VariableSAExpenses1,
                // GrossProfit1 = i.GrossProfit1,
                // UnitGrossProfit1 = i.UnitGrossProfit1,
                // UnitLEP1 = i.UnitLEP1,
                // NetProfit1 = i.NetProfit1,
                // UnitNetProfit1 = i.UnitNetProfit1,
                // GrossProfitRate1 = i.GrossProfitRate1,
                // NetProfitRate1 = i.NetProfitRate1,
                // ArticleNo = i.ArticleNo,
                // RMBExchangeRate = i.RMBExchangeRate,

            });
        }
    }
}