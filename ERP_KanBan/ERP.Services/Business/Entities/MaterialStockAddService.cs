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
    public class MaterialStockAddService : BusinessService
    {
        private ERP.Services.Entities.MaterialStockAddService MaterialStockAdd { get; }
        public MaterialStockAddService(
            Services.Entities.MaterialStockAddService materialStockAddService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.MaterialStockAdd = materialStockAddService;
        }
        public IQueryable<Models.Views.MaterialStockAdd> Get()
        {
            return MaterialStockAdd.Get().Select(i => new Models.Views.MaterialStockAdd
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                MaterialStockId = i.MaterialStockId,
                StockInQty = i.StockInQty,
                StockOutQty = i.StockOutQty,
                SotckOutCostQty = i.SotckOutCostQty,
                PurPrice = i.PurPrice,
                PurDollarCodeId = i.PurDollarCodeId,
                PurDollarNameTw = i.PurDollarNameTw,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MaterialStockAdd> items)
        {
            MaterialStockAdd.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MaterialStockAdd, bool>> predicate)
        {
            MaterialStockAdd.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MaterialStockAdd> BuildRange(IEnumerable<Models.Views.MaterialStockAdd> items)
        {
            return items.Select(i => new ERP.Models.Entities.MaterialStockAdd
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                MaterialStockId = i.MaterialStockId,
                StockInQty = i.StockInQty,
                StockOutQty = i.StockOutQty,
                SotckOutCostQty = i.SotckOutCostQty,
                PurPrice = i.PurPrice,
                PurDollarCodeId = i.PurDollarCodeId,
                PurDollarNameTw = i.PurDollarNameTw,
            });
        }

 
    }
}