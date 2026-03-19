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
using KellermanSoftware.CompareNetObjects;

namespace ERP.Services.Business
{
    public class ArticlePartUsageService : BusinessService
    {
        private ERP.Services.Business.Entities.ArticleService Article { get; set; }
        private ERP.Services.Business.Entities.ArticleSizeRunService ArticleSizeRun { get; set; }
        private ERP.Services.Business.Entities.ArticlePartService _ArticlePart { get; set; }
        private ERP.Services.Business.Entities.ArticleSizeRunUsageService _ArticleSizeRunUsage { get; set; }
        private ERP.Services.Entities.StyleService Style { get; set; }
        private ERP.Services.Entities.StyleItemService StyleItem { get; set; }
        private ERP.Services.Entities.ArticlePartService ArticlePart { get; set; }
        private ERP.Services.Entities.ArticleSizeRunUsageService ArticleSizeRunUsage { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.PartService Part { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }

        public ArticlePartUsageService(
            ERP.Services.Business.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Business.Entities.ArticleService articleService,
            ERP.Services.Business.Entities.ArticlePartService _articlePartService,
            ERP.Services.Business.Entities.ArticleSizeRunUsageService _articleSizeRunUsageService,
            ERP.Services.Entities.ArticlePartService articlePartService,
            ERP.Services.Entities.StyleService styleService,
            ERP.Services.Entities.StyleItemService styleItemService,
            ERP.Services.Entities.ArticleSizeRunUsageService articleSizeRunUsageService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.PartService partService,
            ERP.Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            ArticleSizeRun = articleSizeRunService;
            _ArticlePart = _articlePartService;
            _ArticleSizeRunUsage = _articleSizeRunUsageService;

            ArticlePart = articlePartService;
            Style = styleService;
            StyleItem = styleItemService;
            ArticleSizeRunUsage = articleSizeRunUsageService;
            Part = partService;
            Material = materialService;
            CodeItem = codeItemService;
        }

        //批量
        public ERP.Models.Views.ArticlePartUsageGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.ArticlePartUsageGroup();

            var article = Article.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (article != null)
            {
                var styleParts = (
                    from s in Style.Get()
                    join si in StyleItem.Get() on new { StyleId = s.Id, LocaleId = s.LocaleId } equals new { StyleId = si.StyleId, LocaleId = si.LocaleId }
                    where s.ArticleId == id && s.LocaleId == localeId
                    group si by new { si.ArticlePartId, s.ArticleId, s.LocaleId } into g
                    select new
                    {
                        ArticleId = g.Key.ArticleId,
                        ArticlePartId = g.Key.ArticlePartId,
                        LocaleId = g.Key.LocaleId,
                        MaterialId = g.Max(x => x.MaterialId)
                    }
                );

                var articleParts = (
                    from ap in ArticlePart.Get()
                    join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                    join c in CodeItem.Get() on new { UnitId = ap.UnitCodeId, LocaleId = ap.LocaleId } equals new { UnitId = c.Id, LocaleId = c.LocaleId }
                    join s in styleParts on new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId, ArticleId = ap.ArticleId } equals new { ArticlePartId = s.ArticlePartId, LocaleId = s.LocaleId, ArticleId = s.ArticleId } into sGRP
                    from s in sGRP.DefaultIfEmpty()
                    join m in Material.Get() on new { MaterialId = s.MaterialId, LocaleId = s.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                    from m in mGRP.DefaultIfEmpty()
                    where ap.ArticleId == article.Id && ap.LocaleId == article.LocaleId
                    select new Models.Views.ArticlePartUsage
                    {
                        Id = ap.Id,
                        LocaleId = ap.LocaleId,
                        PartId = ap.PartId,
                        PartNo = p.PartNo,
                        PartNameTw = p.PartNameTw,
                        PartNameEn = p.PartNameEn,
                        UnitCodeId = ap.UnitCodeId,
                        UnitName = c.NameTW,
                        MaterialNameTw = m.MaterialName,
                        MaterialNameEng = m.MaterialNameEng,
                        MaterialId = s.MaterialId,
                        ArticleId = ap.ArticleId,
                        Division = ap.Division,
                        KnifeNo = ap.KnifeNo,
                        PieceOfPair = ap.PieceOfPair,
                    }
                )
                .ToList();

                var articleSizerun = ArticleSizeRun.Get().Where(i => i.ArticleId == id && i.LocaleId == localeId).ToList();
                var articleSizerunIds = articleSizerun.Select(i => i.Id).ToList();
                var articleSizerunUsage = ArticleSizeRunUsage.Get().Where(i => articleSizerunIds.Contains(i.ArticleSizeRunId) && i.LocaleId == localeId).ToList();
                articleParts.ForEach(ap =>
                {
                    var articleHead = new List<string>();
                    var articleSizeRun = new List<string>();
                    var articleSizeRunId = new List<string>();

                    var lastUpdateTime = articleSizerun[0].LastUpdateTime;
                    var modifyUserName = articleSizerun[0].ModifyUserName;

                    articleSizerun.ForEach(i =>
                    {
                        // 組合訂單 =========
                        var field = "";
                        if (i.ArticleSizeSuffix != null && (i.ArticleSizeSuffix.Contains("J") || i.ArticleSizeSuffix.Contains("j")))
                        {
                            field = "S" + String.Format("{0:000000}", (i.ArticleInnerSize * 10));
                        }
                        else
                        {
                            field = "S" + String.Format("{0:000000}", (i.ArticleInnerSize * 10));
                        }

                        var item = articleSizerunUsage.Where(u => u.ArticleSizeRunId == i.Id && u.ArticlePartId == ap.Id).OrderByDescending(i => i.LastUpdateTime).FirstOrDefault();

                        var usage = item == null ? 0 : item.UnitUsage;
                        articleSizeRun.Add("\"" + field + "\":" + usage);
                        articleHead.Add("\"" + field + "\":\"" + i.ArticleDisplaySize + "\"");
                        articleSizeRunId.Add("\"" + field + "\":" + i.Id);

                        if (item != null)
                        {
                            lastUpdateTime = item.LastUpdateTime;
                            modifyUserName = item.ModifyUserName;
                        }
                    });

                    ap.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                    ap.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
                    ap.ArticleSizeRunId = "{" + string.Join(",", articleSizeRunId) + "}";
                    ap.LastUpdateTime = lastUpdateTime;
                    ap.ModifyUserName = modifyUserName;

                    ap.HasUsage = articleSizerunUsage.Where(u => u.Id > 0 && u.ArticlePartId == ap.Id).Any() ? 1 : 0;
                });

                group.Article = article;
                group.ArticlePartUsage = articleParts.OrderBy(i => i.PartNo).ToList();
            }
            return group;
        }
        public ERP.Models.Views.ArticlePartUsageGroup Save(ArticlePartUsageGroup group)
        {
            var article = group.Article;
            var articlePartUsages = group.ArticlePartUsage.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (article != null)
                {
                    var _article = Article.Get().Where(i => i.LocaleId == article.LocaleId && i.Id == article.Id).FirstOrDefault();
                    articlePartUsages.ForEach(i =>
                    {
                        var sizeusages = new List<ERP.Models.Entities.ArticleSizeRunUsage>();

                        var usageJson = i.ArticleSizeRun;
                        var idJson = i.ArticleSizeRunId;

                        // 反序列化兩個 JSON 字串
                        var usageDict = JsonSerializer.Deserialize<Dictionary<string, decimal>>(usageJson);
                        var idDict = JsonSerializer.Deserialize<Dictionary<string, decimal>>(idJson);

                        var dataTime = DateTime.Now;
                        foreach (var key in usageDict.Keys)
                        {
                            if (key.StartsWith("S") && key.Length == 7 && idDict.ContainsKey(key))
                            {
                                sizeusages.Add(new ERP.Models.Entities.ArticleSizeRunUsage
                                {
                                    ArticlePartId = i.Id,
                                    ArticleSizeRunId = idDict[key],  // 從 ArticleSizeRunId 拿 ID
                                    UnitUsage = usageDict[key],      // 從 ArticleSizeRun 拿用量
                                    ModifyUserName = i.ModifyUserName,
                                    LocaleId = i.LocaleId,
                                    LastUpdateTime = dataTime,
                                });
                            }
                        }
                        ArticleSizeRunUsage.RemoveRange(a => a.LocaleId == article.LocaleId && a.ArticlePartId == i.Id);
                        ArticleSizeRunUsage.CreateRange(sizeusages);
                    });
                }
                UnitOfWork.Commit();
                return Get((int)article.Id, (int)article.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

        }
        //單部位
        public List<ERP.Models.Views.ArticleSizeRunUsage> GetArticleSizeRunUsage(int articlePartId, int localeId)
        {
            return _ArticleSizeRunUsage.Get().Where(i => i.ArticlePartId == articlePartId && i.LocaleId == localeId).ToList();
        }

        public ERP.Models.Views.ArticlePartGroup GetArticlePartGroup(int id, int localeId)
        {
            var group = new ERP.Models.Views.ArticlePartGroup();

            var article = Article.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (article != null)
            {
                var styleParts = (
                    from s in Style.Get()
                    join si in StyleItem.Get() on new { StyleId = s.Id, LocaleId = s.LocaleId } equals new { StyleId = si.StyleId, LocaleId = si.LocaleId }
                    where s.ArticleId == id && s.LocaleId == localeId
                    group si by new { si.ArticlePartId, s.ArticleId, s.LocaleId } into g
                    select new
                    {
                        ArticleId = g.Key.ArticleId,
                        ArticlePartId = g.Key.ArticlePartId,
                        LocaleId = g.Key.LocaleId,
                        MaterialId = g.Max(x => x.MaterialId),
                    }
                );
                var articleParts = (
                    from ap in ArticlePart.Get()
                    join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                    join c in CodeItem.Get() on new { UnitId = ap.UnitCodeId, LocaleId = ap.LocaleId } equals new { UnitId = c.Id, LocaleId = c.LocaleId }
                    join s in styleParts on new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId, ArticleId = ap.ArticleId } equals new { ArticlePartId = s.ArticlePartId, LocaleId = s.LocaleId, ArticleId = s.ArticleId } into sGRP
                    from s in sGRP.DefaultIfEmpty()
                    join m in Material.Get() on new { MaterialId = s.MaterialId, LocaleId = s.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                    from m in mGRP.DefaultIfEmpty()
                    where ap.ArticleId == article.Id && ap.LocaleId == article.LocaleId
                    select new Models.Views.ArticlePart
                    {
                        Id = ap.Id,
                        LocaleId = ap.LocaleId,
                        PartId = ap.PartId,
                        PartNo = p.PartNo,
                        PartNameTw = p.PartNameTw,
                        PartNameEn = p.PartNameEn,
                        UnitCodeId = ap.UnitCodeId,
                        UnitName = c.NameTW,
                        MaterialNameTw = m.MaterialName,
                        MaterialNameEng = m.MaterialNameEng,
                        MaterialId = s.MaterialId,
                        ArticleId = ap.ArticleId,
                        Division = ap.Division,
                        PieceOfPair = ap.PieceOfPair,
                        KnifeNo = ap.KnifeNo,
                        AlternateType = ap.AlternateType,
                    }
                )
                .ToList();

                var articleSizerun = ArticleSizeRun.Get().Where(i => i.ArticleId == id && i.LocaleId == localeId).ToList();
                var articleSizerunIds = articleSizerun.Select(i => i.Id).ToList();
                var articleSizerunUsage = ArticleSizeRunUsage.Get().Where(i => articleSizerunIds.Contains(i.ArticleSizeRunId) && i.LocaleId == localeId).ToList();

                var allPartsUsage = (
                    from ap in articleParts
                    join asr in articleSizerun on new { ArticleId = ap.ArticleId, LocaleId = ap.LocaleId } equals new { ArticleId = asr.ArticleId, LocaleId = asr.LocaleId }
                    join asru in articleSizerunUsage on new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId, ArticleSizeRunId = asr.Id } equals new { ArticlePartId = asru.ArticlePartId, LocaleId = asru.LocaleId, ArticleSizeRunId = asru.ArticleSizeRunId } into aGrp
                    from asru in aGrp.DefaultIfEmpty()
                    select new ArticleSizeRunUsage
                    {
                        Id = asru == null ? 0 : asru.Id,
                        ModifyUserName = asru == null ? "" : asru.ModifyUserName,
                        LastUpdateTime = asru == null ? (DateTime?)null : asru.LastUpdateTime,
                        UnitUsage = asru == null ? 0 : asru.UnitUsage,

                        LocaleId = ap.LocaleId,
                        ArticleSizeRunId = asr.Id,
                        ArticlePartId = ap.Id,
                        ArticleId = ap.ArticleId,
                        ArticleSize = asr.ArticleSize,
                        ArticleSizeSuffix = asr.ArticleSizeSuffix,
                        ArticleInnerSize = asr.ArticleInnerSize,
                        ArticleDisplaySize = asr.ArticleDisplaySize,
                    }
                )
                .ToList();

                articleParts.ForEach(i =>
                {
                    i.HasUsage = allPartsUsage.Where(u => u.Id > 0 && u.ArticlePartId == i.Id).Any() ? 1 : 0;
                });
                group.Article = article;
                group.ArticlePart = articleParts.OrderBy(i => i.PartNo).ToList();
                group.ArticleSizeRunUsage = allPartsUsage;
            }
            return group;
        }
        public List<ERP.Models.Views.ArticleSizeRunUsage> SaveArticlePartGroup(ERP.Models.Views.ArticlePartGroup group)
        {
            var article = group.Article;
            var articlePart = group.ArticlePart.ToList();
            var items = group.ArticleSizeRunUsage.ToList();
            try
            {
                if (items.Any())
                {
                    var articleId = article.Id;
                    var articlePartId = articlePart[0].Id;
                    var localeId = article.LocaleId;
                    var compareLogic = GetDiffLogic();
                    var updateTime = DateTime.Now;
                    var valueChange = false;

                    UnitOfWork.BeginTransaction();

                    var oldItems = _ArticleSizeRunUsage.Get().Where(i => i.ArticlePartId == articlePartId && i.LocaleId == localeId).ToList();

                    if (oldItems.Any())
                    {
                        items.ForEach(i =>
                        {
                            var oldItem = oldItems.Where(o => o.Id == i.Id).FirstOrDefault();

                            if (oldItem != null)    // 已存在資料，只有資料異動才更新時間
                            {
                                ComparisonResult result = compareLogic.Compare(oldItem, i);
                                if (!result.AreEqual)
                                {
                                    valueChange = true;
                                    i.ModifyUserName = article.ModifyUserName;
                                    i.LastUpdateTime = updateTime;
                                    // Console.WriteLine("========================");
                                    // Console.WriteLine(oldItem.ArticleDisplaySize);
                                    // Console.WriteLine(result.DifferencesString);
                                    // Console.WriteLine("========================");
                                }
                                else    // 已存在資料，資料沒異動不更新時間
                                {
                                    i = oldItem;
                                }
                            }
                            else
                            {
                                //有新的資料
                                valueChange = true;
                                i.ModifyUserName = article.ModifyUserName;
                                i.LastUpdateTime = updateTime;
                            }
                        });
                    }
                    else
                    {
                        valueChange = true;
                        items.ForEach(i =>
                        {
                            i.ModifyUserName = article.ModifyUserName;
                            i.LastUpdateTime = updateTime;
                        });
                    }

                    if (valueChange)    //資料沒改變就不儲存
                    {
                        _ArticleSizeRunUsage.RemoveRange(i => i.LocaleId == localeId && i.ArticlePartId == articlePartId);
                        _ArticleSizeRunUsage.CreateRangeKeepTime(items);
                    }

                    UnitOfWork.Commit();

                    items = _ArticleSizeRunUsage.Get().Where(i => i.ArticlePartId == articlePartId && i.LocaleId == localeId).OrderBy(i => i.ArticleInnerSize).ToList();
                }

                return items;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public List<ERP.Models.Views.ArticleSizeRunUsage> SaveArticleSizeRunUsage(List<ERP.Models.Views.ArticleSizeRunUsage> items)
        {
            try
            {
                if (items.Any())
                {
                    var articleId = items[0].ArticleId;
                    var articlePartId = items[0].ArticlePartId;
                    var localeId = items[0].LocaleId;

                    UnitOfWork.BeginTransaction();
                    _ArticleSizeRunUsage.RemoveRange(i => i.LocaleId == localeId && i.ArticlePartId == articlePartId);
                    _ArticleSizeRunUsage.CreateRange(items);
                    UnitOfWork.Commit();

                    items = _ArticleSizeRunUsage.Get().Where(i => i.ArticlePartId == articlePartId && i.LocaleId == localeId).ToList();
                }

                return items;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public List<ERP.Models.Views.ArticleSizeRunUsage> RemoveArticleSizeRunUsage(int articleId, int articlePartId, int localeId)
        {
            var items = new List<ERP.Models.Views.ArticleSizeRunUsage>();

            try
            {
                UnitOfWork.BeginTransaction();
                _ArticleSizeRunUsage.RemoveRange(i => i.LocaleId == localeId && i.ArticlePartId == articlePartId);

                items = ArticleSizeRun.Get()
                    .Where(i => i.ArticleId == articleId && i.LocaleId == localeId)
                    .Select(i => new ERP.Models.Views.ArticleSizeRunUsage
                    {
                        Id = 0,
                        ModifyUserName = "",
                        UnitUsage = 0,

                        LocaleId = localeId,
                        ArticleSizeRunId = i.Id,
                        ArticlePartId = articlePartId,
                        ArticleId = articleId,
                        ArticleSize = i.ArticleSize,
                        ArticleSizeSuffix = i.ArticleSizeSuffix,
                        ArticleInnerSize = i.ArticleInnerSize,
                        ArticleDisplaySize = i.ArticleDisplaySize,
                    })
                    .ToList();

                UnitOfWork.Commit();

                return items;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        private CompareLogic GetDiffLogic()
        {
            // constructor
            var compareLogic = new CompareLogic(
                new ComparisonConfig
                {
                    CompareChildren = true,
                    IgnoreCollectionOrder = true,
                    MaxDifferences = int.MaxValue,
                    ShowBreadcrumb = false,
                });

            // set compare key
            var spec = new Dictionary<Type, IEnumerable<string>>();
            spec.Add(typeof(ArticleSizeRunUsage), new string[] { "Id" });
            compareLogic.Config.CollectionMatchingSpec = spec;

            // ignore property
            compareLogic.Config.MembersToInclude.Add("UnitUsage");

            return compareLogic;
        }
    }
}
