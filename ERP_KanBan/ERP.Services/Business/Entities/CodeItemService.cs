using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class CodeItemService : BusinessService
    {
        private Services.Entities.CodeItemService CodeItem { get; }

        public CodeItemService(Services.Entities.CodeItemService codeItemService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.CodeItem> Get()
        {
            return CodeItem.Get().Select(i => new Models.Views.CodeItem
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
                Disable = i.Disable == 1 ? true : false,
            });
        }
        public Models.Views.CodeItem Create(Models.Views.CodeItem item)
        {
            var _item = CodeItem.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.CodeItem Update(Models.Views.CodeItem item)
        {
            var _item = CodeItem.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.CodeItem item)
        {
            CodeItem.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.CodeItem Build(Models.Views.CodeItem item)
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
                Disable = item.Disable == true ? 1 : 0,
            };
        }
        
    }
}