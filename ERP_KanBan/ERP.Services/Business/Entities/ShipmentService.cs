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
    public class ShipmentService : BusinessService
    {
        private Services.Entities.SaleService Sale { get; }
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.CustomerService Customer { get; }
        public ShipmentService(
            Services.Entities.SaleService saleService,
            Services.Entities.OrdersService ordersService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.CustomerService customerService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Sale = saleService;
            Orders = ordersService;
            CodeItem = codeItemService;
            Customer = customerService;
        }
        public IQueryable<Models.Views.Shipment> Get()
        {
            return (
                from i in Sale.Get()
                join o in Orders.Get() on new { OrdersId = i.OrdersId, LocaleId = i.RefLocaleId } equals new { OrdersId = o.Id, LocaleId = (decimal?)o.LocaleId }
                join d in CodeItem.Get() on new { DollarCodeId = i.DollarCodeId, LocaleId = i.LocaleId } equals new { DollarCodeId = d.Id, LocaleId = d.LocaleId } into dGrp
                from d in dGrp.DefaultIfEmpty()
                select new Models.Views.Shipment
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    SaleDate = i.SaleDate,
                    OrdersId = i.OrdersId,
                    SaleQty = i.SaleQty,
                    DollarCodeId = i.DollarCodeId,
                    Amount = i.Amount,
                    Discount = i.Discount,
                    ToolingCost = i.ToolingCost,
                    OtherCharge = i.OtherCharge,
                    OtherChargeDesc = i.OtherChargeDesc,
                    IsCFM = i.ARId == 0 ? false : true,
                    ARId = i.ARId,
                    ARDate = i.ARDate,
                    CloseDate = i.CloseDate,
                    FeedbackFund = i.FeedbackFund,
                    InvoiceId = i.ShippingId,
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
                    CustomerId = i.CustomerId,
                    Customer = i.Customer,
                    OrderNo = i.OrderNo,
                    Brand = i.BrandCode,
                    BrandId = i.BrandCodeId,
                    CompanyNo = i.CompanyNo,
                    CompanyId = i.CompanyId,
                    Price = i.Price,
                    QuotationId = i.QuotationId,
                    OutsolePrice = i.OutsolePrice,
                    MidsolePrice = i.MidsolePrice,
                    ToolingOtherPrice = i.ToolingOtherPrice,
                    ToolingTotalPrice = i.ToolingTotalPrice,
                    ToolFundIntel = i.ToolFundIntel,
                    FactoryPrice = i.FactoryPrice,
                    InvoicePrice = i.InvoicePrice,
                    EffectiveDate = i.EffectiveDate,

                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    RefCurrency = d.NameTW,
                    RefCurrencyId = o.DollarCodeId,
                    RefProductTypeId = o.ProductType,
                    RefArticleId = o.ArticleId,
                    RefArticleNo = o.ArticleNo,
                    RefStyleId = o.StyleId,
                    RefStyleNo = o.StyleNo,

                    Confirmer = i.Confirmer,
                    ConfirmDate = i.ConfirmDate,
                }
            );
        }

        /*
         * get default Shipment
         */
        public Models.Views.Shipment GetByOrder(Models.Views.Orders order)
        {
            var customer = Customer.Get().Where(i => i.ChineseName == order.Customer && i.LocaleId == order.ARLocaleId).FirstOrDefault();
            var brand = CodeItem.Get().Where(i => i.NameTW == order.Brand && i.LocaleId == order.ARLocaleId && i.CodeType == "25").FirstOrDefault();
            var currency = CodeItem.Get().Where(i => i.NameTW == order.Dollar && i.LocaleId == order.ARLocaleId && i.CodeType == "02").FirstOrDefault();
            return new Models.Views.Shipment
            {
                OrdersId = order.Id,
                ARId = 0,
                // ARDate = null,
                RefLocaleId = order.LocaleId,
                Season = order.Season,
                SaleDate = DateTime.Now,
                CloseDate = DateTime.Now,
                OrderNo = order.OrderNo,
                CompanyNo = order.CompanyNo,
                CompanyId = order.CompanyId,
                Customer = order.Customer,
                CustomerId = customer != null ? customer.Id : 0,
                Brand = order.Brand,
                BrandId = brand != null ? brand.Id : 0,
                RefProductTypeId = order.ProductType,
                RefArticleId = order.ArticleId,
                RefArticleNo = order.ArticleNo,
                RefStyleId = order.StyleId,
                RefStyleNo = order.StyleNo,
                CustomerOrderNo = order.CustomerOrderNo,
                GBSPOReferenceNo = order.GBSPOReferenceNo,
                DollarCodeId = currency != null ? currency.Id : 0,
            };
        }
        public Models.Views.Shipment Create(Models.Views.Shipment item)
        {
            var _item = Sale.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Shipment Update(Models.Views.Shipment item)
        {
            var _item = Sale.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }

        // Lock for account (ARId = 0)
        public void UpdateConfirm(int localeId, string confirmer, IEnumerable<decimal> confirmIds, IEnumerable<decimal> unConfirmIds)
        {
            Sale.UpdateRange(
                i => confirmIds.Contains(i.ShippingId) && i.LocaleId == localeId,
                u => u.SetProperty(p => p.ARId, v => 1).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );
            Sale.UpdateRange(
                i => unConfirmIds.Contains(i.ShippingId) && i.LocaleId == localeId,
                u => u.SetProperty(p => p.ARId, v => 0).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );
        }

        // Lock for account (ARId = 0)
        // public void UpdateConfirm1(int localeId, string confirmer, IEnumerable<decimal> confirmIds, IEnumerable<decimal> unConfirmIds)
        // {
        //     // update Sale Id = 0 ,remove shipping Id, umcloseed Only
        //     Sale.UpdateRange(
        //         i => confirmIds.Contains(i.ShippingId) && i.LocaleId == localeId,
        //         u => new Models.Entities.Sale { ARId = 1, Confirmer = confirmer, ConfirmDate = DateTime.Now }
        //     );

        //     // updat shipping id = paymentId where Id in ShipmentId, closed only
        //     Sale.UpdateRange(
        //         i => unConfirmIds.Contains(i.ShippingId) && i.LocaleId == localeId,
        //         u => new Models.Entities.Sale { ARId = 0, Confirmer = confirmer, ConfirmDate = DateTime.Now }
        //     );
        // }

        public void UpdateInvoice(int invoiceId, int newInvoiceId, int localeId, IEnumerable<decimal> shimpmentIds)
        {
            // update shipiing Id = 0 ,remove shipping Id
            Sale.UpdateRange(
                i => i.ShippingId == invoiceId && i.LocaleId == localeId,
                // i => new Models.Entities.Sale { ShippingId = 0, ARId = 0, ARDate = null }
                u => u.SetProperty(p => p.ShippingId, v => 0).SetProperty(p => p.ARId, v => 0).SetProperty(p => p.ARDate, v => null)
            );

            // updat shipping id = paymentId where Id in ShipmentId
            Sale.UpdateRange(
                i => shimpmentIds.Contains(i.Id) && i.LocaleId == localeId,
                // i => new Models.Entities.Sale { ShippingId = newInvoiceId }
                u => u.SetProperty(p => p.ShippingId, v => newInvoiceId)
            );
        }
        public void Remove(Models.Views.Shipment item)
        {
            Sale.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Sale Build(Models.Views.Shipment item)
        {
            return new Models.Entities.Sale()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                SaleDate = item.SaleDate,
                OrdersId = item.OrdersId,
                SaleQty = item.SaleQty,
                DollarCodeId = item.DollarCodeId,
                Amount = item.Amount,
                Discount = item.Discount,           // TF
                ToolingCost = item.ToolingCost,     // TC
                OtherCharge = item.OtherCharge,     //OtherCharge 1
                OtherChargeDesc = item.OtherChargeDesc,
                // ARId = item.ARId,
                ARId = item.IsCFM == true ? 1 : 0,
                ARDate = item.ARDate,
                CloseDate = item.CloseDate,
                FeedbackFund = item.FeedbackFund, //ChargeAdj
                ShippingId = item.InvoiceId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
                RefLocaleId = item.RefLocaleId,
                OrdersSubId = item.OrdersSubId,
                LessCharge = item.LessCharge,       // 0.05%
                CLB = item.CLB,                     //OtherCharge 2       

                Season = item.Season,
                IsPriceBySeason = item.IsPriceBySeason,
                CustomerId = item.CustomerId,
                Customer = item.Customer,
                OrderNo = item.OrderNo,
                BrandCode = item.Brand,
                BrandCodeId = item.BrandId,
                CompanyNo = item.CompanyNo,
                CompanyId = item.CompanyId,
                Price = item.Price,
                QuotationId = item.QuotationId,
                OutsolePrice = item.OutsolePrice,
                MidsolePrice = item.MidsolePrice,
                ToolingOtherPrice = item.ToolingOtherPrice,
                ToolingTotalPrice = item.ToolingTotalPrice,
                ToolFundIntel = item.ToolFundIntel,
                FactoryPrice = item.FactoryPrice,
                InvoicePrice = item.InvoicePrice,
                EffectiveDate = item.EffectiveDate,
                Confirmer = item.Confirmer,
                ConfirmDate = item.ConfirmDate,
                // APTFundF1Id = item.APTFundF1Id,
                // APTFundVId = item.APTFundVId,
                // APTFundId = item.APTFundId,
            };
        }
    }
}