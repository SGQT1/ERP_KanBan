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
    public class MaterialStockSizeService : BusinessService
    {
        private Services.Entities.MaterialStockSizeService MaterialStockSize { get; }
        private Services.Entities.StockIOService StockIO { get; }
        private Services.Entities.StockIOSizeService StockIOSize { get; }

        public MaterialStockSizeService(
            Services.Entities.MaterialStockSizeService materialStockSizeService,
            Services.Entities.StockIOService stockIOService,
            Services.Entities.StockIOSizeService stockIOSizeService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.MaterialStockSize = materialStockSizeService;
            this.StockIO = stockIOService;
            this.StockIOSize = stockIOSizeService;
        }
        public IQueryable<Models.Views.MaterialStockSize> Get()
        {
            return MaterialStockSize.Get().Select(i => new Models.Views.MaterialStockSize
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialStockId = i.MaterialStockId,
                ShoeSize = i.ShoeSize,
                ShoeSizeSuffix = i.ShoeSizeSuffix,
                ShoeInnerSize = i.ShoeInnerSize,
                PCLQty = i.PCLQty,
                PurQty = i.PurQty,
                DisplaySize = i.DisplaySize,
            });

        }
        public void CreateRange(IEnumerable<Models.Views.MaterialStockSize> items)
        {
            MaterialStockSize.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MaterialStockSize, bool>> predicate)
        {
            MaterialStockSize.RemoveRange(predicate);
        }

        public void UpdateSizeQty(int id, int localeId)
        {
            var sizeItems = (
                    from mi in StockIO.Get()
                    join mis in StockIOSize.Get() on new { MaterialStockItemId = mi.Id, LocaleId = mi.LocaleId } equals new { MaterialStockItemId = mis.StockIOId, LocaleId = mis.LocaleId }
                    where mi.MaterialStockId == id && mi.LocaleId == localeId
                    select new
                    {
                        LocaleId = mi.LocaleId,
                        MaterialStockId = mi.MaterialStockId,
                        ShoeSize = mis.ShoeSize,
                        ShoeSizeSuffix = mis.ShoeSizeSuffix,
                        ShoeInnerSize = mis.ShoeInnerSize,
                        PCLQty = mis.PCLQty,
                        PurQty = mis.PurQty,
                        DisplaySize = mis.DisplaySize,
                    }
                )
                .GroupBy(g => new { g.LocaleId, g.MaterialStockId, g.ShoeSize, g.ShoeSizeSuffix, g.ShoeInnerSize })
                .Select(i => new Models.Entities.MaterialStockSize
                {
                    LocaleId = i.Key.LocaleId,
                    MaterialStockId = i.Key.MaterialStockId,
                    ShoeSize = i.Key.ShoeSize,
                    ShoeSizeSuffix = i.Key.ShoeSizeSuffix,
                    ShoeInnerSize = i.Key.ShoeInnerSize,
                    PCLQty = i.Sum(g => g.PCLQty),
                    PurQty = i.Sum(g => g.PurQty),
                    // DisplaySize = i.Key.DisplaySize, 
                    DisplaySize = i.Max(g => g.DisplaySize) //印位Display會自動把4.0轉乘4, 造成兩比重複資料，這裡刪掉materialStockSize後，只加入一筆
                })
                .Distinct()
                .ToList();

            MaterialStockSize.RemoveRange(i => i.MaterialStockId == id && i.LocaleId == localeId);
            MaterialStockSize.CreateRange(sizeItems);
        }
        private IEnumerable<ERP.Models.Entities.MaterialStockSize> BuildRange(IEnumerable<Models.Views.MaterialStockSize> items)
        {
            return items.Select(item => new ERP.Models.Entities.MaterialStockSize
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MaterialStockId = item.MaterialStockId,
                ShoeSize = item.ShoeSize,
                ShoeSizeSuffix = item.ShoeSizeSuffix,
                ShoeInnerSize = item.ShoeInnerSize,
                PCLQty = item.PCLQty,
                PurQty = item.PurQty,
                DisplaySize = item.DisplaySize,
            });
        }
    }
}