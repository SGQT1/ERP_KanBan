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
    public class MonthMaterialStockItemInService : BusinessService
    {
        private ERP.Services.Entities.StockIOMonthInService StockIOMonthIn { get; set; }

        public MonthMaterialStockItemInService(
            ERP.Services.Entities.StockIOMonthInService stockIOMonthInService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            StockIOMonthIn = stockIOMonthInService;
        }

        public IQueryable<Models.Views.MonthMaterialStockItemIn> Get()
        {
            var items = (
                from m in StockIOMonthIn.Get()
                select new Models.Views.MonthMaterialStockItemIn
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    IOMonth = m.IOMonth,
                    MATERIALSTOCKId = m.MATERIALSTOCKId,
                    IOQty = m.IOQty,
                    IOAmount = m.IOAmount,
                    SourceType = m.SourceType
                });
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MonthMaterialStockItemIn> items)
        {
            StockIOMonthIn.CreateRange(BuildRange(items));
        }
        public void ExecuteSqlCommand(string predicate)
        {
            StockIOMonthIn.ExecuteSqlCommand(predicate);
        }

        public void RemoveRange(Expression<Func<ERP.Models.Entities.StockIOMonthIn, bool>> predicate)
        {
            StockIOMonthIn.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.StockIOMonthIn> BuildRange(IEnumerable<Models.Views.MonthMaterialStockItemIn> items)
        {
            return items.Select(item => new ERP.Models.Entities.StockIOMonthIn
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                IOMonth = item.IOMonth,
                MATERIALSTOCKId = item.MATERIALSTOCKId,
                IOQty = item.IOQty,
                IOAmount = item.IOAmount,
                SourceType = item.SourceType
            });
        }
    }
}