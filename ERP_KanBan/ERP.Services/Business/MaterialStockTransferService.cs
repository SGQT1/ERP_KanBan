using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace ERP.Services.Business
{
    public class MaterialStockTransferService : BusinessService
    {
        private ERP.Services.Business.Entities.MaterialStockService MaterialStock { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemPOService MaterialStockItemPO { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemSizeService MaterialStockItemSize { get; set; }
        private ERP.Services.Business.Entities.MaterialStockSizeService MaterialStockSize { get; set; }

        private ERP.Services.Business.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogSizeItemService ReceivedLogSizeItem { get; set; }
        private ERP.Services.Business.Entities.POService PO { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.StockIOService StockIO { get; set; }
        private ERP.Services.Business.Entities.MaterialQuotService MaterialQuot { get; set; }
        private ERP.Services.Business.Entities.ShipmentLogService ShipmentLog { get; set; }
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.ExchangeRateService ExchangeRate { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.WarehouseService Warehouse { get; set; }
        private ERP.Services.Business.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.MPSDailyService MPSDaily { get; set; }

        public MaterialStockTransferService(
            ERP.Services.Business.Entities.MaterialStockService materialStockService,
            ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItemService,
            ERP.Services.Business.Entities.MaterialStockItemPOService MaterialStockItemPOService,
            ERP.Services.Business.Entities.MaterialStockItemSizeService materialStockItemSizeService,
            ERP.Services.Business.Entities.MaterialStockSizeService materialStockSizeService,

            ERP.Services.Business.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Business.Entities.ReceivedLogSizeItemService receivedLogSizeItemService,

            ERP.Services.Business.Entities.POService poService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Business.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Business.Entities.ShipmentLogService shipmentLogService,
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.ExchangeRateService exchangeRateService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.WarehouseService warehouseService,
            ERP.Services.Business.Entities.MaterialService materialService,
            ERP.Services.Business.Entities.MPSDailyService mpsDailyService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MaterialStock = materialStockService;
            MaterialStockItem = MaterialStockItemService;
            MaterialStockItemPO = MaterialStockItemPOService;
            MaterialStockItemSize = materialStockItemSizeService;
            MaterialStockSize = materialStockSizeService;

            PO = poService;
            POItem = poItemService;
            StockIO = stockIOService;
            MaterialQuot = materialQuotService;
            ShipmentLog = shipmentLogService;
            Orders = ordersService;
            ExchangeRate = exchangeRateService;

            CodeItem = codeItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogSizeItem = receivedLogSizeItemService;
            Warehouse = warehouseService;

            Material = materialService;
            MPSDaily = mpsDailyService;
        }

        //每次有異動，MateriaStock都要更新
        public void UpdateMaterialStockInfo(int materialStockId, int localeId)
        {
            MaterialStock.UpdatStockQty(materialStockId, localeId);
            MaterialStockSize.UpdateSizeQty(materialStockId, localeId);
        }
        // 更新收貨的入庫數(會影響來料入庫的匯入數)
        public void UpdateReceivedLogStockQty(List<decimal> receivedLogIds, int localeId)
        {
            ReceivedLog.UpdateStockQty(receivedLogIds, localeId);
        }

        public IQueryable<Models.Views.MaterialStockTransferItem> Get()
        {
            var result = (
                from so in StockIO.Get().Where(i => i.PCLIOQty < 0)
                join si in StockIO.Get().Where(i => i.PCLIOQty > 0) on new { LocaleId = so.LocaleId, RefNo = so.RefNo } equals new { LocaleId = si.LocaleId, RefNo = si.RefNo }
                join ms in MaterialStock.Get() on new { LocaleId = so.LocaleId, MaterialStockId = so.MaterialStockId } equals new { LocaleId = ms.LocaleId, MaterialStockId = ms.Id }
                select new Models.Views.MaterialStockTransferItem
                {
                    Id = so.Id,
                    LocaleId = so.LocaleId,
                    IODate = so.IODate,
                    SourceType = so.SourceType,
                    MaterialId = so.MaterialId,
                    WarehouseId = so.WarehouseId,
                    OrderNo = so.OrderNo,
                    PurUnitPrice = so.PurUnitPrice,
                    PurDollarCodeId = so.PurDollarCodeId,
                    BankingRate = so.BankingRate,
                    StockDollarCodeId = so.StockDollarCodeId,
                    Remark = so.Remark,
                    RefNo = so.RefNo,
                    MaterialStockId = so.MaterialStockId,
                    ModifyUserName = so.ModifyUserName,
                    LastUpdateTime = so.LastUpdateTime,
                    PCLIOQty = so.PCLIOQty,

                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    WarehouseNo = ms.WarehouseNo,
                    // PurCurrency = StockIO.PurDollarNameTw
                    StockCurrency = ms.StockDollarNameTw,
                    MaterialStockIdIn = si.MaterialStockId,
                    IdIn = si.Id,
                    LocaleIdIn = si.LocaleId,
                    IODateIn = si.IODate,
                    SourceTypeIn = si.SourceType,
                    MaterialIdIn = si.MaterialId,
                    WarehouseIdIn = si.WarehouseId,
                    OrderNoIn = si.OrderNo,
                    PCLIOQtyIn = si.PCLIOQty,
                    RefNoIn = si.RefNo,
                    RemarkIn = si.Remark,
                }
            );
            return result;
        }
        //出入庫 StockIO
        public Models.Views.MaterialStockTransferItemGroup GetMaterialStockTransferItemGroup(int materialStockItemId, int localeId)
        {
            var stockItemOut = MaterialStockItem.Get().Where(i => i.Id == materialStockItemId && i.LocaleId == localeId && i.PCLIOQty < 0).FirstOrDefault();
            var stockItemSize = stockItemOut != null ? MaterialStockItemSize.Get().Where(i => i.StockIOId == stockItemOut.Id && i.LocaleId == stockItemOut.LocaleId).ToList() : null;
            var stockItemIn = new Models.Views.MaterialStockItem();

            if (stockItemOut != null)
            {
                stockItemOut.UsageQty = StockIO.Get().Where(s => s.MaterialStockId == stockItemOut.MaterialStockId && s.LocaleId == stockItemOut.LocaleId && s.PCLIOQty < 0).Sum(s => s.PCLIOQty);
                stockItemIn = MaterialStockItem.Get().Where(i => i.RefNo == stockItemOut.RefNo && i.LocaleId == localeId && i.PCLIOQty > 0).FirstOrDefault();

            }

            return new Models.Views.MaterialStockTransferItemGroup
            {
                MaterialStockItemOut = stockItemOut,
                MaterialStockItemIn = stockItemIn,
                MaterialStockItemSize = stockItemSize,
            };
        }
        public Models.Views.MaterialStockTransferItemGroup SaveMaterialStockTransferItemGroup(ERP.Models.Views.MaterialStockTransferItemGroup item)
        {
            var materialStockItemOut = item.MaterialStockItemOut;
            var materialStockItemIn = item.MaterialStockItemIn;
            var materialStockItemSize = item.MaterialStockItemSize.Where(i => i.PurQty != 0).ToList();   //排出出庫數為0

            try
            {
                UnitOfWork.BeginTransaction();

                // 材料庫存互轉，轉出轉入都按照該MaterialStock做處理，採購幣別都是按照出庫的幣別別跟金額來處理，所以先要有所有出庫的出入庫來作業
                // Step1: 取得出庫的MaterialStock, 跟所有的StokIO, 要算新的單價，正常應該是匯入的時候取得，但最好是更新的時候做一次
                // 處理拖外收貨單價，只有針對入庫的才有，出庫的話這段會跳過。以下變數提供給StockIO使用
                var _purUnitPrice = materialStockItemOut.PurUnitPrice;         //採購單價，出庫的平均單價
                var _purDollarCodeId = materialStockItemOut.PurDollarCodeId;   //採購幣別
                var _exchangeRate = materialStockItemOut.BankingRate;          //根據前端帶入的幣別轉換，但可能因為出入庫放是不同，匯率會是直接用庫存別別的匯率

                // 匯率的建法，只有美金對全部幣別，還有全部幣別對台幣。所以最好的方式是把資料一次都抓出來(最近出庫日的那天所有幣別)。
                var _exchDate = ExchangeRate.Get().Where(i => i.ExchDate <= materialStockItemOut.IODate).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate);
                var exchangeItems = ExchangeRate.Get().Where(i => i.ExchDate == _exchDate).OrderByDescending(i => i.ExchDate).ToList();

                // 出庫
                var _materialStockOut = materialStockItemOut.MaterialStockId != 0 ? MaterialStock.Get().Where(i => i.Id == materialStockItemOut.MaterialStockId && i.LocaleId == materialStockItemOut.LocaleId).FirstOrDefault() :
                                            MaterialStock.Get().Where(i => i.OrderNo == materialStockItemOut.OrderNo && i.LocaleId == materialStockItemOut.LocaleId &&
                                                                    i.MaterialId == materialStockItemOut.MaterialId && i.ParentMaterialId == 0 &&
                                                                    i.WarehouseId == materialStockItemOut.WarehouseId && i.PCLUnitCodeId == materialStockItemOut.PurUnitCodeId).FirstOrDefault();

                var _materialStockIn = materialStockItemIn.MaterialStockId != 0 ? MaterialStock.Get().Where(i => i.Id == materialStockItemIn.MaterialStockId && i.LocaleId == materialStockItemIn.LocaleId).FirstOrDefault() :
                        MaterialStock.Get().Where(i => i.OrderNo == materialStockItemIn.OrderNo && i.LocaleId == materialStockItemIn.LocaleId &&
                                                i.MaterialId == materialStockItemIn.MaterialId && i.ParentMaterialId == 0 &&
                                                i.WarehouseId == materialStockItemIn.WarehouseId && i.PCLUnitCodeId == materialStockItemIn.PurUnitCodeId).FirstOrDefault();

                var _codeTypes = new List<string> { "02", "21" };
                var _codeItems = CodeItem.Get().Where(i => i.LocaleId == materialStockItemOut.LocaleId && _codeTypes.Contains(i.CodeType)).ToList();
                var _warehosue = Warehouse.Get().Where(i => i.LocaleId == materialStockItemOut.LocaleId).ToList();

                //處理出庫
                {

                    {
                        //資料重整，舊系統的有很多欄位不用，新系統要合併先把資料重整
                        materialStockItemOut.WarehouseNo = _warehosue.Where(i => i.Id == materialStockItemOut.WarehouseId).Max(i => i.WarehouseNo);
                        materialStockItemOut.PCLIOQty = materialStockItemOut.PurIOQty;
                        materialStockItemOut.PCLUnitNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.PCLUnitCodeId).Max(i => i.NameTW);
                        materialStockItemOut.PurUnit = _codeItems.Where(i => i.Id == materialStockItemOut.PurUnitCodeId).Max(i => i.NameTW);
                        materialStockItemOut.PurDollarNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.StockDollarCodeId).Max(i => i.NameTW);
                    }

                    if (_materialStockOut != null)
                    {
                        // 所有出庫的入庫資料檔做單價計算，如果有出庫前的入庫記錄，就用這範圍，沒有就用全部，比較精準
                        var stockInItems = MaterialStockItem.Get().Where(i => i.MaterialStockId == _materialStockOut.Id && i.LocaleId == _materialStockOut.LocaleId && i.PCLIOQty > 0).Select(i => new { i.Id, i.IODate, i.SourceType, i.PurDollarCodeId, i.PCLIOQty, i.PurUnitPrice, i.BankingRate, i.TransRate }).ToList();
                        var items = stockInItems.Where(i => i.SourceType == 0 && i.IODate <= materialStockItemOut.IODate).Any() ? stockInItems.Where(i => i.SourceType == 0 && i.IODate <= materialStockItemOut.IODate).ToList() : stockInItems;

                        if (items.Any())
                        {
                            var _stockInAmount = items.Sum(s => s.PCLIOQty * s.PurUnitPrice * s.BankingRate / s.TransRate);
                            var _stockInQty = items.Sum(s => s.PCLIOQty);
                            _purUnitPrice = _stockInAmount == 0 || _stockInQty == 0 ? 0 : _stockInAmount / _stockInQty;
                        }

                        //每次儲存都先更新單價
                        // materialStockItemOut.PurUnitPrice = (decimal)_avgUnitPrice;
                        // materialStockItemIn.PurUnitPrice = (decimal)_avgUnitPrice;
                        materialStockItemOut.Remark = materialStockItemOut.SourceType == 14 ? "轉批次庫存-" + materialStockItemIn.OrderNo : materialStockItemOut.SourceType == 13 ? "轉可用庫存" : materialStockItemOut.SourceType == 15 ? "批次庫存互轉-" + materialStockItemIn.OrderNo : "";
                        materialStockItemIn.Remark = materialStockItemIn.SourceType == 14 ? "轉批次庫存" : materialStockItemIn.SourceType == 13 ? "轉可用庫存-" + materialStockItemOut.OrderNo : materialStockItemIn.SourceType == 15 ? "批次庫存互轉-" + materialStockItemOut.OrderNo : "";
                    }

                    //判斷有沒有MaterialStock, 照理不會出現
                    if (_materialStockOut == null)
                    {
                        _materialStockOut = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                        {
                            LocaleId = materialStockItemOut.LocaleId,
                            MaterialId = materialStockItemOut.MaterialId,
                            MaterialName = materialStockItemOut.MaterialName,
                            MaterialNameEng = materialStockItemOut.MaterialNameEng,
                            WarehouseId = materialStockItemOut.WarehouseId,
                            WarehouseNo = materialStockItemOut.WarehouseNo,
                            OrderNo = materialStockItemOut.OrderNo,
                            PCLUnitCodeId = materialStockItemOut.PCLUnitCodeId,
                            PCLUnitNameTw = materialStockItemOut.PCLUnitNameTw,
                            PCLUnitNameEn = materialStockItemOut.PCLUnitNameTw,
                            TransRate = 1,
                            PurUnitCodeId = materialStockItemOut.PurUnitCodeId,
                            PurUnitNameTw = materialStockItemOut.PurUnit,
                            PurUnitNameEn = materialStockItemOut.PurUnit,
                            PCLPlanQty = materialStockItemOut.PCLIOQty,
                            PCLQty = materialStockItemOut.PCLIOQty,
                            PurQty = materialStockItemOut.PCLIOQty,
                            PCLAllocationQty = 0,
                            PurAllocationQty = 0,
                            Amount = (materialStockItemOut.PurIOQty) * materialStockItemOut.PurUnitPrice,
                            StockDollarCodeId = (decimal)materialStockItemOut.StockDollarCodeId,
                            StockDollarNameTw = materialStockItemOut.PurDollarNameTw,
                            StockDollarNameEn = materialStockItemOut.PurDollarNameTw,
                            ParentMaterialId = 0,
                            ParentMaterialNameTw = "",
                            ParentMaterialNameEn = "",
                            LastStockIOId = 0,
                            ModifyUserName = materialStockItemOut.ModifyUserName,
                            PurUnitPrice = materialStockItemOut.PurUnitPrice,
                            // PurDollarCodeId = materialStockItemOut.PurDollarCodeId,
                            // PurDollarNameEn = materialStockItemOut.PurDollarNameTw,
                            // PurDollarNameTw = materialStockItemOut.PurDollarNameTw,
                            // ExchangeRate = 1,
                        });
                    }
                    else
                    {

                        _materialStockOut.WarehouseNo = materialStockItemOut.WarehouseNo;
                        _materialStockOut.PurUnitCodeId = materialStockItemOut.PurUnitCodeId;
                        _materialStockOut.PurUnitNameTw = materialStockItemOut.PurUnit;
                        _materialStockOut.PurUnitNameEn = materialStockItemOut.PurUnit;

                        _materialStockOut.PCLUnitCodeId = materialStockItemOut.PurUnitCodeId;
                        _materialStockOut.PCLUnitNameTw = materialStockItemOut.PurUnit;
                        _materialStockOut.PCLUnitNameEn = materialStockItemOut.PurUnit;


                        _materialStockOut = MaterialStock.Update(_materialStockOut);
                    }

                    //出庫的匯率＝1, 採購幣別、庫存幣別都等於MateriaStock的幣別。用平均單價
                    var _materialStockItem = MaterialStockItem.Get().Where(i => i.Id == materialStockItemOut.Id && i.LocaleId == materialStockItemOut.LocaleId).FirstOrDefault();
                    if (_materialStockItem != null)
                    {
                        // MaterialStockItem
                        materialStockItemOut.Id = _materialStockItem.Id;
                        materialStockItemOut.LocaleId = _materialStockItem.LocaleId;
                        materialStockItemOut.PurUnitCodeId = _materialStockOut.PurUnitCodeId;

                        materialStockItemOut.PurDollarCodeId = _materialStockOut.StockDollarCodeId;
                        materialStockItemOut.StockDollarCodeId = _materialStockOut.StockDollarCodeId;
                        materialStockItemOut.PurUnitPrice = _purUnitPrice;
                        materialStockItemOut.BankingRate = 1;

                        materialStockItemOut = MaterialStockItem.Update(materialStockItemOut);
                    }
                    else
                    {
                        materialStockItemOut.MaterialStockId = _materialStockOut.Id;
                        materialStockItemOut.LocaleId = _materialStockOut.LocaleId;
                        materialStockItemOut.PurUnitCodeId = _materialStockOut.PurUnitCodeId;

                        materialStockItemOut.PurDollarCodeId = _materialStockOut.StockDollarCodeId;
                        materialStockItemOut.StockDollarCodeId = _materialStockOut.StockDollarCodeId;
                        materialStockItemOut.PurUnitPrice = _purUnitPrice;
                        materialStockItemOut.BankingRate = 1;

                        materialStockItemOut = MaterialStockItem.Create(materialStockItemOut);

                        //更新回去StockIO的單號,
                        materialStockItemOut.RefNo = "IO" + materialStockItemOut.LocaleId.ToString() + DateTime.Now.ToString("yyyyMMdd") + "-" + materialStockItemOut.Id;
                        MaterialStockItem.UpdatRefNo((int)materialStockItemOut.Id, (int)materialStockItemOut.LocaleId, materialStockItemOut.RefNo);
                    }

                    //處理 StockSize
                    //刪除所有的size
                    MaterialStockItemSize.RemoveRange(i => i.StockIOId == materialStockItemOut.Id && i.LocaleId == materialStockItemOut.LocaleId);
                    // 有size的就新增
                    if (materialStockItemSize.Count() > 0)
                    {
                        if (materialStockItemOut.Id != 0)
                        {
                            materialStockItemSize.ForEach(i =>
                            {
                                i.StockIOId = materialStockItemOut.Id;
                                i.LocaleId = materialStockItemOut.LocaleId;
                                i.PCLQty = i.PurQty;
                                i.ShoeSizeSuffix = i.ShoeSizeSuffix == null ? "" : i.ShoeSizeSuffix;
                                i.ShoeInnerSize = i.ShoeInnerSize == 0 ? (i.ShoeSizeSuffix == "J" ? (double)i.ShoeSize : (double)i.ShoeSize * 1000) : i.ShoeInnerSize;
                            });
                            MaterialStockItemSize.CreateRange(materialStockItemSize);
                        }
                    }
                    // 更新總庫存
                    MaterialStockSize.UpdateSizeQty((int)_materialStockOut.Id, (int)_materialStockOut.LocaleId);
                    // 更新庫存數MaterialStock
                    MaterialStock.UpdatStockQty((int)_materialStockOut.Id, (int)_materialStockOut.LocaleId);
                    // 更新收貨的庫存數
                    if (materialStockItemOut.ReceivedLogId != null && materialStockItemOut.ReceivedLogId != 0)
                    {
                        ReceivedLog.UpdateStockQty(new List<decimal> { (decimal)materialStockItemOut.ReceivedLogId }, materialStockItemOut.LocaleId);
                    }
                }
                //處理入庫
                {
                    var _stockInCurrency = "USD";
                    var _stockInexchangeRate = (decimal)0;
                    
                    {
                        //資料重整，舊系統的有很多欄位不用，新系統要合併先把資料重整
                        materialStockItemIn.WarehouseNo = _warehosue.Where(i => i.Id == materialStockItemIn.WarehouseId).Max(i => i.WarehouseNo);   //  抓入庫資料
                        materialStockItemIn.PCLIOQty = 0 - materialStockItemOut.PurIOQty;   // 抓出庫資料的正數
                        materialStockItemIn.PurIOQty = 0 - materialStockItemOut.PurIOQty;   // 抓出庫資料的正數
                        materialStockItemIn.PCLUnitNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.PCLUnitCodeId).Max(i => i.NameTW);
                        materialStockItemIn.PurUnit = _codeItems.Where(i => i.Id == materialStockItemOut.PurUnitCodeId).Max(i => i.NameTW);
                        materialStockItemIn.PurDollarNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.StockDollarCodeId).Max(i => i.NameTW);

                        materialStockItemIn.RefNo = materialStockItemOut.RefNo;
                        materialStockItemIn.RefUserName = materialStockItemOut.RefUserName;
                    }
                    
                    if (_materialStockIn == null)
                    {
                        _materialStockIn = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                        {
                            LocaleId = materialStockItemOut.LocaleId,
                            MaterialId = materialStockItemOut.MaterialId,
                            MaterialName = materialStockItemOut.MaterialName,
                            MaterialNameEng = materialStockItemOut.MaterialNameEng,
                            WarehouseId = materialStockItemIn.WarehouseId,
                            WarehouseNo = materialStockItemIn.WarehouseNo,
                            OrderNo = materialStockItemIn.OrderNo,
                            PCLUnitCodeId = materialStockItemIn.PCLUnitCodeId,
                            PCLUnitNameTw = materialStockItemIn.PCLUnitNameTw,
                            PCLUnitNameEn = materialStockItemIn.PCLUnitNameTw,
                            TransRate = 1,
                            PurUnitCodeId = materialStockItemIn.PurUnitCodeId,
                            PurUnitNameTw = materialStockItemIn.PurUnit,
                            PurUnitNameEn = materialStockItemIn.PurUnit,
                            PCLPlanQty = materialStockItemIn.PCLIOQty,
                            PCLQty = materialStockItemIn.PCLIOQty,
                            PurQty = materialStockItemIn.PCLIOQty,
                            PCLAllocationQty = 0,
                            PurAllocationQty = 0,
                            Amount = (materialStockItemIn.PurIOQty) * materialStockItemIn.PurUnitPrice,
                            StockDollarCodeId = _codeItems.Where(i => i.NameTW == _stockInCurrency).Max(i => i.Id),
                            StockDollarNameTw = _stockInCurrency, //materialStockItem.StockCurrency,
                            StockDollarNameEn = _stockInCurrency, //materialStockItem.StockCurrency,
                            ParentMaterialId = 0,
                            ParentMaterialNameTw = "",
                            ParentMaterialNameEn = "",
                            LastStockIOId = 0,
                            ModifyUserName = materialStockItemOut.ModifyUserName,
                            PurUnitPrice = materialStockItemOut.PurUnitPrice,
                            // PurDollarCodeId = materialStockItemIn.PurDollarCodeId,
                            // PurDollarNameEn = materialStockItemIn.PurDollarNameTw,
                            // PurDollarNameTw = materialStockItemIn.PurDollarNameTw,
                            // ExchangeRate = 1,
                        });
                    }
                    else
                    {
                        _materialStockIn.WarehouseNo = materialStockItemIn.WarehouseNo;
                        _materialStockIn.PurUnitCodeId = materialStockItemIn.PurUnitCodeId;
                        _materialStockIn.PurUnitNameTw = materialStockItemIn.PurUnit;
                        _materialStockIn.PurUnitNameEn = materialStockItemIn.PurUnit;

                        _materialStockIn.PCLUnitCodeId = materialStockItemIn.PurUnitCodeId;
                        _materialStockIn.PCLUnitNameTw = materialStockItemIn.PurUnit;
                        _materialStockIn.PCLUnitNameEn = materialStockItemIn.PurUnit;

                        // _materialStockIn.PurDollarCodeId = materialStockItemIn.PurDollarCodeId;
                        // _materialStockIn.PurDollarNameEn = materialStockItemIn.PurDollarNameTw;
                        // _materialStockIn.PurDollarNameTw = materialStockItemIn.PurDollarNameTw;

                        _materialStockIn = MaterialStock.Update(_materialStockIn);
                    }
                    _stockInCurrency = _materialStockIn.StockDollarNameTw;
                    materialStockItemIn.StockDollarCodeId = _materialStockIn.StockDollarCodeId;
                    materialStockItemIn.PurDollarCodeId = _materialStockOut.StockDollarCodeId; //轉入的採購金額應該為轉出的庫存。由此來轉換匯率

                    //在這裡要重取庫存幣別轉換，因為有可能轉入的庫存幣別跟轉出的不同
                    _stockInexchangeRate = materialStockItemIn.PurDollarNameTw == _stockInCurrency ? 1 : _stockInCurrency == "USD" ?
                                            exchangeItems.Where(i => i.CurrencyTw == _stockInCurrency && i.TransCurrencyTw == materialStockItemIn.PurDollarNameTw).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault() :
                                            exchangeItems.Where(i => i.CurrencyTw == materialStockItemIn.PurDollarNameTw && i.TransCurrencyTw == _stockInCurrency).OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();

                    var _materialStockItem = MaterialStockItem.Get().Where(i => i.Id == materialStockItemIn.Id && i.LocaleId == materialStockItemIn.LocaleId).FirstOrDefault();
                    if (_materialStockItem != null)
                    {
                        // MaterialStockItem
                        materialStockItemIn.Id = _materialStockItem.Id;
                        materialStockItemIn.LocaleId = _materialStockItem.LocaleId;
                        materialStockItemIn.PurUnitCodeId = _materialStockIn.PurUnitCodeId;

                        // materialStockItemIn.PurDollarCodeId = _materialStock.StockDollarCodeId;
                        // materialStockItemIn.StockDollarCodeId = _materialStockIn.StockDollarCodeId;
                        materialStockItemIn.PurUnitPrice = _purUnitPrice;
                        materialStockItemIn.BankingRate = _stockInexchangeRate;

                        materialStockItemIn = MaterialStockItem.Update(materialStockItemIn);
                    }
                    else
                    {
                        materialStockItemIn.MaterialStockId = _materialStockIn.Id;
                        materialStockItemIn.LocaleId = _materialStockIn.LocaleId;
                        materialStockItemIn.PurUnitCodeId = _materialStockIn.PurUnitCodeId;

                        // materialStockItemIn.PurDollarCodeId = _materialStockIn.StockDollarCodeId;
                        // materialStockItemIn.StockDollarCodeId = _materialStockIn.StockDollarCodeId;
                        materialStockItemIn.PurUnitPrice = _purUnitPrice;
                        materialStockItemIn.BankingRate = _stockInexchangeRate;

                        materialStockItemIn = MaterialStockItem.Create(materialStockItemIn);
                    }

                    //處理 StockSize
                    //刪除所有的size
                    MaterialStockItemSize.RemoveRange(i => i.StockIOId == materialStockItemIn.Id && i.LocaleId == materialStockItemIn.LocaleId);
                    // 有size的就新增
                    if (materialStockItemSize.Count() > 0)
                    {
                        if (materialStockItemIn.Id != 0)
                        {
                            materialStockItemSize.ForEach(i =>
                            {
                                i.StockIOId = materialStockItemIn.Id;
                                i.LocaleId = materialStockItemIn.LocaleId;
                                i.PCLQty = i.PurQty;
                                i.ShoeSizeSuffix = i.ShoeSizeSuffix == null ? "" : i.ShoeSizeSuffix;
                                i.ShoeInnerSize = i.ShoeInnerSize == 0 ? (i.ShoeSizeSuffix == "J" ? (double)i.ShoeSize : (double)i.ShoeSize * 1000) : i.ShoeInnerSize;
                            });
                            MaterialStockItemSize.CreateRange(materialStockItemSize);
                        }
                    }
                    // 更新總庫存
                    MaterialStockSize.UpdateSizeQty((int)_materialStockIn.Id, (int)_materialStockIn.LocaleId);
                    // 更新庫存數MaterialStock
                    MaterialStock.UpdatStockQty((int)_materialStockIn.Id, (int)_materialStockIn.LocaleId);
                    // 更新收貨的庫存數
                    if (materialStockItemIn.ReceivedLogId != null && materialStockItemIn.ReceivedLogId != 0)
                    {
                        ReceivedLog.UpdateStockQty(new List<decimal> { (decimal)materialStockItemIn.ReceivedLogId }, materialStockItemIn.LocaleId);
                    }
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return GetMaterialStockTransferItemGroup((int)materialStockItemOut.Id, (int)materialStockItemOut.LocaleId);
        }
        public Models.Views.MaterialStockTransferItemGroup SaveMaterialStockTransferItemGroup1(ERP.Models.Views.MaterialStockTransferItemGroup item)
        {
            var materialStockItemOut = item.MaterialStockItemOut;
            var materialStockItemIn = item.MaterialStockItemIn;
            var materialStockItemSize = item.MaterialStockItemSize.Where(i => i.PurQty != 0).ToList();   //排出出庫數為0

            try
            {
                UnitOfWork.BeginTransaction();
                {
                    var _materialStockOut = materialStockItemOut.MaterialStockId != 0 ? MaterialStock.Get().Where(i => i.Id == materialStockItemOut.MaterialStockId && i.LocaleId == materialStockItemOut.LocaleId).FirstOrDefault() :
                                            MaterialStock.Get().Where(i => i.OrderNo == materialStockItemOut.OrderNo && i.LocaleId == materialStockItemOut.LocaleId &&
                                                                    i.MaterialId == materialStockItemOut.MaterialId && i.ParentMaterialId == 0 &&
                                                                    i.WarehouseId == materialStockItemOut.WarehouseId && i.PCLUnitCodeId == materialStockItemOut.PurUnitCodeId).FirstOrDefault();

                    var _codeTypes = new List<string> { "02", "21" };
                    var _codeItems = CodeItem.Get().Where(i => i.LocaleId == materialStockItemOut.LocaleId && _codeTypes.Contains(i.CodeType)).ToList();
                    var _warehosue = Warehouse.Get().Where(i => i.LocaleId == materialStockItemOut.LocaleId).ToList();

                    var _avgUnitPrice = materialStockItemOut.PurUnitPrice;  // 其出要等於轉出的單價，轉入的單價＝轉出。統一用_avgUnitPrice來設置

                    {
                        //資料重整，舊系統的有很多欄位不用，新系統要合併先把資料重整
                        materialStockItemOut.WarehouseNo = _warehosue.Where(i => i.Id == materialStockItemOut.WarehouseId).Max(i => i.WarehouseNo);
                        materialStockItemOut.PCLIOQty = materialStockItemOut.PurIOQty;
                        materialStockItemOut.PCLUnitNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.PCLUnitCodeId).Max(i => i.NameTW);
                        materialStockItemOut.PurUnit = _codeItems.Where(i => i.Id == materialStockItemOut.PurUnitCodeId).Max(i => i.NameTW);
                        materialStockItemOut.PurDollarNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.StockDollarCodeId).Max(i => i.NameTW);

                        //每次儲存都先更新單價
                        materialStockItemOut.PurUnitPrice = (decimal)_avgUnitPrice;
                        materialStockItemIn.PurUnitPrice = (decimal)_avgUnitPrice;

                        materialStockItemOut.Remark = materialStockItemOut.SourceType == 14 ? "轉批次庫存-" + materialStockItemIn.OrderNo : materialStockItemOut.SourceType == 13 ? "轉可用庫存" : materialStockItemOut.SourceType == 15 ? "批次庫存互轉-" + materialStockItemIn.OrderNo : "";
                        materialStockItemIn.Remark = materialStockItemIn.SourceType == 14 ? "轉批次庫存" : materialStockItemIn.SourceType == 13 ? "轉可用庫存-" + materialStockItemOut.OrderNo : materialStockItemIn.SourceType == 15 ? "批次庫存互轉-" + materialStockItemOut.OrderNo : "";
                    }

                    if (_materialStockOut == null)
                    {
                        _materialStockOut = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                        {
                            LocaleId = materialStockItemOut.LocaleId,
                            MaterialId = materialStockItemOut.MaterialId,
                            MaterialName = materialStockItemOut.MaterialName,
                            MaterialNameEng = materialStockItemOut.MaterialNameEng,
                            WarehouseId = materialStockItemOut.WarehouseId,
                            WarehouseNo = materialStockItemOut.WarehouseNo,
                            OrderNo = materialStockItemOut.OrderNo,
                            PCLUnitCodeId = materialStockItemOut.PCLUnitCodeId,
                            PCLUnitNameTw = materialStockItemOut.PCLUnitNameTw,
                            PCLUnitNameEn = materialStockItemOut.PCLUnitNameTw,
                            TransRate = 1,
                            PurUnitCodeId = materialStockItemOut.PurUnitCodeId,
                            PurUnitNameTw = materialStockItemOut.PurUnit,
                            PurUnitNameEn = materialStockItemOut.PurUnit,
                            PCLPlanQty = materialStockItemOut.PCLIOQty,
                            PCLQty = materialStockItemOut.PCLIOQty,
                            PurQty = materialStockItemOut.PCLIOQty,
                            PCLAllocationQty = 0,
                            PurAllocationQty = 0,
                            Amount = (materialStockItemOut.PurIOQty) * materialStockItemOut.PurUnitPrice,
                            StockDollarCodeId = (decimal)materialStockItemOut.StockDollarCodeId,
                            StockDollarNameTw = materialStockItemOut.PurDollarNameTw,
                            StockDollarNameEn = materialStockItemOut.PurDollarNameTw,
                            ParentMaterialId = 0,
                            ParentMaterialNameTw = "",
                            ParentMaterialNameEn = "",
                            LastStockIOId = 0,
                            ModifyUserName = materialStockItemOut.ModifyUserName,
                            PurUnitPrice = materialStockItemOut.PurUnitPrice,
                            // PurDollarCodeId = materialStockItemOut.PurDollarCodeId,
                            // PurDollarNameEn = materialStockItemOut.PurDollarNameTw,
                            // PurDollarNameTw = materialStockItemOut.PurDollarNameTw,
                            // ExchangeRate = 1,
                        });
                    }
                    else
                    {
                        // _avgUnitPrice = (decimal)_materialStockOut.AvgUnitPrice; // 現不做轉出單價=庫存單價，維持

                        _materialStockOut.WarehouseNo = materialStockItemOut.WarehouseNo;
                        _materialStockOut.PurUnitCodeId = materialStockItemOut.PurUnitCodeId;
                        _materialStockOut.PurUnitNameTw = materialStockItemOut.PurUnit;
                        _materialStockOut.PurUnitNameEn = materialStockItemOut.PurUnit;

                        _materialStockOut.PCLUnitCodeId = materialStockItemOut.PurUnitCodeId;
                        _materialStockOut.PCLUnitNameTw = materialStockItemOut.PurUnit;
                        _materialStockOut.PCLUnitNameEn = materialStockItemOut.PurUnit;

                        // _materialStockOut.PurDollarCodeId = materialStockItemOut.PurDollarCodeId;
                        // _materialStockOut.PurDollarNameEn = materialStockItemOut.PurDollarNameTw;
                        // _materialStockOut.PurDollarNameTw = materialStockItemOut.PurDollarNameTw;

                        _materialStockOut = MaterialStock.Update(_materialStockOut);
                    }


                    var _materialStockItem = MaterialStockItem.Get().Where(i => i.Id == materialStockItemOut.Id && i.LocaleId == materialStockItemOut.LocaleId).FirstOrDefault();
                    if (_materialStockItem != null)
                    {
                        // MaterialStockItem
                        materialStockItemOut.Id = _materialStockItem.Id;
                        materialStockItemOut.LocaleId = _materialStockItem.LocaleId;
                        materialStockItemOut.StockDollarCodeId = _materialStockOut.StockDollarCodeId;
                        materialStockItemOut.PurUnitCodeId = _materialStockOut.PurUnitCodeId;

                        materialStockItemOut = MaterialStockItem.Update(materialStockItemOut);
                    }
                    else
                    {
                        materialStockItemOut.MaterialStockId = _materialStockOut.Id;
                        materialStockItemOut.StockDollarCodeId = _materialStockOut.StockDollarCodeId;
                        materialStockItemOut.PurUnitCodeId = _materialStockOut.PurUnitCodeId;

                        materialStockItemOut = MaterialStockItem.Create(materialStockItemOut);

                        //更新回去StockIO的單號,
                        materialStockItemOut.RefNo = "IO" + materialStockItemOut.LocaleId.ToString() + DateTime.Now.ToString("yyyyMMdd") + "-" + materialStockItemOut.Id;
                        MaterialStockItem.UpdatRefNo((int)materialStockItemOut.Id, (int)materialStockItemOut.LocaleId, materialStockItemOut.RefNo);
                    }

                    //處理 StockSize
                    //刪除所有的size
                    MaterialStockItemSize.RemoveRange(i => i.StockIOId == materialStockItemOut.Id && i.LocaleId == materialStockItemOut.LocaleId);
                    // 有size的就新增
                    if (materialStockItemSize.Count() > 0)
                    {
                        if (materialStockItemOut.Id != 0)
                        {
                            materialStockItemSize.ForEach(i =>
                            {
                                i.StockIOId = materialStockItemOut.Id;
                                i.LocaleId = materialStockItemOut.LocaleId;
                                i.PCLQty = i.PurQty;
                                i.ShoeSizeSuffix = i.ShoeSizeSuffix == null ? "" : i.ShoeSizeSuffix;
                                i.ShoeInnerSize = i.ShoeInnerSize == 0 ? (i.ShoeSizeSuffix == "J" ? (double)i.ShoeSize : (double)i.ShoeSize * 1000) : i.ShoeInnerSize;
                            });
                            MaterialStockItemSize.CreateRange(materialStockItemSize);
                        }
                    }
                    // 更新總庫存
                    MaterialStockSize.UpdateSizeQty((int)_materialStockOut.Id, (int)_materialStockOut.LocaleId);
                    // 更新庫存數MaterialStock
                    MaterialStock.UpdatStockQty((int)_materialStockOut.Id, (int)_materialStockOut.LocaleId);
                    // 更新收貨的庫存數
                    if (materialStockItemOut.ReceivedLogId != null && materialStockItemOut.ReceivedLogId != 0)
                    {
                        ReceivedLog.UpdateStockQty(new List<decimal> { (decimal)materialStockItemOut.ReceivedLogId }, materialStockItemOut.LocaleId);
                    }
                }
                {
                    var _materialStockIn = materialStockItemIn.MaterialStockId != 0 ? MaterialStock.Get().Where(i => i.Id == materialStockItemIn.MaterialStockId && i.LocaleId == materialStockItemIn.LocaleId).FirstOrDefault() :
                        MaterialStock.Get().Where(i => i.OrderNo == materialStockItemIn.OrderNo && i.LocaleId == materialStockItemIn.LocaleId &&
                                                i.MaterialId == materialStockItemIn.MaterialId && i.ParentMaterialId == 0 &&
                                                i.WarehouseId == materialStockItemIn.WarehouseId && i.PCLUnitCodeId == materialStockItemIn.PurUnitCodeId).FirstOrDefault();

                    var _codeTypes = new List<string> { "02", "21" };
                    var _codeItems = CodeItem.Get().Where(i => i.LocaleId == materialStockItemIn.LocaleId && _codeTypes.Contains(i.CodeType)).ToList();
                    var _warehosue = Warehouse.Get().Where(i => i.LocaleId == materialStockItemIn.LocaleId).ToList();

                    {
                        //資料重整，舊系統的有很多欄位不用，新系統要合併先把資料重整
                        materialStockItemIn.WarehouseNo = _warehosue.Where(i => i.Id == materialStockItemIn.WarehouseId).Max(i => i.WarehouseNo);   //  抓入庫資料
                        materialStockItemIn.PCLIOQty = 0 - materialStockItemOut.PurIOQty;   // 抓出庫資料的正數
                        materialStockItemIn.PurIOQty = 0 - materialStockItemOut.PurIOQty;   // 抓出庫資料的正數
                        materialStockItemIn.PCLUnitNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.PCLUnitCodeId).Max(i => i.NameTW);
                        materialStockItemIn.PurUnit = _codeItems.Where(i => i.Id == materialStockItemOut.PurUnitCodeId).Max(i => i.NameTW);
                        materialStockItemIn.PurDollarNameTw = _codeItems.Where(i => i.Id == materialStockItemOut.StockDollarCodeId).Max(i => i.NameTW);

                        materialStockItemIn.RefNo = materialStockItemOut.RefNo;
                        materialStockItemIn.RefUserName = materialStockItemOut.RefUserName;
                    }

                    if (_materialStockIn == null)
                    {
                        _materialStockIn = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                        {
                            LocaleId = materialStockItemOut.LocaleId,
                            MaterialId = materialStockItemOut.MaterialId,
                            MaterialName = materialStockItemOut.MaterialName,
                            MaterialNameEng = materialStockItemOut.MaterialNameEng,
                            WarehouseId = materialStockItemIn.WarehouseId,
                            WarehouseNo = materialStockItemIn.WarehouseNo,
                            OrderNo = materialStockItemIn.OrderNo,
                            PCLUnitCodeId = materialStockItemIn.PCLUnitCodeId,
                            PCLUnitNameTw = materialStockItemIn.PCLUnitNameTw,
                            PCLUnitNameEn = materialStockItemIn.PCLUnitNameTw,
                            TransRate = 1,
                            PurUnitCodeId = materialStockItemIn.PurUnitCodeId,
                            PurUnitNameTw = materialStockItemIn.PurUnit,
                            PurUnitNameEn = materialStockItemIn.PurUnit,
                            PCLPlanQty = materialStockItemIn.PCLIOQty,
                            PCLQty = materialStockItemIn.PCLIOQty,
                            PurQty = materialStockItemIn.PCLIOQty,
                            PCLAllocationQty = 0,
                            PurAllocationQty = 0,
                            Amount = (materialStockItemIn.PurIOQty) * materialStockItemIn.PurUnitPrice,
                            StockDollarCodeId = (decimal)materialStockItemIn.StockDollarCodeId,
                            StockDollarNameTw = materialStockItemIn.PurDollarNameTw,
                            StockDollarNameEn = materialStockItemIn.PurDollarNameTw,
                            ParentMaterialId = 0,
                            ParentMaterialNameTw = "",
                            ParentMaterialNameEn = "",
                            LastStockIOId = 0,
                            ModifyUserName = materialStockItemOut.ModifyUserName,
                            PurUnitPrice = materialStockItemOut.PurUnitPrice,
                            // PurDollarCodeId = materialStockItemIn.PurDollarCodeId,
                            // PurDollarNameEn = materialStockItemIn.PurDollarNameTw,
                            // PurDollarNameTw = materialStockItemIn.PurDollarNameTw,
                            // ExchangeRate = 1,
                        });
                    }
                    else
                    {
                        _materialStockIn.WarehouseNo = materialStockItemIn.WarehouseNo;
                        _materialStockIn.PurUnitCodeId = materialStockItemIn.PurUnitCodeId;
                        _materialStockIn.PurUnitNameTw = materialStockItemIn.PurUnit;
                        _materialStockIn.PurUnitNameEn = materialStockItemIn.PurUnit;

                        _materialStockIn.PCLUnitCodeId = materialStockItemIn.PurUnitCodeId;
                        _materialStockIn.PCLUnitNameTw = materialStockItemIn.PurUnit;
                        _materialStockIn.PCLUnitNameEn = materialStockItemIn.PurUnit;

                        // _materialStockIn.PurDollarCodeId = materialStockItemIn.PurDollarCodeId;
                        // _materialStockIn.PurDollarNameEn = materialStockItemIn.PurDollarNameTw;
                        // _materialStockIn.PurDollarNameTw = materialStockItemIn.PurDollarNameTw;

                        _materialStockIn = MaterialStock.Update(_materialStockIn);
                    }


                    var _materialStockItem = MaterialStockItem.Get().Where(i => i.Id == materialStockItemIn.Id && i.LocaleId == materialStockItemIn.LocaleId).FirstOrDefault();
                    if (_materialStockItem != null)
                    {
                        // MaterialStockItem
                        materialStockItemIn.Id = _materialStockItem.Id;
                        materialStockItemIn.LocaleId = _materialStockItem.LocaleId;
                        materialStockItemIn.StockDollarCodeId = _materialStockIn.StockDollarCodeId;
                        materialStockItemIn.PurUnitCodeId = _materialStockIn.PurUnitCodeId;

                        materialStockItemIn = MaterialStockItem.Update(materialStockItemIn);
                    }
                    else
                    {
                        materialStockItemIn.MaterialStockId = _materialStockIn.Id;
                        materialStockItemIn.StockDollarCodeId = _materialStockIn.StockDollarCodeId;
                        materialStockItemIn.PurUnitCodeId = _materialStockIn.PurUnitCodeId;

                        materialStockItemIn = MaterialStockItem.Create(materialStockItemIn);
                    }

                    //處理 StockSize
                    //刪除所有的size
                    MaterialStockItemSize.RemoveRange(i => i.StockIOId == materialStockItemIn.Id && i.LocaleId == materialStockItemIn.LocaleId);
                    // 有size的就新增
                    if (materialStockItemSize.Count() > 0)
                    {
                        if (materialStockItemIn.Id != 0)
                        {
                            materialStockItemSize.ForEach(i =>
                            {
                                i.StockIOId = materialStockItemIn.Id;
                                i.LocaleId = materialStockItemIn.LocaleId;
                                i.PCLQty = i.PurQty;
                                i.ShoeSizeSuffix = i.ShoeSizeSuffix == null ? "" : i.ShoeSizeSuffix;
                                i.ShoeInnerSize = i.ShoeInnerSize == 0 ? (i.ShoeSizeSuffix == "J" ? (double)i.ShoeSize : (double)i.ShoeSize * 1000) : i.ShoeInnerSize;
                            });
                            MaterialStockItemSize.CreateRange(materialStockItemSize);
                        }
                    }
                    // 更新總庫存
                    MaterialStockSize.UpdateSizeQty((int)_materialStockIn.Id, (int)_materialStockIn.LocaleId);
                    // 更新庫存數MaterialStock
                    MaterialStock.UpdatStockQty((int)_materialStockIn.Id, (int)_materialStockIn.LocaleId);
                    // 更新收貨的庫存數
                    if (materialStockItemIn.ReceivedLogId != null && materialStockItemIn.ReceivedLogId != 0)
                    {
                        ReceivedLog.UpdateStockQty(new List<decimal> { (decimal)materialStockItemIn.ReceivedLogId }, materialStockItemIn.LocaleId);
                    }
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return GetMaterialStockTransferItemGroup((int)materialStockItemOut.Id, (int)materialStockItemOut.LocaleId);
        }

        public void RemoveMaterialStockTransferItemGroup(ERP.Models.Views.MaterialStockTransferItemGroup item)
        {
            UnitOfWork.BeginTransaction();
            try
            {

                MaterialStockItemSize.RemoveRange(i => i.StockIOId == item.MaterialStockItemIn.Id && i.LocaleId == item.MaterialStockItemIn.LocaleId);
                MaterialStockItemSize.RemoveRange(i => i.StockIOId == item.MaterialStockItemOut.Id && i.LocaleId == item.MaterialStockItemOut.LocaleId);
                MaterialStockItem.RemoveRange(i => i.Id == item.MaterialStockItemIn.Id && i.LocaleId == item.MaterialStockItemIn.LocaleId);
                MaterialStockItem.RemoveRange(i => i.Id == item.MaterialStockItemOut.Id && i.LocaleId == item.MaterialStockItemOut.LocaleId);

                UpdateMaterialStockInfo((int)item.MaterialStockItemIn.MaterialStockId, (int)item.MaterialStockItemIn.LocaleId);
                UpdateMaterialStockInfo((int)item.MaterialStockItemOut.MaterialStockId, (int)item.MaterialStockItemOut.LocaleId);

                if (item.MaterialStockItemIn.ReceivedLogId != 0)
                {
                    UpdateReceivedLogStockQty(new List<decimal> { (decimal)item.MaterialStockItemIn.ReceivedLogId }, (int)item.MaterialStockItemIn.LocaleId);
                }

                if (item.MaterialStockItemOut.ReceivedLogId != 0)
                {
                    UpdateReceivedLogStockQty(new List<decimal> { (decimal)item.MaterialStockItemOut.ReceivedLogId }, (int)item.MaterialStockItemOut.LocaleId);
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        // 出庫
        // 1. 領出幣別、單位，要跟著MaterialStock 的資料
        // 2. 舊系統要能正常運作，所以在前庫存有塞資料(等於當時的庫存)，如果庫存的資料有曾經異動，可能會跟最新庫存不同
        // 3. 全部轉換到新系統後，要重新跟新相關的庫存幣別
        public Models.Views.MaterialStockTransferItemGroup GetMaterialStockTransferItemGroupFromOutImport(int materialStockId, int localeId, int mpsDailyId, int mpsLocaleId, int sourceType)
        {
            var materialStock = MaterialStock.Get().Where(i => i.Id == materialStockId && i.LocaleId == localeId).FirstOrDefault();
            var group = new Models.Views.MaterialStockTransferItemGroup { };
            if (materialStock != null)
            {
                group.MaterialStockItemOut = new Models.Views.MaterialStockItem
                {
                    Id = 0,
                    LocaleId = materialStock.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = sourceType,
                    MaterialId = materialStock.MaterialId,
                    WarehouseId = materialStock.WarehouseId,
                    OrderNo = sourceType == 13 ? materialStock.OrderNo : sourceType == 14 ? "" : sourceType == 15 ? materialStock.OrderNo : "",
                    PCLUnitCodeId = materialStock.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = materialStock.PurUnitCodeId,

                    ReceivedLogId = 0,
                    // PurUnitPrice = (materialStock.Amount == 0 || materialStock.PCLQty == 0) ? 0 : materialStock.Amount / materialStock.PCLQty,
                    PurUnitPrice = (decimal)materialStock.AvgUnitPrice,
                    BankingRate = 1,

                    PurDollarCodeId = materialStock.StockDollarCodeId,
                    StockDollarCodeId = materialStock.StockDollarCodeId,
                    Remark = sourceType == 14 ? "轉批次庫存" : sourceType == 13 ? "轉可用庫存" : sourceType == 15 ? "批次庫存互轉" : "",
                    RefNo = "",
                    StockQty = materialStock.PCLQty == null ? 0 : materialStock.PCLQty,    //庫存數
                    UsageQty = materialStock.RealStockOutQty == null ? 0 : materialStock.RealStockOutQty,   //領用數
                    MPSQty = 0, // 派工數量
                    PCLIOQty = 0 - materialStock.PCLQty,
                    PurIOQty = 0 - materialStock.PCLQty,
                    // OrgUnitId = mpsDaily != null ? mpsDaily.OrgUnitId : 0,
                    // OrgUnitNameTw = mpsDaily != null ? mpsDaily.OrgUnitNameTw : "",
                    // OrgUnitNameEn = mpsDaily != null ? mpsDaily.OrgUnitNameEn : "",

                    OrgUnitId = 0,
                    OrgUnitNameTw = "",
                    OrgUnitNameEn = "",

                    MPSProcessId = 0,
                    MPSProcessNameTw = "",
                    MPSProcessNameEn = "",

                    RefUserName = "",
                    MaterialStockId = materialStock.Id,
                    PrePCLQty = materialStock.PCLQty,
                    PreAmount = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,

                    MaterialName = materialStock.MaterialName,
                    MaterialNameEng = materialStock.MaterialNameEng,
                    WarehouseNo = materialStock.WarehouseNo,
                };
                group.MaterialStockItemIn = new Models.Views.MaterialStockItem
                {
                    Id = 0,
                    LocaleId = materialStock.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = sourceType,
                    MaterialId = materialStock.MaterialId,
                    WarehouseId = materialStock.WarehouseId,
                    // OrderNo = materialStock.OrderNo,
                    OrderNo = sourceType == 13 ? "" : sourceType == 14 ? materialStock.OrderNo : sourceType == 15 ? "" : "",
                    PCLUnitCodeId = materialStock.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = materialStock.PurUnitCodeId,

                    ReceivedLogId = 0,
                    // PurUnitPrice = (materialStock.Amount == 0 || materialStock.PCLQty == 0) ? 0 : materialStock.Amount / materialStock.PCLQty,
                    PurUnitPrice = (decimal)materialStock.AvgUnitPrice,
                    BankingRate = 1,
                    PurDollarCodeId = materialStock.StockDollarCodeId,
                    StockDollarCodeId = materialStock.StockDollarCodeId,
                    Remark = sourceType == 14 ? "轉批次庫存" : sourceType == 13 ? "轉可用庫存" : sourceType == 15 ? "批次庫存互轉" : "",
                    RefNo = "",

                    StockQty = materialStock.PCLQty == null ? 0 : materialStock.PCLQty,    //庫存數
                    UsageQty = materialStock.RealStockOutQty == null ? 0 : materialStock.RealStockOutQty,   //領用數
                    MPSQty = 0, // 派工數量

                    PCLIOQty = materialStock.PCLQty,
                    PurIOQty = materialStock.PCLQty,
                    // OrgUnitId = mpsDaily != null ? mpsDaily.OrgUnitId : 0,
                    // OrgUnitNameTw = mpsDaily != null ? mpsDaily.OrgUnitNameTw : "",
                    // OrgUnitNameEn = mpsDaily != null ? mpsDaily.OrgUnitNameEn : "",

                    OrgUnitId = 0,
                    OrgUnitNameTw = "",
                    OrgUnitNameEn = "",

                    MPSProcessId = 0,
                    MPSProcessNameTw = "",
                    MPSProcessNameEn = "",

                    RefUserName = "",
                    MaterialStockId = 0,
                    PrePCLQty = 0,
                    PreAmount = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,

                    MaterialName = materialStock.MaterialName,
                    MaterialNameEng = materialStock.MaterialNameEng,
                    WarehouseNo = materialStock.WarehouseNo,
                };

                group.MaterialStockItemSize = MaterialStockSize.Get().Where(i => i.MaterialStockId == materialStockId && i.LocaleId == localeId)
                .Select(i => new Models.Views.MaterialStockItemSize
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    StockIOId = 0,
                    ShoeSize = i.ShoeSize,
                    ShoeSizeSuffix = i.ShoeSizeSuffix,
                    ShoeInnerSize = i.ShoeInnerSize,
                    PCLQty = 0 - i.PCLQty,
                    PurQty = 0 - i.PurQty,
                    ReLogSizeItemId = i.Id,
                    DisplaySize = i.ShoeSize.ToString()
                }).ToList();
            }
            return group;

        }

        // 出庫的材料匯入查詢，資料來自MaterialStock+派工單
        public IQueryable<Models.Views.MaterialStockOut> GetMaterialForStockOut()
        {
            var materialStocks = (
                from ms in MaterialStock.Get()
                select new
                {
                    Id = ms.Id,
                    LocaleId = ms.LocaleId,
                    MaterialId = ms.MaterialId,
                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    WarehouseId = ms.WarehouseId,
                    WarehouseNo = ms.WarehouseNo,
                    OrderNo = ms.OrderNo,
                    PCLUnitCodeId = ms.PCLUnitCodeId,
                    PCLUnitNameTw = ms.PCLUnitNameTw,
                    PCLUnitNameEn = ms.PCLUnitNameEn,
                    PCLQty = ms.PCLQty,
                    ParentMaterialId = ms.ParentMaterialId,
                }
            )
            .Select(i => new Models.Views.MaterialStockOut
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialId = i.MaterialId,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
                WarehouseId = i.WarehouseId,
                WarehouseNo = i.WarehouseNo,
                OrderNo = i.OrderNo,
                PCLUnitCodeId = i.PCLUnitCodeId,
                PCLUnitNameTw = i.PCLUnitNameTw,
                PCLUnitNameEn = i.PCLUnitNameEn,
                PCLQty = i.PCLQty,
                ParentMaterialId = i.ParentMaterialId,
                DailyNo = "",
                MPSDailyId = 0,
                MPSDailyLocaleId = 0,
            });

            return materialStocks;
        }

    }
}
