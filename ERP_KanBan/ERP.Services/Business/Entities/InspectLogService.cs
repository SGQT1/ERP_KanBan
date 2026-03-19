using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class InspectLogService : BusinessService
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

        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }

        public InspectLogService(
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
        }
        // 來料收貨
        public IQueryable<Models.Views.InspectLog> Get()
        {
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.InspectLog
                {
                    Id = rl.Id,
                    LocaleId = rl.LocaleId,
                    RefLocaleId = rl.RefLocaleId,
                    ReceivedDate = rl.ReceivedDate,
                    ShippingListNo = rl.ShippingListNo,
                    ShippingListVendorId = rl.ShippingListVendorId,
                    // ShippingListVendorName = Vendor.Get().Where(i => i.Id == rl.ShippingListVendorId && i.LocaleId == rl.LocaleId).Select(i => i.ShortNameTw).First(),
                    ShippingListVendorName = v.ShortNameTw,
                    POItemId = rl.POItemId,
                    UnitPrice = rl.UnitPrice,
                    ReceivedQty = rl.ReceivedQty,
                    SubTotalPrice = rl.SubTotalPrice,
                    Remark = rl.Remark,
                    APId = rl.APId,
                    QCDate = rl.QCDate,
                    IQCFlag = rl.IQCFlag,
                    IQCGetQty = rl.IQCGetQty,
                    IQCTestQty = rl.IQCTestQty,
                    IQCPassQty = rl.IQCPassQty,
                    IQCRejectionQty = rl.IQCRejectionQty,
                    IQCResult = rl.IQCResult,
                    IQCMen = rl.IQCMen,
                    IQCRemark = rl.IQCRemark,
                    SamplingMethod = rl.SamplingMethod,
                    WarehouseId = rl.WarehouseId,
                    StockQty = rl.StockQty,
                    OrderNo = rl.OrderNo,
                    IsAccounting = rl.IsAccounting,
                    TransferInId = rl.TransferInId,
                    TransferInLocaleId = rl.TransferInLocaleId,
                    TaiwanInvoiceNo = rl.TaiwanInvoiceNo,
                    TransferQty = rl.TransferQty,
                    WeightUnitCodeId = rl.WeightUnitCodeId,
                    NetWeight = rl.NetWeight,
                    GrossWeight = rl.GrossWeight,
                    ModifyUserName = rl.ModifyUserName,
                    LastUpdateTime = rl.LastUpdateTime,
                    ReceivedCount = rl.ReceivedCount,
                    ReceivedLogId = rla.ReceivedLogId,
                    RefPONo = rla.RefPONo,
                    Type = rla.Type,
                    MaterialId = rla.MaterialId,
                    MaterialNameTw = rla.MaterialNameTw,
                    ParentMaterialId = rla.ParentMaterialId,
                    ParentMaterialNameTw = rla.ParentMaterialNameTw,
                    PCLUnitCodeId = rla.PCLUnitCodeId,
                    PCLUnitNameTw = rla.PCLUnitNameTw,
                    PurUnitCodeId = rla.PurUnitCodeId,
                    PurUnitNameTw = rla.PurUnitNameTw,
                    PayQty = rla.PayQty,
                    FreeQty = rla.FreeQty,
                    TotalQty = rla.PayQty + rla.FreeQty,
                    PurDollarCodeId = rla.PurDollarCodeId,
                    PurDollarNameTw = rla.PurDollarNameTw,
                    StockDollarCodeId = rla.StockDollarCodeId,
                    StockDollarNameTw = rla.StockDollarNameTw,
                    ReceivedBarcode = rl.RefLocaleId.ToString() + "*" + rl.POItemId.ToString(),
                    TransferInLocale = Company.Get().Where(i => i.Id == rl.TransferInLocaleId).Max(i => i.CompanyNo),
                    CloseMonth = rla.CloseMonth,
                    POType = pi.POType,
                    ReceivedType = MaterialStockItem.Get().Where(i => i.ReceivedLogId == rl.Id && i.LocaleId == rl.LocaleId && i.RefNo.StartsWith("OPDI-")).Count() > 0 ? 2 : 1,
                    MaterialNameEng = Material.Get().Where(i => i.Id == rla.MaterialId && i.LocaleId == rla.LocaleId).Max(i => i.MaterialNameEng),
                    WarehouseNo = Warehouse.Get().Where(i => i.Id == rl.WarehouseId && i.LocaleId == rl.LocaleId).Max(i => i.WarehouseNo),
                });
            return result;
        }

        public Models.Views.InspectLog Update(Models.Views.InspectLog item)
        {
            ReceivedLog.UpdateRange(
                i => i.Id == item.Id && i.LocaleId == item.LocaleId,
                // u => new Models.Entities.ReceivedLog {
                //     QCDate = item.QCDate,
                //     SamplingMethod = item.SamplingMethod,
                //     IQCMen = item.IQCMen,
                //     IQCResult = item.IQCResult,
                //     IQCFlag = item.IQCFlag,
                //     SubTotalPrice = item.SubTotalPrice,
                //     IQCPassQty = item.IQCPassQty,
                //     IQCRejectionQty = item.IQCRejectionQty,
                //     IQCTestQty = item.IQCTestQty,
                // }
                u => u.SetProperty(p => p.QCDate, v => item.QCDate).SetProperty(p => p.SamplingMethod, v => item.SamplingMethod).SetProperty(p => p.IQCMen, v => item.IQCMen)
                      .SetProperty(p => p.IQCResult, v => item.IQCResult).SetProperty(p => p.IQCFlag, v => item.IQCFlag).SetProperty(p => p.SubTotalPrice, v => item.SubTotalPrice)
                      .SetProperty(p => p.IQCPassQty, v => item.IQCPassQty).SetProperty(p => p.IQCRejectionQty, v => item.IQCRejectionQty).SetProperty(p => p.IQCTestQty, v => item.IQCTestQty)
            );
            ReceivedLogAdd.UpdateRange(
                i => i.ReceivedLogId == item.Id && i.LocaleId == item.LocaleId,
                 // u => new Models.Entities.ReceivedLogAdd {
                 //     PayQty = item.PayQty,
                 //     FreeQty = item.FreeQty
                 // }
                 u => u.SetProperty(p => p.PayQty, v => item.PayQty).SetProperty(p => p.FreeQty, v => item.FreeQty)
            );

            return Get().Where(i => i.Id == item.Id && i.LocaleId == item.LocaleId).FirstOrDefault();
        }

        public void Remove(Models.Views.InspectLog item)
        {
            ReceivedLog.UpdateRange(
                i => i.Id == item.Id && i.LocaleId == item.LocaleId,
                // u => new Models.Entities.ReceivedLog {
                //     QCDate = item.QCDate,
                //     SamplingMethod = item.SamplingMethod,
                //     IQCMen = item.IQCMen,
                //     IQCResult = item.IQCResult,
                //     IQCFlag = item.IQCFlag,
                //     SubTotalPrice = item.SubTotalPrice,
                //     IQCPassQty = item.IQCPassQty,
                //     IQCRejectionQty = item.IQCRejectionQty,
                //     IQCTestQty = item.IQCTestQty,
                // }
                u => u.SetProperty(p => p.QCDate, v => item.QCDate).SetProperty(p => p.SamplingMethod, v => item.SamplingMethod).SetProperty(p => p.IQCMen, v => item.IQCMen)
                      .SetProperty(p => p.IQCResult, v => item.IQCResult).SetProperty(p => p.IQCFlag, v => item.IQCFlag).SetProperty(p => p.SubTotalPrice, v => item.SubTotalPrice)
                      .SetProperty(p => p.IQCPassQty, v => item.IQCPassQty).SetProperty(p => p.IQCRejectionQty, v => item.IQCRejectionQty).SetProperty(p => p.IQCTestQty, v => item.IQCTestQty)
            );
            ReceivedLogAdd.UpdateRange(
                i => i.ReceivedLogId == item.Id && i.LocaleId == item.LocaleId,
                // u => new Models.Entities.ReceivedLogAdd {
                //     PayQty = item.PayQty,
                //     FreeQty = item.FreeQty
                // }
                u => u.SetProperty(p => p.PayQty, v => item.PayQty).SetProperty(p => p.FreeQty, v => item.FreeQty)
            );
        }
    }
}