using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class CompanyService : BusinessService
    {
        private Services.Entities.CompanyService Company { get; }

        public CompanyService(Services.Entities.CompanyService companyService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.Company = companyService;
        }
        public IQueryable<Models.Views.Company> Get()
        {
            return Company.Get().Select(i => new Models.Views.Company
            {
                Id = i.Id,
                CompanyNo = i.CompanyNo,
                ChineseName = i.ChineseName,
                EnglishName = i.EnglishName,
                ChineseShortName = i.ChineseShortName,
                EnglishShortName = i.EnglishShortName,
                TelNo = i.TelNo,
                FaxNo = i.FaxNo,
                ChineseAddress = i.ChineseAddress,
                EnglishAddress = i.EnglishAddress,
                OfficialAddress = i.OfficialAddress,
                UnifiedNo = i.UnifiedNo,
                TaxNo = i.TaxNo,
                Owner = i.Owner,
                InvoiceAddress = i.InvoiceAddress,
                InvoiceTitle = i.InvoiceTitle,
                WebsiteUrl = i.WebsiteURL,
                StockClosedMonth = i.StockClosedMonth,
                StockClosedDay = i.StockClosedDay,
                AccountClosedMonth = i.AccountClosedMonth,
                AccountClosedDay = i.AccountClosedDay,
                BusinessTaxRate = i.BusinessTaxRate,
                Remark = i.Remark,
                AccountDollarNameTw = i.AccountDollarNameTw,
                LastUpdateTime = i.LastUpdateTime,
                ModifyUserName = i.ModifyUserName,
                IgnoreSunday = i.IgnoreSunday,
                IgnoreHoliday = i.IgnoreHoliday,
                Contact = i.Contact,
                ContactMobileNo = i.ContactMobileNo,
                ContactEmail = i.ContactEmail,
                Enable = i.Enable,
                IsVirtual = i.IsVirtual,
            });
        }
    }
}