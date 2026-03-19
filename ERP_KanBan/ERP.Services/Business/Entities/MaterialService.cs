using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MaterialService : BusinessService
    {
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public MaterialService(
            Services.Entities.MaterialService materialService, 
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.Material = materialService;
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.Material> Get() 
        {
            return Material.Get().Select(i => new Models.Views.Material
            {
                Id = i.Id,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
                CategoryCodeId = i.CategoryCodeId,
                MaterialCodeId = i.MaterialCodeId,
                NameCodeId = i.NameCodeId,
                ThicknessCodeId = i.ThicknessCodeId,
                TextureCodeId = i.TextureCodeId,
                ColorCodeId = i.ColorCodeId,
                CanvasCodeId = i.CanvasCodeId,
                ProcessCodeId = i.ProcessCodeId,
                SpecCodeId = i.SpecCodeId,
                VesicantCodeId = i.VesicantCodeId,
                InColorCodeId = i.InColorCodeId,
                OutColorCodeId = i.OutColorCodeId,
                UsedUnitCodeId = i.UsedUnitCodeId,
                PriceUnitCodeId = i.PriceUnitCodeId,
                UsedPriceRate = i.UsedPriceRate,
                PurchasingUnitCodeId = i.PurchasingUnitCodeId,
                UsedPurchasingRate = i.UsedPurchasingRate,
                MinPurchasingQty = i.MinPurchasingQty,
                MinStockOutQty = i.MinStockOutQty,
                WeightEachUnit = i.WeightEachUnit,
                WeightUnitCodeId = i.WeightUnitCodeId,
                UsedWeightRate = i.UsedWeightRate,
                VolumeEachUnit = i.VolumeEachUnit,
                VolumeUnitCodeId = i.VolumeUnitCodeId,
                UsedVolumeRate = i.UsedVolumeRate,
                LastQuotationPrice = i.LastQuotationPrice,
                HighestPrice = i.HighestPrice,
                BeforeLastPrice = i.BeforeLastPrice,
                LowestPrice = i.LowestPrice,
                AcceptabilityOverRate = i.AcceptabilityOverRate,
                ModifyUserName = i.ModifyUserName,
                OtherDescTW = i.OtherDescTW,
                Hardness = i.Hardness,
                OtherDescEng = i.OtherDescEng,
                WeightEachYard = i.WeightEachYard,
                LastUpdateTime = i.LastUpdateTime,
                SemiGoods = i.SemiGoods,
                OtherName = i.OtherName,
                AddOnDesc = i.AddOnDesc,
                LocaleId = i.LocaleId,
                SamplingMethod = i.SamplingMethod,
                MaterialNo = i.MaterialNo,
                Text1 = i.Text1,
                Text2 = i.Text2,
                GroupId = i.GroupId,
                CategoryCode = CodeItem.Get().Where(c => c.Id == i.CategoryCodeId && c.LocaleId == i.LocaleId && c.CodeType == "11").Max(c => c.NameTW),
                VolumeUnitCode = CodeItem.Get().Where(c => c.Id == i.VolumeUnitCodeId && c.LocaleId == i.LocaleId && c.CodeType == "21").Max(c => c.NameTW),

                RefLocaleId = i.LocaleId, // for cross copy
            });
        }

        public IQueryable<Models.Views.Material> GetCache() 
        {
            return Material.Get().Select(i => new Models.Views.Material
            {
                Id = i.Id,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
                CategoryCodeId = i.CategoryCodeId,
                SemiGoods = i.SemiGoods,
                LocaleId = i.LocaleId,
                TextureCodeId = i.TextureCodeId,
            });
        }
        public Models.Views.Material Create(Models.Views.Material item)
        {
            var _item = Material.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Material Update(Models.Views.Material item)
        {
            var _item = Material.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }

        public void UpdateInspect(Models.Views.Material item)
        {
            Material.UpdateRange(
                i => i.Id == item.Id && i.LocaleId == item.LocaleId,
                // u => new Models.Entities.Material { SamplingMethod = item.SamplingMethod }
                u => u.SetProperty(p => p.SamplingMethod, v => item.SamplingMethod)
            );
        }

        public void Remove(Models.Views.Material item)
        {
            Material.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Material Build(Models.Views.Material item) {
            return new Models.Entities.Material()
            {
                Id = item.Id,
                MaterialName = item.MaterialName,
                MaterialNameEng = item.MaterialNameEng,
                CategoryCodeId = item.CategoryCodeId,
                MaterialCodeId = item.MaterialCodeId,
                NameCodeId = item.NameCodeId,
                ThicknessCodeId = item.ThicknessCodeId,
                TextureCodeId = item.TextureCodeId,
                ColorCodeId = item.ColorCodeId,
                CanvasCodeId = item.CanvasCodeId,
                ProcessCodeId = item.ProcessCodeId,
                SpecCodeId = item.SpecCodeId,
                VesicantCodeId = item.VesicantCodeId,
                InColorCodeId = item.InColorCodeId,
                OutColorCodeId = item.OutColorCodeId,
                UsedUnitCodeId = item.UsedUnitCodeId,
                PriceUnitCodeId = item.PriceUnitCodeId,
                UsedPriceRate = item.UsedPriceRate,
                PurchasingUnitCodeId = item.PurchasingUnitCodeId,
                UsedPurchasingRate = item.UsedPurchasingRate,
                MinPurchasingQty = item.MinPurchasingQty,
                MinStockOutQty = item.MinStockOutQty,
                WeightEachUnit = item.WeightEachUnit,
                WeightUnitCodeId = item.WeightUnitCodeId,
                UsedWeightRate = item.UsedWeightRate,
                VolumeEachUnit = item.VolumeEachUnit,
                VolumeUnitCodeId = item.VolumeUnitCodeId,
                UsedVolumeRate = item.UsedVolumeRate,
                LastQuotationPrice = item.LastQuotationPrice,
                HighestPrice = item.HighestPrice,
                BeforeLastPrice = item.BeforeLastPrice,
                LowestPrice = item.LowestPrice,
                AcceptabilityOverRate = item.AcceptabilityOverRate,
                ModifyUserName = item.ModifyUserName,
                OtherDescTW = item.OtherDescTW,
                Hardness = item.Hardness,
                OtherDescEng = item.OtherDescEng,
                WeightEachYard = item.WeightEachYard,
                LastUpdateTime = item.LastUpdateTime,
                SemiGoods = item.SemiGoods,
                OtherName = item.OtherName,
                AddOnDesc = item.AddOnDesc,
                LocaleId = item.LocaleId,
                SamplingMethod = item.SamplingMethod,
                MaterialNo = item.MaterialNo,
                Text1 = item.Text1,
                Text2 = item.Text2,
                GroupId = item.GroupId,
            };
        }
   
        public void CreateRange(IEnumerable<Models.Views.Material> items)
        {
            Material.CreateRange(BuildRange(items));
        }
        public IEnumerable<Models.Entities.Material> BuildRange(IEnumerable<Models.Views.Material> items)
        {
            return items.Select(item => new Models.Entities.Material
            {
                Id = item.Id,
                MaterialName = item.MaterialName,
                MaterialNameEng = item.MaterialNameEng,
                CategoryCodeId = item.CategoryCodeId,
                MaterialCodeId = item.MaterialCodeId,
                NameCodeId = item.NameCodeId,
                ThicknessCodeId = item.ThicknessCodeId,
                TextureCodeId = item.TextureCodeId,
                ColorCodeId = item.ColorCodeId,
                CanvasCodeId = item.CanvasCodeId,
                ProcessCodeId = item.ProcessCodeId,
                SpecCodeId = item.SpecCodeId,
                VesicantCodeId = item.VesicantCodeId,
                InColorCodeId = item.InColorCodeId,
                OutColorCodeId = item.OutColorCodeId,
                UsedUnitCodeId = item.UsedUnitCodeId,
                PriceUnitCodeId = item.PriceUnitCodeId,
                UsedPriceRate = item.UsedPriceRate,
                PurchasingUnitCodeId = item.PurchasingUnitCodeId,
                UsedPurchasingRate = item.UsedPurchasingRate,
                MinPurchasingQty = item.MinPurchasingQty,
                MinStockOutQty = item.MinStockOutQty,
                WeightEachUnit = item.WeightEachUnit,
                WeightUnitCodeId = item.WeightUnitCodeId,
                UsedWeightRate = item.UsedWeightRate,
                VolumeEachUnit = item.VolumeEachUnit,
                VolumeUnitCodeId = item.VolumeUnitCodeId,
                UsedVolumeRate = item.UsedVolumeRate,
                LastQuotationPrice = item.LastQuotationPrice,
                HighestPrice = item.HighestPrice,
                BeforeLastPrice = item.BeforeLastPrice,
                LowestPrice = item.LowestPrice,
                AcceptabilityOverRate = item.AcceptabilityOverRate,
                ModifyUserName = item.ModifyUserName,
                OtherDescTW = item.OtherDescTW,
                Hardness = item.Hardness,
                OtherDescEng = item.OtherDescEng,
                WeightEachYard = item.WeightEachYard,
                LastUpdateTime = item.LastUpdateTime,
                SemiGoods = item.SemiGoods,
                OtherName = item.OtherName,
                AddOnDesc = item.AddOnDesc,
                LocaleId = item.LocaleId,
                SamplingMethod = item.SamplingMethod,
                MaterialNo = item.MaterialNo,
                Text1 = item.Text1,
                Text2 = item.Text2,
                GroupId = item.GroupId,     
            });
        }
    }
}