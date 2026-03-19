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
    public class MPSOutsourceMonthAPService : SearchService
    {
        ERP.Services.Business.Entities.MPSProcedurePOService MPSProcedurePO;
        ERP.Services.Business.Entities.MPSProcedureVendorService MPSProcedureVendor;
        ERP.Services.Business.Entities.MPSAPService MPSAP;
        ERP.Services.Business.Entities.MPSReceivedLogService MPSReceivedLog;

        public MPSOutsourceMonthAPService(
            ERP.Services.Business.Entities.MPSProcedurePOService mpsProcedurePOService,
            ERP.Services.Business.Entities.MPSProcedureVendorService mpsProcedureVendorService,
            ERP.Services.Business.Entities.MPSAPService mpsAPService,
            ERP.Services.Business.Entities.MPSReceivedLogService mpsReceivedLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePO = mpsProcedurePOService;
            MPSProcedureVendor = mpsProcedureVendorService;
            MPSAP = mpsAPService;
            MPSReceivedLog = mpsReceivedLogService;
        }

        public IQueryable<Models.Views.PayableForOutsource> GetMonthlyAPForOutsource(string predicate)
        {
            var result = (
                    from mrcl in MPSReceivedLog.Get()
                    join mppo in MPSProcedurePO.Get() on new { POId = mrcl.MpsProcedurePOId, LocaleId = mrcl.LocaleId } equals new { POId = mppo.Id, LocaleId = mppo.LocaleId }
                    join mpv in MPSProcedureVendor.Get() on new { VendorId = mppo.MpsProcedureVendorId, LocaleId = mppo.LocaleId } equals new { VendorId = mpv.Id, LocaleId = mpv.LocaleId }
                    join map in MPSAP.Get().GroupBy(i => new { i.PayMonth, i.Vendor, i.PaymentLocaleId })
                                           .Select(i => new { PayMonth = i.Key.PayMonth, Vendor = i.Key.Vendor, PaymentLocaleId = i.Key.PaymentLocaleId, AP = i.Sum(g => g.ChargeQty * g.UnitPrice), Discount = i.Sum(g => g.AdjustAmount), SubAmount = i.Sum(g => g.SubAmount) })
                                           on new { PayMonth = mrcl.PayMonth, Vendor = mpv.NameTw, PaymentLocaleId = mppo.PaymentLocaleId } equals new { PayMonth = map.PayMonth, Vendor = map.Vendor, PaymentLocaleId = map.PaymentLocaleId } into mapGRP
                    from map in mapGRP.DefaultIfEmpty()
                    select new
                    {
                        Vendor = mpv.NameTw,
                        VendorId = mppo.MpsProcedureVendorId,
                        SubAmount = map.SubAmount == null ? 0 : map.SubAmount,
                        PayDollar = mppo.DollarNameTw,
                        CloseMonth = Convert.ToInt32(mrcl.PayMonth),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                        CloseMonth1 = mrcl.PayMonth,                        // for select column
                        PayQty = mrcl.ChargeQty,
                        UnitPrice = mppo.UnitPrice,
                        Discount = map.Discount == null ? 0 : map.Discount,
                        PaymentLocaleId = mppo.PaymentLocaleId,
                        PurLocaleId = mppo.LocaleId,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(i => new { i.Vendor, i.VendorId, i.SubAmount, i.PayDollar, i.PaymentLocaleId, i.PurLocaleId, i.Discount, i.CloseMonth1 })
                .Select(i => new Models.Views.PayableForOutsource
                {
                    Vendor = i.Key.Vendor,
                    VendorId = i.Key.VendorId,
                    PlanPayTTL = i.Key.SubAmount,
                    PayDollar = i.Key.PayDollar,
                    CloseMonth = i.Key.CloseMonth1,
                    CloseMonthFrom = i.Min(g => g.CloseMonth1),
                    CloseMonthTo = i.Max(g => g.CloseMonth1),
                    APTTL = i.Sum(g => g.PayQty * g.UnitPrice),
                    Discount = i.Key.Discount,
                    PayRate = i.Sum(g => g.PayQty * g.UnitPrice) == 0 ? 0 : i.Key.SubAmount / i.Sum(g => g.PayQty * g.UnitPrice),
                    PaymentLocaleId = i.Key.PaymentLocaleId,
                    PurLocaleId = i.Key.PurLocaleId
                })
                .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.PayableItemForOutsource> GetMonthlyAPItemForOutsource(string predicate)
        {
            var item1 = (
                from mrcl in MPSReceivedLog.Get()
                join mppo in MPSProcedurePO.Get() on new { POId = mrcl.MpsProcedurePOId, LocaleId = mrcl.LocaleId } equals new { POId = mppo.Id, LocaleId = mppo.LocaleId }
                join mpv in MPSProcedureVendor.Get() on new { VendorId = mppo.MpsProcedureVendorId, LocaleId = mppo.LocaleId } equals new { VendorId = mpv.Id, LocaleId = mpv.LocaleId }
                join map in MPSAP.Get().GroupBy(i => new { i.PayMonth, i.Vendor, i.PaymentLocaleId })
                                       .Select(i => new { PayMonth = i.Key.PayMonth, Vendor = i.Key.Vendor, PaymentLocaleId = i.Key.PaymentLocaleId, AP = i.Sum(g => g.ChargeQty * g.UnitPrice), Discount = i.Sum(g => g.AdjustAmount), SubAmount = i.Sum(g => g.SubAmount) })
                                       on new { PayMonth = mrcl.PayMonth, Vendor = mpv.NameTw, PaymentLocaleId = mppo.PaymentLocaleId } equals new { PayMonth = map.PayMonth, Vendor = map.Vendor, PaymentLocaleId = map.PaymentLocaleId } into mapGRP
                from map in mapGRP.DefaultIfEmpty()
                select new
                {
                    WarehouseNo = mrcl.WarehouseNo,
                    ReceivedDate = mrcl.ReceivedDate,
                    FreeQty = mrcl.FreeQty,
                    QCBackQty = mrcl.QCBackQty,
                    ReceivedQty = mrcl.ReceivedQty,
                    PayQty = mrcl.ChargeQty,
                    CloseMonth = Convert.ToInt32(mrcl.PayMonth),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                    CloseMonth1 = mrcl.PayMonth,

                    VendorId = mppo.MpsProcedureVendorId,
                    PayDollar = mppo.DollarNameTw,
                    UnitPrice = mppo.UnitPrice,
                    PONo = mppo.PONo,
                    PlanQty = mppo.Qty,
                    PurQty = mppo.Qty,
                    Unit = mppo.PurUnitName,
                    VendorETD = mppo.VendorETD,
                    OrderNo = mppo.OrderNo,
                    StyleNo = mppo.StyleNo,
                    MPSProcedure = mppo.MpsProcedureGroupNameTw,
                    PaymentLocaleId = mppo.PaymentLocaleId,
                    PurLocaleId = mppo.LocaleId,

                    Vendor = mpv.NameTw,
                    VendorShort = mpv.ShortNameTw,

                    SubAmount = (decimal?)map.SubAmount,
                    Discount = map.Discount,
                    PriceType = mppo.PriceType,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new PayableItemForOutsource
            {
                Vendor = i.VendorShort,
                VendorId = i.VendorId,
                SubAmount = i.SubAmount,
                PayDollar = i.PayDollar,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                WarehouseNo = i.WarehouseNo,
                ReceivedDate = i.ReceivedDate,
                PONo = i.PONo,
                PlanQty = i.PlanQty,
                PayQty = i.PayQty,
                FreeQty = i.FreeQty,
                QCBackQty = i.QCBackQty,
                ReceivedQty = i.ReceivedQty,
                PurQty = i.PurQty,
                Unit = i.Unit,
                VendorETD = i.VendorETD,
                OrderNo = i.OrderNo,
                StyleNo = i.StyleNo,
                MPSProcedure = i.MPSProcedure,

                PayQtyTTL = i.PayQty * i.UnitPrice,

                PaymentLocaleId = i.PaymentLocaleId,
                PurLocaleId = i.PurLocaleId,
                CloseMonth = i.CloseMonth1,       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                PriceType = i.PriceType,
            })
            .ToList()
            .AsQueryable();

            return item1.AsQueryable();
        }

    }
}