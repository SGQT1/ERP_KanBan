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
    public class OrdersItemService : BusinessService
    {
        private Services.Entities.OrdersItemService OrdersItem { get; }
        private Services.Business.Entities.OrdersService Orders { get; }
        private Services.Entities.SizeCountryMappingService SizeCountryMapping { get; }

        public OrdersItemService(
            Services.Entities.OrdersItemService ordersItemService,

            Services.Business.Entities.OrdersService ordersService,
            Services.Entities.SizeCountryMappingService sizeCountryMappingService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            OrdersItem = ordersItemService;
            Orders = ordersService;
            SizeCountryMapping = sizeCountryMappingService;
        }
        public IQueryable<Models.Views.OrdersItem> Get()
        {
            return OrdersItem.Get().Select(i => new Models.Views.OrdersItem
            {
                Id = i.Id,
                OrdersId = i.OrdersId,
                ArticleSize = i.ArticleSize,
                ArticleSizeSuffix = i.ArticleSizeSuffix,
                ArticleInnerSize = i.ArticleInnerSize,
                DisplaySize = i.DisplaySize,
                UnitPrice = i.UnitPrice,
                Qty = i.Qty,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                TransferUnitPrice = i.TransferUnitPrice,
                TransferQty = i.TransferQty,
                ToolingFund = i.ToolingFund,
                ToolingCost = i.ToolingCost,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.OrdersItem> ordersItems)
        {
            OrdersItem.CreateRange(BuildRange(ordersItems));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.OrdersItem, bool>> predicate)
        {
            OrdersItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.OrdersItem> BuildRange(IEnumerable<Models.Views.OrdersItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.OrdersItem
            {
                Id = item.Id,
                OrdersId = item.OrdersId,
                ArticleSize = item.ArticleSize,
                ArticleSizeSuffix = item.ArticleSizeSuffix,
                ArticleInnerSize = item.ArticleInnerSize,
                DisplaySize = item.DisplaySize,
                UnitPrice = item.UnitPrice,
                Qty = item.Qty,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
                LocaleId = item.LocaleId,
                //MixedQty1 = item.MixedQty1,
                //MixedQty2 = item.MixedQty2,
                //MixedQty3 = item.MixedQty3,
                //MixedQty4 = item.MixedQty4,
                //MixedQty5 = item.MixedQty5,
                TransferUnitPrice = item.TransferUnitPrice,
                TransferQty = item.TransferQty,
                ToolingFund = item.ToolingFund,
                ToolingCost = item.ToolingCost,
            });
        }

        /*
         * Get OrdersItem Mapping Country Size in ShoeSize,ShoeSizeSuffix
         */
        public IEnumerable<Models.Views.OrdersItem> GetSizeMapping(int ordersId, int localeId)
        {
            var orders = Orders.Get().Where(i => i.Id == ordersId && i.LocaleId == localeId).FirstOrDefault();
            var ordersItem = Get().Where(i => i.OrdersId == ordersId & i.LocaleId == localeId).ToList();
            var mapType = 0;
            if (orders != null && ordersItem.Count() > 0)
            {
                // 先用ArticleSize比，如果有資料 MapType＝0,沒有就是1同時在找尋一次用OrderSize比
                var sizeCountry = SizeCountryMapping.Get().Where(i => i.SizeCountryCodeId == orders.ArticleSizeCountryCodeId && i.MappingCodeId == orders.OrderSizeCountryCodeId && i.LocaleId == localeId).ToList();
                if (sizeCountry.Count == 0)
                {
                    mapType = 1;
                    sizeCountry = SizeCountryMapping.Get().Where(i => i.SizeCountryCodeId == orders.OrderSizeCountryCodeId && i.MappingCodeId == orders.ArticleSizeCountryCodeId && i.LocaleId == localeId).ToList();
                }
                
                if (orders.ArticleSizeCountryCodeId != orders.OrderSizeCountryCodeId)
                {
                    if( mapType == 0 ) {
                        ordersItem = (
                            from oi in ordersItem 
                            join s in sizeCountry on new { ArticleInnerSize = oi.ArticleInnerSize } equals new { ArticleInnerSize = s.InnerShoeSize } into sGRP 
                            from s in sGRP.DefaultIfEmpty() 
                            select new Models.Views.OrdersItem
                            {
                                Id = oi.Id,
                                OrdersId = oi.OrdersId,
                                ArticleSize = oi.ArticleSize,
                                ArticleSizeSuffix = oi.ArticleSizeSuffix,
                                ArticleInnerSize = oi.ArticleInnerSize,
                                DisplaySize = oi.DisplaySize,
                                UnitPrice = oi.UnitPrice,
                                Qty = oi.Qty,
                                ModifyUserName = oi.ModifyUserName,
                                LastUpdateTime = oi.LastUpdateTime,
                                LocaleId = oi.LocaleId,
                                TransferUnitPrice = oi.TransferUnitPrice,
                                TransferQty = oi.TransferQty,
                                ToolingFund = oi.ToolingFund,
                                ToolingCost = oi.ToolingCost,

                                OrderSize = s != null ? s.MappingSize : 0,
                                OrderSizeSuffix = s != null && s.MappingSizeSuffix != null ? s.MappingSizeSuffix : "",
                            }
                        ).ToList();
                    }
                    else {
                        ordersItem = (
                            from oi in ordersItem 
                            join s in sizeCountry on new { ArticleInnerSize = oi.ArticleInnerSize } equals new { ArticleInnerSize = s.InnerMappingSize } into sGRP from s in sGRP.DefaultIfEmpty() select new Models.Views.OrdersItem
                            {
                                Id = oi.Id,
                                OrdersId = oi.OrdersId,
                                ArticleSize = oi.ArticleSize,
                                ArticleSizeSuffix = oi.ArticleSizeSuffix,
                                ArticleInnerSize = oi.ArticleInnerSize,
                                DisplaySize = oi.DisplaySize,
                                UnitPrice = oi.UnitPrice,
                                Qty = oi.Qty,
                                ModifyUserName = oi.ModifyUserName,
                                LastUpdateTime = oi.LastUpdateTime,
                                LocaleId = oi.LocaleId,
                                TransferUnitPrice = oi.TransferUnitPrice,
                                TransferQty = oi.TransferQty,
                                ToolingFund = oi.ToolingFund,
                                ToolingCost = oi.ToolingCost,

                                OrderSize = s != null ? s.ShoeSize : 0,
                                OrderSizeSuffix = s != null && s.ShoeSizeSuffix != null ? s.ShoeSizeSuffix : "",
                            }
                        ).ToList();

                    }

                }
                else
                {
                    ordersItem.ForEach(i =>
                    {
                        i.OrderSize = i.ArticleSize;
                        i.OrderSizeSuffix = i.ArticleSizeSuffix;
                    });
                }
            }
            return ordersItem.OrderBy(i => i.ArticleInnerSize);
        }
    }
}