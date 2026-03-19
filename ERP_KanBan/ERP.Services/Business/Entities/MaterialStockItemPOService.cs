using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MaterialStockItemPOService : BusinessService
    {
        private ERP.Services.Entities.ProcessPOService ProcessPO { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.StockIOService StockIO { get; set; }

        public MaterialStockItemPOService(
            ERP.Services.Entities.ProcessPOService processPOService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.StockIOService stockIOService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            ProcessPO = processPOService;
            POItem = poItemService;
            Material = materialService;
            StockIO = stockIOService;
        }
        public IQueryable<Models.Views.MaterialStockItemPO> Get()
        {
            var result = (
                from sp in ProcessPO.Get()
                join pi in POItem.Get() on new { POItemId = sp.POItemId, LocaleId = sp.LocaleId } equals new { POItemId = pi.Id, LocaleId = pi.LocaleId }
                join si in StockIO.Get() on new { StockIOId = sp.StockIOId, LocaleId = sp.LocaleId } equals new { StockIOId = si.Id, LocaleId = si.LocaleId }
                select new Models.Views.MaterialStockItemPO
                {
                    Id = sp.Id,
                    LocaleId = sp.LocaleId,
                    POId = sp.POId,
                    POItemId = sp.POItemId,
                    StockIOId = sp.StockIOId,
                    MaterialCost = sp.MaterialCost,
                    StockDollarCodeId = sp.StockDollarCodeId,
                    OrderNo = sp.OrderNo,
                    OPCount = sp.OPCount,
                    ModifyUserName = sp.ModifyUserName,
                    LastUpdateTime = sp.LastUpdateTime,
                    ParentMaterialId = pi.MaterialId,
                    ParentMaterialNameTw = Material.Get().Where(i => i.Id == pi.MaterialId && i.LocaleId == pi.LocaleId).Max(i => i.MaterialName),
                    ParentMaterialNameEng = Material.Get().Where(i => i.Id == pi.MaterialId && i.LocaleId == pi.LocaleId).Max(i => i.MaterialNameEng),
                    MaterialId = si.MaterialId,
                    MaterialNameTw = Material.Get().Where(i => i.Id == si.MaterialId && i.LocaleId == si.LocaleId).Max(i => i.MaterialName),
                    MaterialNameEng = Material.Get().Where(i => i.Id == si.MaterialId && i.LocaleId == si.LocaleId).Max(i => i.MaterialNameEng),
                    PurQty = pi.Qty,
                    PONo = pi.PONo,
                    POType = pi.POType,
                    UnitPrice = si.PCLIOQty < 0 ? sp.MaterialCost / (0-si.PCLIOQty) : sp.MaterialCost / (si.PCLIOQty),
                    StockOutQty = si.PCLIOQty,
                }
            );
            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.MaterialStockItemPO> items)
        {
            ProcessPO.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.ProcessPO, bool>> predicate)
        {
            ProcessPO.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.ProcessPO> BuildRange(IEnumerable<Models.Views.MaterialStockItemPO> items)
        {
            return items.Select(item => new ERP.Models.Entities.ProcessPO
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                POId = item.POId,
                POItemId = item.POItemId,
                StockIOId = item.StockIOId,
                MaterialCost = item.MaterialCost,
                StockDollarCodeId = item.StockDollarCodeId,
                OrderNo = item.OrderNo,
                OPCount = item.OPCount,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }


    }
}