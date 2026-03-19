using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class MaterialStockCloseTimeService : SearchService
    {
        private ERP.Services.Entities.StockIOService StockIO { get; set; }
        private ERP.Services.Entities.StockIOMonthSchService StockIOMonthSch { get; set; }


        public MaterialStockCloseTimeService(
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.StockIOMonthSchService stockIOMonthSchService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            StockIO = stockIOService;
            StockIOMonthSch = stockIOMonthSchService;
        }

        public IQueryable<Models.Views.MonthMaterialStockIO> GetCloseTome(string predicate)
        {

            var _stockIO = StockIO.Get()
                .GroupBy(i => new { Year = i.IODate.Year, Month = i.IODate.Month, LocaleId = i.LocaleId })
                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, LocaleId = g.Key.LocaleId, LastIODate = g.Max(i => i.LastUpdateTime) })
                .Distinct()
                .ToList();

            var stockIOYM = _stockIO
                .Select(i => new { IOMonth = Convert.ToDecimal(i.Year.ToString("0000") + i.Month.ToString("00")), i.LastIODate, i.LocaleId })
                .AsQueryable();

            var stockIOYMSch = StockIOMonthSch.Get().ToList();

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
                    IsReCal = sch == null ? 1 : 0
                })
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return stockIOMonth.AsQueryable();
        }

    }
}