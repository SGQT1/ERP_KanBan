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

namespace ERP.Services.Business
{
    public class InspectLogService : BusinessService
    {
        private ERP.Services.Business.Entities.InspectLogService InspectLog { get; set; }
        private ERP.Services.Business.Entities.InspectLogSizeItemService InspectLogSizeItem { get; set; }
        private ERP.Services.Business.Entities.POItemService POItem { get; set; }
        private ERP.Services.Business.Entities.POItemSizeService POItemSize { get; set; }
        private ERP.Services.Business.Entities.TransferService Transfer { get; set; }
        private ERP.Services.Business.Entities.TransferItemService TransferItem { get; set; }
        private ERP.Services.Business.Entities.TransferSizeItemService TransferSizeItem { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.ExchangeRateService ExchangeRate { get; set; }

        private ERP.Services.Business.MaterialStockService _MaterialStock { get; set; }
        private ERP.Services.Business.Entities.MaterialStockService MaterialStock { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemSizeService MaterialStockItemSize { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemPOService MaterialStockItemPO { get; set; }
        private ERP.Services.Business.Entities.ReceivedLogService _ReceivedLog { get; set; }
        private ERP.Services.Business.Entities.VendorService Vendor { get; set; }


        public InspectLogService(
            ERP.Services.Business.Entities.InspectLogService receivedLogService,
            ERP.Services.Business.Entities.InspectLogSizeItemService receivedLogSizeItemService,
            ERP.Services.Business.Entities.POItemService poItemService,
            ERP.Services.Business.Entities.POItemSizeService poItemSizeService,
            ERP.Services.Business.Entities.TransferService transferService,
            ERP.Services.Business.Entities.TransferItemService transferItemService,
            ERP.Services.Business.Entities.TransferSizeItemService transferSizeItemService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.MaterialService materialService,
            ERP.Services.Business.MaterialStockService _materialStockService,
            ERP.Services.Business.Entities.MaterialStockService materialStockService,
            ERP.Services.Business.Entities.MaterialStockItemService materialStockItemService,
            ERP.Services.Business.Entities.MaterialStockItemSizeService materialStockItemSizeService,
            ERP.Services.Business.Entities.MaterialStockItemPOService materialStockItemPOService,
            ERP.Services.Business.Entities.ExchangeRateService exchangeRateService,
            ERP.Services.Business.Entities.VendorService vendorService,
            ERP.Services.Business.Entities.ReceivedLogService _receivedLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            InspectLog = receivedLogService;
            InspectLogSizeItem = receivedLogSizeItemService;
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
            MaterialStockItemPO = materialStockItemPOService;
            ExchangeRate = exchangeRateService;
            Vendor = vendorService;
            _ReceivedLog = _receivedLogService;
        }

        public ERP.Models.Views.InspectLogGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.InspectLogGroup { };
            var inspectLog = InspectLog.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (inspectLog != null)
            {
                var sizeItem = InspectLogSizeItem.Get().Where(i => i.ReceivedLogId == inspectLog.Id && i.LocaleId == inspectLog.LocaleId).OrderBy(i => i.SeqNo).ToList();
                var poitem = POItem.Get().Where(i => i.Id == inspectLog.POItemId && i.LocaleId == inspectLog.RefLocaleId).FirstOrDefault();
                group.InspectLog = inspectLog;
                group.ReceivedLog = new ReceivedLog
                {
                    Id = inspectLog.Id,
                    LocaleId = inspectLog.LocaleId,
                    RefLocaleId = inspectLog.RefLocaleId,
                    ReceivedDate = inspectLog.ReceivedDate,
                    ShippingListNo = inspectLog.ShippingListNo,
                    ShippingListVendorId = inspectLog.ShippingListVendorId,
                    ShippingListVendorName = inspectLog.ShippingListVendorName,
                    POItemId = inspectLog.POItemId,
                    UnitPrice = inspectLog.UnitPrice,
                    ReceivedQty = inspectLog.ReceivedQty,
                    SubTotalPrice = inspectLog.SubTotalPrice,
                    Remark = inspectLog.Remark,
                    APId = inspectLog.APId,
                    QCDate = inspectLog.QCDate,
                    IQCFlag = inspectLog.IQCFlag,
                    IQCGetQty = inspectLog.IQCGetQty,
                    IQCTestQty = inspectLog.IQCTestQty,
                    IQCPassQty = inspectLog.IQCPassQty,
                    IQCRejectionQty = inspectLog.IQCPassQty,
                    IQCResult = inspectLog.IQCResult,
                    IQCMen = inspectLog.IQCMen,
                    IQCRemark = inspectLog.IQCRemark,
                    SamplingMethod = inspectLog.SamplingMethod,
                    WarehouseId = inspectLog.WarehouseId,
                    StockQty = inspectLog.StockQty,
                    OrderNo = inspectLog.OrderNo,
                    IsAccounting = inspectLog.IsAccounting,
                    TransferInId = inspectLog.TransferInId,
                    TransferInLocaleId = inspectLog.TransferInLocaleId,
                    TaiwanInvoiceNo = inspectLog.TaiwanInvoiceNo,
                    TransferQty = inspectLog.TransferQty,
                    WeightUnitCodeId = inspectLog.WeightUnitCodeId,
                    NetWeight = inspectLog.NetWeight,
                    GrossWeight = inspectLog.GrossWeight,
                    ModifyUserName = inspectLog.ModifyUserName,
                    LastUpdateTime = inspectLog.LastUpdateTime,
                    ReceivedCount = inspectLog.ReceivedCount,
                    ReceivedLogId = inspectLog.ReceivedLogId,
                    RefPONo = inspectLog.RefPONo,
                    Type = inspectLog.Type,
                    MaterialId = inspectLog.MaterialId,
                    MaterialNameTw = inspectLog.MaterialNameTw,
                    ParentMaterialId = inspectLog.ParentMaterialId,
                    ParentMaterialNameTw = inspectLog.ParentMaterialNameTw,
                    PCLUnitCodeId = inspectLog.PCLUnitCodeId,
                    PCLUnitNameTw = inspectLog.PCLUnitNameTw,
                    PurUnitCodeId = inspectLog.PurUnitCodeId,
                    PurUnitNameTw = inspectLog.PurUnitNameTw,
                    PayQty = inspectLog.PayQty,
                    FreeQty = inspectLog.FreeQty,
                    TotalQty = inspectLog.TotalQty,
                    PurDollarCodeId = inspectLog.PurDollarCodeId,
                    PurDollarNameTw = inspectLog.PurDollarNameTw,
                    StockDollarCodeId = inspectLog.StockDollarCodeId,
                    StockDollarNameTw = inspectLog.StockDollarNameTw,
                    ReceivedBarcode = inspectLog.ReceivedBarcode,
                    TransferInLocale = inspectLog.TransferInLocale,
                    CloseMonth = inspectLog.CloseMonth,
                    POType = inspectLog.POType,
                    ReceivedType = inspectLog.ReceivedType,
                    MaterialNameEng = inspectLog.MaterialNameEng,
                    WarehouseNo = inspectLog.WarehouseNo,
                };
                group.InspectLogSizeItem = sizeItem;
                group.POItem = poitem == null ? new POItem { } : poitem;

                //判斷要不要顯示QC收貨跟直接入庫
                group.SaveReceivedLogOption = (group.POItem.PurLocaleId != inspectLog.LocaleId) ?
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
                    SaveQCResult = group.ReceivedLog.IQCResult == 2 ? true : false,
                    SaveStockIn = group.ReceivedLog.IQCResult == 2 ? true : false,
                    SaveStockOut = group.ReceivedLog.IQCResult == 2 ? true : false,

                };
            }
            return group;
        }

        public ERP.Models.Views.InspectLogGroup Save(InspectLogGroup item)
        {
            var type = item.SaveReceivedLogOption.ReceivedType;
            var saveQCResult = item.SaveReceivedLogOption.SaveQCResult;
            var saveStockIn = item.SaveReceivedLogOption.SaveStockIn;
            var saveStockOut = item.SaveReceivedLogOption.SaveStockOut;

            var poItem = item.POItem;
            var materialStock = new ERP.Models.Views.MaterialStock();

            // var ioDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000"));
            var ioDate = item.ReceivedLog.ReceivedDate;
            var stockCurrency = "USD";

            var inspectLog = item.InspectLog;
            var inspectLogSizeItem = item.InspectLogSizeItem;
            var _inspectLog = InspectLog.Get().Where(i => i.Id == inspectLog.Id && i.LocaleId == inspectLog.LocaleId).FirstOrDefault();

            try
            {
                UnitOfWork.BeginTransaction();

                var exchangeRate = (decimal)0;
                var exchangeItems = new List<Models.Views.ExchangeRate>();

                // var exchangeRate = poItem.Currency == "USD" ? 1 :
                //     ExchangeRate.Get().Where(i => i.ExchDate <= ioDate && i.CurrencyTw == stockCurrency && i.TransCurrencyTw == poItem.Currency).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).First();

                if (_inspectLog != null)
                {
                    _inspectLog.QCDate = inspectLog.QCDate;
                    _inspectLog.SamplingMethod = inspectLog.SamplingMethod;
                    _inspectLog.IQCMen = inspectLog.IQCMen;
                    _inspectLog.IQCResult = inspectLog.IQCResult;
                    _inspectLog.IQCFlag = inspectLog.IQCFlag;
                    _inspectLog.SubTotalPrice = inspectLog.SubTotalPrice;
                    _inspectLog.IQCTestQty = inspectLog.IQCTestQty;
                    _inspectLog.IQCPassQty = inspectLog.IQCPassQty;
                    _inspectLog.IQCRejectionQty = inspectLog.IQCRejectionQty;
                    _inspectLog.PayQty = inspectLog.PayQty;
                    _inspectLog.FreeQty = inspectLog.FreeQty;

                    InspectLog.Update(_inspectLog);

                    if (_inspectLog.Id != 0)
                    {
                        InspectLogSizeItem.RemoveRange(i => i.ReceivedLogId == inspectLog.Id && i.LocaleId == inspectLog.LocaleId);
                        InspectLogSizeItem.CreateRange(inspectLogSizeItem);
                    }
                }

                if (saveStockIn)
                {
                    // 匯率的建法，只有美金對全部幣別，還有全部幣別對台幣。所以最好的方式是把資料一次都抓出來(最近出庫日的那天所有幣別)。
                    var _exchDate = ExchangeRate.Get().Where(i => i.ExchDate <= ioDate).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate);
                    exchangeItems = ExchangeRate.Get().Where(i => i.ExchDate == _exchDate).OrderByDescending(i => i.ExchDate).ToList();

                    // stpe2-1: 有沒有MaterialaStock,沒有就新增一筆
                    materialStock = MaterialStock.Get()
                        .Where(i => i.OrderNo == inspectLog.OrderNo && i.LocaleId == inspectLog.LocaleId &&
                                    i.MaterialId == inspectLog.MaterialId && i.ParentMaterialId == 0 &&
                                    i.WarehouseId == inspectLog.WarehouseId && i.PCLUnitCodeId == inspectLog.PurUnitCodeId).FirstOrDefault();


                    if (materialStock == null)
                    {
                        // 新資料，庫存幣別一律為USD, 所以抓的是UST to Others, 用ReversedBankingRate
                        exchangeRate = poItem.Currency == "USD" ? 1 :
                        exchangeItems.Where(i => i.CurrencyTw == stockCurrency && i.TransCurrencyTw == poItem.Currency).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault();
                        materialStock = MaterialStock.Create(new ERP.Models.Views.MaterialStock
                        {
                            LocaleId = inspectLog.LocaleId,
                            MaterialId = inspectLog.MaterialId,
                            MaterialName = inspectLog.MaterialNameTw,
                            MaterialNameEng = inspectLog.MaterialNameEng,
                            WarehouseId = inspectLog.WarehouseId,
                            WarehouseNo = inspectLog.WarehouseNo,
                            OrderNo = inspectLog.OrderNo,
                            PCLUnitCodeId = inspectLog.PCLUnitCodeId,
                            PCLUnitNameTw = inspectLog.PCLUnitNameTw,
                            PCLUnitNameEn = inspectLog.PCLUnitNameTw,
                            TransRate = 1,
                            PurUnitCodeId = inspectLog.PurUnitCodeId,
                            PurUnitNameTw = inspectLog.PurUnitNameTw,
                            PurUnitNameEn = inspectLog.PurUnitNameTw,
                            PCLPlanQty = inspectLog.PlanQty,
                            PCLQty = inspectLog.IQCGetQty,
                            PurQty = inspectLog.ReceivedQty,
                            PCLAllocationQty = 0,
                            PurAllocationQty = 0,
                            Amount = (inspectLog.PayQty) * inspectLog.UnitPrice,
                            StockDollarCodeId = (decimal)inspectLog.StockDollarCodeId,
                            StockDollarNameTw = inspectLog.StockDollarNameTw,
                            StockDollarNameEn = inspectLog.StockDollarNameTw,
                            ParentMaterialId = 0,
                            ParentMaterialNameTw = "",
                            ParentMaterialNameEn = "",
                            LastStockIOId = 0,
                            ModifyUserName = inspectLog.ModifyUserName,
                            PurUnitPrice = inspectLog.UnitPrice,
                            // PurDollarCodeId = inspectLog.PurDollarCodeId,
                            // PurDollarNameEn = inspectLog.PurUnitNameTw,
                            // PurDollarNameTw = inspectLog.PurUnitNameTw,
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

                    // 判斷有沒有這筆收貨有沒有StockIO，先取會全部的StockIO, 入如果裡面有相同的 receivedLogId, 就不處理入庫(就是只更新收貨)
                    var stockItems = MaterialStockItem.Get().Where(i => i.MaterialStockId == materialStock.Id && i.LocaleId == materialStock.LocaleId).ToList();
                    var _exisStockIn = stockItems.Where(i => i.ReceivedLogId == inspectLog.Id).FirstOrDefault();

                    var _purUnitPrice = _inspectLog.UnitPrice;
                    var _purDollarCodeId = _inspectLog.PurDollarCodeId;
                    var _purUnitPriceLog = "";

                    // dbo.ProcessPO.POItemId, dbo.POItem.Id, dbo.ProcessPO.LocaleId, dbo.POItem.LocaleId, dbo.POItem.Qty, dbo.ProcessPO.MaterialCost, dbo.ProcessPO.Id
                    var _stockInPOs = MaterialStockItemPO.Get().Where(i => i.POItemId == poItem.Id && i.LocaleId == poItem.LocaleId).ToList();

                    //拖外加工單價需要在加上原材料單價
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
                        });
                        var _stockInPO = _stockInPOs.GroupBy(i => new { i.POItemId, i.LocaleId, i.PurQty, i.StockOutQty }).Select(i => new { i.Key.POItemId, i.Key.LocaleId, PurQty = i.Key.PurQty, StockOutQty = i.Key.StockOutQty, MaterialCost = i.Sum(g => g.MaterialCost) }).First();
                        // 貼合單價 = 收貨單價＋原材料拖外出庫的單價
                        // _purUnitPrice = _stockInPO == null ? _purUnitPrice : (decimal)((_inspectLog.UnitPrice * exchangeRate) + (_stockInPO.MaterialCost / _stockInPO.PurQty)); // 用採購數量：選錯拖外採購單的雖然金額錯，但出庫的領料金額應該會一樣
                        _purUnitPrice = _stockInPO == null ? _purUnitPrice : (decimal)((_inspectLog.UnitPrice * exchangeRate) + (_stockInPO.MaterialCost / _stockInPO.StockOutQty));  // 這裡改成用總金額除拖外出庫的數量不是採購單數量，因為倉庫很容易選錯拖外採購單，所以直接用出庫金額/出庫數量
                        
                        _purDollarCodeId = _stockInPO == null ? _purDollarCodeId : materialStock.StockDollarCodeId;
                        _purUnitPriceLog = string.Format("P:{0:F6}+M:{1:F6}", poItem.UnitPrice * exchangeRate, _stockInPO.MaterialCost /  _stockInPO.StockOutQty); //拖外記錄單價組成，這時候exchangeRate是POItem的

                        exchangeRate = _stockInPO == null ? exchangeRate : 1; //如果有拖外出庫，這時候exchangeRate改為1

                        
                    }

                    if (_exisStockIn == null) // 如果已經有StockIO, 就跳過不新增        
                    {
                        var sumQty = stockItems.Sum(s => s.PCLIOQty);
                        var maxSeq = stockItems.Max(s => s.SeqNo) == null ? 0 : stockItems.Max(s => s.SeqNo);
                        var stockIn = new MaterialStockItem()
                        {
                            LocaleId = materialStock.LocaleId,
                            IODate = (DateTime)_inspectLog.QCDate,
                            SourceType = 0,
                            MaterialId = materialStock.MaterialId,
                            WarehouseId = materialStock.WarehouseId,
                            OrderNo = materialStock.OrderNo,
                            PCLUnitCodeId = materialStock.PCLUnitCodeId,
                            TransRate = 1,
                            PurUnitCodeId = materialStock.PurUnitCodeId,
                            // PCLIOQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                            // PurIOQty = (receivedLog.TotalQty - receivedLog.FreeQty) * 1,
                            PCLIOQty = inspectLog.IQCGetQty,
                            PurIOQty = inspectLog.ReceivedQty,
                            ReceivedLogId = inspectLog.Id,
                            // PurUnitPrice = inspectLog.UnitPrice,
                            // PurDollarCodeId = inspectLog.PurDollarCodeId,
                            PurUnitPrice = _purUnitPrice,//receivedLog.UnitPrice,
                            PurDollarCodeId = _purDollarCodeId,//receivedLog.PurDollarCodeId,
                            BankingRate = exchangeRate,
                            StockDollarCodeId = materialStock.StockDollarCodeId,
                            Remark = inspectLog.TransferInId == 0 ? "來料(QC入庫)" : "轉廠來料(QC入庫)",
                            RefNo = "QCA-" + inspectLog.Id,
                            OrgUnitId = 0,
                            OrgUnitNameTw = "",
                            OrgUnitNameEn = "",
                            MPSProcessId = 0,
                            MPSProcessNameTw = "",
                            MPSProcessNameEn = _purUnitPriceLog,    // 拖外單價
                            RefUserName = inspectLog.ModifyUserName,
                            MaterialStockId = materialStock.Id,
                            PrePCLQty = sumQty,
                            PreAmount = 0,
                            ModifyUserName = inspectLog.ModifyUserName,
                            SeqNo = maxSeq + 1
                        };
                        _exisStockIn = MaterialStockItem.Create(stockIn);
                    }
                    else
                    {
                        // 取新單價
                        decimal _amount = _exisStockIn.PCLIOQty * _exisStockIn.PurUnitPrice * _exisStockIn.BankingRate / _exisStockIn.TransRate,
                                _pclQty = _exisStockIn.PCLIOQty,
                                _avgPrice = _purUnitPrice;

                        var items = MaterialStockItem.Get().Where(i => i.MaterialStockId == _exisStockIn.Id && i.LocaleId == _exisStockIn.LocaleId && i.PCLIOQty > 0 && i.IODate <= _exisStockIn.IODate).Select(i => new { i.Id, i.PurDollarCodeId, i.PCLIOQty, i.PurUnitPrice, i.BankingRate, i.TransRate }).ToList();
                        if (items.Any() && _exisStockIn.PCLIOQty < 0)
                        {
                            // _purDollarCodeId = items.OrderBy(i => i.Id).Select(i => i.PurDollarCodeId).FirstOrDefault();
                            _purDollarCodeId = _exisStockIn.StockDollarCodeId;
                            _amount = items.Sum(s => s.PCLIOQty * s.PurUnitPrice * s.BankingRate / s.TransRate);
                            _pclQty = items.Sum(s => s.PCLIOQty);
                            _avgPrice = _amount == 0 || _pclQty == 0 ? 0 : _amount / _pclQty;
                        }

                        _exisStockIn.IODate = (DateTime)_inspectLog.QCDate;  
                        _exisStockIn.PCLIOQty = inspectLog.IQCGetQty;
                        _exisStockIn.PurIOQty = inspectLog.ReceivedQty;
                        _exisStockIn.ModifyUserName = inspectLog.ModifyUserName;

                        _exisStockIn.BankingRate = exchangeRate;
                        _exisStockIn.PurUnitPrice = _avgPrice;
                        _exisStockIn.PurDollarCodeId = _purDollarCodeId;
                        _exisStockIn.MPSProcessNameEn = _purUnitPriceLog;    // 拖外單價

                        _exisStockIn = MaterialStockItem.Update(_exisStockIn);
                    }

                    if (inspectLogSizeItem.Count() > 0)
                    {
                        MaterialStockItemSize.RemoveRange(i => i.StockIOId == _exisStockIn.Id && i.LocaleId == _exisStockIn.LocaleId);
                        var stockInSize = InspectLogSizeItem.Get().Where(i => i.ReceivedLogId == inspectLog.Id && i.LocaleId == inspectLog.LocaleId)
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

                    // 更新收貨的庫存總數
                    if (_inspectLog.Id != 0)
                    {
                        _ReceivedLog.UpdateStockQty(new List<decimal> { _inspectLog.Id }, _inspectLog.LocaleId);
                    }
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }


            return Get((int)inspectLog.Id, (int)inspectLog.LocaleId);
        }

        public ERP.Models.Views.InspectLogGroup Remove(InspectLogGroup item)
        {
            var inspectLog = item.InspectLog;
            var inspectLogSizeItem = item.InspectLogSizeItem;

            var _inspectLog = InspectLog.Get().Where(i => i.Id == inspectLog.Id && i.LocaleId == inspectLog.LocaleId).FirstOrDefault();

            if (_inspectLog != null)
            {
                _inspectLog.IQCPassQty = 0;
                _inspectLog.IQCRejectionQty = 0;

                _inspectLog.QCDate = null;
                _inspectLog.SamplingMethod = 0;
                _inspectLog.IQCMen = "";
                _inspectLog.IQCResult = 0;
                _inspectLog.IQCFlag = 0;
                _inspectLog.PayQty = 0;
                _inspectLog.FreeQty = 0;
                _inspectLog.IQCTestQty = 0;

                InspectLog.Remove(_inspectLog);

                if (_inspectLog.Id != 0)
                {
                    InspectLogSizeItem.RemoveRange(i => i.ReceivedLogId == inspectLog.Id && i.LocaleId == inspectLog.LocaleId);
                    InspectLogSizeItem.CreateRange(inspectLogSizeItem);
                }
            }

            return Get((int)inspectLog.Id, (int)inspectLog.LocaleId);
        }

    }
}
