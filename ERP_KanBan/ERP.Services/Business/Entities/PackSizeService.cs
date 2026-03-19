using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackSizeService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.OrdersItemService OrdersItem { get; set; }
        // private ERP.Services.Business.Entities.SizeMappingItemService SizeCountryMapping { get; }
        private ERP.Services.Business.PackArticleService PackArticle { get; set; }

        public PackSizeService(
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemService ordersItemService,
            // ERP.Services.Business.Entities.SizeMappingItemService sizeCountryMappingService,
            ERP.Services.Business.PackArticleService packArticleService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersItem = ordersItemService;
            // SizeCountryMapping = sizeCountryMappingService;
            PackArticle = packArticleService;
        }
        public IEnumerable<Models.Views.PackSizeItem> Get(int ordersId, int localeId)
        {
            var orders = Orders.Get().Where(i => i.Id == ordersId && i.LocaleId == localeId).FirstOrDefault();
            var ordersItem = OrdersItem.GetSizeMapping(ordersId, localeId);
            if (orders != null && ordersItem.Count() > 0)
            {
                var packArticle = PackArticle.GetPackArticleGroup((int)orders.ArticleId, localeId);
                var packArticleItem = packArticle.PackArticleItem == null ? new List<Models.Views.PackArticleItem>() : packArticle.PackArticleItem.ToList();

                var packSizeItem = (
                    from oi in ordersItem
                    join pa in packArticleItem on new { oi.ArticleInnerSize } equals new { pa.ArticleInnerSize } into paGRP
                    from pa in paGRP.DefaultIfEmpty()
                    select new Models.Views.PackSizeItem
                    {
                        Id = oi.Id,
                        LocaleId = oi.LocaleId,
                        OrdersId = (decimal)oi.OrdersId,
                        OrdersNo = orders.OrderNo,
                        Qty = oi.Qty,
                        AvailableQty = oi.Qty,
                        AdjQty = 0,
                        CustomerOrderNo = orders.CustomerOrderNo,
                        RefCustomer = orders.Customer,
                        StyleNo = orders.StyleNo,
                        CSD = orders.CSD,
                        ArticleSize = (oi.ArticleSize.ToString("0.0") + oi.ArticleSizeSuffix).Trim(),
                        OrderSize = (oi.OrderSize.ToString("0.0") + oi.OrderSizeSuffix).Trim(),
                        ItemInnerSize = oi.ArticleInnerSize,
                        PairsOfCTN = pa == null ? 10 : pa.PairsOfCTN,
                        GWOfCTN = pa == null ? 0 : pa.GWOfCTN,
                        NWOfCTN = pa == null ? 0 : pa.NWOfCTN,
                        MEAS = pa == null ? 0 : pa.MEAS,
                        CTNSpec = pa == null ? "" : pa.CTNSpec,
                        CTNL = pa == null ? "" : pa.CTNL,
                        CTNW = pa == null ? "" : pa.CTNW,
                        CTNH = pa == null ? "" : pa.CTNH,
                        BOXSpec = pa == null ? "" : pa.BOXSpec,
                        BOXL = pa == null ? "" : pa.BOXL,
                        BOXW = pa == null ? "" : pa.BOXW,
                        BOXH = pa == null ? "" : pa.BOXH,
                        RefLocalId = (decimal)orders.ARLocaleId,
                        SizeCountryNameTw = orders.ArticleSizeCountryCode,
                        MappingSizeCountryNameTw = orders.OrderSizeCountryCode
                    }
                );
                return packSizeItem;
            }
            else
            {
                return new List<Models.Views.PackSizeItem>();
            }
        }
        public IEnumerable<Models.Views.PackSizeItem> Get(string ordersNo, int localeId)
        {
            // var ordersId = Orders.Get().Where(i => i.OrderNo.CompareTo(ordersNo) == 0 && i.LocaleId == localeId).Select(i => i.Id).FirstOrDefault();
            var order = Orders.Get().Where(i => i.OrderNo == ordersNo && i.LocaleId == localeId).FirstOrDefault();
            if (order == null)
            {
                return new List<Models.Views.PackSizeItem>();
            }
            return Get((int)order.Id, (int)order.LocaleId);
        }
    }
}