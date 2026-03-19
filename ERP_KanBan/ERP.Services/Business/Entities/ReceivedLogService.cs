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
    public class ReceivedLogService : BusinessService
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

        public ReceivedLogService(
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
        // 來料收貨
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
                join m in Material.Get() on new { MaterialId = rla.MaterialId, LocaleId = rla.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new Models.Views.ReceivedLog
                {
                    Id = rl.Id,
                    LocaleId = rl.LocaleId,
                    RefLocaleId = rl.RefLocaleId,
                    ReceivedDate = rl.ReceivedDate,
                    ShippingListNo = rl.ShippingListNo,
                    ShippingListVendorId = rl.ShippingListVendorId,
                    ShippingListVendorName = v.ShortNameTw,
                    POItemId = rl.POItemId,
                    UnitPrice = rl.UnitPrice,
                    ReceivedQty = rl.ReceivedQty,
                    PreReceivedQty = ReceivedLog.Get().Where(i => i.POItemId == pi.Id && i.RefLocaleId == pi.LocaleId && i.TransferInId == 0).Sum(i => i.ReceivedQty),
                    SubTotalPrice = rl.SubTotalPrice,
                    Remark = rl.Remark,
                    APId = rl.APId,
                    QCDate = rl.QCDate,
                    IQCFlag = rl.IQCFlag,
                    IQCGetQty = rl.IQCGetQty,
                    IQCTestQty = rl.IQCTestQty,
                    IQCPassQty = rl.IQCPassQty,
                    IQCRejectionQty = rl.IQCPassQty,
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
                    ETD = pi.FactoryETD,
                    AllocateQty = rla.PayQty, // 記錄PayQty的數量，因為修改時這個數字可能會變，無法用與檢查
                    PurUnitPrice = pi.UnitPrice,
                    POLastUpdateTime = pi.LastUpdateTime,
                    SemiGoods = m.SemiGoods,
                });
            return result;
        }

        // 託外收貨
        public IQueryable<Models.Views.ReceivedLog> GetOutsource()
        {
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                join m in MaterialStockItem.Get().Where(i => i.RefNo.StartsWith("OPDI-")).Select(i => new { ReceivedLogId = i.ReceivedLogId, LocaleId = i.LocaleId }).Distinct()
                    on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = (decimal)m.ReceivedLogId, LocaleId = m.LocaleId }
                select new Models.Views.ReceivedLog
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
                    IQCRejectionQty = rl.IQCPassQty,
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
                    ReceivedBarcode = rl.LocaleId.ToString() + "*" + rl.POItemId.ToString(),
                    TransferInLocale = Company.Get().Where(i => i.Id == rl.TransferInLocaleId).Max(i => i.CompanyNo),
                    CloseMonth = rla.CloseMonth,
                    POType = pi.POType,
                    ReceivedType = 2,
                    PurLocaleId = pi.PurLocaleId,
                    ReceivingLocaleId = pi.ReceivingLocaleId,
                    PaymentLocaleId = pi.PaymentCodeId,
                    ReceivedLocaleId = pi.ReceivingLocaleId,
                });
            return result;
        }

        // 轉廠匯入
        public IQueryable<Models.Views.TransferItem> GetReceivedLogForTransfer()
        {
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = rla.MaterialId, LocaleId = rla.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                    // where rl.TransferInId == 0 && rl.IQCGetQty > (rl.StockQty + rl.TransferQty) // 改成只檢查Recevid Id, 不檢查數量
                where rl.TransferInId == 0 && rl.TransferQty == 0   // 改成已經轉廠就排除
                select new Models.Views.TransferItem
                {
                    Id = 0,
                    LocaleId = rl.LocaleId,
                    Recipient = rl.ModifyUserName,
                    MaterialId = rla.MaterialId,
                    MaterialNameTw = m.MaterialName,
                    MaterialNameTwCust = m.MaterialName,
                    MaterialNameEnCust = m.MaterialNameEng,
                    MaterialNameEn = m.MaterialNameEng,
                    ReceivedDate = rl.ReceivedDate,
                    ReceivedQty = rl.ReceivedQty,
                    TransferQty = rl.ReceivedQty,
                    TransferQtyCust = rl.ReceivedQty,
                    NetWeight = rl.NetWeight,
                    GrossWeight = rl.GrossWeight,
                    UnitPrice = rl.UnitPrice,
                    OrderNo = rl.OrderNo,
                    PurUnitNameTw = rla.PurUnitNameTw,

                    IQCGetQty = rl.IQCGetQty,
                    StockQty = rl.StockQty,
                    Vendor = Vendor.Get().Where(i => i.Id == rl.ShippingListVendorId && i.LocaleId == rl.LocaleId).Select(i => i.ShortNameTw).First(),
                    RefLocaleId = rl.RefLocaleId,
                    Remark = rl.Remark,
                    UnitPriceCust = rl.UnitPrice,
                    DollarCodeNameTwCust = rla.PurDollarNameTw,
                    POItemId = rl.POItemId,
                    ReceivedLogId = rl.Id,
                    ReceivedBarcode = rl.LocaleId.ToString() + "*" + rl.POItemId.ToString(),
                    ReceivedLogLocaleId = rl.LocaleId,
                    PONo = rla.RefPONo,
                    TargetCompanyId = rl.RefLocaleId,
                    TargetCompany = Company.Get().Where(i => i.Id == rl.RefLocaleId).Max(i => i.CompanyNo),
                    TransferType = 0,
                    UnitCodeId = rla.PurUnitCodeId,
                    UnitCodeNameTwCust = rla.PurUnitNameTw,
                    WeiUnitCodeNameTwCust = CodeItem.Get().Where(i => i.Id == rl.WeightUnitCodeId && i.LocaleId == rl.LocaleId && i.CodeType == "21").Max(i => i.NameTW),
                    // SubCount = (int)rl.ReceivedCount,
                    SubCount = 0,
                    AmountCust = rl.ReceivedQty * rl.UnitPrice
                }
            );
            return result;
        }

        // 來料品檢
        public IQueryable<Models.Views.ReceivedLog> GetInspectLog()
        {
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.ReceivedLog
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
                    // ReceivedType = MaterialStockItem.Get().Where(i => i.ReceivedLogId == rl.Id && i.LocaleId == rl.LocaleId && i.RefNo.StartsWith("OPDI-")).Count() > 0 ? 2 : 1,
                    MaterialNameEng = Material.Get().Where(i => i.Id == rla.MaterialId && i.LocaleId == rla.LocaleId).Max(i => i.MaterialNameEng),
                    WarehouseNo = Warehouse.Get().Where(i => i.Id == rl.WarehouseId && i.LocaleId == rl.LocaleId).Max(i => i.WarehouseNo),
                });
            return result;
        }

        public Models.Views.ReceivedLog Create(Models.Views.ReceivedLog item)
        {
            var _item = ReceivedLog.Create(Build(item));

            item.Id = _item.Id;
            item.ReceivedLogId = _item.Id;
            var test = BuildAdd(item);
            var _itemAdd = ReceivedLogAdd.Create(BuildAdd(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.ReceivedLog Update(Models.Views.ReceivedLog item)
        {
            var _item = ReceivedLog.Update(Build(item));

            var _itemAdd = ReceivedLogAdd.Get().Where(i => i.ReceivedLogId == item.Id && i.LocaleId == item.LocaleId).FirstOrDefault();
            if (_itemAdd == null)
            {
                _itemAdd = _itemAdd = ReceivedLogAdd.Create(BuildAdd(item));
            }
            else
            {
                _itemAdd = ReceivedLogAdd.Update(BuildAdd(item));
            }


            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }

        // 更新庫存數
        public void UpdateStockQty(List<decimal> ids, decimal localeId)
        {
            ReceivedLog.UpdateRange(
                i => ids.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.ReceivedLog
                // {
                //     StockQty = StockIO.Get().Where(i => i.ReceivedLogId == u.Id && i.LocaleId == u.LocaleId).Sum(i => i.PCLIOQty) == null ? 0 :
                //                StockIO.Get().Where(i => i.ReceivedLogId == u.Id && i.LocaleId == u.LocaleId).Sum(i => i.PCLIOQty)
                // }
                u => u.SetProperty(p => p.StockQty, v => StockIO.Get().Where(i => i.ReceivedLogId == v.Id && i.LocaleId == v.LocaleId).Sum(i => i.PCLIOQty) == null ? 0 : StockIO.Get().Where(i => i.ReceivedLogId == v.Id && i.LocaleId == v.LocaleId).Sum(i => i.PCLIOQty))
            );
        }
        public void UpdatePrice(Models.Views.ReceivedLog item)
        {
            ReceivedLog.UpdateRange(
                i => i.Id == item.Id && i.LocaleId == item.LocaleId,
                 // u => new ReceivedLog { UnitPrice = (decimal)item.PurUnitPrice, SubTotalPrice = (decimal)item.PurUnitPrice * item.PayQty, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.UnitPrice, v => (decimal)item.PurUnitPrice).SetProperty(p => p.SubTotalPrice, v => (decimal)item.PurUnitPrice * item.PayQty).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );
        }
        // 更新轉廠數
        public void UpdateTransferQty(List<decimal> ids, decimal localeId)
        {
            ReceivedLog.UpdateRange(
                i => ids.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.ReceivedLog
                // {
                //     TransferQty = TransferItem.Get().Where(i => i.ReceivedLogId == u.Id && i.LocaleId == u.LocaleId).Sum(i => i.TransferQty) == null ? 0 :
                //                   TransferItem.Get().Where(i => i.ReceivedLogId == u.Id && i.LocaleId == u.LocaleId).Sum(i => i.TransferQty)
                // }
                u => u.SetProperty(p => p.TransferQty, v => TransferItem.Get().Where(i => i.ReceivedLogId == v.Id && i.LocaleId == v.LocaleId).Sum(i => i.TransferQty) == null ? 0 : TransferItem.Get().Where(i => i.ReceivedLogId == v.Id && i.LocaleId == v.LocaleId).Sum(i => i.TransferQty))
            );
        }
        public void RemoveTransferQty(List<decimal> ids, decimal localeId)
        {
            ReceivedLog.UpdateRange(
                i => ids.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.ReceivedLog { TransferQty = 0,ShippingListNo = ""}
                u => u.SetProperty(p => p.TransferQty, v => 0).SetProperty(p => p.ShippingListNo, v => "")
            );
        }
        public void Remove(Models.Views.ReceivedLog item)
        {
            // ReceivedLog.Remove(Build(item));
            // ReceivedLogAdd.Remove(BuildAdd(item));
            ReceivedLog.RemoveRange(i => i.Id == item.Id && i.LocaleId == item.LocaleId);
            ReceivedLogAdd.RemoveRange(i => i.ReceivedLogId == item.Id && i.LocaleId == item.LocaleId);
        }
        //for update, transfer view model to entity
        private Models.Entities.ReceivedLog Build(Models.Views.ReceivedLog item)
        {
            return new Models.Entities.ReceivedLog()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                RefLocaleId = item.RefLocaleId,
                ReceivedDate = item.ReceivedDate,
                ShippingListNo = item.ShippingListNo,
                ShippingListVendorId = item.ShippingListVendorId,
                POItemId = item.POItemId,
                UnitPrice = item.UnitPrice,
                ReceivedQty = item.ReceivedQty,
                SubTotalPrice = item.SubTotalPrice,
                Remark = item.Remark,
                APId = item.APId,
                QCDate = item.QCDate,
                IQCFlag = item.IQCFlag,
                IQCGetQty = item.IQCGetQty,
                IQCTestQty = item.IQCTestQty,
                IQCPassQty = item.IQCPassQty,
                IQCRejectionQty = item.IQCPassQty,
                IQCResult = item.IQCResult,
                IQCMen = item.IQCMen,
                IQCRemark = item.IQCRemark,
                SamplingMethod = item.SamplingMethod,
                WarehouseId = item.WarehouseId,
                StockQty = item.StockQty,
                OrderNo = item.OrderNo,
                IsAccounting = item.IsAccounting,
                TransferInId = item.TransferInId,
                TransferInLocaleId = item.TransferInLocaleId,
                TaiwanInvoiceNo = item.TaiwanInvoiceNo,
                TransferQty = item.TransferQty,
                WeightUnitCodeId = item.WeightUnitCodeId,
                NetWeight = item.NetWeight,
                GrossWeight = item.GrossWeight,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                ReceivedCount = item.ReceivedCount,
            };
        }
        private Models.Entities.ReceivedLogAdd BuildAdd(Models.Views.ReceivedLog item)
        {
            var closeMonth = item.TransferInId > 0 ? "" : item.ReceivedDate.ToString("yyyyMM");

            if (item.TransferInId == 0)
            {
                var vendor = Vendor.Get().Where(i => i.Id == item.ShippingListVendorId && i.LocaleId == item.LocaleId).First();
                var closeDate = vendor.DayOfMonth == 0 ? new DateTime(item.ReceivedDate.AddMonths(1).Year, item.ReceivedDate.AddMonths(1).Month, 1).AddDays(-1) :
                                                        new DateTime(item.ReceivedDate.Year, item.ReceivedDate.Month, vendor.DayOfMonth);
                closeMonth = item.ReceivedDate.Date <= closeDate.Date ? closeMonth : new DateTime(item.ReceivedDate.AddMonths(1).Year, item.ReceivedDate.AddMonths(1).Month, 1).ToString("yyyyMM");
            }
            var stockCurrencyId = CodeItem.Get().Where(i => i.LocaleId == item.LocaleId && i.NameTW == item.StockDollarNameTw && i.CodeType == "02").Max(i => i.Id);
            return new Models.Entities.ReceivedLogAdd()
            {
                ReceivedLogId = item.ReceivedLogId,
                LocaleId = item.LocaleId,
                RefPONo = item.RefPONo,
                Type = item.Type,
                MaterialId = item.MaterialId,
                MaterialNameTw = item.MaterialNameTw,
                ParentMaterialId = item.ParentMaterialId,
                ParentMaterialNameTw = item.ParentMaterialNameTw,
                PCLUnitCodeId = item.PCLUnitCodeId,
                PCLUnitNameTw = item.PCLUnitNameTw,
                PurUnitCodeId = item.PurUnitCodeId,
                PurUnitNameTw = item.PurUnitNameTw,
                PayQty = item.PayQty,
                FreeQty = item.FreeQty,
                PurDollarCodeId = item.PurDollarCodeId,
                PurDollarNameTw = item.PurDollarNameTw,
                StockDollarCodeId = stockCurrencyId,
                StockDollarNameTw = item.StockDollarNameTw,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                CloseMonth = closeMonth,
            };
        }

    }
}