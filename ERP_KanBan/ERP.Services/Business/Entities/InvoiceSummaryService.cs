using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class InvoiceSummaryService : BusinessService
    {
        private Services.Entities.SimpleSaleService SimpleSale { get; } //精簡的出貨資料，之後可以刪除
        public InvoiceSummaryService(
            Services.Entities.SimpleSaleService simpleService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            SimpleSale = simpleService;
        }
        
        public void Update(Models.Views.Invoice invoice, IEnumerable<Models.Views.InvoiceItem> items)
        {
            var shipmentIds = items.Select(i => i.Id).ToList();
            // SimpleSale.RemoveRange(i => shipmentIds.Contains(i.SaleId) && i.LocaleId == invoice.LocaleId);
            SimpleSale.RemoveRange(i => i.ShippingId == invoice.Id && i.LocaleId == invoice.LocaleId);
            SimpleSale.CreateRange(BuildRange(invoice, items));
        }

        public void Remove(Models.Views.Invoice invoice, IEnumerable<Models.Views.InvoiceItem> items)
        {
            var shipmentIds = items.Select(i => i.Id).ToList();
            // SimpleSale.RemoveRange(i => shipmentIds.Contains(i.SaleId) && i.LocaleId == invoice.LocaleId);
            SimpleSale.RemoveRange(i => i.ShippingId == invoice.Id && i.LocaleId == invoice.LocaleId);
        }

        private IQueryable<Models.Views.InvoiceSummary> Get()
        {
            return SimpleSale.Get().Select(i => new InvoiceSummary {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrdersId = i.OrdersId,
                RefLocaleId = i.RefLocaleId,
                SaleId = i.SaleId,
                SaleDate = i.SaleDate,
                CloseDate = i.CloseDate,
                OBDate = i.OBDate,
                PaymentDate = i.PaymentDate,
                SaleQty = i.SaleQty,
                ShippingId = i.ShippingId,
                InvoiceNo = i.InvoiceNo,
                ARCustomerNameTw = i.ARCustomerNameTw,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }

        //for update, transfer view model to entity
        private IEnumerable<Models.Entities.SimpleSale> BuildRange(Models.Views.Invoice invoice, IEnumerable<Models.Views.InvoiceItem> items)
        {
            return items.Select(i => new Models.Entities.SimpleSale {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrdersId = i.OrdersId,
                RefLocaleId = (decimal)i.RefLocaleId,
                SaleId = i.Id,
                SaleDate = i.SaleDate,
                CloseDate = i.CloseDate,
                OBDate = invoice.OBDate,//i.ARDate,
                PaymentDate = null,
                SaleQty = i.SaleQty,
                ShippingId = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                ARCustomerNameTw = invoice.Customer,
                ModifyUserName = "N_"+i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
    }
}