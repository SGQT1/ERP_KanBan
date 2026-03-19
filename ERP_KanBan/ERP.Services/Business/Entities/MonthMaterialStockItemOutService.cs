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
    public class MonthMaterialStockItemOutService : BusinessService
    {
        private ERP.Services.Entities.StockIOMonthOutService StockIOMonthOut { get; set; }

        public MonthMaterialStockItemOutService(
            ERP.Services.Entities.StockIOMonthOutService stockIOMonthOutService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            StockIOMonthOut = stockIOMonthOutService;
        }

        public IQueryable<Models.Views.MonthMaterialStockItemOut> Get()
        {
            var items = (
                from m in StockIOMonthOut.Get()
                select new Models.Views.MonthMaterialStockItemOut
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

        public void CreateRange(IEnumerable<Models.Views.MonthMaterialStockItemOut> items)
        {
            StockIOMonthOut.CreateRange(BuildRange(items));
        }
        public void ExecuteSqlCommand(string predicate)
        {
            StockIOMonthOut.ExecuteSqlCommand(predicate);
        }

        public void RemoveRange(Expression<Func<ERP.Models.Entities.StockIOMonthOut, bool>> predicate)
        {
            StockIOMonthOut.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.StockIOMonthOut> BuildRange(IEnumerable<Models.Views.MonthMaterialStockItemOut> items)
        {
            return items.Select(item => new ERP.Models.Entities.StockIOMonthOut
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