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

    public class MPSStyleItemUsageService : BusinessService
    {
        private ERP.Services.Entities.MpsStyleItemService MPSStyleItem { get; set; }
        private ERP.Services.Entities.MpsStyleItemUsageService MPSStyleItemUsage { get; set; }

        public MPSStyleItemUsageService(
            ERP.Services.Entities.MpsStyleItemService mpsStyleItemService,
            ERP.Services.Entities.MpsStyleItemUsageService mpsStyleItemUsageService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSStyleItem = mpsStyleItemService;
            MPSStyleItemUsage = mpsStyleItemUsageService;
        }

        public IQueryable<Models.Views.MPSStyleItemUsage> Get()
        {
            var result = (
                from si in MPSStyleItem.Get()
                join su in MPSStyleItemUsage.Get() on new { MPSStyleItemId = si.Id, LocaleId = si.LocaleId } equals new { MPSStyleItemId = su.MpsStyleItemId, LocaleId = su.LocaleId } into suGRP
                from su in suGRP.DefaultIfEmpty()
                select new Models.Views.MPSStyleItemUsage
                {
                    Id = su.Id,
                    MpsStyleItemId = su.MpsStyleItemId,
                    ArticleSize = su.ArticleSize,
                    ArticleSizeSuffix = su.ArticleSizeSuffix,
                    ArticleInnerSize = su.ArticleInnerSize,
                    PreUnitUsage = su.PreUnitUsage,
                    UnitUsage = su.UnitUsage,
                    ModifyUserName = su.ModifyUserName,
                    LastUpdateTime = su.LastUpdateTime,
                    LocaleId = su.LocaleId,
                    MpsStyleId = si.MpsStyleId,
                    StyleItemId = si.Id,
                });
            return result;
        }

        public void CreateRange(IEnumerable<Models.Views.MPSStyleItemUsage> items)
        {
            MPSStyleItemUsage.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsStyleItemUsage, bool>> predicate)
        {
            MPSStyleItemUsage.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsStyleItemUsage> BuildRange(IEnumerable<Models.Views.MPSStyleItemUsage> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsStyleItemUsage
            {
                Id = item.Id,
                MpsStyleItemId = item.MpsStyleItemId,
                ArticleSize = item.ArticleSize,
                ArticleSizeSuffix = item.ArticleSizeSuffix,
                ArticleInnerSize = item.ArticleInnerSize,
                PreUnitUsage = item.PreUnitUsage,
                UnitUsage = item.UnitUsage,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            });
        }

    }
}
