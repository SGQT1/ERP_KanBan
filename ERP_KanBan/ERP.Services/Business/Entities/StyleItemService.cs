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
    public class StyleItemService : BusinessService
    {
        private Services.Entities.StyleService Style { get; }
        private Services.Entities.StyleItemService StyleItem { get; }
        private Services.Entities.ArticlePartService ArticlePart { get; }
        private Services.Entities.PartService Part { get; }
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public StyleItemService(
            Services.Entities.StyleService styleService,
            Services.Entities.StyleItemService styleItemService,
            Services.Entities.ArticlePartService articlePartService,
            Services.Entities.PartService partService,
            Services.Entities.MaterialService materialService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Style = styleService;
            StyleItem = styleItemService;
            ArticlePart = articlePartService;
            Part = partService;
            Material = materialService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.StyleItem> Get()
        {
            var style = (
                from si in StyleItem.Get()
                join s in Style.Get() on new { StyleId = si.StyleId, LocaleId = si.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId }
                join ap in ArticlePart.Get() on new { ArticlePartId = si.ArticlePartId, LocaleId = si.LocaleId } equals new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId }
                join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                join m in Material.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new Models.Views.StyleItem
                {
                    Id = si.Id,
                    StyleId = si.StyleId,
                    ArticlePartId = si.ArticlePartId,
                    MaterialId = si.MaterialId,
                    Remark = si.Remark,
                    ModifyUserName = si.ModifyUserName,
                    LastUpdateTime = si.LastUpdateTime,
                    LocaleId = si.LocaleId,
                    EnableMaterial = si.EnableMaterial,
                    PartNo = p.PartNo,
                    PartNameTw = p.PartNameTw,
                    PartNameEn = p.PartNameEn,
                    MaterialNameTw = m.MaterialName,
                    MaterialNameEn = m.MaterialNameEng,
                    PieceOfPair = ap.PieceOfPair,
                    KnifeNo = ap.KnifeNo,
                    StyleNo = s.StyleNo,
                    ArticleId = s.ArticleId,
                    AlternateType = ap.AlternateType,
                    PartId = ap.PartId,
                    UnitCodeId = ap.UnitCodeId,
                    UnitCode = CodeItem.Get().Where(i => i.CodeType == "21" && i.Id == ap.UnitCodeId && i.LocaleId == ap.LocaleId).Max(i => i.NameTW),
                }
            );
            return style;
        }

        //以ArticlePart為主，用這個會連帶處理ArticlePart
        public IQueryable<Models.Views.StyleItem> GetByArticlePart(int articleId, int styleId, int localeId)
        {
            var styleItems = (
                from si in StyleItem.Get()
                join s in Style.Get() on new { StyleId = si.StyleId, LocaleId = si.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId }
                where si.StyleId == styleId && si.LocaleId == localeId
                select new
                {
                    Id = si.Id,
                    EnableMaterial = si.EnableMaterial,
                    StyleNo = s.StyleNo,
                    StyleId = si.StyleId,
                    MaterialId = si.MaterialId,
                    Remark = si.Remark,
                    ModifyUserName = si.ModifyUserName,
                    LastUpdateTime = si.LastUpdateTime,
                    ArticlePartId = si.ArticlePartId,
                    LocaleId = si.LocaleId,
                }
            );

            var style = (
                from ap in ArticlePart.Get()
                join si in styleItems on new { ArticlePartId = ap.Id, LocaleId = ap.LocaleId } equals new { ArticlePartId = si.ArticlePartId, LocaleId = si.LocaleId } into siGRP
                from si in siGRP.DefaultIfEmpty()
                join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                join m in Material.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                where ap.ArticleId == articleId && ap.LocaleId == localeId
                select new
                {
                    ArticleId = ap.ArticleId,
                    ArticlePartId = ap.Id,
                    AlternateType = ap.AlternateType,
                    UnitCodeId = ap.UnitCodeId,
                    KnifeNo = ap.KnifeNo,
                    PieceOfPair = ap.PieceOfPair,
                    LocaleId = ap.LocaleId,
                    PartId = ap.PartId,
                    Division = ap.Division,
                    DivisionOther = ap.DivisionOther,

                    Id = (decimal?)si.Id,
                    EnableMaterial = (int?)si.EnableMaterial,
                    StyleNo = (string?)si.StyleNo,
                    StyleId = (decimal?)si.StyleId,
                    MaterialId = (decimal?)si.MaterialId,
                    Remark = (string?)si.Remark,
                    PartNo = (string?)p.PartNo,
                    PartNameTw = (string?)p.PartNameTw,
                    PartNameEn = (string?)p.PartNameEn,
                    MaterialNameTw = (string?)m.MaterialName,
                    MaterialNameEn = (string?)m.MaterialNameEng,
                    UnitCode = CodeItem.Get().Where(i => i.CodeType == "21" && i.Id == ap.UnitCodeId && i.LocaleId == ap.LocaleId).Max(i => i.NameTW),
                    ModifyUserName = (string?)si.ModifyUserName,
                    LastUpdateTime = (DateTime?)si.LastUpdateTime,
                }
            )
            .Select(i => new Models.Views.StyleItem
            {
                ArticleId = i.ArticleId,
                ArticlePartId = i.ArticlePartId,
                AlternateType = i.AlternateType,
                UnitCodeId = i.UnitCodeId,
                KnifeNo = i.KnifeNo,
                PieceOfPair = i.PieceOfPair,
                LocaleId = i.LocaleId,
                PartId = i.PartId,
                Division = i.Division,
                DivisionOther = i.DivisionOther,

                Id = i.Id ?? 0,
                EnableMaterial = i.EnableMaterial ?? 0,
                StyleNo = i.StyleNo ?? "",
                StyleId = i.StyleId ?? 0,
                MaterialId = i.MaterialId ?? 0,
                Remark = i.Remark ?? "",
                PartNo = i.PartNo ?? "",
                PartNameTw = i.PartNameTw ?? "",
                PartNameEn = i.PartNameEn ?? "",
                MaterialNameTw = i.MaterialNameTw ?? "",
                MaterialNameEn = i.MaterialNameEn ?? "",
                UnitCode = i.UnitCode ?? "",
                ModifyUserName = i.ModifyUserName ?? "",
                LastUpdateTime = i.LastUpdateTime ?? DateTime.Now,
            })
            .ToList();
            return style.AsQueryable();
        }
        public IQueryable<Models.Views.StyleItem> BuildByArticlePart()
        {
            var style = (
                from ap in ArticlePart.Get()
                join p in Part.Get() on new { PartId = ap.PartId, LocaleId = ap.LocaleId } equals new { PartId = p.Id, LocaleId = p.LocaleId }
                select new Models.Views.StyleItem
                {
                    Id = 0,
                    LocaleId = ap.LocaleId,

                    EnableMaterial = 0,
                    Division = ap.Division,
                    DivisionOther = ap.DivisionOther,
                    PartNo = p.PartNo,
                    PartId = ap.PartId,
                    MaterialId = 0,
                    MaterialNameTw = "",
                    MaterialNameEn = "",
                    AlternateType = ap.AlternateType,
                    UnitCodeId = ap.UnitCodeId,
                    KnifeNo = ap.KnifeNo,
                    PieceOfPair = ap.PieceOfPair,
                    Remark = "",

                    // ModifyUserName = "",
                    // LastUpdateTime = si.LastUpdateTime,

                    PartNameTw = p.PartNameTw,
                    PartNameEn = p.PartNameEn,
                    StyleNo = "",
                    ArticleId = ap.ArticleId,
                    StyleId = 0,
                    ArticlePartId = ap.Id
                }
            );
            return style;
        }

        public void CreateRange(IEnumerable<Models.Views.StyleItem> items)
        {
            StyleItem.CreateRange(BuildRange(items));
        }
        public void CreateRangeKeepTime(IEnumerable<Models.Views.StyleItem> items)
        {
            StyleItem.CreateRangeKeepTime(BuildRange(items));
        }
        public void CreateRangeKeepAll(IEnumerable<Models.Views.StyleItem> items)
        {
            StyleItem.CreateRangeKeepAll(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.StyleItem, bool>> predicate)
        {
            StyleItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.StyleItem> BuildRange(IEnumerable<Models.Views.StyleItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.StyleItem
            {
                Id = item.Id,
                StyleId = item.StyleId,
                ArticlePartId = item.ArticlePartId,
                MaterialId = item.MaterialId,
                Remark = item.Remark,
                EnableMaterial = item.EnableMaterial,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            });
        }

        public Models.Views.StyleItem Create(Models.Views.StyleItem item)
        {
            var _item = StyleItem.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }

        private ERP.Models.Entities.StyleItem Build(Models.Views.StyleItem item)
        {
            return new ERP.Models.Entities.StyleItem
            {
                Id = item.Id,
                StyleId = item.StyleId,
                ArticlePartId = item.ArticlePartId,
                MaterialId = item.MaterialId,
                Remark = item.Remark,
                EnableMaterial = item.EnableMaterial,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            };
        }
    }
}