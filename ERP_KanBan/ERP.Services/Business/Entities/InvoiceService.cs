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
    public class InvoiceService : BusinessService
    {
        private Services.Entities.ShippingService Shipping { get; }
        private Services.Entities.SaleService Sale { get; }
        private Services.Entities.ShippingPaidLogService ShippingPaidLog { get; }
        private Services.Entities.OrdersService Orders { get; }
        private Services.Business.Entities.TypeService Type { get; }
        public InvoiceService(
            Services.Entities.ShippingService shippingService,
            Services.Entities.SaleService saleService,
            Services.Entities.ShippingPaidLogService shippingPaidLogService,
            Services.Entities.OrdersService ordersService,
            Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Shipping = shippingService;
            Sale = saleService;
            ShippingPaidLog = shippingPaidLogService;
            Orders = ordersService;
            Type = typeService;
        }
        public IQueryable<Models.Views.Invoice> Get()
        {
            return (
                from i in Shipping.Get()
                join s in Sale.Get() on new { InvoiceId = i.Id, LocaleId = i.LocaleId } equals new { InvoiceId = s.ShippingId, LocaleId = s.LocaleId } into sGrp
                from s in sGrp.DefaultIfEmpty()
                select new Models.Views.Invoice
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    InvoiceNo = i.InvoiceNo,
                    InvoiceDate = i.InvoiceDate,
                    CustomerId = i.CustomerId,
                    Customer = i.CustomerNameTw,
                    OBDate = i.OBDate,
                    ExportQty = i.ExportQty,
                    PayDollarCodeId = i.PayDollarCodeId,
                    PayDollarCodeDesc = i.PayDollarCodeDesc,
                    OtherCost = i.OtherCost,
                    ARTotal = i.ARTotal,
                    ARId = ((int)i.ARId == 1 || (int)s.ARId == 1) ? 1 : 0,
                    Remark = i.Remark,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,

                    // ARReceived = i.ARReceived,
                    ARReceived = ShippingPaidLog.Get().Where(p => p.InvoiceNo == i.InvoiceNo && p.LocaleId == i.LocaleId).Sum(i => i.AROff) == null ? 0 :
                                 ShippingPaidLog.Get().Where(p => p.InvoiceNo == i.InvoiceNo && p.LocaleId == i.LocaleId).Sum(i => i.AROff),
                    Lock = ((int)i.ARId == 1 || (int)s.ARId == 1) ? 1 : 0,
                    DocumentDispatchDate = i.DocumentDispatchDate,
                    PaymentDueDate = i.PaymentDueDate,
                    CHODate = i.CHODate,

                    CompanyId = s.CompanyId,
                    Company = s.CompanyNo,
                    BrandId = s.BrandCodeId,
                    Brand = s.BrandCode,
                    Confirmer = i.Confirmer,
                    ConfirmDate = i.ConfirmDate,
                }
            ).Distinct();
        }
        public IQueryable<Models.Views.Invoice> Get(string predicate)
        {
            var invoice = (
                from i in Shipping.Get()
                join s in Sale.Get() on new { InvoiceId = i.Id, LocaleId = i.LocaleId } equals new { InvoiceId = s.ShippingId, LocaleId = s.LocaleId } into sGrp
                from s in sGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId } equals new { OrdersId = o.Id, LocaleId = (decimal?)o.LocaleId } into oGrp
                from o in oGrp.DefaultIfEmpty()
                select new
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    InvoiceNo = i.InvoiceNo,
                    InvoiceDate = i.InvoiceDate,
                    CustomerId = i.CustomerId,
                    CustomerNameTw = i.CustomerNameTw,
                    OBDate = i.OBDate,
                    ArrivalDate = i.ArrivalDate,
                    ExportQty = i.ExportQty,
                    PayType = i.PayType,
                    PayTypeDesc = i.PayTypeDesc,
                    PayDollarCodeId = i.PayDollarCodeId,
                    PayDollarCodeDesc = i.PayDollarCodeDesc,
                    OtherCost = i.OtherCost,
                    ARTotal = i.ARTotal,
                    ARId = i.ARId,
                    Remark = i.Remark,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    ARReceivedDate = i.ARReceivedDate,
                    TransitType = i.TransitType,
                    ARReceived = i.ARReceived,
                    Lock = (int)i.ARId,
                    DeliveryDate = i.DeliveryDate,
                    DocumentDispatchDate = i.DocumentDispatchDate,
                    PaymentDueDate = i.PaymentDueDate,
                    CHODate = i.CHODate,

                    CompanyId = s.CompanyId,
                    Company = s.CompanyNo,
                    BrandId = s.BrandCodeId,
                    Brand = s.BrandCode,
                    OrderNo = s.OrderNo,
                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    Confirmer = i.Confirmer,
                    ConfirmDate = i.ConfirmDate,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new Models.Views.Invoice
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    InvoiceNo = i.InvoiceNo,
                    InvoiceDate = i.InvoiceDate,
                    CustomerId = i.CustomerId,
                    Customer = i.CustomerNameTw,
                    OBDate = i.OBDate,
                    ExportQty = i.ExportQty,
                    PayDollarCodeId = i.PayDollarCodeId,
                    PayDollarCodeDesc = i.PayDollarCodeDesc,
                    OtherCost = i.OtherCost,
                    ARTotal = i.ARTotal,
                    ARId = i.ARId,
                    Remark = i.Remark,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    ARReceived = i.ARReceived,
                    Lock = i.Lock,
                    DocumentDispatchDate = i.DocumentDispatchDate,
                    PaymentDueDate = i.PaymentDueDate,
                    CHODate = i.CHODate,
                    Confirmer = i.Confirmer,
                    ConfirmDate = i.ConfirmDate,
                    // ArrivalDate = i.ArrivalDate,
                    // PayType = i.PayType,
                    // PayTypeDesc = i.PayTypeDesc,
                    // ARReceivedDate = i.ARReceivedDate,
                    // TransitType = i.TransitType,
                    // DeliveryDate = i.DeliveryDate,
                    // CompanyId = i.CompanyId,
                    // Company = i.Company,
                    // BrandId = i.BrandId,
                    // Brand = i.Brand
                })
                .Distinct();
            return invoice;
        }
        public Models.Views.Invoice Create(Models.Views.Invoice item)
        {
            var _item = Shipping.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Invoice Update(Models.Views.Invoice item)
        {
            var _item = Shipping.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Invoice item)
        {
            Shipping.Remove(Build(item));
        }

        // lock by shipment
        public void UpdateConfirm(int localeId, string confirmer, IEnumerable<decimal> confirmIds, IEnumerable<decimal> unConfirmIds)
        {
            // update Sale Id = 0 ,remove shipping Id, umcloseed Only
            Shipping.UpdateRange(
                i => confirmIds.Contains(i.Id) && i.LocaleId == localeId,
                u => u.SetProperty(p => p.ARId, v => 1).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );
            Shipping.UpdateRange(
                i => unConfirmIds.Contains(i.Id) && i.LocaleId == localeId,
                u => u.SetProperty(p => p.ARId, v => 0).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );
        }
        // public void UpdateConfirm1(int localeId, string confirmer, IEnumerable<decimal> confirmIds, IEnumerable<decimal> unConfirmIds)
        // {
        //     // update Sale Id = 0 ,remove shipping Id, umcloseed Only
        //     Shipping.UpdateRange(
        //         i => confirmIds.Contains(i.Id) && i.LocaleId == localeId,
        //         u => new Models.Entities.Shipping { ARId = 1, Confirmer = confirmer, ConfirmDate = DateTime.Now }
        //     );

        //     // updat shipping id = paymentId where Id in ShipmentId, closed only
        //     Shipping.UpdateRange(
        //         i => unConfirmIds.Contains(i.Id) && i.LocaleId == localeId,
        //         u => new Models.Entities.Shipping { ARId = 0, Confirmer = confirmer, ConfirmDate = DateTime.Now }
        //     );
        // }
        // update payment amount on invocie
        public void UpdatePayments(List<Models.Views.PaymentSummary> payments)
        {
            payments.ForEach(i =>
            {
                Shipping.UpdateRange(
                    s => s.InvoiceNo == i.InvoiceNo && s.LocaleId == i.LocaleId,
                    // s => new Models.Entities.Shipping { ARReceived = i.AROffTotal, ARReceivedDate = i.LastPaidDate }
                    setters => setters.SetProperty(p => p.ARReceived, v => i.AROffTotal).SetProperty(p => p.ARReceivedDate, v => i.LastPaidDate)
                );
            });
        }
        // after lock to update data(OBDate)
        public Models.Views.Invoice UpdateLock(Models.Views.Invoice item)
        {
            Shipping.UpdateRange(
                s => s.Id == item.Id && s.LocaleId == item.LocaleId,
                // s => new Models.Entities.Shipping { OBDate = item.OBDate, DocumentDispatchDate = item.DocumentDispatchDate, Remark = item.Remark }
                setters => setters.SetProperty(p => p.OBDate, v => item.OBDate).SetProperty(p => p.DocumentDispatchDate, v => item.DocumentDispatchDate).SetProperty(p => p.Remark, v => item.Remark)
            );
            return Get().Where(i => i.Id == item.Id && i.LocaleId == item.LocaleId).FirstOrDefault();
        }
        public IQueryable<Models.Views.OrdersShipment> GetOrderShipment(string predicate)
        {
            var payLog = ShippingPaidLog.Get().GroupBy(i => new { i.InvoiceNo, i.LocaleId }).Select(i => new { InvoiceNo =i.Key.InvoiceNo, LocaleId = i.Key.LocaleId, PaidDate = i.Max(g => g.PaidDate) });
            var orders = (
                from o in Orders.Get()
                join s in Sale.Get() on new { OrdersId = o.Id, LocaleId = (decimal?)o.LocaleId } equals new { OrdersId = s.OrdersId, LocaleId = s.RefLocaleId } into sGrp
                from s in sGrp.DefaultIfEmpty()
                join i in Shipping.Get() on new { Invoice = s.ShippingId, LocaleId = s.LocaleId } equals new { Invoice = i.Id, LocaleId = i.LocaleId } into iGrp
                from i in iGrp.DefaultIfEmpty()
                join p in payLog on new { InvoiceNo = i.InvoiceNo, LocaleId = i.LocaleId } equals new { InvoiceNo = p.InvoiceNo, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    OrderNo = o.OrderNo,
                    Customer = o.Customer,
                    CustomerOrderNo = o.CustomerOrderNo,
                    GBSPOReferenceNo = o.GBSPOReferenceNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    Brand = o.Brand,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    OrderQty = o.OrderQty,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OPD = o.OWD,
                    OrderDate = o.OrderDate,
                    OWD = o.OWD,
                    OWRD = o.OWRD,
                    RSD = o.RSD,
                    Season = o.Season,
                    LastNo = o.LastNo,
                    OutsoleNo = o.OutsoleNo,
                    ProductTypeId = o.ProductType,
                    TransitTypeId = o.TransitType,

                    AvgPrice = (decimal?)s.Price ?? 0,
                    ShippingQty = (decimal?)s.SaleQty ?? 0,
                    // ShortageQty = (decimal?)(o.OrderQty - s.SaleQty),
                    // SubTotal = (decimal?)(s.Amount + s.FeedbackFund + s.OtherCharge + s.LessCharge + s.CLB),
                    ShippingAmount = (decimal?)s.Amount ?? 0,
                    LessCharge = (decimal?)s.LessCharge ?? 0,
                    OtherCharge = (decimal?)s.OtherCharge ?? 0,
                    FeedbackFund = (decimal?)s.FeedbackFund ?? 0,
                    ToolingCost = (decimal?)s.ToolingCost ?? 0,
                    Discount = (decimal?)s.Discount ?? 0,
                    OutsolePrice = (decimal?)s.OutsolePrice ?? 0,
                    MidsolePrice = (decimal?)s.MidsolePrice ?? 0,
                    ToolingOtherPrice = (decimal?)s.ToolingOtherPrice ?? 0,
                    ToolingTotalPrice = (decimal?)s.ToolingTotalPrice ?? 0,
                    CLB = (decimal?)s.CLB ?? 0,
                    ShippingDate = (DateTime?)s.SaleDate,
                    // ShippingMonth = (string?)s.SaleDate.ToString("yyyy-MM"),

                    CurrencyId = (decimal?)i.PayDollarCodeId ?? 0,
                    Currency = (string?)i.PayDollarCodeDesc ?? "",
                    // ARSubTotal = (s != null ? (decimal)(s.Amount + s.FeedbackFund + s.OtherCharge + s.LessCharge + s.CLB) : 0) +
                    //  ((i != null && s != null && i.OtherCost != 0 && i.OtherCost != 0 && s.SaleQty != 0) ? i.OtherCost / i.ExportQty * s.SaleQty : 0),
                    ARTotal = (decimal?)i.ARTotal ?? 0,
                    ARReceived = (decimal?)i.ARReceived ?? 0,
                    InvoiceNo = (string?)i.InvoiceNo ?? "",
                    InvoiceDate = (DateTime?)i.InvoiceDate,
                    OBDate = (DateTime?)i.OBDate,
                    ShippingOtherCost = (decimal?)i.OtherCost ?? 0,
                    ExportQty = (decimal?)i.ExportQty ?? 0,
                    // OtherCost = (i != null && s != null && i.OtherCost != 0 && i.OtherCost != 0 && s.SaleQty != 0) ? i.OtherCost / i.ExportQty * s.SaleQty : 0,
                    InvoiceRemark = (string?)i.Remark ?? "",
                    // IsCFM = i != null && i.ARId == 1 ? true : false,
                    ARId = (decimal?)i.ARId ?? 0,
                    RequestLocaleId = (decimal?)i.LocaleId ?? 0,
                    ShipmentId = (decimal?)s.Id ?? 0,
                    InvoiceId = (decimal?)i.Id ?? 0,
                    Confirmer = (string?)i.Confirmer ?? "",
                    ConfirmDate = (DateTime?)i.ConfirmDate,
                    ModifyUserName = (string?)i.ModifyUserName ?? "",
                    LastUpdateTime = (DateTime?)i.LastUpdateTime,
                    // PaidDate = ShippingPaidLog.Get().Where(p => p.InvoiceNo == i.InvoiceNo && p.LocaleId == i.LocaleId).OrderByDescending(p => p.PaidDate).Select(p => (DateTime?)p.PaidDate).FirstOrDefault(),
                    PaidDate = (DateTime?)p.PaidDate ?? null,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.OrdersShipment
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrderNo = i.OrderNo,
                Customer = i.Customer,
                CustomerOrderNo = i.CustomerOrderNo,
                GBSPOReferenceNo = i.GBSPOReferenceNo,
                CompanyId = i.CompanyId,
                Company = i.CompanyNo,
                Brand = i.Brand,
                ArticleNo = i.ArticleNo,
                StyleNo = i.StyleNo,
                ShoeName = i.ShoeName,
                OrderQty = i.OrderQty,
                CSD = i.CSD,
                LCSD = i.LCSD,
                OPD = i.OWD,
                OrderDate = i.OrderDate,
                OWD = i.OWD,
                OWRD = i.OWRD,
                RSD = i.RSD,
                Season = i.Season,
                Last = i.LastNo,
                Outsole = i.OutsoleNo,
                ProductTypeId = i.ProductTypeId,
                TransitTypeId = i.TransitTypeId,


                AvgPrice = i.AvgPrice,
                ShippingQty = i.ShippingQty,
                ShortageQty = (i.OrderQty - i.ShippingQty),
                ShippingAmount = i.ShippingAmount,
                LessCharge = i.LessCharge,
                OtherCharge = i.OtherCharge,
                FeedbackFund = i.FeedbackFund,
                ToolingCost = i.ToolingCost,
                CLB = i.CLB,
                SubTotal = (i.ShippingAmount + i.FeedbackFund + i.OtherCharge + i.LessCharge + i.CLB),

                Discount = i.Discount,
                OutsolePrice = i.OutsolePrice,
                MidsolePrice = i.MidsolePrice,
                ToolingOtherPrice = i.ToolingOtherPrice,
                ToolingTotalPrice = i.ToolingTotalPrice,

                ShippingDate = i.ShippingDate,
                // ShippingMonth = i.ShippingDate?.ToString("yyyy-MM") ?? "",
                CurrencyId = i.CurrencyId,
                Currency = i.Currency,
                ARTotal = i.ARTotal,
                ARReceived = i.ARReceived,
                InvoiceNo = i.InvoiceNo ?? "",
                InvoiceDate = i.InvoiceDate,
                OBDate = i.OBDate,
                // OtherCost = (i.ShippingOtherCost / i.ExportQty * i.ShippingQty),
                // ARSubTotal = (i.ShippingAmount + i.FeedbackFund + i.OtherCharge + i.LessCharge + i.CLB) + (i.ShippingOtherCost / i.ExportQty * i.ShippingQty),
                OtherCost = (i.ShippingOtherCost > 0 && i.ExportQty > 0 && i.ShippingQty > 0) ? i.ShippingOtherCost / i.ExportQty * i.ShippingQty : 0,
                ARSubTotal = ((i.ShippingAmount + i.FeedbackFund + i.OtherCharge + i.LessCharge + i.CLB)) + ((i.ShippingOtherCost > 0 && i.ExportQty > 0 && i.ShippingQty > 0) ? i.ShippingOtherCost / i.ExportQty * i.ShippingQty : 0),
                InvoiceRemark = i.InvoiceRemark,
                IsCFM = i.ARId == 1 ? true : false,
                ARId = i != null ? i.ARId : 0,
                RequestLocaleId = i.RequestLocaleId,
                ShipmentId = i.ShipmentId,
                InvoiceId = i.InvoiceId,
                Confirmer = i.Confirmer,
                ConfirmDate = i.ConfirmDate,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                PaidDate = i.PaidDate,
                // PaidDate = i.InvoiceNo != null ? ShippingPaidLog.Get().Where(p => p.InvoiceNo == i.InvoiceNo && p.LocaleId == i.LocaleId).OrderByDescending(p => p.PaidDate).Select(p => (DateTime?)p.PaidDate).FirstOrDefault() : null,
            })
            .ToList();

            orders.ForEach(o =>
            {   
                o.ShippingMonth = o.ShippingDate != null ? ((DateTime)o.ShippingDate).ToString("yyyy-MM") : "";
                o.ProductType = Type.GetProductType().Where(t => t.Id == o.ProductTypeId).Select(t => t.NameTw).Max();
                o.TransitType = Type.GetTransitType().Where(t => t.Id == o.TransitTypeId).Select(t => t.NameTw).Max();
            });
            return orders.AsQueryable();
        }
        //for update, transfer view model to entity
        private Models.Entities.Shipping Build(Models.Views.Invoice item)
        {
            return new Models.Entities.Shipping()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                InvoiceNo = item.InvoiceNo,
                InvoiceDate = item.InvoiceDate,
                CustomerId = item.CustomerId,
                CustomerNameTw = item.Customer,
                OBDate = item.OBDate,

                ExportQty = item.ExportQty,
                PayDollarCodeId = item.PayDollarCodeId,
                PayDollarCodeDesc = item.PayDollarCodeDesc,
                OtherCost = item.OtherCost,
                ARTotal = item.ARTotal,
                ARId = item.ARId,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,

                ARReceived = item.ARReceived,
                // Lock = item.Lock,
                DocumentDispatchDate = item.DocumentDispatchDate,
                PaymentDueDate = item.PaymentDueDate,
                CHODate = item.CHODate,
                Confirmer = item.Confirmer,
                ConfirmDate = item.ConfirmDate
                // ArrivalDate = i.ArrivalDate,
                // PayType = 0,
                // PayTypeDesc = "",
                // ARReceivedDate = i.ARReceivedDate,
                // TransitType = i.TransitType,
                // DeliveryDate = i.DeliveryDate,
                // ShippingPortId = 0,
                // ShippingPortName = "",
                // TargetPortId = 0,
                // TargetPortName = "",
            };
        }
    }
}