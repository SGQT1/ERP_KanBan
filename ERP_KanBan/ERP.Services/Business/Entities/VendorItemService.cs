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
    public class VendorItemService : BusinessService
    {
        private Services.Entities.VendorItemService VendorItem { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public VendorItemService(
            Services.Entities.VendorItemService vendorItemService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.VendorItem = vendorItemService;
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.VendorItem> Get()
        {
            return VendorItem.Get().Select(i => new Models.Views.VendorItem
            {
                Id = i.Id,
                VendorId = i.VendorId,
                BankName = i.BankName,
                AccountName = i.AccountName,
                AccountNo = i.AccountNo,
                BankAddress = i.BankAddress,
                MoneyCodeId = i.MoneyCodeId,
                MoneyCode = CodeItem.Get().Where(c => c.Id == i.MoneyCodeId && c.LocaleId == i.LocaleId && c.CodeType == "02").Max(c => c.NameTW),
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.VendorItem> items)
        {
            VendorItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.VendorItem, bool>> predicate)
        {
            VendorItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.VendorItem> BuildRange(IEnumerable<Models.Views.VendorItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.VendorItem
            {
                Id = item.Id,
                VendorId = item.VendorId,
                BankName = item.BankName,
                AccountName = item.AccountName,
                AccountNo = item.AccountNo,
                BankAddress = item.BankAddress,
                MoneyCodeId = item.MoneyCodeId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            });
        }
    }
}