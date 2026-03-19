using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class LastService : BusinessService
    {
        private Services.Entities.LastService Last { get; }

        private Services.Entities.CustomerService Customer { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public LastService(
            Services.Entities.LastService lastService,
            Services.Entities.CustomerService customerService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Last = lastService;
            Customer = customerService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.Last> Get()
        {
            return Last.Get().Select(item => new Models.Views.Last
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                LastNo = item.LastNo,
                Remark = item.Remark,
                FishGoodsPhotoURL = item.FishGoodsPhotoURL,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                OwnerCompanyId = item.OwnerCompanyId,
                OwnerCustomerId = item.OwnerCustomerId,
                StoragePlace = item.StoragePlace,
                TotalValue = item.TotalValue,
                MoneyCodeId = item.MoneyCodeId,

                OwnerCustomer = Customer.Get().Where(c => c.LocaleId == item.LocaleId && c.Id == item.OwnerCustomerId).Max(c => c.ChineseName),
                MoneyCode = CodeItem.Get().Where(c => c.LocaleId == item.LocaleId && c.Id == item.MoneyCodeId && c.CodeType == "02").Max(c => c.NameTW),
            });
        }
        public IQueryable<Models.Views.Last> GetCopyLast(string predicate, int localeId)
        {
            var notIn = Last.Get().Where(i => i.LocaleId == localeId).Select(i => i.LastNo).Distinct();
            var vendor = Last.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Where(i => !notIn.Contains(i.LastNo))
                .Select(i => new Models.Views.Last
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    LastNo = i.LastNo,
                    Remark = i.Remark,
                    FishGoodsPhotoURL = i.FishGoodsPhotoURL,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    OwnerCompanyId = i.OwnerCompanyId,
                    OwnerCustomerId = i.OwnerCustomerId,
                    StoragePlace = i.StoragePlace,
                    TotalValue = i.TotalValue,
                    MoneyCodeId = i.MoneyCodeId,

                    OwnerCustomer = Customer.Get().Where(c => c.LocaleId == i.LocaleId && c.Id == i.OwnerCustomerId).Max(c => c.ChineseName),
                    MoneyCode = CodeItem.Get().Where(c => c.LocaleId == i.LocaleId && c.Id == i.MoneyCodeId && c.CodeType == "02").Max(c => c.NameTW),
                })
                .ToList()
                .AsQueryable();
            return vendor;
        }
        public Models.Views.Last Create(Models.Views.Last item)
        {
            var _item = Last.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Last Update(Models.Views.Last item)
        {
            var _item = Last.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Last item)
        {
            Last.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Last Build(Models.Views.Last item)
        {
            return new Models.Entities.Last()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                LastNo = item.LastNo,
                Remark = item.Remark,
                FishGoodsPhotoURL = item.FishGoodsPhotoURL,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                OwnerCompanyId = item.OwnerCompanyId,
                OwnerCustomerId = item.OwnerCustomerId,
                StoragePlace = item.StoragePlace,
                TotalValue = item.TotalValue,
                MoneyCodeId = item.MoneyCodeId,
            };
        }
    }
}