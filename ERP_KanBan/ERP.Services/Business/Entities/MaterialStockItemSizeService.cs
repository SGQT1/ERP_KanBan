using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MaterialStockItemSizeService : BusinessService
    {
        private Services.Entities.MaterialStockService MaterialStock { get; }
        private Services.Entities.StockIOService StockIO { get; }
        private Services.Entities.StockIOSizeService StockIOSize { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.ReceivedLogSizeItemService ReceivedLogSizeItem { get; }

        public MaterialStockItemSizeService(
            Services.Entities.MaterialStockService materialStock,
            Services.Entities.StockIOService stockIOService,
            Services.Entities.StockIOSizeService stockIOSizeService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.ReceivedLogSizeItemService receivedLogSizeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.MaterialStock = materialStock;
            this.StockIO = stockIOService;
            this.StockIOSize = stockIOSizeService;
            this.CodeItem = codeItemService;
            this.ReceivedLogSizeItem = receivedLogSizeItemService;

        }
        public IQueryable<Models.Views.MaterialStockItemSize> Get()
        {
            return StockIOSize.Get().Select(i => new Models.Views.MaterialStockItemSize
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                StockIOId = i.StockIOId,
                ShoeSize = i.ShoeSize,
                ShoeSizeSuffix = i.ShoeSizeSuffix,
                ShoeInnerSize = i.ShoeInnerSize,
                PCLQty = i.PCLQty,
                PurQty = i.PurQty,
                ReLogSizeItemId = i.ReLogSizeItemId,
                DisplaySize = i.DisplaySize,
            });

        }
        public void CreateRange(IEnumerable<Models.Views.MaterialStockItemSize> items)
        {
            StockIOSize.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.StockIOSize, bool>> predicate)
        {
            StockIOSize.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.StockIOSize> BuildRange(IEnumerable<Models.Views.MaterialStockItemSize> items)
        {
            return items.Select(item => new ERP.Models.Entities.StockIOSize
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                StockIOId = item.StockIOId,
                ShoeSize = item.ShoeSize,
                ShoeSizeSuffix = item.ShoeSizeSuffix,
                ShoeInnerSize = item.ShoeInnerSize,
                PCLQty = item.PCLQty,
                PurQty = item.PurQty,
                ReLogSizeItemId = item.ReLogSizeItemId,
                DisplaySize = item.DisplaySize,
            });
        }
    }
}