using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;
using NPOI.HSSF.UserModel; // .xls 用 HSSFWorkbook
using NPOI.SS.UserModel;
using System.IO;
using ERP.Models.Views.Common;
using Newtonsoft.Json;

namespace ERP.Services.Search
{
    public class StyleService : SearchService
    {
        private ERP.Services.Entities.StyleService Style { get; set; }
        private ERP.Services.Entities.StyleItemService StyleItem { get; set; }
        private ERP.Services.Entities.ArticleService Article { get; set; }
        private ERP.Services.Entities.ArticleSizeRunService ArticleSizeRun { get; set; }
        private ERP.Services.Entities.ArticlePartService ArticlePart { get; set; }
        private ERP.Services.Entities.ArticleSizeRunUsageService ArticleSizeRunUsage { get; set; }
        private ERP.Services.Entities.StyleSizeRunUsageService StyleSizeRunUsage { get; set; }
        private ERP.Services.Entities.PartService Part { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.KnifeService Knife { get; set; }
        private ERP.Services.Entities.OutsoleService Outsole { get; set; }
        private ERP.Services.Entities.LastService Last { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }

        private ERP.Services.Entities.BOMLogService StyleLog { get; set; }
        public StyleService(
            ERP.Services.Entities.StyleService styleService,
            ERP.Services.Entities.StyleItemService styleItemService,
            ERP.Services.Entities.StyleSizeRunUsageService styleSizeRunUsageService,
            ERP.Services.Entities.ArticleService articleService,
            ERP.Services.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Entities.ArticlePartService articlePartService,
            ERP.Services.Entities.ArticleSizeRunUsageService articleSizeRunUsageService,
            ERP.Services.Entities.PartService partService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.KnifeService knifeService,
            ERP.Services.Entities.OutsoleService outsoleService,
            ERP.Services.Entities.LastService lastService,

            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.MRPItemService mrpItemService,
            ERP.Services.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.BOMLogService styleLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Style = styleService;
            StyleItem = styleItemService;
            StyleSizeRunUsage = styleSizeRunUsageService;
            Article = articleService;
            ArticleSizeRun = articleSizeRunService;
            ArticlePart = articlePartService;
            ArticleSizeRunUsage = articleSizeRunUsageService;
            Part = partService;
            Material = materialService;
            Knife = knifeService;
            Outsole = outsoleService;
            Last = lastService;
            CodeItem = codeItemService;

            Orders = ordersService;
            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;

            StyleLog = styleLogService;
        }
        public IQueryable<Models.Views.Style> GetStyle(string predicate)
        {
            var styles = (
                from s in Style.Get()
                join a in Article.Get() on new { ArticleId = s.ArticleId, LocaleId = s.LocaleId } equals new { ArticleId = a.Id, LocaleId = a.LocaleId }
                join k in Knife.Get() on new { KnifeId = s.KnifeId, LocaleId = s.LocaleId } equals new { KnifeId = (decimal?)k.Id, LocaleId = k.LocaleId } into kGRP
                from k in kGRP.DefaultIfEmpty()
                join o in Outsole.Get() on new { OutsoleId = s.OutsoleId, LocaleId = s.LocaleId } equals new { OutsoleId = (decimal?)o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join l in Last.Get() on new { LastId = s.LastId, LocaleId = s.LocaleId } equals new { LastId = (decimal?)l.Id, LocaleId = l.LocaleId } into lGRP
                from l in lGRP.DefaultIfEmpty()
                join c1 in CodeItem.Get() on new { BrandId = a.BrandCodeId, LocaleId = a.LocaleId } equals new { BrandId = c1.Id, LocaleId = c1.LocaleId } into c1GRP
                from c1 in c1GRP.DefaultIfEmpty()
                join c2 in CodeItem.Get() on new { CategoryCodeId = s.CategoryCodeId, LocaleId = s.LocaleId } equals new { CategoryCodeId = (decimal?)c2.Id, LocaleId = c2.LocaleId } into c2GRP
                from c2 in c2GRP.DefaultIfEmpty()
                join c3 in CodeItem.Get() on new { ShoeClassCodeId = s.ShoeClassCodeId, LocaleId = s.LocaleId } equals new { ShoeClassCodeId = (decimal?)c3.Id, LocaleId = c3.LocaleId } into c3GRP
                from c3 in c3GRP.DefaultIfEmpty()
                select new Models.Views.Style
                {
                    Id = s.Id,
                    ArticleId = s.ArticleId,
                    ColorCode = s.ColorCode,
                    StyleNo = s.StyleNo,
                    ColorDesc = s.ColorDesc,
                    InsockLabel = s.InsockLabel,
                    OutsoleColorDescTW = s.OutsoleColorDescTW,
                    OutsoleColorDescEN = s.OutsoleColorDescEN,
                    ModifyUserName = s.ModifyUserName,
                    LastUpdateTime = s.LastUpdateTime,
                    OutsoleId = s.OutsoleId,
                    ShellId = s.ShellId,
                    LocaleId = s.LocaleId,
                    CustomerId = s.CustomerId,
                    SizeCountryCodeId = s.SizeCountryCodeId,
                    ShoeClassCodeId = s.ShoeClassCodeId,
                    CategoryCodeId = s.CategoryCodeId,
                    doMRP = s.doMRP,
                    KnifeId = s.KnifeId,
                    LastId = s.LastId,
                    IsSpecial = s.IsSpecial,

                    ArticleNo = a.ArticleNo,
                    ArticleName = a.ArticleName,
                    BrandCodeId = a.BrandCodeId,
                    Knife = k.KnifeNo,
                    Brand = c1.NameTW,
                    Last = l.LastNo,
                    Outsole = o.OutsoleNo,
                    ShoeClass = c3.NameTW,
                    Category = c2.NameTW,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            return styles.AsQueryable();
        }
        public IQueryable<Models.Views.StyleMaterial> GetStyleMaterial(string predicate, string[] filters)
        {
            var isSpecial = false;

            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                isSpecial = (bool)extenFilters.Field9;
            }
            var styleItems = (
                from s in Style.Get()
                join a in Article.Get() on new { ArticleId = s.ArticleId, LocaleId = s.LocaleId } equals new { ArticleId = a.Id, LocaleId = a.LocaleId }
                join si in StyleItem.Get() on new { StyleId = s.Id, LocaleId = s.LocaleId } equals new { StyleId = si.StyleId, LocaleId = si.LocaleId }
                join ap in ArticlePart.Get() on new { ArticlePartId = si.ArticlePartId, LocaleId = si.LocaleId } equals new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId } into apGRP
                from ap in apGRP.DefaultIfEmpty()
                join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                join m in Material.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                join c in CodeItem.Get() on new { CategoryCodeId = m.CategoryCodeId, LocaleId = m.LocaleId } equals new { CategoryCodeId = (decimal?)c.Id, LocaleId = c.LocaleId } into cGRP
                from c in cGRP.DefaultIfEmpty()
                join k in Knife.Get() on new { KnifeId = s.KnifeId, LocaleId = s.LocaleId } equals new { KnifeId = (decimal?)k.Id, LocaleId = k.LocaleId } into kGRP
                from k in kGRP.DefaultIfEmpty()
                join o in Outsole.Get() on new { OutsoleId = s.OutsoleId, LocaleId = s.LocaleId } equals new { OutsoleId = (decimal?)o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join l in Last.Get() on new { LastId = s.LastId, LocaleId = s.LocaleId } equals new { LastId = (decimal?)l.Id, LocaleId = l.LocaleId } into lGRP
                from l in lGRP.DefaultIfEmpty()
                select new
                {
                    ArticleNo = a.ArticleNo,
                    ArticleName = a.ArticleName,
                    StyleNo = s.StyleNo,
                    EnableMaterial = si.EnableMaterial,
                    PartNo = p.PartNo,
                    PartNameTw = p.PartNameTw,
                    Id = m.Id,
                    MaterialName = m.MaterialName,
                    MaterialNameEng = m.MaterialNameEng,
                    KnifeNo = k.KnifeNo,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = l.LastNo,
                    LocaleId = s.LocaleId,
                    IsSpecial = false,
                    Brand = CodeItem.Get().Where(i => i.Id == a.BrandCodeId && i.LocaleId == a.LocaleId).Max(i => i.NameTW),
                    CategoryCodeId = m.CategoryCodeId,
                    CategoryCode = c.NameTW,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.StyleMaterial
            {
                Id = i.Id,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialName,
                ArticleNo = i.ArticleNo,
                ArticleName = i.ArticleName,
                StyleNo = i.StyleNo,
                PartNo = i.PartNo,
                PartNameTw = i.PartNameTw,
                KnifeNo = i.KnifeNo,
                OutsoleNo = i.OutsoleNo,
                LastNo = i.LastNo,
                EnableMaterial = i.EnableMaterial,
                IsSpecial = i.IsSpecial,
                LocaleId = i.LocaleId,
                Brand = i.Brand,
                CategoryCodeId = i.CategoryCodeId,
                CategoryCode = i.CategoryCode,
            })
            .Distinct()
            .ToList();

            var specialItems = (
                from s in Style.Get()
                join a in Article.Get() on new { ArticleId = s.ArticleId, LocaleId = s.LocaleId } equals new { ArticleId = a.Id, LocaleId = a.LocaleId }
                join ssru in StyleSizeRunUsage.Get() on new { StyleId = s.Id, LocaleId = s.LocaleId } equals new { StyleId = ssru.StyleId, LocaleId = ssru.LocaleId }
                join m in Material.Get() on new { MaterialId = ssru.MaterialId, LocaleId = ssru.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                join c in CodeItem.Get() on new { CategoryCodeId = m.CategoryCodeId, LocaleId = m.LocaleId } equals new { CategoryCodeId = (decimal?)c.Id, LocaleId = c.LocaleId } into cGRP
                from c in cGRP.DefaultIfEmpty()
                join k in Knife.Get() on new { KnifeId = s.KnifeId, LocaleId = s.LocaleId } equals new { KnifeId = (decimal?)k.Id, LocaleId = k.LocaleId } into kGRP
                from k in kGRP.DefaultIfEmpty()
                join o in Outsole.Get() on new { OutsoleId = s.OutsoleId, LocaleId = s.LocaleId } equals new { OutsoleId = (decimal?)o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join l in Last.Get() on new { LastId = s.LastId, LocaleId = s.LocaleId } equals new { LastId = (decimal?)l.Id, LocaleId = l.LocaleId } into lGRP
                from l in lGRP.DefaultIfEmpty()
                select new
                {
                    ArticleNo = a.ArticleNo,
                    ArticleName = a.ArticleName,
                    StyleNo = s.StyleNo,
                    EnableMaterial = 1,
                    PartNo = "鞋型專用",
                    PartNameTw = "鞋型專用",
                    Id = m.Id,
                    MaterialName = m.MaterialName,
                    MaterialNameEng = m.MaterialNameEng,
                    KnifeNo = k.KnifeNo,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = l.LastNo,
                    LocaleId = s.LocaleId,
                    IsSpecial = true,
                    Brand = CodeItem.Get().Where(i => i.Id == a.BrandCodeId && i.LocaleId == a.LocaleId).Max(i => i.NameTW),
                    CategoryCodeId = m.CategoryCodeId,
                    CategoryCode = c.NameTW,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.StyleMaterial
            {
                Id = i.Id,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialName,
                ArticleNo = i.ArticleNo,
                ArticleName = i.ArticleName,
                StyleNo = i.StyleNo,
                PartNo = i.PartNo,
                PartNameTw = i.PartNameTw,
                KnifeNo = i.KnifeNo,
                OutsoleNo = i.OutsoleNo,
                LastNo = i.LastNo,
                IsSpecial = i.IsSpecial,
                LocaleId = i.LocaleId,
                EnableMaterial = i.EnableMaterial,
                Brand = i.Brand,
                CategoryCodeId = i.CategoryCodeId,
                CategoryCode = i.CategoryCode,
            })
            .Distinct()
            .ToList();

            if (isSpecial)
            {
                return specialItems.AsQueryable();
            }

            var items = styleItems.Union(specialItems);
            return items.AsQueryable();
        }
        public IQueryable<Models.Views.StyleMaterial> GetStyleMaterialByBOM(string predicate)
        {
            var styleItems = (
                from o in Orders.Get()
                join m in MRPItem.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = m.OrdersId, LocaleId = m.LocaleId }
                select new
                {
                    ArticleNo = o.ArticleNo,
                    ArticleName = o.ArticleName,
                    StyleNo = o.StyleNo,
                    EnableMaterial = 1,
                    PartNameTw = m.PartNameTw,
                    Id = m.MaterialId,
                    MaterialName = m.MaterialNameTw,
                    MaterialNameEng = m.MaterialNameEn,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = o.LastNo,
                    LocaleId = o.LocaleId,
                    IsSpecial = false,
                    Season = o.Season,
                    Brand = o.Brand,
                    CompanyId = o.CompanyId,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.StyleMaterial
            {
                Id = i.Id,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialName,
                ArticleNo = i.ArticleNo,
                ArticleName = i.ArticleName,
                StyleNo = i.StyleNo,
                PartNameTw = i.PartNameTw,
                OutsoleNo = i.OutsoleNo,
                LastNo = i.LastNo,
                EnableMaterial = i.EnableMaterial,
                IsSpecial = i.IsSpecial,
                LocaleId = i.LocaleId,
                Season = i.Season,
                Brand = i.Brand
            })
            .Distinct()
            .ToList();

            var specialItems = (
                from o in Orders.Get()
                join m in MRPItemOrders.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = m.OrdersId, LocaleId = m.LocaleId }
                select new
                {
                    ArticleNo = o.ArticleNo,
                    ArticleName = o.ArticleName,
                    StyleNo = o.StyleNo,
                    EnableMaterial = 1,
                    PartNameTw = m.PartNameTw,
                    Id = m.MaterialId,
                    MaterialName = m.MaterialNameTw,
                    MaterialNameEng = m.MaterialNameEn,
                    OutsoleNo = o.OutsoleNo,
                    LastNo = o.LastNo,
                    LocaleId = o.LocaleId,
                    IsSpecial = true,
                    Season = o.Season,
                    Brand = o.Brand,
                    CompanyId = o.CompanyId,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.StyleMaterial
            {
                Id = i.Id,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialName,
                ArticleNo = i.ArticleNo,
                ArticleName = i.ArticleName,
                StyleNo = i.StyleNo,
                PartNameTw = i.PartNameTw,
                OutsoleNo = i.OutsoleNo,
                LastNo = i.LastNo,
                IsSpecial = i.IsSpecial,
                LocaleId = i.LocaleId,
                EnableMaterial = i.EnableMaterial,
                Season = i.Season,
                Brand = i.Brand
            })
            .Distinct()
            .ToList();

            var items = styleItems.Union(specialItems);
            return items.AsQueryable();
        }
        public IQueryable<Models.Views.StyleLog> GetStyleLog(string predicate)
        {
            var logs = (
                from sl in StyleLog.Get()
                join s in Style.Get() on new { StyleId = sl.StyleId, LocaleId = sl.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId } into sGRP
                from s in sGRP.DefaultIfEmpty()
                join a in Article.Get() on new { ArticleId = s.ArticleId, LocaleId = s.LocaleId } equals new { ArticleId = a.Id, LocaleId = a.LocaleId } into aGRP
                from a in aGRP.DefaultIfEmpty()
                join p in Part.Get() on new { PartId = sl.PartId, LocaleId = sl.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                join c1 in CodeItem.Get() on new { BrandId = a.BrandCodeId, LocaleId = a.LocaleId } equals new { BrandId = c1.Id, LocaleId = c1.LocaleId } into c1GRP
                from c1 in c1GRP.DefaultIfEmpty()
                select new Models.Views.StyleLog
                {
                    Id = sl.Id,
                    LocaleId = sl.LocaleId,
                    StyleId = sl.StyleId,
                    PartId = sl.PartId,
                    ArticlePartId = sl.ArticlePartId,
                    MaterialId = sl.MaterialId,
                    TransDesc = sl.TransDesc,
                    Remark = sl.Remark,
                    MaterialNameTw = sl.MaterialNameTw,
                    ModifyUserName = sl.ModifyUserName,
                    LastUpdateTime = sl.LastUpdateTime,

                    ArticleNo = a.ArticleNo,
                    StyleNo = s.StyleNo,
                    PartNo = p.PartNo,
                    PartNameTw = p.PartNameTw,
                    PartNameEn = p.PartNameEn,
                    // MaterialNameEn = i.MaterialNameEn,
                    Brand = c1.NameTW,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            // .OrderByDescending(i => i.LastUpdateTime)
            .Take(5000)
            .ToList();

            logs.ForEach(i =>
            {
                i.StyleNo = i.StyleNo == null ? i.MaterialNameTw : i.StyleNo;
                i.PartNo = i.StyleNo == null ? "ALL" : i.PartNo;
                i.PartNameTw = i.StyleNo == null ? "ALL" : i.PartNameTw;
            });

            return logs.AsQueryable();
        }

        public IEnumerable<Models.Views.ArticlePartUsage> GetStylePartUsage(int styleId, int localeId)
        {
            var items = new List<Models.Views.ArticlePartUsage>();
            var style = Style.Get().Where(i => i.Id == styleId && i.LocaleId == localeId).FirstOrDefault();
            if (style != null)
            {
                var article = Article.Get().Where(i => i.Id == style.ArticleId && i.LocaleId == style.LocaleId).FirstOrDefault();

                var styleParts = (
                    from s in Style.Get()
                    join si in StyleItem.Get() on new { StyleId = s.Id, LocaleId = s.LocaleId } equals new { StyleId = si.StyleId, LocaleId = si.LocaleId }
                    join m in Material.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                    where s.Id == style.Id && s.LocaleId == style.LocaleId
                    select new
                    {
                        MaterialName = (string?)m.MaterialName,
                        MaterialNameEng = (string?)m.MaterialNameEng,
                        ArticleId = (decimal?)s.ArticleId,
                        LocaleId = (decimal?)s.LocaleId,
                        ArticlePartId = (decimal?)si.ArticlePartId,
                        MaterialId = (decimal?)si.MaterialId,
                    }
                );
                var articleParts = (
                    from ap in ArticlePart.Get()
                    join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                    join c in CodeItem.Get() on new { UnitId = ap.UnitCodeId, LocaleId = ap.LocaleId } equals new { UnitId = c.Id, LocaleId = c.LocaleId }
                    join s in styleParts on new { ArticlePartId = (decimal?)ap.Id, LocaleId = (decimal?)ap.LocaleId, ArticleId = (decimal?)ap.ArticleId } equals new { ArticlePartId = s.ArticlePartId, LocaleId = s.LocaleId, ArticleId = s.ArticleId } into sGRP
                    from s in sGRP.DefaultIfEmpty()
                    where ap.ArticleId == article.Id && ap.LocaleId == article.LocaleId
                    select new Models.Views.ArticlePartUsage
                    {
                        ArticleId = ap.ArticleId,
                        Id = ap.Id,
                        LocaleId = ap.LocaleId,
                        PartId = ap.PartId,
                        PartNo = p.PartNo,
                        PartNameTw = p.PartNameTw,
                        PartNameEn = p.PartNameEn,
                        UnitCodeId = ap.UnitCodeId,
                        UnitName = c.NameTW,
                        MaterialNameTw = s.MaterialName ?? "",
                        MaterialNameEng = s.MaterialNameEng ?? "",
                        MaterialId = s.MaterialId ?? 0m,
                    }
                )
                .ToList();

                var articleSizerun = ArticleSizeRun.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();
                var articleSizerunIds = articleSizerun.Select(i => i.Id).ToList();
                var articleSizerunUsage = ArticleSizeRunUsage.Get().Where(i => articleSizerunIds.Contains(i.ArticleSizeRunId) && i.LocaleId == style.LocaleId).ToList();
                var styleSizerunUsage = (
                    from su in StyleSizeRunUsage.Get()
                    join s in Style.Get() on new { StyleId = su.StyleId, LocaleId = su.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId }
                    join m in Material.Get() on new { MaterialId = su.MaterialId, LocaleId = su.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                    join c in CodeItem.Get() on new { UnitId = su.UnitCodeId, LocaleId = su.LocaleId } equals new { UnitId = c.Id, LocaleId = c.LocaleId }
                    where su.StyleId == style.Id && su.LocaleId == style.LocaleId
                    select new
                    {
                        Id = su.Id,
                        LocaleId = su.LocaleId,
                        PartId = 0,
                        PartNo = "",
                        PartNameTw = "型體專用",
                        PartNameEn = "Style Only",
                        UnitCodeId = su.UnitCodeId,
                        UnitName = c.NameTW,
                        MaterialNameTw = m.MaterialName,
                        MaterialNameEng = m.MaterialNameEng,
                        MaterialId = su.MaterialId,
                        ArticleSizeRunId = su.ArticleSizeRunId,
                        UnitUsage = su.UnitUsage,
                        ArticleId = s.ArticleId,
                    }
                )
                .ToList();

                // article part
                articleParts.ForEach(ap =>
                {
                    var articleHead = new List<string>();
                    var articleSizeRun = new List<string>();
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

                        var item = articleSizerunUsage.Where(u => u.ArticleSizeRunId == i.Id && u.ArticlePartId == ap.Id).FirstOrDefault();
                        var usage = item == null ? 0 : item.UnitUsage;
                        articleSizeRun.Add("\"" + field + "\":" + usage);
                        articleHead.Add("\"" + field + "\":\"" + i.ArticleDisplaySize + "\"");
                    });

                    ap.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                    ap.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
                });

                // style only
                var styleOnlyParts = new List<Models.Views.ArticlePartUsage>();
                var styleMaterials = styleSizerunUsage.Select(i => new
                {
                    UnitCodeId = i.UnitCodeId,
                    UnitName = i.UnitName,
                    MaterialNameTw = i.MaterialNameTw,
                    MaterialNameEng = i.MaterialNameEng,
                    MaterialId = i.MaterialId,
                    ArticleId = i.ArticleId,
                })
                .Distinct()
                .ToList();
                styleMaterials.ForEach(so =>
                {
                    var articleHead = new List<string>();
                    var articleSizeRun = new List<string>();
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

                        var item = styleSizerunUsage.Where(u => u.ArticleSizeRunId == i.Id && u.MaterialNameTw == so.MaterialNameTw).FirstOrDefault();
                        var usage = item == null ? 0 : item.UnitUsage;
                        articleSizeRun.Add("\"" + field + "\":" + usage);
                        articleHead.Add("\"" + field + "\":\"" + i.ArticleDisplaySize + "\"");
                    });

                    var articleSizeRunStr = "{" + string.Join(",", articleSizeRun) + "}";
                    var articleHeadStr = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";

                    styleOnlyParts.Add(new ArticlePartUsage
                    {
                        Id = 0,
                        LocaleId = localeId,
                        PartId = 0,
                        PartNo = "",
                        PartNameTw = "型體專用",
                        PartNameEn = "Style Only",
                        UnitCodeId = so.UnitCodeId,
                        UnitName = so.UnitName,
                        MaterialNameTw = so.MaterialNameTw,
                        MaterialNameEng = so.MaterialNameEng,
                        MaterialId = so.MaterialId,
                        ArticleHead = articleHeadStr,
                        ArticleSizeRun = articleSizeRunStr,
                        ArticleId = so.ArticleId,
                    });
                });

                // sort
                articleParts = articleParts.OrderBy(i => i.PartNo).ToList();
                styleOnlyParts = styleOnlyParts.OrderBy(i => i.MaterialNameTw).ToList();
                var allParts = articleParts.Union(styleOnlyParts);

                items = allParts.ToList();
            }
            return items;
        }
        public IEnumerable<Models.Views.ArticlePartUsage> GetStylePartUsage1(int styleId, int localeId)
        {
            var items = new List<Models.Views.ArticlePartUsage>();
            var style = Style.Get().Where(i => i.Id == styleId && i.LocaleId == localeId).FirstOrDefault();
            if (style != null)
            {
                var article = Article.Get().Where(i => i.Id == style.ArticleId && i.LocaleId == style.LocaleId).FirstOrDefault();

                var styleParts = (
                    from s in Style.Get()
                    join si in StyleItem.Get() on new { StyleId = s.Id, LocaleId = s.LocaleId } equals new { StyleId = si.StyleId, LocaleId = si.LocaleId }
                    join m in Material.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                    where s.Id == style.Id && s.LocaleId == style.LocaleId
                    select new
                    {
                        MaterialName = m.MaterialName,
                        MaterialNameEng = m.MaterialNameEng,
                        ArticleId = s.ArticleId,
                        LocaleId = s.LocaleId,
                        ArticlePartId = si.ArticlePartId,
                        MaterialId = si.MaterialId,
                    }
                );
                var articleParts = (
                    from ap in ArticlePart.Get()
                    join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                    join c in CodeItem.Get() on new { UnitId = ap.UnitCodeId, LocaleId = ap.LocaleId } equals new { UnitId = c.Id, LocaleId = c.LocaleId }
                    join s in styleParts on new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId, ArticleId = ap.ArticleId } equals new { ArticlePartId = s.ArticlePartId, LocaleId = s.LocaleId, ArticleId = s.ArticleId } into sGRP
                    from s in sGRP.DefaultIfEmpty()
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
                        MaterialNameTw = s.MaterialName,
                        MaterialNameEng = s.MaterialNameEng,
                        MaterialId = s.MaterialId,
                        ArticleId = ap.ArticleId,
                    }
                )
                .ToList();

                var articleSizerun = ArticleSizeRun.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();
                var articleSizerunIds = articleSizerun.Select(i => i.Id).ToList();
                var articleSizerunUsage = ArticleSizeRunUsage.Get().Where(i => articleSizerunIds.Contains(i.ArticleSizeRunId) && i.LocaleId == style.LocaleId).ToList();
                var styleSizerunUsage = (
                    from su in StyleSizeRunUsage.Get()
                    join s in Style.Get() on new { StyleId = su.StyleId, LocaleId = su.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId }
                    join m in Material.Get() on new { MaterialId = su.MaterialId, LocaleId = su.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                    join c in CodeItem.Get() on new { UnitId = su.UnitCodeId, LocaleId = su.LocaleId } equals new { UnitId = c.Id, LocaleId = c.LocaleId }
                    where su.StyleId == style.Id && su.LocaleId == style.LocaleId
                    select new
                    {
                        Id = su.Id,
                        LocaleId = su.LocaleId,
                        PartId = 0,
                        PartNo = "",
                        PartNameTw = "型體專用",
                        PartNameEn = "Style Only",
                        UnitCodeId = su.UnitCodeId,
                        UnitName = c.NameTW,
                        MaterialNameTw = m.MaterialName,
                        MaterialNameEng = m.MaterialNameEng,
                        MaterialId = su.MaterialId,
                        ArticleSizeRunId = su.ArticleSizeRunId,
                        UnitUsage = su.UnitUsage,
                        ArticleId = s.ArticleId,
                    }
                )
                .ToList();

                // article part
                articleParts.ForEach(ap =>
                {
                    var articleHead = new List<string>();
                    var articleSizeRun = new List<string>();
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

                        var item = articleSizerunUsage.Where(u => u.ArticleSizeRunId == i.Id && u.ArticlePartId == ap.Id).FirstOrDefault();
                        var usage = item == null ? 0 : item.UnitUsage;
                        articleSizeRun.Add("\"" + field + "\":" + usage);
                        articleHead.Add("\"" + field + "\":\"" + i.ArticleDisplaySize + "\"");
                    });

                    ap.ArticleSizeRun = "{" + string.Join(",", articleSizeRun) + "}";
                    ap.ArticleHead = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";
                });

                // style only
                var styleOnlyParts = new List<Models.Views.ArticlePartUsage>();
                var styleMaterials = styleSizerunUsage.Select(i => new
                {
                    UnitCodeId = i.UnitCodeId,
                    UnitName = i.UnitName,
                    MaterialNameTw = i.MaterialNameTw,
                    MaterialNameEng = i.MaterialNameEng,
                    MaterialId = i.MaterialId,
                    ArticleId = i.ArticleId,
                })
                .Distinct()
                .ToList();
                styleMaterials.ForEach(so =>
                {
                    var articleHead = new List<string>();
                    var articleSizeRun = new List<string>();
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

                        var item = styleSizerunUsage.Where(u => u.ArticleSizeRunId == i.Id && u.MaterialNameTw == so.MaterialNameTw).FirstOrDefault();
                        var usage = item == null ? 0 : item.UnitUsage;
                        articleSizeRun.Add("\"" + field + "\":" + usage);
                        articleHead.Add("\"" + field + "\":\"" + i.ArticleDisplaySize + "\"");
                    });

                    var articleSizeRunStr = "{" + string.Join(",", articleSizeRun) + "}";
                    var articleHeadStr = "{" + string.Join(",", articleHead.Distinct().OrderBy(c => c)) + "}";

                    styleOnlyParts.Add(new ArticlePartUsage
                    {
                        Id = 0,
                        LocaleId = localeId,
                        PartId = 0,
                        PartNo = "",
                        PartNameTw = "型體專用",
                        PartNameEn = "Style Only",
                        UnitCodeId = so.UnitCodeId,
                        UnitName = so.UnitName,
                        MaterialNameTw = so.MaterialNameTw,
                        MaterialNameEng = so.MaterialNameEng,
                        MaterialId = so.MaterialId,
                        ArticleHead = articleHeadStr,
                        ArticleSizeRun = articleSizeRunStr,
                        ArticleId = so.ArticleId,
                    });
                });

                // sort
                articleParts = articleParts.OrderBy(i => i.PartNo).ToList();
                styleOnlyParts = styleOnlyParts.OrderBy(i => i.MaterialNameTw).ToList();
                var allParts = articleParts.Union(styleOnlyParts);

                items = allParts.ToList();
            }
            return items;
        }
        public IQueryable<Models.Views.Article> GetArticle(string predicate)
        {
            var styles = (
                from a in Article.Get()
                join k in Knife.Get() on new { KnifeId = a.KnifeId, LocaleId = a.LocaleId } equals new { KnifeId = (decimal?)k.Id, LocaleId = k.LocaleId } into kGRP
                from k in kGRP.DefaultIfEmpty()
                join o in Outsole.Get() on new { OutsoleId = a.OutsoleId, LocaleId = a.LocaleId } equals new { OutsoleId = (decimal?)o.Id, LocaleId = o.LocaleId } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join l in Last.Get() on new { LastId = a.LastId, LocaleId = a.LocaleId } equals new { LastId = (decimal?)l.Id, LocaleId = l.LocaleId } into lGRP
                from l in lGRP.DefaultIfEmpty()
                join c1 in CodeItem.Get() on new { BrandId = a.BrandCodeId, LocaleId = a.LocaleId } equals new { BrandId = c1.Id, LocaleId = c1.LocaleId } into c1GRP
                from c1 in c1GRP.DefaultIfEmpty()
                    // join c2 in CodeItem.Get() on new { CategoryCodeId = a.CategoryCodeId, LocaleId = a.LocaleId } equals new { CategoryCodeId = (decimal?)c2.Id, LocaleId = c2.LocaleId } into c2GRP
                    // from c2 in c2GRP.DefaultIfEmpty()
                    // join c3 in CodeItem.Get() on new { ShoeClassCodeId = a.ShoeClassCodeId, LocaleId = a.LocaleId } equals new { ShoeClassCodeId = (decimal?)c3.Id, LocaleId = c3.LocaleId } into c3GRP
                    // from c3 in c3GRP.DefaultIfEmpty()
                select new Models.Views.Article
                {
                    Id = a.Id,
                    ArticleNo = a.ArticleNo,
                    ArticleName = a.ArticleName,
                    SizeRange = a.SizeRange,
                    ModifyUserName = a.ModifyUserName,
                    LastUpdateTime = a.LastUpdateTime,
                    LocaleId = a.LocaleId,
                    ProjectType = a.ProjectType,
                    BrandCodeId = a.BrandCodeId,
                    OutsoleId = a.OutsoleId,
                    KnifeId = a.KnifeId,
                    LastId = a.LastId,
                    Gender = a.Gender,
                    forCBD = a.forCBD,
                    IsAlternate = a.IsAlternate,
                    ShellId = a.ShellId,
                    DayCapacity = a.DayCapacity,
                    LastTurnover = a.LastTurnover,

                    Brand = c1.NameTW,
                    KnifeNo = k.KnifeNo,
                    LastNo = l.LastNo,
                    OutsoleNo = o.OutsoleNo,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            return styles.AsQueryable();
        }
        public IEnumerable<Models.Views.ArticlePartUsage> GetArticlePartUsage(int id, int localeId)
        {
            {
                var items = new List<ERP.Models.Views.ArticlePartUsage>();

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
                        join m in Material.Get() on new { MaterialId = s.MaterialId, LocaleId = s.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
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
                            ArticleNo = article.ArticleNo,
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
                    });


                    items = articleParts.OrderBy(i => i.PartNo).ToList();
                }
                return items;
            }
        }
        public byte[] GetUsageExcel(int id, int localeId)
        {
            var items = GetArticlePartUsage(id, localeId).ToList();
            var sb = new StringBuilder();
            foreach (var i in items)
            {
                // 跟 Java 程式一致的順序與欄位
                string knifeNo = i.KnifeNo?.Split('/')[0] ?? "";
                string piece = i.KnifeNo?.Contains("/") == true ? i.KnifeNo.Split('/')[1] : "";

                sb.Append(i.PartNo).Append('\t');
                sb.Append(i.PartNameTw).Append('\t');
                sb.Append(" ").Append('\t'); // 空欄
                sb.Append(" ").Append('\t'); // 空欄
                sb.Append(knifeNo).Append('\t');
                sb.Append(" ").Append('\t'); // 空欄
                sb.Append(i.MaterialNameTw).Append('\t');
                sb.Append(" ").Append('\t'); // 空欄
                sb.Append(" ").Append('\t'); // 空欄
                sb.Append(i.PieceOfPair.ToString()).Append('\t');
                sb.Append(i.ArticleNo).Append('\t');
                sb.Append('\n');
            }

            // ✅ 加上 UTF-8 with BOM，避免 Excel 顯示亂碼
            // var utf8WithBom = new UTF8Encoding(true);
            // return utf8WithBom.GetBytes(sb.ToString());

            var big5Encoding = Encoding.GetEncoding("big5");
            return big5Encoding.GetBytes(sb.ToString());
        }
    }
}