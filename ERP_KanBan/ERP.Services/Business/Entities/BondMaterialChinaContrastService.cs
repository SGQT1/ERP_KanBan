using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using Newtonsoft.Json;

namespace ERP.Services.Business.Entities
{
    public class BondMaterialChinaContrastService : BusinessService
    {
        private Services.Entities.BondMaterialChinaContrastService BondMaterialChinaContrast { get; }
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.MaterialQuotService MaterialQuot { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public BondMaterialChinaContrastService(
            Services.Entities.BondMaterialChinaContrastService BondMaterialChinaContrastService,
            Services.Entities.MaterialService materialService,
            Services.Entities.MaterialQuotService materialQuotService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            BondMaterialChinaContrast = BondMaterialChinaContrastService;
            Material = materialService;
            MaterialQuot = materialQuotService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.BondMaterialChinaContrast> Get()
        {
            return BondMaterialChinaContrast.Get().Select(i => new Models.Views.BondMaterialChinaContrast
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialId = i.MaterialId,
                MaterialName = i.MaterialName,
                UnitName = i.UnitName,
                BondMaterialName = i.BondMaterialName,
                WeightEachUnit = i.WeightEachUnit,
                UnitPrice = i.UnitPrice,
                DollarName = i.DollarName,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }

        public IQueryable<Models.Views.BondMaterialChinaContrast> GetWithItem(string[] filters)
        {
            var diff = false;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                diff = (bool)extenFilters.Field9;
            }

            var quot = (
                from q in MaterialQuot.Get()
                join c in CodeItem.Get() on new { UnitCodeId = q.UnitCodeId, LocaleId = q.LocaleId } equals new { UnitCodeId = c.Id, LocaleId = c.LocaleId }
                select new
                {
                    Id = q.Id,
                    MaterialId = q.MaterialId,
                    UnitName = c.NameTW,
                    UnitCodeId = q.UnitCodeId,
                    EffectiveDate = q.EffectiveDate,
                    LocaleId = q.LocaleId,
                });

            var result = (
                from m in Material.Get()
                join b in BondMaterialChinaContrast.Get() on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = b.MaterialId, LocaleId = b.LocaleId } into bGRP
                from b in bGRP.DefaultIfEmpty()
                select new Models.Views.BondMaterialChinaContrast
                {
                    LocaleId = m.LocaleId,
                    MaterialId = m.Id,
                    MaterialName = m.MaterialName,
                    UnitName = b.Id > 0 ? b.UnitName : quot.Where(i => i.LocaleId == m.LocaleId && i.MaterialId == m.Id).OrderByDescending(i => i.EffectiveDate).Select(i => i.UnitName).First(),
                    TextureCodeId = m.TextureCodeId,
                    CategoryCodeId = m.CategoryCodeId,

                    Id = b.Id,
                    BondMaterialName = b.BondMaterialName,
                    WeightEachUnit = b.WeightEachUnit,
                    UnitPrice = b.UnitPrice,
                    DollarName = b.DollarName,
                    ModifyUserName = b.ModifyUserName,
                    LastUpdateTime = b.LastUpdateTime,
                    RefMaterialName = b.MaterialName,
                    RefUnitName = b.UnitName,
                });

            if (diff)
            {
                result = result.Where(i => i.MaterialName != i.RefMaterialName && i.Id > 0);
            }
            return result;
        }
        public Models.Views.BondMaterialChinaContrast Create(Models.Views.BondMaterialChinaContrast item)
        {
            var _item = BondMaterialChinaContrast.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public Models.Views.BondMaterialChinaContrast Update(Models.Views.BondMaterialChinaContrast item)
        {
            var _item = BondMaterialChinaContrast.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.BondMaterialChinaContrast item)
        {
            BondMaterialChinaContrast.Remove(Build(item));
        }
        private Models.Entities.BondMaterialChinaContrast Build(Models.Views.BondMaterialChinaContrast item)
        {
            return new Models.Entities.BondMaterialChinaContrast()
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId,
                MaterialId = item.MaterialId,
                MaterialName = item.MaterialName,
                UnitName = item.UnitName,
                BondMaterialName = item.BondMaterialName,
                WeightEachUnit = item.WeightEachUnit,
                UnitPrice = item.UnitPrice,
                DollarName = item.DollarName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.BondMaterialChinaContrast> items)
        {
            BondMaterialChinaContrast.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.BondMaterialChinaContrast> items)
        {
            BondMaterialChinaContrast.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.BondMaterialChinaContrast, bool>> predicate)
        {
            BondMaterialChinaContrast.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.BondMaterialChinaContrast> BuildRange(IEnumerable<Models.Views.BondMaterialChinaContrast> items)
        {
            return items.Select(item => new Models.Entities.BondMaterialChinaContrast()
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId,
                MaterialId = item.MaterialId,
                MaterialName = item.MaterialName,
                UnitName = item.UnitName,
                BondMaterialName = item.BondMaterialName,
                WeightEachUnit = item.WeightEachUnit,
                UnitPrice = item.UnitPrice,
                DollarName = item.DollarName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

    }
}