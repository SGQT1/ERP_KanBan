using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class WIPInvTxnService : BusinessService
    {
        private Services.Entities.WIPInvTxnService WIPInvTxn { get; }
        public WIPInvTxnService(
            Services.Entities.WIPInvTxnService wipInvTxnService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.WIPInvTxn = wipInvTxnService;
        }
        public IQueryable<Models.Views.WIPInvTxn> Get()
        {
            return WIPInvTxn.Get().Select(i => new Models.Views.WIPInvTxn
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                OrderNo = i.OrderNo,
                WarehourseNo = i.WarehourseNo,
                TotalQty = i.TotalQty,
                WIPType = i.WIPType,
            });
        }

        public Models.Views.WIPInvTxn Create(Models.Views.WIPInvTxn item)
        {
            var _item = WIPInvTxn.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.WIPInvTxn Update(Models.Views.WIPInvTxn item)
        {
            var _item = WIPInvTxn.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.WIPInvTxn item)
        {
            WIPInvTxn.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.WIPInvTxn Build(Models.Views.WIPInvTxn item)
        {
            return new Models.Entities.WIPInvTxn()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                OrderNo = item.OrderNo,
                WarehourseNo = item.WarehourseNo,
                TotalQty = item.TotalQty,
                WIPType = item.WIPType,
            };
        }

    }
}