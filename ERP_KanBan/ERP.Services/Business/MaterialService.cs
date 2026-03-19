using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace ERP.Services.Business
{
    public class MaterialService : BusinessService
    {
        private ERP.Services.Business.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.SOMService SOM { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.StyleItemService StyleItem { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.MaterialSamplingMethodService MaterialSamplingMethod { get; set; }

        private ERP.Services.Business.CacheService Cache { get; set; }

        public MaterialService(
            ERP.Services.Business.Entities.MaterialService materialService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.SOMService somService,
            ERP.Services.Entities.StyleItemService styleItemService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.MaterialSamplingMethodService materialSamplingMethodService,

            ERP.Services.Business.CacheService cacheService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Material = materialService;
            CodeItem = codeItemService;
            SOM = somService;
            StyleItem = styleItemService;
            POItem = poItemService;
            MaterialSamplingMethod = materialSamplingMethodService;

            Cache = cacheService;
        }

        public ERP.Models.Views.MaterialGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.MaterialGroup { };
            var material = Material.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();

            if (material != null)
            {
                var styleUse = StyleItem.Get().Where(i => i.MaterialId == id && i.LocaleId == localeId).Any();
                var poUse = POItem.Get().Where(i => i.MaterialId == id && i.LocaleId == localeId).Any();
                // var somPUse = SOM.Get().Where(i => i.ParentId == id && i.LocaleId == localeId).Any();
                var somCUse = SOM.Get().Where(i => i.ChildId == id && i.LocaleId == localeId).Any();

                group.Material = material;
                group.SOM = SOM.Get().Where(i => i.ParentId == id && i.LocaleId == localeId).OrderBy(i => i.SeqNo).ToList();
                group.UseFor = (styleUse || poUse || somCUse) ? true : false;
            }
            return group;
        }

        public ERP.Models.Views.MaterialGroup Save(ERP.Models.Views.MaterialGroup item)
        {
            var material = item.Material;
            var soms = item.SOM.ToList();

            UnitOfWork.BeginTransaction();
            try
            {
                // Duplicate Material Check, if true return false;
                if (material != null)
                {
                    if (material.Id > 0)
                    {
                        var _m = Material.Get().Where(i => i.LocaleId == material.LocaleId && i.Id != material.Id && i.MaterialName == material.MaterialName).Any();

                        if (_m) return null;
                    }
                    else
                    {
                        var _m = Material.Get().Where(i => i.LocaleId == material.LocaleId && i.MaterialName == material.MaterialName).Any();

                        if (_m) return null;
                    }
                }

                // update 驗貨方式、相關資料
                {
                    material.UsedPriceRate = 1;
                    material.UsedPurchasingRate = 1;
                    material.UsedWeightRate = 1;
                    material.UsedVolumeRate = 1;

                    material.AddOnDesc = "";
                    material.Hardness = "";
                    material.MaterialNo = "";
                    material.OtherDescEng = "";
                    material.OtherDescTW = "";
                    material.OtherName = "";
                    material.Text1 = "";
                    material.Text2 = "";
                    material.WeightEachYard = "";
                }

                // Id >> exist, ChineseName >> duplicate
                var _item = Material.Get().Where(i => i.LocaleId == material.LocaleId && i.Id == material.Id).FirstOrDefault();
                if (_item != null)
                {
                    material.Id = _item.Id;
                    material.LocaleId = _item.LocaleId;
                    material.GroupId = soms.Count() == 0 ? 1 : _item.GroupId + 1;

                    material = Material.Update(material);
                }
                else
                {
                    material = Material.Create(material);
                }

                if (material.Id != 0)
                {
                    SOM.RemoveRange(i => i.ParentId == material.Id);

                    soms.ForEach(i =>
                    {
                        i.ParentId = material.Id;
                        i.LocaleId = material.LocaleId;
                    });

                    SOM.CreateRange(soms);
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

            // Cache.LoadMaterialCache((int)material.LocaleId);
            return Get((int)material.Id, (int)material.LocaleId);
        }
        public void Remove(ERP.Models.Views.MaterialGroup item)
        {
            var material = item.Material;

            UnitOfWork.BeginTransaction();
            try
            {
                Material.Remove(material);
                SOM.RemoveRange(i => i.ParentId == material.Id && i.LocaleId == material.LocaleId);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

            // Cache.LoadMaterialCache((int)material.LocaleId);
        }

        public IQueryable<Models.Views.Material> GetPackingMaterial()
        {
            //CodeType:11 = Material Category
            var categroy = CodeItem.Get()
                .Where(i => i.CodeType == "11" &&
                            (i.NameTW.Contains("安全標") ||
                              i.NameTW.Contains("包裝") ||
                              i.NameTW.Contains("外箱") ||
                              i.NameTW.Contains("內盒") ||
                              i.NameTW.Contains("標籤")))
                 .Select(i => new { i.Id, i.LocaleId });

            var materials = (
                    from m in Material.Get()
                    join c in categroy on new { CategoryCodeId = (decimal)m.CategoryCodeId, LocaleId = m.LocaleId } equals new { CategoryCodeId = c.Id, LocaleId = c.LocaleId }
                    select m
                );
            return materials;
        }
        public IQueryable<Models.Views.Material> GetCrossMaterial(string predicate, string[] filters)
        {
            var extendPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("LocaleId")).ToArray();
                extendPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }

            var lMaterials = Material.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate).Select(m => m.MaterialName);

            var materials = Material.Get()
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Where(m => !lMaterials.Contains(m.MaterialName))
            .ToList();
            return materials.AsQueryable();
        }
        public void SaveCrossMaterial(List<Models.Views.Material> items)
        {
            var localeId = items.Max(i => i.LocaleId);
            var refLocaleId = items.Max(i => i.RefLocaleId);
            var modifyName = items.Max(i => i.ModifyUserName);

            var codeType = new string[] { "11", "21" };
            var codeItem = CodeItem.Get().Where(i => codeType.Contains(i.CodeType)).Select(i => new { i.Id, i.NameTW }).ToList();

            // get CategoryId, UnitId by Locale
            items.ForEach((Action<Models.Views.Material>)(i =>
            {
                i.CategoryCodeId = Enumerable.Where(codeItem, c => c.NameTW == i.CategoryCode).Max(c => c.Id);
                i.VolumeUnitCodeId = Enumerable.Where(codeItem, c => c.NameTW == i.VolumeUnitCode).Max(c => c.Id);
            }));

            try
            {
                // Material
                var materials = items.Where(i => i.SemiGoods == 0).ToList();
                // SemiGoods Material
                var semiGoods = items.Where(i => i.SemiGoods == 1).ToList();
                var _semiGoodIds = semiGoods.Select(i => i.Id).ToList();

                // Children of SemiGoods Material
                var _som = SOM.Get().Where(i => _semiGoodIds.Contains(i.ParentId) && i.LocaleId == refLocaleId).ToList();

                var _without = Material.Get().Where(i => i.LocaleId == localeId).Select(i => i.MaterialName);
                var _somWithout = Material.Get().Where(i => _som.Select(s => s.ChildId).Contains(i.Id) && i.LocaleId == refLocaleId && !_without.Contains(i.MaterialName))
                    .Select(i => new Models.Views.Material
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        MaterialName = i.MaterialName,
                        MaterialNameEng = i.MaterialNameEng,
                        CategoryCode = i.CategoryCode,
                        VolumeUnitCode = i.VolumeUnitCode,
                        SemiGoods = i.SemiGoods,
                        RefLocaleId = i.RefLocaleId,
                    })
                    .ToList();

                _somWithout.ForEach((Action<Models.Views.Material>)(i =>
                {
                    i.LocaleId = localeId;
                    i.ModifyUserName = modifyName;
                    i.CategoryCodeId = Enumerable.Where(codeItem, c => c.NameTW == i.CategoryCode).Max(c => c.Id);
                    i.VolumeUnitCodeId = Enumerable.Where(codeItem, c => c.NameTW == i.VolumeUnitCode).Max(c => c.Id);
                }));

                var allMaterials = (materials.Union(semiGoods).Union(_somWithout))
                    .GroupBy(g => new { g.LocaleId, g.MaterialName, g.MaterialNameEng, g.CategoryCode, g.VolumeUnitCode, g.SemiGoods, g.RefLocaleId, g.ModifyUserName, g.CategoryCodeId, g.VolumeUnitCodeId })
                    .Select(i => new Models.Views.Material
                    {
                        Id = 0,
                        LocaleId = i.Key.LocaleId,
                        MaterialName = i.Key.MaterialName,
                        MaterialNameEng = i.Key.MaterialNameEng,
                        CategoryCode = i.Key.CategoryCode,
                        VolumeUnitCode = i.Key.VolumeUnitCode,
                        SemiGoods = i.Key.SemiGoods,
                        RefLocaleId = i.Key.RefLocaleId,
                        ModifyUserName = i.Key.ModifyUserName,
                        CategoryCodeId = i.Key.CategoryCodeId,
                        VolumeUnitCodeId = i.Key.VolumeUnitCodeId,
                    }).ToList();

                Material.CreateRange(allMaterials);

                //SOM
                if (semiGoods.Count() > 0)
                {
                    var sAll = (semiGoods.Select(i => i.MaterialName).Union(_som.Select(i => i.ChildMaterialName))).Distinct();
                    var newMaterial = Material.Get().Where(i => i.LocaleId == localeId && sAll.Contains(i.MaterialName)).ToList();

                    var soms = new List<Models.Views.SOM>();

                    _som.ForEach(i =>
                    {
                        soms.Add(new Models.Views.SOM
                        {
                            Id = i.Id,
                            ParentId = newMaterial.Where(m => m.MaterialName == i.ParentMaterialName && m.LocaleId == localeId).Max(m => m.Id),
                            ParentMaterialName = i.ParentMaterialName,
                            ChildId = newMaterial.Where(m => m.MaterialName == i.ChildMaterialName && m.LocaleId == localeId).Max(m => m.Id),
                            ChildMaterialName = i.ChildMaterialName,
                            SeqNo = i.SeqNo,
                            Qty = i.Qty,
                            ModifyUserName = modifyName,
                            LastUpdateTime = i.LastUpdateTime,
                            LocaleId = localeId,
                            ItemGroupCode = i.ItemGroupCode,
                            ParentGroupCode = i.ParentGroupCode
                        });
                    });
                    SOM.CreateRange(soms);
                }


                UnitOfWork.BeginTransaction();

            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

    }
}
