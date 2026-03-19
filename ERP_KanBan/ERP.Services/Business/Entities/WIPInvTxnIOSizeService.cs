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
    public class WIPInvTxnIOSizeService : BusinessService
    {
        private Services.Entities.WIPInvTxnIOSizeService WIPInvTxnIOSize { get; }
        public WIPInvTxnIOSizeService(
            Services.Entities.WIPInvTxnIOSizeService wipInvTxnIOSizeService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.WIPInvTxnIOSize = wipInvTxnIOSizeService;
        }
        public IQueryable<Models.Views.WIPInvTxnIOSize> Get()
        {
            return WIPInvTxnIOSize.Get().Select(i => new Models.Views.WIPInvTxnIOSize
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                WIPInvTxnIOId = i.WIPInvTxnIOId,
                DisplaySize = i.DisplaySize,
                ShoeSize = i.ShoeSize,
                ShoeSizeSuffix = i.ShoeSizeSuffix,
                ShoeInnerSize = i.ShoeInnerSize,
                SizeQty = i.SizeQty,
                SizeIOQty = i.SizeIOQty,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }

        public Models.Views.WIPInvTxnIOSize Create(Models.Views.WIPInvTxnIOSize item)
        {
            var _item = WIPInvTxnIOSize.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.WIPInvTxnIOSize Update(Models.Views.WIPInvTxnIOSize item)
        {
            var _item = WIPInvTxnIOSize.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.WIPInvTxnIOSize item)
        {
            WIPInvTxnIOSize.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.WIPInvTxnIOSize Build(Models.Views.WIPInvTxnIOSize item)
        {
            return new Models.Entities.WIPInvTxnIOSize()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                WIPInvTxnIOId = item.WIPInvTxnIOId,
                DisplaySize = item.DisplaySize,
                ShoeSize = item.ShoeSize,
                ShoeSizeSuffix = item.ShoeSizeSuffix,
                ShoeInnerSize = item.ShoeInnerSize,
                SizeQty = item.SizeQty,
                SizeIOQty = item.SizeIOQty,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,

            };
        }

    }
}