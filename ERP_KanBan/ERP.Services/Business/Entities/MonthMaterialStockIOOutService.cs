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

    public class MonthMaterialStockIOOutService : BusinessService
    {
        private ERP.Services.Entities.MonthMaterialStockIOOutService MonthMaterialStockIOOut { get; set; }

        public MonthMaterialStockIOOutService(
            ERP.Services.Entities.MonthMaterialStockIOOutService monthMaterialStockIOOutService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MonthMaterialStockIOOut = monthMaterialStockIOOutService;
        }

        public IQueryable<Models.Views.MonthMaterialStockIOOut> Get()
        {
            var items = (
                from m in MonthMaterialStockIOOut.Get()
                select new Models.Views.MonthMaterialStockIOOut
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    IOMonth = m.IOMonth,
                    MaterialStockId = m.MaterialStockId,
                    IOQty =  m.IOQty,
                    UnitPrice = m.UnitPrice,
                    BankingRate = m.BankingRate,
                    IOAmount = m.IOAmount,
                    SourceType = m.SourceType
                });
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MonthMaterialStockIOOut> items)
        {
            MonthMaterialStockIOOut.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MonthMaterialStockIOOut, bool>> predicate)
        {
            MonthMaterialStockIOOut.RemoveRange(predicate);
        }

        public void ExecuteSqlCommand(string predicate)
        {
            MonthMaterialStockIOOut.ExecuteSqlCommand(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MonthMaterialStockIOOut> BuildRange(IEnumerable<Models.Views.MonthMaterialStockIOOut> items)
        {
            return items.Select(item => new ERP.Models.Entities.MonthMaterialStockIOOut
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                IOMonth = item.IOMonth,
                MaterialStockId = item.MaterialStockId,
                IOQty =  item.IOQty,
                UnitPrice = item.UnitPrice,
                BankingRate = item.BankingRate,
                IOAmount = item.IOAmount,
                SourceType = item.SourceType
            });
        }

    }
}
