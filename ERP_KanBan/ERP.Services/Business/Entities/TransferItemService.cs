using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class TransferItemService : BusinessService
    {
        private Services.Entities.TransferService Transfer { get; }
        private Services.Entities.TransferItemService TransferItem { get; }
        private Services.Entities.ReceivedLogService ReceivedLog { get; }
        private Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; }

        private Services.Entities.POService PO { get; }
        private Services.Entities.POItemService POItem { get; }
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.VendorService Vendor { get; }
        private Services.Entities.CompanyService Company { get; }

        public TransferItemService(
            Services.Entities.TransferService transferService,
            Services.Entities.TransferItemService transferItemService,
            Services.Entities.ReceivedLogService receivedLogService,
            Services.Entities.ReceivedLogAddService receivedLogAddService,
            Services.Entities.POService poService,
            Services.Entities.POItemService poItemService,
            Services.Entities.MaterialService materialService,
            Services.Entities.VendorService vendorService,
            Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Transfer = transferService;
            this.TransferItem = transferItemService;
            this.ReceivedLog = receivedLogService;
            this.ReceivedLogAdd = receivedLogAddService;
            this.PO = poService;
            this.POItem = poItemService;
            this.Material = materialService;
            this.Vendor = vendorService;
            this.Company = companyService;
        }
        public IQueryable<Models.Views.Transfer> GetTransfers(string predicate)
        {
            var result = (
                from t in Transfer.Get()
                join ti in TransferItem.Get() on new { Id = t.Id, LocaleId = t.LocaleId } equals new { Id = ti.TransferId, LocaleId = ti.LocaleId }
                join rcl in ReceivedLog.Get() on new { ReceivedLogId = ti.ReceivedLogId, LocaleId = ti.LocaleId } equals new { ReceivedLogId = rcl.Id, LocaleId = rcl.LocaleId } into rclGRP
                from rcl in rclGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rcl.POItemId, LocaleId = rcl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join po in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId } into poGRP
                from po in poGRP.DefaultIfEmpty()
                select new
                {
                    Id = t.Id,
                    LocaleId = t.LocaleId,
                    ContainerNo = t.ContainerNo,
                    ShipmentNo = t.ShipmentNo,
                    ShippingDate = t.ShippingDate,
                    Vessel = t.Vessel,
                    OBDate = t.OBDate,
                    ArrivalDate = t.ArrivalDate,
                    ShippingPortId = t.ShippingPortId,
                    ShippingPortName = t.ShippingPortName,
                    TargetPortId = t.TargetPortId,
                    TargetPortName = t.TargetPortName,
                    ModifyUserName = t.ModifyUserName,
                    LastUpdateTime = t.LastUpdateTime,
                    PaymentLocaleId = t.PaymentLocaleId,
                    UnitPricePercent = t.UnitPricePercent,
                    Remark = t.Remark,
                    OrderNo = pi.OrderNo,
                    MaterialNameTw = ti.MaterialNameTwCust,
                    PONo = po.BatchNo + "-" + po.SeqId,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(t => new Models.Views.Transfer
                {
                    Id = t.Id,
                    LocaleId = t.LocaleId,
                    ContainerNo = t.ContainerNo,
                    ShipmentNo = t.ShipmentNo,
                    ShippingDate = t.ShippingDate,
                    Vessel = t.Vessel,
                    OBDate = t.OBDate,
                    ArrivalDate = t.ArrivalDate,
                    ShippingPortId = t.ShippingPortId,
                    ShippingPortName = t.ShippingPortName,
                    TargetPortId = t.TargetPortId,
                    TargetPortName = t.TargetPortName,
                    ModifyUserName = t.ModifyUserName,
                    LastUpdateTime = t.LastUpdateTime,
                    PaymentLocaleId = t.PaymentLocaleId,
                    UnitPricePercent = t.UnitPricePercent,
                    Remark = t.Remark,
                })
                .Distinct()
                .ToList()
                .AsQueryable();
            return result;
        }
        public IQueryable<Models.Views.TransferItem> Get()
        {
            var result = (
                from t in Transfer.Get()
                join ti in TransferItem.Get() on new { Id = t.Id, LocaleId = t.LocaleId } equals new { Id = ti.TransferId, LocaleId = ti.LocaleId }
                join rcl in ReceivedLog.Get() on new { ReceivedLogId = ti.ReceivedLogId, LocaleId = ti.LocaleId } equals new { ReceivedLogId = rcl.Id, LocaleId = rcl.LocaleId } into rclGRP
                from rcl in rclGRP.DefaultIfEmpty()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rcl.Id, LocaleId = rcl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rcl.POItemId, LocaleId = rcl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join po in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId } into poGRP
                from po in poGRP.DefaultIfEmpty()
                join rclt in ReceivedLog.Get() on new { ReceivedLogId = ti.Id, LocaleId = ti.LocaleId } equals new { ReceivedLogId = rclt.TransferInId, LocaleId = rclt.TransferInLocaleId } into rcltGRP
                from rclt in rcltGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rcl.ShippingListVendorId, LocaleId = rcl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.TransferItem
                {
                    Id = ti.Id,
                    LocaleId = ti.LocaleId,
                    TransferId = ti.TransferId,
                    ReceivedLogId = ti.ReceivedLogId,
                    TransferQty = ti.TransferQty,
                    TargetCompanyId = ti.TargetCompanyId,
                    TargetCompany = Company.Get().Where(i => i.Id == ti.TargetCompanyId).Max(i => i.CompanyNo),
                    MaterialNameTwCust = ti.MaterialNameTwCust,
                    MaterialNameEnCust = ti.MaterialNameEnCust,
                    TransferQtyCust = ti.TransferQtyCust,
                    UnitCodeNameTwCust = ti.UnitCodeNameTwCust,
                    DollarCodeNameTwCust = ti.DollarCodeNameTwCust,
                    UnitPriceCust = ti.UnitPriceCust,
                    TaxRateCust = ti.TaxRateCust,
                    AmountCust = ti.AmountCust,
                    WeiUnitCodeNameTwCust = ti.WeiUnitCodeNameTwCust,
                    NetWeight = ti.NetWeight,
                    GrossWeight = ti.GrossWeight,
                    SubCount = ti.SubCount,
                    Mark = ti.Mark,
                    Remark = ti.Remark,
                    TransferType = ti.TransferType,
                    ModifyUserName = ti.ModifyUserName, // 轉出人
                    LastUpdateTime = ti.LastUpdateTime,

                    ShipmentNo = t.ShipmentNo,
                    ShippingDate = t.ShippingDate,
                    OrderNo = rcl.OrderNo,
                    // StyleNo = 
                    UnitPrice = rcl.UnitPrice,
                    UnitCodeId = pi.UnitCodeId,
                    MaterialId = m.Id,
                    MaterialNameTw = m.MaterialName,
                    MaterialNameEn = m.MaterialNameEng,
                    Vendor = v.ShortNameTw,
                    HasReceivedLogId = rclt.Id,
                    POItemId = pi.Id,
                    SamplingMethod = rcl.SamplingMethod,
                    ReceivedLogLocaleId = rcl.LocaleId,
                    POItemLocaleId = pi.LocaleId,
                    PONo = po.BatchNo + "-" + po.SeqId,
                    Recipient = rcl.ModifyUserName, // 收貨人
                    FreeQty = rla.FreeQty,
                    PayQty = rla.PayQty,
                    TargetReceivedLogId = ReceivedLog.Get().Where(i => i.TransferInId == ti.Id && i.TransferInLocaleId == ti.LocaleId && i.LocaleId == ti.TargetCompanyId).Max(i => i.Id)                  
                });
            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.TransferItem> items)
        {
            TransferItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.TransferItem, bool>> predicate)
        {
            TransferItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.TransferItem> BuildRange(IEnumerable<Models.Views.TransferItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.TransferItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                TransferId = item.TransferId,
                ReceivedLogId = item.ReceivedLogId,
                TransferQty = item.TransferQty,
                TargetCompanyId = item.TargetCompanyId,
                MaterialNameTwCust = item.MaterialNameTwCust,
                MaterialNameEnCust = item.MaterialNameEnCust,
                TransferQtyCust = item.TransferQtyCust,
                UnitCodeNameTwCust = item.UnitCodeNameTwCust,
                DollarCodeNameTwCust = item.DollarCodeNameTwCust,
                UnitPriceCust = item.UnitPriceCust,
                TaxRateCust = item.TaxRateCust,
                AmountCust = item.TransferQty * item.UnitPriceCust,
                WeiUnitCodeNameTwCust = item.WeiUnitCodeNameTwCust,
                NetWeight = Math.Round(item.NetWeight, 2, MidpointRounding.AwayFromZero),
                GrossWeight = Math.Round(item.GrossWeight, 2, MidpointRounding.AwayFromZero),
                SubCount = item.SubCount,
                Mark = item.Mark,
                Remark = item.Remark,
                TransferType = item.TransferType,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}