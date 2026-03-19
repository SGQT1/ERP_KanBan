using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
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
using NPOI.SS.Formula.Functions;

namespace ERP.Services.Search
{
    public class OrdersStockService : SearchService
    {

        private Services.Business.Entities.OrdersStockService OrdersStock { get; set; }

        private Services.Entities.OrdersStockLocaleService OrdersStockLocale { get; }
        private Services.Entities.OrdersStockLocaleInService OrdersStockLocaleIn { get; }
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CTNLabelStockOutService CTNLabelStockOut { get; }
        private Services.Entities.CTNLabelStockInService CTNLabelStockIn { get; }
        private Services.Business.Entities.OrdersStockItemService OrdersStockItem { get; set; }

        public OrdersStockService(
            Services.Business.Entities.OrdersStockService ordersStockService,
            Services.Entities.OrdersStockLocaleService ordersStockLocaleService,
            Services.Entities.OrdersStockLocaleInService ordersStockLocaleInService,
            Services.Business.Entities.OrdersStockItemService ordersStockItemService,
            Services.Entities.OrdersService ordersService,
            Services.Entities.CTNLabelStockOutService ctnLabelStockOutService,
Services.Entities.CTNLabelStockInService ctnLabelStockInService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OrdersStock = ordersStockService;
            OrdersStockLocale = ordersStockLocaleService;
            OrdersStockLocaleIn = ordersStockLocaleInService;
            OrdersStockItem = ordersStockItemService;
            Orders = ordersService;
            CTNLabelStockOut = ctnLabelStockOutService;
            CTNLabelStockIn = ctnLabelStockInService;
        }


        public IQueryable<Models.Views.OrdersStockItem> GetOrderSotck(string predicate)
        {
            return OrdersStock.Get(predicate);
        }
        public List<Models.Views.OrdersStock> GetOrdersStockGroup(string predicate)
        {

            return OrdersStock.GetGroup(predicate);
        }
        public IQueryable<Models.Views.OrdersStock> GetOrdersStockSummary(string predicate)
        {
            return OrdersStock.GetSummary(predicate);

        }

        public List<Models.Views.OrdersStockTaking> GetStockTakingsByOrder(string predicate)
        {
            return OrdersStock.GetStockTakingsByOrder(predicate);
        }

        public List<Models.Views.OrdersStockTaking> GetStockTakingsByStyle(string predicate)
        {
            return OrdersStock.GetStockTakingsByStyle(predicate);
        }

        public IQueryable<Models.Views.OrdersStockLocale> GetOrdersStockLocale(string predicate)
        {
            var stockOuts = CTNLabelStockOut.Get().Select(i => i.LabelCode);
            var result = (
                from s in OrdersStockLocale.Get()
                join si in OrdersStockLocaleIn.Get().Where(i => !stockOuts.Contains(i.CTNLabelCode)) on new { Id = s.Id, s.LocaleId } equals new { Id = si.OrdersStockLocaleId, si.LocaleId } into siGRP
                from si in siGRP.DefaultIfEmpty()
                select new
                {
                    Id = s.Id,
                    LocaleId = s.LocaleId,
                    FactoryNo = s.FactoryNo,
                    BlockNo = s.BlockNo,
                    AreaNo = s.AreaNo,
                    LocaleNo = s.LocaleNo,
                    MaxQty = s.MaxQty,
                    LocaleDesc = s.LocaleDesc,
                    OrderNo = si.OrderNo,
                    // StyleNo = o.StyleNo,
                    StockInTime = si.StockInTime,
                    CTNLabelCode = si.CTNLabelCode,
                    SubLabelCode = si.SubLabelCode,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(g => new { g.Id, g.LocaleId, g.FactoryNo, g.BlockNo, g.AreaNo, g.LocaleNo, g.MaxQty, g.LocaleDesc })
            .Select(i => new Models.Views.OrdersStockLocale
            {
                Id = i.Key.Id,
                LocaleId = i.Key.LocaleId,
                FactoryNo = i.Key.FactoryNo,
                BlockNo = i.Key.BlockNo,
                AreaNo = i.Key.AreaNo,
                LocaleNo = i.Key.LocaleNo,
                MaxQty = i.Key.MaxQty,
                LocaleDesc = i.Key.LocaleDesc,
                Qty = i.Count(g => g.CTNLabelCode != null)
            })
            .ToList();
            return result.AsQueryable();
        }
        public IQueryable<Models.Views.OrdersStockLocaleIn> GetOrdersStockLocaleIn(string predicate)
        {
            var stockOuts = CTNLabelStockOut.Get().Select(i => i.LabelCode);
            var result = (
                from os in OrdersStockLocaleIn.Get().Where(i => !stockOuts.Contains(i.CTNLabelCode))
                join o in Orders.Get() on new { OrderNo = os.OrderNo, LocaleId = os.LocaleId } equals new { OrderNo = o.OrderNo, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new Models.Views.OrdersStockLocaleIn
                {
                    Id = os.Id,
                    LocaleId = os.LocaleId,
                    ModifyUserName = os.ModifyUserName,
                    LastUpdateTime = os.LastUpdateTime,
                    OrdersStockLocaleId = os.OrdersStockLocaleId,
                    OrdersStockLocaleCode = os.OrdersStockLocaleCode,
                    CTNLabelId = os.CTNLabelId,
                    CTNLabelCode = os.CTNLabelCode,
                    StockInTime = os.StockInTime,
                    Remark = os.Remark,
                    Version = os.Version,
                    OrderNo = os.OrderNo,
                    SeqNo = os.SeqNo,
                    SubLabelCode = os.SubLabelCode,
                    CustomerOrderNo = o.CustomerOrderNo,
                    Customer = o.Customer,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return result.AsQueryable();
        }
        public IQueryable<Models.Views.OrdersStockLocaleIn> GetAllOrdersStockLocaleIn(string predicate)
        {
            var stockOuts = CTNLabelStockOut.Get().Select(i => i.LabelCode);
            var result = (
                from os in OrdersStockLocaleIn.Get().Where(i => !stockOuts.Contains(i.CTNLabelCode))
                join s in OrdersStockLocale.Get() on new { OrdersStockLocaleId = os.OrdersStockLocaleId, LocaleId = os.LocaleId } equals new { OrdersStockLocaleId = s.Id, LocaleId = s.LocaleId }
                join si in CTNLabelStockIn.Get() on new { CTNLabelCode = os.CTNLabelCode, LocaleId = os.LocaleId } equals new { CTNLabelCode = si.LabelCode, LocaleId = si.LocaleId }
                join o in Orders.Get() on new { OrderNo = os.OrderNo, LocaleId = os.LocaleId } equals new { OrderNo = o.OrderNo, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new Models.Views.OrdersStockLocaleIn
                {
                    Id = os.Id,
                    LocaleId = os.LocaleId,
                    ModifyUserName = os.ModifyUserName,
                    LastUpdateTime = os.LastUpdateTime,
                    OrdersStockLocaleId = os.OrdersStockLocaleId,
                    OrdersStockLocaleCode = os.OrdersStockLocaleCode,
                    CTNLabelId = os.CTNLabelId,
                    CTNLabelCode = os.CTNLabelCode,
                    StockInTime = os.StockInTime,
                    Remark = os.Remark,
                    Version = os.Version,
                    OrderNo = os.OrderNo,
                    SeqNo = os.SeqNo,
                    SubLabelCode = os.SubLabelCode,
                    CustomerOrderNo = o.CustomerOrderNo,
                    Customer = o.Customer,
                    FactoryNo = s.FactoryNo,
                    LocaleDesc = s.LocaleDesc,
                    CNTLStockInTime = si.StockInTime
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return result.AsQueryable();
        }
    }
}