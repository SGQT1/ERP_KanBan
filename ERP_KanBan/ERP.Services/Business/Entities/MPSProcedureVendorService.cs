using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSProcedureVendorService : BusinessService
    {
        private ERP.Services.Entities.MpsProcedureVendorService MPSProcedureVendor { get; set; }

        public MPSProcedureVendorService(
            ERP.Services.Entities.MpsProcedureVendorService mpsProcedureVendor,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedureVendor = mpsProcedureVendor;
        }

        public IQueryable<Models.Views.MPSProcedureVendor> Get()
        {
            return MPSProcedureVendor.Get().Select(i => new Models.Views.MPSProcedureVendor
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                VendorNo = i.VendorNo,
                ShortNameTw = i.ShortNameTw,
                NameTw = i.NameTw,
                NameEn = i.NameEn,
                NameLocal = i.NameLocal,
                OwnerName = i.OwnerName,
                TelNo1 = i.TelNo1,
                TelNo2 = i.TelNo2,
                FaxNo1 = i.FaxNo1,
                FaxNo2 = i.FaxNo2,
                CountryCodeId = i.CountryCodeId,
                AreaCodeId = i.AreaCodeId,
                UnifiedInvoiceNo = i.UnifiedInvoiceNo,
                CompanyAddressZip = i.CompanyAddressZip,
                CompanyAddress = i.CompanyAddress,
                Contact = i.Contact,
                ContactMobileNo = i.ContactMobileNo,
                ContactEmail = i.ContactEmail,
                PaymentCodeId = i.PaymentCodeId,
                DayOfMonth = i.DayOfMonth,
                PaymentPoint = i.PaymentPoint,
                Remark = i.Remark,
                CheckTitle = i.CheckTitle,
                CheckAddressZip = i.CheckAddressZip,
                CheckAddress = i.CheckAddress,
                CloseOff = i.CloseOff,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public Models.Views.MPSProcedureVendor Create(Models.Views.MPSProcedureVendor item)
        {
            var _item = MPSProcedureVendor.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcedureVendor Update(Models.Views.MPSProcedureVendor item)
        {
            var _item = MPSProcedureVendor.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcedureVendor item)
        {
            MPSProcedureVendor.Remove(Build(item));
        }
        private Models.Entities.MpsProcedureVendor Build(Models.Views.MPSProcedureVendor item)
        {
            return new Models.Entities.MpsProcedureVendor()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                VendorNo = item.VendorNo,
                ShortNameTw = item.ShortNameTw,
                NameTw = item.NameTw,
                NameEn = item.NameEn,
                NameLocal = item.NameLocal,
                OwnerName = item.OwnerName,
                TelNo1 = item.TelNo1,
                TelNo2 = item.TelNo2,
                FaxNo1 = item.FaxNo1,
                FaxNo2 = item.FaxNo2,
                CountryCodeId = item.CountryCodeId,
                AreaCodeId = item.AreaCodeId,
                UnifiedInvoiceNo = item.UnifiedInvoiceNo,
                CompanyAddressZip = item.CompanyAddressZip,
                CompanyAddress = item.CompanyAddress,
                Contact = item.Contact,
                ContactMobileNo = item.ContactMobileNo,
                ContactEmail = item.ContactEmail,
                PaymentCodeId = item.PaymentCodeId,
                DayOfMonth = item.DayOfMonth,
                PaymentPoint = item.PaymentPoint,
                Remark = item.Remark,
                CheckTitle = item.CheckTitle,
                CheckAddressZip = item.CheckAddressZip,
                CheckAddress = item.CheckAddress,
                CloseOff = item.CloseOff,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,

            };
        }
    }
}
