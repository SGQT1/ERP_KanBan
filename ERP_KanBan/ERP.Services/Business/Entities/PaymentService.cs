using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PaymentService : BusinessService
    {
        private Services.Entities.ShippingPaidLogService ShippingPaidLog { get; }
        private Services.Entities.ShippingService Shipping { get; }
        private Services.Entities.SaleService Sale { get; }
        private Services.Business.Entities.OrdersService Orders { get; }
        // private Services.Business.Entities.QuotationService Quotation { get; }

        public PaymentService(
            Services.Entities.ShippingPaidLogService shippingPaidLogService,
            Services.Entities.ShippingService shippingService,
            Services.Entities.SaleService saleService,
            Services.Business.Entities.OrdersService ordersService,
            // Services.Business.Entities.QuotationService quotationService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            ShippingPaidLog = shippingPaidLogService;
            Shipping = shippingService;
            Sale = saleService;
            Orders = ordersService;
            // Quotation = quotationService;
        }
        public IQueryable<Models.Views.Payment> Get()
        {
            return (
                from i in ShippingPaidLog.Get()
                join p in Shipping.Get() on new { InvoiceNo = i.InvoiceNo } equals new { InvoiceNo = p.InvoiceNo }
                join s in Sale.Get() on new { InvoiceId = p.Id } equals new { InvoiceId = s.ShippingId } into sGrp
                from s in sGrp.DefaultIfEmpty()
                select new Models.Views.Payment
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    InvoiceNo = i.InvoiceNo,
                    PaidDate = i.PaidDate,
                    PayDollarCodeDesc = i.PayDollarCodeDesc,
                    ARPaid = i.ARPaid,
                    AROff = i.AROff,
                    DiffDesc = i.DiffDesc,
                    IsCFM = i.IsCFM,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,

                    InvoiceId = p.Id,
                    CustomerId = s.CustomerId,
                    Customer = s.Customer,
                    ARTotal = p.ARTotal,
                    ARReceived = p.ARReceived,
                    // CompanyId = s.CompanyId,
                    // Company = s.CompanyNo,
                    Brand = s.BrandCode,
                    BrandId = s.BrandCodeId,
                    Confirmer = i.Confirmer,
                    ConfirmDate = i.ConfirmDate,
                }
            )
            .Distinct();
        }
        public IQueryable<Models.Views.Payment> Get(string predicate)
        {
            var payments = (
                from i in ShippingPaidLog.Get()
                join p in Shipping.Get() on new { InvoiceNo = i.InvoiceNo, LocaleId = i.LocaleId } equals new { InvoiceNo = p.InvoiceNo, LocaleId = p.LocaleId }
                join s in Sale.Get() on new { InvoiceId = p.Id, LocaleId = p.LocaleId } equals new { InvoiceId = s.ShippingId, LocaleId = s.LocaleId } into sGrp
                from s in sGrp.DefaultIfEmpty()
                select new
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    InvoiceNo = i.InvoiceNo,
                    PaidDate = i.PaidDate,
                    PayDollarCodeDesc = i.PayDollarCodeDesc,
                    ARPaid = i.ARPaid,
                    AROff = i.AROff,
                    DiffDesc = i.DiffDesc,
                    IsCFM = i.IsCFM,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,

                    InvoiceId = p.Id,
                    CustomerId = p.CustomerId,
                    Customer = p.CustomerNameTw,
                    ARTotal = p.ARTotal,
                    ARReceived = p.ARReceived,

                    CompanyId = s.CompanyId,
                    Company = s.CompanyNo,
                    Brand = s.BrandCode,
                    BrandId = s.BrandCodeId,
                    Confirmer = i.Confirmer,
                    ConfirmDate = i.ConfirmDate,
                })
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.Payment
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                InvoiceNo = i.InvoiceNo,
                PaidDate = i.PaidDate,
                PayDollarCodeDesc = i.PayDollarCodeDesc,
                ARPaid = i.ARPaid,
                AROff = i.AROff,
                DiffDesc = i.DiffDesc,
                IsCFM = i.IsCFM,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,

                InvoiceId = i.InvoiceId,
                CustomerId = i.CustomerId,
                Customer = i.Customer,
                // CompanyId = i.CompanyId,
                // Company = i.Company,
                BrandId = i.BrandId,
                Brand = i.Brand,
                ARTotal = i.ARTotal,
                ARReceived = i.ARReceived,
                Confirmer = i.Confirmer,
                ConfirmDate = i.ConfirmDate,
            })
            .Distinct();
            return payments;
        }
        public Models.Views.Payment Create(Models.Views.Payment item)
        {
            var _item = ShippingPaidLog.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void CreateRange(List<Models.Views.Payment> items)
        {
            ShippingPaidLog.CreateRange(BuildRange(items));
        }
        public Models.Views.Payment Update(Models.Views.Payment item)
        {
            var _item = ShippingPaidLog.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Payment item)
        {
            ShippingPaidLog.Remove(Build(item));
        }
        public void RemoveRange(List<Models.Views.Payment> items)
        {
            var ids = items.Select(i => i.Id);
            ShippingPaidLog.RemoveRange(i => ids.Contains(i.Id));
        }

        public void UpdateConfirm(int localeId, string confirmer, IEnumerable<decimal> confirmIds, IEnumerable<decimal> unConfirmIds)
        {
            // update shipiing Id = 0 ,remove shipping Id, umcloseed Only
            ShippingPaidLog.UpdateRange(
                i => confirmIds.Contains(i.Id) && i.LocaleId == localeId && i.IsCFM == 0,
                // u => new Models.Entities.ShippingPaidLog { IsCFM = 1, Confirmer = confirmer, ConfirmDate = DateTime.Now }
                u => u.SetProperty(p => p.IsCFM, v => 1).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );

            // updat shipping id = paymentId where Id in ShipmentId, closed only
            ShippingPaidLog.UpdateRange(
                i => unConfirmIds.Contains(i.Id) && i.LocaleId == localeId && i.IsCFM == 1,
                // u => new Models.Entities.ShippingPaidLog { IsCFM = 0, Confirmer = confirmer, ConfirmDate = DateTime.Now }
                u => u.SetProperty(p => p.IsCFM, v => 0).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );
        }
        //Calculate Payment amount by InvoiceNo
        public IQueryable<Models.Views.PaymentSummary> GetPaymentSummaries(IEnumerable<string> invoiceNos)
        {
            var invoices = (
                from invoice in Shipping.Get().Where(i => invoiceNos.Contains(i.InvoiceNo))
                join payment in ShippingPaidLog.Get() on new { InvoiceNo = invoice.InvoiceNo, LocaleId = invoice.LocaleId } equals new { InvoiceNo = payment.InvoiceNo, LocaleId = payment.LocaleId } into pGrp
                from payment in pGrp.DefaultIfEmpty()
                select new Models.Views.PaymentSummary
                {
                    LocaleId = invoice.LocaleId,
                    InvoiceNo = invoice.InvoiceNo,
                    LastPaidDate = payment.PaidDate,
                    ARPaidTotal = payment == null ? 0 : payment.ARPaid,
                    AROffTotal = payment == null ? 0 : payment.AROff,
                }
            ).ToList();

            var summary = invoices
                .GroupBy(i => new { i.InvoiceNo, i.LocaleId })
                .Select(g => new Models.Views.PaymentSummary
                {
                    LocaleId = g.Key.LocaleId,
                    InvoiceNo = g.Key.InvoiceNo,
                    LastPaidDate = g.Max(i => i.LastPaidDate),
                    ARPaidTotal = g.Sum(i => i.ARPaidTotal),
                    AROffTotal = g.Sum(i => i.AROffTotal),
                }).ToList();
            return summary.AsQueryable();
        }
        //for update, transfer view model to entity
        private Models.Entities.ShippingPaidLog Build(Models.Views.Payment item)
        {
            return new Models.Entities.ShippingPaidLog()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                InvoiceNo = item.InvoiceNo,
                PaidDate = item.PaidDate,
                PayDollarCodeDesc = item.PayDollarCodeDesc,
                ARPaid = item.ARPaid,
                AROff = item.AROff,
                DiffDesc = item.DiffDesc,
                IsCFM = item.IsCFM,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
                Confirmer = item.Confirmer,
                ConfirmDate = item.ConfirmDate,
            };
        }
        private IEnumerable<Models.Entities.ShippingPaidLog> BuildRange(List<Models.Views.Payment> items)
        {
            return items.Select(item => new Models.Entities.ShippingPaidLog()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                InvoiceNo = item.InvoiceNo,
                PaidDate = item.PaidDate,
                PayDollarCodeDesc = item.PayDollarCodeDesc,
                ARPaid = item.ARPaid,
                AROff = item.AROff,
                DiffDesc = item.DiffDesc,
                IsCFM = item.IsCFM,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
                Confirmer = item.Confirmer,
                ConfirmDate = item.ConfirmDate,
            });
        }
    }
}