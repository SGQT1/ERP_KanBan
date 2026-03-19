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

    public class MPSLiveItemSizeService : BusinessService
    {
        private ERP.Services.Entities.MpsLiveService MPSLive { get; set; }
        private ERP.Services.Entities.MpsLiveItemService MPSLiveItem { get; set; }
        private ERP.Services.Entities.MpsLiveItemSizeService MPSLiveItemSize { get; set; }
        private ERP.Services.Entities.MpsOrdersItemService MPSOrdersItem { get; set; }

        public MPSLiveItemSizeService(
            ERP.Services.Entities.MpsLiveService mpsLiveService,
            ERP.Services.Entities.MpsLiveItemService mpsLiveItemSize,
            ERP.Services.Entities.MpsLiveItemSizeService mpsLiveItemSizeService,
            ERP.Services.Entities.MpsOrdersItemService mpsOrdersItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSLive = mpsLiveService;
            MPSLiveItem = mpsLiveItemSize;
            MPSLiveItemSize = mpsLiveItemSizeService;
            MPSOrdersItem = mpsOrdersItemService;
        }

        public IQueryable<Models.Views.MPSPlanItemSize> Get()
        {
            var result = (
                from ms in MPSLiveItemSize.Get()
                join oi in MPSOrdersItem.Get() on new { MPSOrderItemId = ms.MpsOrdersItemId, LocaleId = ms.LocaleId } equals new { MPSOrderItemId = oi.Id, LocaleId = oi.LocaleId }
                select new Models.Views.MPSPlanItemSize
                {
                    Id = ms.Id,
                    ModifyUserName = ms.ModifyUserName,
                    LastUpdateTime = ms.LastUpdateTime,
                    LocaleId = ms.LocaleId,
                    MPSLiveItemId = ms.MpsLiveItemId,
                    MPSOrdersItemId = ms.MpsOrdersItemId,
                    SubQty = ms.SubQty,
                    SeqId = ms.MpsLiveItemId,

                    MPSOrdersId = oi.MpsOrdersId,
                    ArticleInnerSize = oi.ArticleInnerSize,
                    DisplaySize = oi.DisplaySize,
                    KnifeDisplaySize = oi.KnifeDisplaySize,
                    OutsoleDisplaySize = oi.OutsoleDisplaySize,
                    LastDisplaySize = oi.LastDisplaySize,
                    ShellDisplaySize = oi.ShellDisplaySize,
                    Other1SizeDesc = oi.Other1SizeDesc,
                    Other2SizeDesc = oi.Other2SizeDesc,
                }
            );
            return result;
            // return MPSLiveItemSize.Get().Select(i => new Models.Views.MPSPlanItemSize
            // {
            //     Id = i.Id,
            //     ModifyUserName = i.ModifyUserName,
            //     LastUpdateTime = i.LastUpdateTime,
            //     LocaleId = i.LocaleId,
            //     MPSLiveItemId = i.MpsLiveItemId,
            //     MPSOrdersItemId = i.MpsOrdersItemId,
            //     SubQty = i.SubQty,
            // });
        }

        public void CreateRange(IEnumerable<Models.Views.MPSPlanItemSize> items)
        {
            MPSLiveItemSize.CreateRangeKeepId(BuildRange(items));  // Id is be Referenced
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsLiveItemSize, bool>> predicate)
        {
            MPSLiveItemSize.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MpsLiveItemSize> BuildRange(IEnumerable<Models.Views.MPSPlanItemSize> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsLiveItemSize
            {
                Id = item.Id,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
                MpsLiveItemId = item.MPSLiveItemId,
                MpsOrdersItemId = item.MPSOrdersItemId,
                SubQty = item.SubQty,
            });
        }

    }
}
