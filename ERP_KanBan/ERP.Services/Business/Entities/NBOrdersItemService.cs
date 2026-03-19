using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP.Services.Business.Entities
{
    public class NBOrdersItemService : BusinessService
    {
        private Services.Entities.NBOrdersItemService NBOrdersItem { get; }
        public NBOrdersItemService(
            Services.Entities.NBOrdersItemService nbOrdersItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            NBOrdersItem = nbOrdersItemService;
        }
        public IQueryable<Models.Views.NBOrdersItem> Get()
        {
            return NBOrdersItem.Get().Select(item => new Models.Views.NBOrdersItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                OrderNo = item.CustomerPONo,
                Brand = item.Brand,
                Region = item.Region,
                Market = item.Market,
                VendorShortName = item.VendorShortName,
                ProductType = item.ProductType,
                ProductClass = item.ProductClass,
                PONo = item.PONo,
                POReferenceNo = item.POReferenceNo,
                CustomerOrderNo = item.CustomerOrderNo,
                POType = item.POType,
                LineNos = item.LineNos,
                StylePartNo = item.StylePartNo,
                StyleStatus = item.StyleStatus,
                ShipMode = item.ShipMode,
                ColorWidth = item.ColorWidth,
                Size = item.Size,
                SizeSuffix = item.SizeSuffix,
                Quantity = item.Quantity,
                FOBPrice = item.FOBPrice,
                Customer = item.Customer,
                Warehouse = item.Warehouse,
                OrigReqXFD = item.OrigReqXFD,
                OrigCFMXFD = item.OrigCFMXFD,
                POReleaseDate = item.POReleaseDate,
                ExpOrActXFD = item.ExpOrActXFD,
                ReasonCode = item.ReasonCode,
                StyleDescription = item.StyleDescription,
                Model = item.Model,
                Season = item.Season,
            });
        }

        public List<string> OrdersDate(int localeId){
            return NBOrdersItem.Get()
                .Where(i => i.LocaleId == localeId)
                .Select(i => i.POReleaseDate)
                .Distinct()
                .OrderByDescending(i => i)
                .ToList();
        }

        public ERP.Models.Views.NBOrdersItem Get(decimal id, decimal localeId)
        {
            return Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }

        public void CreateRange(List<ERP.Models.Views.NBOrdersItem> nbOrdersItem)
        {
            NBOrdersItem.CreateRange(BuildRange(nbOrdersItem));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.NBOrdersItem, bool>> predicate)
        {
            NBOrdersItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.NBOrdersItem> BuildRange(List<ERP.Models.Views.NBOrdersItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.NBOrdersItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CustomerPONo = item.OrderNo == null ? "" : item.OrderNo,
                Brand = item.Brand == null ? "" : item.Brand,
                Region = item.Region == null ? "" : item.Region,
                Market = item.Market == null ? "" : item.Market,
                VendorShortName = item.VendorShortName == null ? "" : item.VendorShortName,
                ProductType = item.ProductType == null ? "" : item.ProductType,
                ProductClass = item.ProductClass == null ? "" : item.ProductClass,
                PONo = item.PONo == null ? "" : item.PONo,
                POReferenceNo = item.POReferenceNo == null ? "" : item.POReferenceNo,
                CustomerOrderNo = item.CustomerOrderNo == null ? "" : item.CustomerOrderNo,
                POType = item.POType == null ? "" : item.POType,
                LineNos = item.LineNos == null ? "" : item.LineNos,
                StylePartNo = item.StylePartNo == null ? "" : item.StylePartNo,
                StyleStatus = item.StyleStatus == null ? "" : item.StyleStatus,
                ShipMode = item.ShipMode == null ? "" : item.ShipMode,
                ColorWidth = item.ColorWidth == null ? "" : item.ColorWidth,
                Size = item.Size == null ? "" : item.Size,
                SizeSuffix = item.SizeSuffix == null ? "" : item.SizeSuffix,
                Quantity = item.Quantity == null ? "" : item.Quantity,
                FOBPrice = item.FOBPrice == null ? "" : item.FOBPrice,
                Customer = item.Customer == null ? "" : item.Customer,
                Warehouse = item.Warehouse == null ? "" : item.Warehouse,
                OrigReqXFD = item.OrigReqXFD == null ? "" : item.OrigReqXFD,
                OrigCFMXFD = item.OrigCFMXFD == null ? "" : item.OrigCFMXFD,
                POReleaseDate = item.POReleaseDate == null ? "" : item.POReleaseDate,
                ExpOrActXFD = item.ExpOrActXFD == null ? "" : item.ExpOrActXFD,
                ReasonCode = item.ReasonCode == null ? "" : item.ReasonCode,
                StyleDescription = item.StyleDescription == null ? "" : item.StyleDescription,
                Model = item.Model == null ? "" : item.Model,
                Season = item.Season == null ? "" : item.Season,
            });
        }

    }
}