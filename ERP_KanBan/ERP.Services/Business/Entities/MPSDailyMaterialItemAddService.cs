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

    public class MPSDailyMaterialItemAddService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyMaterialAddService MPSDailyMaterialAdd { get; set; }
        private ERP.Services.Entities.MpsDailyMaterialItemAddService MPSDailyMaterialItemAdd { get; set; }

        public MPSDailyMaterialItemAddService(
            ERP.Services.Entities.MpsDailyMaterialAddService mpsDailyMaterialAdd,
            ERP.Services.Entities.MpsDailyMaterialItemAddService mpsDailyMaterialItemAddService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyMaterialAdd = mpsDailyMaterialAdd;
            MPSDailyMaterialItemAdd = mpsDailyMaterialItemAddService;
        }

        public IQueryable<Models.Views.MPSDailyMaterialItemAdd> Get()
        {
            // 暫時取用 MPSStyleItem
            var items = (
                from mi in MPSDailyMaterialItemAdd.Get()
                join m in MPSDailyMaterialAdd.Get() on new { MPSDailyMaterialId = mi.MpsDailyMaterialAddId, LocaleId = mi.LocaleId } equals new { MPSDailyMaterialId = m.Id, LocaleId = m.LocaleId }
                select new Models.Views.MPSDailyMaterialItemAdd
                {
                    Id = mi.Id,
                    LocaleId = mi.LocaleId,
                    MpsDailyMaterialAddId = mi.MpsDailyMaterialAddId,
                    ArticleInnerSize = mi.ArticleInnerSize,
                    DisplaySize = mi.DisplaySize,
                    LSubQty = mi.LSubQty,
                    RSubQty = mi.RSubQty,
                    SubQty = mi.SubQty,
                    UnitUsage = mi.UnitUsage,
                    SubUsage = mi.SubUsage,
                    PreSubUsage = mi.PreSubUsage,
                    ModifyUserName = mi.ModifyUserName,
                    LastUpdateTime = mi.LastUpdateTime,

                    MpsStyleItemId = m.MpsStyleItemId,
                }
            );
            return items;
        }

        public void CreateRange(IEnumerable<Models.Views.MPSDailyMaterialItemAdd> items)
        {
            MPSDailyMaterialItemAdd.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsDailyMaterialItemAdd, bool>> predicate)
        {
            MPSDailyMaterialItemAdd.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyMaterialItemAdd> BuildRange(IEnumerable<Models.Views.MPSDailyMaterialItemAdd> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyMaterialItemAdd
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsDailyMaterialAddId = item.MpsDailyMaterialAddId,
                ArticleInnerSize = item.ArticleInnerSize,
                DisplaySize = item.DisplaySize,
                LSubQty = item.LSubQty,
                RSubQty = item.RSubQty,
                SubQty = item.SubQty,
                UnitUsage = item.UnitUsage,
                SubUsage = item.SubUsage,
                PreSubUsage = item.PreSubUsage,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

    }
}
