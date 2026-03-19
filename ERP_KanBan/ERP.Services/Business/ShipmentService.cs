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
    public class ShipmentService : BusinessService
    {
        private ERP.Services.Business.Entities.ShipmentService Shipment;
        private ERP.Services.Business.Entities.ShipmentItemService ShipmentItem;
        private ERP.Services.Business.Entities.OrdersService Orders;
        private ERP.Services.Business.Entities.OrdersItemService OrdersItem;
        private ERP.Services.Business.Entities.QuotationService Quotation;

        public ShipmentService(
            ERP.Services.Business.Entities.ShipmentService shipmentService,
            ERP.Services.Business.Entities.ShipmentItemService shipmentItemService,
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemService ordersItemService,
            ERP.Services.Business.Entities.QuotationService quotationService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Shipment = shipmentService;
            ShipmentItem = shipmentItemService;
            Orders = ordersService;
            Quotation = quotationService;
            OrdersItem = ordersItemService;
        }
        public ERP.Models.Views.ShipmentGroup GetShipmentGroup(int id, int localeId)
        {
            var group = new ShipmentGroup { };
            // from Sale
            var shipment = Shipment.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (shipment != null)
            {
                group.Shipment = shipment;
                // from SaleItem          
                group.ShipmentItem = ShipmentItem.Get().Where(i => i.SaleId == shipment.Id && i.LocaleId == shipment.LocaleId).ToList();
                
                // from Orders
                var orders = Orders.Get(i => i.Id == shipment.OrdersId && i.LocaleId == shipment.RefLocaleId).FirstOrDefault();
                group.Orders = orders == null ? new Orders() : orders;
                // from SaleItem Tooling
                group.ShipmentSummary = orders == null ? new ShipmentSummary() : GetShipmentSummary(group.Orders);
                // from Quotation
                group.Quotation = orders == null ? new List<Quotation>() : GetQuotations(group.Orders, 0);
            }
            return group;
        }
        /*
         * build ShipmentGroup ,not into database
         */
        public ERP.Models.Views.ShipmentGroup BuildShipmentGroup(string orderNo, int localeId, int shipmentType)
        {
            var group = new ShipmentGroup { };
            var order = Orders.Get(i => i.OrderNo.CompareTo(orderNo) == 0 && i.ARLocaleId == localeId).FirstOrDefault();
            if (order != null)
            {
                //順序
                group.Orders = order;
                group.Shipment = Shipment.GetByOrder(order);
                group.Quotation = GetQuotations(order, shipmentType);
                group.ShipmentItem = GetShipmentSize(order);
                group.ShipmentSummary = GetShipmentSummary(order);

                if (group.Quotation.Any())
                {
                    //fill quotation rule
                    var quotation = group.Quotation.Where(i => i.CompanyId == order.CompanyId).Any() ?
                        group.Quotation.Where(i => i.CompanyId == order.CompanyId).OrderByDescending(i => i.EffectiveDate).FirstOrDefault() :
                        group.Quotation.OrderByDescending(i => i.EffectiveDate).FirstOrDefault();

                    //update price to Shipment
                    group.Shipment.SaleDate = DateTime.Now;
                    group.Shipment.QuotationId = quotation.Id;
                    group.Shipment.OutsolePrice = quotation.OutsolePrice;
                    group.Shipment.MidsolePrice = quotation.MidsolePrice;
                    group.Shipment.ToolingOtherPrice = quotation.ToolingOtherPrice;
                    group.Shipment.ToolingTotalPrice = quotation.ToolingTotalPrice;
                    group.Shipment.ToolFundIntel = quotation.ToolFundIntel;
                    group.Shipment.FactoryPrice = quotation.FactoryPriceIntel;
                    group.Shipment.InvoicePrice = quotation.InvoicePriceIntel;
                    group.Shipment.EffectiveDate = quotation.EffectiveDate;

                    // update tc/tf to Shipment Items
                    var items = group.ShipmentItem.ToList();
                    items.ForEach(i =>
                    {
                        i.UnitPrice = (decimal)quotation.InvoicePriceIntel;
                        i.ToolingCost = quotation.ToolingTotalPrice;
                        i.ToolingFund = quotation.ToolFundIntel;
                    });
                    //update price, tooling
                    group.ShipmentItem = items;
                }

            }
            return group;
        }
        public Models.Views.ShipmentGroup SaveShipmentGroup(ShipmentGroup item)
        {
            var shipment = item.Shipment;
            var shipmentItems = item.ShipmentItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (shipment != null && shipmentItems.Count() > 0)
                {
                    if (shipment.Id == 0)   
                    {
                        shipment = Shipment.Create(shipment);
                    }
                    else
                    {
                        shipment = Shipment.Update(shipment);
                    }

                    //ShipmentItem
                    if (shipment.Id != 0)
                    {
                        shipmentItems.ForEach(i => { i.SaleId = shipment.Id; });
                        ShipmentItem.RemoveRange((int)shipment.Id, (int)shipment.LocaleId);
                        ShipmentItem.CreateRange(shipmentItems);
                    }
                }
                UnitOfWork.Commit();
                return this.GetShipmentGroup((int)shipment.Id, (int)shipment.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void RemoveShipmentGroup(ShipmentGroup item)
        {
            try
            {
                UnitOfWork.BeginTransaction();
                ShipmentItem.RemoveRange((int)item.Shipment.Id, (int)item.Shipment.LocaleId);
                Shipment.Remove(item.Shipment);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        private Models.Views.ShipmentSummary GetShipmentSummary(Orders item)
        {
            var summary = new ShipmentSummary();
            if(item != null ) {
                var shipmentsByArticle = Shipment.Get().Where(i => i.RefArticleNo == item.ArticleNo && i.RefLocaleId == item.LocaleId).ToList();
                var shipmentsByOrders = Shipment.Get().Where(i => i.OrdersId == item.Id && i.RefLocaleId == item.LocaleId).ToList();

                summary.OrderQty = item.OrderQty;
                summary.ShipmentQtyTotal = shipmentsByOrders.Sum(i => i.SaleQty);
                summary.ArticleToolingCost = shipmentsByArticle.Sum(i => i.ToolingCost);
                summary.ArticleToolingFund = shipmentsByArticle.Sum(i => i.Discount);
                summary.StyleToolingCost = shipmentsByArticle.Where(i => i.RefStyleNo == item.RefStyleNo).Sum(i => i.ToolingCost);
                summary.StyleToolingFund = shipmentsByArticle.Where(i => i.RefStyleNo == item.RefStyleNo).Sum(i => i.Discount);
            }

            return summary;
        }
        /*
         * find quotation by styleNo of Order, if has value then retrun else find by ArticleNo of   
         */
        private List<Models.Views.Quotation> GetQuotations(Orders item, int shipmentType)
        {
            //find by style
            var quotionsByStyle = Quotation.Get(i =>
                       (i.StyleNo.Length > 0 && i.StyleNo == item.StyleNo) &&
                       i.EffectiveDate <= item.OrderDate &&
                       i.Confirmed == 1 &&
                       i.ProductTypeId == item.ProductType
                    )
                    .OrderByDescending(i => i.EffectiveDate)
                    .ToList();

            if (quotionsByStyle.Count() > 0)
            {
                // shipmentType: 0 = All, 1 = Export, 2 = Domestic,  
                if (shipmentType != 0)
                {
                    return quotionsByStyle.Where(i => i.ShipmentTypeId == shipmentType).ToList();
                }
                return quotionsByStyle;
            }
            
            //find by article
            var quotionsByArticle = Quotation.Get(i =>
                       i.ArticleNo == item.ArticleNo &&
                       i.StyleNo.Trim().Length == 0 &&
                       i.EffectiveDate <= item.OrderDate &&
                       i.Confirmed == 1 &&
                       i.ProductTypeId == item.ProductType
                    )
                    .OrderBy(i => i.StyleNo)
                    .ThenByDescending(i => i.EffectiveDate)
                    .ToList();
            
            // shipmentType: 0 = All, 1 = Export, 2 = Domestic,  
            if (shipmentType != 0)
            {
                return quotionsByStyle.Where(i => i.ShipmentTypeId == shipmentType).ToList();
            }
            return quotionsByArticle;
        }
        private List<Models.Views.ShipmentItem> GetShipmentSize(Orders item)
        {
            return OrdersItem.Get()
                .Where(i => i.OrdersId == item.Id && i.LocaleId == item.LocaleId)
                .Select(i => new Models.Views.ShipmentItem
                {
                    OrdersItemId = i.Id,
                    ArticleSize = i.ArticleSize,
                    ArticleSizeSuffix = i.ArticleSizeSuffix,
                    ArticleInnerSize = i.ArticleInnerSize,
                    DisplaySize = i.DisplaySize,
                    SaleQty = i.Qty,
                    // UnitPrice = si.UnitPrice,
                    // ToolingFund = si.ToolingFund,
                    // ToolingCost = si.ToolingCost,
                })
                .ToList();
        }
    }
}