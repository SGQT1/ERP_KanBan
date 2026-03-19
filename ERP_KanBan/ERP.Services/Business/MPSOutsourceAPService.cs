using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using Newtonsoft.Json;
using ERP.Models.Views.Common;

namespace ERP.Services.Business
{
    public class MPSOutsourceAPService : BusinessService
    {

        private ERP.Services.Business.Entities.MPSReceivedLogService MPSReceivedLog { get; set; }
        private ERP.Services.Business.Entities.MPSProcedurePOService MPSProcedurePO { get; set; }
        private ERP.Services.Business.Entities.MPSProcedurePOItemService MPSProcedurePOItem { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureVendorService MPSProcedureVendor { get; set; }
        private ERP.Services.Business.Entities.MPSAPService MPSAP { get; set; }

        private ERP.Services.Business.Entities.MPSReceivedLogSizeItemService MPSReceivedLogSizeItem { get; set; }
        private ERP.Services.Business.Entities.MPSProcedurePOSizeService MPSProcedurePOSize { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }



        public MPSOutsourceAPService(
            ERP.Services.Business.Entities.MPSProcedurePOService mpsProcedurePOService,
            ERP.Services.Business.Entities.MPSReceivedLogService mpsReceivedLogService,
            ERP.Services.Business.Entities.MPSProcedureVendorService mpsProcedureVendorService,
            ERP.Services.Business.Entities.MPSProcedurePOItemService mpsProcedurePOItemService,
            ERP.Services.Business.Entities.MPSAPService mpsAPService,
            ERP.Services.Entities.OrdersService ordersService,

            ERP.Services.Business.Entities.MPSReceivedLogSizeItemService mpsReceivedLogSizeItemService,
            ERP.Services.Business.Entities.MPSProcedurePOSizeService mpsProcedurePOSizeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePO = mpsProcedurePOService;
            MPSProcedurePOItem = mpsProcedurePOItemService;
            MPSReceivedLog = mpsReceivedLogService;
            MPSProcedureVendor = mpsProcedureVendorService;
            MPSAP = mpsAPService;
            Orders = ordersService;

            MPSReceivedLogSizeItem = mpsReceivedLogSizeItemService;
            MPSProcedurePOSize = mpsProcedurePOSizeService;
        }

        public IQueryable<ERP.Models.Views.MPSAPSummary> GetMPSOutsourceReceivedForAP(string predicate, string[] filters)
        {
            /* Select doPay,A.PriceType,AK.WarehouseNo,Convert(varchar(10),ReceivedDate,112) ReceivedDate, PayMonth, NameTw,OrderNo,NULL, ChargeQty, FreeQty, AddFreeQty, QCBackQty, ReceivedQty, PurUnitName,UnitPrice,DiscountRate,
                      Convert(Decimal(20,2),0),Convert(Decimal(20,2),ChargeQty*UnitPrice),DollarNameTw,'',PONo,StyleNo,NULL,A.PaymentLocaleId,A.Id,AK.Id,AK.LocaleId,DayOfMonth 
               From MpsReceivedLog AK  
               INNER JOIN MpsProcedurePO A ON AK.MpsProcedurePOId=A.Id and AK.RefLocaleId=A.LocaleId 
               INNER JOIN MpsProcedureVendor B ON A.MpsProcedureVendorId=B.Id and A.LocaleId=B.LocaleId 
               Where PayMonth='202502' and AK.Id not in (Select MpsReceivedLogId From MpsAP Where LocaleId=AK.LocaleId) and AK.LocaleId=10 Order by ReceivedDate, NameTw, PONo
             */
            var localeId = 0;
            var hasAP = false;

            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                localeId = (int)extenFilters.Field1;
                hasAP = (Boolean)extenFilters.Field9;
            }

            var hasPayIds = MPSAP.Get().Where(i => i.LocaleId == localeId).Select(i => i.MpsReceivedLogId);
            var baseItems = (
                    from mrcl in MPSReceivedLog.Get().Where(i => i.LocaleId == localeId)
                    join mppo in MPSProcedurePO.Get() on new { POId = mrcl.MpsProcedurePOId, LocaleId = mrcl.LocaleId } equals new { POId = mppo.Id, LocaleId = mppo.LocaleId }
                    select new
                    {
                        Vendor = mppo.Vendor,
                        VendorId = mppo.MpsProcedureVendorId,
                        // SubAmount = map.SubAmount == null ? 0 : map.SubAmount,
                        PayDollar = mppo.DollarNameTw,
                        CloseMonth = Convert.ToInt32(mrcl.PayMonth),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                        CloseMonth1 = mrcl.PayMonth,                        // for select column
                        PayQty = mrcl.ChargeQty,
                        UnitPrice = mppo.UnitPrice,
                        // Discount = map.Discount == null ? 0 : map.Discount,
                        PaymentLocaleId = mppo.PaymentLocaleId,
                        PurLocaleId = mppo.LocaleId,
                        OrderNo = mppo.OrderNo,
                        StyleNo = mppo.StyleNo,
                        Id = mrcl.Id,
                    }
                );
            var items = hasAP ? baseItems.Where(i => hasPayIds.Contains(i.Id)) : baseItems.Where(i => !hasPayIds.Contains(i.Id));
            var result = items
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(i => new { i.Vendor, i.VendorId, i.PayDollar, i.PaymentLocaleId, i.PurLocaleId, i.CloseMonth1 })
                .Select(i => new Models.Views.MPSAPSummary
                {
                    Vendor = i.Key.Vendor,
                    VendorId = i.Key.VendorId,
                    // PlanPayTTL = i.Key.SubAmount,
                    PayDollar = i.Key.PayDollar,
                    CloseMonth = i.Key.CloseMonth1,
                    CloseMonthFrom = i.Min(g => g.CloseMonth1),
                    CloseMonthTo = i.Max(g => g.CloseMonth1),
                    APTTL = i.Sum(g => g.PayQty * g.UnitPrice),
                    // Discount = i.Key.Discount,
                    // PayRate = i.Sum(g => g.PayQty * g.UnitPrice) == 0 ? 0 : i.Key.SubAmount / i.Sum(g => g.PayQty * g.UnitPrice),
                    PaymentLocaleId = i.Key.PaymentLocaleId,
                    PurLocaleId = i.Key.PurLocaleId
                })
                .ToList();

            return result.AsQueryable();
        }

        public IQueryable<ERP.Models.Views.MPSReceivedLogForAP> GetMPSOutsourceAPItem(string predicate, string[] filters)
        {
            var localeId = 0;
            var hasAP = false;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                localeId = (int)extenFilters.Field1;
                hasAP = (Boolean)extenFilters.Field9;
            }
            var hasPayIds = MPSAP.Get().Where(i => i.LocaleId == localeId).Select(i => i.MpsReceivedLogId);
            var baseItems = (
                    from mpr in MPSReceivedLog.Get().Where(i => i.LocaleId == localeId)
                    join mp in MPSProcedurePO.Get() on new { POId = mpr.MpsProcedurePOId, LocaleId = mpr.LocaleId } equals new { POId = mp.Id, LocaleId = mp.LocaleId }
                    join mpi in MPSProcedurePOItem.Get() on new { MPSProcedurePOId = mp.Id, LocaleId = mp.LocaleId } equals new { MPSProcedurePOId = mpi.MpsProcedurePOId, LocaleId = mpi.LocaleId }
                    select new
                    {
                        Id = mpr.Id,
                        LocaleId = mpr.LocaleId,
                        RefLocaleId = mpr.RefLocaleId,
                        ReceivedDate = mpr.ReceivedDate,
                        ReceivedQty = mpr.ReceivedQty,
                        ChargeQty = mpr.ChargeQty,
                        FreeQty = mpr.FreeQty,
                        QCBackQty = mpr.QCBackQty,
                        DoPay = mpr.doPay,
                        ModifyUserName = mpr.ModifyUserName,
                        LastUpdateTime = mpr.LastUpdateTime,
                        Remark = mpr.Remark,
                        PayMonth = mpr.PayMonth,
                        AddFreeQty = mpr.AddFreeQty,
                        DiscountRate = mpr.DiscountRate,
                        WarehouseNo = mpr.WarehouseNo,

                        OrderNo = mp.OrderNo,
                        PONo = mp.PONo,
                        StyleNo = mp.StyleNo,
                        PurUnitName = mp.PurUnitName,
                        DayOfMonth = mp.DayOfMonth,
                        PriceType = mp.PriceType,
                        VendorId = mp.MpsProcedureVendorId,
                        UnitPrice = mp.UnitPrice,
                        DollarNameTw = mp.DollarNameTw,
                        PaymentLocaleId = mp.PaymentLocaleId,
                        ProcedureNameTw = mpi.ProcedureNameTw,
                        CSD = Orders.Get().Where(i => i.OrderNo == mp.OrderNo && i.LocaleId == mp.LocaleId).Max(i => i.CSD),

                        PurLocaleId = mp.LocaleId,
                        CloseMonth = Convert.ToInt32(mpr.PayMonth),       // for condition, cause column type is string , query by linq is too slow, covert to interger 
                        CloseMonth1 = mpr.PayMonth,
                        Vendor = mp.Vendor,
                        DoPayUserName = "",
                    }
            );

            var items = hasAP ? baseItems.Where(i => hasPayIds.Contains(i.Id)) : baseItems.Where(i => !hasPayIds.Contains(i.Id));

            var result = items
                    .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                    .Select(i => new MPSReceivedLogForAP
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        RefLocaleId = i.RefLocaleId,
                        ReceivedDate = i.ReceivedDate,
                        ReceivedQty = i.ReceivedQty,
                        ChargeQty = i.ChargeQty,
                        FreeQty = i.FreeQty,
                        QCBackQty = i.QCBackQty,
                        DoPay = i.DoPay,
                        ModifyUserName = i.ModifyUserName,
                        LastUpdateTime = i.LastUpdateTime,
                        Remark = i.Remark,
                        PayMonth = i.PayMonth,
                        AddFreeQty = i.AddFreeQty,
                        DiscountRate = i.DiscountRate,
                        WarehouseNo = i.WarehouseNo,

                        OrderNo = i.OrderNo,
                        PONo = i.PONo,
                        StyleNo = i.StyleNo,
                        PurUnitName = i.PurUnitName,
                        DayOfMonth = i.DayOfMonth,
                        PriceType = i.PriceType,
                        VendorId = i.VendorId,
                        UnitPrice = i.UnitPrice,
                        DollarNameTw = i.DollarNameTw,
                        PaymentLocaleId = i.PaymentLocaleId,
                        ProcedureNameTw = i.ProcedureNameTw,
                        CSD = i.CSD,
                        Amount = i.ChargeQty * i.UnitPrice,
                        Vendor = i.Vendor,
                    })
                    .OrderBy(i => i.ReceivedDate)
                    .ToList()
                    .AsQueryable();

            return result;
        }

        public List<MPSReceivedLogForAP> Save(List<MPSReceivedLogForAP> items)
        {
            try
            {
                UnitOfWork.BeginTransaction();

                var localeId = items[0].LocaleId;
                var doPayUserName = items[0].DoPayUserName;

                var Ids = items.Select(i => i.Id).ToList();
                var doPayIds = items.Where(i => i.DoPay == 1).Select(i => i.Id).ToList();
                var noPayIds = items.Where(i => i.DoPay == 0).Select(i => i.Id).ToList();

                MPSReceivedLog.UpdateDoPay((int)localeId, doPayIds, noPayIds);

                var aps = items.Where(i => i.DoPay == 1).Select(i => new MPSAP
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    MpsReceivedLogId = i.Id,
                    MpsProcedurePOId = i.POId,
                    PayMonth = i.PayMonth,
                    Vendor = i.Vendor,
                    PaymentLocaleId = (decimal)i.PaymentLocaleId,
                    ChargeQty = i.ChargeQty,
                    PurUnitName = i.PurUnitName,
                    UnitPrice = (decimal)i.UnitPrice,
                    DollarNameTw = i.DollarNameTw,
                    AdjustAmount = 0,
                    SubAmount = (decimal)i.Amount,
                    Remark = i.Remark,
                    OrderNo = i.OrderNo,
                    ModifyUserName = doPayUserName,
                    LastUpdateTime = DateTime.Now,
                })
                .ToList();

                // 新增MPS AP
                MPSAP.RemoveRange(i => Ids.Contains(i.MpsReceivedLogId) && i.LocaleId == localeId);
                MPSAP.CreateRange(aps);

                UnitOfWork.Commit();
                return items;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        // 暫時沒用到
        public ERP.Models.Views.MPSOutsourceAPGroup Get(int id, int localeId)
        {
            var group = new MPSOutsourceAPGroup();

            var ap = MPSAP.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (ap != null)
            {
                var apItems = (
                    from mpr in MPSReceivedLog.Get()
                    join mp in MPSProcedurePO.Get() on new { MPSProcedurePOId = mpr.MpsProcedurePOId, LocaleId = mpr.LocaleId } equals new { MPSProcedurePOId = mp.Id, LocaleId = mp.LocaleId }
                    join mpi in MPSProcedurePOItem.Get() on new { MPSProcedurePOId = mp.Id, LocaleId = mp.LocaleId } equals new { MPSProcedurePOId = mpi.MpsProcedurePOId, LocaleId = mpi.LocaleId }
                    where mpr.PayMonth == ap.PayMonth && mp.Vendor == ap.Vendor && mp.PaymentLocaleId == ap.PaymentLocaleId
                    select new Models.Views.MPSReceivedLogForAP
                    {

                        Id = mpr.Id,
                        LocaleId = mpr.LocaleId,
                        RefLocaleId = mpr.RefLocaleId,
                        ReceivedDate = mpr.ReceivedDate,
                        ReceivedQty = mpr.ReceivedQty,
                        ChargeQty = mpr.ChargeQty,
                        FreeQty = mpr.FreeQty,
                        QCBackQty = mpr.QCBackQty,
                        DoPay = mpr.doPay,
                        ModifyUserName = mpr.ModifyUserName,
                        LastUpdateTime = mpr.LastUpdateTime,
                        Remark = mpr.Remark,
                        PayMonth = mpr.PayMonth,
                        AddFreeQty = mpr.AddFreeQty,
                        DiscountRate = mpr.DiscountRate,
                        WarehouseNo = mpr.WarehouseNo,

                        OrderNo = mp.OrderNo,
                        PONo = mp.PONo,
                        StyleNo = mp.StyleNo,
                        PurUnitName = mp.PurUnitName,
                        DayOfMonth = mp.DayOfMonth,
                        PriceType = mp.PriceType,
                        VendorId = mp.MpsProcedureVendorId,
                        UnitPrice = mp.UnitPrice,
                        DollarNameTw = mp.DollarNameTw,
                        PaymentLocaleId = mp.PaymentLocaleId,
                        ProcedureNameTw = mpi.ProcedureNameTw,
                        CSD = Orders.Get().Where(i => i.OrderNo == mp.OrderNo && i.LocaleId == mp.LocaleId).Max(i => i.CSD),
                    }
                )
                .ToList();

                group.MPSAP = ap;
                group.MPSReceivedLogForAP = apItems;
            }
            return group;
        }
        // 暫時沒用到
        public MPSOutsourceAPGroup Save(MPSOutsourceAPGroup group)
        {
            var ap = group.MPSAP;
            var apItems = group.MPSReceivedLogForAP.ToList();
            try
            {
                UnitOfWork.BeginTransaction();

                var doPayIds = apItems.Where(i => i.DoPay == 1).Select(i => i.Id).ToList();
                var noPayIds = apItems.Where(i => i.DoPay == 0).Select(i => i.Id).ToList();
                MPSReceivedLog.UpdateDoPay((int)ap.LocaleId, doPayIds, noPayIds);

                UnitOfWork.Commit();
                return Get((int)group.MPSAP.Id, (int)group.MPSAP.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
