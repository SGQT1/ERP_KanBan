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

    public class MonthMaterialStockIOInService : BusinessService
    {
        private ERP.Services.Entities.MonthMaterialStockIOInService MonthMaterialStockIOIn { get; set; }

        public MonthMaterialStockIOInService(
            ERP.Services.Entities.MonthMaterialStockIOInService monthMaterialStockIOInService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MonthMaterialStockIOIn = monthMaterialStockIOInService;
        }

        public IQueryable<Models.Views.MonthMaterialStockIOIn> Get()
        {
            var items = (
                from m in MonthMaterialStockIOIn.Get()
                select new Models.Views.MonthMaterialStockIOIn
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

        public void CreateRange(IEnumerable<Models.Views.MonthMaterialStockIOIn> items)
        {
            MonthMaterialStockIOIn.CreateRange(BuildRange(items));
        }
        public void ExecuteSqlCommand(string predicate)
        {
            MonthMaterialStockIOIn.ExecuteSqlCommand(predicate);
        }

        public void RemoveRange(Expression<Func<ERP.Models.Entities.MonthMaterialStockIOIn, bool>> predicate)
        {
            MonthMaterialStockIOIn.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MonthMaterialStockIOIn> BuildRange(IEnumerable<Models.Views.MonthMaterialStockIOIn> items)
        {
            return items.Select(item => new ERP.Models.Entities.MonthMaterialStockIOIn
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
