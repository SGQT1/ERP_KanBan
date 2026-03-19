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
    public class MPSProcedureVendorItemService : BusinessService
    {
        private Services.Entities.MpsProcedureVendorItemService MpsProcedureVendorItem { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public MPSProcedureVendorItemService(
            Services.Entities.MpsProcedureVendorItemService mpsProcedureVendorItemService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.MpsProcedureVendorItem = mpsProcedureVendorItemService;
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.MPSProcedureVendorItem> Get()
        {
            return MpsProcedureVendorItem.Get().Select(i => new Models.Views.MPSProcedureVendorItem
            {
                Id = i.Id,
                MPSProcedureVendorId = i.MpsProcedureVendorId,
                BankName = i.BankName,
                AccountName = i.AccountName,
                AccountNo = i.AccountNo,
                BankAddress = i.BankAddress,
                DollarNameTw = i.DollarNameTw,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSProcedureVendorItem> items)
        {
            MpsProcedureVendorItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsProcedureVendorItem, bool>> predicate)
        {
            MpsProcedureVendorItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsProcedureVendorItem> BuildRange(IEnumerable<Models.Views.MPSProcedureVendorItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcedureVendorItem
            {
                Id = item.Id,
                MpsProcedureVendorId = item.MPSProcedureVendorId,
                BankName = item.BankName,
                AccountName = item.AccountName,
                AccountNo = item.AccountNo,
                BankAddress = item.BankAddress,
                DollarNameTw = item.DollarNameTw,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            });
        }
    }
}