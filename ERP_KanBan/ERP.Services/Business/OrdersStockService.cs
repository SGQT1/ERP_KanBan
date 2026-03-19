using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class OrdersStockService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersStockService OrdersStock { get; set; }
        private ERP.Services.Business.Entities.OrdersStockItemService OrdersStockItem{ get; set; }
        public OrdersStockService(
            ERP.Services.Business.Entities.OrdersStockService ordersStockService,
            ERP.Services.Business.Entities.OrdersStockItemService ordersStockItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OrdersStock = ordersStockService;
            OrdersStockItem = ordersStockItemService;
        }

        public IQueryable<ERP.Models.Views.OrdersStockItem> Get()
        {
            return OrdersStockItem.Get();
        }

        public IEnumerable<ERP.Models.Views.OrdersStockItem> SaveWithStockIn(IEnumerable<ERP.Models.Views.OrdersStockItem> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var localeId = items.FirstOrDefault().LocaleId;
                var labelCodes = items.Select(i => i.LabelCode).ToArray();

                if(true) {
                    OrdersStockItem.StockInRemoveRange(labelCodes, localeId);
                    var stockIns = items.Where(i => i.StockInGrossWeight != null && i.StockInTime != null);
                    if(stockIns.Count() > 0) {
                        OrdersStockItem.StockInCreateRange(stockIns);
                    }
                }

                OrdersStockItem.StockOutRemoveRange(labelCodes, localeId);
                var stockOuts = items.Where(i => i.StockOutGrossWeight != null && i.StockOutTime != null);
                if(stockOuts.Count() > 0) {
                    OrdersStockItem.StockOutCreateRange(stockOuts);
                }

                UnitOfWork.Commit();
            }
            catch(Exception e)
            {
                UnitOfWork.Rollback();
            }
            return items;
        }

        public IEnumerable<ERP.Models.Views.OrdersStockItem> Save(IEnumerable<ERP.Models.Views.OrdersStockItem> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var localeId = items.FirstOrDefault().LocaleId;
                var customLabelCodes = items.Select(i => i.SubLabelCode).ToArray();
                var labelCodeMaps = OrdersStockItem.Get().Where(i => customLabelCodes.Contains(i.SubLabelCode)).Select(i => new {i.LabelCode, i.SubLabelCode}).Distinct().ToList();

                var mainLabelCodes = labelCodeMaps.Select(i => i.LabelCode).ToArray();
                OrdersStockItem.StockOutRemoveRange(mainLabelCodes, localeId);

                var stockOuts = items.Where(i => i.StockOutGrossWeight != null && i.StockOutTime != null).ToList();
                if(stockOuts.Count() > 0) {
                    stockOuts.ForEach(i => {
                        var _code =  labelCodeMaps.Where(s => s.SubLabelCode == i.SubLabelCode).FirstOrDefault();
                        if(_code != null ) {
                            i.LabelCode = _code.LabelCode;
                        }
                    });
                    OrdersStockItem.StockOutCreateRange(stockOuts);
                }

                UnitOfWork.Commit();
            }
            catch(Exception e)
            {
                UnitOfWork.Rollback();
            }
            return items;
        }
    }
}