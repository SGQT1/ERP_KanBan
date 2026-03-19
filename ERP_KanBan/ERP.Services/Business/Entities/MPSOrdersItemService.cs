using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSOrdersItemService : BusinessService
    {
        private ERP.Services.Entities.MpsOrdersService MPSOrders { get; set; }
        private ERP.Services.Entities.MpsOrdersItemService MPSOrdersItem { get; set; }

        public MPSOrdersItemService(
            ERP.Services.Entities.MpsOrdersItemService mpsOrdersitemService,
            ERP.Services.Entities.MpsOrdersService mpsOrdersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSOrders = mpsOrdersService;
            MPSOrdersItem = mpsOrdersitemService;
        }

        public IQueryable<Models.Views.MPSOrdersItem> Get()
        {
            var items = (
                from mi in MPSOrdersItem.Get()
                join m in MPSOrders.Get() on new { MpsOrdersId = mi.MpsOrdersId, LocaleId = mi.LocaleId } equals new { MpsOrdersId = m.Id, LocaleId = m.LocaleId }
                select new Models.Views.MPSOrdersItem
                {
                    Id = mi.Id,
                    MpsOrdersId = mi.MpsOrdersId,
                    ArticleSize = mi.ArticleSize,
                    ArticleSizeSuffix = mi.ArticleSizeSuffix,
                    ArticleInnerSize = mi.ArticleInnerSize,
                    DisplaySize = mi.DisplaySize,
                    OrderQty = mi.OrderQty,
                    Qty = mi.Qty,
                    KnifeDisplaySize = mi.KnifeDisplaySize,
                    KnifeInnerSize = mi.KnifeInnerSize,
                    OutsoleDisplaySize = mi.OutsoleDisplaySize,
                    OutsoleInnerSize = mi.OutsoleInnerSize,
                    LastDisplaySize = mi.LastDisplaySize,
                    LastInnerSize = mi.LastInnerSize,
                    ShellDisplaySize = mi.ShellDisplaySize,
                    ShellInnerSize = mi.ShellInnerSize,
                    Other1SizeDesc = mi.Other1SizeDesc,
                    Other1InnerSize = mi.Other1InnerSize,
                    Other2SizeDesc = mi.Other2SizeDesc,
                    Other2InnerSize = mi.Other2InnerSize,
                    ModifyUserName = mi.ModifyUserName,
                    LastUpdateTime = mi.LastUpdateTime,
                    LocaleId = mi.LocaleId,
                    OrderNo = m.OrderNo
                });
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MPSOrdersItem> items)
        {
            // MPSOrdersItem.CreateRange(BuildRange(items));
            MPSOrdersItem.CreateRangeKeepId(BuildRange(items)); // MpsDailyMaterialItem會參考MPSOrderItemId, 所以要用KeepId這個方式
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsOrdersItem, bool>> predicate)
        {
            MPSOrdersItem.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MpsOrdersItem> BuildRange(IEnumerable<Models.Views.MPSOrdersItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsOrdersItem
            {
                Id = item.Id,
                MpsOrdersId = item.MpsOrdersId,
                ArticleSize = item.ArticleSize,
                ArticleSizeSuffix = item.ArticleSizeSuffix,
                ArticleInnerSize = item.ArticleInnerSize,
                DisplaySize = item.DisplaySize,
                OrderQty = item.OrderQty,
                Qty = item.Qty,
                KnifeDisplaySize = item.KnifeDisplaySize,
                KnifeInnerSize = item.KnifeInnerSize,
                OutsoleDisplaySize = item.OutsoleDisplaySize,
                OutsoleInnerSize = item.OutsoleInnerSize,
                LastDisplaySize = item.LastDisplaySize,
                LastInnerSize = item.LastInnerSize,
                ShellDisplaySize = item.ShellDisplaySize,
                ShellInnerSize = item.ShellInnerSize,
                Other1SizeDesc = item.Other1SizeDesc,
                Other1InnerSize = item.Other1InnerSize,
                Other2SizeDesc = item.Other2SizeDesc,
                Other2InnerSize = item.Other2InnerSize,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            });
        }

    }
}
