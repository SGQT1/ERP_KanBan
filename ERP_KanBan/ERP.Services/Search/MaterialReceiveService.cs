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
    public class MaterialReceiveService : BusinessService
    {
        private Services.Entities.ReceivedLogService ReceivedLog { get; }
        private Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; }
        private Services.Entities.TransferItemService TransferItem { get; }
        private Services.Entities.APMonthItemService APMonthItem { get; }

        private Services.Entities.VendorService Vendor { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.POItemService POItem { get; }
        private Services.Entities.WarehouseService Warehouse { get; }

        private Services.Entities.StockIOService StockIO { get; }

        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }

        public MaterialReceiveService(
            Services.Entities.ReceivedLogService receivedLogService,
            Services.Entities.ReceivedLogAddService receivedLogAddService,
            Services.Entities.APMonthItemService apMonthItemService,

            Services.Entities.TransferItemService transferItemService,
            Services.Entities.VendorService vendorService,
            Services.Entities.CompanyService companyService,
            Services.Entities.MaterialService materialService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.POItemService poItemService,
            Services.Business.Entities.MaterialStockItemService materialStockItemService,
            Services.Entities.WarehouseService warehouseService,
            Services.Entities.StockIOService stockIOService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.ReceivedLog = receivedLogService;
            this.ReceivedLogAdd = receivedLogAddService;
            this.APMonthItem = apMonthItemService;

            this.TransferItem = transferItemService;
            this.Vendor = vendorService;
            this.Company = companyService;
            this.Material = materialService;
            this.CodeItem = codeItemService;
            this.POItem = poItemService;
            this.MaterialStockItem = materialStockItemService;
            this.Warehouse = warehouseService;
            this.StockIO = stockIOService;
        }
        public IQueryable<Models.Views.ReceivedLog> Get()
        {
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                join w in Warehouse.Get() on new {WarehouseId = rl.WarehouseId, LocaleId = rl.LocaleId } equals new { WarehouseId = w.Id, LocaleId = w.LocaleId } into wGRP
                from w in wGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = rla.MaterialId, LocaleId = rla.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                select new Models.Views.ReceivedLog
                {
                    TransferInId = rl.TransferInId,
                    TransferInLocaleId = rl.TransferInLocaleId,
                    PaymentLocaleId = pi.PaymentLocaleId,
                    ReceivingLocaleId = pi.ReceivingLocaleId,
                    ReceivedLocaleId = pi.ReceivingLocaleId,
                    ReceivedDate = rl.ReceivedDate,
                    RefPONo = rla.RefPONo,
                    ShippingListVendorId = rl.ShippingListVendorId,
                    ShippingListVendorName = v.NameTw,
                    MaterialId = rla.MaterialId,
                    MaterialNameTw = rla.MaterialNameTw,
                    MaterialNameEng = m.MaterialNameEng,
                    PCLUnitCodeId = rla.PCLUnitCodeId,
                    PCLUnitNameTw = rla.PCLUnitNameTw,
                    PurUnitCodeId = rla.PurUnitCodeId,
                    PurUnitNameTw = rla.PurUnitNameTw,
                    ReceivedQty = rl.ReceivedQty,
                    IQCGetQty = rl.IQCGetQty,
                    StockQty = rl.StockQty,
                    WarehouseId = rl.WarehouseId,
                    WarehouseNo = w.WarehouseNo,
                    ReceivedBarcode = rl.RefLocaleId.ToString() + "*" + rl.POItemId.ToString(),
                    OrderNo = rl.OrderNo,
                    ETD = pi.FactoryETD,
                    ModifyUserName = rl.ModifyUserName,
                    LastUpdateTime = rl.LastUpdateTime,
                    NetWeight = rl.NetWeight,
                    GrossWeight = rl.GrossWeight,
                    WeightUnitCodeId = rl.WeightUnitCodeId,
                    CloseMonth = rla.CloseMonth,

                    Id = rl.Id,
                    LocaleId = rl.LocaleId,
                    POItemId = rl.POItemId,
                    RefLocaleId = rl.RefLocaleId,
                    ShippingListNo = rl.ShippingListNo,
                    UnitPrice = rl.UnitPrice,
                    SubTotalPrice = rl.SubTotalPrice,

                    // Remark = rl.Remark,
                    // APId = rl.APId,
                    // QCDate = rl.QCDate,
                    // IQCFlag = rl.IQCFlag,
                    // IQCTestQty = rl.IQCTestQty,
                    // IQCPassQty = rl.IQCPassQty,
                    // IQCRejectionQty = rl.IQCPassQty,
                    // IQCResult = rl.IQCResult,
                    // IQCMen = rl.IQCMen,
                    // IQCRemark = rl.IQCRemark,
                    // SamplingMethod = rl.SamplingMethod,
                    // IsAccounting = rl.IsAccounting,
                    // TaiwanInvoiceNo = rl.TaiwanInvoiceNo,
                    // TransferQty = rl.TransferQty,
                    // ReceivedCount = rl.ReceivedCount,
                    // ReceivedLogId = rla.ReceivedLogId,
                    // Type = rla.Type,
                    // ParentMaterialId = rla.ParentMaterialId,
                    // ParentMaterialNameTw = rla.ParentMaterialNameTw,
                    // PayQty = rla.PayQty,
                    // FreeQty = rla.FreeQty,
                    // TotalQty = rla.PayQty + rla.FreeQty,
                    // PurDollarCodeId = rla.PurDollarCodeId,
                    // PurDollarNameTw = rla.PurDollarNameTw,
                    // StockDollarCodeId = rla.StockDollarCodeId,
                    // StockDollarNameTw = rla.StockDollarNameTw,
                    // TransferInLocale = Company.Get().Where(i => i.Id == rl.TransferInLocaleId).Max(i => i.CompanyNo),
                    // POType = pi.POType,
                    // ReceivedType = MaterialStockItem.Get().Where(i => i.ReceivedLogId == rl.Id && i.LocaleId == rl.LocaleId && i.RefNo.StartsWith("OPDI-")).Count() > 0 ? 2 : 1,
                    // MaterialNameEng = Material.Get().Where(i => i.Id == rla.MaterialId && i.LocaleId == rla.LocaleId).Max(i => i.MaterialNameEng),
                });

            return result;
        }
    }
}