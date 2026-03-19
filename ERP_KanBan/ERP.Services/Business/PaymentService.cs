using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class PaymentService : BusinessService
    {
        private ERP.Services.Business.Entities.PaymentService Payment { get; set; }
        private ERP.Services.Business.Entities.InvoiceService Invoice { get; set; }
        public PaymentService(
            ERP.Services.Business.Entities.PaymentService paymentService,
            ERP.Services.Business.Entities.InvoiceService invoiceService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Payment = paymentService;
            Invoice = invoiceService;
        }

        public ERP.Models.Views.Payment Get(int id, int localeId)
        {
            return Payment.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.Payment BuildPayment(string invoiceNo, int localeId)
        {
            var invoice = Invoice.Get().Where(i => i.InvoiceNo == invoiceNo && i.LocaleId == localeId).FirstOrDefault();

            if (invoice != null)
            {
                // realtime update payment aroff, because payment not equals with invoice
                // var iARReceived = Payment.Get().Where( p => p.InvoiceNo == invoice.InvoiceNo && p.LocaleId == invoice.LocaleId).Sum(i => i.AROff);
            
                return new ERP.Models.Views.Payment
                {
                    LocaleId = localeId,
                    InvoiceNo = invoiceNo,
                    PayDollarCodeDesc = invoice.PayDollarCodeDesc,
                    ARPaid = (invoice.ARTotal - (decimal)invoice.ARReceived) < 0 ?0 : (invoice.ARTotal - (decimal)invoice.ARReceived),// ARPaid, AROff is AR Balance
                    AROff = (invoice.ARTotal - (decimal)invoice.ARReceived) < 0 ?0 : (invoice.ARTotal - (decimal)invoice.ARReceived),// ARPaid, AROff is AR Balance
                    // ARPaid = (invoice.ARTotal - (decimal)iARReceived) < 0 ?0 : (invoice.ARTotal - (decimal)iARReceived),// ARPaid, AROff is AR Balance
                    // AROff = (invoice.ARTotal - (decimal)iARReceived) < 0 ?0 : (invoice.ARTotal - (decimal)iARReceived),// ARPaid, AROff is AR Balance
                    IsCFM = 0,

                    InvoiceId = invoice.Id,
                    CustomerId = invoice.CustomerId,
                    Customer = invoice.Customer,
                    CompanyId = invoice.CompanyId,
                    Company = invoice.Company,
                    Brand = invoice.Brand,
                    BrandId = invoice.BrandId,
                    ARTotal = invoice.ARTotal,
                    ARReceived = invoice.ARReceived
                    // ARReceived = iARReceived
                };
            }
            return new ERP.Models.Views.Payment { };
        }
        public ERP.Models.Views.Payment Save(ERP.Models.Views.Payment item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var _item = Payment.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id && i.InvoiceNo == item.InvoiceNo).FirstOrDefault();
                if (_item != null)
                {
                    item = Payment.Update(item);
                }
                else
                {
                    item = Payment.Create(item);
                }
                UnitOfWork.Commit();
                var invoiceNos = new List<string>();
                invoiceNos.Add(item.InvoiceNo);
                UpdateInvoice(invoiceNos);
                return Get((int)item.Id, (int)item.LocaleId);
            }
            catch
            {
                UnitOfWork.Rollback();
                return item;
            }
        }
        public List<ERP.Models.Views.Payment> SaveBatch(List<ERP.Models.Views.Payment> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var invoice = items.Select(i => i.InvoiceNo);
                Payment.CreateRange(items);
                UnitOfWork.Commit();
                
                UpdateInvoice(invoice);

                return Payment.Get().Where(i => invoice.Contains(i.InvoiceNo)).ToList();
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
        }
        public void Remove(ERP.Models.Views.Payment item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Payment.Remove(item);
                UnitOfWork.Commit();
                
                var invoiceNos = new List<string>();
                invoiceNos.Add(item.InvoiceNo);
                UpdateInvoice(invoiceNos);
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }
        public void RemoveBatch(List<ERP.Models.Views.Payment> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Payment.RemoveRange(items);
                UnitOfWork.Commit();
                UpdateInvoice(items.Select(i => i.InvoiceNo));
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }

        public IEnumerable<ERP.Models.Views.Payment> ClosePayment(IEnumerable<ERP.Models.Views.Payment> payments)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (payments != null || payments.Count() > 0)
                {
                    var confirmer = payments.Select(i => i.Confirmer).FirstOrDefault();
                    var localeId = payments.Select(i => i.LocaleId).FirstOrDefault();
                    var confirmIds = payments.Where(i => i.IsCFM == 1).Select(i => i.Id);
                    var unConfirmIds = payments.Where(i => i.IsCFM == 0).Select(i => i.Id);
                    Payment.UpdateConfirm((int) localeId, confirmer, confirmIds, unConfirmIds);
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
            return payments;
        }
        private void UpdateInvoice(IEnumerable<string> invoices)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Invoice.UpdatePayments(Payment.GetPaymentSummaries(invoices).ToList());
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }
    }
}