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

    public class MPSLiveItemService : BusinessService
    {
        private ERP.Services.Entities.MpsLiveService MPSLive { get; set; }
        private ERP.Services.Entities.MpsLiveItemService MPSLiveItem { get; set; }
        private ERP.Services.Entities.MpsLiveItemSizeService MPSLiveItemSize { get; set; }

        public MPSLiveItemService(
            ERP.Services.Entities.MpsLiveService mpsLiveService,
            ERP.Services.Entities.MpsLiveItemService mpsLiveItemSize,
            ERP.Services.Entities.MpsLiveItemSizeService mpsLiveItemSizeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSLive = mpsLiveService;
            MPSLiveItem = mpsLiveItemSize;
            MPSLiveItemSize = mpsLiveItemSizeService;
        }

        public IQueryable<Models.Views.MPSPlanItem> Get()
        {
            var items = (
                from mi in MPSLiveItem.Get()
                join m in MPSLive.Get() on new { MPSLiveId = mi.MpsLiveId, LocaleId = mi.LocaleId } equals new { MPSLiveId = m.Id, LocaleId = m.LocaleId }
                select new Models.Views.MPSPlanItem
                {
                    Id = mi.Id,
                    MPSLiveId = mi.MpsLiveId,
                    PlanDate = mi.PlanDate,
                    PlanQty = mi.PlanQty,
                    ModifyUserName = mi.ModifyUserName,
                    LastUpdateTime = mi.LastUpdateTime,
                    LocaleId = mi.LocaleId,
                    HasSize = MPSLiveItemSize.Get().Where(i => i.MpsLiveItemId == mi.Id && i.LocaleId == mi.LocaleId).Count(),
                    SeqId = mi.Id,
                });
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MPSPlanItem> items)
        {
            MPSLiveItem.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsLiveItem, bool>> predicate)
        {
            MPSLiveItem.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MpsLiveItem> BuildRange(IEnumerable<Models.Views.MPSPlanItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsLiveItem
            {
                Id = item.Id,
                MpsLiveId = item.MPSLiveId,
                PlanDate = item.PlanDate,
                PlanQty = item.PlanQty,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId
            });
        }

        public Models.Views.MPSPlanItem Create(Models.Views.MPSPlanItem item)
        {
            var _item = MPSLiveItem.CreateKeepId(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault(); //新增也要保留Id
        }
        private ERP.Models.Entities.MpsLiveItem Build(Models.Views.MPSPlanItem item)
        {
            return new ERP.Models.Entities.MpsLiveItem
            {
                Id = item.Id,
                MpsLiveId = item.MPSLiveId,
                PlanDate = item.PlanDate,
                PlanQty = item.PlanQty,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId
            };
        }

    }
}
