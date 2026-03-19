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
    public class MaterialStockService : BusinessService
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
        private ERP.Services.Business.Entities.OrgUnitService OrgUnit { get; set; }
        private ERP.Services.Business.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.MPSDailyService MPSDaily { get; set; }

        private ERP.Services.Business.Entities.MaterialStockService _MaterialStock { get; set; }
        private ERP.Services.Entities.MpsDailyService _MPSDaily { get; set; }

        public MaterialStockService(
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
            ERP.Services.Business.Entities.OrgUnitService orgUnitService,

            ERP.Services.Business.Entities.MaterialStockService _materialStockService,
            ERP.Services.Entities.MpsDailyService _mpsDailyService,
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
            OrgUnit = orgUnitService;

            Material = materialService;
            MPSDaily = mpsDailyService;

            _MaterialStock = _materialStockService;
            _MPSDaily = _mpsDailyService;
        }

        //全部出入庫 MaterialStock，
        public Models.Views.MaterialStockGroup GetMaterialStockGroup(int materialStockId, int localeId)
        {
            var stock = MaterialStock.Get().Where(i => i.Id == materialStockId && i.LocaleId == localeId).FirstOrDefault();
            var stockItem = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStockId && i.LocaleId == localeId).OrderBy(i => i.IODate).ToList();
            var stockSize = MaterialStockSize.Get().Where(i => i.MaterialStockId == materialStockId && i.LocaleId == localeId).OrderBy(i => i.ShoeInnerSize).ToList();
            //額外產生序號
            var seqNo = 1;
            stockItem.ForEach(i =>
            {
                i.SeqNo = seqNo;
                seqNo += 1;
            });

            return new Models.Views.MaterialStockGroup
            {
                MaterialStock = stock,
                MaterialStockItem = stockItem,
                MaterialStockSize = stockSize,
            };
        }
        public Models.Views.MaterialStockGroup SaveMaterialStockGroup(ERP.Models.Views.MaterialStockGroup item)
        {
            var materialStock = item.MaterialStock;
            var materialStockItem = item.MaterialStockItem.ToList();
            var _receivedLogIds = materialStockItem.Where(i => i.ReceivedLogId != 0).Select(i => (decimal)i.ReceivedLogId).Distinct().ToList(); //更新收貨的入庫數

            try
            {
                UnitOfWork.BeginTransaction();

                if (materialStock != null)
                {
                    // MaterialStock
                    var _mGroup = GetMaterialStockGroup((int)materialStock.Id, (int)materialStock.LocaleId);
                    var _materialStock = _mGroup.MaterialStock;
                    var _materialStockItem = _mGroup.MaterialStockItem;

                    if (_materialStock != null)
                    {
                        materialStock.Id = _materialStock.Id;
                        materialStock.LocaleId = _materialStock.LocaleId;
                        materialStock.PurQty = materialStockItem.Sum(i => i.PCLIOQty);
                        materialStock.PCLQty = materialStockItem.Sum(i => i.PCLIOQty);

                        materialStock = MaterialStock.Update(materialStock);

                        var _stockIds = materialStockItem.Select(i => i.Id).ToArray();

                        materialStockItem.ForEach(i =>
                        {
                            i.WarehouseId = materialStock.WarehouseId;
                        });

                        MaterialStockItem.RemoveRange(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId && !_stockIds.Contains(i.Id));
                        MaterialStockItem.UpdateRange(materialStockItem);
                    }
                    else
                    {// 理論上用不到，因為MaterialStock的新增來自入庫
                        materialStock = MaterialStock.Create(materialStock);
                        materialStockItem.ForEach(i =>
                        {
                            i.LocaleId = materialStock.LocaleId;
                            i.MaterialStockId = materialStock.Id;
                        });
                        MaterialStockItem.CreateRange(materialStockItem);
                    }

                    // 更新來料收貨的入庫數

                    if (_receivedLogIds.Any())
                    {
                        UpdateReceivedLogStockQty(_receivedLogIds, (int)_materialStock.LocaleId);
                    }
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return GetMaterialStockGroup((int)item.MaterialStock.Id, (int)item.MaterialStock.LocaleId);
        }
        public void Remove(ERP.Models.Views.MaterialStock item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var stockIds = MaterialStockItem.Get().Where(i => i.MaterialStockId == item.Id && i.LocaleId == item.LocaleId).Select(i => i.Id).ToArray();
                var recIds = MaterialStockItem.Get().Where(i => i.MaterialStockId == item.Id && i.LocaleId == item.LocaleId && i.ReceivedLogId != 0).Select(i => (decimal)i.ReceivedLogId).Distinct().ToList();

                MaterialStockItemSize.RemoveRange(i => stockIds.Contains(i.StockIOId) && i.LocaleId == item.LocaleId);
                MaterialStockItem.RemoveRange(i => i.MaterialStockId == item.Id && i.LocaleId == item.LocaleId);
                MaterialStockSize.RemoveRange(i => i.MaterialStockId == item.Id && i.LocaleId == item.LocaleId);
                MaterialStock.Remove(item);


                if (recIds.Any())
                {
                    UpdateReceivedLogStockQty(recIds, (int)item.LocaleId);
                }


                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void RemoveMaterialStockGroup(ERP.Models.Views.MaterialStockGroup item)
        {
            var materialStock = item.MaterialStock;
            Remove(materialStock);
        }

        //每次有異動，MateriaStock都要更新
        public void UpdateMaterialStockInfo(int materialStockId, int localeId)
        {
            MaterialStock.UpdatStockQty(materialStockId, localeId);
            MaterialStockSize.UpdateSizeQty(materialStockId, localeId);
        }
        //更新收貨的入庫數(會影響來料入庫的匯入數)
        public void UpdateReceivedLogStockQty(List<decimal> receivedLogIds, int localeId)
        {
            ReceivedLog.UpdateStockQty(receivedLogIds, localeId);
        }

        //庫存修正
        public IQueryable<Models.Views.MaterialStock> UpdateMaterialStockAmount(List<int> materialStockIds, int localeId)
        {
            materialStockIds.ForEach(i =>
            {
                MaterialStock.UpdatStockQty(i, localeId);
            });

            return MaterialStock.Get().Where(i => materialStockIds.Contains((int)i.Id) && i.LocaleId == localeId);
        }

        public IQueryable<Models.Views.MaterialStock> UpdateMaterialStockQty(List<decimal> materialStockIds, int localeId)
        {
            var mStocks = MaterialStock.Get().Where(i => i.LocaleId == localeId && materialStockIds.Contains(i.Id)).ToList();

            var stockItems = new List<Models.Views.MaterialStockItem>();
            mStocks.ForEach(i =>
            {
                if (i.PCLQty == 0)
                {
                    var item = new Models.Views.MaterialStockItem
                    {
                        Id = 0,
                        LocaleId = i.LocaleId,
                        IODate = DateTime.Today,
                        // SourceType = i.SourceType,
                        MaterialId = i.MaterialId,
                        WarehouseId = i.WarehouseId,
                        OrderNo = i.OrderNo,
                        PCLUnitCodeId = i.PCLUnitCodeId,
                        TransRate = 1,
                        PurUnitCodeId = i.PurUnitCodeId,
                        // PCLIOQty = i.PCLIOQty,
                        PCLIOQty = (0 - i.RealStockQty ?? 0),
                        PurIOQty = 0,
                        // ReceivedLogId = item.ReceivedLogId,
                        PurUnitPrice = 0,
                        PurDollarCodeId = i.StockDollarCodeId,
                        BankingRate = 1,
                        StockDollarCodeId = i.StockDollarCodeId,
                        Remark = "Material Balance",
                        RefNo = "",
                        OrgUnitId = 0,
                        OrgUnitNameTw = "",
                        OrgUnitNameEn = "",
                        MPSProcessId = 0,
                        MPSProcessNameTw = "",
                        MPSProcessNameEn = "",
                        RefUserName = "",
                        MaterialStockId = i.Id,
                        PrePCLQty = 0,
                        PreAmount = 0,
                        ModifyUserName = "SYS_ADMIN",
                        LastUpdateTime = DateTime.Now,
                    };
                    if (i.PCLQty < i.RealStockQty)
                    {
                        item.SourceType = 9;

                    }
                    else if (i.PCLQty > i.RealStockQty)
                    {
                        item.SourceType = 8;
                    }

                    stockItems.Add(item);

                } else if (i.PCLQty > 0 && i.RealStockQty < 0) {

                    var item = new Models.Views.MaterialStockItem {
                        Id = 0,
                        LocaleId = i.LocaleId,
                        IODate = DateTime.Today,
                        // SourceType = i.SourceType,
                        MaterialId = i.MaterialId,
                        WarehouseId = i.WarehouseId,
                        OrderNo = i.OrderNo,
                        PCLUnitCodeId = i.PCLUnitCodeId,
                        TransRate = 1,
                        PurUnitCodeId = i.PurUnitCodeId,
                        // PCLIOQty = i.PCLIOQty,
                        PCLIOQty = (i.PCLQty - i.RealStockQty ?? 0),
                        PurIOQty = 0,
                        // ReceivedLogId = item.ReceivedLogId,
                        PurUnitPrice = 0,
                        PurDollarCodeId = i.StockDollarCodeId,
                        BankingRate = 1,
                        StockDollarCodeId = i.StockDollarCodeId,
                        Remark = "Material Balance",
                        RefNo = "",
                        OrgUnitId = 0,
                        OrgUnitNameTw = "",
                        OrgUnitNameEn = "",
                        MPSProcessId = 0,
                        MPSProcessNameTw = "",
                        MPSProcessNameEn = "",
                        RefUserName = "",
                        MaterialStockId = i.Id,
                        PrePCLQty = 0,
                        PreAmount = 0,
                        ModifyUserName = "SYS_ADMIN",
                        LastUpdateTime = DateTime.Now,
                    };
                    item.SourceType = 8;
                    stockItems.Add(item);
                }
            });

            if (stockItems.Any())
            {
                MaterialStockItem.CreateRange(stockItems);
            }

            return MaterialStock.Get().Where(i => materialStockIds.Contains((int)i.Id) && i.LocaleId == localeId);

        }
        //出入庫 StockIO
        public Models.Views.MaterialStockItemGroup GetMaterialStockItemGroup(int materialStockItemId, int localeId)
        {
            var stockItem = MaterialStockItem.Get().Where(i => i.Id == materialStockItemId && i.LocaleId == localeId).FirstOrDefault();
            var stockItemSize = stockItem != null ? MaterialStockItemSize.Get().Where(i => i.StockIOId == stockItem.Id && i.LocaleId == stockItem.LocaleId).ToList() : null;
            Models.Views.MaterialStockItemPO stockItemPO = new Models.Views.MaterialStockItemPO { };
            List<Models.Views.OutsourcePOItem> poItems = new List<Models.Views.OutsourcePOItem> { };

            if (stockItem != null)
            {
                stockItem.UsageQty = StockIO.Get().Where(s => s.MaterialStockId == stockItem.MaterialStockId && s.LocaleId == stockItem.LocaleId && s.PCLIOQty < 0).Sum(s => s.PCLIOQty);

                //來料出庫才取po
                if (stockItem.SourceType == 5)
                {
                    var poTypes = new List<int?> { 2, 6 };
                    var _outsourcePO = MaterialStockItemPO.Get().Where(i => i.StockIOId == materialStockItemId && i.LocaleId == localeId).FirstOrDefault();

                    stockItemPO = _outsourcePO != null ? _outsourcePO : stockItemPO;
                    poItems = POItem.Get()
                            .Where(i => i.OrderNo != "" && i.OrderNo == stockItem.OrderNo && poTypes.Contains(i.POType) && i.Status != 2)
                            .Select(i => new Models.Views.OutsourcePOItem
                            {
                                Id = i.Id,
                                LocaleId = i.LocaleId,
                                POId = i.POId,
                                MaterialId = i.MaterialId,
                                MaterialName = i.Material,
                                MaterialNameEng = i.MaterialEng,
                                PONo = i.PONo,
                                DisplayItem = i.PONo + "-【" + i.Material + "】",
                                DisplayItemEng = i.PONo + "-【" + i.MaterialEng + "】",
                                Qty = i.Qty,
                            })
                            .OrderBy(i => i.PONo)
                            .ThenBy(i => i.MaterialName)
                            .ToList();
                }
            }

            return new Models.Views.MaterialStockItemGroup
            {
                MaterialStockItem = stockItem,
                MaterialStockItemSize = stockItemSize,
                MaterialStockItemPO = stockItemPO,
                OutsourcePOItem = poItems,
            };
        }
        public Models.Views.MaterialStockItemGroup SaveMaterialStockItemGroup(ERP.Models.Views.MaterialStockItemGroup item)
        {
            var materialStockItem = item.MaterialStockItem;
            var materialStockItemSize = item.MaterialStockItemSize.Where(i => i.PurQty != 0).ToList();   //排出出庫數為0
            var materialStockItemPO = item.MaterialStockItemPO;

            try
            {
                UnitOfWork.BeginTransaction();
                var _materialStock = materialStockItem.MaterialStockId != 0 ? MaterialStock.Get().Where(i => i.Id == materialStockItem.MaterialStockId && i.LocaleId == materialStockItem.LocaleId).FirstOrDefault() :
                                        MaterialStock.Get().Where(i => i.OrderNo == materialStockItem.OrderNo && i.LocaleId == materialStockItem.LocaleId &&
                                                                i.MaterialId == materialStockItem.MaterialId && i.ParentMaterialId == 0 &&
                                                                i.WarehouseId == materialStockItem.WarehouseId && i.PCLUnitCodeId == materialStockItem.PurUnitCodeId).FirstOrDefault();
                var _receivedLog = materialStockItem.ReceivedLogId == 0 ? null : ReceivedLog.Get().Where(i => i.Id == materialStockItem.ReceivedLogId && i.LocaleId == materialStockItem.LocaleId).FirstOrDefault();

                var _codeTypes = new List<string> { "02", "21" };
                var _codeItems = CodeItem.Get().Where(i => i.LocaleId == materialStockItem.LocaleId && _codeTypes.Contains(i.CodeType)).ToList();
                var _warehosue = Warehouse.Get().Where(i => i.LocaleId == materialStockItem.LocaleId).ToList();
                var _stockCurrency = "USD";

                var exchangeRate = (decimal)0;
                // 匯率的建法，只有美金對全部幣別，還有全部幣別對台幣。所以最好的方式是把資料一次都抓出來(最近出庫日的那天所有幣別)。
                var _exchDate = ExchangeRate.Get().Where(i => i.ExchDate <= materialStockItem.IODate).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate);
                var exchangeItems = ExchangeRate.Get().Where(i => i.ExchDate == _exchDate).OrderByDescending(i => i.ExchDate).ToList();
                {
                    //因為拖外的採購單價會被改變，這裡先還原為採購單單價
                    materialStockItem.PurDollarCodeId = _receivedLog != null ? _receivedLog.PurDollarCodeId : materialStockItem.PurDollarCodeId;
                    materialStockItem.PurUnitPrice = _receivedLog != null ? _receivedLog.UnitPrice : materialStockItem.PurUnitPrice;

                    //資料重整，舊系統的有很多欄位不用，新系統要合併先把資料重整
                    materialStockItem.WarehouseNo = _warehosue.Where(i => i.Id == materialStockItem.WarehouseId).Max(i => i.WarehouseNo);
                    materialStockItem.PurIOQty = materialStockItem.Id == 0 ? materialStockItem.PCLIOQty : materialStockItem.PurIOQty;
                    materialStockItem.PCLUnitNameTw = _codeItems.Where(i => i.Id == materialStockItem.PCLUnitCodeId).Max(i => i.NameTW);
                    materialStockItem.PurUnit = _codeItems.Where(i => i.Id == materialStockItem.PurUnitCodeId).Max(i => i.NameTW);
                    materialStockItem.PurDollarNameTw = _codeItems.Where(i => i.Id == materialStockItem.PurDollarCodeId).Max(i => i.NameTW);
                }

                // Step1: 處理MaterialStock ============
                if (_materialStock == null)
                {
                    // 新資料，庫存幣別一律為USD, 所以抓的是UST to Others, 用ReversedBankingRate
                    exchangeRate = materialStockItem.PurDollarNameTw == "USD" ? 1 : exchangeItems.Where(i => i.CurrencyTw == _stockCurrency && i.TransCurrencyTw == materialStockItem.PurDollarNameTw).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault();

                    _materialStock = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                    {
                        LocaleId = materialStockItem.LocaleId,
                        MaterialId = materialStockItem.MaterialId,
                        MaterialName = materialStockItem.MaterialName,
                        MaterialNameEng = materialStockItem.MaterialNameEng,
                        WarehouseId = materialStockItem.WarehouseId,
                        WarehouseNo = materialStockItem.WarehouseNo,
                        OrderNo = materialStockItem.OrderNo,
                        PCLUnitCodeId = materialStockItem.PCLUnitCodeId,
                        PCLUnitNameTw = materialStockItem.PCLUnitNameTw,
                        PCLUnitNameEn = materialStockItem.PCLUnitNameTw,
                        TransRate = 1,
                        PurUnitCodeId = materialStockItem.PurUnitCodeId,
                        PurUnitNameTw = materialStockItem.PurUnit,
                        PurUnitNameEn = materialStockItem.PurUnit,
                        PCLPlanQty = materialStockItem.PCLIOQty,
                        PCLQty = materialStockItem.PCLIOQty,
                        PurQty = materialStockItem.PCLIOQty,
                        PCLAllocationQty = 0,
                        PurAllocationQty = 0,
                        Amount = (materialStockItem.PurIOQty) * materialStockItem.PurUnitPrice,
                        // StockDollarCodeId = (decimal)materialStockItem.StockDollarCodeId,
                        StockDollarCodeId = _codeItems.Where(i => i.NameTW == _stockCurrency).Max(i => i.Id),
                        StockDollarNameTw = _stockCurrency, //materialStockItem.StockCurrency,
                        StockDollarNameEn = _stockCurrency, //materialStockItem.StockCurrency,
                        ParentMaterialId = 0,
                        ParentMaterialNameTw = "",
                        ParentMaterialNameEn = "",
                        LastStockIOId = 0,
                        ModifyUserName = materialStockItem.ModifyUserName,
                        PurUnitPrice = materialStockItem.PurUnitPrice,
                        // PurDollarCodeId = materialStockItem.PurDollarCodeId,
                        // PurDollarNameEn = materialStockItem.PurDollarNameTw,
                        // PurDollarNameTw = materialStockItem.PurDollarNameTw,
                        // ExchangeRate = exchangeRate,
                    });
                }
                else
                {
                    // 舊資料，要判斷庫存幣別，USD, 所以抓的是UST to Others, 用ReversedBankingRate, NTD 要使用 Otehrs to NTD, 用的是BankingRate
                    _stockCurrency = _materialStock.StockDollarNameTw; //materialStockItem.StockCurrency;
                    exchangeRate = materialStockItem.PurDollarNameTw == _stockCurrency ? 1 : _stockCurrency == "USD" ?
                    exchangeItems.Where(i => i.CurrencyTw == _stockCurrency && i.TransCurrencyTw == materialStockItem.PurDollarNameTw).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault() :
                    exchangeItems.Where(i => i.CurrencyTw == materialStockItem.PurDollarNameTw && i.TransCurrencyTw == _stockCurrency).OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();

                    _materialStock.StockDollarCodeId = _codeItems.Where(i => i.NameTW == _stockCurrency).Max(i => i.Id);
                    _materialStock.WarehouseNo = materialStockItem.WarehouseNo;
                    _materialStock.PurUnitCodeId = materialStockItem.PurUnitCodeId;
                    _materialStock.PurUnitNameTw = materialStockItem.PurUnit;
                    _materialStock.PurUnitNameEn = materialStockItem.PurUnit;

                    _materialStock.PCLUnitCodeId = materialStockItem.PurUnitCodeId;
                    _materialStock.PCLUnitNameTw = materialStockItem.PurUnit;
                    _materialStock.PCLUnitNameEn = materialStockItem.PurUnit;

                    // _materialStock.PurDollarCodeId = materialStockItem.PurDollarCodeId;
                    // _materialStock.PurDollarNameEn = materialStockItem.PurDollarNameTw;
                    // _materialStock.PurDollarNameTw = materialStockItem.PurDollarNameTw;
                    // _materialStock.ExchangeRate = exchangeRate;

                    _materialStock = MaterialStock.Update(_materialStock);
                }


                // Step2: 處理MaterialStockItem ============
                // Step2.1: 先看有沒有存在的資料，這裡要分處理出庫或入庫
                var _materialStockItem = MaterialStockItem.Get().Where(i => i.Id == materialStockItem.Id && i.LocaleId == materialStockItem.LocaleId).FirstOrDefault();

                // 處理拖外收貨單價，只有針對入庫的才有，出庫的話這段會跳過。以下變數提供給StockIO使用
                var _purUnitPrice = materialStockItem.PurUnitPrice;         //採購單價
                var _purDollarCodeId = materialStockItem.PurDollarCodeId;   //採購幣別
                var _exchangeRate = exchangeRate;                           //根據前端帶入的幣別轉換，但可能因為出入庫放是不同，匯率會是直接用庫存別別的匯率
                var _purUnitPriceLog = "";

                //入庫處理，來料入庫，單價來自收貨，但其中有拖外的，單價要收貨+原材料(MaterialStockItemPO)
                if (materialStockItem.PCLIOQty > 0)
                {
                    // 看看是不是拖外收貨入庫
                    var _stockInPOs = _receivedLog == null ? new List<Models.Views.MaterialStockItemPO>() : MaterialStockItemPO.Get().Where(i => i.POItemId == _receivedLog.POItemId && i.LocaleId == _receivedLog.LocaleId).ToList();

                    // MaterialStockItemPO有值表示拖外材料入庫，透過 receivedLog 的POItemId, 跟 MaterialStockItemPO的POItemId比對
                    if (_stockInPOs.Any())
                    {
                        //匯率轉換，拖外入庫的幣別一律轉換成庫存幣別
                        //抓出出入庫日期最近的一筆美金/台幣匯率，存入MaterialStockItemPO的已經換算過了
                        var _usdTontd = exchangeItems.Where(i => i.ExchDate <= materialStockItem.IODate && i.CurrencyTw == "USD" && i.TransCurrencyTw == "NTD").OrderByDescending(i => i.ExchDate).First();
                        _stockInPOs.ForEach(i =>
                        {
                            if (_materialStock.StockDollarCodeId != i.StockDollarCodeId)
                            {
                                i.MaterialCost = _materialStock.StockDollarNameTw == "USD" ? i.MaterialCost * _usdTontd.ReversedBankingRate : i.MaterialCost * _usdTontd.BankingRate;
                            }
                        });

                        var _stockInPO = _stockInPOs.GroupBy(i => new { i.POItemId, i.LocaleId, i.PurQty, i.StockOutQty }).Select(i => new { i.Key.POItemId, i.Key.LocaleId, PurQty = i.Key.PurQty, StockOutQty = i.Key.StockOutQty, MaterialCost = i.Sum(g => g.MaterialCost) }).First();
                        // 貼合單價 = 收貨單價＋原材料拖外出庫的單價
                        _purUnitPrice = (decimal)((_receivedLog.UnitPrice * exchangeRate) + (_stockInPO.MaterialCost / _stockInPO.PurQty)); // 用採購數量：選錯拖外採購單的雖然金額錯，但出庫的領料金額應該會一樣
                        // _purUnitPrice = decimal)((_receivedLog.UnitPrice * exchangeRate) + (_stockInPO.MaterialCost / _stockInPO.StockOutQty)); // 這裡改成用總金額除拖外出庫的數量不是採購單數量，因為倉庫很容易選錯拖外採購單，所以直接用出庫金額/出庫數量
                        _purDollarCodeId = materialStockItem.StockDollarCodeId;
                        _purUnitPriceLog = string.Format("P:{0:F6}+M:{1:F6}", _receivedLog.UnitPrice * exchangeRate, _stockInPO.MaterialCost / _stockInPO.PurQty); //拖外記錄單價組成，這時候exchangeRate是POItem的

                        //計算過後才更新匯率成1, 這時的採購幣別也已經改了
                        _exchangeRate = 1;
                    }
                }
                else
                {
                    // 出庫的幣別一定要等於StockDollarCodeId
                    _purDollarCodeId = _materialStock.StockDollarCodeId;
                    _exchangeRate = 1;

                    // 重新設定出庫的單價，出庫的單價如果有來料入庫，就用來料入庫沒有才用其他的，同時要用之前的入庫資料來計算
                    var stockInItems = MaterialStockItem.Get().Where(i => i.MaterialStockId == _materialStock.Id && i.LocaleId == _materialStock.LocaleId && i.PCLIOQty > 0).Select(i => new { i.Id, i.IODate, i.SourceType, i.PurDollarCodeId, i.PCLIOQty, i.PurUnitPrice, i.BankingRate, i.TransRate }).ToList();
                    var items = stockInItems.Where(i => i.SourceType == 0 && i.IODate <= materialStockItem.IODate).Any() ? stockInItems.Where(i => i.SourceType == 0 && i.IODate <= materialStockItem.IODate).ToList() : stockInItems;
                    if (items.Any())
                    {
                        var _stockInAmount = items.Sum(s => s.PCLIOQty * s.PurUnitPrice * s.BankingRate / s.TransRate);
                        var _stockInQty = items.Sum(s => s.PCLIOQty);
                        _purUnitPrice = _stockInAmount == 0 || _stockInQty == 0 ? 0 : _stockInAmount / _stockInQty;
                    }
                }

                // 更新MaterialStockItem
                if (_materialStockItem != null)
                {
                    // MaterialStockItem
                    materialStockItem.Id = _materialStockItem.Id;
                    materialStockItem.LocaleId = _materialStockItem.LocaleId;
                    materialStockItem.BankingRate = _exchangeRate; //(decimal)_materialStock.ExchangeRate;
                    materialStockItem.PurUnitPrice = _purUnitPrice; // 重新更新出庫單價
                    materialStockItem.PurDollarCodeId = _purDollarCodeId; //(decimal)_materialStock.PurDollarCodeId;
                    materialStockItem.MPSProcessNameEn = _purUnitPriceLog;    // 拖外單價細項


                    materialStockItem.StockDollarCodeId = _materialStock.StockDollarCodeId;
                    materialStockItem.PurUnitCodeId = _materialStock.PurUnitCodeId;
                    materialStockItem = MaterialStockItem.Update(materialStockItem);
                }
                else
                {

                    materialStockItem.MaterialStockId = _materialStock.Id;
                    materialStockItem.LocaleId = _materialStock.LocaleId;
                    materialStockItem.BankingRate = _exchangeRate;//(decimal)_materialStock.ExchangeRate;
                    materialStockItem.PurUnitPrice = _purUnitPrice;
                    materialStockItem.PurDollarCodeId = _purDollarCodeId; //(decimal)_materialStock.PurDollarCodeId;
                    materialStockItem.MPSProcessNameEn = _purUnitPriceLog;    // 拖外單價


                    materialStockItem.StockDollarCodeId = _materialStock.StockDollarCodeId;
                    materialStockItem.PurUnitCodeId = _materialStock.PurUnitCodeId;

                    materialStockItem.SeqNo = 1;
                    materialStockItem = MaterialStockItem.Create(materialStockItem);
                }

                //處理 StockSize
                //刪除所有的size
                MaterialStockItemSize.RemoveRange(i => i.StockIOId == materialStockItem.Id && i.LocaleId == materialStockItem.LocaleId);
                // 有size的就新增
                if (materialStockItemSize.Count() > 0)
                {
                    if (materialStockItem.Id != 0)
                    {
                        materialStockItemSize.ForEach(i =>
                        {
                            i.StockIOId = materialStockItem.Id;
                            i.LocaleId = materialStockItem.LocaleId;
                            i.PCLQty = i.PurQty;
                            i.ShoeSizeSuffix = i.ShoeSizeSuffix == null ? "" : i.ShoeSizeSuffix;
                            i.ShoeInnerSize = i.ShoeInnerSize == 0 ? (i.ShoeSizeSuffix == "J" ? (double)i.ShoeSize : (double)i.ShoeSize * 1000) : i.ShoeInnerSize;
                        });
                        MaterialStockItemSize.CreateRange(materialStockItemSize);
                    }
                }

                // 更新Size總庫存
                MaterialStockSize.UpdateSizeQty((int)_materialStock.Id, (int)_materialStock.LocaleId);

                // 更新庫存數MaterialStock
                MaterialStock.UpdatStockQty((int)_materialStock.Id, (int)_materialStock.LocaleId);
                // 更新收貨的庫存數
                if (materialStockItem.ReceivedLogId != null && materialStockItem.ReceivedLogId != 0)
                {
                    ReceivedLog.UpdateStockQty(new List<decimal> { (decimal)materialStockItem.ReceivedLogId }, materialStockItem.LocaleId);
                }

                // 拖外加工出庫-特殊流程
                if (materialStockItem.SourceType == 5)
                {
                    if (materialStockItemPO != null && materialStockItemPO.POItemId != 0)
                    {
                        MaterialStockItemPO.RemoveRange(i => i.StockIOId == materialStockItem.Id && i.LocaleId == materialStockItem.LocaleId);

                        materialStockItemPO.StockIOId = materialStockItem.Id;
                        materialStockItemPO.OrderNo = materialStockItem.OrderNo;
                        materialStockItemPO.StockDollarCodeId = materialStockItem.StockDollarCodeId;
                        materialStockItemPO.MaterialCost = 0 - (materialStockItem.PurUnitPrice * materialStockItem.PCLIOQty * materialStockItem.BankingRate);

                        MaterialStockItemPO.CreateRange(new List<ERP.Models.Views.MaterialStockItemPO> { materialStockItemPO });
                    }
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return GetMaterialStockItemGroup((int)materialStockItem.Id, (int)materialStockItem.LocaleId);
        }
        public void RemoveMaterialStockItemGroup(ERP.Models.Views.MaterialStockItemGroup item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var materialStock = MaterialStock.Get()
                        .Where(i => i.Id == item.MaterialStockItem.MaterialStockId && i.LocaleId == item.MaterialStockItem.LocaleId).FirstOrDefault();

                //拖外出庫
                if (item.MaterialStockItem.SourceType == 5)
                {
                    MaterialStockItemPO.RemoveRange(i => i.StockIOId == item.MaterialStockItem.Id && i.LocaleId == item.MaterialStockItem.LocaleId);
                }

                MaterialStockItemSize.RemoveRange(i => i.StockIOId == item.MaterialStockItem.Id && i.LocaleId == item.MaterialStockItem.LocaleId);
                MaterialStockItem.RemoveRange(i => i.Id == item.MaterialStockItem.Id && i.LocaleId == item.MaterialStockItem.LocaleId);

                var hasItem = MaterialStockItem.Get().Where(i => i.MaterialStockId == item.MaterialStockItem.MaterialStockId && i.LocaleId == item.MaterialStockItem.LocaleId).Any();

                // 沒有入庫就會刪掉MaterialStock
                if (hasItem)
                {
                    // _MaterialStock.UpdateMaterialStockInfo((int)item.MaterialStockItem.MaterialStockId, (int)item.MaterialStockItem.LocaleId);
                    UpdateMaterialStockInfo((int)item.MaterialStockItem.MaterialStockId, (int)item.MaterialStockItem.LocaleId);
                }
                else
                {
                    _MaterialStock.Remove(materialStock);
                }



                if (item.MaterialStockItem.ReceivedLogId != 0)
                {
                    UpdateReceivedLogStockQty(new List<decimal> { (decimal)item.MaterialStockItem.ReceivedLogId }, (int)item.MaterialStockItem.LocaleId);
                }



                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        //批次分批領料出庫
        public IQueryable<ERP.Models.Views.MaterialStockItem> SaveBatchMaterialStockItem(List<Models.Views.MaterialStockItem> items)
        {
            if (items.Count() > 0)
            {
                UnitOfWork.BeginTransaction();
                try
                {
                    {
                        //整理資料
                        items.ForEach(i =>
                        {
                            i.PurDollarCodeId = i.PurDollarCodeId == 0 ? i.StockDollarCodeId : i.PurDollarCodeId;
                        });
                    }
                    var localeId = items[0].LocaleId;
                    var msIds = items.Select(i => i.MaterialStockId).ToList();

                    // have stockId can update only, not have stockId add
                    var addItems = items.Where(i => i.Id == 0 || i.Id == null).ToList();
                    var updateItems = items.Where(i => i.Id > 0).ToList();
                    var msiIds = items.Select(i => i.Id).ToList();

                    if (updateItems.Count() > 0)
                    {
                        // 更新批次出庫的，要在重新抓一次單價
                        var uMSIds = updateItems.Select(i => i.MaterialStockId).ToList();
                        var sIOs = StockIO.Get().Where(i => uMSIds.Contains(i.MaterialStockId) && i.LocaleId == localeId && i.PCLIOQty > 0).ToList();
                        var avgPrice = sIOs.Sum(s => s.PurUnitPrice * s.BankingRate * s.PCLIOQty) / sIOs.Sum(s => s.PCLIOQty);

                        updateItems.ForEach(i =>
                        {
                            i.BankingRate = i.PCLIOQty < 0 ? 1 : i.BankingRate;
                            i.PurUnitPrice = avgPrice;
                        });

                        MaterialStockItem.UpdateRange(updateItems);
                    }
                    if (addItems.Count() > 0)
                    {
                        var sizeItems = MaterialStockSize.Get().Where(i => msIds.Contains(i.MaterialStockId) && i.LocaleId == localeId).ToList();
                        var stockItemsize = new List<Models.Views.MaterialStockItemSize>();

                        addItems.ForEach(i =>
                        {
                            var item = MaterialStockItem.Create(i);
                            msiIds.Add(item.Id);

                            var itemSizes = sizeItems.Where(i => i.MaterialStockId == item.MaterialStockId && i.LocaleId == item.LocaleId)
                            .Select(i => new Models.Views.MaterialStockItemSize
                            {
                                Id = 0,
                                LocaleId = item.LocaleId,
                                StockIOId = item.Id,
                                ShoeSize = i.ShoeSize,
                                ShoeSizeSuffix = i.ShoeSizeSuffix,
                                ShoeInnerSize = i.ShoeInnerSize,

                                PCLQty = 0 - i.PCLQty,
                                PurQty = 0 - i.PurQty,
                                ReLogSizeItemId = i.Id,
                                DisplaySize = i.DisplaySize
                            });

                            stockItemsize.AddRange(itemSizes);
                        });

                        // 統一新增SIZE避免序號問題
                        if (stockItemsize.Count() > 0)
                        {
                            MaterialStockItemSize.CreateRange(stockItemsize);
                        }
                    }

                    msIds.ForEach(i =>
                    {
                        // 更新總庫存
                        MaterialStockSize.UpdateSizeQty((int)i, (int)localeId);
                        // 更新庫存數MaterialStock
                        MaterialStock.UpdatStockQty((int)i, (int)localeId);
                    });

                    UnitOfWork.Commit();

                    items = MaterialStockItem.Get().Where(i => msiIds.Contains(i.Id) && i.LocaleId == localeId).ToList();
                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }

            return items.AsQueryable();
        }
        // 批次刪除出入庫
        public void RemoveBatchMaterialStockItem(List<Models.Views.MaterialStockItem> items)
        {
            if (items.Count() > 0)
            {
                UnitOfWork.BeginTransaction();
                try
                {
                    var localeId = items[0].LocaleId;
                    var msIds = items.Select(i => i.MaterialStockId).ToList();
                    var msItems = items.Select(i => i.Id).ToList();


                    if (msItems.Any())
                    {
                        MaterialStockItemSize.RemoveRange(i => i.LocaleId == localeId && msItems.Contains(i.StockIOId));
                        MaterialStockItem.RemoveRange(i => i.LocaleId == localeId && msItems.Contains(i.Id));
                    }

                    msIds.ForEach(i =>
                    {
                        // 更新總庫存
                        MaterialStockSize.UpdateSizeQty((int)i, (int)localeId);
                        // 更新庫存數MaterialStock
                        MaterialStock.UpdatStockQty((int)i, (int)localeId);
                    });

                    UnitOfWork.Commit();
                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
        }

        // 取得收貨資料後，按收貨Id取所有資料(size)
        // 來料入庫，把收貨資料轉成來料入庫資料
        public Models.Views.MaterialStockItemGroup GetMaterialStockItemFromImport(int receivedId, int localeId)
        {
            var receivedLog = ReceivedLog.Get().Where(i => i.Id == receivedId && i.LocaleId == localeId).FirstOrDefault();
            var group = new Models.Views.MaterialStockItemGroup { };
            if (receivedLog != null)
            {
                var item = receivedLog;
                var sizeItems = ReceivedLogSizeItem.Get().Where(i => i.ReceivedLogId == receivedId && i.LocaleId == localeId)
                .Select(i => new Models.Views.MaterialStockItemSize
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    StockIOId = 0,
                    ShoeSize = Convert.ToDecimal(i.SeqNo),
                    ShoeSizeSuffix = "",
                    ShoeInnerSize = (double)i.ShoeInnerSize,
                    PCLQty = i.ReceivedQty,
                    PurQty = i.ReceivedQty,
                    ReLogSizeItemId = i.Id,
                    DisplaySize = i.DisplaySize
                })
                .ToList();

                sizeItems.ForEach(i =>
                {
                    i.ShoeSize = i.ShoeInnerSize < 1000 ? (decimal)i.ShoeInnerSize : (decimal)i.ShoeInnerSize / 1000;
                    i.ShoeSizeSuffix = i.ShoeInnerSize < 1000 ? "J" : "";
                });

                group.MaterialStockItem = new Models.Views.MaterialStockItem
                {
                    Id = 0,
                    LocaleId = item.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = 0,
                    MaterialId = item.MaterialId,
                    WarehouseId = item.WarehouseId,
                    OrderNo = item.OrderNo,
                    PCLUnitCodeId = item.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = item.PurUnitCodeId,
                    PCLIOQty = item.IQCGetQty,
                    PurIOQty = item.ReceivedQty,
                    ReceivedLogId = item.Id,
                    PurUnitPrice = item.UnitPrice,
                    PurDollarCodeId = item.PurDollarCodeId,
                    BankingRate = 1,
                    StockDollarCodeId = CodeItem.Get().Where(u => u.NameTW == "USD" && u.LocaleId == item.LocaleId && u.CodeType == "02").Max(u => u.Id),
                    Remark = "來料入庫",
                    RefNo = "SI-" + item.Id,
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

                    MaterialName = item.MaterialNameTw,
                    MaterialNameEng = item.MaterialNameEng,
                    WarehouseNo = item.WarehouseNo,
                };
                group.MaterialStockItemSize = sizeItems;
            }
            return group;

        }
        // 其他入庫，把庫存資料轉成其他入庫資料
        public Models.Views.MaterialStockItemGroup GetMaterialOtherStockInFromImport(int materialId, int localeId)
        {
            var material = Material.Get().Where(i => i.Id == materialId && i.LocaleId == localeId).FirstOrDefault();
            var group = new Models.Views.MaterialStockItemGroup { };
            if (material != null)
            {
                var item = MaterialStock.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId).FirstOrDefault();
                // var quot = MaterialQuot.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId).OrderByDescending(i => i.EffectiveDate).FirstOrDefault();
                var stockIn1 = MaterialStockItem.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId && i.SourceType == 0 && i.PCLIOQty > 0 && i.PurUnitPrice > 0).Select(i => new { i.IODate, i.PurUnitPrice, i.PurDollarCodeId, i.PurDollarNameTw }).OrderByDescending(i => i.IODate).Distinct().FirstOrDefault();
                var stockIn2 = MaterialStockItem.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId && i.SourceType == 0 && i.PCLIOQty > 0 && i.PurUnitPrice > 0).Select(i => new { i.IODate, i.PurUnitPrice, i.PurDollarCodeId, i.PurDollarNameTw }).OrderByDescending(i => i.IODate).Distinct().FirstOrDefault();

                var stockIn = stockIn1 != null ? stockIn1 : (stockIn2 != null ? stockIn2 : null);

                var dollarCodeId = CodeItem.Get().Where(u => u.NameTW == "USD" && u.LocaleId == localeId && u.CodeType == "02").Max(u => u.Id);

                group.MaterialStockItem = new Models.Views.MaterialStockItem
                {
                    Id = 0,
                    LocaleId = material.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = 8,
                    MaterialId = material.Id,

                    MaterialName = material.MaterialName,
                    MaterialNameEng = material.MaterialNameEng,

                    WarehouseId = item == null ? 0 : item.WarehouseId,
                    WarehouseNo = item == null ? "" : item.WarehouseNo,
                    PCLUnitCodeId = item == null ? (decimal)material.VolumeUnitCodeId : item.PCLUnitCodeId,
                    PurUnitCodeId = item == null ? (decimal)material.VolumeUnitCodeId : item.PurUnitCodeId,
                    // PurUnitPrice = (item == null || item.PurUnitPrice == null) ? 0 : (decimal)item.PurUnitPrice,
                    // PurDollarCodeId = (item == null || item.PurDollarCodeId == null) ? dollarCodeId : (decimal)item.PurDollarCodeId,

                    // PurDollarCodeId = (quot == null || quot.DollarCodeId == null) ? dollarCodeId : quot.DollarCodeId,
                    // PurUnitPrice = (quot == null || quot.UnitPrice == null) ? 0 : (decimal)quot.UnitPrice,
                    PurUnitPrice = stockIn == null ? 0 : stockIn.PurUnitPrice,
                    PurDollarCodeId = stockIn == null ? 0 : stockIn.PurDollarCodeId,

                    MaterialStockId = 0,
                    PCLIOQty = 0,
                    PurIOQty = 0,
                    PrePCLQty = 0,
                    PreAmount = 0,
                    TransRate = 1,
                    ReceivedLogId = 0,
                    BankingRate = 1,    //匯率會在儲存的時候處理，這裡先隨便帶
                    StockDollarCodeId = item == null ? dollarCodeId : item.StockDollarCodeId,
                    Remark = "其他入庫",
                    OrderNo = "",
                    RefNo = "",
                    OrgUnitId = 0,
                    OrgUnitNameTw = "",
                    OrgUnitNameEn = "",
                    MPSProcessId = 0,
                    MPSProcessNameTw = "",
                    MPSProcessNameEn = "",
                    RefUserName = "",

                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,

                };
                group.MaterialStockItemSize = new List<Models.Views.MaterialStockItemSize>();
            }
            return group;
        }
        public Models.Views.MaterialStockItemGroup GetMaterialOtherStockInFromImport1(int materialId, int localeId)
        {
            var material = Material.Get().Where(i => i.Id == materialId && i.LocaleId == localeId).FirstOrDefault();
            var group = new Models.Views.MaterialStockItemGroup { };
            if (material != null)
            {
                var item = MaterialStock.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId).FirstOrDefault();
                var quot = MaterialQuot.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId).OrderByDescending(i => i.EffectiveDate).FirstOrDefault();
                var dollarCodeId = CodeItem.Get().Where(u => u.NameTW == "USD" && u.LocaleId == localeId && u.CodeType == "02").Max(u => u.Id);

                group.MaterialStockItem = new Models.Views.MaterialStockItem
                {
                    Id = 0,
                    LocaleId = material.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = 8,
                    MaterialId = material.Id,

                    MaterialName = material.MaterialName,
                    MaterialNameEng = material.MaterialNameEng,

                    WarehouseId = item == null ? 0 : item.WarehouseId,
                    WarehouseNo = item == null ? "" : item.WarehouseNo,
                    PCLUnitCodeId = item == null ? (decimal)material.VolumeUnitCodeId : item.PCLUnitCodeId,
                    PurUnitCodeId = item == null ? (decimal)material.VolumeUnitCodeId : item.PurUnitCodeId,
                    // PurUnitPrice = (item == null || item.PurUnitPrice == null) ? 0 : (decimal)item.PurUnitPrice,
                    // PurDollarCodeId = (item == null || item.PurDollarCodeId == null) ? dollarCodeId : (decimal)item.PurDollarCodeId,
                    PurDollarCodeId = (quot == null || quot.DollarCodeId == null) ? dollarCodeId : quot.DollarCodeId,
                    PurUnitPrice = (quot == null || quot.UnitPrice == null) ? 0 : (decimal)quot.UnitPrice,

                    MaterialStockId = 0,

                    PCLIOQty = 0,
                    PurIOQty = 0,
                    PrePCLQty = 0,
                    PreAmount = 0,
                    TransRate = 1,
                    ReceivedLogId = 0,
                    BankingRate = 1,    //匯率會在儲存的時候處理，這裡先隨便帶
                    StockDollarCodeId = item == null ? dollarCodeId : item.StockDollarCodeId,
                    Remark = "其他入庫",
                    OrderNo = "",
                    RefNo = "",
                    OrgUnitId = 0,
                    OrgUnitNameTw = "",
                    OrgUnitNameEn = "",
                    MPSProcessId = 0,
                    MPSProcessNameTw = "",
                    MPSProcessNameEn = "",
                    RefUserName = "",

                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,

                };
                group.MaterialStockItemSize = new List<Models.Views.MaterialStockItemSize>();
            }
            return group;
        }
        //退料入庫，把1,7的出庫資訊當入入庫資訊
        public Models.Views.MaterialStockItemGroup GetMaterialReturnStockInFromImport(int stockId, int localeId)
        {
            var stockItem = MaterialStockItem.Get().Where(i => i.Id == stockId && i.LocaleId == localeId).FirstOrDefault();

            var group = new Models.Views.MaterialStockItemGroup { };
            if (stockItem != null)
            {
                stockItem.Id = 0;
                stockItem.SourceType = 6;
                stockItem.IODate = DateTime.Now.Date;
                stockItem.Remark = "退料入庫";
                stockItem.PCLIOQty = 0 - stockItem.PCLIOQty;
                stockItem.PurIOQty = 0 - stockItem.PCLIOQty;
                stockItem.RefUserName = "";
                stockItem.ModifyUserName = "";
                stockItem.LastUpdateTime = DateTime.Now;
                stockItem.ReceivedLogId = 0;
                stockItem.PreAmount = 0;
                stockItem.PrePCLQty = 0;
                stockItem.UsageQty = StockIO.Get().Where(s => s.MaterialStockId == stockItem.MaterialStockId && s.LocaleId == stockItem.LocaleId && s.PCLIOQty < 0).Sum(s => s.PCLIOQty); ;

                group.MaterialStockItem = stockItem;
                group.MaterialStockItemSize = MaterialStockItemSize.Get().Where(i => i.StockIOId == stockId && i.LocaleId == localeId)
                .Select(i => new Models.Views.MaterialStockItemSize
                {
                    Id = 0,
                    LocaleId = i.LocaleId,
                    StockIOId = 0,
                    ShoeSize = i.ShoeSize,
                    ShoeSizeSuffix = i.ShoeSizeSuffix,
                    ShoeInnerSize = i.ShoeInnerSize,
                    PCLQty = 0 - i.PCLQty,
                    PurQty = 0 - i.PurQty,
                    ReLogSizeItemId = i.ReLogSizeItemId,
                    DisplaySize = i.DisplaySize
                });
            }
            return group;
        }


        // 出庫
        // 1. 領出幣別、單位，要跟著MaterialStock 的資料
        // 2. 舊系統要能正常運作，所以在前庫存有塞資料(等於當時的庫存)，如果庫存的資料有曾經異動，可能會跟最新庫存不同
        // 3. 全部轉換到新系統後，要重新跟新相關的庫存幣別
        public Models.Views.MaterialStockItemGroup GetMaterialStockItemFromOutImport(int materialStockId, int localeId, int mpsDailyId, int mpsLocaleId, int sourceType)
        {
            var materialStock = MaterialStock.Get().Where(i => i.Id == materialStockId && i.LocaleId == localeId).FirstOrDefault();
            var group = new Models.Views.MaterialStockItemGroup { };
            if (materialStock != null)
            {
                decimal _amount = 0, _pclQty = 0, _avgPrice = 0;

                // 改取一年內的單價，一般案批買的沒差別，但通用材料的只取比較新買的單價
                var items = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == localeId && i.PCLIOQty > 0 && i.IODate >= DateTime.Today.AddDays(-365)).Select(i => new { i.Id, i.PurDollarCodeId, i.PCLIOQty, i.PurUnitPrice, i.BankingRate, i.TransRate, i.SourceType, i.IODate }).ToList();
                if (items.Any())
                {
                    // 如果一年內有來料入庫的單價，就取來料入庫的單價，沒有就用所有入庫的平均
                    items = items.Where(i => i.SourceType == 0).Any() ? items.Where(i => i.SourceType == 0).ToList() : items;

                    _amount = items.Sum(s => s.PCLIOQty * s.PurUnitPrice * s.BankingRate / s.TransRate);
                    _pclQty = items.Sum(s => s.PCLIOQty);
                    _avgPrice = _amount == 0 || _pclQty == 0 ? 0 : _amount / _pclQty;
                }


                var mpsDaily = MPSDaily.Get().Where(i => i.Id == mpsDailyId && i.LocaleId == mpsLocaleId).FirstOrDefault();
                group.MaterialStockItem = new Models.Views.MaterialStockItem
                {
                    Id = 0,
                    LocaleId = materialStock.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = sourceType,
                    MaterialId = materialStock.MaterialId,
                    WarehouseId = materialStock.WarehouseId,
                    OrderNo = materialStock.OrderNo,
                    PCLUnitCodeId = materialStock.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = materialStock.PurUnitCodeId,

                    ReceivedLogId = 0,
                    PurUnitPrice = _avgPrice,
                    BankingRate = 1, //出庫的化用的是出庫單價。匯率一律為1
                    // PurDollarCodeId = _purDollarCodeId, //(decimal)materialStock.PurDollarCodeId,
                    PurDollarCodeId = materialStock.StockDollarCodeId, // 採購筆等於出庫幣別
                    StockDollarCodeId = materialStock.StockDollarCodeId,
                    Remark = sourceType == 1 ? "分批領料出庫" : sourceType == 7 ? "補料領料出庫" : sourceType == 9 ? "其他領料出庫" : sourceType == 12 ? "共用領料出庫" : sourceType == 5 ? "拖外加工出庫" : "",
                    RefNo = mpsDaily != null ? mpsDaily.DailyNo : materialStock.OrderNo,

                    // PCLIOQty = materialStock.PCLQty,
                    // PurIOQty = materialStock.PCLQty,
                    StockQty = materialStock.PCLQty == null ? 0 : materialStock.PCLQty,    //庫存數
                    UsageQty = materialStock.RealStockOutQty == null ? 0 : materialStock.RealStockOutQty,   //領用數
                    MPSQty = mpsDaily != null ? (decimal)(mpsDaily.TotalUsage) : 0, // 派工數量

                    PCLIOQty = 0 - (mpsDaily != null ? (decimal)(mpsDaily.TotalUsage) : materialStock.PCLQty),
                    PurIOQty = 0 - (mpsDaily != null ? (decimal)(mpsDaily.TotalUsage) : materialStock.PCLQty),
                    // OrgUnitId = mpsDaily != null ? mpsDaily.OrgUnitId : 0,
                    // OrgUnitNameTw = mpsDaily != null ? mpsDaily.OrgUnitNameTw : "",
                    // OrgUnitNameEn = mpsDaily != null ? mpsDaily.OrgUnitNameEn : "",

                    OrgUnitId = 0,
                    OrgUnitNameTw = "",
                    OrgUnitNameEn = "",

                    MPSProcessId = mpsDaily != null ? (decimal)(mpsDaily.MPSProcessId) : 0,
                    MPSProcessNameTw = mpsDaily != null ? mpsDaily.MPSProcessNameTw : "",
                    MPSProcessNameEn = mpsDaily != null ? mpsDaily.MPSProcessNameEn : "",

                    RefUserName = "",
                    MaterialStockId = materialStock.Id,
                    PrePCLQty = materialStock.PCLQty,
                    PreAmount = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,

                    MaterialName = materialStock.MaterialName,
                    MaterialNameEng = materialStock.MaterialNameEng,
                    WarehouseNo = materialStock.WarehouseNo,
                    DailyNo = mpsDaily != null ? mpsDaily.DailyNo : "",
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
                    // DisplaySize = i.ShoeSize.ToString("0.0")
                    DisplaySize = i.ShoeSize.ToString(),
                }).ToList();

                //來料出庫才取po
                if (sourceType == 5)
                {
                    //取得ProcessPO
                    group.MaterialStockItemPO = new Models.Views.MaterialStockItemPO
                    {
                        Id = 0,
                        LocaleId = localeId,
                        POId = 0,
                        POItemId = 0,
                        StockIOId = 0,
                        MaterialCost = 0,
                        StockDollarCodeId = materialStock.StockDollarCodeId,
                        OrderNo = materialStock.OrderNo,
                        OPCount = 1,
                        // ModifyUserName = null,
                        // LastUpdateTime = null,
                        ParentMaterialId = 0,
                        ParentMaterialNameTw = "",
                        ParentMaterialNameEng = "",
                        MaterialId = 0,
                        MaterialNameTw = "",
                        MaterialNameEng = "",
                        PurQty = 0,
                        PONo = "",
                        POType = 0,
                    };

                    //取得拖外加工PO
                    var poTypes = new List<int?> { 2, 6 };
                    group.OutsourcePOItem = POItem.Get()
                            .Where(i => i.OrderNo != "" && i.OrderNo == materialStock.OrderNo && poTypes.Contains(i.POType) && i.Status != 2)
                            .Select(i => new Models.Views.OutsourcePOItem
                            {
                                Id = i.Id,
                                LocaleId = i.LocaleId,
                                POId = i.POId,
                                MaterialId = i.MaterialId,
                                MaterialName = i.Material,
                                MaterialNameEng = i.MaterialEng,
                                PONo = i.PONo,
                                DisplayItem = i.PONo + "-【" + i.Material + "】",
                                DisplayItemEng = i.PONo + "-【" + i.MaterialEng + "】",
                                Qty = i.Qty,
                            })
                            .OrderBy(i => i.PONo)
                            .ThenBy(i => i.MaterialName)
                            .ToList();
                }
            }
            return group;
        }

        // 取入庫匯入的資料
        // 來料入庫的材料匯入查詢，資料來自收貨記錄
        public IQueryable<Models.Views.ReceivedLog> GetMaterialForPOStockIn()
        {
            var result = ReceivedLog.Get().Where(i => i.TransferInId == 0 && (i.StockQty + i.TransferQty) < i.IQCGetQty && i.IQCResult >= 2);
            return result;
        }

        // 其他入庫的材料匯入查詢，資料來自材料基本擋
        public IQueryable<Models.Views.Material> GetMaterialForOtherStockIn()
        {
            return Material.Get();
        }
        // 退料入庫，從分批領料出庫跟補料出庫中選取
        public IQueryable<Models.Views.MaterialStockItem> GetMaterialForReturnStockIn()
        {
            var sourceType = new List<int> { 1, 7 };

            var result = MaterialStockItem.Get().Where(i => sourceType.Contains(i.SourceType));
            return result;
        }

        // 出庫的材料匯入查詢，資料來自MaterialStock+派工單
        public IQueryable<Models.Views.MaterialStockOut> GetMaterialForPOStockOut(string predicate)
        {
            var materialStocks = (
                from ms in MaterialStock.Get()
                join mps in MPSDaily.Get() on new { OrderNo = ms.OrderNo, MaterialName = ms.MaterialName, LocaleId = ms.LocaleId } equals new { OrderNo = mps.OrderNo, MaterialName = mps.MaterialNameTw, LocaleId = mps.LocaleId } into mpsGRP
                from mps in mpsGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
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
                    DailyNo = mps.DailyNo,
                    MPSDailyId = mps.Id,
                    MPSDailyLocaleId = mps.LocaleId,
                    SemiGoods = m.SemiGoods,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
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
                SemiGoods = i.SemiGoods,
                DailyNo = i.DailyNo,
                MPSDailyId = i.MPSDailyId,
                MPSDailyLocaleId = i.MPSDailyLocaleId,
            })
            .Distinct();

            return materialStocks;
        }
        public IQueryable<Models.Views.MaterialStockOut> GetMaterialForPOStockOut()
        {
            var materialStocks = (
                from ms in MaterialStock.Get()
                join mps in MPSDaily.Get() on new { OrderNo = ms.OrderNo, MaterialName = ms.MaterialName, LocaleId = ms.LocaleId } equals new { OrderNo = mps.OrderNo, MaterialName = mps.MaterialNameTw, LocaleId = mps.LocaleId } into mpsGRP
                from mps in mpsGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
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
                    DailyNo = mps.DailyNo,
                    MPSDailyId = mps.Id,
                    MPSDailyLocaleId = mps.LocaleId,
                    SemiGoods = m.SemiGoods,
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
                SemiGoods = i.SemiGoods,
                DailyNo = i.DailyNo,
                MPSDailyId = i.MPSDailyId,
                MPSDailyLocaleId = i.MPSDailyLocaleId,
            })
            .Distinct();

            return materialStocks;
        }

        // 批次出庫的材料匯入查詢，資料來自MaterialStock+派工單
        public IQueryable<Models.Views.MaterialStockItem> GetMaterialForBatchPOStockOut(string predicate)
        {
            var materialStocks = (
                from ms in MaterialStock.Get()
                join mps in MPSDaily.Get() on new { OrderNo = ms.OrderNo, MaterialName = ms.MaterialName, LocaleId = ms.LocaleId } equals new { OrderNo = mps.OrderNo, MaterialName = mps.MaterialNameTw, LocaleId = mps.LocaleId } into mpsGRP
                from mps in mpsGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new
                {
                    Id = ms.Id,
                    LocaleId = ms.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = 1,
                    MaterialId = ms.MaterialId,
                    WarehouseId = ms.WarehouseId,
                    OrderNo = ms.OrderNo,
                    PCLUnitCodeId = ms.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = ms.PurUnitCodeId,
                    ReceivedLogId = 0,
                    PurUnitPrice = (decimal?)StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId && i.PCLIOQty > 0).Average(i => i.PurUnitPrice * i.BankingRate),
                    // PurUnitPrice = (ms.Amount == 0 || ms.PCLQty == 0) ? 0 : ms.Amount / ms.PCLQty,
                    // BankingRate = ms.ExchangeRate,
                    BankingRate = 1,
                    PurDollarCodeId = ms.StockDollarCodeId,
                    StockDollarCodeId = ms.StockDollarCodeId,
                    Remark = "分批領料出庫(批量)",
                    RefNo = "",
                    StockQty = ms.PCLQty,    //庫存數
                    UsageQty = ms.RealStockOutQty,   //領用數
                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    WarehouseNo = ms.WarehouseNo,

                    SemiGoods = m.SemiGoods,

                    DailyId = (decimal?)mps.Id,
                    DailyNo = (string?)mps.DailyNo,
                    MPSQty = (decimal?)(mps.TotalUsage), // 派工數量
                    PCLIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    PurIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    OrgUnitId = (decimal?)mps.OrgUnitId,
                    OrgUnitNameTw = (string?)mps.OrgUnitNameTw,
                    OrgUnitNameEn = (string?)mps.OrgUnitNameEn,
                    MPSProcessId = (decimal?)(mps.MPSProcessId),
                    MPSProcessNameTw = (string?)mps.MPSProcessNameTw,
                    MPSProcessNameEn = (string?)mps.MPSProcessNameEn,
                    RefUserName = "",
                    MaterialStockId = (decimal?)ms.Id,
                    PrePCLQty = (decimal?)ms.PCLQty,
                    PreAmount = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    MPSDailyId = (decimal?)mps.Id,
                    MPSDailyLocaleId = (decimal?)mps.LocaleId,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.MaterialStockItem
            {
                Id = 0,
                LocaleId = i.LocaleId,
                IODate = DateTime.Now.Date,
                SourceType = 1,
                MaterialId = i.MaterialId,
                WarehouseId = i.WarehouseId,
                OrderNo = i.OrderNo,
                PCLUnitCodeId = i.PCLUnitCodeId,
                TransRate = i.TransRate,
                PurUnitCodeId = i.PurUnitCodeId,
                ReceivedLogId = i.ReceivedLogId,
                PurUnitPrice = i.PurUnitPrice ?? 0,
                BankingRate = (decimal)i.BankingRate,
                PurDollarCodeId = (decimal)i.PurDollarCodeId,
                StockDollarCodeId = i.StockDollarCodeId,
                Remark = i.Remark,
                RefNo = i.OrderNo,

                StockQty = i.StockQty,    //庫存數
                UsageQty = i.UsageQty,   //領用數
                // MPSQty = i.MPSQty,// 派工數量

                PCLIOQty = 0 - i.StockQty,
                PurIOQty = 0 - i.StockQty,

                OrgUnitId = i.OrgUnitId,
                OrgUnitNameTw = i.OrgUnitNameTw,
                OrgUnitNameEn = i.OrgUnitNameEn ?? "",

                MPSProcessId = i.MPSProcessId,
                MPSProcessNameTw = i.MPSProcessNameTw,
                MPSProcessNameEn = i.MPSProcessNameEn,

                RefUserName = i.RefUserName,
                MaterialStockId = i.MaterialStockId ?? 0,
                PrePCLQty = i.PrePCLQty ?? 0,
                PreAmount = i.PreAmount,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = DateTime.Now,

                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
                WarehouseNo = i.WarehouseNo,

                DailyId = i.MPSDailyId,
                DailyNo = i.DailyNo,
                SeqNo = 2,
                SemiGoods = i.SemiGoods,
            })
            .ToList();

            return materialStocks.AsQueryable();
        }
        public IQueryable<Models.Views.MaterialStockItem> GetMaterialForBatchPOStockOut()
        {
            var materialStocks = (
                from ms in MaterialStock.Get()
                join mps in MPSDaily.GetHead() on new { OrderNo = ms.OrderNo, MaterialName = ms.MaterialName, LocaleId = ms.LocaleId } equals new { OrderNo = mps.OrderNo, MaterialName = mps.MaterialNameTw, LocaleId = mps.LocaleId } into mpsGRP
                from mps in mpsGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new
                {
                    Id = ms.Id,
                    LocaleId = ms.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = 1,
                    MaterialId = ms.MaterialId,
                    WarehouseId = ms.WarehouseId,
                    OrderNo = ms.OrderNo,
                    PCLUnitCodeId = ms.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = ms.PurUnitCodeId,
                    ReceivedLogId = 0,
                    PurUnitPrice = (decimal?)StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId && i.PCLIOQty > 0).Average(i => i.PurUnitPrice * i.BankingRate),
                    // PurUnitPrice = (ms.Amount == 0 || ms.PCLQty == 0) ? 0 : ms.Amount / ms.PCLQty,
                    // BankingRate = ms.ExchangeRate,
                    BankingRate = 1,
                    PurDollarCodeId = ms.StockDollarCodeId,
                    StockDollarCodeId = ms.StockDollarCodeId,
                    Remark = "分批領料出庫(批量)",
                    RefNo = "",
                    StockQty = ms.PCLQty,    //庫存數
                    UsageQty = ms.RealStockOutQty,   //領用數
                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    WarehouseNo = ms.WarehouseNo,

                    SemiGoods = m.SemiGoods,

                    DailyNo = (string?)mps.DailyNo,
                    MPSQty = (decimal?)(mps.TotalUsage), // 派工數量
                    PCLIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    PurIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    OrgUnitId = (decimal?)mps.OrgUnitId,
                    OrgUnitNameTw = (string?)mps.OrgUnitNameTw,
                    OrgUnitNameEn = (string?)mps.OrgUnitNameEn,
                    MPSProcessId = (decimal?)(mps.MPSProcessId),
                    MPSProcessNameTw = (string?)mps.MPSProcessNameTw,
                    MPSProcessNameEn = (string?)mps.MPSProcessNameEn,
                    RefUserName = "",
                    MaterialStockId = (decimal?)ms.Id,
                    PrePCLQty = (decimal?)ms.PCLQty,
                    PreAmount = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    MPSDailyId = (decimal?)mps.Id,
                    MPSDailyLocaleId = (decimal?)mps.LocaleId,
                }
            )
            .Select(i => new Models.Views.MaterialStockItem
            {
                Id = 0,
                LocaleId = i.LocaleId,
                IODate = DateTime.Now.Date,
                SourceType = 1,
                MaterialId = i.MaterialId,
                WarehouseId = i.WarehouseId,
                OrderNo = i.OrderNo,
                PCLUnitCodeId = i.PCLUnitCodeId,
                TransRate = i.TransRate,
                PurUnitCodeId = i.PurUnitCodeId,
                ReceivedLogId = i.ReceivedLogId,
                PurUnitPrice = i.PurUnitPrice ?? 0,
                BankingRate = (decimal)i.BankingRate,
                PurDollarCodeId = (decimal)i.PurDollarCodeId,
                StockDollarCodeId = i.StockDollarCodeId,
                Remark = i.Remark,
                RefNo = i.OrderNo,

                StockQty = i.StockQty,    //庫存數
                UsageQty = i.UsageQty,   //領用數
                // MPSQty = i.MPSQty,// 派工數量

                PCLIOQty = 0 - i.StockQty,
                PurIOQty = 0 - i.StockQty,

                OrgUnitId = i.OrgUnitId,
                OrgUnitNameTw = i.OrgUnitNameTw,
                OrgUnitNameEn = i.OrgUnitNameEn ?? "",

                MPSProcessId = i.MPSProcessId,
                MPSProcessNameTw = i.MPSProcessNameTw,
                MPSProcessNameEn = i.MPSProcessNameEn,

                RefUserName = i.RefUserName,
                MaterialStockId = i.MaterialStockId ?? 0,
                PrePCLQty = i.PrePCLQty ?? 0,
                PreAmount = i.PreAmount,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = DateTime.Now,

                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
                WarehouseNo = i.WarehouseNo,

                DailyId = i.MPSDailyId,
                DailyNo = i.DailyNo,
                SeqNo = 2,
                SemiGoods = i.SemiGoods,
            });

            return materialStocks;
        }
        public IQueryable<Models.Views.MaterialStockItem> GetMaterialForBatchPOStockOut2()
        {
            var materialStocks = (
                from ms in MaterialStock.Get()
                join mps in MPSDaily.Get() on new { OrderNo = ms.OrderNo, MaterialName = ms.MaterialName, LocaleId = ms.LocaleId } equals new { OrderNo = mps.OrderNo, MaterialName = mps.MaterialNameTw, LocaleId = mps.LocaleId } into mpsGRP
                from mps in mpsGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new
                {
                    Id = ms.Id,
                    LocaleId = ms.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = 1,
                    MaterialId = ms.MaterialId,
                    WarehouseId = ms.WarehouseId,
                    OrderNo = ms.OrderNo,
                    PCLUnitCodeId = ms.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = ms.PurUnitCodeId,
                    ReceivedLogId = 0,
                    PurUnitPrice = (decimal?)StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId && i.PCLIOQty > 0).Average(i => i.PurUnitPrice * i.BankingRate),
                    // PurUnitPrice = (ms.Amount == 0 || ms.PCLQty == 0) ? 0 : ms.Amount / ms.PCLQty,
                    // BankingRate = ms.ExchangeRate,
                    BankingRate = 1,
                    PurDollarCodeId = ms.StockDollarCodeId,
                    StockDollarCodeId = ms.StockDollarCodeId,
                    Remark = "分批領料出庫(批量)",
                    RefNo = "",
                    StockQty = ms.PCLQty,    //庫存數
                    UsageQty = ms.RealStockOutQty,   //領用數
                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    WarehouseNo = ms.WarehouseNo,

                    SemiGoods = m.SemiGoods,

                    DailyNo = (string?)mps.DailyNo,
                    MPSQty = (decimal?)(mps.TotalUsage), // 派工數量
                    PCLIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    PurIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    OrgUnitId = (decimal?)mps.OrgUnitId,
                    OrgUnitNameTw = (string?)mps.OrgUnitNameTw,
                    OrgUnitNameEn = (string?)mps.OrgUnitNameEn,
                    MPSProcessId = (decimal?)(mps.MPSProcessId),
                    MPSProcessNameTw = (string?)mps.MPSProcessNameTw,
                    MPSProcessNameEn = (string?)mps.MPSProcessNameEn,
                    RefUserName = "",
                    MaterialStockId = (decimal?)ms.Id,
                    PrePCLQty = (decimal?)ms.PCLQty,
                    PreAmount = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    MPSDailyId = (decimal?)mps.Id,
                    MPSDailyLocaleId = (decimal?)mps.LocaleId,
                }
            )
            .Select(i => new Models.Views.MaterialStockItem
            {
                Id = 0,
                LocaleId = i.LocaleId,
                IODate = DateTime.Now.Date,
                SourceType = 1,
                MaterialId = i.MaterialId,
                WarehouseId = i.WarehouseId,
                OrderNo = i.OrderNo,
                PCLUnitCodeId = i.PCLUnitCodeId,
                TransRate = i.TransRate,
                PurUnitCodeId = i.PurUnitCodeId,
                ReceivedLogId = i.ReceivedLogId,
                PurUnitPrice = i.PurUnitPrice ?? 0,
                BankingRate = (decimal)i.BankingRate,
                PurDollarCodeId = (decimal)i.PurDollarCodeId,
                StockDollarCodeId = i.StockDollarCodeId,
                Remark = i.Remark,
                RefNo = i.OrderNo,

                StockQty = i.StockQty,    //庫存數
                UsageQty = i.UsageQty,   //領用數
                MPSQty = i.MPSQty,// 派工數量

                PCLIOQty = 0 - i.StockQty,
                PurIOQty = 0 - i.StockQty,

                OrgUnitId = i.OrgUnitId,
                OrgUnitNameTw = i.OrgUnitNameTw,
                OrgUnitNameEn = i.OrgUnitNameEn ?? "",

                MPSProcessId = i.MPSProcessId,
                MPSProcessNameTw = i.MPSProcessNameTw,
                MPSProcessNameEn = i.MPSProcessNameEn,

                RefUserName = i.RefUserName,
                MaterialStockId = i.MaterialStockId ?? 0,
                PrePCLQty = i.PrePCLQty ?? 0,
                PreAmount = i.PreAmount,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = DateTime.Now,

                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
                WarehouseNo = i.WarehouseNo,

                DailyId = i.MPSDailyId,
                DailyNo = i.DailyNo,
                SeqNo = 2,
                SemiGoods = i.SemiGoods,
            });

            return materialStocks;
        }
        // 批次出庫的材料匯入查詢，資料來自MaterialStock+派工單
        public IQueryable<Models.Views.MaterialStockItem> GetMaterialForBatchPOStockOut1()
        {
            var materialStocks = (
                from ms in MaterialStock.Get()
                join mps in MPSDaily.Get() on new { OrderNo = ms.OrderNo, MaterialName = ms.MaterialName, LocaleId = ms.LocaleId } equals new { OrderNo = mps.OrderNo, MaterialName = mps.MaterialNameTw, LocaleId = mps.LocaleId } into mpsGRP
                from mps in mpsGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ms.MaterialId, LocaleId = ms.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new
                {
                    Id = ms.Id,
                    LocaleId = ms.LocaleId,
                    IODate = DateTime.Now.Date,
                    SourceType = 1,
                    MaterialId = ms.MaterialId,
                    WarehouseId = ms.WarehouseId,
                    OrderNo = ms.OrderNo,
                    PCLUnitCodeId = ms.PCLUnitCodeId,
                    TransRate = 1,
                    PurUnitCodeId = ms.PurUnitCodeId,
                    ReceivedLogId = 0,
                    PurUnitPrice = (decimal?)StockIO.Get().Where(i => i.MaterialStockId == ms.Id && i.LocaleId == ms.LocaleId && i.PCLIOQty > 0).Average(i => i.PurUnitPrice * i.BankingRate),
                    // PurUnitPrice = (ms.Amount == 0 || ms.PCLQty == 0) ? 0 : ms.Amount / ms.PCLQty,
                    // BankingRate = ms.ExchangeRate,
                    BankingRate = 1,
                    PurDollarCodeId = ms.StockDollarCodeId,
                    StockDollarCodeId = ms.StockDollarCodeId,
                    Remark = "分批領料出庫(批量)",
                    RefNo = "",
                    StockQty = ms.PCLQty,    //庫存數
                    UsageQty = ms.RealStockOutQty,   //領用數
                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    WarehouseNo = ms.WarehouseNo,

                    SemiGoods = m.SemiGoods,

                    DailyNo = (string?)mps.DailyNo,
                    MPSQty = (decimal?)(mps.TotalUsage), // 派工數量
                    PCLIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    PurIOQty = 0 - (mps != null ? (decimal?)(mps.TotalUsage) : ms.PCLQty),
                    OrgUnitId = (decimal?)mps.OrgUnitId,
                    OrgUnitNameTw = (string?)mps.OrgUnitNameTw,
                    OrgUnitNameEn = (string?)mps.OrgUnitNameEn,
                    MPSProcessId = (decimal?)(mps.MPSProcessId),
                    MPSProcessNameTw = (string?)mps.MPSProcessNameTw,
                    MPSProcessNameEn = (string?)mps.MPSProcessNameEn,
                    RefUserName = "",
                    MaterialStockId = (decimal?)ms.Id,
                    PrePCLQty = (decimal?)ms.PCLQty,
                    PreAmount = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    MPSDailyId = (decimal?)mps.Id,
                    MPSDailyLocaleId = (decimal?)mps.LocaleId,
                }
            )
            .Select(i => new Models.Views.MaterialStockItem
            {
                Id = 0,
                LocaleId = i.LocaleId,
                IODate = DateTime.Now.Date,
                SourceType = 1,
                MaterialId = i.MaterialId,
                WarehouseId = i.WarehouseId,
                OrderNo = i.OrderNo,
                PCLUnitCodeId = i.PCLUnitCodeId,
                TransRate = i.TransRate,
                PurUnitCodeId = i.PurUnitCodeId,
                ReceivedLogId = i.ReceivedLogId,
                PurUnitPrice = i.PurUnitPrice ?? 0,
                BankingRate = (decimal)i.BankingRate,
                PurDollarCodeId = (decimal)i.PurDollarCodeId,
                StockDollarCodeId = i.StockDollarCodeId,
                Remark = i.Remark,
                RefNo = i.OrderNo,

                StockQty = i.StockQty,    //庫存數
                UsageQty = i.UsageQty,   //領用數
                MPSQty = i.MPSQty,// 派工數量

                PCLIOQty = 0 - i.StockQty,
                PurIOQty = 0 - i.StockQty,

                OrgUnitId = i.OrgUnitId,
                OrgUnitNameTw = i.OrgUnitNameTw,
                OrgUnitNameEn = i.OrgUnitNameEn ?? "",

                MPSProcessId = i.MPSProcessId,
                MPSProcessNameTw = i.MPSProcessNameTw,
                MPSProcessNameEn = i.MPSProcessNameEn,

                RefUserName = i.RefUserName,
                MaterialStockId = i.MaterialStockId ?? 0,
                PrePCLQty = i.PrePCLQty ?? 0,
                PreAmount = i.PreAmount,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = DateTime.Now,

                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
                WarehouseNo = i.WarehouseNo,

                DailyId = i.MPSDailyId,
                DailyNo = i.DailyNo,
                SeqNo = 2,
                SemiGoods = i.SemiGoods,
            });

            return materialStocks;
        }
    }
}
