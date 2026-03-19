using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSProcedurePOService : BusinessService
    {
        private ERP.Services.Entities.MpsOrdersService MPSOrders { get; set; }
        private ERP.Services.Entities.MpsProcedurePOService MPSProcedurePO { get; set; }
        private ERP.Services.Entities.MpsProcedureVendorService MPSProcedureVendor { get; set; }

        public MPSProcedurePOService(
            ERP.Services.Entities.MpsOrdersService mpsOrdersService,
            ERP.Services.Entities.MpsProcedurePOService mpsProcedurePOService,
            ERP.Services.Entities.MpsProcedureVendorService mpsProcedureVendor,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSOrders = mpsOrdersService;
            MPSProcedurePO = mpsProcedurePOService;
            MPSProcedureVendor = mpsProcedureVendor;
        }

        public IQueryable<Models.Views.MPSProcedurePO> Get()
        {
            return (
                from mp in MPSProcedurePO.Get()
                join mpv in MPSProcedureVendor.Get() on new { MpsProcedureVendorId = mp.MpsProcedureVendorId, LocaleId = mp.LocaleId } equals new { MpsProcedureVendorId = mpv.Id, LocaleId = mpv.LocaleId } into mpvGRP
                from mpv in mpvGRP.DefaultIfEmpty()
                join o in MPSOrders.Get() on new { OrderNo = mp.OrderNo, LocaleId = mp.LocaleId } equals new { OrderNo = o.OrderNo, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                select new Models.Views.MPSProcedurePO
                {
                    Id = mp.Id,
                    LocaleId = mp.LocaleId,
                    Type = mp.Type,
                    MpsProcedureVendorId = mp.MpsProcedureVendorId,
                    OrderNo = mp.OrderNo,
                    SeqId = mp.SeqId,
                    PONo = mp.PONo,
                    StyleNo = mp.StyleNo,
                    MpsProcedureGroupNameTw = mp.MpsProcedureGroupNameTw,
                    PODate = mp.PODate,
                    UnitPrice = mp.UnitPrice,
                    DollarNameTw = mp.DollarNameTw,
                    Qty = mp.Qty,
                    PurUnitName = mp.PurUnitName,
                    VendorETD = mp.VendorETD,
                    PayCodeId = mp.PayCodeId,
                    ReceivingLocaleId = mp.ReceivingLocaleId,
                    PaymentLocaleId = mp.PaymentLocaleId,
                    PaymentCodeId = mp.PaymentCodeId,
                    PaymentPoint = mp.PaymentPoint,
                    IsOverQty = mp.IsOverQty,
                    IsAllowPartial = mp.IsAllowPartial,
                    IsShowSizeRun = mp.IsShowSizeRun,
                    SamplingMethod = mp.SamplingMethod,
                    Status = mp.Status,
                    SpecDesc = mp.SpecDesc,
                    Remark = mp.Remark,
                    PhotoURL = mp.PhotoURL,
                    PhotoURLDescTw = mp.PhotoURLDescTw,
                    ModifyUserName = mp.ModifyUserName,
                    LastUpdateTime = mp.LastUpdateTime,
                    PayQty = mp.PayQty,
                    WarehouseNo = mp.WarehouseNo,
                    PriceType = mp.PriceType,
                    MPSVendor = mpv.NameTw,
                    Vendor = mpv.NameTw,
                    MPSVendorShortName = mpv.ShortNameTw,
                    Amount = mp.UnitPrice * mp.PayQty,
                    DayOfMonth = mpv.DayOfMonth,
                    OrderQty = o.OrderQty,
                    TotalOutsouceQty = MPSProcedurePO.Get().Where(i => i.OrderNo == mp.OrderNo && i.LocaleId == mp.LocaleId && i.MpsProcedureGroupNameTw == mp.MpsProcedureGroupNameTw).Sum(i => i.Qty),
                }
            );
        }

        public IQueryable<Models.Views.MPSProcedurePO> GetEntity()
        {
            return MPSProcedurePO.Get().Select(mp => new Models.Views.MPSProcedurePO
            {
                Id = mp.Id,
                LocaleId = mp.LocaleId,
                Type = mp.Type,
                MpsProcedureVendorId = mp.MpsProcedureVendorId,
                OrderNo = mp.OrderNo,
                SeqId = mp.SeqId,
                PONo = mp.PONo,
                StyleNo = mp.StyleNo,
                MpsProcedureGroupNameTw = mp.MpsProcedureGroupNameTw,
                PODate = mp.PODate,
                UnitPrice = mp.UnitPrice,
                DollarNameTw = mp.DollarNameTw,
                Qty = mp.Qty,
                PurUnitName = mp.PurUnitName,
                VendorETD = mp.VendorETD,
                PayCodeId = mp.PayCodeId,
                ReceivingLocaleId = mp.ReceivingLocaleId,
                PaymentLocaleId = mp.PaymentLocaleId,
                PaymentCodeId = mp.PaymentCodeId,
                PaymentPoint = mp.PaymentPoint,
                IsOverQty = mp.IsOverQty,
                IsAllowPartial = mp.IsAllowPartial,
                IsShowSizeRun = mp.IsShowSizeRun,
                SamplingMethod = mp.SamplingMethod,
                Status = mp.Status,
                SpecDesc = mp.SpecDesc,
                Remark = mp.Remark,
                PhotoURL = mp.PhotoURL,
                PhotoURLDescTw = mp.PhotoURLDescTw,
                ModifyUserName = mp.ModifyUserName,
                LastUpdateTime = mp.LastUpdateTime,
                PayQty = mp.PayQty,
                WarehouseNo = mp.WarehouseNo,
                PriceType = mp.PriceType,
            });
        }
        public Models.Views.MPSProcedurePO Create(Models.Views.MPSProcedurePO item)
        {
            var _item = MPSProcedurePO.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcedurePO Update(Models.Views.MPSProcedurePO item)
        {
            var _item = MPSProcedurePO.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcedurePO item)
        {
            MPSProcedurePO.Remove(Build(item));
        }
        private Models.Entities.MpsProcedurePO Build(Models.Views.MPSProcedurePO item)
        {
            return new Models.Entities.MpsProcedurePO()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                Type = item.Type,
                MpsProcedureVendorId = item.MpsProcedureVendorId,
                OrderNo = item.OrderNo,
                SeqId = item.SeqId,
                PONo = item.PONo,
                StyleNo = item.StyleNo,
                MpsProcedureGroupNameTw = item.MpsProcedureGroupNameTw,
                PODate = item.PODate,
                UnitPrice = item.UnitPrice,
                DollarNameTw = item.DollarNameTw,
                Qty = item.Qty,
                PurUnitName = item.PurUnitName,
                VendorETD = item.VendorETD,
                PayCodeId = item.PayCodeId,
                ReceivingLocaleId = item.ReceivingLocaleId,
                PaymentLocaleId = item.PaymentLocaleId,
                PaymentCodeId = item.PaymentCodeId,
                PaymentPoint = item.PaymentPoint,
                IsOverQty = item.IsOverQty,
                IsAllowPartial = item.IsAllowPartial,
                IsShowSizeRun = item.IsShowSizeRun,
                SamplingMethod = item.SamplingMethod,
                Status = item.Status,
                SpecDesc = item.SpecDesc,
                Remark = item.Remark,
                PhotoURL = item.PhotoURL,
                PhotoURLDescTw = item.PhotoURLDescTw,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PayQty = item.PayQty,
                WarehouseNo = item.WarehouseNo,
                PriceType = item.PriceType,
            };
        }

        public void CreateRange(List<Models.Views.MPSProcedurePO> items)
        {
            MPSProcedurePO.CreateRange(BuildRange(items));
        }
        private IEnumerable<ERP.Models.Entities.MpsProcedurePO> BuildRange(IEnumerable<Models.Views.MPSProcedurePO> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcedurePO
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                Type = item.Type,
                MpsProcedureVendorId = item.MpsProcedureVendorId,
                OrderNo = item.OrderNo,
                SeqId = item.SeqId,
                PONo = item.PONo,
                StyleNo = item.StyleNo,
                MpsProcedureGroupNameTw = item.MpsProcedureGroupNameTw,
                PODate = item.PODate,
                UnitPrice = item.UnitPrice,
                DollarNameTw = item.DollarNameTw,
                Qty = item.Qty,
                PurUnitName = item.PurUnitName,
                VendorETD = item.VendorETD,
                PayCodeId = item.PayCodeId,
                ReceivingLocaleId = item.ReceivingLocaleId,
                PaymentLocaleId = item.PaymentLocaleId,
                PaymentCodeId = item.PaymentCodeId,
                PaymentPoint = item.PaymentPoint,
                IsOverQty = item.IsOverQty,
                IsAllowPartial = item.IsAllowPartial,
                IsShowSizeRun = item.IsShowSizeRun,
                SamplingMethod = item.SamplingMethod,
                Status = item.Status,
                SpecDesc = item.SpecDesc,
                Remark = item.Remark,
                PhotoURL = item.PhotoURL,
                PhotoURLDescTw = item.PhotoURLDescTw,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PayQty = item.PayQty,
                WarehouseNo = item.WarehouseNo,
                PriceType = item.PriceType,
            });
        }
    }
}
