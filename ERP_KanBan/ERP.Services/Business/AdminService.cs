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

namespace ERP.Services.Business
{
    public class AdminService : BusinessService
    {
        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }
        private ERP.Services.Business.Entities.MaterialStockService MaterialStock { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        private ERP.Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; set; }
        private ERP.Services.Entities.ProcessPOService ProcessPO { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.ExchangeRateService ExchangeRate { get; set; }
        private ERP.Services.Entities.StockIOService StockIO { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }

        private ERP.Services.Entities.MaterialStockService _MaterialStock { get; }

        public AdminService(
            ERP.Services.Business.Entities.MaterialStockItemService materialStockItemService,
            ERP.Services.Business.Entities.MaterialStockService materialStockService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.ReceivedLogAddService receivedLogAddService,
            ERP.Services.Entities.ProcessPOService processPOService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.ExchangeRateService exchangeRateService,
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.MaterialStockService _materialStockService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MaterialStockItem = materialStockItemService;
            ReceivedLog = receivedLogService;
            ReceivedLogAdd = receivedLogAddService;
            ProcessPO = processPOService;
            POItem = poItemService;
            ExchangeRate = exchangeRateService;
            StockIO = stockIOService;
            CodeItem = codeItemService;
            MaterialStock = materialStockService;
            _MaterialStock = _materialStockService;
        }

        public void UpdateStockIOUnitPrice(int localeId, int? id)
        {
            var _id = id ?? 0;
            DateTime startTime = new DateTime(2024, 10, 1);
            var sourceType = 0;

            var items = (from si in MaterialStockItem.Get()
                         join r in ReceivedLog.Get() on new { si.ReceivedLogId, si.LocaleId } equals new { ReceivedLogId = (decimal?)r.Id, r.LocaleId }
                         join p in ProcessPO.Get().Select(i => new { i.POItemId, i.LocaleId }).Distinct()
                              on new { r.POItemId, r.LocaleId } equals new { p.POItemId, p.LocaleId }
                         where si.LocaleId == localeId && sourceType == 0 && si.PCLIOQty > 0 && si.Id >= _id
                         orderby si.Id
                         select new { si.Id, si.MaterialStockId, si.LocaleId, p.POItemId }).ToList();

            var totalRecords = items.Count();
            var currentRecords = 0;
            int batchSize = 200;

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                var batchItems = items.Skip(i).Take(batchSize).ToList();

                try
                {
                    UnitOfWork.BeginTransaction();

                    foreach (var item in batchItems)
                    {
                        currentRecords++;
                        Console.WriteLine($"[{currentRecords}/{totalRecords}] Processing SId:{item.Id}, MSId:{item.MaterialStockId}, POId:{item.POItemId}");

                        var stockIO = MaterialStockItem.Get().First(s => s.Id == item.Id && s.LocaleId == item.LocaleId);
                        var poItem = POItem.Get().Where(p => p.Id == item.POItemId && p.LocaleId == item.LocaleId)
                            .Select(p => new
                            {
                                p.Id,
                                p.Qty,
                                p.UnitPrice,
                                PurCurrency = CodeItem.Get().Where(c => c.Id == p.DollarCodeId && c.LocaleId == localeId).Max(c => c.NameTW)
                            }).First();

                        var oPOs = ProcessPO.Get().Where(p => p.POItemId == item.POItemId && p.LocaleId == item.LocaleId).ToList();
                        if (stockIO == null || !oPOs.Any()) continue;

                        var stockCurrency = stockIO.StockCurrency;
                        var exchangeDate = GetLatestExchangeDate(stockCurrency, poItem.PurCurrency, stockIO.IODate);
                        var exchangeRates = ExchangeRate.Get().Where(e => e.ExchDate == exchangeDate).ToList();
                        var exchangeRate = GetExchangeRate(stockCurrency, poItem.PurCurrency, exchangeRates);

                        var usdToNtd = exchangeRates.FirstOrDefault(e => e.NameTw == "USD" && e.TransNameTw == "NTD");
                        foreach (var opo in oPOs)
                        {
                            if (stockIO.StockDollarCodeId != opo.StockDollarCodeId)
                            {
                                opo.MaterialCost = stockCurrency == "USD" ? opo.MaterialCost * usdToNtd.ReversedBankingRate : opo.MaterialCost * usdToNtd.BankingRate;
                            }
                        }

                        var stockInPO = oPOs.GroupBy(p => new { p.POItemId, p.LocaleId })
                            .Select(g => new { g.Key.POItemId, g.Key.LocaleId, MaterialCost = g.Sum(x => x.MaterialCost) })
                            .FirstOrDefault();

                        var purUnitPrice = (decimal)((poItem.UnitPrice * exchangeRate) + (stockInPO?.MaterialCost ?? 0) / poItem.Qty);
                        var purUnitPriceLog = string.Format("P:{0:F6}+M:{1:F6}", poItem.UnitPrice * exchangeRate, (stockInPO?.MaterialCost ?? 0) / poItem.Qty);

                        StockIO.UpdateRange(
                            s => s.Id == stockIO.Id && s.LocaleId == stockIO.LocaleId,
                            // u => new Models.Entities.StockIO
                            // {
                            //     PurDollarCodeId = stockIO.StockDollarCodeId,
                            //     PurUnitPrice = purUnitPrice,
                            //     BankingRate = stockInPO != null ? 1 : exchangeRate,
                            //     PreAmount = (decimal)poItem.UnitPrice,
                            //     PrePCLQty = exchangeRate,
                            //     MPSProcessNameEn = purUnitPriceLog
                            // }
                            u => u.SetProperty(p => p.PurDollarCodeId, p => stockIO.StockDollarCodeId).SetProperty(p => p.PurUnitPrice, p => purUnitPrice)
                                              .SetProperty(p => p.BankingRate, p => stockInPO != null ? 1 : exchangeRate).SetProperty(p => p.PreAmount, p => (decimal)poItem.UnitPrice)
                                              .SetProperty(p => p.PrePCLQty, p => exchangeRate).SetProperty(p => p.MPSProcessNameEn, p => purUnitPriceLog)
                        );

                        MaterialStock.UpdatStockQty((int)stockIO.MaterialStockId, (int)stockIO.LocaleId);
                    }

                    UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Batch {i / batchSize + 1} failed: {ex.Message}");
                    UnitOfWork.Rollback();
                }
            }
        }

        private DateTime GetLatestExchangeDate(string from, string to, DateTime baseDate)
        {
            if (from == "USD")
                return ExchangeRate.Get().Where(e => e.ExchDate <= baseDate && e.NameTw == from && e.TransNameTw == to)
                    .OrderByDescending(e => e.ExchDate).Select(e => e.ExchDate).First();
            else
                return ExchangeRate.Get().Where(e => e.ExchDate <= baseDate && e.NameTw == to && e.TransNameTw == from)
                    .OrderByDescending(e => e.ExchDate).Select(e => e.ExchDate).First();
        }

        private decimal GetExchangeRate(string from, string to, List<ERP.Models.Entities.ExchangeRate> rates)
        {
            if (from == to) return 1;
            if (from == "USD") return rates.First(e => e.NameTw == from && e.TransNameTw == to).ReversedBankingRate;
            else return rates.First(e => e.NameTw == to && e.TransNameTw == from).BankingRate;
        }


        // 更新拖外加工單價
        public void UpdateStockIOUnitPrice1(int localeId, int? id)
        {
            UnitOfWork.BeginTransaction();

            var _id = id != null ? id : 0;

            DateTime startTime = new DateTime(2024, 10, 1, 0, 0, 0);
            var sourceType = 0;
            var ids = new List<decimal> { 6245473 }; // for test
            var items = (
                from si in MaterialStockItem.Get()
                join r in ReceivedLog.Get() on new { ReceivedLogId = si.ReceivedLogId, LocaleId = si.LocaleId } equals new { ReceivedLogId = (decimal?)r.Id, LocaleId = r.LocaleId }
                join p in ProcessPO.Get().Select(i => new { i.POItemId, i.LocaleId }).Distinct() on new { POItemId = r.POItemId, LocaleId = r.LocaleId } equals new { POItemId = p.POItemId, LocaleId = p.LocaleId }
                // where si.LocaleId == localeId && si.LastUpdateTime >= startTime && sourceType == 0 && si.PCLIOQty > 0 && si.Id > _id
                where si.LocaleId == localeId && sourceType == 0 && si.PCLIOQty > 0 && si.Id >= _id
                select new
                {
                    si.Id,
                    si.MaterialStockId,
                    si.LocaleId,
                    p.POItemId
                })
                // .Where(i => i.Id > _id)
                // .Where(i => ids.Contains(i.Id))
                // .Where(i => ids.Contains(i.MaterialStockId))
                .OrderBy(i => i.Id)
                .ToList();

            var totalRecords = items.Count();
            var currentRecords = 0;


            items.ForEach(i =>
            {
                currentRecords += 1;

                var stockIO = MaterialStockItem.Get().Where(s => s.Id == i.Id && s.LocaleId == i.LocaleId).First();
                var poItem = POItem.Get().Where(p => p.Id == i.POItemId && p.LocaleId == i.LocaleId)
                            .Select(i => new
                            {
                                i.Id,
                                i.Qty,
                                i.UnitPrice,
                                PurCurrency = CodeItem.Get().Where(c => c.Id == i.DollarCodeId && c.LocaleId == localeId).Max(c => c.NameTW)
                            })
                            .First();
                var oPOs = ProcessPO.Get().Where(p => p.POItemId == i.POItemId && p.LocaleId == i.LocaleId).ToList();

                var _stockCurrency = stockIO.StockCurrency;

                if (stockIO != null)
                {
                    Console.WriteLine(">>> {0}/{1}(SId:{2}, MSId:{3}, PId:{4}) : Start", currentRecords, totalRecords, i.Id, i.MaterialStockId, i.POItemId);
                    var _exchDate = startTime;

                    if (_stockCurrency == "USD")
                    {
                        _exchDate = ExchangeRate.Get().Where(i => i.ExchDate <= stockIO.IODate && i.NameTw == _stockCurrency && i.TransNameTw == poItem.PurCurrency).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate);
                    }
                    else
                    {
                        _exchDate = ExchangeRate.Get().Where(i => i.ExchDate <= stockIO.IODate && i.NameTw == poItem.PurCurrency && i.TransNameTw == _stockCurrency).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate);
                    }

                    var exchangeItems = ExchangeRate.Get().Where(i => i.ExchDate == _exchDate).OrderByDescending(i => i.ExchDate).ToList();
                    var _purUnitPrice = stockIO.PurUnitPrice;

                    var exchangeRate = poItem.PurCurrency == _stockCurrency ? 1 :
                        _stockCurrency == "USD" ? exchangeItems.Where(i => i.NameTw == _stockCurrency && i.TransNameTw == poItem.PurCurrency).Select(i => i.ReversedBankingRate).FirstOrDefault() :
                        exchangeItems.Where(i => i.NameTw == poItem.PurCurrency && i.TransNameTw == _stockCurrency).Select(i => i.BankingRate).FirstOrDefault();

                    try
                    {
                        if (oPOs.Any())
                        {
                            var _usdTontd = exchangeItems.Where(i => i.ExchDate <= stockIO.IODate && i.NameTw == "USD" && i.TransNameTw == "NTD").OrderByDescending(i => i.ExchDate).First();
                            // 新資料，庫存幣別一律為USD, 所以抓的是UST to Others, 用ReversedBankingRate
                            oPOs.ForEach(i =>
                            {
                                if (stockIO.StockDollarCodeId != i.StockDollarCodeId)
                                {
                                    i.MaterialCost = stockIO.StockCurrency == "USD" ? i.MaterialCost * _usdTontd.ReversedBankingRate : i.MaterialCost * _usdTontd.BankingRate;
                                }
                            });

                            var _stockInPO = oPOs.GroupBy(i => new { i.POItemId, i.LocaleId }).Select(i => new { i.Key.POItemId, i.Key.LocaleId, MaterialCost = i.Sum(g => g.MaterialCost) }).First();
                            // 貼合單價 = 收貨單價＋原材料拖外出庫的單價
                            _purUnitPrice = _stockInPO == null ? _purUnitPrice : (decimal)((poItem.UnitPrice * exchangeRate) + (_stockInPO.MaterialCost / poItem.Qty));
                            var _exchangeRate = _stockInPO == null ? exchangeRate : 1;

                            var _purUnitPriceLog = string.Format("P:{0:F6}+M:{1:F6}", poItem.UnitPrice * exchangeRate, _stockInPO.MaterialCost / poItem.Qty);

                            StockIO.UpdateRange(
                                i => i.Id == stockIO.Id && i.LocaleId == stockIO.LocaleId,
                                // u => new Models.Entities.StockIO { PurDollarCodeId = stockIO.StockDollarCodeId, PurUnitPrice = _purUnitPrice, BankingRate = _exchangeRate, PreAmount = (decimal)poItem.UnitPrice, PrePCLQty = exchangeRate, MPSProcessNameEn = _purUnitPriceLog }
                                u => u.SetProperty(p => p.PurDollarCodeId, p => stockIO.StockDollarCodeId).SetProperty(p => p.PurUnitPrice, p => _purUnitPrice).SetProperty(p => p.BankingRate, p => _exchangeRate).SetProperty(p => p.PreAmount, p => (decimal)poItem.UnitPrice).SetProperty(p => p.PrePCLQty, p => exchangeRate).SetProperty(p => p.MPSProcessNameEn, p => _purUnitPriceLog)
                            );

                            // StockIO.UpdateRange(
                            //     i => i.PCLIOQty < 0 && i.MaterialStockId == stockIO.MaterialStockId && i.LocaleId == stockIO.LocaleId && i.IODate >= stockIO.IODate,
                            //     u => new Models.Entities.StockIO { PurDollarCodeId = stockIO.StockDollarCodeId, PurUnitPrice = _purUnitPrice, BankingRate = _exchangeRate, PreAmount = (decimal)poItem.UnitPrice, PrePCLQty = exchangeRate }
                            // );

                            // 更新MaterialStock單價
                            MaterialStock.UpdatStockQty((int)stockIO.MaterialStockId, (int)stockIO.LocaleId);

                        }
                        UnitOfWork.Commit();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(">>> {0}/{1} : Error", currentRecords, totalRecords);
                    }
                }
            });
        }

        public void UpdateOutsourceStockOutUnitPrice(int localeId)
        {
            UnitOfWork.BeginTransaction();

            DateTime startTime = new DateTime(2024, 10, 1, 0, 0, 0);
            var sourceType = 0;
            var ids = new List<decimal> { 3716752 }; // for test
            var items = (
                from si in MaterialStockItem.Get()
                join r in ReceivedLog.Get() on new { ReceivedLogId = si.ReceivedLogId, LocaleId = si.LocaleId } equals new { ReceivedLogId = (decimal?)r.Id, LocaleId = r.LocaleId }
                join p in ProcessPO.Get().Select(i => new { i.POItemId, i.LocaleId }).Distinct() on new { POItemId = r.POItemId, LocaleId = r.LocaleId } equals new { POItemId = p.POItemId, LocaleId = p.LocaleId }
                where si.LocaleId == localeId && si.LastUpdateTime >= startTime && sourceType == 0 && si.PCLIOQty > 0
                select new
                {
                    si.MaterialStockId,
                    si.LocaleId,
                })
                // .Where(i => ids.Contains(i.Id))
                // .Where(i => ids.Contains(i.MaterialStockId))
                .OrderBy(i => i.MaterialStockId)
                .Distinct()
                .ToList();

            var totalRecords = items.Count();
            var currentRecords = 0;


            items.ForEach(ms =>
            {
                currentRecords += 1;
                try
                {
                    Console.WriteLine(">>> {0}/{1}({2}) : Start", currentRecords, totalRecords, ms.MaterialStockId);
                    // MaterialStock.UpdatStockQty((int)ms.MaterialStockId, (int)ms.LocaleId);
                    var _ms = MaterialStock.Get().Where(i => i.Id == ms.MaterialStockId && i.LocaleId == ms.LocaleId).FirstOrDefault();

                    if (_ms != null)
                    {
                        StockIO.UpdateRange(
                            i => i.PCLIOQty < 0 && i.MaterialStockId == ms.MaterialStockId && i.LocaleId == ms.LocaleId && i.IODate >= startTime,
                            // u => new Models.Entities.StockIO { PurDollarCodeId = u.StockDollarCodeId, PurUnitPrice = (decimal)_ms.AvgUnitPrice }
                            setters => setters.SetProperty(p => p.PurDollarCodeId, v => v.StockDollarCodeId).SetProperty(p => p.PurUnitPrice, p => (decimal)_ms.AvgUnitPrice)
                        );

                    }

                    UnitOfWork.Commit();

                }
                catch (Exception e)
                {
                    Console.WriteLine(">>> {0}/{1} : Error", currentRecords, totalRecords);
                }
            });
        }

        public void UpdateMaterialStockUnitPrice(int localeId, int? id)
        {
            var _id = id ?? 0;
            var startTime = new DateTime(2024, 10, 1, 0, 0, 0);
            var batchSize = 200;
            var totalProcessed = 0;

            var items = StockIO.Get()
                .Where(i => i.LocaleId == localeId && i.IODate >= startTime && i.MaterialStockId >= _id)
                .Select(i => new { i.MaterialStockId, i.LocaleId })
                .Distinct()
                .OrderBy(i => i.MaterialStockId)
                .ToList();

            var totalRecords = items.Count;

            for (int i = 0; i < totalRecords; i++)
            {
                if (i % batchSize == 0)
                {
                    // 每200筆重新啟動交易
                    UnitOfWork.BeginTransaction();
                }

                var current = items[i];
                totalProcessed++;

                Console.WriteLine($">>> {totalProcessed}/{totalRecords} (MaterialStockId: {current.MaterialStockId})");

                try
                {
                    UpdatStockQtyForBatch((int)current.MaterialStockId, (int)current.LocaleId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($">>> Error on {totalProcessed}: {ex.Message}");
                    // 可考慮記錄或儲存失敗的項目 ID
                }

                if (i % batchSize == batchSize - 1 || i == totalRecords - 1)
                {
                    // 每200筆或最後一筆時 Commit 並釋放資源
                    UnitOfWork.Commit();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            Console.WriteLine($">>> Done. Processed: {totalProcessed} items.");
        }
        public void UpdatStockQtyForBatch(int id, int localeId)
        {
            // var qty = StockIO.Get().Where(i => i.MaterialStockId == id && i.LocaleId == localeId).Sum(i => i.PCLIOQty);
            var stockQty = (decimal)0;
            var amount = (decimal?)0;
            var purQty = (decimal)0;
            var price = (decimal?)0;
            var avgPrice = (decimal?)0;
            var usage = (decimal?)0;
            var maxIO = (decimal)0;
            var _exchangeRate = (decimal)0;

            var sIOs = StockIO.Get().Where(i => i.MaterialStockId == id && i.LocaleId == localeId).ToList();
            var costSourceType = new int[] { 1, 7, 9, 12 };
            stockQty = sIOs.Sum(i => i.PCLIOQty);
            usage = sIOs.Where(i => i.PCLIOQty < 0 && costSourceType.Contains(i.SourceType)).Sum(i => i.PCLIOQty);
            maxIO = sIOs.Any() ? sIOs.OrderByDescending(i => i.IODate).ThenByDescending(i => i.Id).FirstOrDefault().Id : 0;

            // Step 1: 用入庫計算平均單價
            // 用入庫單價計算平均單價。並存入平均單價的欄位。用採購單的單價，就存入採購單的欄位
            var _amount = sIOs.Where(i => i.PCLIOQty > 0).Sum(i => i.PCLIOQty * i.PurUnitPrice * i.BankingRate);
            var _purQty = sIOs.Where(i => i.PCLIOQty > 0).Sum(i => i.PCLIOQty);
            var _price = (_amount == 0 || _purQty == 0) ? 0 : _amount / _purQty;
            avgPrice = _price;

            // Step 2: 用採購單計算單價
            // 取得這個MaterialStock 下的所有採購單
            var pos = (
                from m in MaterialStock.Get()
                join p in POItem.Get().Where(i => i.POType != 2 && i.POType != 6) on new { OrderNO = m.OrderNo, MaterialId = m.MaterialId, LocaleId = m.LocaleId } equals
                                          new { OrderNO = p.OrderNo, MaterialId = p.MaterialId, LocaleId = p.LocaleId }
                join r in ReceivedLog.Get() on new { POItemId = p.Id, LocaleId = p.LocaleId } equals new { POItemId = r.POItemId, LocaleId = r.RefLocaleId }
                join ra in ReceivedLogAdd.Get() on new { ReceivedLogId = r.Id, LocaleId = r.LocaleId } equals new { ReceivedLogId = ra.ReceivedLogId, LocaleId = ra.LocaleId }
                where m.Id == id && m.LocaleId == localeId
                select new { p.MaterialId, p.ParentMaterialId, p.OrderNo, p.Qty, p.UnitPrice, p.UnitCodeId, p.DollarCodeId, p.Id, p.OrdersId, m.StockDollarCodeId, m.StockDollarNameTw, ra.PurDollarNameTw, r.ReceivedDate }
            )
            .Select(i => new
            {
                i.UnitPrice,
                i.Qty,
                i.OrderNo,
                i.MaterialId,
                i.ParentMaterialId,
                i.UnitCodeId,
                i.DollarCodeId,
                i.PurDollarNameTw,
                i.Id,
                i.OrdersId,
                i.StockDollarCodeId,
                i.StockDollarNameTw,
                i.ReceivedDate,
            })
            .ToList();
            // 有採購過改用採購單單價，把單價放入平均單價
            if (pos.Any())
            {
                // 取得庫存與採購幣別
                var stockDollar = pos[0].StockDollarNameTw;
                var purDollars = pos.Select(i => i.PurDollarNameTw).Distinct().ToArray();

                // 有採購單才查詢po,有時候PO有問題，所以找尋採購單都有的幣別對照的那個日期
                var _exchDate = stockDollar == "USD" ? ExchangeRate.Get().Where(d => d.NameTw == stockDollar && purDollars.Contains(d.TransNameTw)).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate) :
                                ExchangeRate.Get().Where(d => purDollars.Contains(d.NameTw) && d.TransNameTw == stockDollar).OrderByDescending(i => i.ExchDate).Max(i => i.ExchDate);
                var exchangeItems = ExchangeRate.Get().Where(i => i.ExchDate == _exchDate).OrderByDescending(i => i.ExchDate).ToList();

                if (pos[0].OrdersId > 0) // 批次採購，把所有採購單的單價加起來
                {
                    pos.ForEach(i =>
                    {
                        var exchangeRate = i.PurDollarNameTw == i.StockDollarNameTw ? 1 : i.StockDollarNameTw == "USD" ?
                        exchangeItems.Where(d => d.NameTw == i.StockDollarNameTw && d.TransNameTw == i.PurDollarNameTw).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault() :
                        exchangeItems.Where(d => d.NameTw == i.PurDollarNameTw && d.TransNameTw == i.StockDollarNameTw).OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();

                        amount += i.UnitPrice * i.Qty * exchangeRate;
                    });
                    purQty = pos.Sum(i => i.Qty);
                    price = amount / purQty;
                }
                else  // 單筆採購，只取最新的單價
                {
                    var po = pos.OrderByDescending(i => i.Id).First();
                    var exchangeRate = po.PurDollarNameTw == po.StockDollarNameTw ? 1 : po.StockDollarNameTw == "USD" ?
                        exchangeItems.Where(d => d.NameTw == po.StockDollarNameTw && d.TransNameTw == po.PurDollarNameTw).OrderByDescending(i => i.ExchDate).Select(i => i.ReversedBankingRate).FirstOrDefault() :
                        exchangeItems.Where(d => d.NameTw == po.PurDollarNameTw && d.TransNameTw == po.StockDollarNameTw).OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();

                    price = po.UnitPrice * exchangeRate;
                }
            }
            else //拖外加工、一般沒有採購單，用原始的單價
            {
                // amount = sIOs.Where(i => i.PCLIOQty > 0).Sum(i => i.PCLIOQty * i.PurUnitPrice * i.BankingRate);
                // purQty = sIOs.Where(i => i.PCLIOQty > 0).Sum(i => i.PCLIOQty);
                // price = (amount == 0 || purQty == 0) ? 0 : amount / purQty;
                amount = _amount;
                purQty = _purQty;
                price = _price;
            }

            var usageCost = (amount == 0 || purQty == 0) ? 0 : price * (decimal)(0 - usage);

            _MaterialStock.UpdateRange(
                i => i.Id == id && i.LocaleId == localeId,
                // u => new ERP.Models.Entities.MaterialStock { 
                // PCLQty = stockQty, PurQty = purQty, Amount = (decimal)(price * stockQty), PurUnitPrice = price, TotalUsageCost = (decimal)usageCost, LastStockIOId = maxIO, AvgUnitPrice = avgPrice }
                u => u.SetProperty(p => p.PCLQty, p => stockQty).SetProperty(p => p.PurQty, p => purQty).SetProperty(p => p.Amount, p => (decimal)(price * stockQty))
                      .SetProperty(p => p.PurUnitPrice, p => price).SetProperty(p => p.TotalUsageCost, p => (decimal)usageCost).SetProperty(p => p.LastStockIOId, p => maxIO).SetProperty(p => p.AvgUnitPrice, p => avgPrice)
            );
        }
        // 更新MaterialStock
        public void UpdateMaterialStockUnitPrice1(int localeId)
        {
            UnitOfWork.BeginTransaction();
            DateTime startTime = new DateTime(2024, 10, 1, 0, 0, 0);
            var ids = new List<decimal> { 324872, 324875, 329561, 329562 };
            var items = StockIO.Get().Where(i => i.LocaleId == localeId && i.IODate >= startTime)
                .Select(i => new { i.MaterialStockId, i.LocaleId })
                .Distinct()
                // .Where(i => ids.Contains(i.MaterialStockId))
                .OrderBy(i => i.MaterialStockId)
                .ToList();

            var totalRecords = items.Count();
            var currentRecords = 0;
            items.ForEach(i =>
            {
                currentRecords += 1;

                Console.WriteLine(">>> {0}/{1}({2}) : Start", currentRecords, totalRecords, i.MaterialStockId);

                try
                {
                    MaterialStock.UpdatStockQty((int)i.MaterialStockId, (int)i.LocaleId);
                    UnitOfWork.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(">>> {0}/{1} : Error", currentRecords, totalRecords);
                }
            });
        }
    }
}
