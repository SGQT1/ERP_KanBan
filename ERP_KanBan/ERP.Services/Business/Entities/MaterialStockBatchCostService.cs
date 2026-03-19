using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MaterialStockBatchCostService : BusinessService
    {
        private ERP.Services.Entities.MaterialStockBatchCostService MaterialStockBatchCost { get; }

        public MaterialStockBatchCostService(
            ERP.Services.Entities.MaterialStockBatchCostService materialStockBatchCostService, 
            UnitOfWork unitOfWork
        ):base(unitOfWork)
        {
            MaterialStockBatchCost = materialStockBatchCostService;
        }
        public IQueryable<Models.Views.MaterialStockBatchCost> Get()
        {
            return MaterialStockBatchCost.Get().Select(i => new Models.Views.MaterialStockBatchCost
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialName = i.MaterialName,
                OrderNo = i.OrderNo,
                IOMonth = i.IOMonth,
                CostType = i.CostType,
                IOType = i.IOType,
                PCLUnitNameTw = i.PCLUnitNameTw,
                StockDollarNameTw = i.StockDollarNameTw,
                IOQty = i.IOQty,
                IOAmount = i.IOAmount,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MaterialStockBatchCost> items)
        {
            MaterialStockBatchCost.CreateRange(BuildRange(items));
        }

        public void RemoveRange(Expression<Func<ERP.Models.Entities.MaterialStockBatchCost, bool>> predicate)
        {
            MaterialStockBatchCost.RemoveRange(predicate);
        }

        public void ExecuteSqlCommand(string predicate)
        {
            MaterialStockBatchCost.ExecuteSqlCommand(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MaterialStockBatchCost> BuildRange(IEnumerable<Models.Views.MaterialStockBatchCost> items)
        {
            return items.Select(item => new ERP.Models.Entities.MaterialStockBatchCost
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MaterialName = item.MaterialName,
                OrderNo = item.OrderNo,
                IOMonth = item.IOMonth,
                CostType = item.CostType,
                IOType = item.IOType,
                PCLUnitNameTw = item.PCLUnitNameTw,
                StockDollarNameTw = item.StockDollarNameTw,
                IOQty = item.IOQty,
                IOAmount = item.IOAmount,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}