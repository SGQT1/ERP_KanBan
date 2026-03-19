using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Services.Business
{
    public class MonthMaterialStockService : BusinessService
    {
        private ERP.Services.Business.Entities.ExchangeRateService ExchangeRate { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.MaterialStockService MaterialStock { get; set; }
        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }
        private ERP.Services.Business.Entities.MonthMaterialStockBatchCostService MonthMaterialStockBatchCost { get; set; }
        private ERP.Services.Business.Entities.MonthMaterialStockIOInService MonthMaterialStockIOIn { get; set; }
        private ERP.Services.Business.Entities.MonthMaterialStockIOOutService MonthMaterialStockIOOut { get; set; }
        private ERP.Services.Business.Entities.MonthMaterialStockIOSchService MonthMaterialStockIOSch { get; set; }

        private ERP.Services.Business.Entities.MonthMaterialStockItemService MonthMaterialStockItem { get; set; }
        private ERP.Services.Business.Entities.MonthMaterialStockItemSchService MonthMaterialStockItemSch { get; set; }
        private ERP.Services.Business.Entities.MonthMaterialStockItemOutService MonthMaterialStockItemOut { get; set; }
        private ERP.Services.Business.Entities.MonthMaterialStockItemInService MonthMaterialStockItemIn { get; set; }
        private ERP.Services.Business.Entities.MaterialStockBatchCostService MaterialStockBatchCost { get; set; }

        public MonthMaterialStockService(
            ERP.Services.Business.Entities.ExchangeRateService exchangeRateService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.MaterialStockService materialStockService,
            ERP.Services.Business.Entities.MaterialStockItemService materialStockItemService,

            ERP.Services.Business.Entities.MonthMaterialStockBatchCostService monthMaterialStockBatchCostService,
            ERP.Services.Business.Entities.MonthMaterialStockIOInService monthMaterialStockIOInService,
            ERP.Services.Business.Entities.MonthMaterialStockIOOutService monthMaterialStockIOOutService,
            ERP.Services.Business.Entities.MonthMaterialStockIOSchService monthMaterialStockIOSchService,

            ERP.Services.Business.Entities.MonthMaterialStockItemService monthMaterialStockItemService,
            ERP.Services.Business.Entities.MonthMaterialStockItemSchService monthMaterialStockItemSchService,
            ERP.Services.Business.Entities.MonthMaterialStockItemOutService monthMaterialStockItemOutService,
            ERP.Services.Business.Entities.MonthMaterialStockItemInService monthMaterialStockItemInService,
            ERP.Services.Business.Entities.MaterialStockBatchCostService materialStockBatchCostService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            ExchangeRate = exchangeRateService;
            CodeItem = codeItemService;

            MaterialStock = materialStockService;
            MaterialStockItem = materialStockItemService;
            MonthMaterialStockBatchCost = monthMaterialStockBatchCostService;
            MonthMaterialStockIOIn = monthMaterialStockIOInService;
            MonthMaterialStockIOOut = monthMaterialStockIOOutService;
            MonthMaterialStockIOSch = monthMaterialStockIOSchService;

            MonthMaterialStockItem = monthMaterialStockItemService;
            MonthMaterialStockItemSch = monthMaterialStockItemSchService;
            MonthMaterialStockItemOut = monthMaterialStockItemOutService;
            MonthMaterialStockItemIn = monthMaterialStockItemInService;
            MaterialStockBatchCost = materialStockBatchCostService;
        }

        public IQueryable<Models.Views.MonthMaterialStockItem> GetItems()
        {
            var restul = (
                from s in MonthMaterialStockItem.Get()
                join sch in MonthMaterialStockItemSch.Get() on new { IOMonth = s.IOMonth, LocaleId = s.LocaleId } equals new { IOMonth = (decimal?)sch.IOMonth, LocaleId = (decimal?)sch.LocaleId } into schGRP
                from sch in schGRP.DefaultIfEmpty()
                select new Models.Views.MonthMaterialStockItem
                {
                    LocaleId = s.LocaleId,
                    IOMonth = s.IOMonth,
                    MaxUpdateTime = s.MaxUpdateTime,
                    Id = sch.Id == null ? 0 : sch.Id,
                    CalTime = sch.CalTime,
                    ModifyUserName = sch.Id == null ? "" : (sch.ModifyUserName ?? ""),  // net9 會把select的null結果變成是一個空類別，sch == null 會不成立，改用sch.Id
                    IsReCal = sch.Id == null ? 1 : 0,
                    Records = MonthMaterialStockItemOut.Get().Where(i => i.IOMonth == s.IOMonth && i.LocaleId == s.LocaleId).Count(),
                }
            );
            return restul;
        }
        public void BuildMonthMaterialStock(List<Models.Views.MonthMaterialStockIO> items)
        {
            try
            {
                items.ForEach(i =>
                {
                    SaveMonthMaterialStock(i);
                    // SaveMonthMaterialStock_1(i);
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveMonthMaterialStock(Models.Views.MonthMaterialStockIO item)
        {
            // # Delete from StockIOMonthIn  Where IOMonth=202406 and LocaleId=6#
            // # Delete from StockIOMonthOut  Where IOMonth=202406 and LocaleId=6#
            // # insert into StockIOMonthIn (LocaleId,IOMonth,MATERIALSTOCKId,IOQty,IOAmount,SourceType) Select LocaleId,202406,MATERIALSTOCKId,SUM(PCLIOQty),Convert(Decimal(20,2),Sum(PurUnitPrice*PCLIOQty*BankingRate)),SourceType  from StockIO Where CONVERT(varchar(6),IODate, 112)=202406 and SourceType <= 12 and PCLIOQty>0 and LocaleId=6 group by LocaleId,MATERIALSTOCKId,SourceType#
            // # insert into StockIOMonthOut (LocaleId,IOMonth,MATERIALSTOCKId,IOQty,IOAmount,SourceType) Select LocaleId,202406,MATERIALSTOCKId,SUM(PCLIOQty),Convert(Decimal(20,2),Sum(PurUnitPrice*PCLIOQty*BankingRate)),SourceType  from StockIO Where CONVERT(varchar(6),IODate, 112)=202406 and SourceType <= 12 and PCLIOQty<0 and LocaleId=6 group by LocaleId,MATERIALSTOCKId,SourceType#
            // # Delete from MaterialStockBatchCost Where IOMonth=202406 and LocaleId=6#
            // # insert into MaterialStockBatchCost (LocaleId,MaterialName,OrderNo,IOMonth,CostType,IOType,PCLUnitNameTw,StockDollarNameTw, IOQty,IOAmount,ModifyUserName,LastUpdateTime) Select A.LocaleId, MaterialName, OrderNo, IOMonth,Case When SourceType=12 then 2 else 1 end CostType, Case SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end IOType, PCLUnitNameTw, StockDollarNameTw, Sum(IOQty) IOQty ,Convert(Decimal(20,2),Sum(IOAmount)) IOAmount, 'tdm_admin', getdate() from StockIOMonthOut A  INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId Where IOMonth=202406 and SourceType in (1,7,9,12) and A.LocaleId=6 group by A.LocaleId, MaterialName, OrderNo, IOMonth,  Case When SourceType=12 then 2 else 1 end, Case SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end, PCLUnitNameTw,StockDollarNameTw#
            // # insert into MaterialStockBatchCost (LocaleId,MaterialName,OrderNo,IOMonth,CostType,IOType,PCLUnitNameTw,StockDollarNameTw, IOQty,IOAmount,ModifyUserName,LastUpdateTime) Select A.LocaleId, MaterialName, OrderNo, IOMonth,Case When SourceType=12 then 2 else 1 end CostType, Case SourceType when 6 then 2 when 8 then 4 else 6 end IOType,  PCLUnitNameTw, StockDollarNameTw, Sum(IOQty) IOQty ,Convert(Decimal(20,2),Sum(IOAmount)) IOAmount, 'tdm_admin', getdate() from StockIOMonthIn A  INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId Where IOMonth=202406 and SourceType in (6,8,12) and A.LocaleId=6 group by A.LocaleId, MaterialName, OrderNo, IOMonth,  Case When SourceType=12 then 2 else 1 end, Case SourceType when 6 then 2 when 8 then 4 else 6 end, PCLUnitNameTw,StockDollarNameTw#
            // # Delete from StockIOMonthSch Where IOMonth=202406 and LocaleId=6#
            // # insert into StockIOMonthSch  (LocaleId,IOMonth,CalTime,ModifyUserName,LastUpdateTime) Values (6,202406, getdate(), 'tdm_admin', getdate()) #

            try
            {
                UnitOfWork.BeginTransaction();


                var ioMonth = item.IOMonth;
                var ioYYYY = Convert.ToInt16(item.IOMonth.ToString().Substring(0, 4));
                var ioMM = Convert.ToInt16(item.IOMonth.ToString().Substring(4, 2));
                var userName = item.ModifyUserName;

                var localeId = item.LocaleId;

                var exchangeRate = ExchangeRate.Get().Where(i => i.CurrencyTw == "USD" && i.TransCurrencyTw == "NTD").OrderByDescending(i => i.ExchDate).Select(i => i.BankingRate).FirstOrDefault();
                var dollarCodeId = CodeItem.Get().Where(i => i.NameTW == "USD" && i.LocaleId == localeId && i.CodeType == "02").Select(i => i.Id).FirstOrDefault();

                // MonthMaterialStockItemIn.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                // MonthMaterialStockIOOut.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                // MonthMaterialStockBatchCost.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                // MonthMaterialStockIOSch.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);

                MonthMaterialStockItemIn.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                MonthMaterialStockItemOut.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                MonthMaterialStockItemSch.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                MaterialStockBatchCost.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);

                // var _stockInStr = @"INSERT INTO StockIOMonthIn (LocaleId,IOMonth,MaterialStockId,IOQty,IOAmount,SourceType) 
                //                     Select LocaleId, {0}, MaterialStockId, SUM(PCLIOQty), Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)), SourceType 
                //                     FROM StockIO 
                //                     WHERE CONVERT(varchar(6),IODate, 112)= {0} and SourceType <= 12 and PCLIOQty > 0 and LocaleId = {1} 
                //                     GROUP BY LocaleId, MaterialStockId,SourceType";

                var _stockInStr = @"INSERT INTO StockIOMonthIn (LocaleId,IOMonth,MaterialStockId,IOQty,IOAmount,SourceType) 
                    Select LocaleId, {0}, MaterialStockId, SUM(PCLIOQty), Case When StockDollarCodeId = '77' then Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)) / {2} else Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)) End, SourceType 
                    FROM StockIO 
                    WHERE CONVERT(varchar(6),IODate, 112)= {0} and SourceType <= 12 and PCLIOQty > 0 and LocaleId = {1} 
                    GROUP BY LocaleId, MaterialStockId,SourceType, StockDollarCodeId";

                _stockInStr = string.Format(_stockInStr, ioMonth, localeId, exchangeRate);
                MonthMaterialStockItemIn.ExecuteSqlCommand(_stockInStr);

                // var _stockOutStr = @"INSERT INTO StockIOMonthOut (LocaleId,IOMonth,MaterialStockId,IOQty,IOAmount,SourceType) 
                //                     Select LocaleId, {0}, MaterialStockId, SUM(PCLIOQty), Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)), SourceType 
                //                     FROM StockIO 
                //                     WHERE CONVERT(varchar(6),IODate, 112)= {0} and SourceType <= 12 and PCLIOQty < 0 and LocaleId = {1} 
                //                     GROUP BY LocaleId, MaterialStockId,SourceType";
                var _stockOutStr = @"INSERT INTO StockIOMonthOut (LocaleId,IOMonth,MaterialStockId,IOQty,IOAmount,SourceType) 
                                    Select LocaleId, {0}, MaterialStockId, SUM(PCLIOQty), Case When StockDollarCodeId = '77' then Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)) / {2} else Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)) End, SourceType 
                                    FROM StockIO 
                                    WHERE CONVERT(varchar(6),IODate, 112)= {0} and SourceType <= 12 and PCLIOQty < 0 and LocaleId = {1} 
                                    GROUP BY LocaleId, MaterialStockId,SourceType, StockDollarCodeId";


                _stockOutStr = string.Format(_stockOutStr, ioMonth, localeId, exchangeRate);
                MonthMaterialStockItemOut.ExecuteSqlCommand(_stockOutStr);

                var _stockOutCostStr = @"INSERT INTO MaterialStockBatchCost (LocaleId,MaterialName,OrderNo,IOMonth,CostType,IOType,PCLUnitNameTw,StockDollarNameTw, IOQty,IOAmount,ModifyUserName,LastUpdateTime) 
                                         SELECT A.LocaleId, B.MaterialName, B.OrderNo, A.IOMonth, CASE WHEN A.SourceType=12 THEN 2 ELSE 1 END CostType, CASE SourceType WHEN 1 THEN 1 WHEN 7 THEN 3 WHEN 9 THEN 5 ELSE 6 END IOType, B.PCLUnitNameTw, B.StockDollarNameTw, Sum(A.IOQty) IOQty, Convert(Decimal(20,2), Sum(A.IOAmount)) IOAmount, '{2}', getdate() 
                                         FROM StockIOMonthOut A  
                                         INNER JOIN MaterialStock B ON A.MaterialStockId = B.Id AND A.LocaleId = B.LocaleId 
                                         WHERE A.IOMonth = {0} AND A.SourceType IN (1, 7, 9, 12) AND A.LocaleId = {1} 
                                         GROUP BY A.LocaleId, B.MaterialName, B.OrderNo, A.IOMonth, CASE WHEN A.SourceType = 12 THEN 2 ELSE 1 END, CASE A.SourceType WHEN 1 THEN 1 WHEN 7 THEN 3 WHEN 9 THEN 5 ELSE 6 END, B.PCLUnitNameTw, B.StockDollarNameTw";
                _stockOutCostStr = string.Format(_stockOutCostStr, ioMonth, localeId, userName);
                MaterialStockBatchCost.ExecuteSqlCommand(_stockOutCostStr);

                var _stockInCostStr = @"INSERT INTO MaterialStockBatchCost (LocaleId, MaterialName, OrderNo, IOMonth, CostType, IOType, PCLUnitNameTw, StockDollarNameTw, IOQty, IOAmount, ModifyUserName, LastUpdateTime) 
                                        SELECT A.LocaleId, B.MaterialName, B.OrderNo, A.IOMonth,CASE WHEN A.SourceType=12 THEN 2 ELSE 1 END CostType, CASE A.SourceType WHEN 6 THEN 2 WHEN 8 THEN 4 ELSE 6 END IOType,  B.PCLUnitNameTw, B.StockDollarNameTw, Sum(A.IOQty) IOQty, Convert(Decimal(20,2), Sum(A.IOAmount)) IOAmount, '{2}', getdate()
                                        FROM StockIOMonthIn A 
                                        INNER JOIN MaterialStock B ON A.MaterialStockId = B.Id and A.LocaleId = B.LocaleId 
                                        WHERE A.IOMonth = {0} AND A.SourceType IN (6, 8, 12) AND A.LocaleId = {1} 
                                        GROUP BY A.LocaleId, B.MaterialName, B.OrderNo, A.IOMonth,  CASE WHEN A.SourceType=12 THEN 2 ELSE 1 END, CASE A.SourceType WHEN 6 THEN 2 WHEN 8 THEN 4 ELSE 6 END, B.PCLUnitNameTw, B.StockDollarNameTw";

                _stockInCostStr = string.Format(_stockInCostStr, ioMonth, localeId, userName);
                MaterialStockBatchCost.ExecuteSqlCommand(_stockInCostStr);

                // # Delete from StockIOMonthSch Where IOMonth=202406 and LocaleId=6#
                // # insert into StockIOMonthSch  (LocaleId,IOMonth,CalTime,ModifyUserName,LastUpdateTime) Values (6,202406, getdate(), 'tdm_admin', getdate()) #
                MonthMaterialStockItemSch.Create(new Models.Views.MonthMaterialStockItemSch
                {
                    LocaleId = localeId,
                    IOMonth = ioMonth,
                    CalTime = DateTime.Now,
                    ModifyUserName = userName,
                    LastUpdateTime = DateTime.Now,
                });

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void SaveMonthMaterialStock_1(Models.Views.MonthMaterialStockIO item)
        {
            // # Delete from StockIOMonthIn  Where IOMonth=202406 and LocaleId=6#
            // # Delete from StockIOMonthOut  Where IOMonth=202406 and LocaleId=6#
            // # insert into StockIOMonthIn (LocaleId,IOMonth,MATERIALSTOCKId,IOQty,IOAmount,SourceType) Select LocaleId,202406,MATERIALSTOCKId,SUM(PCLIOQty),Convert(Decimal(20,2),Sum(PurUnitPrice*PCLIOQty*BankingRate)),SourceType  from StockIO Where CONVERT(varchar(6),IODate, 112)=202406 and SourceType <= 12 and PCLIOQty>0 and LocaleId=6 group by LocaleId,MATERIALSTOCKId,SourceType#
            // # insert into StockIOMonthOut (LocaleId,IOMonth,MATERIALSTOCKId,IOQty,IOAmount,SourceType) Select LocaleId,202406,MATERIALSTOCKId,SUM(PCLIOQty),Convert(Decimal(20,2),Sum(PurUnitPrice*PCLIOQty*BankingRate)),SourceType  from StockIO Where CONVERT(varchar(6),IODate, 112)=202406 and SourceType <= 12 and PCLIOQty<0 and LocaleId=6 group by LocaleId,MATERIALSTOCKId,SourceType#
            // # Delete from MaterialStockBatchCost Where IOMonth=202406 and LocaleId=6#
            // # insert into MaterialStockBatchCost (LocaleId,MaterialName,OrderNo,IOMonth,CostType,IOType,PCLUnitNameTw,StockDollarNameTw, IOQty,IOAmount,ModifyUserName,LastUpdateTime) Select A.LocaleId, MaterialName, OrderNo, IOMonth,Case When SourceType=12 then 2 else 1 end CostType, Case SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end IOType, PCLUnitNameTw, StockDollarNameTw, Sum(IOQty) IOQty ,Convert(Decimal(20,2),Sum(IOAmount)) IOAmount, 'tdm_admin', getdate() from StockIOMonthOut A  INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId Where IOMonth=202406 and SourceType in (1,7,9,12) and A.LocaleId=6 group by A.LocaleId, MaterialName, OrderNo, IOMonth,  Case When SourceType=12 then 2 else 1 end, Case SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end, PCLUnitNameTw,StockDollarNameTw#
            // # insert into MaterialStockBatchCost (LocaleId,MaterialName,OrderNo,IOMonth,CostType,IOType,PCLUnitNameTw,StockDollarNameTw, IOQty,IOAmount,ModifyUserName,LastUpdateTime) Select A.LocaleId, MaterialName, OrderNo, IOMonth,Case When SourceType=12 then 2 else 1 end CostType, Case SourceType when 6 then 2 when 8 then 4 else 6 end IOType,  PCLUnitNameTw, StockDollarNameTw, Sum(IOQty) IOQty ,Convert(Decimal(20,2),Sum(IOAmount)) IOAmount, 'tdm_admin', getdate() from StockIOMonthIn A  INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId Where IOMonth=202406 and SourceType in (6,8,12) and A.LocaleId=6 group by A.LocaleId, MaterialName, OrderNo, IOMonth,  Case When SourceType=12 then 2 else 1 end, Case SourceType when 6 then 2 when 8 then 4 else 6 end, PCLUnitNameTw,StockDollarNameTw#
            // # Delete from StockIOMonthSch Where IOMonth=202406 and LocaleId=6#
            // # insert into StockIOMonthSch  (LocaleId,IOMonth,CalTime,ModifyUserName,LastUpdateTime) Values (6,202406, getdate(), 'tdm_admin', getdate()) #

            try
            {
                UnitOfWork.BeginTransaction();

                var ioMonth = item.IOMonth;
                var ioYYYY = Convert.ToInt16(item.IOMonth.ToString().Substring(0, 4));
                var ioMM = Convert.ToInt16(item.IOMonth.ToString().Substring(4, 2));
                var userName = item.ModifyUserName;

                var localeId = item.LocaleId;

                MonthMaterialStockIOIn.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                MonthMaterialStockIOOut.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                MonthMaterialStockBatchCost.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);
                MonthMaterialStockIOSch.RemoveRange(i => i.IOMonth == ioMonth && i.LocaleId == localeId);

                // Select LocaleId,202406,MATERIALSTOCKId,SUM(PCLIOQty),Convert(Decimal(20,2),Sum(PurUnitPrice*PCLIOQty*BankingRate)),SourceType  
                // from StockIO 
                // Where CONVERT(varchar(6),IODate, 112)=202406 and SourceType <= 12 and PCLIOQty>0 and LocaleId=6 
                // group by LocaleId,MATERIALSTOCKId,SourceType

                var _stockInStr = @"INSERT INTO MonthMaterialStockIOIn (LocaleId,IOMonth,MaterialStockId,IOQty,IOAmount,SourceType) 
                                    Select LocaleId, {0}, MaterialStockId, SUM(PCLIOQty), Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)), SourceType 
                                    FROM StockIO 
                                    WHERE CONVERT(varchar(6),IODate, 112)= {0} and SourceType <= 12 and PCLIOQty > 0 and LocaleId = {1} 
                                    GROUP BY LocaleId, MaterialStockId,SourceType";

                _stockInStr = string.Format(_stockInStr, ioMonth, localeId);
                MonthMaterialStockIOIn.ExecuteSqlCommand(_stockInStr);

                // Select LocaleId,202406,MATERIALSTOCKId,SUM(PCLIOQty),Convert(Decimal(20,2),Sum(PurUnitPrice*PCLIOQty*BankingRate)),SourceType  
                // from StockIO 
                // Where CONVERT(varchar(6),IODate, 112)=202406 and SourceType <= 12 and PCLIOQty<0 and LocaleId=6 
                // group by LocaleId,MATERIALSTOCKId,SourceType

                var _stockOutStr = @"INSERT INTO MonthMaterialStockIOOut (LocaleId,IOMonth,MaterialStockId,IOQty,IOAmount,SourceType) 
                                    Select LocaleId, {0}, MaterialStockId, SUM(PCLIOQty), Convert(Decimal(20,2),Sum(PurUnitPrice * PCLIOQty * BankingRate)), SourceType 
                                    FROM StockIO 
                                    WHERE CONVERT(varchar(6),IODate, 112)= {0} and SourceType <= 12 and PCLIOQty < 0 and LocaleId = {1} 
                                    GROUP BY LocaleId, MaterialStockId,SourceType";
                _stockOutStr = string.Format(_stockOutStr, ioMonth, localeId);
                MonthMaterialStockIOOut.ExecuteSqlCommand(_stockOutStr);

                // Select A.LocaleId, MaterialName, OrderNo, IOMonth,Case When SourceType=12 then 2 else 1 end CostType, Case SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end IOType, PCLUnitNameTw, StockDollarNameTw, 
                // Sum(IOQty) IOQty ,Convert(Decimal(20,2),Sum(IOAmount)) IOAmount, 'tdm_admin', getdate() 
                // from StockIOMonthOut A  
                // INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId 
                // Where IOMonth=202406 and SourceType in (1,7,9,12) and A.LocaleId=6 
                // group by A.LocaleId, MaterialName, OrderNo, IOMonth,  Case When SourceType=12 then 2 else 1 end, Case SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end, PCLUnitNameTw,StockDollarNameTw

                var _stockOutCostStr = @"INSERT INTO MonthMaterialStockBatchCost(LocaleId, MaterialName, OrderNo, IOMonth, CostType, IOType, PCLUnitNameTw, StockDollarNameTw, IOQty, IOAmount, ModifyUserName, LastUpdateTime, PurUnitPrice, PurDollarCodeId, ExchangeRate, PurDollarNameTw)
                                        SELECT A.LocaleId, B.MaterialName, B.OrderNo, A.IOMonth, Case When A.SourceType=12 then 2 else 1 end CostType, Case A.SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end IOType, B.PCLUnitNameTw, B.StockDollarNameTw, Sum(A.IOQty) IOQty , Convert(Decimal(20,2),Sum(A.IOQty)*B.PurUnitPrice*b.ExchangeRate) IOAmount, '{2}', getdate(),
                                        B.PurUnitPrice, B.PurDollarCodeId, B.ExchangeRate, B.PurDollarNameTw
                                        FROM MonthMaterialStockIOOut A 
                                        INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId
                                        WHERE IOMonth={0} and SourceType in (1,7,9,12) and A.LocaleId={1}
                                        GROUP BY A.LocaleId, B.MaterialName, B.OrderNo, IOMonth,  Case When A.SourceType=12 then 2 else 1 end, Case A.SourceType when 1 then 1 when 7 then 3 when 9 then 5 else 6 end, B.PCLUnitNameTw,B.StockDollarNameTw, B.PurUnitPrice, B.PurDollarCodeId, B.ExchangeRate, B.PurDollarNameTw";
                _stockOutCostStr = string.Format(_stockOutCostStr, ioMonth, localeId, userName);
                MonthMaterialStockBatchCost.ExecuteSqlCommand(_stockOutCostStr);

                // Select A.LocaleId, MaterialName, OrderNo, IOMonth,Case When SourceType=12 then 2 else 1 end CostType, Case SourceType when 6 then 2 when 8 then 4 else 6 end IOType,  PCLUnitNameTw, StockDollarNameTw,
                // Sum(IOQty) IOQty ,Convert(Decimal(20,2),Sum(IOAmount)) IOAmount, 'tdm_admin', getdate() 
                // from StockIOMonthIn A  INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId 
                // Where IOMonth=202406 and SourceType in (6,8,12) and A.LocaleId=6 
                // group by A.LocaleId, MaterialName, OrderNo, IOMonth,  Case When SourceType=12 then 2 else 1 end, Case SourceType when 6 then 2 when 8 then 4 else 6 end, PCLUnitNameTw,StockDollarNameTw#

                var _stockInCostStr = @"INSERT INTO MonthMaterialStockBatchCost(LocaleId,MaterialName,OrderNo,IOMonth,CostType,IOType,PCLUnitNameTw,StockDollarNameTw, IOQty,IOAmount,ModifyUserName,LastUpdateTime, PurUnitPrice, PurDollarCodeId, ExchangeRate, PurDollarNameTw)
                                        SELECT A.LocaleId, B.MaterialName, B.OrderNo, A.IOMonth,Case When A.SourceType=12 then 2 else 1 end CostType, Case A.SourceType when 6 then 2 when 8 then 4 else 6 end IOType, B.PCLUnitNameTw,B. StockDollarNameTw, Sum(A.IOQty) IOQty , Convert(Decimal(20,2),Sum(A.IOQty)*B.PurUnitPrice*b.ExchangeRate) IOAmount, '{2}', getdate(),
                                        B.PurUnitPrice, B.PurDollarCodeId, B.ExchangeRate, B.PurDollarNameTw
                                        FROM MonthMaterialStockIOIn A 
                                        INNER JOIN MaterialStock B ON A.MaterialStockId=B.Id and A.LocaleId=B.LocaleId
                                        WHERE IOMonth={0} and SourceType in (6,8,12) and A.LocaleId={1}
                                        GROUP BY A.LocaleId, B.MaterialName, B.OrderNo, A.IOMonth, Case When A.SourceType=12 then 2 else 1 end, Case A.SourceType when 6 then 2 when 8 then 4 else 6 end, B.PCLUnitNameTw,B.StockDollarNameTw, B.PurUnitPrice, B.PurDollarCodeId, B.ExchangeRate, B.PurDollarNameTw";
                _stockInCostStr = string.Format(_stockInCostStr, ioMonth, localeId, userName);
                MonthMaterialStockBatchCost.ExecuteSqlCommand(_stockInCostStr);

                // # Delete from StockIOMonthSch Where IOMonth=202406 and LocaleId=6#
                // # insert into StockIOMonthSch  (LocaleId,IOMonth,CalTime,ModifyUserName,LastUpdateTime) Values (6,202406, getdate(), 'tdm_admin', getdate()) #
                MonthMaterialStockIOSch.Create(new Models.Views.MonthMaterialStockIOSch
                {
                    LocaleId = localeId,
                    IOMonth = ioMonth,
                    CalTime = DateTime.Now,
                    ModifyUserName = userName,
                    LastUpdateTime = DateTime.Now,
                    Records = MonthMaterialStockBatchCost.Get().Where(i => i.IOMonth == ioMonth && i.LocaleId == localeId).Count()
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
}
