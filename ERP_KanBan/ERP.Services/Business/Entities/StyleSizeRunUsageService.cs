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
    public class StyleSizeRunUsageService : BusinessService
    {
        private Services.Entities.StyleService Style { get; }
        private Services.Entities.StyleSizeRunUsageService StyleSizeRunUsage { get; }
        private Services.Entities.ArticleSizeRunService ArticleSizeRun { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.MaterialService Material { get; }
        public StyleSizeRunUsageService(
            Services.Entities.StyleService styleService,
            Services.Entities.StyleSizeRunUsageService sizeRunUsageService,
            Services.Entities.ArticleSizeRunService articleSizeRunService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.MaterialService materialService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Style = styleService;
            StyleSizeRunUsage = sizeRunUsageService;
            ArticleSizeRun = articleSizeRunService;
            CodeItem = codeItemService;
            Material = materialService;
        }
        public IQueryable<Models.Views.StyleSizeRunUsage> Get()
        {
            var style = (
                from ssr in StyleSizeRunUsage.Get()
                join s in Style.Get() on new { StyleId = ssr.StyleId, LocaleId = ssr.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId }
                // join asr in ArticleSizeRun.Get() on new { ArticleSizeRunId = ssr.ArticleSizeRunId, LocaleId = ssr.LocaleId } equals new { ArticleSizeRunId = asr.Id, LocaleId = asr.LocaleId } 
                join asr in ArticleSizeRun.Get() on new { ArticleSizeRunId = ssr.ArticleSizeRunId, LocaleId = ssr.LocaleId } equals new { ArticleSizeRunId = asr.Id, LocaleId = asr.LocaleId } into asrGRP
                from asr in asrGRP.DefaultIfEmpty()
                join c in CodeItem.Get() on new { UnitCodeId = ssr.UnitCodeId, LocaleId = ssr.LocaleId } equals new { UnitCodeId = c.Id, LocaleId = c.LocaleId } into cGRP
                from c in cGRP.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = ssr.MaterialId, LocaleId = ssr.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                select new Models.Views.StyleSizeRunUsage
                {
                    Id = ssr.Id,
                    StyleId = ssr.StyleId,
                    ArticleSizeRunId = ssr.ArticleSizeRunId,
                    MaterialId = ssr.MaterialId,
                    OrdersId = ssr.OrdersId,
                    ModifyUserName = ssr.ModifyUserName,
                    LastUpdateTime = ssr.LastUpdateTime,
                    LocaleId = ssr.LocaleId,
                    UnitUsage = ssr.UnitUsage,
                    UnitCodeId = ssr.UnitCodeId,
                    StyleNo = s.StyleNo,

                    MaterialName = (string?)m.MaterialName ?? "",
                    UnitCode = (string?)c.NameTW ?? "",
                    ArticleSize = (decimal?)asr.ArticleSize,
                    ArticleSizeSuffix = (string?)asr.ArticleSizeSuffix ?? "",
                    ArticleInnerSize = (double?)asr.ArticleInnerSize,
                    ArticleDisplaySize = (string?)asr.ArticleDisplaySize ?? "",
                }
            );
            return style;
        }

        public void CreateRange(IEnumerable<Models.Views.StyleSizeRunUsage> items)
        {
            StyleSizeRunUsage.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.StyleSizeRunUsage, bool>> predicate)
        {
            StyleSizeRunUsage.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.StyleSizeRunUsage> BuildRange(IEnumerable<Models.Views.StyleSizeRunUsage> items)
        {
            return items.Select(item => new ERP.Models.Entities.StyleSizeRunUsage
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                StyleId = item.StyleId,
                ArticleSizeRunId = item.ArticleSizeRunId,
                MaterialId = item.MaterialId,
                OrdersId = item.OrdersId,
                ModifyUserName = item.ModifyUserName,

                UnitUsage = item.UnitUsage,
                UnitCodeId = item.UnitCodeId,
            });
        }

    }
}