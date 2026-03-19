using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MonthMaterialStockItemService : BusinessService
    {
        private ERP.Services.Entities.StockIOYMService StockIOYM { get; set; }

        public MonthMaterialStockItemService(
            ERP.Services.Entities.StockIOYMService stockIOYMService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            StockIOYM = stockIOYMService;
        }

        public IQueryable<Models.Views.MonthMaterialStockItem> Get()
        {
            var items = (
                from m in StockIOYM.Get()
                select new Models.Views.MonthMaterialStockItem
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    IOMonth = m.IOMonth,
                    MaxUpdateTime = m.MaxUpdateTime,
                });
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MonthMaterialStockItem> items)
        {
            StockIOYM.CreateRange(BuildRange(items));
        }
        public void ExecuteSqlCommand(string predicate)
        {
            StockIOYM.ExecuteSqlCommand(predicate);
        }

        public void RemoveRange(Expression<Func<ERP.Models.Entities.StockIOYM, bool>> predicate)
        {
            StockIOYM.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.StockIOYM> BuildRange(IEnumerable<Models.Views.MonthMaterialStockItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.StockIOYM
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId ?? 0,
                IOMonth = item.IOMonth ?? 0,
                MaxUpdateTime = item.MaxUpdateTime,
            });
        }
    }
}