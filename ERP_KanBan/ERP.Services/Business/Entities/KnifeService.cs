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
    public class KnifeService : BusinessService
    {
        private Services.Entities.KnifeService Knife { get; }
        private Services.Entities.CustomerService Customer { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public KnifeService(
            Services.Entities.KnifeService knifeService,
            Services.Entities.CustomerService customerService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Knife = knifeService;
            Customer = customerService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.Knife> Get()
        {
            return Knife.Get().Select(i => new Models.Views.Knife
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                KnifeNo = i.KnifeNo,
                Remark = i.Remark,
                KnifePhotoURL = i.KnifePhotoURL,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                OwnerCompanyId = i.OwnerCompanyId,
                OwnerCustomerId = i.OwnerCustomerId,
                StoragePlace = i.StoragePlace,
                TotalValue = i.TotalValue,
                MoneyCodeId = i.MoneyCodeId,

                OwnerCustomer = Customer.Get().Where(c => c.LocaleId == i.LocaleId && c.Id == i.OwnerCustomerId).Max(c => c.ChineseName),
                MoneyCode = CodeItem.Get().Where(c => c.LocaleId == i.LocaleId && c.Id == i.MoneyCodeId && c.CodeType == "02").Max(c => c.NameTW),
            });
        }
        public IQueryable<Models.Views.Knife> GetCopyKnife(string predicate, int localeId)
        {
            var notIn = Knife.Get().Where(i => i.LocaleId == localeId).Select(i => i.KnifeNo).Distinct();
            var knife = Knife.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Where(i => !notIn.Contains(i.KnifeNo))
                .Select(i => new Models.Views.Knife
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    KnifeNo = i.KnifeNo,
                    Remark = i.Remark,
                    KnifePhotoURL = i.KnifePhotoURL,
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
            return knife;
        }
        public Models.Views.Knife Create(Models.Views.Knife item)
        {
            var _item = Knife.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Knife Update(Models.Views.Knife item)
        {
            var _item = Knife.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Knife item)
        {
            Knife.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Knife Build(Models.Views.Knife item)
        {
            return new Models.Entities.Knife()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                KnifeNo = item.KnifeNo,
                Remark = item.Remark,
                KnifePhotoURL = item.KnifePhotoURL,
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