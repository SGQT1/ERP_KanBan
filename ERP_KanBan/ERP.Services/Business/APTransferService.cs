using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class APTransferService : BusinessService
    {
        private ERP.Services.Business.Entities.APTransferService APTransfer { get; set; }
        private ERP.Services.Business.Entities.APTransferItemService APTransferItem { get; set; }
        private ERP.Services.Business.Entities.APMonthService APMonth { get; set; }
        private ERP.Services.Business.Entities.APMonthItemService APMonthItem { get; set; }

        private ERP.Services.Business.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogAddService ReceivedLogAdd { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.POService PO { get; set; }
        public APTransferService(
            ERP.Services.Business.Entities.APTransferService apTransferService,
            ERP.Services.Business.Entities.APTransferItemService apTransferItemService,
            ERP.Services.Business.Entities.APMonthService apMonthService,
            ERP.Services.Business.Entities.APMonthItemService apMonthItemService,

            ERP.Services.Business.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Business.Entities.ReceivedLogAddService receivedLogAddService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.POService poService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            APTransfer = apTransferService;
            APTransferItem = apTransferItemService;
            APMonth = apMonthService;
            APMonthItem = apMonthItemService;

            Orders = ordersService;
            PO = poService;
            POItem = poItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogAdd = receivedLogAddService;
            Vendor = vendorService;
            Company = companyService;
        }

        public ERP.Models.Views.APTransferGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.APTransferGroup { };
            var apTransfer = APTransfer.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (apTransfer != null)
            {
                group.APTransfer = apTransfer;
                group.APTransferItem = APTransferItem.Get().Where(i => i.APTransferId == id && i.LocaleId == localeId).OrderBy(i => i.VendorNameTw).ToList();
            }
            return group;
        }

        public ERP.Models.Views.APTransferGroup Build(APTransfer item)
        {
            var group = new ERP.Models.Views.APTransferGroup { };
            var apTransfer = item;
            if (apTransfer != null)
            {
                var payLocaleId = apTransfer.PaymentLocaleId;
                // var purLocaleId = apTransfer.PurLocaleId;
                var purLocaleId = apTransfer.LocaleId;
                var closeMonth = Convert.ToInt32(apTransfer.YYYYMM);
                var localeId = apTransfer.LocaleId;

                group.APTransfer = apTransfer;
                group.APTransferItem = BuildAPTransferItem(closeMonth, (int)purLocaleId, (int)payLocaleId, (int)localeId);
            }
            return group;
        }

        public ERP.Models.Views.APTransferGroup Save(APTransferGroup item)
        {
            var apTransfer = item.APTransfer;
            var apTransferItem = item.APTransferItem.ToList();

            if (apTransfer != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Transfer
                    {
                        var _apTransfer = APTransfer.Get().Where(i => i.LocaleId == apTransfer.LocaleId && i.Id == apTransfer.Id).FirstOrDefault();

                        if (_apTransfer != null)
                        {
                            apTransfer.Id = _apTransfer.Id;
                            apTransfer.LocaleId = _apTransfer.LocaleId;
                            apTransfer = APTransfer.Update(apTransfer);
                        }
                        else
                        {
                            apTransfer = APTransfer.Create(apTransfer);
                        }
                    }

                    //Transfer Item
                    {
                        if (apTransfer.Id != 0)
                        {
                            apTransferItem.ForEach(i => i.APTransferId = apTransfer.Id);

                            APTransferItem.RemoveRange(i => i.APTransferId == apTransfer.Id && i.LocaleId == apTransfer.LocaleId);
                            APTransferItem.CreateRange(apTransferItem);

                            // var aps = APTransferItem.Get().Where(i => i.APTransferId == apTransfer.Id && i.LocaleId == apTransfer.LocaleId && i.IsTransfer == 1).ToList();
                            // var vendors = aps.Select(i => i.VendorNameTw).Distinct();
                            // var apYM = apTransfer.YYYYMM;

                            // var APMonthIds = APMonth.Get().Where(i => i.LocaleId == apTransfer.LocaleId && i.YYYYMM == apYM && !vendors.Contains(i.VendorNameTw)).Select(i => i.Id).ToList();
                            // APMonth.RemoveRange(i => i.LocaleId == apTransfer.LocaleId && APMonthIds.Contains(i.Id));
                            // APMonthItem.RemoveRange(i => i.LocaleId == apTransfer.LocaleId && APMonthIds.Contains(i.APMonthId));

                        }
                    }
                    UnitOfWork.Commit();

                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
            return Get((int)apTransfer.Id, (int)apTransfer.LocaleId);

        }

        public void Remove(APTransferGroup item)
        {
            var apTransfer = item.APTransfer;
            UnitOfWork.BeginTransaction();
            try
            {
                APTransferItem.RemoveRange(i => i.APTransferId == apTransfer.Id && i.LocaleId == apTransfer.LocaleId);
                APTransfer.Remove(apTransfer);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public IQueryable<Models.Views.APTransferItem> BuildAPTransferItem(int colseMonth, int purLocaleId, int paymentLocaleId, int localeId)
        {
            // step 1: 取收貨資料
            // step 2: 取前期未沖帳資料
            // step 3: 彙總收貨廠商＋前期未沖廠商(但不包括當期新增廠商)
            // var company = Company.Get().ToList();
            var recdItems = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItemId = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItemId = pi.Id, LocaleId = pi.LocaleId }
                join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                join o in Orders.Get() on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                where rl.TransferInId == 0 && rl.IsAccounting == 0 && (v.PaymentPoint == 0 ? rl.IQCResult : 4) > 1 && Convert.ToInt32(rla.CloseMonth) == colseMonth && pi.PaymentLocaleId == paymentLocaleId
                group new { rl, rla, pi, p, o, v } by new { v.NameTw, o.CompanyId, pi.PaymentLocaleId, pi.PurLocaleId, pi.LocaleId, rla.CloseMonth } into rGRP
                select new Models.Views.APTransferItem
                {
                    Id = 0,
                    LocaleId = localeId,
                    APTransferId = 0,
                    IsTransfer = 0,
                    VendorNameTw = rGRP.Key.NameTw,
                    APQty = 0,
                    UnitPrice = 0,
                    SaleAmount = rGRP.Sum(g => g.rl.SubTotalPrice / (1 + g.v.QuoTaxIn)),
                    Tax = rGRP.Sum(g => g.v.IsTaxAdded == 1 ? g.rl.SubTotalPrice / (1 + g.v.QuoTaxIn) * g.v.TaxRate : 0),
                    TTL = 0,
                    IsIntergrate = 0,
                    IsFromInvoice = 0,
                    YYYYMM = rGRP.Key.CloseMonth,
                }
            )
            
            .ToList();

            var preAPItems = (
                from api in APMonthItem.Get()
                join ap in APMonth.Get() on new { APMonthId = api.APMonthId, LocaleId = api.LocaleId } equals new { APMonthId = (decimal?)ap.Id, LocaleId = (decimal?)ap.LocaleId }
                where Convert.ToInt32(api.APYM) < colseMonth && api.LocaleId == localeId && api.IsGet == 0 && api.PaymentLocaleId == paymentLocaleId
                group new { api, ap } by new { ap.VendorNameTw } into apiGRP
                select new Models.Views.APTransferItem
                {
                    Id = 0,
                    LocaleId = localeId,
                    APTransferId = 0,
                    IsTransfer = 0,
                    VendorNameTw = apiGRP.Key.VendorNameTw,
                    APQty = 0,
                    UnitPrice = 0,
                    SaleAmount = 0,
                    Tax = 0,
                    TTL = 0,
                    IsIntergrate = 0,
                    IsFromInvoice = 0,
                    YYYYMM = apiGRP.Max(g => g.api.APYM),
                }
            )
            .ToList();

            var recdVendor = recdItems.Select(i => i.VendorNameTw);
            var items = recdItems.Union(preAPItems.Where(i => !recdVendor.Contains(i.VendorNameTw))).ToList();

            // items.ForEach(i =>
            // {
            //     i.CompanyNo = company.Where(c => c.Id == i.CompanyId).Max(c => c.CompanyNo);
            //     i.PurLocale = company.Where(c => c.Id == i.PurLocaleId).Max(c => c.CompanyNo);
            //     i.PaymentLocale = company.Where(c => c.Id == i.PaymentLocaleId).Max(c => c.CompanyNo);
            // });

            return items.AsQueryable();
        }
    }
}
