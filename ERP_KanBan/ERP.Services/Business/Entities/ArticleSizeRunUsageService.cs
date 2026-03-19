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
    public class ArticleSizeRunUsageService : BusinessService
    {
        private Services.Entities.ArticleService Article { get; }
        private Services.Entities.ArticlePartService ArticlePart { get; }
        private Services.Entities.ArticleSizeRunUsageService ArticleSizeRunUsage { get; }
        private Services.Entities.ArticleSizeRunService ArticleSizeRun { get; }
        private Services.Entities.PartService Part { get; }

        private Services.Entities.StyleItemService StyleItem { get;}

        public ArticleSizeRunUsageService(
            Services.Entities.ArticleSizeRunUsageService articleSizeRunUsageService,
            Services.Entities.ArticleService articleService,
            Services.Entities.ArticlePartService articlePartService,
            Services.Entities.ArticleSizeRunService articleSizeRunService,
            Services.Entities.PartService partService,
            Services.Entities.StyleItemService styleItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            ArticlePart = articlePartService;
            ArticleSizeRunUsage = articleSizeRunUsageService;
            ArticleSizeRun = articleSizeRunService;
            Part = partService;
            StyleItem = styleItemService;
        }
        public IQueryable<Models.Views.ArticleSizeRunUsage> Get()
        {
            var result = (
                from asru in ArticleSizeRunUsage.Get()
                join ap in ArticlePart.Get() on new { ArticlePartId = asru.ArticlePartId, LocaleId = asru.LocaleId } equals new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId }
                join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                join asr in ArticleSizeRun.Get() on new { ArticleSizeRunId = asru.ArticleSizeRunId, LocaleId = asru.LocaleId } equals new { ArticleSizeRunId = asr.Id, LocaleId = asr.LocaleId }
                // join si in StyleItem.Get() on new { ArticlePartId = asru.ArticlePartId, LocaleId = asru.LocaleId } equals new { ArticlePartId = si.ArticlePartId, LocaleId = si.LocaleId }
                select new Models.Views.ArticleSizeRunUsage
                {
                    Id = asru.Id,
                    ModifyUserName = asru.ModifyUserName,
                    LastUpdateTime = asru.LastUpdateTime,
                    LocaleId = asru.LocaleId,
                    ArticleSizeRunId = asru.ArticleSizeRunId,
                    ArticlePartId = asru.ArticlePartId,
                    UnitUsage = asru.UnitUsage,
                    ArticleId = ap.ArticleId,
                    PartNameTw = p.PartNameTw,
                    ArticleSize = asr.ArticleSize,
                    ArticleSizeSuffix = asr.ArticleSizeSuffix,
                    ArticleInnerSize = asr.ArticleInnerSize,
                    ArticleDisplaySize = asr.ArticleDisplaySize,
                    // StyleId = si.StyleId,
                    // StyleItemId = si.Id,
                    PartId = p.Id,
                }
            );
            return result;
        }
        public IQueryable<Models.Views.ArticleSizeRunUsage> GetWithStyle()
        {
            var result = (
                from asru in ArticleSizeRunUsage.Get()
                join ap in ArticlePart.Get() on new { ArticlePartId = asru.ArticlePartId, LocaleId = asru.LocaleId } equals new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId }
                join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                join asr in ArticleSizeRun.Get() on new { ArticleSizeRunId = asru.ArticleSizeRunId, LocaleId = asru.LocaleId } equals new { ArticleSizeRunId = asr.Id, LocaleId = asr.LocaleId }
                join si in StyleItem.Get() on new { ArticlePartId = asru.ArticlePartId, LocaleId = asru.LocaleId } equals new { ArticlePartId = si.ArticlePartId, LocaleId = si.LocaleId }
                select new Models.Views.ArticleSizeRunUsage
                {
                    Id = asru.Id,
                    ModifyUserName = asru.ModifyUserName,
                    LastUpdateTime = asru.LastUpdateTime,
                    LocaleId = asru.LocaleId,
                    ArticleSizeRunId = asru.ArticleSizeRunId,
                    ArticlePartId = asru.ArticlePartId,
                    UnitUsage = asru.UnitUsage,
                    ArticleId = ap.ArticleId,
                    PartNameTw = p.PartNameTw,
                    ArticleSize = asr.ArticleSize,
                    ArticleSizeSuffix = asr.ArticleSizeSuffix,
                    ArticleInnerSize = asr.ArticleInnerSize,
                    ArticleDisplaySize = asr.ArticleDisplaySize,
                    StyleId = si.StyleId,
                    StyleItemId = si.Id,
                    PartId = p.Id,
                }
            );
            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.ArticleSizeRunUsage> items)
        {
            ArticleSizeRunUsage.CreateRange(BuildRange(items));
        }
        public void CreateRangeKeepTime(IEnumerable<Models.Views.ArticleSizeRunUsage> items)
        {
            ArticleSizeRunUsage.CreateRangeKeepTime(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.ArticleSizeRunUsage, bool>> predicate)
        {
            ArticleSizeRunUsage.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.ArticleSizeRunUsage> BuildRange(IEnumerable<Models.Views.ArticleSizeRunUsage> items)
        {
            return items.Select(item => new ERP.Models.Entities.ArticleSizeRunUsage
            {
                Id = item.Id,
                ArticleSizeRunId = item.ArticleSizeRunId,
                ArticlePartId = item.ArticlePartId,
                UnitUsage = item.UnitUsage,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = (DateTime)item.LastUpdateTime,
                LocaleId = item.LocaleId
            });
        }

    
    }
}