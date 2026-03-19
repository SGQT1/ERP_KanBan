using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class InvoiceItemService : BusinessService
    {
        private Services.Business.Entities.ShipmentService Shipment { get; }
        public InvoiceItemService(
            Services.Business.Entities.ShipmentService shipmentService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Shipment = shipmentService;
        }
        public IQueryable<Models.Views.InvoiceItem> Get()
        {
            return Shipment.Get().Select(i => new Models.Views.InvoiceItem
            {
                Id = i.Id,
                InvoiceId = i.InvoiceId,
                LocaleId = i.LocaleId,
                SaleDate = i.SaleDate,
                OrdersId = i.OrdersId,
                SaleQty = i.SaleQty,
                DollarCodeId = i.DollarCodeId,
                Discount = i.Discount,
                ToolingCost = i.ToolingCost,
                OtherCharge = i.OtherCharge,
                OtherChargeDesc = i.OtherChargeDesc,
                ARId = i.ARId,
                ARDate = i.ARDate,
                CloseDate = i.CloseDate,
                FeedbackFund = i.FeedbackFund,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                APTFundId = i.APTFundId,
                RefLocaleId = i.RefLocaleId,
                OrdersSubId = i.OrdersSubId,
                LessCharge = i.LessCharge,
                CLB = i.CLB,
                APTFundF1Id = i.APTFundF1Id,
                APTFundVId = i.APTFundVId,
                Season = i.Season,
                IsPriceBySeason = i.IsPriceBySeason,
                Customer = i.Customer,
                CustomerId = i.CustomerId,
                OrderNo = i.OrderNo,
                CustomerOrderNo = i.CustomerOrderNo,
                GBSPOReferenceNo = i.GBSPOReferenceNo,
                Currency = i.RefCurrency,
                CurrencyId = i.RefCurrencyId,
                Brand = i.Brand,
                BrandId = i.BrandId,
                Company = i.CompanyNo,
                CompanyId = i.CompanyId,
                ProductTypeId = i.RefProductTypeId,
                ArticleId = i.RefArticleId,
                ArticleNo = i.RefArticleNo,
                StyleId = i.RefStyleId,
                StyleNo = i.RefStyleNo,
                Amount = i.Amount,
                SubTotal = (decimal)(i.Amount + i.FeedbackFund + i.OtherCharge + i.LessCharge + i.CLB),
                // SubTotal = i.Amount,
                AvgPrice = i.SaleQty == 0 ? 0 : (decimal)(i.Amount + i.FeedbackFund + i.OtherCharge + i.LessCharge + i.CLB) / i.SaleQty,
                Confirmer = i.Confirmer,
                ConfirmDate = i.ConfirmDate,
            });
        }

        public void Update(int invoiceId, int newInvoiceId, int localeId, IEnumerable<Models.Views.InvoiceItem> items)
        {
            var shipmentIds = items.Select(i => i.Id).ToList();
            Shipment.UpdateInvoice(invoiceId, newInvoiceId, localeId, shipmentIds);
        }

        public void UpdateConfirm(int localeId, string confirmer, IEnumerable<decimal> confirmIds, IEnumerable<decimal> unConfirmIds)
        {
            Shipment.UpdateConfirm(localeId, confirmer, confirmIds, unConfirmIds);
        }

    }
}