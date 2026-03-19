using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.Json;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;

namespace ERP.Services.Business
{
    public class StylePartUsageService : BusinessService
    {

        private ERP.Services.Business.Entities.StyleService Style { get; set; }
        private ERP.Services.Business.Entities.StyleSizeRunUsageService StyleSizeRunUsage { get; set; }
        private ERP.Services.Business.Entities.ArticleSizeRunService ArticleSizeRun { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }

        public StylePartUsageService(
            ERP.Services.Business.Entities.StyleSizeRunUsageService styleSizeRunUsageService,
            ERP.Services.Business.Entities.StyleService styleService,
            ERP.Services.Business.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            StyleSizeRunUsage = styleSizeRunUsageService;
            ArticleSizeRun = articleSizeRunService;
            Style = styleService;
            Material = materialService;
            CodeItem = codeItemService;
        }
        public ERP.Models.Views.StylePartGroup GetStylePartGroup(int id, int localeId)
        {
            var group = new ERP.Models.Views.StylePartGroup();

            var style = Style.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (style != null)
            {
                var articleSizeRun = ArticleSizeRun.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();
                var styleSizeRunUsages = StyleSizeRunUsage.Get().Where(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId).ToList();   // 這裡是已同一個article為主體，把沒有ArticleRun的作轉換
                var styleParts = styleSizeRunUsages
                    .GroupBy(g => new
                    {
                        g.StyleId,
                        g.LocaleId,
                        g.MaterialId,
                        g.MaterialName,
                        g.UnitCodeId,
                        g.UnitCode
                    })
                    .Select(i => new StylePart
                    {
                        StyleId = i.Key.StyleId,
                        LocaleId = i.Key.LocaleId,
                        MaterialId = i.Key.MaterialId,
                        MaterialName = i.Key.MaterialName,
                        UnitCodeId = i.Key.UnitCodeId,
                        UnitCode = i.Key.UnitCode,
                    })
                    .ToList();

                // 處理ArticleSizeRun已經刪除的資料。
                var items = styleSizeRunUsages.Where(i => i.ArticleInnerSize > 0).ToList();
                if (!items.Any())
                {
                    var usages = new List<ERP.Models.Views.StyleSizeRunUsage>();
                    styleParts.ForEach(i =>
                    {
                        articleSizeRun.ForEach(a =>
                        {
                            usages.Add(new ERP.Models.Views.StyleSizeRunUsage
                            {
                                Id = 0,
                                StyleId = style.Id,
                                ArticleSizeRunId = a.Id,
                                MaterialId = i.MaterialId,
                                OrdersId = 0,
                                LocaleId = style.LocaleId,
                                UnitUsage = 0,
                                UnitCodeId = i.UnitCodeId,

                                MaterialName = i.MaterialName,
                                UnitCode = i.UnitCode,
                                ArticleSize = a.ArticleSize,
                                ArticleSizeSuffix = a.ArticleSizeSuffix,
                                ArticleInnerSize = a.ArticleInnerSize,
                                ArticleDisplaySize = a.ArticleDisplaySize,
                            });
                        });
                    });
                    styleSizeRunUsages = usages;
                }

                group.Style = style;
                group.StylePart = styleParts.OrderBy(i => i.MaterialName).ToList();
                group.ArticleSizeRun = articleSizeRun;
                group.StyleSizeRunUsage = styleSizeRunUsages.OrderBy(i => i.ArticleInnerSize).ToList();
            }
            return group;
        }
        public ERP.Models.Views.StylePartGroup GetStylePartGroupByNo(string styleNo, int localeId)
        {
            var group = new ERP.Models.Views.StylePartGroup();

            var style = Style.Get().Where(i => i.StyleNo == styleNo && i.LocaleId == localeId).FirstOrDefault();
            if (style != null)
            {
                var articleSizeRun = ArticleSizeRun.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();
                var styleSizeRunUsages = StyleSizeRunUsage.Get().Where(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId).ToList();   // 這裡是已同一個article為主體，把沒有ArticleRun的作轉換
                var styleParts = styleSizeRunUsages
                    .GroupBy(g => new
                    {
                        g.StyleId,
                        g.LocaleId,
                        g.MaterialId,
                        g.MaterialName,
                        g.UnitCodeId,
                        g.UnitCode
                    })
                    .Select(i => new StylePart
                    {
                        StyleId = i.Key.StyleId,
                        LocaleId = i.Key.LocaleId,
                        MaterialId = i.Key.MaterialId,
                        MaterialName = i.Key.MaterialName,
                        UnitCodeId = i.Key.UnitCodeId,
                        UnitCode = i.Key.UnitCode,
                    })
                    .ToList();

                // 處理ArticleSizeRun已經刪除的資料。
                var items = styleSizeRunUsages.Where(i => i.ArticleInnerSize > 0).ToList();
                if (!items.Any())
                {
                    var usages = new List<ERP.Models.Views.StyleSizeRunUsage>();
                    styleParts.ForEach(i =>
                    {
                        articleSizeRun.ForEach(a =>
                        {
                            usages.Add(new ERP.Models.Views.StyleSizeRunUsage
                            {
                                Id = 0,
                                StyleId = style.Id,
                                ArticleSizeRunId = a.Id,
                                MaterialId = i.MaterialId,
                                OrdersId = 0,
                                LocaleId = style.LocaleId,
                                UnitUsage = 0,
                                UnitCodeId = i.UnitCodeId,

                                MaterialName = i.MaterialName,
                                UnitCode = i.UnitCode,
                                ArticleSize = a.ArticleSize,
                                ArticleSizeSuffix = a.ArticleSizeSuffix,
                                ArticleInnerSize = a.ArticleInnerSize,
                                ArticleDisplaySize = a.ArticleDisplaySize,
                            });
                        });
                    });
                    styleSizeRunUsages = usages;
                }

                group.Style = style;
                group.StylePart = styleParts.OrderBy(i => i.MaterialName).ToList();
                group.ArticleSizeRun = articleSizeRun;
                group.StyleSizeRunUsage = styleSizeRunUsages.OrderBy(i => i.ArticleInnerSize).ToList();
            }
            return group;
        }
        public ERP.Models.Views.StylePartGroup CopyStylePartGroup(ERP.Models.Views.CopyStylePartGroup group)
        {
            var _group = new ERP.Models.Views.StylePartGroup();

            var style = group.Style;
            var articleSizeRun = group.ArticleSizeRun;
            var refStyleGroup = GetStylePartGroupByNo(group.RefStyleNo, (int)group.RefLocaleId);

            if (refStyleGroup.Style != null)
            {
                var refStyle = refStyleGroup.Style;
                var refStylePart = refStyleGroup.StylePart.ToList();
                var refStyleSizeRunUsage = refStyleGroup.StyleSizeRunUsage.ToList();

                if (refStyle.LocaleId == style.LocaleId)
                {
                    var newStyleItemUsage = (
                        from su in refStyleSizeRunUsage
                        join a in articleSizeRun on new { ArticleInnerSize = su.ArticleInnerSize } equals new { ArticleInnerSize = (double?)a.ArticleInnerSize }
                        select new ERP.Models.Views.StyleSizeRunUsage
                        {
                            Id = 0,
                            StyleId = style.Id,
                            MaterialId = su.MaterialId,
                            OrdersId = 0,
                            LocaleId = style.LocaleId,
                            UnitUsage = su.UnitUsage,
                            UnitCodeId = su.UnitCodeId,

                            MaterialName = su.MaterialName,
                            UnitCode = su.UnitCode,

                            ArticleSizeRunId = a.Id,
                            ArticleSize = a.ArticleSize,
                            ArticleSizeSuffix = a.ArticleSizeSuffix,
                            ArticleInnerSize = a.ArticleInnerSize,
                            ArticleDisplaySize = a.ArticleDisplaySize,
                        }
                    );

                    var styleParts = newStyleItemUsage
                        .GroupBy(g => new
                        {
                            g.StyleId,
                            g.LocaleId,
                            g.MaterialId,
                            g.MaterialName,
                            g.UnitCodeId,
                            g.UnitCode
                        })
                        .Select(i => new StylePart
                        {
                            StyleId = i.Key.StyleId,
                            LocaleId = i.Key.LocaleId,
                            MaterialId = i.Key.MaterialId,
                            MaterialName = i.Key.MaterialName,
                            UnitCodeId = i.Key.UnitCodeId,
                            UnitCode = i.Key.UnitCode,
                        })
                        .ToList();


                    _group.Style = style;
                    _group.ArticleSizeRun = articleSizeRun;
                    _group.StylePart = styleParts.OrderBy(i => i.MaterialName).ToList();
                    _group.StyleSizeRunUsage = newStyleItemUsage;
                }
                else
                {

                    var mNames = refStyleSizeRunUsage.Select(i => i.MaterialName).Distinct().ToList();
                    var uNames = refStyleSizeRunUsage.Select(i => i.UnitCode).Distinct().ToList();

                    var materials = Material.Get().Where(i => i.LocaleId == style.LocaleId && mNames.Contains(i.MaterialName)).Select(i => new { i.Id, i.MaterialName, i.LocaleId }).ToList();
                    var units = CodeItem.Get().Where(i => i.LocaleId == style.LocaleId && i.CodeType == "21" && uNames.Contains(i.NameTW)).Select(i => new { i.Id, i.NameTW, i.LocaleId }).ToList();

                    var newStyleItemUsage = (
                        from su in refStyleSizeRunUsage
                        join a in articleSizeRun on new { ArticleInnerSize = su.ArticleInnerSize } equals new { ArticleInnerSize = (double?)a.ArticleInnerSize }
                        select new ERP.Models.Views.StyleSizeRunUsage
                        {
                            Id = 0,
                            OrdersId = 0,
                            StyleId = style.Id,
                            LocaleId = style.LocaleId,
                            UnitUsage = su.UnitUsage,

                            MaterialId = su.MaterialId,
                            MaterialName = su.MaterialName,
                            UnitCode = su.UnitCode,
                            UnitCodeId = su.UnitCodeId,

                            ArticleSizeRunId = a.Id,
                            ArticleSize = a.ArticleSize,
                            ArticleSizeSuffix = a.ArticleSizeSuffix,
                            ArticleInnerSize = a.ArticleInnerSize,
                            ArticleDisplaySize = a.ArticleDisplaySize,
                        }
                    )
                    .ToList();

                    newStyleItemUsage.ForEach(i =>
                    {
                        var material = materials.Where(m => m.MaterialName == i.MaterialName && m.LocaleId == i.LocaleId).FirstOrDefault();
                        var unit = units.Where(m => m.NameTW == i.UnitCode && m.LocaleId == i.LocaleId).FirstOrDefault();

                        if (material != null)
                        {
                            i.MaterialId = material.Id;
                            i.MaterialName = material.MaterialName;
                        }
                        else
                        {
                            i.MaterialId = 0;
                            i.MaterialName = i.MaterialName;
                        }

                        if (unit != null)
                        {
                            i.UnitCodeId = unit.Id;
                            i.UnitCode = unit.NameTW;
                        }
                        else
                        {
                            i.UnitCodeId = 0;
                            i.UnitCode = i.UnitCode;
                        }
                    });

                    var styleParts = newStyleItemUsage
                        .GroupBy(g => new
                        {
                            g.StyleId,
                            g.LocaleId,
                            g.MaterialId,
                            g.MaterialName,
                            g.UnitCodeId,
                            g.UnitCode
                        })
                        .Select(i => new StylePart
                        {
                            StyleId = i.Key.StyleId,
                            LocaleId = i.Key.LocaleId,
                            MaterialId = i.Key.MaterialId,
                            MaterialName = i.Key.MaterialName,
                            UnitCodeId = i.Key.UnitCodeId,
                            UnitCode = i.Key.UnitCode,
                        })
                        .ToList();

                    _group.Style = style;
                    _group.ArticleSizeRun = articleSizeRun;
                    _group.StylePart = styleParts.OrderBy(i => i.MaterialName).ToList();
                    _group.StyleSizeRunUsage = newStyleItemUsage;
                }

            }

            return _group;
        }
        
        public List<ERP.Models.Views.StyleSizeRunUsage> GetStyleSizeRunUsage(int styleId, int materialId, int localeId)
        {
            return StyleSizeRunUsage.Get().Where(i => i.StyleId == styleId && i.LocaleId == localeId && i.MaterialId == materialId).ToList();
        }
        public IQueryable<ERP.Models.Views.StylePart> GetStylePart(string predicate)
        {
            var styleParts = StyleSizeRunUsage.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(g => new
                {
                    g.StyleId,
                    g.LocaleId,
                    g.MaterialId,
                    g.MaterialName,
                    g.UnitCodeId,
                    g.UnitCode,
                    g.StyleNo,
                })
                .Select(i => new StylePart
                {
                    StyleId = i.Key.StyleId,
                    LocaleId = i.Key.LocaleId,
                    MaterialId = i.Key.MaterialId,
                    MaterialName = i.Key.MaterialName,
                    UnitCodeId = i.Key.UnitCodeId,
                    UnitCode = i.Key.UnitCode,
                    StyleNo = i.Key.StyleNo,
                })
                .OrderBy(i => i.MaterialName).ToList()
                .Take(1000)
                .ToList();

            return styleParts.AsQueryable();

        }
        public List<ERP.Models.Views.StyleSizeRunUsage> SaveStyleSizeRunUsage(List<ERP.Models.Views.StyleSizeRunUsage> items)
        {
            try
            {
                if (items.Any())
                {
                    var styleId = items[0].StyleId;
                    var materialId = items[0].MaterialId;
                    var localeId = items[0].LocaleId;

                    UnitOfWork.BeginTransaction();
                    StyleSizeRunUsage.RemoveRange(i => i.StyleId == styleId && i.LocaleId == localeId && i.MaterialId == materialId);
                    StyleSizeRunUsage.CreateRange(items);
                    UnitOfWork.Commit();

                    items = StyleSizeRunUsage.Get().Where(i => i.StyleId == styleId && i.LocaleId == localeId && i.MaterialId == materialId).ToList();
                }

                return items;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        // remove material of style
        public ERP.Models.Views.StylePartGroup RemoveStyleSizeRunUsage(int styleId, int materialId, int localeId)
        {
            var items = new List<ERP.Models.Views.StyleSizeRunUsage>();

            try
            {
                UnitOfWork.BeginTransaction();
                StyleSizeRunUsage.RemoveRange(i => i.StyleId == styleId && i.LocaleId == localeId && i.MaterialId == materialId);
                UnitOfWork.Commit();
                return GetStylePartGroup(styleId, localeId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.StylePartGroup SaveStylePartGroup(ERP.Models.Views.StylePartGroup group)
        {
            var style = group.Style;
            var styleParts = group.StylePart.ToList();
            var styleUsages = group.StyleSizeRunUsage.ToList();
            var sizeRuns = group.ArticleSizeRun.ToList();
            try
            {
                if (styleParts.Any())
                {
                    var materialId = styleParts.Select(i => i.MaterialId);
                    var usages = new List<ERP.Models.Views.StyleSizeRunUsage>();

                    styleParts.ForEach(i =>
                    {
                        var items = styleUsages.Where(u => u.StyleId == style.Id && u.LocaleId == style.LocaleId && u.MaterialId == i.MaterialId).ToList();
                        sizeRuns.ForEach(s =>
                        {
                            var item = items.Where(u => u.ArticleInnerSize == s.ArticleInnerSize).FirstOrDefault();

                            if (item == null)
                            {
                                item = new ERP.Models.Views.StyleSizeRunUsage
                                {
                                    Id = 0,
                                    StyleId = style.Id,
                                    ArticleSizeRunId = s.Id,
                                    MaterialId = i.MaterialId,
                                    OrdersId = 0,
                                    ModifyUserName = style.ModifyUserName,
                                    LocaleId = style.LocaleId,
                                    UnitUsage = 0,
                                    UnitCodeId = i.UnitCodeId,
                                };
                            }
                            else
                            {
                                item.UnitCodeId = i.UnitCodeId;
                            }

                            usages.Add(item);
                        });
                    });

                    UnitOfWork.BeginTransaction();
                    StyleSizeRunUsage.RemoveRange(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId);
                    StyleSizeRunUsage.CreateRange(usages);
                    UnitOfWork.Commit();
                }

                return GetStylePartGroup((int)style.Id, (int)style.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        // remove all material of style
        public ERP.Models.Views.StylePartGroup RemoveStylePartGroup(int styleId, int localeId)
        {
            try
            {
                UnitOfWork.BeginTransaction();
                StyleSizeRunUsage.RemoveRange(i => i.StyleId == styleId && i.LocaleId == localeId);
                UnitOfWork.Commit();
                return GetStylePartGroup(styleId, localeId);

            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

    }
}
