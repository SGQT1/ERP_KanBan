using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ArticleSizeRunService : BusinessService
    {
        private Services.Entities.ArticleSizeRunService ArticleSizeRun { get; }

        public ArticleSizeRunService(
            Services.Entities.ArticleSizeRunService articleSizeRunService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.ArticleSizeRun = articleSizeRunService;
        }
        public IQueryable<Models.Views.ArticleSizeRun> Get()
        {
            return ArticleSizeRun.Get().Select(i => new Models.Views.ArticleSizeRun
            {
                Id = i.Id,
                ArticleId = i.ArticleId,
                ArticleSize = i.ArticleSize,
                ArticleSizeSuffix = i.ArticleSizeSuffix,
                ArticleInnerSize = i.ArticleInnerSize,
                KnifeSize = i.KnifeSize,
                KnifeSizeSuffix = i.KnifeSizeSuffix,
                KnifeInnerSize = i.KnifeInnerSize,
                KnifeDisplaySize = i.KnifeDisplaySize,
                OutsoleSize = i.OutsoleSize,
                OutsoleSizeSuffix = i.OutsoleSizeSuffix,
                OutsoleInnerSize = i.OutsoleInnerSize,
                OutsoleDisplaySize = i.OutsoleDisplaySize,
                LastSize = i.LastSize,
                LastSizeSuffix = i.LastSizeSuffix,
                LastInnerSize = i.LastInnerSize,
                LastDisplaySize = i.LastDisplaySize,
                ShellSize = i.ShellSize,
                ShellSizeSuffix = i.ShellSizeSuffix,
                ShellInnerSize = i.ShellInnerSize,
                Other1Size = i.Other1Size,
                Other1SizeSuffix = i.Other1SizeSuffix,
                Other1InnerSize = i.Other1InnerSize,
                Other1Desc = i.Other1Desc,
                Other2Size = i.Other2Size,
                Other2SizeSuffix = i.Other2SizeSuffix,
                Other2InnerSize = i.Other2InnerSize,
                Other2SizeDesc = i.Other2SizeDesc,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                ArticleDisplaySize = i.ArticleDisplaySize,
                ShellDisplaySize = i.ShellDisplaySize,
            });
        }

        public void CreateRange(IEnumerable<Models.Views.ArticleSizeRun> items)
        {
            ArticleSizeRun.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.ArticleSizeRun, bool>> predicate)
        {
            ArticleSizeRun.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.ArticleSizeRun> BuildRange(IEnumerable<Models.Views.ArticleSizeRun> items)
        {
            return items.Select(item => new ERP.Models.Entities.ArticleSizeRun
            {
                Id = item.Id,
                ArticleId = item.ArticleId,
                ArticleSize = item.ArticleSize,
                ArticleSizeSuffix = item.ArticleSizeSuffix,
                ArticleInnerSize = item.ArticleSizeSuffix.Contains("J") ? (double)item.ArticleSize : (double)item.ArticleSize * 1000,
                KnifeSize = item.KnifeSize,
                KnifeSizeSuffix = item.KnifeSizeSuffix,
                KnifeInnerSize = item.KnifeSizeSuffix.Contains("J") ? (double)item.KnifeSize : (double)item.KnifeSize * 1000,
                KnifeDisplaySize = item.KnifeDisplaySize,
                OutsoleSize = item.OutsoleSize,
                OutsoleSizeSuffix = item.OutsoleSizeSuffix,
                OutsoleInnerSize = item.OutsoleSizeSuffix.Contains("J") ? (double)item.OutsoleSize : (double)item.OutsoleSize * 1000,
                OutsoleDisplaySize = item.OutsoleDisplaySize,
                LastSize = item.LastSize,
                LastSizeSuffix = item.LastSizeSuffix,
                LastInnerSize = item.LastSizeSuffix.Contains("J") ? (double)item.LastSize : (double)item.LastSize * 1000,
                LastDisplaySize = item.LastDisplaySize,
                ShellSize = item.ShellSize,
                ShellSizeSuffix = item.ShellSizeSuffix,
                ShellInnerSize = item.ShellSizeSuffix.Contains("J") ? (double)item.ShellSize : (double)item.ShellSize * 1000,
                Other1Size = item.Other1Size,
                Other1SizeSuffix = item.Other1SizeSuffix,
                Other1InnerSize = item.Other1SizeSuffix.Contains("J") ? (double)item.Other1Size : (double)item.Other1Size * 1000,
                Other1Desc = item.Other1Desc,
                Other2Size = item.Other2Size,
                Other2SizeSuffix = item.Other2SizeSuffix,
                Other2InnerSize = item.Other2SizeSuffix.Contains("J") ? (double)item.Other2Size : (double)item.Other2Size * 1000,
                Other2SizeDesc = item.Other2SizeDesc,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
                ArticleDisplaySize = item.ArticleDisplaySize,
                ShellDisplaySize = item.ShellDisplaySize,
            });
        }

    }
}