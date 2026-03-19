using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using Microsoft.Extensions.Configuration;
using Diamond.DataSource.Extensions;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using ERP.Models.Views.Common;

namespace ERP.Services.Business.Entities
{
    public class StyleService : BusinessService
    {
        public IConfiguration Configuration { get; }
        private Services.Entities.StyleService Style { get; }
        private Services.Entities.ArticleService Article { get; }
        private Services.Entities.KnifeService Knife { get; }
        private Services.Entities.OutsoleService Outsole { get; }
        private Services.Entities.LastService Last { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.CustomerService Customer { get; }
        public StyleService(
            IConfiguration iConfig,
            Services.Entities.StyleService styleService,
            Services.Entities.ArticleService articleService,
            Services.Entities.KnifeService knifeService,
            Services.Entities.OutsoleService outsoleService,
            Services.Entities.LastService lastService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.CustomerService customerService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Configuration = iConfig;
            Style = styleService;
            Article = articleService;
            Knife = knifeService;
            Outsole = outsoleService;
            Last = lastService;
            CodeItem = codeItemService;
            Customer = customerService;
        }
        public IQueryable<Models.Views.Style> Get()
        {
            var style = (
                from s in Style.Get()
                join a in Article.Get() on new { LocaleId = s.LocaleId, ArticleId = s.ArticleId } equals new { LocaleId = a.LocaleId, ArticleId = a.Id }
                join k in Knife.Get() on new { LocaleId = s.LocaleId, KnifeId = s.KnifeId } equals new { LocaleId = k.LocaleId, KnifeId = (decimal?)k.Id } into kGrp
                from k in kGrp.DefaultIfEmpty()
                join o in Outsole.Get() on new { LocaleId = s.LocaleId, OutsoleId = s.OutsoleId } equals new { LocaleId = o.LocaleId, OutsoleId = (decimal?)o.Id } into oGrp
                from o in oGrp.DefaultIfEmpty()
                join l in Last.Get() on new { LocaleId = s.LocaleId, LastId = s.LastId } equals new { LocaleId = l.LocaleId, LastId = (decimal?)l.Id } into lGrp
                from l in lGrp.DefaultIfEmpty()
                select new Models.Views.Style
                {
                    Id = s.Id,
                    ArticleId = s.ArticleId,
                    ColorCode = s.ColorCode,
                    StyleNo = s.StyleNo,
                    ProcessNoteTW = s.ProcessNoteTW,
                    ColorDesc = s.ColorDesc,
                    InsockLabel = s.InsockLabel,
                    OutsoleColorDescTW = s.OutsoleColorDescTW,
                    OutsoleColorDescEN = s.OutsoleColorDescEN,
                    Photo1NoteTW = s.Photo1NoteTW,
                    Photo1URL = s.Photo1URL,
                    Photo2NoteTw = s.Photo2NoteTw,
                    Photo2URL = s.Photo2URL,
                    Photo3NoteTW = s.Photo3NoteTW,
                    Photo3URL = s.Photo3URL,
                    Photo4NoteTW = s.Photo4NoteTW,
                    Photo4URL = s.Photo4URL,
                    Photo5NoteTW = s.Photo5NoteTW,
                    Photo5URL = s.Photo5URL,
                    Photo6NoteTW = s.Photo6NoteTW,
                    Photo6URL = s.Photo6URL,
                    Photo7NoteTW = s.Photo7NoteTW,
                    Photo7URL = s.Photo7URL,
                    Photo8NoteTW = s.Photo8NoteTW,
                    Photo8URL = s.Photo8URL,
                    ModifyUserName = s.ModifyUserName,
                    ProcessNoteEng = s.ProcessNoteEng,
                    Photo1NoteEng = s.Photo1NoteEng,
                    Photo2NoteEng = s.Photo2NoteEng,
                    Photo3NoteEng = s.Photo3NoteEng,
                    Photo4NoteEng = s.Photo4NoteEng,
                    Photo5NoteEng = s.Photo5NoteEng,
                    Photo6NoteEng = s.Photo6NoteEng,
                    Photo7NoteEng = s.Photo7NoteEng,
                    Photo8NoteEng = s.Photo8NoteEng,
                    LastUpdateTime = s.LastUpdateTime,
                    OutsoleId = s.OutsoleId,
                    ShellId = s.ShellId,
                    FinishGoodsPhotoURL = s.FinishGoodsPhotoURL,
                    LocaleId = s.LocaleId,
                    CustomerId = s.CustomerId,
                    SizeCountryCodeId = s.SizeCountryCodeId,
                    ShoeClassCodeId = s.ShoeClassCodeId,
                    CategoryCodeId = s.CategoryCodeId,
                    doMRP = s.doMRP,
                    KnifeId = s.KnifeId,
                    LastId = s.LastId,
                    MoldNo = s.MoldNo,
                    ProjectId = s.ProjectId,
                    Version = s.Version,
                    CBDPrice = s.CBDPrice,
                    SampleSize = s.SampleSize,
                    SampleSizeSuffix = s.SampleSizeSuffix,
                    SampleInnerSize = s.SampleInnerSize,
                    IsSpecial = s.IsSpecial,

                    ArticleNo = a.ArticleNo,
                    ArticleName = a.ArticleName,
                    BrandCodeId = a.BrandCodeId,
                    Knife = k.KnifeNo,
                    Outsole = o.OutsoleNo,
                    Last = l.LastNo,
                    Brand = CodeItem.Get().Where(i => i.CodeType == "25" && i.Id == a.BrandCodeId && i.LocaleId == a.LocaleId).Max(i => i.NameTW),
                    ShoeClass = CodeItem.Get().Where(i => i.CodeType == "26" && i.Id == s.ShoeClassCodeId && i.LocaleId == s.LocaleId).Max(i => i.NameTW),
                    StyleSizeCountry = CodeItem.Get().Where(i => i.CodeType == "35" && i.Id == s.SizeCountryCodeId && i.LocaleId == s.LocaleId).Max(i => i.NameTW),
                    Category = CodeItem.Get().Where(i => i.CodeType == "41" && i.Id == s.CategoryCodeId && i.LocaleId == s.LocaleId).Max(i => i.NameTW),

                    Customer = Customer.Get().Where(i => i.Id == s.CustomerId && i.LocaleId == s.LocaleId).Max(i => i.ChineseName),
                }
            );
            return style;
        }

        // public IQueryable<Models.Views.Style> GetEntity()
        // {
        //     var style = (
        //         from s in Style.Get()
        //         join a in Article.Get() on new { LocaleId = s.LocaleId, ArticleId = s.ArticleId } equals new { LocaleId = a.LocaleId, ArticleId = a.Id }
        //         join k in Knife.Get() on new { LocaleId = s.LocaleId, KnifeId = s.KnifeId } equals new { LocaleId = k.LocaleId, KnifeId = (decimal?)k.Id } into kGrp
        //         from k in kGrp.DefaultIfEmpty()
        //         join o in Outsole.Get() on new { LocaleId = s.LocaleId, OutsoleId = s.OutsoleId } equals new { LocaleId = o.LocaleId, OutsoleId = (decimal?)o.Id } into oGrp
        //         from o in oGrp.DefaultIfEmpty()
        //         join l in Last.Get() on new { LocaleId = s.LocaleId, LastId = s.LastId } equals new { LocaleId = l.LocaleId, LastId = (decimal?)l.Id } into lGrp
        //         from l in lGrp.DefaultIfEmpty()
        //         select new Models.Views.Style
        //         {
        //             Id = s.Id,
        //             ArticleId = s.ArticleId,
        //             ColorCode = s.ColorCode,
        //             StyleNo = s.StyleNo,
        //             ProcessNoteTW = s.ProcessNoteTW,
        //             ColorDesc = s.ColorDesc,
        //             InsockLabel = s.InsockLabel,
        //             OutsoleColorDescTW = s.OutsoleColorDescTW,
        //             OutsoleColorDescEN = s.OutsoleColorDescEN,
        //             Photo1NoteTW = s.Photo1NoteTW,
        //             Photo1URL = s.Photo1URL,
        //             Photo2NoteTw = s.Photo2NoteTw,
        //             Photo2URL = s.Photo2URL,
        //             Photo3NoteTW = s.Photo3NoteTW,
        //             Photo3URL = s.Photo3URL,
        //             Photo4NoteTW = s.Photo4NoteTW,
        //             Photo4URL = s.Photo4URL,
        //             Photo5NoteTW = s.Photo5NoteTW,
        //             Photo5URL = s.Photo5URL,
        //             Photo6NoteTW = s.Photo6NoteTW,
        //             Photo6URL = s.Photo6URL,
        //             Photo7NoteTW = s.Photo7NoteTW,
        //             Photo7URL = s.Photo7URL,
        //             Photo8NoteTW = s.Photo8NoteTW,
        //             Photo8URL = s.Photo8URL,
        //             ModifyUserName = s.ModifyUserName,
        //             ProcessNoteEng = s.ProcessNoteEng,
        //             Photo1NoteEng = s.Photo1NoteEng,
        //             Photo2NoteEng = s.Photo2NoteEng,
        //             Photo3NoteEng = s.Photo3NoteEng,
        //             Photo4NoteEng = s.Photo4NoteEng,
        //             Photo5NoteEng = s.Photo5NoteEng,
        //             Photo6NoteEng = s.Photo6NoteEng,
        //             Photo7NoteEng = s.Photo7NoteEng,
        //             Photo8NoteEng = s.Photo8NoteEng,
        //             LastUpdateTime = s.LastUpdateTime,
        //             OutsoleId = s.OutsoleId,
        //             ShellId = s.ShellId,
        //             FinishGoodsPhotoURL = s.FinishGoodsPhotoURL,
        //             LocaleId = s.LocaleId,
        //             CustomerId = s.CustomerId,
        //             SizeCountryCodeId = s.SizeCountryCodeId,
        //             ShoeClassCodeId = s.ShoeClassCodeId,
        //             CategoryCodeId = s.CategoryCodeId,
        //             doMRP = s.doMRP,
        //             KnifeId = s.KnifeId,
        //             LastId = s.LastId,
        //             MoldNo = s.MoldNo,
        //             ProjectId = s.ProjectId,
        //             Version = s.Version,
        //             CBDPrice = s.CBDPrice,
        //             SampleSize = s.SampleSize,
        //             SampleSizeSuffix = s.SampleSizeSuffix,
        //             SampleInnerSize = s.SampleInnerSize,
        //             IsSpecial = s.IsSpecial,

        //             ArticleNo = a.ArticleNo,
        //             ArticleName = a.ArticleName,
        //             BrandCodeId = a.BrandCodeId,
        //             Knife = k.KnifeNo,
        //             Outsole = o.OutsoleNo,
        //             Last = l.LastNo,
        //             Brand = CodeItem.Get().Where(i => i.CodeType == "25" && i.Id == a.BrandCodeId && i.LocaleId == a.LocaleId).Max(i => i.NameTW),
        //             ShoeClass = CodeItem.Get().Where(i => i.CodeType == "26" && i.Id == s.ShoeClassCodeId && i.LocaleId == s.LocaleId).Max(i => i.NameTW),
        //             StyleSizeCountry = CodeItem.Get().Where(i => i.CodeType == "35" && i.Id == s.SizeCountryCodeId && i.LocaleId == s.LocaleId).Max(i => i.NameTW),
        //             Category = CodeItem.Get().Where(i => i.CodeType == "41" && i.Id == s.CategoryCodeId && i.LocaleId == s.LocaleId).Max(i => i.NameTW),

        //             Customer = Customer.Get().Where(i => i.Id == s.CustomerId && i.LocaleId == s.LocaleId).Max(i => i.ChineseName),
        //         }
        //     );
        //     return style;
        // }

        public IQueryable<Models.Views.Style> GetCopyStyle(string predicate, string[] filters)
        {
            var isWithoutStyle = false;
            var localeId = 0;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                localeId = (int)extenFilters.Field1;
                isWithoutStyle = (bool)extenFilters.Field9;
            }
            //找出StyleNo不能重複的，但ArticleNo卻需要在裡面有的
            var notIn = Style.Get().Where(i => i.LocaleId == localeId).Select(i => i.StyleNo).Distinct();
            var hasIn = Article.Get().Where(i => i.LocaleId == localeId).Select(i => i.ArticleNo).Distinct();

            var baseQuery = (
                    from s in Style.Get()
                    join a in Article.Get() on new { LocaleId = s.LocaleId, ArticleId = s.ArticleId } equals new { LocaleId = a.LocaleId, ArticleId = a.Id }
                    join k in Knife.Get() on new { LocaleId = s.LocaleId, KnifeId = s.KnifeId } equals new { LocaleId = k.LocaleId, KnifeId = (decimal?)k.Id } into kGrp
                    from k in kGrp.DefaultIfEmpty()
                    join o in Outsole.Get() on new { LocaleId = s.LocaleId, OutsoleId = s.OutsoleId } equals new { LocaleId = o.LocaleId, OutsoleId = (decimal?)o.Id } into oGrp
                    from o in oGrp.DefaultIfEmpty()
                    join l in Last.Get() on new { LocaleId = s.LocaleId, LastId = s.LastId } equals new { LocaleId = l.LocaleId, LastId = (decimal?)l.Id } into lGrp
                    from l in lGrp.DefaultIfEmpty()
                    select new Models.Views.Style
                    {
                        Id = s.Id,
                        ArticleId = s.ArticleId,
                        ColorCode = s.ColorCode,
                        StyleNo = s.StyleNo,
                        LastUpdateTime = s.LastUpdateTime,
                        OutsoleId = s.OutsoleId,
                        ShellId = s.ShellId,
                        LocaleId = s.LocaleId,
                        CustomerId = s.CustomerId,
                        doMRP = s.doMRP,
                        KnifeId = s.KnifeId,
                        LastId = s.LastId,
                        MoldNo = s.MoldNo,
                        Version = s.Version,
                        IsSpecial = s.IsSpecial,

                        ArticleNo = a.ArticleNo,
                        ArticleName = a.ArticleName,
                        BrandCodeId = a.BrandCodeId,
                        Brand = CodeItem.Get().Where(i => i.CodeType == "25" && i.Id == a.BrandCodeId && i.LocaleId == a.LocaleId).Max(i => i.NameTW),
                        Knife = k.KnifeNo,
                        Outsole = o.OutsoleNo,
                        Last = l.LastNo,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);
            var style = isWithoutStyle ? baseQuery.Where(i => !notIn.Contains(i.StyleNo) && hasIn.Contains(i.ArticleNo)).AsQueryable() // 這個條件是不能查到已經有的配色。用於舊系統的跨廠複製。但是如果同一個工廠的配色複製就不能用，所以先取消這個
                                       : baseQuery.Where(i => hasIn.Contains(i.ArticleNo)).AsQueryable();
            // var style = Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate).Where(i => !notIn.Contains(i.StyleNo) && hasIn.Contains(i.ArticleNo)).AsQueryable();

            return style;
        }
        public Models.Views.Style Create(Models.Views.Style item)
        {
            var _item = Style.Create(Build(item));
            SavePhoto(_item.FinishGoodsPhotoURL, item.FinishGoodsPhoto);
            SavePhoto(_item.Photo1URL, item.Photo1Photo);
            SavePhoto(_item.Photo2URL, item.Photo2Photo);
            SavePhoto(_item.Photo3URL, item.Photo3Photo);
            SavePhoto(_item.Photo4URL, item.Photo4Photo);
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Style Update(Models.Views.Style item)
        {
            var _item = Style.Update(Build(item));
            SavePhoto(_item.FinishGoodsPhotoURL, item.FinishGoodsPhoto);
            SavePhoto(_item.Photo1URL, item.Photo1Photo);
            SavePhoto(_item.Photo2URL, item.Photo2Photo);
            SavePhoto(_item.Photo3URL, item.Photo3Photo);
            SavePhoto(_item.Photo4URL, item.Photo4Photo);
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Style item)
        {
            Style.Remove(Build(item));
        }
        private Models.Entities.Style Build(Models.Views.Style item)
        {
            return new Models.Entities.Style()
            {
                Id = item.Id,
                ArticleId = item.ArticleId,
                ColorCode = item.ColorCode,
                StyleNo = item.StyleNo,
                ProcessNoteTW = item.ProcessNoteTW,
                ColorDesc = item.ColorDesc,
                InsockLabel = item.InsockLabel,
                OutsoleColorDescTW = item.OutsoleColorDescTW,
                OutsoleColorDescEN = item.OutsoleColorDescEN,
                Photo1NoteTW = item.Photo1NoteTW,
                // Photo1URL = item.Photo1URL,
                Photo1URL = (item.Photo1Photo != null && item.Photo1Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style01.jpg" : "",
                Photo2NoteTw = item.Photo2NoteTw,
                // Photo2URL = item.Photo2URL,
                Photo2URL = (item.Photo2Photo != null && item.Photo2Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style02.jpg" : "",
                Photo3NoteTW = item.Photo3NoteTW,
                // Photo3URL = item.Photo3URL,
                Photo3URL = (item.Photo3Photo != null && item.Photo3Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style03.jpg" : "",
                Photo4NoteTW = item.Photo4NoteTW,
                // Photo4URL = item.Photo4URL,
                Photo4URL = (item.Photo4Photo != null && item.Photo4Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style04.jpg" : "",
                Photo5NoteTW = item.Photo5NoteTW,
                // Photo5URL = item.Photo5URL,
                Photo5URL = (item.Photo5Photo != null && item.Photo5Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style05.jpg" : "",
                Photo6NoteTW = item.Photo6NoteTW,
                // Photo6URL = item.Photo6URL,
                Photo6URL = (item.Photo6Photo != null && item.Photo6Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style06.jpg" : "",
                Photo7NoteTW = item.Photo7NoteTW,
                // Photo7URL = item.Photo7URL,
                Photo7URL = (item.Photo7Photo != null && item.Photo7Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style07.jpg" : "",
                Photo8NoteTW = item.Photo8NoteTW,
                // Photo8URL = item.Photo8URL,
                Photo8URL = (item.Photo8Photo != null && item.Photo8Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style08.jpg" : "",
                ModifyUserName = item.ModifyUserName,
                ProcessNoteEng = item.ProcessNoteEng,
                Photo1NoteEng = item.Photo1NoteEng,
                Photo2NoteEng = item.Photo2NoteEng,
                Photo3NoteEng = item.Photo3NoteEng,
                Photo4NoteEng = item.Photo4NoteEng,
                Photo5NoteEng = item.Photo5NoteEng,
                Photo6NoteEng = item.Photo6NoteEng,
                Photo7NoteEng = item.Photo7NoteEng,
                Photo8NoteEng = item.Photo8NoteEng,
                LastUpdateTime = item.LastUpdateTime,
                OutsoleId = item.OutsoleId,
                ShellId = item.ShellId,
                // FinishGoodsPhotoURL = item.FinishGoodsPhotoURL,//p:/csw/style/1752564406269_D1GH2422-09.jpg
                FinishGoodsPhotoURL = (item.FinishGoodsPhoto != null && item.FinishGoodsPhoto.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style00.jpg" : "",

                LocaleId = item.LocaleId,
                CustomerId = item.CustomerId,
                SizeCountryCodeId = item.SizeCountryCodeId,
                ShoeClassCodeId = item.ShoeClassCodeId,
                CategoryCodeId = item.CategoryCodeId,
                doMRP = item.doMRP,
                KnifeId = item.KnifeId,
                LastId = item.LastId,
                MoldNo = item.MoldNo,
                ProjectId = item.ProjectId,
                Version = item.Version,
                CBDPrice = item.CBDPrice,
                SampleSize = item.SampleSize,
                SampleSizeSuffix = item.SampleSizeSuffix,
                SampleInnerSize = item.SampleInnerSize,
                IsSpecial = item.IsSpecial,
            };
        }
        public string GetPhoto(string path)
        {
            string base64Str = "";
            try
            {
                if (path != null && path.Length > 0)
                {
                    path = PathMap(path);

                    if (File.Exists(path))
                    {
                        var bytes = File.ReadAllBytes(path);
                        base64Str = "data:image/bmp;base64," + Convert.ToBase64String(bytes);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return base64Str;
        }
        public void SavePhoto(string path, string image)
        {
            if (path == null || image == null || (path.Length == 0 && image.Length == 0)) return;

            byte[] _imageBytes;
            var base64Content = image.Split(',')[1];

            try
            {
                path = PathMap(path);

                _imageBytes = Convert.FromBase64String(base64Content);
                System.IO.File.WriteAllBytes(path, _imageBytes);//檔案時體化    
            }
            catch (Exception e)
            {
                Console.WriteLine("SavePhoto Faile!:" + e.StackTrace);
                throw e;
            }
        }
        private string PathMap(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var companySetting = Configuration.GetSection(Environment.GetEnvironmentVariable("ERP_ENVIRONMENT"));
                var disk = companySetting.GetValue<string>("PhotoDisk");
                var host = companySetting.GetValue<string>("PhotoHost");

                // 使用 Regex 忽略大小寫取代 disk 為 host
                path = Regex.Replace(path, Regex.Escape(disk), host, RegexOptions.IgnoreCase);
                
                // 統一路徑符號為 Windows 的形式
                path = path.Replace("/", "\\");

            }
            return path;
        }
        private string PhotoRootPath()
        {
            var companySetting = Configuration.GetSection(Environment.GetEnvironmentVariable("ERP_ENVIRONMENT"));
            var rootPaht = companySetting.GetValue<string>("PhotoDisk");
            return rootPaht;
        }
    }
}