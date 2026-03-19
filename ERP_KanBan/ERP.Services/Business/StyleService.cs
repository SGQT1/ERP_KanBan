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
using KellermanSoftware.CompareNetObjects;

namespace ERP.Services.Business
{
    public class StyleService : BusinessService
    {
        private ERP.Services.Business.Entities.StyleService Style { get; set; }
        private ERP.Services.Business.Entities.StyleItemService StyleItem { get; set; }
        private ERP.Services.Business.Entities.StyleItemService _StyleItem { get; set; }
        private ERP.Services.Entities.BOMLogService BOMLog { get; set; }
        private ERP.Services.Entities.StyleSizeRunUsageService StyleSizeRunUsage { get; set; }
        private ERP.Services.Entities.ArticlePartService ArticlePart { get; set; }
        private ERP.Services.Entities.ArticlePartService _ArticlePart { get; set; }
        private ERP.Services.Business.Entities.ArticleService Article { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }

        private ERP.Services.Entities.CustomerService Customer { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.PartService Part { get; set; }

        public StyleService(
            ERP.Services.Business.Entities.StyleService styleService,
            ERP.Services.Business.Entities.StyleItemService styleItemService,
            ERP.Services.Business.Entities.StyleItemService _styleItemService,
            ERP.Services.Entities.BOMLogService bomLogService,
            ERP.Services.Entities.StyleSizeRunUsageService styleSizeRunUsageService,
            ERP.Services.Entities.ArticlePartService articlePartService,
            ERP.Services.Entities.ArticlePartService _articlePartService,
            ERP.Services.Business.Entities.ArticleService articleService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.CustomerService customerService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.PartService partService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Style = styleService;
            StyleItem = styleItemService;
            _StyleItem = _styleItemService;
            BOMLog = bomLogService;
            StyleSizeRunUsage = styleSizeRunUsageService;
            ArticlePart = articlePartService;
            _ArticlePart = _articlePartService;
            Article = articleService;
            Orders = ordersService;

            Customer = customerService;
            CodeItem = codeItemService;
            Material = materialService;
            Part = partService;
        }

        public ERP.Models.Views.Style GetStyle(int id, int localeId)
        {
            return Style.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.StyleGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.StyleGroup();

            var style = Style.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (style != null)
            {
                style.FinishGoodsPhoto = Style.GetPhoto(style.FinishGoodsPhotoURL);
                style.Photo1Photo = Style.GetPhoto(style.Photo1URL);
                style.Photo2Photo = Style.GetPhoto(style.Photo2URL);
                style.Photo3Photo = Style.GetPhoto(style.Photo3URL);
                style.Photo4Photo = Style.GetPhoto(style.Photo4URL);

                var styleItems = StyleItem.GetByArticlePart((int)style.ArticleId, (int)style.Id, (int)style.LocaleId);
                var ordersCount = Orders.Get().Where(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId).Count();

                group.Style = style;
                group.StyleItem = styleItems.OrderBy(i => i.PartNo).ToList();
                group.StyleUseFor = new StyleUseFor { OrdersCount = ordersCount };
            }

            return group;
        }

        public ERP.Models.Views.StyleGroup BuildStyleGroup(int id, int localeId)
        {
            var group = new ERP.Models.Views.StyleGroup();

            var article = Article.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (article != null)
            {
                var style = new ERP.Models.Views.Style
                {
                    Id = 0,
                    ArticleId = article.Id,
                    ColorCode = "",
                    StyleNo = article.ArticleNo + '-',
                    ProcessNoteTW = "",
                    ColorDesc = "",
                    InsockLabel = "",
                    OutsoleColorDescTW = "",
                    OutsoleColorDescEN = "",
                    Photo1NoteTW = "",
                    Photo1URL = "",
                    Photo2NoteTw = "",
                    Photo2URL = "",
                    Photo3NoteTW = "",
                    Photo3URL = "",
                    Photo4NoteTW = "",
                    Photo4URL = "",
                    Photo5NoteTW = "",
                    Photo5URL = "",
                    Photo6NoteTW = "",
                    Photo6URL = "",
                    Photo7NoteTW = "",
                    Photo7URL = "",
                    Photo8NoteTW = "",
                    Photo8URL = "",
                    ModifyUserName = "",
                    ProcessNoteEng = "",
                    Photo1NoteEng = "",
                    Photo2NoteEng = "",
                    Photo3NoteEng = "",
                    Photo4NoteEng = "",
                    Photo5NoteEng = "",
                    Photo6NoteEng = "",
                    Photo7NoteEng = "",
                    Photo8NoteEng = "",
                    LastUpdateTime = DateTime.Now,
                    ShellId = 0,
                    FinishGoodsPhotoURL = "",
                    LocaleId = article.LocaleId,
                    CustomerId = 0,
                    SizeCountryCodeId = 0,
                    ShoeClassCodeId = 0,
                    CategoryCodeId = 0,
                    doMRP = 0,
                    KnifeId = article.KnifeId,
                    LastId = article.LastId,
                    OutsoleId = article.OutsoleId,
                    MoldNo = "",
                    ProjectId = 0,
                    Version = 1,
                    CBDPrice = 0,
                    SampleSize = "",
                    SampleSizeSuffix = "",
                    SampleInnerSize = 0,
                    IsSpecial = 0,

                    ArticleNo = article.ArticleNo,
                    ArticleName = article.ArticleName,
                    BrandCodeId = article.BrandCodeId,
                    Knife = "",
                    Outsole = "",
                    Last = "",
                };
                var styleItems = StyleItem.BuildByArticlePart().Where(i => i.ArticleId == article.Id && i.LocaleId == article.LocaleId).OrderBy(i => i.PartNo).ToList();

                group.Style = style;
                group.StyleItem = styleItems;
            }

            return group;
        }

        public ERP.Models.Views.StyleGroup CopyStyleGroup(int styleId, int refLocaleId, int localeId)
        {
            var group = new ERP.Models.Views.StyleGroup();
            var refStyleGroup = Get(styleId, refLocaleId);


            if (refStyleGroup.Style != null)
            {
                var refStyle = refStyleGroup.Style;
                var refStyleItem = refStyleGroup.StyleItem;

                if (refLocaleId == localeId)
                {
                    // 同工廠配色複製
                    var style = new ERP.Models.Views.Style
                    {
                        Id = 0,
                        ArticleId = refStyle.ArticleId,
                        ColorCode = "",
                        StyleNo = refStyle.ArticleNo + '-',
                        ProcessNoteTW = refStyle.ProcessNoteTW,
                        ColorDesc = "",
                        InsockLabel = refStyle.InsockLabel,
                        OutsoleColorDescTW = refStyle.OutsoleColorDescTW,
                        OutsoleColorDescEN = refStyle.OutsoleColorDescEN,
                        Photo1NoteTW = "",
                        Photo1URL = "",
                        Photo2NoteTw = "",
                        Photo2URL = "",
                        Photo3NoteTW = "",
                        Photo3URL = "",
                        Photo4NoteTW = "",
                        Photo4URL = "",
                        Photo5NoteTW = "",
                        Photo5URL = "",
                        Photo6NoteTW = "",
                        Photo6URL = "",
                        Photo7NoteTW = "",
                        Photo7URL = "",
                        Photo8NoteTW = "",
                        Photo8URL = "",
                        ModifyUserName = "",
                        ProcessNoteEng = "",
                        Photo1NoteEng = "",
                        Photo2NoteEng = "",
                        Photo3NoteEng = "",
                        Photo4NoteEng = "",
                        Photo5NoteEng = "",
                        Photo6NoteEng = "",
                        Photo7NoteEng = "",
                        Photo8NoteEng = "",
                        LastUpdateTime = DateTime.Now,
                        ShellId = refStyle.ShellId,
                        FinishGoodsPhotoURL = "",
                        LocaleId = refStyle.LocaleId,
                        CustomerId = refStyle.CustomerId,
                        SizeCountryCodeId = refStyle.SizeCountryCodeId,
                        ShoeClassCodeId = refStyle.ShoeClassCodeId,
                        CategoryCodeId = refStyle.CategoryCodeId,
                        doMRP = 0,
                        KnifeId = refStyle.KnifeId,
                        LastId = refStyle.LastId,
                        OutsoleId = refStyle.OutsoleId,
                        MoldNo = refStyle.MoldNo,
                        ProjectId = refStyle.ProjectId,
                        Version = 1,
                        CBDPrice = refStyle.CBDPrice,
                        SampleSize = refStyle.SampleSize,
                        SampleSizeSuffix = refStyle.SampleSizeSuffix,
                        SampleInnerSize = refStyle.SampleInnerSize,
                        IsSpecial = refStyle.IsSpecial,

                        ArticleNo = refStyle.ArticleNo,
                        ArticleName = refStyle.ArticleName,
                        BrandCodeId = refStyle.BrandCodeId,
                        Knife = refStyle.Knife,
                        Outsole = refStyle.Outsole,
                        Last = refStyle.Last,
                    };
                    var styleItem = refStyleItem.ToList();
                    styleItem.ForEach(i =>
                    {
                        i.Id = 0;
                        i.ModifyUserName = "";
                        i.LastUpdateTime = DateTime.Now;
                    });

                    group.Style = style;
                    group.StyleItem = styleItem.OrderBy(i => i.PartNo);
                }
                else
                {
                    // 跨廠複製
                    var style = Style.Get().Where(i => i.StyleNo == refStyle.StyleNo && i.LocaleId == localeId).FirstOrDefault();
                    var article = Article.Get().Where(i => i.ArticleNo == refStyle.ArticleNo && i.LocaleId == localeId).FirstOrDefault();

                    if (article != null)
                    {
                        var customerId = Customer.Get().Where(i => i.ChineseName == refStyle.Customer && i.LocaleId == localeId).Max(i => i.Id);

                        var codeTypes = new List<string> { "35", "26", "41", "21" };
                        var codeItems = CodeItem.Get().Where(i => i.LocaleId == localeId && codeTypes.Contains(i.CodeType)).ToList();

                        // var sizeCountryCodeId = codeItems.Where(i => i.CodeType == "35" && i.NameTW == refStyle.StyleSizeCountry).Max(i => i.Id);
                        // var shoeClassCodeId = codeItems.Where(i => i.CodeType == "26" && i.NameTW == refStyle.ShoeClass).Max(i => i.Id);
                        // var categoryCodeId = codeItems.Where(i => i.CodeType == "41" && i.NameTW == refStyle.Category).Max(i => i.Id);

                        var sizeCountryCodeId = codeItems.Where(i => i.CodeType == "35" && i.NameTW == refStyle.StyleSizeCountry).Select(i => i.Id).FirstOrDefault();
                        var shoeClassCodeId = codeItems.Where(i => i.CodeType == "26" && i.NameTW == refStyle.ShoeClass).Select(i => i.Id).FirstOrDefault();
                        var categoryCodeId = codeItems.Where(i => i.CodeType == "41" && i.NameTW == refStyle.Category).Select(i => i.Id).FirstOrDefault();

                        var pNames = refStyleItem.Select(i => i.PartNameTw).Distinct().ToList();
                        var parts = Part.Get().Where(i => i.LocaleId == localeId && pNames.Contains(i.PartNameTw)).ToList();

                        var mNames = refStyleItem.Select(i => i.MaterialNameTw).Distinct().ToList();
                        var materials = Material.Get().Where(i => i.LocaleId == localeId && mNames.Contains(i.MaterialName)).Distinct().ToList();
                        var articleParts = ArticlePart.Get().Where(i => i.LocaleId == localeId && i.ArticleId == article.Id).Distinct().ToList();

                        style = new ERP.Models.Views.Style
                        {
                            Id = 0,
                            LocaleId = localeId,
                            ArticleId = article.Id,
                            ColorCode = refStyle.ColorCode,
                            StyleNo = refStyle.StyleNo,
                            ProcessNoteTW = refStyle.ProcessNoteTW,
                            ColorDesc = refStyle.ColorDesc,
                            InsockLabel = refStyle.InsockLabel,
                            OutsoleColorDescTW = refStyle.OutsoleColorDescTW,
                            OutsoleColorDescEN = refStyle.OutsoleColorDescEN,
                            Photo1NoteTW = "",
                            Photo1URL = "",
                            Photo2NoteTw = "",
                            Photo2URL = "",
                            Photo3NoteTW = "",
                            Photo3URL = "",
                            Photo4NoteTW = "",
                            Photo4URL = "",
                            Photo5NoteTW = "",
                            Photo5URL = "",
                            Photo6NoteTW = "",
                            Photo6URL = "",
                            Photo7NoteTW = "",
                            Photo7URL = "",
                            Photo8NoteTW = "",
                            Photo8URL = "",

                            ProcessNoteEng = "",
                            Photo1NoteEng = "",
                            Photo2NoteEng = "",
                            Photo3NoteEng = "",
                            Photo4NoteEng = "",
                            Photo5NoteEng = "",
                            Photo6NoteEng = "",
                            Photo7NoteEng = "",
                            Photo8NoteEng = "",
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ShellId = refStyle.ShellId,
                            FinishGoodsPhotoURL = "",
                            CustomerId = customerId != null ? customerId : 0,
                            SizeCountryCodeId = sizeCountryCodeId != null ? sizeCountryCodeId : 0,
                            ShoeClassCodeId = shoeClassCodeId != null ? shoeClassCodeId : 0,
                            CategoryCodeId = categoryCodeId != null ? categoryCodeId : 0,
                            doMRP = 0,
                            KnifeId = article.KnifeId,
                            LastId = article.LastId,
                            OutsoleId = article.OutsoleId,
                            MoldNo = refStyle.MoldNo,
                            ProjectId = 0,
                            Version = 1,
                            CBDPrice = refStyle.CBDPrice,
                            SampleSize = refStyle.SampleSize,
                            SampleSizeSuffix = refStyle.SampleSizeSuffix,
                            SampleInnerSize = refStyle.SampleInnerSize,
                            IsSpecial = refStyle.IsSpecial,
                            BrandCodeId = article.BrandCodeId,
                            ArticleNo = refStyle.ArticleNo,
                            ArticleName = refStyle.ArticleName,
                            Knife = refStyle.Knife,
                            Outsole = refStyle.Outsole,
                            Last = refStyle.Last,
                        };

                        var styleItem = refStyleItem.ToList();
                        styleItem.ForEach(i =>
                        {
                            i.Id = 0;
                            i.LocaleId = localeId;
                            i.ModifyUserName = "";
                            i.LastUpdateTime = DateTime.Now;

                            var partId = parts.Where(p => p.PartNameTw == i.PartNameTw).Select(p => p.Id).DefaultIfEmpty(0).Max();
                            i.PartId = partId;

                            var articlePartId = articleParts.Where(a => a.PartId == partId && a.LocaleId == localeId).Select(a => a.Id).DefaultIfEmpty(0).Max();
                            i.ArticlePartId = articlePartId;

                            var unitCodeId = codeItems.Where(u => u.CodeType == "21" && u.NameTW == i.UnitCode).Select(u => u.Id).DefaultIfEmpty(0).Max();
                            i.UnitCodeId = unitCodeId;

                            var materialId = materials.Where(m => m.MaterialName == i.MaterialNameTw).Select(m => m.Id).DefaultIfEmpty(0).Max();
                            i.MaterialId = materialId;
                        });
                        group.Style = style;
                        group.StyleItem = styleItem;
                    }
                }

            }

            return group;
        }

        public ERP.Models.Views.StyleGroup Save(ERP.Models.Views.StyleGroup item)
        {
            var style = item.Style;
            var styleItem = item.StyleItem.ToList();
            var compareLogic = GetDiffLogic();

            if (style != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Style
                    {
                        var _style = Style.Get().Where(i => i.LocaleId == style.LocaleId && i.Id == style.Id).FirstOrDefault();
                        if (_style != null)
                        {
                            style.Id = _style.Id;
                            style.LocaleId = _style.LocaleId;
                            style = Style.Update(style);
                        }
                        else
                        {
                            style = Style.Create(style);
                        }
                    }

                    //Style Item
                    {
                        if (style.Id != 0 && styleItem.Any())
                        {

                            // 保留原有的styleItem
                            var oldStyleItem = _StyleItem.GetByArticlePart((int)style.ArticleId, (int)style.Id, (int)style.LocaleId).ToList();
                            var oldArticlePart = _ArticlePart.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();

                            // var articleParts = item.StyleItem.Select(i => new ERP.Models.Entities.ArticlePart
                            // {
                            //     // Id = oldArticlePart.Where(a => a.PartId == i.PartId).Select(a => a.Id).FirstOrDefault(),
                            //     Id = oldArticlePart.Where(a => a.Id == i.ArticlePartId).Select(a => a.Id).FirstOrDefault(),
                            //     ArticleId = style.ArticleId,
                            //     Division = i.Division,
                            //     DivisionOther = i.DivisionOther,
                            //     PartId = i.PartId,
                            //     UnitCodeId = i.UnitCodeId,
                            //     AlternateType = (int)i.AlternateType,
                            //     ModifyUserName = (i.ModifyUserName == null || i.ModifyUserName.Length == 0) ? style.ModifyUserName : i.ModifyUserName,
                            //     LocaleId = (i.LocaleId == null || i.LocaleId == 0) ? style.LocaleId : i.LocaleId,
                            //     KnifeNo = i.KnifeNo,
                            //     PieceOfPair = i.PieceOfPair,
                            // });

                            var articleParts = (
                                from si in styleItem
                                join oap in oldArticlePart on new { ArticlePartId = si.ArticlePartId, LocaleId = si.LocaleId } equals new { ArticlePartId = oap.Id, LocaleId = oap.LocaleId } into oapGRP
                                from oap in oapGRP.DefaultIfEmpty()
                                select new ERP.Models.Entities.ArticlePart
                                {
                                    Id = oap == null ? 0 : oap.Id,
                                    ArticleId = style.ArticleId,
                                    Division = si.Division,
                                    DivisionOther = si.DivisionOther,
                                    PartId = si.PartId,
                                    UnitCodeId = si.UnitCodeId,
                                    AlternateType = (int)si.AlternateType,
                                    ModifyUserName = (si.ModifyUserName == null || si.ModifyUserName.Length == 0) ? style.ModifyUserName : si.ModifyUserName,
                                    LocaleId = (si.LocaleId == null || si.LocaleId == 0) ? style.LocaleId : si.LocaleId,
                                    KnifeNo = si.KnifeNo,
                                    PieceOfPair = si.PieceOfPair,
                                }
                            );

                            ArticlePart.RemoveRange(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId);
                            ArticlePart.CreateRangeKeepId(articleParts);
                            articleParts = ArticlePart.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();

                            StyleItem.RemoveRange(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId);
                            styleItem.ForEach(i =>
                            {
                                if (i.Id != 0)
                                {
                                    // 判斷是不是有修改的Item
                                    var oldItem = oldStyleItem.Where(o => o.Id == i.Id).FirstOrDefault();
                                    if (oldItem != null)    // 已存在資料，只有資料異動才更新時間
                                    {
                                        ComparisonResult result = compareLogic.Compare(oldItem, i);
                                        if (!result.AreEqual)
                                        {
                                            i.StyleId = style.Id;
                                            i.LocaleId = style.LocaleId;
                                            i.ModifyUserName = style.ModifyUserName;
                                            i.LastUpdateTime = style.LastUpdateTime;

                                            // Console.WriteLine("========================");
                                            // Console.WriteLine(oldItem.PartNameTw);
                                            // Console.WriteLine(result.DifferencesString);
                                            // Console.WriteLine("========================");
                                        }
                                        else    // 已存在資料，資料沒異動不更新時間
                                        {
                                            i = oldItem;
                                        }
                                    }
                                }
                                else
                                {
                                    i.Id = 0;
                                    i.StyleId = style.Id;
                                    i.LocaleId = style.LocaleId;
                                    i.ModifyUserName = style.ModifyUserName;
                                    i.LastUpdateTime = style.LastUpdateTime;
                                }
                                var ap = articleParts.Where(p => p.PartId == i.PartId).FirstOrDefault();
                                i.ArticlePartId = ap != null ? ap.Id : 0;
                            });
                            var test = styleItem.Where(i => i.ModifyUserName == null).ToList();
                            StyleItem.CreateRangeKeepTime(styleItem);   // 因為資料表Id為自動新增，所以需要每次都新的Id

                            // 處理BOMLog
                            // step 1, 取得新增後的items
                            var bomLogs = new List<ERP.Models.Entities.BOMLog> { };
                            styleItem = StyleItem.GetByArticlePart((int)style.ArticleId, (int)style.Id, (int)style.LocaleId).ToList();

                            // step2, 取的刪除的
                            var removedItems = oldStyleItem.Where(old => old.ArticlePartId != null && !styleItem.Any(current => current.PartNo == old.PartNo)).ToList();
                            removedItems.ForEach(i =>
                            {
                                bomLogs.Add(new ERP.Models.Entities.BOMLog
                                {
                                    LocaleId = style.LocaleId,
                                    StyleId = style.Id,
                                    PartId = i.PartId,
                                    MaterialId = i.MaterialId,
                                    TransDesc = "Delete",
                                    ModifyUserName = style.ModifyUserName,
                                    ArticlePartId = i.ArticlePartId,
                                    MaterialNameTw = i.MaterialNameTw
                                });
                            });
                            // step3, 取得異動的
                            styleItem.ForEach(i =>
                            {
                                var diffItem = oldStyleItem.Where(o => o.ArticlePartId == i.ArticlePartId).FirstOrDefault();

                                if (diffItem == null)
                                {
                                    bomLogs.Add(new ERP.Models.Entities.BOMLog
                                    {
                                        LocaleId = style.LocaleId,
                                        StyleId = style.Id,
                                        PartId = i.PartId,
                                        MaterialId = i.MaterialId,
                                        TransDesc = "New",
                                        ModifyUserName = style.ModifyUserName,
                                        ArticlePartId = i.ArticlePartId,
                                        MaterialNameTw = i.MaterialNameTw
                                    });
                                }
                                else
                                {
                                    if (diffItem.EnableMaterial != i.EnableMaterial ||
                                        diffItem.MaterialId != i.MaterialId || diffItem.PartId != i.PartId || diffItem.UnitCodeId != i.UnitCodeId ||
                                        diffItem.PieceOfPair != i.PieceOfPair || diffItem.AlternateType != i.AlternateType || diffItem.KnifeNo != i.KnifeNo)
                                    {
                                        bomLogs.Add(new ERP.Models.Entities.BOMLog
                                        {
                                            LocaleId = style.LocaleId,
                                            StyleId = style.Id,
                                            PartId = i.PartId,
                                            MaterialId = i.MaterialId,
                                            TransDesc = "Update",
                                            ModifyUserName = style.ModifyUserName,
                                            ArticlePartId = i.ArticlePartId,
                                            MaterialNameTw = i.MaterialNameTw
                                        });
                                    }
                                }
                            });
                            // step4, 新增
                            if (bomLogs.Any())
                            {
                                BOMLog.CreateRange(bomLogs);
                            }
                        }
                    }
                    UnitOfWork.Commit();

                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
            // Cache.LoadMaterialCache((int)vendor.LocaleId);
            return Get((int)style.Id, (int)style.LocaleId);
        }

        public ERP.Models.Views.StyleGroup Save1(ERP.Models.Views.StyleGroup item)
        {
            var style = item.Style;
            var styleItem = item.StyleItem.ToList();

            if (style != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Style
                    {
                        var _style = Style.Get().Where(i => i.LocaleId == style.LocaleId && i.Id == style.Id).FirstOrDefault();
                        if (_style != null)
                        {
                            style.Id = _style.Id;
                            style.LocaleId = _style.LocaleId;
                            style = Style.Update(style);
                        }
                        else
                        {
                            style = Style.Create(style);
                        }
                    }

                    //Style Item
                    {
                        if (style.Id != 0 && styleItem.Any())
                        {
                            var bomLogs = new List<ERP.Models.Entities.BOMLog> { };
                            // 保留原有的styleItem
                            var oldStyleItem = _StyleItem.Get().Where(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId).ToList();
                            var oldArticlePart = _ArticlePart.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();

                            var articleParts = item.StyleItem.Select(i => new ERP.Models.Entities.ArticlePart
                            {
                                Id = oldArticlePart.Where(a => a.PartId == i.PartId).Select(a => a.Id).FirstOrDefault(),
                                ArticleId = style.ArticleId,
                                Division = i.Division,
                                DivisionOther = i.DivisionOther,
                                PartId = i.PartId,
                                UnitCodeId = i.UnitCodeId,
                                AlternateType = (int)i.AlternateType,
                                ModifyUserName = i.ModifyUserName,
                                LocaleId = i.LocaleId,
                                KnifeNo = i.KnifeNo,
                                PieceOfPair = i.PieceOfPair,
                            });
                            ArticlePart.RemoveRange(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId);
                            ArticlePart.CreateRangeKeepId(articleParts);
                            articleParts = ArticlePart.Get().Where(i => i.ArticleId == style.ArticleId && i.LocaleId == style.LocaleId).ToList();

                            StyleItem.RemoveRange(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId);
                            styleItem.ForEach(i =>
                            {
                                var ap = articleParts.Where(p => p.PartId == i.PartId).FirstOrDefault();
                                i.ArticlePartId = ap != null ? ap.Id : 0;

                                i.StyleId = style.Id;
                                i.ModifyUserName = style.ModifyUserName;
                                i.LocaleId = style.LocaleId;
                            });
                            StyleItem.CreateRange(styleItem);

                            // 處理BOMLog
                            // step 1, 取得新增後的items
                            styleItem = StyleItem.Get().Where(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId).ToList();

                            // step2, 取的刪除的
                            var removedItems = oldStyleItem.Where(old => old.ArticlePartId != null && !styleItem.Any(current => current.ArticlePartId == old.ArticlePartId)).ToList();
                            removedItems.ForEach(i =>
                            {
                                bomLogs.Add(new ERP.Models.Entities.BOMLog
                                {
                                    LocaleId = style.LocaleId,
                                    StyleId = style.Id,
                                    PartId = i.PartId,
                                    MaterialId = i.MaterialId,
                                    TransDesc = "Delete",
                                    ModifyUserName = style.ModifyUserName,
                                    ArticlePartId = i.ArticlePartId,
                                    MaterialNameTw = i.MaterialNameTw
                                });
                            });
                            // step3, 取得異動的
                            styleItem.ForEach(i =>
                            {
                                var diffItem = oldStyleItem.Where(o => o.ArticlePartId == i.ArticlePartId).FirstOrDefault();

                                if (diffItem == null)
                                {
                                    bomLogs.Add(new ERP.Models.Entities.BOMLog
                                    {
                                        LocaleId = style.LocaleId,
                                        StyleId = style.Id,
                                        PartId = i.PartId,
                                        MaterialId = i.MaterialId,
                                        TransDesc = "New",
                                        ModifyUserName = style.ModifyUserName,
                                        ArticlePartId = i.ArticlePartId,
                                        MaterialNameTw = i.MaterialNameTw
                                    });
                                }
                                else
                                {
                                    if (diffItem.EnableMaterial != i.EnableMaterial ||
                                        diffItem.MaterialId != i.MaterialId || diffItem.PartId != i.PartId || diffItem.UnitCodeId != i.UnitCodeId ||
                                        diffItem.PieceOfPair != i.PieceOfPair || diffItem.AlternateType != i.AlternateType || diffItem.KnifeNo != i.KnifeNo)
                                    {
                                        bomLogs.Add(new ERP.Models.Entities.BOMLog
                                        {
                                            LocaleId = style.LocaleId,
                                            StyleId = style.Id,
                                            PartId = i.PartId,
                                            MaterialId = i.MaterialId,
                                            TransDesc = "Update",
                                            ModifyUserName = style.ModifyUserName,
                                            ArticlePartId = i.ArticlePartId,
                                            MaterialNameTw = i.MaterialNameTw
                                        });
                                    }
                                }
                            });
                            // step4, 新增
                            if (bomLogs.Any())
                            {
                                BOMLog.CreateRange(bomLogs);
                            }
                        }
                    }
                    UnitOfWork.Commit();

                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
            // Cache.LoadMaterialCache((int)vendor.LocaleId);
            return Get((int)style.Id, (int)style.LocaleId);
        }

        public void Remove(ERP.Models.Views.StyleGroup group)
        {
            var style = group.Style;
            try
            {
                UnitOfWork.BeginTransaction();
                BOMLog.Create(new Models.Entities.BOMLog
                {
                    LocaleId = style.LocaleId,
                    StyleId = style.Id,
                    PartId = 0,
                    MaterialId = 0,
                    TransDesc = "Delete All",
                    ModifyUserName = style.ModifyUserName,
                    ArticlePartId = 0,
                    MaterialNameTw = style.StyleNo + " All Material"
                });

                StyleSizeRunUsage.RemoveRange(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId);
                StyleItem.RemoveRange(i => i.StyleId == style.Id && i.LocaleId == style.LocaleId);
                Style.Remove(style);

                UnitOfWork.Commit();

            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public IEnumerable<ERP.Models.Views.Style> GetStyleNo(string styleNo, int localeId)
        {
            return Style.Get().Where(i => i.StyleNo.ToLower() == styleNo.ToLower() && i.LocaleId == localeId).ToList();
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
            spec.Add(typeof(StyleItem), new string[] { "Id" });
            compareLogic.Config.CollectionMatchingSpec = spec;

            // ignore property
            compareLogic.Config.MembersToIgnore.Add("ModifyUserName");
            compareLogic.Config.MembersToIgnore.Add("LastUpdateTime");

            return compareLogic;
        }
    }
}
