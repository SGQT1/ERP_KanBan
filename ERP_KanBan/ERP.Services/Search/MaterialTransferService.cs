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


namespace ERP.Services.Search
{
    public class MaterialTransferService : BusinessService
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
        private Services.Entities.OrdersService Orders { get; }

        public MaterialTransferService(
            Services.Entities.TransferService transferService,
            Services.Entities.TransferItemService transferItemService,
            Services.Entities.ReceivedLogService receivedLogService,
            Services.Entities.ReceivedLogAddService receivedLogAddService, 
            Services.Entities.POService poService,
            Services.Entities.POItemService poItemService,
            Services.Entities.MaterialService materialService,
            Services.Entities.VendorService vendorService,
            Services.Entities.CompanyService companyService,
            Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Transfer = transferService;
            this.TransferItem = transferItemService;
            this.ReceivedLog = receivedLogService;
            this.PO = poService;
            this.POItem = poItemService;
            this.Material = materialService;
            this.Vendor = vendorService;
            this.Company = companyService;
            this.Orders = ordersService;
            this.ReceivedLogAdd = receivedLogAddService;
        }
        public IQueryable<Models.Views.TransferItem> Get(string predicate)
        {
            var result = (
                from t in Transfer.Get()
                join ti in TransferItem.Get() on new { Id = t.Id, LocaleId = t.LocaleId } equals new { Id = ti.TransferId, LocaleId = ti.LocaleId }
                join rcl in ReceivedLog.Get() on new { ReceivedLogId = ti.ReceivedLogId, LocaleId = ti.LocaleId } equals new { ReceivedLogId = rcl.Id, LocaleId = rcl.LocaleId } into rclGRP
                from rcl in rclGRP.DefaultIfEmpty()
                join rcla in ReceivedLogAdd.Get() on new { ReceivedLogId = rcl.Id, LocaleId = rcl.LocaleId } equals new { ReceivedLogId = rcla.ReceivedLogId, LocaleId = rcla.LocaleId } into rclaGRP
                from rcla in rclaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItemId = rcl.POItemId, LocaleId = rcl.RefLocaleId } equals new { POItemId = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join o in Orders.Get().GroupBy(i => new { i.StyleNo, i.OrderNo}).Select(i => new { StyleNo = i.Key.StyleNo, OrderNo = i.Key.OrderNo, CSD = i.Min(g => g.CSD)}) on new { OrderNo = rcl.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rcl.ShippingListVendorId, LocaleId = rcl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new
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

                    ContainerNo = t.ContainerNo,
                    ShipmentNo = t.ShipmentNo,
                    ShippingDate = t.ShippingDate,
                    PaymentLocaleId = t.PaymentLocaleId,
                    OBDate = t.OBDate,
                    ArrivalDate = t.ArrivalDate,
                    ShippingPortName = t.ShippingPortName,
                    TargetPortName = t.TargetPortName,
                    
                    OrderNo = o.OrderNo,
                    CSD = o.CSD,
                    StyleNo = o.StyleNo,
                                       
                    UnitPrice = rcl.UnitPrice,
                    SamplingMethod = rcl.SamplingMethod,
                    ReceivedLogLocaleId = rcl.LocaleId,
                    Recipient = rcl.ModifyUserName, // 收貨人
                    Vendor = v.ShortNameTw,
                    PayQty = rcla.PayQty,
                    FreeQty = rcla.FreeQty,
                    RefMaterialId = pi.MaterialId,
                    RefParentMaterialId = pi.ParentMaterialId,

                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(ti => new Models.Views.TransferItem
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

                    ContainerNo = ti.ContainerNo,
                    ShipmentNo = ti.ShipmentNo,
                    ShippingDate = ti.ShippingDate,
                    OrderNo = ti.OrderNo,
                    CSD = ti.CSD,
                    StyleNo = ti.StyleNo,
                    PaymentLocaleId = ti.PaymentLocaleId,

                    // UnitPrice = ti.UnitPrice,
                    // UnitCodeId = ti.UnitCodeId,
                    MaterialId = ti.Id,
                    // MaterialNameTw = ti.MaterialNameTw,
                    // MaterialNameEn = ti.MaterialNameEn,
                    Vendor = ti.Vendor,
                    HasReceivedLogId = ti.Id,
                    POItemId = ti.Id,
                    SamplingMethod = ti.SamplingMethod,
                    ReceivedLogLocaleId = ti.LocaleId,
                    POItemLocaleId = ti.LocaleId,
                    // PONo = ti.PONo,
                    Recipient = ti.ModifyUserName, // 收貨人
                    PayQty = ti.PayQty,
                    FreeQty = ti.FreeQty,
                    RefMaterialId = ti.RefMaterialId,
                    RefParentMaterialId = ti.RefParentMaterialId,
                })
                .ToList()
                .AsQueryable();

                return result;
        }
    }
}