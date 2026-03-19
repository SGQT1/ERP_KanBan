using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class SizeMappingService : BusinessService
    {
        private Services.Entities.CodeItemService CodeItem { get; }

        public SizeMappingService(
            Services.Entities.CodeItemService codeItemService, 
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.SizeMapping> Get()
        {
            return CodeItem.Get().Select(i => new Models.Views.SizeMapping
            {
                Id = i.Id,
                CodeType = i.CodeType,
                CodeNo = Convert.ToInt32(i.CodeNo),
                NameTW = i.NameTW,
                NameEng = i.NameEng,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                ReferenceCodeNo = i.ReferenceCodeNo,
                LocaleId = i.LocaleId,
            });
        }
        public Models.Views.SizeMapping Create(Models.Views.SizeMapping item)
        {
            var _item = CodeItem.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.SizeMapping Update(Models.Views.SizeMapping item)
        {
            var _item = CodeItem.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.SizeMapping item)
        {
            CodeItem.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.CodeItem Build(Models.Views.SizeMapping item)
        {
            return new Models.Entities.CodeItem()
            {
                Id = item.Id,
                CodeType = item.CodeType,
                CodeNo = item.CodeNo.ToString(),
                NameTW = item.NameTW,
                NameEng = item.NameEng,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                ReferenceCodeNo = item.ReferenceCodeNo,
                LocaleId = item.LocaleId,
            };
        }
    }
}