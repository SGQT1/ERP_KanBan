using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class SizeMappingItemService : BusinessService
    {
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.SizeCountryMappingService SizeCountryMapping { get; }

        public SizeMappingItemService(
            Services.Entities.SizeCountryMappingService sizeCountryMappingService, 
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.SizeCountryMapping = sizeCountryMappingService;
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.SizeMappingItem> Get()
        {
            return SizeCountryMapping.Get().Select(i => new Models.Views.SizeMappingItem
            {
                Id = i.Id,
                SizeCountryCodeId = i.SizeCountryCodeId,
                ShoeSize = i.ShoeSize,
                ShoeSizeSuffix = i.ShoeSizeSuffix,
                InnerShoeSize = i.InnerShoeSize,
                MappingCodeId = i.MappingCodeId,
                MappingSize = i.MappingSize,
                MappingSizeSuffix = i.MappingSizeSuffix,
                InnerMappingSize = i.InnerMappingSize,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                
                SizeCountryCode = CodeItem.Get().Where(c => c.Id == i.SizeCountryCodeId && c.LocaleId  == i.LocaleId && c.CodeType == "35").Select(c => c.NameTW).Max(),
                MappingCode = CodeItem.Get().Where(c => c.Id == i.MappingCodeId && c.LocaleId  == i.LocaleId && c.CodeType == "35").Select(c => c.NameTW).Max(),
            });
        }
        // public void CreateRange(IEnumerable<Models.Views.SizeMappingItem> items)
        // {
        //     SizeCountryMapping.CreateRange(BuildRange(items));
        // }
        // public void RemoveRange(Expression<Func<ERP.Models.Entities.SizeCountryMapping, bool>> predicate)
        // {
        //     SizeCountryMapping.RemoveRange(predicate);
        // }
        // private IEnumerable<ERP.Models.Entities.SizeCountryMapping> BuildRange(IEnumerable<Models.Views.SizeMappingItem> items)
        // {
        //     return items.Select(item => new ERP.Models.Entities.SizeCountryMapping
        //     {
        //         Id  = item.Id,
        //         SizeCountryCodeId  = item.SizeCountryCodeId,
        //         ShoeSize  = item.ShoeSize,
        //         ShoeSizeSuffix  = item.ShoeSizeSuffix,
        //         InnerShoeSize  = item.InnerShoeSize,
        //         MappingCodeId  = item.MappingCodeId,
        //         MappingSize  = item.MappingSize,
        //         MappingSizeSuffix  = item.MappingSizeSuffix,
        //         InnerMappingSize  = item.InnerMappingSize,
        //         ModifyUserName  = item.ModifyUserName,
        //         LastUpdateTime  = item.LastUpdateTime,
        //         LocaleId  = item.LocaleId,
        //     });
        // }


        public Models.Views.SizeMappingItem Create(Models.Views.SizeMappingItem item)
        {
            var _item = SizeCountryMapping.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.SizeMappingItem Update(Models.Views.SizeMappingItem item)
        {
            var _item = SizeCountryMapping.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.SizeMappingItem item)
        {
            SizeCountryMapping.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.SizeCountryMapping Build(Models.Views.SizeMappingItem item)
        {
            return new ERP.Models.Entities.SizeCountryMapping
            {
                Id  = item.Id,
                SizeCountryCodeId  = item.SizeCountryCodeId,
                ShoeSize  = item.ShoeSize,
                ShoeSizeSuffix  = item.ShoeSizeSuffix,
                InnerShoeSize  = item.InnerShoeSize,
                MappingCodeId  = item.MappingCodeId,
                MappingSize  = item.MappingSize,
                MappingSizeSuffix  = item.MappingSizeSuffix,
                InnerMappingSize  = item.InnerMappingSize,
                ModifyUserName  = item.ModifyUserName,
                LastUpdateTime  = item.LastUpdateTime,
                LocaleId  = item.LocaleId,
            };
        }
        
    }
}