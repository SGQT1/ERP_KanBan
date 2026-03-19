using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class CustomerService : BusinessService
    {
        private Services.Entities.CustomerService Customer { get; }
        private Services.Entities.OrdersService Orders { get; set; }

        public CustomerService(
            Services.Entities.CustomerService customerService, 
            Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Customer = customerService;
            Orders = ordersService;
        }
        public IQueryable<Models.Views.Customer> Get()
        {
            return Customer.Get().Select(i => new Models.Views.Customer
            {
                Id = i.Id,
                ChineseName = i.ChineseName,
                EnglishName = i.EnglishName,
                ChineseShortName = i.ChineseShortName,
                OwnerName = i.OwnerName,
                TelNo1 = i.TelNo1,
                TelNo2 = i.TelNo2,
                FaxNo1 = i.FaxNo1,
                FaxNo2 = i.FaxNo2,
                CountryCodeId = i.CountryCodeId,
                AreaCodeId = i.AreaCodeId,
                BrandCodeId = i.BrandCodeId,
                DollarCodeId = i.DollarCodeId,
                UnifiedInvoiceNo = i.UnifiedInvoiceNo,
                CompanyAddressZip = i.CompanyAddressZip,
                CompanyAddress = i.CompanyAddress,
                InvoiceAddressZip = i.InvoiceAddressZip,
                InvoiceAddress = i.InvoiceAddress,
                Contact = i.Contact,
                ContactMobileNo = i.ContactMobileNo,
                ContactEmail = i.ContactEmail,
                CreditAmount = i.CreditAmount,
                CreditOverRate = i.CreditOverRate,
                FirstTradeDate = i.FirstTradeDate,
                LastTradeDate = i.LastTradeDate,
                InvoiceCodeId = i.InvoiceCodeId,
                TaxCodeId = i.TaxCodeId,
                PaymentCodeId = i.PaymentCodeId,
                PaymentDays = i.PaymentDays,
                PackingDescTW = i.PackingDescTW,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                Mark = i.Mark,
                SideMark = i.SideMark,
                SafeCode = i.SafeCode,
                BarcodeCodeId = i.BarcodeCodeId,
                PriceType = i.PriceType,
                ExportPortId = i.ExportPortId,
                PackingDescEng = i.PackingDescEng,
                LocaleId = i.LocaleId,
                DefaultPhotoURL1 = i.DefaultPhotoURL1,
                DefaultPhotoURL2 = i.DefaultPhotoURL2,
                SpecialNoteTw = i.DefaultPhotoURL3,
                SpecialNoteEn = i.DefaultPhotoURL4,
                PayType = i.PayType,
                DeliveryTerms = i.DeliveryTerms,
                DayOfMonth = i.DayOfMonth,
                ShipmentType = i.ShipmentType,
                TaxRate = i.TaxRate,
                IsTaxAdded = i.IsTaxAdded,
                IsSpecial = i.IsSpecial,
                HasOrders = Orders.Get().Where(o => o.CustomerId == i.Id && o.LocaleId == i.LocaleId).Count()
            });
        }
        public Models.Views.Customer Create(Models.Views.Customer item)
        {
            var _item = Customer.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Customer Update(Models.Views.Customer item)
        {
            var _item = Customer.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Customer item)
        {
            Customer.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Customer Build(Models.Views.Customer item)
        {
            return new Models.Entities.Customer()
            {
                Id = item.Id,
                ChineseName = item.ChineseName,
                EnglishName = item.EnglishName,
                ChineseShortName = item.ChineseShortName,
                OwnerName = item.OwnerName,
                TelNo1 = item.TelNo1,
                TelNo2 = item.TelNo2,
                FaxNo1 = item.FaxNo1,
                FaxNo2 = item.FaxNo2,
                CountryCodeId = item.CountryCodeId,
                AreaCodeId = item.AreaCodeId,
                BrandCodeId = item.BrandCodeId,
                DollarCodeId = item.DollarCodeId,
                UnifiedInvoiceNo = item.UnifiedInvoiceNo,
                CompanyAddressZip = item.CompanyAddressZip,
                CompanyAddress = item.CompanyAddress,
                InvoiceAddressZip = item.InvoiceAddressZip,
                InvoiceAddress = item.InvoiceAddress,
                Contact = item.Contact,
                ContactMobileNo = item.ContactMobileNo,
                ContactEmail = item.ContactEmail,
                CreditAmount = item.CreditAmount,
                CreditOverRate = item.CreditOverRate,
                FirstTradeDate = item.FirstTradeDate,
                LastTradeDate = item.LastTradeDate,
                InvoiceCodeId = item.InvoiceCodeId,
                TaxCodeId = item.TaxCodeId,
                PaymentCodeId = item.PaymentCodeId,
                PaymentDays = item.PaymentDays,
                PackingDescTW = item.PackingDescTW,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
                Mark = item.Mark,
                SideMark = item.SideMark,
                SafeCode = item.SafeCode,
                BarcodeCodeId = item.BarcodeCodeId,
                PriceType = item.PriceType,
                ExportPortId = item.ExportPortId,
                PackingDescEng = item.PackingDescEng,
                LocaleId = item.LocaleId,
                DefaultPhotoURL1 = item.DefaultPhotoURL1,
                DefaultPhotoURL2 = item.DefaultPhotoURL2,
                DefaultPhotoURL3 = item.SpecialNoteTw,
                DefaultPhotoURL4 = item.SpecialNoteEn,
                PayType = item.PayType,
                DeliveryTerms = item.DeliveryTerms,
                DayOfMonth = item.DayOfMonth,
                ShipmentType = item.ShipmentType,
                TaxRate = item.TaxRate,
                IsTaxAdded = item.IsTaxAdded,
                IsSpecial = item.IsSpecial
            };
        }
    }
}