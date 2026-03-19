using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business
{
    public class InvoiceService : BusinessService
    {
        private ERP.Services.Business.Entities.InvoiceService Invoice;
        private ERP.Services.Business.Entities.InvoiceItemService InvoiceItem;
        private ERP.Services.Business.Entities.InvoiceSummaryService InvoiceSummary;    //精簡的出貨資料，之後可以刪除
        private ERP.Services.Business.Entities.PaymentService Payment;
        public InvoiceService(
            ERP.Services.Business.Entities.InvoiceService invoiceService,
            ERP.Services.Business.Entities.InvoiceItemService invoiceItemService,
            ERP.Services.Business.Entities.InvoiceSummaryService invoiceSummaryService,
            ERP.Services.Business.Entities.PaymentService paymentService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Invoice = invoiceService;
            InvoiceItem = invoiceItemService;
            InvoiceSummary = invoiceSummaryService;
            Payment = paymentService;
        }
        public ERP.Models.Views.InvoiceGroup GetInvoiceGroup(int id, int localeId)
        {
            var group = new InvoiceGroup { };
            // from shipping
            var invoice = Invoice.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (invoice != null)
            {
                group.Invoice = invoice;
                group.InvoiceItem = InvoiceItem.Get().Where(i => i.InvoiceId == id && i.LocaleId == localeId).ToList();
                group.Payment = Payment.Get().Where(i => i.InvoiceId == id && i.LocaleId == localeId).ToList();
            }
            return group;
        }
        public ERP.Models.Views.Invoice GetInvoice(string invoice, int localeId)
        {
            return Invoice.Get().Where(i => i.InvoiceNo == invoice && i.LocaleId == localeId).FirstOrDefault();
        }
        /*
         * Step1:InvoiceId=0 >> 新增，InvoiceId!=0 >> 更新
         * Step2:Invoice 有 Lock,表示會計已鎖定，資料庫欄位是ARId，鎖定後只能更新OBDate,DocumentDispatchDate,Remark
         * Step3:Invoice Item ，鎖定後不更新
         */
        public Models.Views.InvoiceGroup SaveInvoiceGroup(InvoiceGroup item)
        {
            var invoice = item.Invoice;
            var invoiceItems = item.InvoiceItem;
            try
            {
                UnitOfWork.BeginTransaction();
                if (invoice != null && invoiceItems.Count() > 0)
                {
                    // var _invoice = Invoice.Get().Where(i => i.InvoiceNo == invoice.InvoiceNo).FirstOrDefault();
                    // if(_invoice == null)
                    if (invoice.Id == 0)
                    {
                        invoice = Invoice.Create(invoice);
                    }
                    else
                    {
                        if(invoice.Lock == 0)
                        {
                            invoice = Invoice.Update(invoice);
                        }
                        else
                        {
                            invoice = Invoice.UpdateLock(invoice);
                        }
                    }

                    //ShipmentItem
                    if (invoice.Id != 0 && invoice.Lock == 0)
                    {
                        InvoiceItem.Update((int)invoice.Id,(int)invoice.Id, (int)invoice.LocaleId, invoiceItems);
                        InvoiceSummary.Update(invoice, invoiceItems);
                    }
                }
                UnitOfWork.Commit();
                return this.GetInvoiceGroup((int)invoice.Id, (int)invoice.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                Console.WriteLine("SaveInvoiceGroup:"+ e);
                throw e;
            }
        }
        public void RemoveInvoiceGroup(InvoiceGroup item)
        {
            var invoice = item.Invoice;
            var invoiceItems = item.InvoiceItem;

            try
            {
                UnitOfWork.BeginTransaction();
                InvoiceItem.Update((int)item.Invoice.Id, 0, (int)item.Invoice.LocaleId, item.InvoiceItem);
                InvoiceSummary.Remove(invoice, invoiceItems);

                Invoice.Remove(item.Invoice);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                Console.WriteLine("SaveInvoiceGroup:"+ e);
                throw e;
            }
        }
        public IEnumerable<ERP.Models.Views.OrdersShipment> CloseInvoice(IEnumerable<ERP.Models.Views.OrdersShipment> shipments)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (shipments != null || shipments.Count() > 0)
                {
                    var confirmer = shipments.Select(i => i.Confirmer).FirstOrDefault();
                    var resquestLocaleId = shipments.Select(i => i.RequestLocaleId).FirstOrDefault();
                    var confirmIds = shipments.Where(i => i.IsCFM == true).Select(i => i.InvoiceId).Distinct();
                    var unConfirmIds = shipments.Where(i => i.IsCFM == false).Select(i => i.InvoiceId).Distinct();
                    Invoice.UpdateConfirm((int) resquestLocaleId, confirmer, confirmIds, unConfirmIds);
                    InvoiceItem.UpdateConfirm((int) resquestLocaleId, confirmer, confirmIds, unConfirmIds);
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
            return shipments;
        } 
    }
}