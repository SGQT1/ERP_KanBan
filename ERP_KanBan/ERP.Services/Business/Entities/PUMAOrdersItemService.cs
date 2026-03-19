using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;

using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Business.Entities
{
    public class PUMAOrdersItemService : BusinessService
    {
        private Services.Entities.PUMAOrdersItemService PUMAOrdersItem { get; }
        public PUMAOrdersItemService(
            Services.Entities.PUMAOrdersItemService pumaOrdersItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            PUMAOrdersItem = pumaOrdersItemService;
        }
        public IQueryable<Models.Views.PUMAOrdersItem> Get()
        {
            return PUMAOrdersItem.Get().Select(i => new Models.Views.PUMAOrdersItem
            {
                Id = i.Id,
                    LocaleId = i.LocaleId,
                    OrderNo = i.OrderNo,
                    CustomerPONo = i.CustomerPONo,
                    CustomerCode = i.CustomerCode,
                    CustomerName = i.CustomerName,
                    CustomerCONo = i.CustomerCONo,
                    UCustomerCode = i.UCustomerCode,
                    UCustomerName = i.UCustomerName,
                    UCustomerCONo = i.UCustomerCONo,
                    OrderReleaseDate = i.OrderReleaseDate,
                    TotalQty = i.TotalQty,
                    StyleNo = i.StyleNo,
                    StyleDesc = i.StyleDesc,
                    Color = i.Color,
                    ColorDesc = i.ColorDesc,
                    CustomerStyleNo = i.CustomerStyleNo,
                    CustomerDesc = i.CustomerDesc,
                    RHD = i.RHD,
                    EHD = i.EHD,
                    CHD = i.CHD,
                    LCHD = i.LCHD,
                    LastUpdate = i.LastUpdate,
                    SupplierCode = i.SupplierCode,
                    FactoryCode = i.FactoryCode,
                    CounryOrigin = i.CounryOrigin,
                    Brand = i.Brand,
                    ULTCustomerDSNo = i.ULTCustomerDSNo,
                    CreateDate = i.CreateDate,
                    Currency = i.Currency,
                    HarmonizedSystemNo = i.HarmonizedSystemNo,
                    NotHarmonizedSystemNo = i.NotHarmonizedSystemNo,
                    DeliveryAddress = i.DeliveryAddress,
                    ShipmentMode = i.ShipmentMode,
                    PaymentMethod = i.PaymentMethod,
                    Status = i.Status,
                    Carrier = i.Carrier,
                    OrderChar = i.OrderChar,
                    SizeTableName = i.SizeTableName,
                    KeySize = i.KeySize,
                    Size = i.Size,
                    Quantity = i.Quantity,
                    Sur = i.Sur,
                    Sku = i.Sku,
                    Ocp = i.Ocp,
                    DeliveryGroup = i.DeliveryGroup,
                    DestinationName = i.DestinationName,
                    VasRemarks = i.VasRemarks,
                    PitRemarks = i.PitRemarks,
                    Season = i.Season,
                    OPD = i.OPD
            });
        }

        public List<string> OrdersDate(int localeId)
        {
            return PUMAOrdersItem.Get()
                .Where(i => i.LocaleId == localeId &&
                    i.OrderReleaseDate.CompareTo("2015-01-01") >= 0)
                .Select(i => i.OrderReleaseDate)
                .Distinct()
                .OrderByDescending(i => i)
                .ToList();
        }

        public ERP.Models.Views.PUMAOrdersItem Get(decimal id, decimal localeId)
        {
            return Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public void CreateRange(List<Models.Views.PUMAOrdersItem> pumaOrdersItem)
        {
            PUMAOrdersItem.CreateRange(BuildRange(pumaOrdersItem));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.PUMAOrdersItem, bool>> predicate)
        {
            PUMAOrdersItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.PUMAOrdersItem> BuildRange(List<Models.Views.PUMAOrdersItem> ordersItems)
        {
            return ordersItems.Select(item => new ERP.Models.Entities.PUMAOrdersItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                OrderNo = item.OrderNo == null ? "" : item.OrderNo,
                CustomerPONo = item.CustomerPONo == null ? "" : item.CustomerPONo,
                CustomerCode = item.CustomerCode == null ? "" : item.CustomerCode,
                CustomerName = item.CustomerName == null ? "" : item.CustomerName,
                CustomerCONo = item.CustomerCONo == null ? "" : item.CustomerCONo,
                UCustomerCode = item.UCustomerCode == null ? "" : item.UCustomerCode,
                UCustomerName = item.UCustomerName == null ? "" : item.UCustomerName,
                UCustomerCONo = item.UCustomerCONo == null ? "" : item.UCustomerCONo,
                OrderReleaseDate = item.OrderReleaseDate == null ? "" : item.OrderReleaseDate,
                TotalQty = item.TotalQty == null ? "" : item.TotalQty,
                StyleNo = item.StyleNo == null ? "" : item.StyleNo,
                StyleDesc = item.StyleDesc == null ? "" : item.StyleDesc,
                Color = item.Color == null ? "" : item.Color,
                ColorDesc = item.ColorDesc == null ? "" : item.ColorDesc,
                CustomerStyleNo = item.CustomerStyleNo == null ? "" : item.CustomerStyleNo,
                CustomerDesc = item.CustomerDesc == null ? "" : item.CustomerDesc,
                RHD = item.RHD == null ? "" : item.RHD,
                EHD = item.EHD == null ? "" : item.EHD,
                CHD = item.CHD == null ? "" : item.CHD,
                LCHD = item.LCHD == null ? "" : item.LCHD,
                LastUpdate = item.LastUpdate == null ? "" : item.LastUpdate,
                SupplierCode = item.SupplierCode == null ? "" : item.SupplierCode,
                FactoryCode = item.FactoryCode == null ? "" : item.FactoryCode,
                CounryOrigin = item.CounryOrigin == null ? "" : item.CounryOrigin,
                Brand = item.Brand == null ? "" : item.Brand,
                ULTCustomerDSNo = item.ULTCustomerDSNo == null ? "" : item.ULTCustomerDSNo,
                CreateDate = item.CreateDate == null ? "" : item.CreateDate,
                Currency = item.Currency == null ? "" : item.Currency,
                HarmonizedSystemNo = item.HarmonizedSystemNo == null ? "" : item.HarmonizedSystemNo,
                NotHarmonizedSystemNo = item.NotHarmonizedSystemNo == null ? "" : item.NotHarmonizedSystemNo,
                DeliveryAddress = item.DeliveryAddress == null ? "" : item.DeliveryAddress,
                ShipmentMode = item.ShipmentMode == null ? "" : item.ShipmentMode,
                PaymentMethod = item.PaymentMethod == null ? "" : item.PaymentMethod,
                Status = item.Status == null ? "" : item.Status,
                Carrier = item.Carrier == null ? "" : item.Carrier,
                OrderChar = item.OrderChar == null ? "" : item.OrderChar,
                SizeTableName = item.SizeTableName == null ? "" : item.SizeTableName,
                KeySize = item.KeySize == null ? "" : item.KeySize,
                Size = item.Size == null ? "" : item.Size,
                Quantity = item.Quantity == null ? "" : item.Quantity,
                Sur = item.Sur == null ? "" : item.Sur,
                Sku = item.Sku == null ? "" : item.Sku,
                Ocp = item.Ocp == null ? "" : item.Ocp,
                DeliveryGroup = item.DeliveryGroup == null ? "" : item.DeliveryGroup,
                DestinationName = item.DestinationName == null ? "" : item.DestinationName,
                VasRemarks = item.VasRemarks == null ? "" : item.VasRemarks,
                PitRemarks = item.PitRemarks == null ? "" : item.PitRemarks,
                Season = item.Season == null ? "" : item.Season,
                OPD = item.OPD == null ? "" : item.OPD,
                
            }) ;
        }
    }
}