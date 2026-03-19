using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MonthMaterialStockIOService : BusinessService
    {
        private ERP.Services.Entities.StockIOService StockIO { get; set; }
        private ERP.Services.Entities.MonthMaterialStockIOSchService MonthMaterialStockIOSch { get; set; }
        
        public MonthMaterialStockIOService(
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.MonthMaterialStockIOSchService monthMaterialStockIOSchService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            StockIO = stockIOService;
            MonthMaterialStockIOSch = monthMaterialStockIOSchService;
        }

        public IQueryable<Models.Views.MonthMaterialStockIO> Get(string predicate)
        {

            var _stockIO = StockIO.Get()
                .GroupBy(i => new { Year = i.IODate.Year, Month = i.IODate.Month, LocaleId = i.LocaleId })
                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, LocaleId = g.Key.LocaleId, LastIODate = g.Max(i => i.LastUpdateTime) })
                .Distinct()
                .ToList();

            var stockIOYM = _stockIO
                .Select(i => new { IOMonth = Convert.ToDecimal(i.Year.ToString("0000") + i.Month.ToString("00")), i.LastIODate, i.LocaleId })
                .AsQueryable();

            var stockIOYMSch = MonthMaterialStockIOSch.Get().ToList();

            var stockIOMonth = (
                from sym in stockIOYM
                join sch in stockIOYMSch on new { IOMonth = sym.IOMonth, LocaleId = sym.LocaleId } equals new { IOMonth = sch.IOMonth, LocaleId = sch.LocaleId } into schGRP
                from sch in schGRP.DefaultIfEmpty()
                select new Models.Views.MonthMaterialStockIO
                {
                    LocaleId = sym.LocaleId,
                    IOMonth = sym.IOMonth,
                    MaxUpdateTime = sym.LastIODate,
                    CalTime = sch == null ? null :sch.CalTime,
                    ModifyUserName = sch == null ? "" : sch.ModifyUserName,
                    IsReCal = sch == null ? 1 : 0,
                    Records = sch == null ? 0 : sch.Records,
                })
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return stockIOMonth.AsQueryable();
        }
    }
}