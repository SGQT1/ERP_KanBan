using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Search
{
    public class InvoiceService : SearchService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.SaleService Sale { get; }
        private Services.Entities.ShippingService Shipping { get; }
        private Services.Entities.ShippingPaidLogService ShippingPaidLog { get; }

        public InvoiceService(
            Services.Entities.ShippingService shippingService,
            Services.Entities.SaleService saleService,
            Services.Entities.OrdersService ordersService,
            Services.Entities.ShippingPaidLogService shippingPaidLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Shipping = shippingService;
            Sale = saleService;
            Orders = ordersService;
            ShippingPaidLog = shippingPaidLogService;
        }
        public IQueryable<Models.Views.AccountReceivable> GetARBalance(string predicate, string[] filters)
        {
            //paidWhere for special condiition, query-string is from UI(javascrip) 
            //step1: 應收餘額：效能關係，先把所有的的資料(訂單、出貨、發票、付款)先join起來，把相同欄位先一次distinct，減少資料庫回傳筆數
            //step2: group by 發票編號，找出所有條件下的發票，再根據欄位計算應收的餘額。

            //Step1, find out data by qeuery, and distinct same data to save time form DB
            var extendPredicate = (filters == null || filters.Length == 0) ? "1=1" : String.Join(" && ", filters);
            var ar = (
                from i in Shipping.Get()
                join s in Sale.Get() on new { Invoice = i.Id, LocaleId = i.LocaleId } equals new { Invoice = s.ShippingId, LocaleId = s.LocaleId }
                join o in Orders.Get() on new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId } equals new { OrdersId = o.Id, LocaleId = (decimal?)o.LocaleId }
                join p in ShippingPaidLog.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate) on new { InvoiceNo = i.InvoiceNo, LocaleId = i.LocaleId } equals new { InvoiceNo = p.InvoiceNo, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new Models.Views.AccountReceivable
                {
                    LocaleId = i.LocaleId,
                    InvoiceNo = i.InvoiceNo,
                    CustomerId = i.CustomerId,
                    Customer = i.CustomerNameTw,
                    PayDollarCodeId = i.PayDollarCodeId,
                    PayDollarCodeDesc = i.PayDollarCodeDesc,
                    ARTotal = i.ARTotal,
                    ARReceived = i.ARReceived,
                    Remark = i.Remark,

                    ARR = p != null ? p.AROff : 0,
                    ARF = (p != null && p.IsCFM == 1) ? p.AROff : 0,
                    CompanyId = o.CompanyId,
                    Company = o.CompanyNo,
                    BrandId = s.BrandCodeId,
                    Brand = s.BrandCode,
                    ShipmentDate = s.SaleDate,
                    PaidDate = p.PaidDate
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            // Step 2 : group by invoice no , by no to caculate arr, arf, balance, shipment date, paid date
            var group = ar
                .GroupBy(i => new { i.LocaleId, i.InvoiceNo, i.Customer, i.CustomerId, i.PayDollarCodeId, i.PayDollarCodeDesc, i.ARTotal })
                .Select(i => new Models.Views.AccountReceivable
                {
                    LocaleId = i.Key.LocaleId,
                    InvoiceNo = i.Key.InvoiceNo,
                    CustomerId = i.Key.CustomerId,
                    Customer = i.Key.Customer,
                    PayDollarCodeId = i.Key.PayDollarCodeId,
                    PayDollarCodeDesc = i.Key.PayDollarCodeDesc,
                    ARTotal = i.Key.ARTotal,
                    ARR = ar.Where(g => g.InvoiceNo == i.Key.InvoiceNo && g.LocaleId == i.Key.LocaleId).Select(g => new{g.PaidDate, g.ARR}).Distinct().Select(i => i.ARR).Sum(),
                    ARF = ar.Where(g => g.InvoiceNo == i.Key.InvoiceNo && g.LocaleId == i.Key.LocaleId).Select(g => new{g.PaidDate, g.ARF}).Distinct().Select(i => i.ARF).Sum(),
                    ARReceived = ar.Where(g => g.InvoiceNo == i.Key.InvoiceNo && g.LocaleId == i.Key.LocaleId).Select(g => new{g.PaidDate, g.ARReceived}).Distinct().Select(i => i.ARReceived).Sum(),
                    // ARReceived = i.Key.ARReceived,
                    // Remark = i.Key.Remark,
                    // BrandId = i.Key.BrandId,
                    // Brand = i.Key.Brand,
                    ShipmentDate = i.Min(g => g.ShipmentDate),
                    PaidDate = i.Max(g => g.PaidDate),
                })
                .Select(i => new Models.Views.AccountReceivable
                {
                    LocaleId = i.LocaleId,
                    InvoiceNo = i.InvoiceNo,
                    CustomerId = i.CustomerId,
                    Customer = i.Customer,
                    PayDollarCodeId = i.PayDollarCodeId,
                    PayDollarCodeDesc = i.PayDollarCodeDesc,
                    ARTotal = i.ARTotal,
                    ARR = i.ARR,
                    ARF = i.ARF,
                    ARReceived = i.ARReceived,
                    Balance = i.ARTotal - i.ARR,
                    Remark = i.Remark,
                    BrandId = i.BrandId,
                    Brand = i.Brand,
                    ShipmentDate = i.ShipmentDate,
                    PaidDate = i.PaidDate,
                    ARStatus = i.ARTotal <= i.ARR ? 1 : 0
                })
                .AsQueryable();
            return group;
        }
    }
}