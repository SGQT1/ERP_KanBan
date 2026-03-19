using System.Linq;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using Diamond.DataSource.Extensions;
using System.Linq.Dynamic.Core;

namespace ERP.Services.Business.Entities
{
    public class VendorService : BusinessService
    {
        private Services.Entities.VendorService Vendor { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        public VendorService(
            Services.Entities.VendorService vendorService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.Vendor = vendorService;
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.Vendor> Get()
        {
            var vendor = Vendor.Get().Select(i => new Models.Views.Vendor
            {
                Id = i.Id,
                TypeCodeId = i.TypeCodeId,
                VendorType = CodeItem.Get().Where(c => c.Id == i.TypeCodeId && c.LocaleId == i.LocaleId && c.CodeType == "23").Max(c => c.NameTW),
                ShortNameTw = i.ShortNameTw.Trim(),
                NameTw = i.NameTw.Trim(),
                NameEn = i.NameEn.Trim(),
                OwnerName = i.OwnerName,
                TelNo1 = i.TelNo1,
                TelNo2 = i.TelNo2,
                FaxNo1 = i.FaxNo1,
                FaxNo2 = i.FaxNo2,
                CountryCodeId = i.CountryCodeId,
                CountryCode = CodeItem.Get().Where(c => c.Id == i.CountryCodeId && c.LocaleId == i.LocaleId && c.CodeType == "01").Max(c => c.NameTW),
                AreaCodeId = i.AreaCodeId,
                AreaCode = CodeItem.Get().Where(c => c.Id == i.AreaCodeId && c.LocaleId == i.LocaleId && c.CodeType == "07").Max(c => c.NameTW),
                UnifiedInvoiceNo = i.UnifiedInvoiceNo,
                CompanyAddressZip = i.CompanyAddressZip,
                CompanyAddress = i.CompanyAddress,
                BillAddressZip = i.BillAddressZip,
                BillAddress = i.BillAddress,
                Contact = i.Contact,
                ContactMobileNo = i.ContactMobileNo,
                ContactEmail = i.ContactEmail,
                FirstTradeDate = i.FirstTradeDate,
                LastTradeDate = i.LastTradeDate,
                TaxCodeId = i.TaxCodeId,
                TaxCode = CodeItem.Get().Where(c => c.Id == i.TaxCodeId && c.LocaleId == i.LocaleId && c.CodeType == "09").Max(c => c.NameTW),
                DollarCodeId = i.DollarCodeId,
                DollarCode = CodeItem.Get().Where(c => c.Id == i.DollarCodeId && c.LocaleId == i.LocaleId && c.CodeType == "02").Max(c => c.NameTW),
                PaymentCodeId = i.PaymentCodeId,
                PaymentCode = CodeItem.Get().Where(c => c.Id == i.PaymentCodeId && c.LocaleId == i.LocaleId && c.CodeType == "10").Max(c => c.NameTW),
                Remark = i.Remark,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                PaymentPoint = i.PaymentPoint,
                CheckTitle = i.CheckTitle,
                CheckAddressZip = i.CheckAddressZip,
                CheckAddress = i.CheckAddress,
                DayOfMonth = i.DayOfMonth,
                CloseOff = i.CloseOff,
                QuoTaxIn = i.QuoTaxIn,
                IsTaxAdded = i.IsTaxAdded,
                TaxRate = i.TaxRate,
            })
            .Distinct();
            return vendor;
        }

        public IQueryable<Models.Views.Vendor> GetVendorCache() {
            return Vendor.Get().Select(i => new Models.Views.Vendor{
                Id = i.Id,
                ShortNameTw = i.ShortNameTw,
                NameTw = i.NameTw,
                DollarCodeId = i.DollarCodeId,
                DollarCode = CodeItem.Get().Where(c => c.Id == i.DollarCodeId && c.LocaleId == i.LocaleId && c.CodeType == "02").Max(c => c.NameTW),
                DayOfMonth = i.DayOfMonth,
                LocaleId = i.LocaleId,
                PaymentCodeId = i.PaymentCodeId,
                PaymentCode = CodeItem.Get().Where(c => c.Id == i.PaymentCodeId && c.LocaleId == i.LocaleId && c.CodeType == "10").Max(c => c.NameTW),
                PaymentPoint = i.PaymentPoint,
                TaxCodeId = i.TaxCodeId,
                TaxCode = CodeItem.Get().Where(c => c.Id == i.TaxCodeId && c.LocaleId == i.LocaleId && c.CodeType == "09").Max(c => c.NameTW),
                CloseOff = i.CloseOff,
            })
            .OrderBy(i => i.ShortNameTw);
        }

        public IQueryable<Models.Views.Vendor> GetCopyVendor(string predicate, int localeId)
        {
            var notIn = Vendor.Get().Where(i => i.LocaleId == localeId).Select(i => i.NameTw).Distinct();
            var vendor = Vendor.Get()
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Where(i => !notIn.Contains(i.NameTw))
            .Select(i => new Models.Views.Vendor
            {
                Id = i.Id,
                TypeCodeId = i.TypeCodeId,
                VendorType = CodeItem.Get().Where(c => c.Id == i.TypeCodeId && c.LocaleId == i.LocaleId && c.CodeType == "23").Max(c => c.NameTW),
                ShortNameTw = i.ShortNameTw,
                NameTw = i.NameTw,
                NameEn = i.NameEn,
                OwnerName = i.OwnerName,
                TelNo1 = i.TelNo1,
                TelNo2 = i.TelNo2,
                FaxNo1 = i.FaxNo1,
                FaxNo2 = i.FaxNo2,
                CountryCodeId = i.CountryCodeId,
                CountryCode = CodeItem.Get().Where(c => c.Id == i.CountryCodeId && c.LocaleId == i.LocaleId && c.CodeType == "01").Max(c => c.NameTW),
                AreaCodeId = i.AreaCodeId,
                AreaCode = CodeItem.Get().Where(c => c.Id == i.AreaCodeId && c.LocaleId == i.LocaleId && c.CodeType == "07").Max(c => c.NameTW),
                UnifiedInvoiceNo = i.UnifiedInvoiceNo,
                CompanyAddressZip = i.CompanyAddressZip,
                CompanyAddress = i.CompanyAddress,
                BillAddressZip = i.BillAddressZip,
                BillAddress = i.BillAddress,
                Contact = i.Contact,
                ContactMobileNo = i.ContactMobileNo,
                ContactEmail = i.ContactEmail,
                FirstTradeDate = i.FirstTradeDate,
                LastTradeDate = i.LastTradeDate,
                TaxCodeId = i.TaxCodeId,
                TaxCode = CodeItem.Get().Where(c => c.Id == i.TaxCodeId && c.LocaleId == i.LocaleId && c.CodeType == "09").Max(c => c.NameTW),
                DollarCodeId = i.DollarCodeId,
                DollarCode = CodeItem.Get().Where(c => c.Id == i.DollarCodeId && c.LocaleId == i.LocaleId && c.CodeType == "02").Max(c => c.NameTW),
                PaymentCodeId = i.PaymentCodeId,
                PaymentCode = CodeItem.Get().Where(c => c.Id == i.PaymentCodeId && c.LocaleId == i.LocaleId && c.CodeType == "10").Max(c => c.NameTW),
                Remark = i.Remark,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                PaymentPoint = i.PaymentPoint,
                CheckTitle = i.CheckTitle,
                CheckAddressZip = i.CheckAddressZip,
                CheckAddress = i.CheckAddress,
                DayOfMonth = i.DayOfMonth,
                CloseOff = i.CloseOff,
                QuoTaxIn = i.QuoTaxIn,
                IsTaxAdded = i.IsTaxAdded,
                TaxRate = i.TaxRate,
            })
            .ToList()
            .AsQueryable();
            return vendor;
        }

        public Models.Views.Vendor Create(Models.Views.Vendor item)
        {
            var _item = Vendor.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Vendor Update(Models.Views.Vendor item)
        {
            var _item = Vendor.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Vendor item)
        {
            Vendor.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Vendor Build(Models.Views.Vendor item)
        {
            return new Models.Entities.Vendor()
            {
                Id = item.Id,
                TypeCodeId = item.TypeCodeId,
                ShortNameTw = item.ShortNameTw,
                NameTw = item.NameTw,
                NameEn = item.NameEn,
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
                BillAddressZip = item.BillAddressZip,
                BillAddress = item.BillAddress,
                Contact = item.Contact,
                ContactMobileNo = item.ContactMobileNo,
                ContactEmail = item.ContactEmail,
                FirstTradeDate = item.FirstTradeDate,
                LastTradeDate = item.LastTradeDate,
                TaxCodeId = item.TaxCodeId,
                DollarCodeId = item.DollarCodeId,
                PaymentCodeId = item.PaymentCodeId,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
                PaymentPoint = item.PaymentPoint,
                CheckTitle = item.CheckTitle,
                CheckAddressZip = item.CheckAddressZip,
                CheckAddress = item.CheckAddress,
                DayOfMonth = item.DayOfMonth,
                CloseOff = item.CloseOff,
                QuoTaxIn = item.QuoTaxIn,
                IsTaxAdded = item.IsTaxAdded,
                TaxRate = item.TaxRate,
            };
        }
    }
}