using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class APTransferService : BusinessService
    {
        private Services.Entities.APTransferService APTransfer { get; }
        private Services.Entities.APTransferItemService APTransferItem { get; }
        private Services.Entities.CompanyService Company { get; }

        public APTransferService(
            Services.Entities.APTransferService apTransferService,
            Services.Entities.APTransferItemService apTransferItemService,
            Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            APTransfer = apTransferService;
            APTransferItem = apTransferItemService;
            Company = companyService;
        }
        public IQueryable<Models.Views.APTransfer> Get()
        {
            return APTransfer.Get().Select(i => new Models.Views.APTransfer
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                YYYYMM = i.YYYYMM,
                PaymentLocaleId = i.PaymentLocaleId,
                PaymentLocale = Company.Get().Where(c => c.Id == i.PaymentLocaleId).Max(i => i.CompanyNo),
                Locale = Company.Get().Where(c => c.Id == i.LocaleId).Max(i => i.CompanyNo),
            });
        }
        public IQueryable<Models.Views.APTransfer> GetWithItem()
        {
            var result = (
                from apt in APTransfer.Get()
                join apti in APTransferItem.Get() on new { APId = apt.Id, LocaleId = apt.LocaleId } equals new { APId = apti.APTransferId, LocaleId = apti.LocaleId } 
                select new {
                    Id = apt.Id,
                    LocaleId = apt.LocaleId,
                    ModifyUserName = apt.ModifyUserName,
                    LastUpdateTime = apt.LastUpdateTime,
                    YYYYMM = apt.YYYYMM,
                    Vendor = apti.VendorNameTw,
                    PaymentLocaleId = apt.PaymentLocaleId
                }
            )
            .Select(i => new Models.Views.APTransfer
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                YYYYMM = i.YYYYMM,
                PaymentLocaleId = i.PaymentLocaleId,
                PaymentLocale = Company.Get().Where(c => c.Id == i.PaymentLocaleId).Max(i => i.CompanyNo),
                Locale = Company.Get().Where(c => c.Id == i.LocaleId).Max(i => i.CompanyNo),
            })
            .Distinct();

            return result;
        }
        public Models.Views.APTransfer Create(Models.Views.APTransfer item)
        {
            var _item = APTransfer.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.APTransfer Update(Models.Views.APTransfer item)
        {
            var _item = APTransfer.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.APTransfer item)
        {
            APTransfer.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.APTransfer Build(Models.Views.APTransfer item)
        {
            return new Models.Entities.APTransfer()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                YYYYMM = item.YYYYMM,
            };
        }


    }
}