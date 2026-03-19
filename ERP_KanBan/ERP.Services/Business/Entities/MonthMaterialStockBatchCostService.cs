using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MonthMaterialStockBatchCostService : BusinessService
    {
        private ERP.Services.Entities.MonthMaterialStockBatchCostService MonthMaterialStockBatchCost { get; set; }

        public MonthMaterialStockBatchCostService(
            ERP.Services.Entities.MonthMaterialStockBatchCostService monthMaterialStockBatchCostService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MonthMaterialStockBatchCost = monthMaterialStockBatchCostService;
        }

        public IQueryable<Models.Views.MonthMaterialStockBatchCost> Get()
        {
            var items = (
                from i in MonthMaterialStockBatchCost.Get()
                select new Models.Views.MonthMaterialStockBatchCost
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
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MonthMaterialStockBatchCost> items)
        {
            MonthMaterialStockBatchCost.CreateRange(BuildRange(items));
        }

        public void RemoveRange(Expression<Func<ERP.Models.Entities.MonthMaterialStockBatchCost, bool>> predicate)
        {
            MonthMaterialStockBatchCost.RemoveRange(predicate);
        }

        public void ExecuteSqlCommand(string predicate)
        {
            MonthMaterialStockBatchCost.ExecuteSqlCommand(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MonthMaterialStockBatchCost> BuildRange(IEnumerable<Models.Views.MonthMaterialStockBatchCost> items)
        {
            return items.Select(item => new ERP.Models.Entities.MonthMaterialStockBatchCost
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
