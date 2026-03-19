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
    public class PurOrdersItemService : BusinessService
    {
        private ERP.Services.Entities.PurOrdersItemService PurOrdersItem { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }

        public PurOrdersItemService(
            ERP.Services.Entities.PurOrdersItemService purOrdersItemService,
            ERP.Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PurOrdersItem = purOrdersItemService;
            Orders = ordersService;
        }
        public IQueryable<Models.Views.PurOrdersItem> Get()
        {
            var result = (
                from pi in PurOrdersItem.Get()
                join o in Orders.Get() on new { Id = pi.OrdersId, LocaleId = pi.LocaleId } equals new { Id = o.Id, LocaleId = o.LocaleId }
                select new Models.Views.PurOrdersItem
                {
                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    PurBatchId = pi.PurBatchId,
                    OrdersId = pi.OrdersId,
                    ModifyUserName = pi.ModifyUserName,
                    LastUpdateTime = pi.LastUpdateTime,
                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    CompanyId = o.CompanyId,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    CSD = o.CSD,
                    ETD = o.ETD,
                    LCSD = o.LCSD,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    RefLocaleId = o.LocaleId,
                    CustomerOrderNo = o.CustomerOrderNo,
                    OWD = o.OWD,
                }
            );

            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.PurOrdersItem> items)
        {
            PurOrdersItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.PurOrdersItem, bool>> predicate)
        {
            PurOrdersItem.RemoveRange(predicate);
        }

        public IEnumerable<Models.Entities.PurOrdersItem> BuildRange(IEnumerable<Models.Views.PurOrdersItem> items)
        {
            return items.Select(item => new Models.Entities.PurOrdersItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                PurBatchId = item.PurBatchId,
                OrdersId = item.OrdersId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}