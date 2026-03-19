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
    public class MaterialStockService : BusinessService
    {
        private ERP.Services.Entities.MaterialStockService MaterialStock { get; }
        private ERP.Services.Entities.MaterialStockService _MaterialStock { get; }
        private ERP.Services.Entities.StockIOService StockIO { get; }

        private ERP.Services.Entities.POItemService POItem { get; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; }
        private ERP.Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; }
        private ERP.Services.Entities.ExchangeRateService ExchangeRate { get; }
        public MaterialStockService(
            Services.Entities.MaterialStockService materialStockService,
            ERP.Services.Entities.MaterialStockService _materialStockService,
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            ERP.Services.Entities.ReceivedLogAddService receivedLogAddService,
            ERP.Services.Entities.ExchangeRateService exchangeRateService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.MaterialStock = materialStockService;
            this._MaterialStock = _materialStockService;
            this.StockIO = stockIOService;
            this.POItem = poItemService;
            this.ReceivedLog = receivedLogService;
            this.ReceivedLogAdd = receivedLogAddService;
            this.ExchangeRate = exchangeRateService;
        }
        public IQueryable<Models.Views.MaterialStock> Get()
        {
            return MaterialStock.Get().Select(i => new Models.Views.MaterialStock
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
                TransRate = i.TransRate,
                PurUnitCodeId = i.PurUnitCodeId,
                PurUnitNameTw = i.PurUnitNameTw,
                PurUnitNameEn = i.PurUnitNameEn,
                PCLPlanQty = i.PCLPlanQty,
                PCLQty = i.PCLQty,
                PurQty = i.PurQty,
                PCLAllocationQty = i.PCLAllocationQty,
                PurAllocationQty = i.PurAllocationQty,
                Amount = i.Amount,
                StockDollarCodeId = i.StockDollarCodeId,
                StockDollarNameTw = i.StockDollarNameTw,
                StockDollarNameEn = i.StockDollarNameEn,
                ParentMaterialId = i.ParentMaterialId,
                ParentMaterialNameTw = i.ParentMaterialNameTw,
                ParentMaterialNameEn = i.ParentMaterialNameEn,
                LastStockIOId = i.LastStockIOId,
                TotalUsageCost = i.TotalUsageCost,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                RefOrderNo = i.OrderNo.Contains("-") ? i.OrderNo.Substring(0, i.OrderNo.IndexOf("-")) : i.OrderNo,
                PurUnitPrice = i.PurUnitPrice,
                // PurDollarCodeId = i.PurDollarCodeId,
                // PurDollarNameTw = i.PurDollarNameTw,
                // PurDollarNameEn = i.PurDollarNameEn,
                // ExchangeRate = i.ExchangeRate,
                AvgUnitPrice = i.AvgUnitPrice == null ? 0 : i.AvgUnitPrice,
                RealStockOutQty = StockIO.Get().Where(s => s.MaterialStockId == i.Id && s.LocaleId == i.LocaleId && s.PCLIOQty < 0).Sum(s => s.PCLIOQty),
                RealStockQty = StockIO.Get().Where(s => s.MaterialStockId == i.Id && s.LocaleId == i.LocaleId).Sum(i => i.PCLIOQty),
            });
        }
        public Models.Views.MaterialStock Create(Models.Views.MaterialStock item)
        {
            var _item = MaterialStock.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MaterialStock Update(Models.Views.MaterialStock item)
        {
            var _item = MaterialStock.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MaterialStock item)
        {
            MaterialStock.Remove(Build(item));
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
                // u => new MaterialStock { PCLQty = stockQty, PurQty = purQty, Amount = (decimal)(price * stockQty), PurUnitPrice = price, TotalUsageCost = (decimal)usageCost, LastStockIOId = maxIO, AvgUnitPrice = avgPrice }
                u => u.SetProperty(p => p.PCLQty, v => stockQty).SetProperty(p => p.PurQty, v => purQty).SetProperty(p => p.Amount, v => (decimal)(price * stockQty))
                      .SetProperty(p => p.PurUnitPrice, v => price).SetProperty(p => p.TotalUsageCost, v => (decimal)usageCost).SetProperty(p => p.LastStockIOId, v => maxIO).SetProperty(p => p.AvgUnitPrice, v => avgPrice)
            );
        }

        /*
         * 改用比較簡單一致的算法計算，不用用採購單價，採購單會有錯，反是收貨單價才會是最正確的
         * 入庫單價會用資料庫排成每日更新有異動的收貨金額，確保這裡的一致性
         */
        public void UpdatStockQty(int id, int localeId)
        {
            var stockQty = (decimal)0;
            var amount = (decimal?)0;
            var purQty = (decimal)0;
            var price = (decimal?)0;
            var avgPrice = (decimal?)0;
            var usage = (decimal?)0;
            var maxIO = (decimal)0;
            var _exchangeRate = (decimal)0;

            var sIOs = StockIO.Get().Where(i => i.MaterialStockId == id && i.LocaleId == localeId).Select(i => new { i.Id, i.IODate, i.SourceType, i.PCLIOQty, i.PurUnitPrice, i.BankingRate, i.ReceivedLogId }).ToList();
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

            // Step 2: 用採購單計算單價，沒有來料入庫的採購單價就是平均單價，因為採購單價再實際上已經無意義
            amount = _amount;
            purQty = _purQty;
            price = _price;

            // 採購單價改用 來料入庫算，這樣才會有拖外加工的錢
            if (sIOs.Where(i => i.SourceType == 0).Any())
            {
                amount = sIOs.Where(i => i.SourceType == 0).Sum(i => i.PurUnitPrice * i.PCLIOQty * i.BankingRate);
                purQty = _purQty;
                price = amount / purQty;
            }

            var usageCost = (amount == 0 || purQty == 0) ? 0 : price * (decimal)(0 - usage);

            _MaterialStock.UpdateRange(
                i => i.Id == id && i.LocaleId == localeId,
                // u => new MaterialStock { PCLQty = stockQty, PurQty = _purQty, Amount = (decimal)(price * stockQty), PurUnitPrice = price, TotalUsageCost = (decimal)usageCost, LastStockIOId = maxIO, AvgUnitPrice = avgPrice }
                u => u.SetProperty(p => p.PCLQty, v => stockQty).SetProperty(p => p.PurQty, v => _purQty).SetProperty(p => p.Amount, v => (decimal)(price * stockQty))
                      .SetProperty(p => p.PurUnitPrice, v => price).SetProperty(p => p.TotalUsageCost, v => (decimal)usageCost).SetProperty(p => p.LastStockIOId, v => maxIO).SetProperty(p => p.AvgUnitPrice, v => avgPrice)
            );
        }
        private Models.Entities.MaterialStock Build(Models.Views.MaterialStock item)
        {
            return new Models.Entities.MaterialStock()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MaterialId = item.MaterialId,
                MaterialName = item.MaterialName,
                MaterialNameEng = item.MaterialNameEng,
                WarehouseId = item.WarehouseId,
                WarehouseNo = item.WarehouseNo,
                OrderNo = item.OrderNo,
                PCLUnitCodeId = item.PCLUnitCodeId,
                PCLUnitNameTw = item.PCLUnitNameTw,
                PCLUnitNameEn = item.PCLUnitNameEn,
                TransRate = item.TransRate,
                PurUnitCodeId = item.PurUnitCodeId,
                PurUnitNameTw = item.PurUnitNameTw,
                PurUnitNameEn = item.PurUnitNameEn,
                PCLPlanQty = item.PCLPlanQty,
                PCLQty = item.PCLQty,
                PurQty = item.PurQty,
                PCLAllocationQty = item.PCLAllocationQty,
                PurAllocationQty = item.PurAllocationQty,
                Amount = item.Amount,
                StockDollarCodeId = item.StockDollarCodeId,
                StockDollarNameTw = item.StockDollarNameTw,
                StockDollarNameEn = item.StockDollarNameEn,
                ParentMaterialId = item.ParentMaterialId,
                ParentMaterialNameTw = item.ParentMaterialNameTw,
                ParentMaterialNameEn = item.ParentMaterialNameEn,
                LastStockIOId = item.LastStockIOId,
                TotalUsageCost = item.TotalUsageCost,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PurUnitPrice = item.PurUnitPrice,
                AvgUnitPrice = item.AvgUnitPrice == null ? 0 : item.AvgUnitPrice,
                // PurDollarCodeId = item.PurDollarCodeId,
                // PurDollarNameTw = item.PurDollarNameTw,
                // PurDollarNameEn = item.PurDollarNameEn,
                // ExchangeRate = item.ExchangeRate,
            };
        }

    }
}