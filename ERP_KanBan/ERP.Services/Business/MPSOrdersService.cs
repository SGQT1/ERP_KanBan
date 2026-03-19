using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSOrdersService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSOrdersService MPSOrders { get; set; }
        private ERP.Services.Business.Entities.MPSOrdersItemService MPSOrdersItem { get; set; }

        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.OrdersItemService OrdersItem { get; set; }
        private ERP.Services.Entities.PCLService PCL { get; set; }

        private ERP.Services.Business.Entities.MPSArticleService MPSArticle { get; set; }
        private ERP.Services.Entities.ArticleSizeRunService ArticleSize { get; set; }
        private ERP.Services.Business.CacheService Cache { get; set; }

        private readonly IServiceScopeFactory _ServiceScopeFactory;
        public MPSOrdersService(
            ERP.Services.Business.Entities.MPSOrdersService mpsOrdersService,
            ERP.Services.Business.Entities.MPSOrdersItemService mpsOrdersItemService,
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemService ordersItemService,
            ERP.Services.Entities.PCLService pclService,
            ERP.Services.Business.Entities.MPSArticleService mpsArticleService,
            ERP.Services.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Business.CacheService cacheService,
            IServiceScopeFactory serviceScopeFactory,  // DI加這行
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSOrders = mpsOrdersService;
            MPSOrdersItem = mpsOrdersItemService;
            Orders = ordersService;
            OrdersItem = ordersItemService;
            PCL = pclService;
            MPSArticle = mpsArticleService; ;
            ArticleSize = articleSizeRunService;
            Cache = cacheService;

            _ServiceScopeFactory = serviceScopeFactory;
        }

        public IQueryable<Models.Views.OrdersForMPS> GetOrdersForMPS()
        {
            var result = (
                from o in Orders.Get()
                join m in MPSOrders.Get() on new { OrdersNo = o.OrderNo, LocaleId = o.LocaleId } equals new { OrdersNo = m.OrderNo, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join ma in MPSArticle.Get() on new { ArticleNo = o.ArticleNo, LocaleId = o.LocaleId } equals new { ArticleNo = ma.ArticleNo, LocaleId = ma.LocaleId } into maGRP
                from ma in maGRP.DefaultIfEmpty()
                select new Models.Views.OrdersForMPS
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    StyleNo = o.StyleNo,
                    ETD = o.ETD,
                    CSD = o.CSD,
                    CustomerId = o.CustomerId,
                    ArticleId = o.ArticleId,
                    StyleId = o.StyleId,
                    CompanyId = o.CompanyId,
                    ShoeName = o.ShoeName,
                    LCSD = o.LCSD,
                    ArticleNo = o.ArticleNo,
                    BrandCodeId = o.BrandCodeId,
                    Brand = o.Brand,
                    Customer = o.Customer,
                    ArticleSizeCountryCode = o.ArticleSizeCountryCode,
                    Status = o.Status,

                    MPSOrdersId = m.Id,
                    // MpsArticleId = MPSArticle.Get().Where(a => a.ArticleNo == o.ArticleNo).Max(a => a.Id),
                    MpsArticleId = ma.Id,
                    ProcessSetId = 0,
                    IncreaseRate = 0,
                    BaseOn = 1,
                    ProcessType = 1,
                }
            )
            .Where(i => i.Status != 2 && i.MPSOrdersId == null && i.OrderQty > 0);


            return result;
        }

        public ERP.Models.Views.MPSOrdersGroup BuildMPSOrders(int id, int localeId)
        {
            var group = new ERP.Models.Views.MPSOrdersGroup();

            var orders = Orders.Get()
                .Where(i => i.Id == id && i.LocaleId == localeId)
                .Select(i => new ERP.Models.Views.MPSOrders
                {
                    Id = 0,
                    LocaleId = localeId,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    OrderNo = i.OrderNo,
                    OrderQty = i.OrderQty,
                    Qty = i.OrderQty,
                    MpsArticleId = MPSArticle.Get().Where(a => a.ArticleNo == i.ArticleNo).Max(a => a.Id),
                    ProcessSetId = 0,
                    StyleNo = i.StyleNo,
                    SizeCountryCodeId = (decimal)i.ArticleSizeCountryCodeId,
                    IncreaseRate = 0,
                    ETD = i.ETD,
                    CSD = i.CSD,
                    BaseOn = 1,
                    CustomerNameTw = i.Customer,
                    ProcessType = 1,
                    ArticleId = i.ArticleId,
                })
                .FirstOrDefault();

            if (orders != null)
            {
                var sizeruns = (
                    from oi in OrdersItem.Get()
                    join ars in ArticleSize.Get().Where(i => i.ArticleId == orders.ArticleId && i.LocaleId == orders.LocaleId) on new { ArticleInnerSize = oi.ArticleInnerSize, LocaleId = oi.LocaleId } equals new { ArticleInnerSize = (decimal)ars.ArticleInnerSize, LocaleId = ars.LocaleId }
                    where oi.OrdersId == id && oi.LocaleId == localeId
                    select new ERP.Models.Views.MPSOrdersItem
                    {
                        Id = 0,
                        MpsOrdersId = 0,
                        ArticleSize = ars.ArticleSize,
                        ArticleSizeSuffix = ars.ArticleSizeSuffix,
                        ArticleInnerSize = (decimal)ars.ArticleInnerSize,
                        DisplaySize = ars.ArticleDisplaySize,

                        KnifeDisplaySize = ars.KnifeDisplaySize,
                        KnifeInnerSize = (double)ars.KnifeInnerSize,
                        OutsoleDisplaySize = ars.OutsoleDisplaySize,
                        OutsoleInnerSize = (double)ars.OutsoleInnerSize,
                        LastDisplaySize = ars.LastDisplaySize,
                        LastInnerSize = (double)ars.LastInnerSize,
                        ShellDisplaySize = ars.ShellDisplaySize,
                        ShellInnerSize = (double)ars.ShellInnerSize,
                        Other1SizeDesc = ars.Other1Desc,
                        Other1InnerSize = (double)ars.Other1InnerSize,
                        Other2SizeDesc = ars.Other2SizeDesc,
                        Other2InnerSize = (double)ars.Other2InnerSize,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        LocaleId = orders.LocaleId,
                        OrderNo = orders.OrderNo,
                        OrderQty = oi.Qty,
                        Qty = oi.Qty,
                    }
                )
                .OrderBy(i => i.ArticleInnerSize)
                .ToList();

                group.MPSOrders = orders;
                group.MPSOrdersItem = sizeruns;
            }
            return group;
        }

        public List<ERP.Models.Views.MPSOrdersItem> GetSizeImport(int mpsOrderId, string orderNo, int localeId)
        {
            var sizeItems = (
                    from oi in OrdersItem.Get()
                    join o in Orders.GetEntity() on new { OrderId = oi.OrdersId, LocaleId = oi.LocaleId } equals new { OrderId = (decimal?)o.Id, LocaleId = o.LocaleId }
                    join ars in ArticleSize.Get() on new { ArticleInnerSize = oi.ArticleInnerSize, LocaleId = oi.LocaleId, ArticleId = o.ArticleId} equals new { ArticleInnerSize = (decimal)ars.ArticleInnerSize, LocaleId = ars.LocaleId, ArticleId = ars.ArticleId }
                    where o.OrderNo == orderNo
                    select new ERP.Models.Views.MPSOrdersItem
                    {
                        Id = MPSOrdersItem.Get().Where(i => i.MpsOrdersId == mpsOrderId && i.LocaleId == localeId && i.ArticleInnerSize == (decimal)ars.ArticleInnerSize).Max(i => i.Id),
                        MpsOrdersId = mpsOrderId,
                        ArticleSize = ars.ArticleSize,
                        ArticleSizeSuffix = ars.ArticleSizeSuffix,
                        ArticleInnerSize = (decimal)ars.ArticleInnerSize,
                        DisplaySize = ars.ArticleDisplaySize,

                        KnifeDisplaySize = ars.KnifeDisplaySize,
                        KnifeInnerSize = (double)ars.KnifeInnerSize,
                        OutsoleDisplaySize = ars.OutsoleDisplaySize,
                        OutsoleInnerSize = (double)ars.OutsoleInnerSize,
                        LastDisplaySize = ars.LastDisplaySize,
                        LastInnerSize = (double)ars.LastInnerSize,
                        ShellDisplaySize = ars.ShellDisplaySize,
                        ShellInnerSize = (double)ars.ShellInnerSize,
                        Other1SizeDesc = ars.Other1Desc,
                        Other1InnerSize = (double)ars.Other1InnerSize,
                        Other2SizeDesc = ars.Other2SizeDesc,
                        Other2InnerSize = (double)ars.Other2InnerSize,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        LocaleId = localeId,
                        OrderNo = orderNo,
                        OrderQty = oi.Qty,
                        Qty = oi.Qty,
                    }
                )
                .OrderBy(i => i.ArticleInnerSize)
                .ToList();
            return sizeItems;
        }

        public ERP.Models.Views.MPSOrdersGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.MPSOrdersGroup();

            var orders = MPSOrders.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (orders != null)
            {
                group.MPSOrders = orders;
                group.MPSOrdersItem = MPSOrdersItem.Get().Where(i => i.MpsOrdersId == orders.Id && i.LocaleId == orders.LocaleId).ToList();
            }
            return group;
        }
        public ERP.Models.Views.MPSOrdersGroup Save(MPSOrdersGroup group)
        {
            var orders = group.MPSOrders;
            var ordersItems = group.MPSOrdersItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (orders != null)
                {
                    //vendor
                    {
                        var _orders = MPSOrders.Get().Where(i => i.LocaleId == orders.LocaleId && i.Id == orders.Id).FirstOrDefault();
                        if (_orders == null)
                        {
                            orders = MPSOrders.Create(orders);
                        }
                        else
                        {
                            orders.Id = _orders.Id;
                            orders.LocaleId = _orders.LocaleId;
                            orders = MPSOrders.Update(orders);
                        }
                    }
                    //items
                    {
                        if (orders.Id != 0)
                        {
                            ordersItems.ForEach(i =>
                            {
                                i.MpsOrdersId = orders.Id;
                                i.OrderNo = orders.OrderNo;
                                i.LocaleId = orders.LocaleId;
                            });
                            MPSOrdersItem.RemoveRange(i => i.MpsOrdersId == orders.Id && i.LocaleId == orders.LocaleId);
                            MPSOrdersItem.CreateRange(ordersItems);
                        }
                    }
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

            ResetCache((int)orders.LocaleId);
            return Get((int)orders.Id, (int)orders.LocaleId);
        }
        public void Remove(MPSOrdersGroup group)
        {
            var orders = group.MPSOrders;
            try
            {
                UnitOfWork.BeginTransaction();
                if (orders != null)
                {
                    MPSOrdersItem.RemoveRange(i => i.Id == orders.Id && i.LocaleId == orders.LocaleId);
                    MPSOrders.Remove(orders);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            ResetCache((int)orders.LocaleId);
        }
        private void ResetCache(int localeId)
        {
            //非同步更新快取（避免 ObjectDisposed）
            Task.Run(() =>
            {
                using (var scope = _ServiceScopeFactory.CreateScope())
                {
                    try
                    {
                        var scopedCache = scope.ServiceProvider.GetRequiredService<CacheService>();
                        scopedCache.LoadMPSOrdersCache(localeId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MPSOrder Cache Update Error >>" + ex.Message);
                    }
                }
            });
        }
    }
}
