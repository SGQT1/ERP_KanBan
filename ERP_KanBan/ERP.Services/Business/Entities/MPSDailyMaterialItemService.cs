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

    public class MPSDailyMaterialItemService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyMaterialService MPSDailyMaterial { get; set; }
        private ERP.Services.Entities.MpsDailyMaterialItemService MPSDailyMaterialItem { get; set; }
        private ERP.Services.Entities.MpsOrdersItemService MPSOrdersItem { get; set; }


        public MPSDailyMaterialItemService(
            ERP.Services.Entities.MpsDailyMaterialService mpsDailyMaterial,
            ERP.Services.Entities.MpsDailyMaterialItemService mpsDailyMaterialItemService,
            ERP.Services.Entities.MpsOrdersItemService mpsOrdersItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyMaterial = mpsDailyMaterial;
            MPSDailyMaterialItem = mpsDailyMaterialItemService;
            MPSOrdersItem = mpsOrdersItemService;
        }

        public IQueryable<Models.Views.MPSDailyMaterialItem> Get()
        {
            // 暫時取用 MPSStyleItem
            var items = (
                from mi in MPSDailyMaterialItem.Get()
                join m in MPSDailyMaterial.Get() on new { MPSDailyMaterialId = mi.MpsDailyMaterialId, LocaleId = mi.LocaleId } equals new { MPSDailyMaterialId = m.Id, LocaleId = m.LocaleId }
                join oi in MPSOrdersItem.Get() on new { MpsOrdersItemId = mi.MpsOrdersItemId, LocaleId = mi.LocaleId } equals new { MpsOrdersItemId = oi.Id, LocaleId = oi.LocaleId }
                select new Models.Views.MPSDailyMaterialItem
                {
                    Id = mi.Id,
                    LocaleId = mi.LocaleId,
                    MpsDailyMaterialId = mi.MpsDailyMaterialId,
                    SubQty = mi.SubQty,
                    UnitUsage = mi.UnitUsage,
                    SubUsage = mi.SubUsage,
                    PreSubUsage = mi.PreSubUsage,
                    MpsOrdersItemId = mi.MpsOrdersItemId,
                    ModifyUserName = mi.ModifyUserName,
                    LastUpdateTime = mi.LastUpdateTime,

                    ArticleInnerSize = oi.ArticleInnerSize,
                    ArticeSize = oi.DisplaySize,
                    MpsStyleItemId = m.MpsStyleItemId,
                }
            );
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MPSDailyMaterialItem> items)
        {
            MPSDailyMaterialItem.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsDailyMaterialItem, bool>> predicate)
        {
            MPSDailyMaterialItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyMaterialItem> BuildRange(IEnumerable<Models.Views.MPSDailyMaterialItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyMaterialItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsDailyMaterialId = item.MpsDailyMaterialId,
                SubQty = item.SubQty,
                UnitUsage = item.UnitUsage,
                SubUsage = item.SubUsage,
                PreSubUsage = item.PreSubUsage,
                MpsOrdersItemId = item.MpsOrdersItemId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

    }
}
