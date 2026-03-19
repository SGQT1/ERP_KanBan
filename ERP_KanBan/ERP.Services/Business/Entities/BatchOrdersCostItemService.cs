using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class BatchOrdersCostItemService : BusinessService
    {
        private Services.Entities.BatchMaterialCostService BatchMaterialCost { get; }

        public BatchOrdersCostItemService(
            Services.Entities.BatchMaterialCostService batchMaterialCost, 
            UnitOfWork unitOfWork
        ):base(unitOfWork)
        {
            this.BatchMaterialCost = batchMaterialCost;
        }
        public IQueryable<ERP.Models.Views.BatchOrdersCostItem> Get()
        {
            return BatchMaterialCost.Get().Select(i => new ERP.Models.Views.BatchOrdersCostItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrderNo = i.OrderNo,
                SemiGoods = i.SemiGoods,
                PartNo = i.PartNo,
                PartNameTw = i.PartNameTw,
                MaterialNameTw = i.MaterialNameTw,
                UnitNameTw = i.UnitNameTw,
                StandardUsage = i.StandardUsage,
                ActualUsage = i.ActualUsage,
                PurUnitNameTw = i.PurUnitNameTw,
                PurDollarNameTw = i.PurDollarNameTw,
                PurUnitPrice = i.PurUnitPrice,
                TransRate = i.TransRate,
                ExchangeRate = i.ExchangeRate,
                CostDollarNameTw = i.CostDollarNameTw,
                CostUnitPrice = i.CostUnitPrice,
                StandardCostAmount = i.StandardCostAmount,
                ActualCostAmount = i.ActualCostAmount,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                IsNew = i.IsNew,
                // UsageType = i.UsageType != null ? (int)i.UsageType : i.StandardUsage == 0 && i.ActualUsage > 0 ? 2 : 1,
                // PriceType = i.PriceType == null ? 0 : (int)i.PriceType,
                UsageType = i.UsageType,
                PriceType = i.PriceType,
                Vendor = i.Vendor,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.BatchOrdersCostItem> items)
        {
            BatchMaterialCost.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<Models.Entities.BatchMaterialCost, bool>> predicate)
        {
            BatchMaterialCost.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.BatchMaterialCost> BuildRange(IEnumerable<Models.Views.BatchOrdersCostItem> items)
        {
            return items.Select(item => new Models.Entities.BatchMaterialCost
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                OrderNo = item.OrderNo,
                SemiGoods = item.SemiGoods,
                PartNo = item.PartNo,
                PartNameTw = item.PartNameTw,
                MaterialNameTw = item.MaterialNameTw,
                UnitNameTw = item.UnitNameTw,
                StandardUsage = item.StandardUsage,
                ActualUsage = item.ActualUsage,
                PurUnitNameTw = item.PurUnitNameTw,
                PurDollarNameTw = item.PurDollarNameTw,
                PurUnitPrice = item.PurUnitPrice,
                TransRate = item.TransRate,
                ExchangeRate = item.ExchangeRate,
                CostDollarNameTw = item.CostDollarNameTw,
                CostUnitPrice = item.CostUnitPrice,
                StandardCostAmount = item.StandardCostAmount,
                ActualCostAmount = item.ActualCostAmount,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                IsNew = item.IsNew,
                UsageType = item.UsageType,
                PriceType = item.PriceType,
                Vendor = item.Vendor,
            });
        }
     }
}