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
using ERP.Models.Entities;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;

namespace ERP.Services.Business
{
    public class ReceivedLogService : BusinessService
    {
        private ERP.Services.Business.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogSizeItemService ReceivedLogSizeItem { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Business.Entities.POItemSizeService POItemSize { get; set; }
        private ERP.Services.Business.Entities.TransferService Transfer { get; set; }
        private ERP.Services.Business.Entities.TransferItemService TransferItem { get; set; }
        private ERP.Services.Business.Entities.TransferSizeItemService TransferSizeItem { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.ExchangeRateService ExchangeRate { get; set; }

        private ERP.Services.Business.MaterialStockService _MaterialStock { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogService _ReceivedLog { get; set; }
        private ERP.Services.Business.Entities.MaterialStockService MaterialStock { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemSizeService MaterialStockItemSize { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemPOService MaterialStockItemPO { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemPOService _MaterialStockItemPO { get; set; }
        private ERP.Services.Business.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Business.Entities.ReceivedStandardService ReceivedStandard { get; set; }

        public ReceivedLogService(
            ERP.Services.Business.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Business.Entities.ReceivedLogSizeItemService receivedLogSizeItemService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Business.Entities.POItemSizeService poItemSizeService,
            ERP.Services.Business.Entities.TransferService transferService,
            ERP.Services.Business.Entities.TransferItemService transferItemService,
            ERP.Services.Business.Entities.TransferSizeItemService transferSizeItemService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.MaterialService materialService,
            ERP.Services.Business.MaterialStockService _materialStockService,
            ERP.Services.Business.Entities.ReceivedLogService _receivedLogService,
            ERP.Services.Business.Entities.MaterialStockService materialStockService,
            ERP.Services.Business.Entities.MaterialStockItemService materialStockItemService,
            ERP.Services.Business.Entities.MaterialStockItemSizeService materialStockItemSizeService,
            ERP.Services.Business.Entities.ExchangeRateService exchangeRateService,
            ERP.Services.Business.Entities.VendorService vendorService,
            ERP.Services.Business.Entities.MaterialStockItemPOService materialStockItemPOService,
            ERP.Services.Business.Entities.MaterialStockItemPOService _materialStockItemPOService,
            ERP.Services.Business.Entities.ReceivedStandardService receivedStandardService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            ReceivedLog = receivedLogService;
            ReceivedLogSizeItem = receivedLogSizeItemService;
            POItem = poItemService;
            POItemSize = poItemSizeService;
            Transfer = transferService;
            TransferItem = transferItemService;
            TransferSizeItem = transferSizeItemService;
            CodeItem = codeItemService;
            Material = materialService;
            MaterialStockItemSize = materialStockItemSizeService;
            MaterialStockItem = materialStockItemService;
            MaterialStock = materialStockService;
            _MaterialStock = _materialStockService;
            _ReceivedLog = _receivedLogService;
            ExchangeRate = exchangeRateService;
            Vendor = vendorService;
            MaterialStockItemPO = materialStockItemPOService;
            _MaterialStockItemPO = _materialStockItemPOService;
            ReceivedStandard = receivedStandardService;
        }

        public ERP.Models.Views.ReceivedLogGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.ReceivedLogGroup { };
            var receivedLog = ReceivedLog.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (receivedLog != null)
            {
                var sizeItem = ReceivedLogSizeItem.Get().Where(i => i.ReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId).ToList();
                var poItem = POItem.Get().Where(i => i.Id == receivedLog.POItemId && i.LocaleId == receivedLog.RefLocaleId).FirstOrDefault();
                var type = MaterialStockItem.Get().Where(i => i.ReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId && i.RefNo.StartsWith("OPDI-")).Count() > 0 ? 2 : 1; //OPDI：拖外收貨

                if (poItem != null)
                {
                    var _material = Material.Get().Where(i => i.MaterialName == poItem.Material && i.LocaleId == localeId).FirstOrDefault();
                    var _rStandard = ReceivedStandard.Get().Where(i => i.CategoryCodeId == _material.CategoryCodeId && i.LocaleId == _material.LocaleId && i.AbovePurQty <= poItem.Qty).OrderByDescending(i => i.Priority).ThenByDescending(i => i.AbovePurQty).FirstOrDefault();

                    var preQtyItems = ReceivedLog.Get().Where(i => i.POItemId == poItem.Id && i.RefLocaleId == poItem.LocaleId && i.TransferInId == 0).Select(i => new { i.Id, i.ReceivedQty, i.PayQty, i.FreeQty, i.IQCGetQty }).ToList();
                    var preReceivedQty = preQtyItems.Sum(i => i.ReceivedQty);
                    var prePayQty = preQtyItems.Sum(i => i.PayQty);
                    var preIQGetQty = preQtyItems.Sum(i => i.IQCGetQty);

                    // var recIds = ReceivedLog.Get().Where(i => i.POItemId == poItem.Id && i.RefLocaleId == poItem.LocaleId).Select(i => (decimal?)i.Id).ToList();
                    var recIds = preQtyItems.Select(i => (decimal?)i.Id);
                    var stockIn = MaterialStockItem.Get().Where(i => i.LocaleId == localeId && recIds.Contains(i.ReceivedLogId) && i.PCLIOQty > 0).Sum(i => i.PCLIOQty);
                    var stockOut = MaterialStockItem.Get().Where(i => i.LocaleId == receivedLog.LocaleId && i.WarehouseId == receivedLog.WarehouseId && i.OrderNo == receivedLog.OrderNo && i.MaterialId == receivedLog.MaterialId && i.PCLIOQty < 0).Sum(i => i.PCLIOQty);

                    receivedLog.PrePayQty = prePayQty;
                    receivedLog.StockInQty = stockIn;
                    receivedLog.StockOutQty = stockOut;
                    receivedLog.MaxReceivedQty = _rStandard == null ? -1 : _rStandard.RejectRate * poItem.Qty;
                }
                group.ReceivedLog = receivedLog;
                group.ReceivedLogSizeItem = sizeItem;
                group.POItem = poItem == null ? new ERP.Models.Views.POItem { } : poItem;
                group.MaterialStockItemPO = poItem == null ? null : MaterialStockItemPO.Get().Where(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId).ToList();
                group.ReceivedType = type;

                //判斷要不要顯示QC收貨跟直接入庫
                group.SaveReceivedLogOption = (poItem == null || (receivedLog.TransferInId == 0 && poItem.PurLocaleId != poItem.ReceivingLocaleId) || (receivedLog.TransferInId > 0 && receivedLog.LocaleId != receivedLog.RefLocaleId)) ?
                new SaveReceivedLogOption
                {
                    ReceivedType = type,
                    SaveQCResult = false,
                    SaveStockIn = false,
                    SaveStockOut = false,

                } :
                new SaveReceivedLogOption
                {
                    ReceivedType = type,
                    SaveQCResult = group.ReceivedLog.IQCResult == 2 ? true : false,
                    SaveStockIn = group.ReceivedLog.IQCResult == 2 ? true : false,
                    SaveStockOut = group.ReceivedLog.IQCResult == 2 ? true : false,

                };
            }
            return group;
        }

        //來料收貨匯入
        public ERP.Models.Views.ReceivedLogGroup GetReceivedLogGroupFromImport(string po, int localeId)
        {
            var group = new ERP.Models.Views.ReceivedLogGroup { };
            var barcode = po.Split('*');
            var poItemLocaleId = Convert.ToInt32(barcode[0]);
            var poItemId = Convert.ToInt32(barcode[1]);


            var poItem = POItem.Get().Where(i => i.Id == poItemId && i.LocaleId == poItemLocaleId && i.Status == 1).FirstOrDefault();

            if (poItem != null)
            {
                var poItemSize = POItemSize.Get().Where(i => i.POItemId == poItemId && i.LocaleId == poItemLocaleId).OrderBy(i => i.SeqNo).ToList();
                // var preReceivedQty = ReceivedLog.Get().Where(i => i.POItemId == poItem.Id && i.RefLocaleId == poItem.LocaleId && i.TransferInId == 0).Sum(i => i.ReceivedQty);

                var preQtyItems = ReceivedLog.Get().Where(i => i.POItemId == poItem.Id && i.RefLocaleId == poItem.LocaleId && i.TransferInId == 0).Select(i => new { i.ReceivedQty, i.PayQty, i.FreeQty, i.IQCGetQty }).ToList();
                var preReceivedQty = preQtyItems.Sum(i => i.ReceivedQty);
                var prePayQty = preQtyItems.Sum(i => i.PayQty);
                var preIQGetQty = preQtyItems.Sum(i => i.IQCGetQty);

                var _vendor = Vendor.Get().Where(i => i.NameTw == poItem.VendorNameTw && i.LocaleId == localeId).FirstOrDefault();
                var _material = Material.Get().Where(i => i.MaterialName == poItem.Material && i.LocaleId == localeId).FirstOrDefault();
                var _unit = CodeItem.Get().Where(i => i.NameTW == poItem.Unit && i.LocaleId == localeId && i.CodeType == "21").FirstOrDefault();
                var _currency = CodeItem.Get().Where(i => i.NameTW == poItem.Currency && i.LocaleId == localeId && i.CodeType == "02").FirstOrDefault();
                var _weight = CodeItem.Get().Where(i => i.NameTW == "KGS" && i.LocaleId == localeId && i.CodeType == "21").FirstOrDefault();
                var _rStandard = ReceivedStandard.Get().Where(i => i.CategoryCodeId == _material.CategoryCodeId && i.LocaleId == _material.LocaleId && i.AbovePurQty <= poItem.Qty).OrderByDescending(i => i.Priority).ThenByDescending(i => i.AbovePurQty).FirstOrDefault();

                var recIds = ReceivedLog.Get().Where(i => i.POItemId == poItem.Id && i.RefLocaleId == poItem.LocaleId).Select(i => (decimal?)i.Id).ToList();
                var stockIn = MaterialStockItem.Get().Where(i => i.LocaleId == localeId && recIds.Contains(i.ReceivedLogId) && i.PCLIOQty > 0).Sum(i => i.PCLIOQty);

                group.ReceivedLog = new ERP.Models.Views.ReceivedLog
                {
                    Id = 0,
                    LocaleId = localeId,
                    RefLocaleId = poItem.LocaleId,
                    ReceivedDate = DateTime.Today,
                    // ShippingListNo = rl.ShippingListNo,
                    ShippingListVendorId = _vendor.Id,
                    ShippingListVendorName = poItem.VendorNameTw,
                    POItemId = poItem.Id,
                    UnitPrice = (decimal)poItem.UnitPrice,
                    ReceivedQty = poItem.Qty,
                    SubTotalPrice = (decimal)poItem.UnitPrice * poItem.Qty,
                    // Remark = rl.Remark,
                    // APId = rl.APId,
                    // QCDate = rl.QCDate,
                    IQCFlag = poItem.SamplingMethod == 2 ? 0 : 1,
                    IQCGetQty = poItem.Qty,
                    // IQCTestQty = rl.IQCTestQty,
                    // IQCPassQty = rl.IQCPassQty,
                    // IQCRejectionQty = rl.IQCPassQty,
                    // IQCResult = _vendor.PaymentPoint == 1 ? 2 : 0,
                    IQCResult = 0,
                    // IQCMen = rl.IQCMen,
                    // IQCRemark = rl.IQCRemark,
                    SamplingMethod = poItem.SamplingMethod,
                    // WarehouseId = rl.WarehouseId,
                    // StockQty = rl.StockQty,
                    OrderNo = poItem.OrderNo,
                    IsAccounting = 0,
                    // TransferInId = rl.TransferInId,
                    // TransferInLocaleId = rl.TransferInLocaleId,
                    // TaiwanInvoiceNo = rl.TaiwanInvoiceNo,
                    // TransferQty = rl.TransferQty,
                    WeightUnitCodeId = _weight.Id,
                    NetWeight = (decimal)0.0000,
                    GrossWeight = (decimal)0.0000,
                    // ModifyUserName = rl.ModifyUserName,
                    // LastUpdateTime = rl.LastUpdateTime,
                    ReceivedCount = 1,
                    // ReceivedLogId = rla.ReceivedLogId,
                    RefPONo = poItem.RefPONo,
                    Type = 1,
                    MaterialId = _material.Id,//poItem.MaterialId,
                    MaterialNameTw = poItem.Material,
                    ParentMaterialId = poItem.ParentMaterialId,
                    ParentMaterialNameTw = poItem.ParentMaterial,
                    PCLUnitCodeId = _unit.Id, //(decimal)poItem.PCLUnitCodeId,
                    PCLUnitNameTw = poItem.Unit,
                    PurUnitCodeId = _unit.Id, //poItem.UnitCodeId,
                    PurUnitNameTw = poItem.Unit,
                    PayQty = poItem.Qty,
                    FreeQty = 0,
                    TotalQty = poItem.Qty + 0,
                    PurDollarCodeId = _currency.Id, // poItem.PayDollarCodeId,
                    PurDollarNameTw = poItem.Currency,
                    // StockDollarCodeId = rla.StockDollarCodeId,
                    // StockDollarNameTw = rla.StockDollarNameTw,
                    ReceivedBarcode = po,
                    PlanQty = poItem.PlanQty ?? 0,
                    MaterialNameEng = poItem.MaterialEng,
                    PreReceivedQty = preReceivedQty,
                    PrePayQty = prePayQty,
                    MaxReceivedQty = _rStandard == null ? poItem.Qty * (decimal)1.9 : _rStandard.RejectRate * poItem.Qty,  //沒有收貨標準的材料，一律用1.01當拒收標準
                    RejectRate = _rStandard == null ? 0 : _rStandard.RejectRate,
                    StockInQty = stockIn,
                    StockOutQty = 0,
                    AllocateQty = 0, // 記錄PayQty的數量，因為修改時這個數字可能會變，無法用與檢查
                };

                var blanceRecQty = poItem.Qty - (decimal)group.ReceivedLog.PreReceivedQty;
                group.ReceivedLog.PayQty = blanceRecQty < 0 ? 0 : blanceRecQty;
                group.ReceivedLog.IQCGetQty = blanceRecQty < 0 ? 0 : blanceRecQty;
                group.ReceivedLog.ReceivedQty = blanceRecQty < 0 ? 0 : blanceRecQty;

                group.ReceivedLogSizeItem = poItemSize.Select(i => new Models.Views.ReceivedLogSizeItem
                {
                    Id = 0,
                    LocaleId = localeId,
                    RefLocaleId = poItem.LocaleId,
                    ReceivedLogId = 0,
                    DisplaySize = i.DisplaySize,
                    ReceivedQty = i.Qty,
                    IQCGetQty = i.Qty,
                    StockQty = 0,
                    TransferQty = 0,
                    SeqNo = i.SeqNo,
                    ShoeInnerSize = Convert.ToDouble(i.SeqNo),
                    // ModifyUserName = i.ModifyUserName,
                    // LastUpdateTime = i.LastUpdateTime,
                });
                group.POItem = poItem;
                group.MaterialStockItemPO = MaterialStockItemPO.Get().Where(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId).ToList();
                group.SaveReceivedLogOption = poItem.PurLocaleId != poItem.ReceivingLocaleId ?
                new SaveReceivedLogOption
                {
                    ReceivedType = 1,
                    SaveQCResult = false,
                    SaveStockIn = false,
                    SaveStockOut = false,

                } :
                new SaveReceivedLogOption
                {
                    ReceivedType = 1,
                    SaveQCResult = poItem.SamplingMethod == 2 ? true : false,
                    SaveStockIn = poItem.SamplingMethod == 2 ? true : false,
                    SaveStockOut = poItem.SamplingMethod == 2 ? true : false,

                };
            }
            return group;
        }
        //來料收貨轉廠匯入
        public ERP.Models.Views.ReceivedLogGroup GetTransferLogGroupFromImport(int transferItemId, int transferItemLocaleId, int targetLocaleId)
        {
            //get received from Transfer data, tranfertItemLocaleId = Locale from Transfer, targetLocaleId = Locale of Data Host
            var group = new ERP.Models.Views.ReceivedLogGroup { };

            var transferItem = TransferItem.Get().Where(i => i.Id == transferItemId && i.LocaleId == transferItemLocaleId && i.TargetCompanyId == targetLocaleId).FirstOrDefault();
            var transferItemSize = TransferSizeItem.Get().Where(i => i.Id == transferItemId && i.LocaleId == transferItemLocaleId).OrderBy(i => i.Id).ToList();
            if (transferItem != null)
            {
                //get codeitem of currency,unit,weigh
                var codeNames = new string[] { transferItem.UnitCodeNameTwCust, transferItem.WeiUnitCodeNameTwCust, transferItem.DollarCodeNameTwCust, "USD" };
                var codeItems = CodeItem.Get().Where(i => (i.CodeType == "21" || i.CodeType == "02") && i.LocaleId == targetLocaleId && codeNames.Contains(i.NameTW)).Select(i => new { Id = i.Id, NameTw = i.NameTW }).ToList();
                var material = Material.Get().Where(i => i.LocaleId == targetLocaleId && i.MaterialName == transferItem.MaterialNameTw).FirstOrDefault();

                group.POItem = POItem.Get().Where(i => i.Id == transferItem.POItemId && i.LocaleId == transferItem.POItemLocaleId).FirstOrDefault();
                group.ReceivedLog = new ERP.Models.Views.ReceivedLog
                {
                    Id = 0,
                    LocaleId = transferItem.TargetCompanyId,
                    RefLocaleId = transferItem.POItemLocaleId ?? 0,
                    ReceivedDate = DateTime.Today,
                    ShippingListNo = transferItem.ShipmentNo,
                    ShippingListVendorId = 0,
                    // ShippingListVendorName = "",
                    POItemId = (decimal)transferItem.POItemId,
                    RefPONo = transferItem.PONo,
                    UnitPrice = (decimal)transferItem.UnitPrice,
                    ReceivedQty = transferItem.TransferQty,
                    SubTotalPrice = 0, //(decimal)transferItem.UnitPrice * transferItem.TransferQty,
                    // Remark = rl.Remark,
                    APId = -1,
                    // QCDate = rl.QCDate,
                    IQCFlag = 1,
                    IQCGetQty = transferItem.TransferQty,
                    // IQCTestQty = rl.IQCTestQty,
                    // IQCPassQty = rl.IQCPassQty,
                    // IQCRejectionQty = rl.IQCPassQty,
                    // IQCResult = rl.IQCResult,
                    // IQCMen = rl.IQCMen,
                    // IQCRemark = rl.IQCRemark,
                    SamplingMethod = (int)transferItem.SamplingMethod,
                    // WarehouseId = rl.WarehouseId,
                    // StockQty = rl.StockQty,
                    OrderNo = transferItem.OrderNo,
                    IsAccounting = 2,
                    TransferInId = transferItemId,
                    TransferInLocaleId = transferItemLocaleId,
                    TaiwanInvoiceNo = "",
                    // TransferQty = rl.TransferQty,
                    // WeightUnitCodeId = rl.WeightUnitCodeId,
                    NetWeight = transferItem.NetWeight,
                    GrossWeight = transferItem.GrossWeight,
                    // ModifyUserName = rl.ModifyUserName,
                    // LastUpdateTime = rl.LastUpdateTime,
                    ReceivedCount = transferItem.SubCount,
                    // ReceivedLogId = rla.ReceivedLogId,
                    // RefPONo = transferItem.PONo,
                    Type = 2,
                    MaterialId = material.Id,
                    MaterialNameTw = transferItem.MaterialNameTw,
                    ParentMaterialId = transferItem.ParentMaterialId,
                    ParentMaterialNameTw = "",
                    PCLUnitCodeId = codeItems.Where(i => i.NameTw == transferItem.UnitCodeNameTwCust).Max(i => i.Id),
                    PCLUnitNameTw = transferItem.UnitCodeNameTwCust,
                    PurUnitCodeId = codeItems.Where(i => i.NameTw == transferItem.UnitCodeNameTwCust).Max(i => i.Id),
                    PurUnitNameTw = transferItem.UnitCodeNameTwCust,
                    PayQty = 0,
                    FreeQty = 0,//transferItem.TransferQty,
                    TotalQty = transferItem.TransferQty + 0,
                    PurDollarCodeId = codeItems.Where(i => i.NameTw == transferItem.DollarCodeNameTwCust).Max(i => i.Id),
                    PurDollarNameTw = transferItem.DollarCodeNameTwCust,
                    WeightUnitCodeId = codeItems.Where(i => i.NameTw == transferItem.WeiUnitCodeNameTwCust).Max(i => i.Id),
                    StockDollarCodeId = codeItems.Where(i => i.NameTw == "USD").Max(i => i.Id),
                    StockDollarNameTw = "USD",
                };
                group.ReceivedLogSizeItem = transferItemSize.Select(i => new Models.Views.ReceivedLogSizeItem
                {
                    Id = 0,
                    LocaleId = transferItemLocaleId,
                    RefLocaleId = i.LocaleId,
                    ReceivedLogId = 0,
                    DisplaySize = i.DisplaySize,
                    ReceivedQty = i.TransferQty,
                    IQCGetQty = i.TransferQty,
                    StockQty = 0,
                    TransferQty = 0,
                    SeqNo = 0,
                    // ModifyUserName = i.ModifyUserName,
                    // LastUpdateTime = i.LastUpdateTime,
                });
                group.MaterialStockItemPO = MaterialStockItemPO.Get().Where(i => i.POItemId == transferItem.POItemId && i.LocaleId == transferItem.POItemLocaleId).ToList();
                group.SaveReceivedLogOption = group.POItem.PurLocaleId != transferItem.TargetCompanyId ?
                new SaveReceivedLogOption
                {
                    ReceivedType = 1,
                    SaveQCResult = false,
                    SaveStockIn = false,
                    SaveStockOut = false,

                } :
                new SaveReceivedLogOption
                {
                    ReceivedType = 1,
                    SaveQCResult = group.POItem.SamplingMethod == 2 ? true : false,
                    SaveStockIn = group.POItem.SamplingMethod == 2 ? true : false,
                    SaveStockOut = group.POItem.SamplingMethod == 2 ? true : false,

                };
            }
            return group;
        }

        /*
        *   收貨的作業流程
        *   分成來料收貨或拖外加工收貨 type = 2 拖外加工收貨，
        *      來料收貨：視前端參數要不要做驗收及出庫
        *      拖外收貨：做驗收及出入庫
        *   Step1 : 收貨資料新增或更新，如果要做驗收(QCDate,IQCMen,IQCResult,IQCFlag,IQCRemark)會填入資料，如果有Size也會同時新增到 ReceivedLogSizeItem，
        *   Step2 : 來料收貨入庫，檢查MaterialStock，沒有就新增，沒有MaterialStock同時新增MaterialStockItem、MaterialStockItemSize。已經有入庫資料的，就不動作
        *   Step3 : 拖外收貨入庫，檢查MaterialStock，沒有就新增，同時刪除MaterialStockItem後新增入庫跟出庫
        *   Step4 : 更新MaterialStockItemSize的入庫數
        *
        */
        public ERP.Models.Views.ReceivedLogGroup Save(ReceivedLogGroup item)
        {
            var type = item.SaveReceivedLogOption.ReceivedType;
            var saveQCResult = item.SaveReceivedLogOption.SaveQCResult;
            var saveStockIn = item.SaveReceivedLogOption.SaveStockIn;
            var saveStockOut = item.SaveReceivedLogOption.SaveStockOut;

            var receivedLog = item.ReceivedLog;
            var receivedLogSizeItem = item.ReceivedLogSizeItem.ToList();
            var materialStock = new ERP.Models.Views.MaterialStock();
            var poItem = item.POItem;

            var ioDate = receivedLog.ReceivedDate;
            var stockCurrency = "USD";
            var exchangeRate = (decimal)0;
            var exchangeItems = new List<Models.Views.ExchangeRate>();

            var isChangeWarehouse = false;
            try
            {
                UnitOfWork.BeginTransaction();

                // Step1: 收貨資料處理
                if (receivedLog != null)
                {
                    if (saveQCResult)   //處理是否直接驗收
                    {
                        receivedLog.QCDate = receivedLog.ReceivedDate;
                        receivedLog.IQCMen = receivedLog.ModifyUserName;
                        receivedLog.IQCResult = 2;
                        receivedLog.IQCFlag = 1;
                        receivedLog.IQCRemark = receivedLog.TransferInId == 0 ? "收貨直接QC&入庫" : "收貨直接QC&入庫(轉廠)";

                        // 匯率的建法，只有美金對全部幣別，還有全部幣別對台幣。所以最好的方式是把資料一次都抓出來(最近出庫日的那天所有幣別)。
                        var _exchDate = ExchangeRate.Get().Where(i => i.ExchDate <= ioDate).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate);
                        exchangeItems = ExchangeRate.Get().Where(i => i.ExchDate == _exchDate).OrderByDescending(i => i.ExchDate).ToList();
                    }

                    //ReceivedLog
                    {
                        // 如果有庫存，就取得庫存幣別
                        {
                            materialStock = MaterialStock.Get()
                                .Where(i => i.OrderNo == receivedLog.OrderNo && i.LocaleId == receivedLog.LocaleId &&
                                            i.MaterialId == receivedLog.MaterialId && i.ParentMaterialId == 0 &&
                                            i.WarehouseId == receivedLog.WarehouseId && i.PCLUnitCodeId == receivedLog.PurUnitCodeId).FirstOrDefault();
                            if (materialStock != null)
                            {
                                stockCurrency = materialStock.StockDollarNameTw;
                                receivedLog.StockDollarCodeId = materialStock.StockDollarCodeId;
                                receivedLog.StockDollarNameTw = materialStock.StockDollarNameTw;
                            }
                            else
                            {
                                receivedLog.StockDollarNameTw = stockCurrency;
                            }
                        }

                        // 取出所有收貨記錄，判斷是否可以超收跟重複收貨
                        // var _allLog = ReceivedLog.Get().Where(i => i.LocaleId == receivedLog.LocaleId && i.RefLocaleId == receivedLog.RefLocaleId && i.POItemId == receivedLog.POItemId).ToList();
                        var _allLog = ReceivedLog.Get().Where(i => i.RefLocaleId == receivedLog.RefLocaleId && i.POItemId == receivedLog.POItemId).ToList();    // 改用 po 條碼找全集團的收獲數，避免重複收貨
                        var _receivedLog = _allLog.Where(i => i.LocaleId == receivedLog.LocaleId && i.Id == receivedLog.Id).FirstOrDefault();
                        // var _receivedLog = ReceivedLog.Get().Where(i => i.LocaleId == receivedLog.LocaleId && i.Id == receivedLog.Id).FirstOrDefault();

                        if (_receivedLog != null)
                        {
                            // 因為是要入庫後產生的資料，改最後再更新
                            // var stockQty = MaterialStockItem.Get().Where(i => i.ReceivedLogId == _receivedLog.Id && i.LocaleId == _receivedLog.LocaleId).Sum(i => i.PCLIOQty);
                            // receivedLog.StockQty = stockQty;

                            receivedLog.Id = _receivedLog.Id;
                            receivedLog.LocaleId = _receivedLog.LocaleId;

                            // 判斷是不是收貨更改倉庫，因為這個會涉及到入庫問題
                            if (receivedLog.WarehouseId != _receivedLog.WarehouseId)
                            {
                                isChangeWarehouse = true;
                            }

                            //更新單價
                            if (poItem != null)
                            {
                                receivedLog.SubTotalPrice = (decimal)poItem.UnitPrice * receivedLog.PayQty;
                                receivedLog.UnitPrice = (decimal)poItem.UnitPrice;
                                receivedLog.PCLUnitCodeId = poItem.UnitCodeId;
                                receivedLog.PCLUnitNameTw = poItem.Unit;
                                receivedLog.PurUnitCodeId = poItem.UnitCodeId;
                                receivedLog.PurUnitNameTw = poItem.Unit;
                                receivedLog.PurDollarCodeId = poItem.PayDollarCodeId;
                                receivedLog.PurDollarNameTw = poItem.Currency;
                            }
                            receivedLog = ReceivedLog.Update(receivedLog);
                        }
                        else
                        {
                            // 判斷是否重複，目前用超收來判斷
                            var maxQty = receivedLog.MaxReceivedQty;
                            var totalQty = _allLog.Sum(i => i.PayQty);

                            if ((totalQty + receivedLog.PayQty) > maxQty)
                            {
                                UnitOfWork.Rollback();
                                throw null;
                            }

                            if (poItem != null)
                            {
                                receivedLog.SubTotalPrice = (decimal)poItem.UnitPrice * receivedLog.PayQty;
                            }
                            receivedLog = ReceivedLog.Create(receivedLog);
                        }
                    }

                    //ReceivedLog Size Item
                    {
                        if (receivedLog.Id != 0)
                        {
                            ReceivedLogSizeItem.RemoveRange(i => i.ReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId);
                            receivedLogSizeItem.ForEach(i =>
                            {
                                i.ReceivedLogId = receivedLog.Id;
                                i.LocaleId = receivedLog.LocaleId;
                                i.StockQty = saveStockIn ? i.IQCGetQty : i.StockQty; // 如果直接入庫就讓StockQty = IQCGetQty
                                i.ShoeInnerSize = i.ShoeInnerSize == null ? Convert.ToDouble(i.SeqNo) : i.ShoeInnerSize;
                            });
                            ReceivedLogSizeItem.CreateRange(receivedLogSizeItem);
                        }
                    }

                    // Step2: 出入庫資料更新
                    if (type != 2)  // 來料收貨直接入庫
                    {
                        if (saveStockIn)
                        {
                            // stpe2-1: 有沒有MaterialaStock,沒有就新增一筆
                            materialStock = MaterialStock.Get()
                                .Where(i => i.OrderNo == receivedLog.OrderNo && i.LocaleId == receivedLog.LocaleId &&
                                            i.MaterialId == receivedLog.MaterialId && i.ParentMaterialId == 0 &&
                                            i.WarehouseId == receivedLog.WarehouseId && i.PCLUnitCodeId == receivedLog.PurUnitCodeId).FirstOrDefault();

                            if (materialStock == null)
                            {
                                // 新資料，庫存幣別一律為USD, 所以抓的是UST to Others, 用ReversedBankingRate
                                exchangeRate = poItem.Currency == "USD" ? 1 :
                                exchangeItems.Where(i => i.CurrencyTw == stockCurrency && i.TransCurrencyTw == poItem.Currency).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault();
                                materialStock = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                                {
                                    LocaleId = receivedLog.LocaleId,
                                    MaterialId = receivedLog.MaterialId,
                                    MaterialName = receivedLog.MaterialNameTw,
                                    MaterialNameEng = receivedLog.MaterialNameEng,
                                    WarehouseId = receivedLog.WarehouseId,
                                    WarehouseNo = receivedLog.WarehouseNo,
                                    OrderNo = receivedLog.OrderNo,
                                    PCLUnitCodeId = receivedLog.PCLUnitCodeId,
                                    PCLUnitNameTw = receivedLog.PCLUnitNameTw,
                                    PCLUnitNameEn = receivedLog.PCLUnitNameTw,
                                    TransRate = 1,
                                    PurUnitCodeId = receivedLog.PurUnitCodeId,
                                    PurUnitNameTw = receivedLog.PurUnitNameTw,
                                    PurUnitNameEn = receivedLog.PurUnitNameTw,
                                    PCLPlanQty = poItem.PlanQty ?? 0,
                                    // PCLQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                                    // PurQty = (receivedLog.TotalQty - receivedLog.FreeQty),
                                    PCLQty = receivedLog.IQCGetQty,
                                    PurQty = receivedLog.ReceivedQty,
                                    PCLAllocationQty = 0,
                                    PurAllocationQty = 0,
                                    Amount = (receivedLog.TotalQty - receivedLog.FreeQty) * receivedLog.UnitPrice,
                                    StockDollarCodeId = (decimal)receivedLog.StockDollarCodeId,
                                    StockDollarNameTw = receivedLog.StockDollarNameTw,
                                    StockDollarNameEn = receivedLog.StockDollarNameTw,
                                    ParentMaterialId = 0,
                                    ParentMaterialNameTw = "",
                                    ParentMaterialNameEn = "",
                                    LastStockIOId = 0,
                                    ModifyUserName = receivedLog.ModifyUserName,
                                    PurUnitPrice = receivedLog.UnitPrice,
                                    // PurDollarCodeId = receivedLog.PurDollarCodeId,
                                    // PurDollarNameEn = receivedLog.PurDollarNameTw,
                                    // PurDollarNameTw = receivedLog.PurDollarNameTw,
                                    // ExchangeRate = exchangeRate
                                });
                            }
                            else
                            {
                                // 舊資料，要判斷庫存幣別，USD, 所以抓的是UST to Others, 用ReversedBankingRate, NTD 要使用 Otehrs to NTD, 用的是BankingRate
                                stockCurrency = materialStock.StockDollarNameTw;
                                exchangeRate = poItem.Currency == stockCurrency ? 1 : stockCurrency == "USD" ?
                                exchangeItems.Where(i => i.CurrencyTw == stockCurrency && i.TransCurrencyTw == poItem.Currency).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault() :
                                exchangeItems.Where(i => i.CurrencyTw == poItem.Currency && i.TransCurrencyTw == stockCurrency).OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();
                            }

                            // 因為有拖外加工的種類，所以來料入庫的單價不一定等於收貨單價，要先判斷
                            var _purUnitPrice = receivedLog.UnitPrice;
                            var _purDollarCodeId = receivedLog.PurDollarCodeId;
                            var _purUnitPriceLog = "";
                            var _hasStockInPO = false;

                            // Step1, 判斷是否有來料加工
                            // dbo.ProcessPO.POItemId, dbo.POItem.Id, dbo.ProcessPO.LocaleId, dbo.POItem.LocaleId, dbo.POItem.Qty, dbo.ProcessPO.MaterialCost, dbo.ProcessPO.Id
                            var _stockInPOs = MaterialStockItemPO.Get().Where(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId).ToList();
                            //拖外加工單價需要在加上原材料單價，這裡的情況可能發生當入庫的時候選錯拖外採購單，所以改用這張拖外採購單所有的材料出庫金額,所有出庫數量，作為平均的材料單價，如果有兩張拖外出庫都選同一張採購單以總和加總問題少
                            if (_stockInPOs.Any())
                            {
                                //抓出出入庫日期最近的一筆美金/台幣匯率，存入MaterialStockItemPO的已經換算過了
                                var _usdTontd = exchangeItems.Where(i => i.ExchDate <= ioDate && i.CurrencyTw == "USD" && i.TransCurrencyTw == "NTD").OrderByDescending(i => i.ExchDate).First();

                                _stockInPOs.ForEach(i =>
                                {
                                    if (materialStock.StockDollarCodeId != i.StockDollarCodeId)
                                    {
                                        i.MaterialCost = materialStock.StockDollarNameTw == "USD" ? i.MaterialCost * _usdTontd.ReversedBankingRate : i.MaterialCost * _usdTontd.BankingRate;
                                    }
                                    i.StockOutQty = i.StockOutQty < 0 ? (0 - i.StockOutQty) : i.StockOutQty;
                                });
                                var _stockInPO = _stockInPOs.GroupBy(i => new { i.POItemId, i.LocaleId, i.PurQty }).Select(i => new { i.Key.POItemId, i.Key.LocaleId, PurQty = i.Key.PurQty, StockOutQty = i.Sum(g => g.StockOutQty), MaterialCost = i.Sum(g => g.MaterialCost) }).First();
                                // 貼合單價 = 收貨單價＋原材料拖外出庫的單價
                                // _purUnitPrice = _stockInPO == null ? _purUnitPrice : (decimal)((receivedLog.UnitPrice * exchangeRate) + (_stockInPO.MaterialCost / _stockInPO.PurQty)); // 用採購數量：選錯拖外採購單的雖然金額錯，但出庫的領料金額應該會一樣
                                _purUnitPrice = _stockInPO == null ? _purUnitPrice : (decimal)((receivedLog.UnitPrice * exchangeRate) + (_stockInPO.MaterialCost / _stockInPO.StockOutQty)); // 用採購數量：選錯拖外採購單的雖然金額錯，但出庫的領料金額應該會一樣
                                _purDollarCodeId = _stockInPO == null ? _purDollarCodeId : materialStock.StockDollarCodeId;
                                _purUnitPriceLog = string.Format("P:{0:F6}+M:{1:F6}", poItem.UnitPrice * exchangeRate, _stockInPO.MaterialCost / _stockInPO.StockOutQty); //拖外記錄單價組成，這時候exchangeRate是POItem的

                                exchangeRate = _stockInPO == null ? exchangeRate : 1; //如果有拖外出庫，這時候exchangeRate改為1
                                _hasStockInPO = true;
                            }

                            // Step2, 處理入庫資料，因為有可能更改倉庫資料，所以庫存要特別處理
                            // 判斷有沒有這筆收貨有沒有StockIO，先取會全部的StockIO, 入如果裡面有相同的 receivedLogId 跟 數量(如果重複收貨就會沒有庫存), 就不處理入庫數量，只更新匯率(就是只更新收貨)
                            var stockItems = new List<ERP.Models.Views.MaterialStockItem> { };

                            if (isChangeWarehouse)
                            {
                                stockItems = MaterialStockItem.Get().Where(i => i.ReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId).ToList();
                            }
                            else
                            {
                                stockItems = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId).ToList();
                            }
                            var _exisStockIn = stockItems.Where(i => i.ReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId).FirstOrDefault();

                            if (_exisStockIn == null)
                            {
                                var sumQty = stockItems.Sum(s => s.PCLIOQty);
                                var maxSeq = stockItems.Max(s => s.SeqNo) == null ? 0 : stockItems.Max(s => s.SeqNo);
                                var stockIn = new MaterialStockItem()
                                {
                                    LocaleId = materialStock.LocaleId,
                                    IODate = ioDate,
                                    SourceType = 0,
                                    MaterialId = materialStock.MaterialId,
                                    WarehouseId = materialStock.WarehouseId,
                                    OrderNo = materialStock.OrderNo,
                                    PCLUnitCodeId = materialStock.PCLUnitCodeId,
                                    TransRate = 1,
                                    PurUnitCodeId = materialStock.PurUnitCodeId,
                                    // PCLIOQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                                    // PurIOQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                                    PCLIOQty = receivedLog.IQCGetQty,
                                    PurIOQty = receivedLog.ReceivedQty,
                                    ReceivedLogId = receivedLog.Id,
                                    PurUnitPrice = _purUnitPrice,//receivedLog.UnitPrice,
                                    PurDollarCodeId = _purDollarCodeId,//receivedLog.PurDollarCodeId,
                                    BankingRate = exchangeRate,
                                    StockDollarCodeId = materialStock.StockDollarCodeId,
                                    Remark = receivedLog.TransferInId == 0 ? "來料(收貨直接QC入庫)" : "轉廠來料(收貨直接QC入庫)",
                                    RefNo = "QCA-" + receivedLog.Id,
                                    OrgUnitId = 0,
                                    OrgUnitNameTw = "",
                                    OrgUnitNameEn = _hasStockInPO ? "拖外" : "",
                                    MPSProcessId = 0,
                                    MPSProcessNameTw = "",
                                    MPSProcessNameEn = _purUnitPriceLog,    // 拖外單價
                                    RefUserName = receivedLog.ModifyUserName,
                                    MaterialStockId = materialStock.Id,
                                    PrePCLQty = sumQty,
                                    PreAmount = 0,
                                    ModifyUserName = receivedLog.ModifyUserName,
                                    SeqNo = maxSeq + 1
                                };
                                _exisStockIn = MaterialStockItem.Create(stockIn);
                            }
                            else
                            {
                                // 如果是更換倉庫，就直接用這筆更換，換掉倉庫跟MaterialStockId
                                if (isChangeWarehouse)
                                {
                                    _exisStockIn.MaterialStockId = materialStock.Id;
                                    _exisStockIn.StockDollarCodeId = materialStock.StockDollarCodeId;
                                    _exisStockIn.WarehouseId = materialStock.WarehouseId;
                                    _exisStockIn.WarehouseNo = materialStock.WarehouseNo;
                                }

                                _exisStockIn.BankingRate = exchangeRate;
                                _exisStockIn.PurUnitPrice = _purUnitPrice;
                                _exisStockIn.PurDollarCodeId = _purDollarCodeId;
                                _exisStockIn.IODate = receivedLog.ReceivedDate;
                                _exisStockIn.PCLIOQty = receivedLog.IQCGetQty;
                                _exisStockIn.PurIOQty = receivedLog.ReceivedQty;
                                _exisStockIn.ModifyUserName = receivedLog.ModifyUserName;
                                _exisStockIn.MPSProcessNameEn = _purUnitPriceLog;    // 拖外單價
                                _exisStockIn.OrgUnitNameEn = _hasStockInPO ? "拖外" : ""; // 拖外

                                _exisStockIn = MaterialStockItem.Update(_exisStockIn);
                            }

                            if (receivedLogSizeItem.Count() > 0)
                            {
                                MaterialStockItemSize.RemoveRange(i => i.StockIOId == _exisStockIn.Id && i.LocaleId == _exisStockIn.LocaleId);
                                var stockInSize = ReceivedLogSizeItem.Get().Where(i => i.ReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId)
                                        .Select(i => new MaterialStockItemSize
                                        {
                                            LocaleId = _exisStockIn.LocaleId,
                                            StockIOId = _exisStockIn.Id,
                                            ShoeSize = Convert.ToDecimal(i.SeqNo),
                                            ShoeSizeSuffix = "",
                                            ShoeInnerSize = (double)i.ShoeInnerSize,
                                            PCLQty = i.ReceivedQty,
                                            PurQty = i.ReceivedQty,
                                            ReLogSizeItemId = i.Id,
                                            DisplaySize = i.DisplaySize,
                                        }).ToList();

                                stockInSize.ForEach(i =>
                                {
                                    i.ShoeSize = i.ShoeInnerSize < 1000 ? (decimal)i.ShoeInnerSize : (decimal)i.ShoeInnerSize / 1000;
                                    i.ShoeSizeSuffix = i.ShoeInnerSize < 1000 ? "J" : "";
                                });

                                MaterialStockItemSize.CreateRange(stockInSize);
                            }

                            // Upadate MateriaStock PCLQty
                            _MaterialStock.UpdateMaterialStockInfo((int)materialStock.Id, (int)materialStock.LocaleId);
                        }
                    }

                    if (type == 2)  // 託外收貨直接入、出庫
                    {
                        materialStock = MaterialStock.Get()
                            .Where(i => i.OrderNo == receivedLog.OrderNo && i.LocaleId == receivedLog.LocaleId &&
                                        i.MaterialId == receivedLog.MaterialId && i.ParentMaterialId == 0 &&
                                        i.WarehouseId == receivedLog.WarehouseId && i.PCLUnitCodeId == receivedLog.PurUnitCodeId).FirstOrDefault();
                        if (materialStock == null)
                        {
                            exchangeRate = poItem.Currency == "USD" ? 1 :
                            exchangeItems.Where(i => i.CurrencyTw == stockCurrency && i.TransCurrencyTw == poItem.Currency).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault();
                            materialStock = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                            {
                                // Id = i.Id,
                                LocaleId = receivedLog.LocaleId,
                                MaterialId = receivedLog.MaterialId,
                                MaterialName = receivedLog.MaterialNameTw,
                                MaterialNameEng = receivedLog.MaterialNameEng,
                                WarehouseId = receivedLog.WarehouseId,
                                WarehouseNo = receivedLog.WarehouseNo,
                                OrderNo = receivedLog.OrderNo,
                                PCLUnitCodeId = receivedLog.PCLUnitCodeId,
                                PCLUnitNameTw = receivedLog.PCLUnitNameTw,
                                PCLUnitNameEn = receivedLog.PCLUnitNameTw,
                                TransRate = 1,
                                PurUnitCodeId = receivedLog.PurUnitCodeId,
                                PurUnitNameTw = receivedLog.PurUnitNameTw,
                                PurUnitNameEn = receivedLog.PurUnitNameTw,
                                PCLPlanQty = poItem.PlanQty ?? 0,
                                PCLQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                                PurQty = (receivedLog.TotalQty - receivedLog.FreeQty),
                                PCLAllocationQty = 0,
                                PurAllocationQty = 0,
                                Amount = (receivedLog.TotalQty - receivedLog.FreeQty) * receivedLog.UnitPrice,
                                StockDollarCodeId = (decimal)receivedLog.StockDollarCodeId,
                                StockDollarNameTw = receivedLog.StockDollarNameTw,
                                StockDollarNameEn = receivedLog.StockDollarNameTw,
                                ParentMaterialId = 0,
                                ParentMaterialNameTw = "",
                                ParentMaterialNameEn = "",
                                LastStockIOId = 0,
                                ModifyUserName = receivedLog.ModifyUserName,
                                PurUnitPrice = receivedLog.UnitPrice,
                                // PurDollarCodeId = receivedLog.PurDollarCodeId,
                                // ExchangeRate = exchangeRate,
                            });
                        }
                        else
                        {
                            stockCurrency = materialStock.StockDollarNameTw;
                            exchangeRate = poItem.Currency == stockCurrency ? 1 : stockCurrency == "USD" ?
                            exchangeItems.Where(i => i.CurrencyTw == stockCurrency && i.TransCurrencyTw == poItem.Currency).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault() :
                            exchangeItems.Where(i => i.CurrencyTw == poItem.Currency && i.TransCurrencyTw == stockCurrency).OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();
                        }

                        var stockIds = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId && i.RefNo == "OPDO-" + receivedLog.Id).Select(i => i.Id).ToList();
                        var stockItems = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId).ToList();
                        var sumQty = stockItems.Sum(s => s.PCLIOQty);
                        var maxSeq = stockItems.Max(s => s.SeqNo) == null ? 0 : stockItems.Max(s => s.SeqNo);

                        // 新增來料(託外加工直接入庫)
                        var stockIn = new MaterialStockItem()
                        {
                            LocaleId = materialStock.LocaleId,
                            IODate = ioDate,
                            SourceType = 0,
                            MaterialId = materialStock.MaterialId,
                            WarehouseId = materialStock.WarehouseId,
                            OrderNo = materialStock.OrderNo,
                            PCLUnitCodeId = materialStock.PCLUnitCodeId,
                            TransRate = 1,
                            PurUnitCodeId = materialStock.PurUnitCodeId,
                            PCLIOQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                            PurIOQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                            ReceivedLogId = receivedLog.Id,
                            PurUnitPrice = receivedLog.UnitPrice,
                            PurDollarCodeId = receivedLog.PurDollarCodeId,
                            BankingRate = exchangeRate,
                            StockDollarCodeId = materialStock.StockDollarCodeId,
                            Remark = "來料(託外加工直接入庫)",
                            RefNo = "OPDI-" + receivedLog.Id,
                            OrgUnitId = 0,
                            OrgUnitNameTw = "",
                            OrgUnitNameEn = "",
                            MPSProcessId = 0,
                            MPSProcessNameTw = "",
                            MPSProcessNameEn = "",
                            RefUserName = receivedLog.ModifyUserName,
                            MaterialStockId = materialStock.Id,
                            PrePCLQty = sumQty,
                            PreAmount = 0,
                            ModifyUserName = receivedLog.ModifyUserName,
                            SeqNo = maxSeq + 1
                        };

                        MaterialStockItem.RemoveRange(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId && i.RefNo == "OPDI-" + receivedLog.Id);
                        stockIn = MaterialStockItem.Create(stockIn);

                        // 來料(託外加工直接出庫)
                        var stockOut = new MaterialStockItem()
                        {
                            LocaleId = materialStock.LocaleId,
                            IODate = ioDate,
                            SourceType = 5,
                            MaterialId = materialStock.MaterialId,
                            WarehouseId = materialStock.WarehouseId,
                            OrderNo = materialStock.OrderNo,
                            PCLUnitCodeId = materialStock.PCLUnitCodeId,
                            TransRate = 1,
                            PurUnitCodeId = materialStock.PurUnitCodeId,
                            PCLIOQty = -((receivedLog.TotalQty - receivedLog.FreeQty) * 1),
                            PurIOQty = -((receivedLog.TotalQty - receivedLog.FreeQty) * 1),
                            ReceivedLogId = 0,  // 出庫的收貨Id 一律為0
                            PurUnitPrice = receivedLog.UnitPrice * exchangeRate, // 轉換成美金的庫存單價
                            PurDollarCodeId = materialStock.StockDollarCodeId,   // 出庫的採購筆別等於庫存幣別
                            BankingRate = 1,
                            StockDollarCodeId = materialStock.StockDollarCodeId,
                            Remark = "來料(託外加工直接出庫)",
                            RefNo = "OPDO-" + receivedLog.Id,
                            OrgUnitId = 0,
                            OrgUnitNameTw = "",
                            OrgUnitNameEn = "",
                            MPSProcessId = 0,
                            MPSProcessNameTw = "",
                            MPSProcessNameEn = "",
                            RefUserName = receivedLog.ModifyUserName,
                            MaterialStockId = materialStock.Id,
                            PrePCLQty = sumQty + stockIn.PCLIOQty,
                            PreAmount = (sumQty + stockIn.PCLIOQty) * (receivedLog.UnitPrice * exchangeRate) * 1 * 1,
                            ModifyUserName = receivedLog.ModifyUserName,
                            SeqNo = maxSeq + 2
                        };
                        MaterialStockItem.RemoveRange(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId && i.RefNo == "OPDO-" + receivedLog.Id);
                        var stockItem = MaterialStockItem.Create(stockOut);

                        // 拖外加工出庫-特殊流程，拖外出庫有多一個拖外資料表。這理由系統抓到第一筆的拖外po塞入
                        var poTypes = new List<int?> { 2, 6 };
                        var outsourcePO = POItem.Get()
                            .Where(i => i.OrderNo != "" && i.OrderNo == stockOut.OrderNo && poTypes.Contains(i.POType) && i.Status != 2 && i.MaterialId == poItem.ParentMaterialId)
                            .FirstOrDefault();
                        MaterialStockItemPO.RemoveRange(i => stockIds.Contains(i.StockIOId) && i.LocaleId == materialStock.LocaleId);

                        if (outsourcePO != null)
                        {
                            var stockItemPO = new Models.Views.MaterialStockItemPO
                            {
                                Id = 0,
                                LocaleId = stockOut.LocaleId,
                                POId = outsourcePO.POId,
                                POItemId = outsourcePO.Id,

                                MaterialId = outsourcePO.MaterialId,
                                StockIOId = stockItem.Id,
                                MaterialCost = receivedLog.UnitPrice == 0 ? 0 : (stockIn.PCLIOQty * receivedLog.UnitPrice * exchangeRate),//stockItem.Amount == null ? 0 : (decimal)stockItem.Amount,
                                StockDollarCodeId = materialStock.StockDollarCodeId,
                                OrderNo = materialStock.OrderNo,
                                OPCount = 1,
                                ModifyUserName = stockOut.ModifyUserName,
                            };
                            MaterialStockItemPO.CreateRange(new List<ERP.Models.Views.MaterialStockItemPO> { stockItemPO });
                        }

                        //最後更新 MaterialStock，另一個Transaction.
                        _MaterialStock.UpdateMaterialStockInfo((int)materialStock.Id, (int)materialStock.LocaleId);
                    }

                    // 更新收貨的庫存總數
                    if (receivedLog.Id != 0)
                    {
                        _ReceivedLog.UpdateStockQty(new List<decimal> { receivedLog.Id }, receivedLog.LocaleId);
                    }
                }


                UnitOfWork.Commit();

            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)receivedLog.Id, (int)receivedLog.LocaleId);
        }

        public void Remove(ReceivedLogGroup item)
        {
            var type = item.ReceivedType;
            var receivedLog = item.ReceivedLog;
            var reLogSizeItem = item.ReceivedLogSizeItem;

            UnitOfWork.BeginTransaction();
            try
            {
                // 有Size才刪除
                // var ids = reLogSizeItem.Select(i => i.Id).ToArray();
                // MaterialStockItemSize.RemoveRange(i => ids.Contains(i.ReLogSizeItemId) && i.LocaleId == receivedLog.LocaleId);

                //先刪除庫存Size
                MaterialStockItemSize.RemoveRange(i =>
                    ReceivedLogSizeItem.Get().Where(r => r.ReceivedLogId == receivedLog.Id && r.LocaleId == receivedLog.LocaleId).Select(r => r.Id).Contains(i.ReLogSizeItemId) &&
                    i.LocaleId == receivedLog.LocaleId);

                // 刪除直接入、出庫 MaterialStock, StockIO
                if (type != 2)
                {
                    var materialStock = MaterialStock.Get()
                        .Where(i => i.OrderNo == receivedLog.OrderNo && i.LocaleId == receivedLog.LocaleId &&
                                    i.MaterialId == receivedLog.MaterialId && i.ParentMaterialId == 0 &&
                                    i.WarehouseId == receivedLog.WarehouseId && i.PCLUnitCodeId == receivedLog.PurUnitCodeId).FirstOrDefault();

                    if (materialStock != null)
                    {

                        MaterialStockItem.RemoveRange(i => i.ReceivedLogId == receivedLog.Id &&
                                                           i.LocaleId == receivedLog.LocaleId && i.RefNo.EndsWith("-" + receivedLog.Id) &&
                                                           i.OrderNo == receivedLog.OrderNo && i.MaterialStockId == materialStock.Id);

                        var hasItem = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId).Any();

                        // 沒有入庫就會刪掉MaterialStock
                        if (hasItem)
                        {
                            _MaterialStock.UpdateMaterialStockInfo((int)materialStock.Id, (int)materialStock.LocaleId);
                        }
                        else
                        {
                            _MaterialStock.Remove(materialStock);
                        }
                    }
                }

                // 託外收貨直接入、出庫
                if (type == 2)
                {
                    var materialStock = MaterialStock.Get()
                        .Where(i => i.OrderNo == receivedLog.OrderNo && i.LocaleId == receivedLog.LocaleId &&
                                    i.MaterialId == receivedLog.MaterialId && i.ParentMaterialId == 0 &&
                                    i.WarehouseId == receivedLog.WarehouseId && i.PCLUnitCodeId == receivedLog.PurUnitCodeId).FirstOrDefault();

                    // var stockIds = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId && i.RefNo == "OPDO-" + receivedLog.Id).Select(i => i.Id).ToList();
                    var stockIds = MaterialStockItem.Get().Where(i => i.LocaleId == receivedLog.LocaleId && i.RefNo.EndsWith("-" + receivedLog.Id) && i.OrderNo == receivedLog.OrderNo && i.MaterialStockId == materialStock.Id).Select(i => i.Id).ToList();
                    MaterialStockItemPO.RemoveRange(i => stockIds.Contains(i.StockIOId) && i.LocaleId == materialStock.LocaleId);
                    MaterialStockItem.RemoveRange(i => i.LocaleId == receivedLog.LocaleId && i.RefNo.EndsWith("-" + receivedLog.Id) && i.OrderNo == receivedLog.OrderNo && i.MaterialStockId == materialStock.Id);



                    var hasItem = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId).Any();

                    // 沒有入庫就會刪掉MaterialStock
                    if (hasItem)
                    {
                        _MaterialStock.UpdateMaterialStockInfo((int)materialStock.Id, (int)materialStock.LocaleId);
                    }
                    else
                    {
                        _MaterialStock.Remove(materialStock);
                    }
                }


                //庫存刪除完在刪除ReceivedLog
                ReceivedLogSizeItem.RemoveRange(i => i.ReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId);
                ReceivedLog.Remove(receivedLog);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        // 收貨單價異常
        public IQueryable<ERP.Models.Views.ReceivedLogDisagree> GetReceivedLogPriceDisagree(string predicate)
        {
            var items = ReceivedLog.Get()
                .Select(i => new
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    RefLocaleId = i.RefLocaleId,
                    ReceivedDate = i.ReceivedDate,
                    ShippingListVendorId = i.ShippingListVendorId,
                    ShippingListVendorName = i.ShippingListVendorName,
                    POItemId = i.POItemId,
                    UnitPrice = i.UnitPrice,
                    ReceivedQty = i.ReceivedQty,
                    SubTotalPrice = i.SubTotalPrice,
                    Remark = i.Remark,
                    WarehouseId = i.WarehouseId,
                    StockQty = i.StockQty,
                    OrderNo = i.OrderNo,
                    TransferInId = i.TransferInId,
                    TransferInLocaleId = i.TransferInLocaleId,
                    TransferQty = i.TransferQty,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    RefPONo = i.RefPONo,
                    MaterialId = i.MaterialId,
                    MaterialNameTw = i.MaterialNameTw,
                    ParentMaterialId = i.ParentMaterialId,
                    ParentMaterialNameTw = i.ParentMaterialNameTw,
                    PCLUnitCodeId = i.PCLUnitCodeId,
                    PCLUnitNameTw = i.PCLUnitNameTw,
                    PurUnitCodeId = i.PurUnitCodeId,
                    PurUnitNameTw = i.PurUnitNameTw,
                    PayQty = i.PayQty,
                    FreeQty = i.FreeQty,
                    TotalQty = i.PayQty + i.FreeQty,
                    PurDollarCodeId = i.PurDollarCodeId,
                    PurDollarNameTw = i.PurDollarNameTw,
                    StockDollarCodeId = i.StockDollarCodeId,
                    StockDollarNameTw = i.StockDollarNameTw,
                    ReceivedBarcode = i.ReceivedBarcode,
                    CloseMonth = Convert.ToInt32(i.CloseMonth),
                    CloseMonth1 = i.CloseMonth,
                    POType = i.POType,
                    MaterialNameEng = i.MaterialNameEng,
                    WarehouseNo = i.WarehouseNo,
                    PurUnitPrice = i.PurUnitPrice,
                    POLastUpdateTime = i.POLastUpdateTime,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Where(i => i.UnitPrice != i.PurUnitPrice)
                .Select(i => new ERP.Models.Views.ReceivedLogDisagree
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    RefLocaleId = i.RefLocaleId,
                    ReceivedDate = i.ReceivedDate,
                    ShippingListVendorId = i.ShippingListVendorId,
                    ShippingListVendorName = i.ShippingListVendorName,
                    POItemId = i.POItemId,
                    UnitPrice = i.UnitPrice,
                    ReceivedQty = i.ReceivedQty,
                    SubTotalPrice = i.SubTotalPrice,
                    Remark = i.Remark,
                    WarehouseId = i.WarehouseId,
                    StockQty = i.StockQty,
                    OrderNo = i.OrderNo,
                    TransferInId = i.TransferInId,
                    TransferInLocaleId = i.TransferInLocaleId,
                    TransferQty = i.TransferQty,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    RefPONo = i.RefPONo,
                    MaterialId = i.MaterialId,
                    MaterialNameTw = i.MaterialNameTw,
                    ParentMaterialId = i.ParentMaterialId,
                    ParentMaterialNameTw = i.ParentMaterialNameTw,
                    PCLUnitCodeId = i.PCLUnitCodeId,
                    PCLUnitNameTw = i.PCLUnitNameTw,
                    PurUnitCodeId = i.PurUnitCodeId,
                    PurUnitNameTw = i.PurUnitNameTw,
                    PayQty = i.PayQty,
                    FreeQty = i.FreeQty,
                    TotalQty = i.PayQty + i.FreeQty,
                    PurDollarCodeId = i.PurDollarCodeId,
                    PurDollarNameTw = i.PurDollarNameTw,
                    StockDollarCodeId = i.StockDollarCodeId,
                    StockDollarNameTw = i.StockDollarNameTw,
                    ReceivedBarcode = i.ReceivedBarcode,
                    CloseMonth = i.CloseMonth1,
                    POType = i.POType,
                    MaterialNameEng = i.MaterialNameEng,
                    WarehouseNo = i.WarehouseNo,
                    PurUnitPrice = i.PurUnitPrice,
                    POLastUpdateTime = i.POLastUpdateTime,
                })
                .ToList();

            return items.AsQueryable();
        }
        // 更新單價異常
        public IQueryable<ERP.Models.Views.ReceivedLogDisagree> UpdateReceivedLogDisagree(List<int> Ids, int localeId)
        {
            var items = ReceivedLog.Get().Where(i => Ids.Contains((int)i.Id) && i.LocaleId == localeId).ToList();

            items.ForEach(i =>
            {
                _ReceivedLog.UpdatePrice(i);
                MaterialStockItem.UpdatePrice(i);
            });

            return ReceivedLog.Get()
                    .Where(i => Ids.Contains((int)i.Id) && i.LocaleId == localeId)
                    .Select(i => new ERP.Models.Views.ReceivedLogDisagree
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        RefLocaleId = i.RefLocaleId,
                        ReceivedDate = i.ReceivedDate,
                        ShippingListVendorId = i.ShippingListVendorId,
                        ShippingListVendorName = i.ShippingListVendorName,
                        POItemId = i.POItemId,
                        UnitPrice = i.UnitPrice,
                        ReceivedQty = i.ReceivedQty,
                        SubTotalPrice = i.SubTotalPrice,
                        Remark = i.Remark,
                        WarehouseId = i.WarehouseId,
                        StockQty = i.StockQty,
                        OrderNo = i.OrderNo,
                        TransferInId = i.TransferInId,
                        TransferInLocaleId = i.TransferInLocaleId,
                        TransferQty = i.TransferQty,
                        ModifyUserName = i.ModifyUserName,
                        LastUpdateTime = i.LastUpdateTime,
                        RefPONo = i.RefPONo,
                        MaterialId = i.MaterialId,
                        MaterialNameTw = i.MaterialNameTw,
                        ParentMaterialId = i.ParentMaterialId,
                        ParentMaterialNameTw = i.ParentMaterialNameTw,
                        PCLUnitCodeId = i.PCLUnitCodeId,
                        PCLUnitNameTw = i.PCLUnitNameTw,
                        PurUnitCodeId = i.PurUnitCodeId,
                        PurUnitNameTw = i.PurUnitNameTw,
                        PayQty = i.PayQty,
                        FreeQty = i.FreeQty,
                        TotalQty = i.PayQty + i.FreeQty,
                        PurDollarCodeId = i.PurDollarCodeId,
                        PurDollarNameTw = i.PurDollarNameTw,
                        StockDollarCodeId = i.StockDollarCodeId,
                        StockDollarNameTw = i.StockDollarNameTw,
                        ReceivedBarcode = i.ReceivedBarcode,
                        CloseMonth = i.CloseMonth,
                        POType = i.POType,
                        MaterialNameEng = i.MaterialNameEng,
                        WarehouseNo = i.WarehouseNo,
                        PurUnitPrice = i.PurUnitPrice,
                        POLastUpdateTime = i.POLastUpdateTime,
                    });
        }
    }
}
