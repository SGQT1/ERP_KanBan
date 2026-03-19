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
    public class MaterialProcessService : BusinessService
    {
        private Services.Entities.TransferService Transfer { get; }
        private Services.Entities.TransferItemService TransferItem { get; }
        private Services.Entities.ReceivedLogService ReceivedLog { get; }
        private Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; }
        private Services.Entities.ShipmentLogService ShipmentLog { get; }
        private Services.Entities.POService PO { get; }
        private Services.Entities.POItemService POItem { get; }
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.VendorService Vendor { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.OrdersService Orders { get; }
        
        private Services.Entities.MaterialStockService MaterialStock { get; }
        private Services.Entities.StockIOService StockIO { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        public MaterialProcessService(
            Services.Entities.TransferService transferService,
            Services.Entities.TransferItemService transferItemService,
            Services.Entities.ReceivedLogService receivedLogService,
            Services.Entities.ReceivedLogAddService receivedLogAddService, 
            Services.Entities.ShipmentLogService shipmentLogService,
            Services.Entities.POService poService,
            Services.Entities.POItemService poItemService,
            Services.Entities.StockIOService stockIOService,
            Services.Entities.MaterialService materialService,
            Services.Entities.VendorService vendorService,
            Services.Entities.CompanyService companyService,
            Services.Entities.OrdersService ordersService,
            Services.Entities.MaterialStockService materialStockService,
            Services.Entities.CodeItemService codeItemService,
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
            this.StockIO = stockIOService;
            this.CodeItem = codeItemService;
            this.ShipmentLog = shipmentLogService;
            this.MaterialStock = materialStockService;
        }
        public IQueryable<Models.Views.POItemProcess> Get(string predicate)
        {
            var stockIOs = (
                from s in StockIO.Get()
                group s by new { s.MaterialStockId, s.LocaleId } into g
                select new
                {
                    MaterialStockId = g.Key.MaterialStockId,
                    LocaleId = g.Key.LocaleId,
                    PCLIOQty = g.Sum(x => x.PCLIOQty)   // 非 null decimal
                }
            );
            var result = (
                    from pi in POItem.Get()
                    join p in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = p.Id, LocaleId = p.LocaleId }
                    join m in Material.Get() on new { MId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId }
                    join rcl in ReceivedLog.Get().Where(i => i.TransferInId == 0).Select(i => new { i.POItemId, i.LocaleId, i.Id, i.RefLocaleId, i.ReceivedDate, i.IQCGetQty, i.ReceivedQty, i.QCDate}) on new { POItemId = pi.Id, LocaleId = pi.LocaleId } equals new { POItemId = rcl.POItemId, LocaleId = rcl.RefLocaleId } into rclGRP
                    from rcl in rclGRP.DefaultIfEmpty()
                    join rcla in ReceivedLogAdd.Get() on new { ReceivedLogId = rcl.Id, LocaleId = rcl.LocaleId } equals new { ReceivedLogId = rcla.ReceivedLogId, LocaleId = rcla.LocaleId } into rclaGRP
                    from rcla in rclaGRP.DefaultIfEmpty()
                    // join o in Orders.Get().GroupBy(i => new { i.StyleNo, i.OrderNo}).Select(i => new { StyleNo = i.Key.StyleNo, OrderNo = i.Key.OrderNo,LCSD = i.Min(g => g.LCSD), CSD = i.Min(g => g.CSD)}) on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                    join o in Orders.Get().Select(i => new { StyleNo = i.StyleNo, OrderNo = i.OrderNo, LCSD = i.LCSD, CSD = i.CSD}) on new { OrderNo = pi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                    from o in oGRP.DefaultIfEmpty()
                    join s in ShipmentLog.Get() on new { OrderNo = o.OrderNo } equals new { OrderNo = s.OrderNo} into sGRP
                    from s in sGRP.DefaultIfEmpty()
                    join v in Vendor.Get() on new { VendorId = p.VendorId, LocaleId = p.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                    from v in vGRP.DefaultIfEmpty()
                    join ms in MaterialStock.Get() on new { OrderNo = pi.OrderNo, MaterialId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { OrderNo = ms.OrderNo, MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } into msGRP
                    from ms in msGRP.DefaultIfEmpty()
                    join si in stockIOs on new { MaterialStockId = ms.Id, LocaleId = ms.LocaleId } equals new { MaterialStockId = si.MaterialStockId, LocaleId = si.LocaleId } into siGRP
                    from si in siGRP.DefaultIfEmpty()
                    select new Models.Views.POItemProcess {
                        Vendor = v.NameTw,
                        MaterialId = pi.MaterialId,
                        MaterialNameTw = m.MaterialName,
                        PODate = p.PODate,
                        ReceivedDate = rcl.ReceivedDate,
                        QCDate = rcl.QCDate,
                        PurUnitCodeId = pi.UnitCodeId,
                        PurUnitCode = CodeItem.Get().Where(i => i.Id == pi.UnitCodeId && i.LocaleId == pi.LocaleId && i.CodeType == "21").Max(i => i.NameTW),
                        PurQty = pi.Qty,
                        IQCGetQty = rcl.IQCGetQty,
                        ReceivedQty = rcl.ReceivedQty,
                        MaterialStockQty = ms.PCLQty,
                        StockQty = (decimal?)si.PCLIOQty ?? 0,
                        // StockQty = StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId).Sum(i => i.PCLIOQty),

                        OrdersId = pi.OrdersId,
                        OrderNo = pi.OrderNo,
                        StyleNo = o.StyleNo,
                        VendorETD = p.VendorETD,
                        LCSD = o.LCSD,
                        CSD = o.CSD,
                        CompanyId = pi.CompanyId,
                        ReceivedLocaleId = pi.ReceivingLocaleId, //收貨地
                        PaymentLocaleId = pi.PaymentLocaleId,
                        PurLocaleId = pi.PurLocaleId,
                        LocaleId = pi.LocaleId,
                        PONo = p.BatchNo + "-" + p.SeqId.ToString(),
                        POItemId = pi.Id,
                        ShipmentDate = s.SaleDate
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList()
                .AsQueryable();

                return result;
        }
    }
}