using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ShipmentItemService : BusinessService
    {
        private Services.Entities.SaleItemService SaleItem { get; }
        public ShipmentItemService(
            Services.Entities.SaleItemService saleItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            SaleItem = saleItemService;
        }

        public IQueryable<Models.Views.ShipmentItem> Get()
        {
            var items = (
                from si in SaleItem.Get()
                select new Models.Views.ShipmentItem
                {
                    Id = si.Id,
                    LocaleId = si.LocaleId,
                    SaleId = si.SaleId,
                    OrdersItemId = si.OrdersItemId,
                    ArticleSize = si.ArticleSize,
                    ArticleSizeSuffix = si.ArticleSizeSuffix,
                    ArticleInnerSize = si.ArticleInnerSize,
                    DisplaySize = si.DisplaySize,
                    SaleQty = si.SaleQty,
                    UnitPrice = si.UnitPrice,
                    ToolingFund = si.ToolingFund,
                    ToolingCost = si.ToolingCost,
                    ModifyUserName = si.ModifyUserName,
                    LastUpdateTime = si.LastUpdateTime,
                }
            );
            return items.OrderBy(i => i.ArticleInnerSize);
        }
        public void CreateRange(IEnumerable<Models.Views.ShipmentItem> items)
        {
            SaleItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(int saleId, int localeId)
        {
            SaleItem.RemoveRange(i => i.SaleId == saleId && i.LocaleId == localeId);
        }

        public IEnumerable<Models.Entities.SaleItem> BuildRange(IEnumerable<Models.Views.ShipmentItem> items)
        {
            return items.Select(item => new Models.Entities.SaleItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                SaleId = item.SaleId,
                OrdersItemId = item.OrdersItemId,
                ArticleSize = item.ArticleSize,
                ArticleSizeSuffix = item.ArticleSizeSuffix,
                ArticleInnerSize = item.ArticleInnerSize,
                DisplaySize = item.DisplaySize,
                SaleQty = item.SaleQty,
                UnitPrice = item.UnitPrice,
                ToolingFund = item.ToolingFund,
                ToolingCost = item.ToolingCost,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}