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
    public class BondProductChinaContrastService : BusinessService
    {
        private Services.Entities.BondProductChinaContrastService BondProductChinaContrast { get; }
        private Services.Entities.ArticleService Article { get; }
        private Services.Entities.StyleService Style { get; }

        public BondProductChinaContrastService(
            Services.Entities.BondProductChinaContrastService BondProductChinaContrastService,
            Services.Entities.ArticleService articleService,
            Services.Entities.StyleService styleService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            BondProductChinaContrast = BondProductChinaContrastService;
            Article = articleService;
            Style = styleService;
        }
        public IQueryable<Models.Views.BondProductChinaContrast> Get()
        {
            return BondProductChinaContrast.Get().Select(i => new Models.Views.BondProductChinaContrast
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                StyleNo = i.StyleNo,
                ShoeName = i.ShoeName,
                UnitName = i.UnitName,
                BondProductName = i.BondProductName,
                WeightEachUnit = i.WeightEachUnit,
                UnitPrice = i.UnitPrice,
                DollarName = i.DollarName,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public IQueryable<Models.Views.BondProductChinaContrast> GetWithItem()
        {
            var result = (
                from s in Style.Get()
                join a in Article.Get() on new { ArticleId = s.ArticleId, LocaleId = s.LocaleId } equals new { ArticleId = a.Id, LocaleId = a.LocaleId }
                join b in BondProductChinaContrast.Get() on new { StyleNo = s.StyleNo, LocaleId = s.LocaleId } equals new { StyleNo = b.StyleNo, LocaleId = b.LocaleId } into bGRP
                from b in bGRP.DefaultIfEmpty()
                select new Models.Views.BondProductChinaContrast
                {
                    LocaleId = s.LocaleId,
                    StyleNo = s.StyleNo,
                    CategoryCodeId = s.CategoryCodeId,
                    ShoeName = a.ArticleName,

                    Id = b.Id,
                    UnitName = b.UnitName,
                    BondProductName = b.BondProductName,
                    WeightEachUnit = b.WeightEachUnit,
                    UnitPrice = b.UnitPrice,
                    DollarName = b.DollarName,
                    ModifyUserName = b.ModifyUserName,
                    LastUpdateTime = b.LastUpdateTime,

                });

            return result.AsQueryable();
        }
        public Models.Views.BondProductChinaContrast Create(Models.Views.BondProductChinaContrast item)
        {
            var _item = BondProductChinaContrast.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public Models.Views.BondProductChinaContrast Update(Models.Views.BondProductChinaContrast item)
        {
            var _item = BondProductChinaContrast.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.BondProductChinaContrast item)
        {
            BondProductChinaContrast.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.BondProductChinaContrast Build(Models.Views.BondProductChinaContrast item)
        {
            return new Models.Entities.BondProductChinaContrast()
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId,
                StyleNo = item.StyleNo,
                ShoeName = item.ShoeName,
                UnitName = item.UnitName,
                BondProductName = item.BondProductName,
                WeightEachUnit = item.WeightEachUnit,
                UnitPrice = item.UnitPrice,
                DollarName = item.DollarName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.BondProductChinaContrast> items)
        {
            BondProductChinaContrast.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.BondProductChinaContrast> items)
        {
            BondProductChinaContrast.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.BondProductChinaContrast, bool>> predicate)
        {
            BondProductChinaContrast.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.BondProductChinaContrast> BuildRange(IEnumerable<Models.Views.BondProductChinaContrast> items)
        {
            return items.Select(item => new Models.Entities.BondProductChinaContrast()
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId,
                StyleNo = item.StyleNo,
                ShoeName = item.ShoeName,
                UnitName = item.UnitName,
                BondProductName = item.BondProductName,
                WeightEachUnit = item.WeightEachUnit,
                UnitPrice = item.UnitPrice,
                DollarName = item.DollarName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }


    }
}