using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class WIPInvTxnIOService : BusinessService
    {
        private Services.Entities.WIPInvTxnIOService WIPInvTxnIO { get; }
        public WIPInvTxnIOService(
            Services.Entities.WIPInvTxnIOService wipInvTxnIOService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.WIPInvTxnIO = wipInvTxnIOService;
        }
        public IQueryable<Models.Views.WIPInvTxnIO> Get()
        {
            return WIPInvTxnIO.Get().Select(i => new Models.Views.WIPInvTxnIO
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                WIPInvTxnId = i.WIPInvTxnId,
                SourceType = i.SourceType,
                OrderNo = i.OrderNo,
                OrderQty = i.OrderQty,
                IODate = i.IODate,
                IOQty = i.IOQty,
                MPSProcessId = i.MPSProcessId,
                MPSProcessName = i.MPSProcessName,
                SourUnit = i.SourUnit,
                Remark = i.Remark,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }

        public Models.Views.WIPInvTxnIO Create(Models.Views.WIPInvTxnIO item)
        {
            var _item = WIPInvTxnIO.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.WIPInvTxnIO Update(Models.Views.WIPInvTxnIO item)
        {
            var _item = WIPInvTxnIO.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.WIPInvTxnIO item)
        {
            WIPInvTxnIO.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.WIPInvTxnIO Build(Models.Views.WIPInvTxnIO item)
        {
            return new Models.Entities.WIPInvTxnIO()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                WIPInvTxnId = item.WIPInvTxnId,
                SourceType = item.SourceType,
                OrderNo = item.OrderNo,
                OrderQty = item.OrderQty,
                IODate = item.IODate,
                IOQty = item.IOQty,
                MPSProcessId = item.MPSProcessId,
                MPSProcessName = item.MPSProcessName,
                SourUnit = item.SourUnit,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.WIPInvTxnIO> items)
        {
            WIPInvTxnIO.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.WIPInvTxnIO, bool>> predicate)
        {
            WIPInvTxnIO.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.WIPInvTxnIO> BuildRange(IEnumerable<Models.Views.WIPInvTxnIO> items)
        {
            return items.Select(item => new ERP.Models.Entities.WIPInvTxnIO
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                WIPInvTxnId = item.WIPInvTxnId,
                SourceType = item.SourceType,
                OrderNo = item.OrderNo,
                OrderQty = item.OrderQty,
                IODate = item.IODate,
                IOQty = item.IOQty,
                MPSProcessId = item.MPSProcessId,
                MPSProcessName = item.MPSProcessName,
                SourUnit = item.SourUnit,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

    }
}