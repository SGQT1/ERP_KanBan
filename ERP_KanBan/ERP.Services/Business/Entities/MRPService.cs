using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MRPService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.PCLService PCL { get; }
        private Services.Entities.PCLBKService PCLBK { get; }
        
        private Services.Entities.StyleService Style { get; }
        private Services.Entities.OrdersTransferService OrdersTransfer { get; set; }
        private Services.Entities.QueueDoMRPService QueueDoMRP { get; set; }
        private Services.Entities.QueueDoMRPLogService QueueDoMRPLog { get; set; }
        private Services.Entities.MRPRemovedService MRPRemoved { get; }

        private Services.Entities.MRPItemService MRPItem { get; }
        
        public MRPService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.PCLService pclService,
            Services.Entities.PCLBKService pclBKService,
            Services.Business.Entities.TypeService typeService,
            Services.Entities.StyleService styleService,
            Services.Entities.OrdersTransferService ordersTransferService,
            Services.Entities.QueueDoMRPService queueDoMRPService,
            Services.Entities.QueueDoMRPLogService queueDoMRPLogService,
            Services.Entities.MRPRemovedService mrpRemovedService,
            Services.Entities.MRPItemService mrpItemService,

            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            PCL = pclService;
            PCLBK = pclBKService;
            Style = styleService;
            OrdersTransfer = ordersTransferService;
            QueueDoMRP = queueDoMRPService;
            QueueDoMRPLog = queueDoMRPLogService;
            MRPRemoved = mrpRemovedService;

            MRPItem = mrpItemService;
        }
        public IQueryable<Models.Views.OrdersDoMRP> GetOrdersDoMRP(string predicate, string sort, int? take)
        {
            var orders = ( 
                from o in Orders.Get()
                join s in Style.Get() on new { StyleId = o.StyleId, LocaleId = o.LocaleId } equals new { StyleId = s.Id, LocaleId = s.LocaleId }
                join ot in OrdersTransfer.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = ot.OrdersId, LocaleId = ot.LocaleId } into otGRP
                from ot in otGRP.DefaultIfEmpty()
                join mr in MRPRemoved.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mr.OrdersId, LocaleId = mr.LocaleId } into mrGRP
                from mr in mrGRP.DefaultIfEmpty()
                join q in QueueDoMRP.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = q.OrdersId, LocaleId = q.LocaleId } into qGRP
                from q in qGRP.DefaultIfEmpty()
                join ql in QueueDoMRPLog.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = (decimal)ql.OrdersId, LocaleId = (decimal)ql.LocaleId } into qlGRP
                from ql in qlGRP.DefaultIfEmpty()
                join p in PCL.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = (decimal)p.OrdersId, LocaleId = (decimal)p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty() // because 強型別,will null exception
                where ot == null && mr == null && q == null &&
                      (s.doMRP == 1 || (s.doMRP == 2 && o.ProductType == 1)) &&
                      (o.doMRP == 1 && (o.Status == 0 || o.Status == 1))
                select new Models.Views.OrdersDoMRP
                {
                    OrdersId = o.Id,
                    OrderNo = o.OrderNo,
                    MRPType = ql == null ? 0 : 1,
                    ArticleId = o.ArticleId,
                    ArticleNo = o.ArticleNo,
                    StyleId = o.StyleId,
                    StyleNo = o.StyleNo,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    LocaleId = o.LocaleId,
                    CompanyId = o.CompanyId,
                    Status = o.Status,
                    doMRP = o.doMRP,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OWD = o.OWD,
                    ETC = o.ETD,
                    OrderQty = o.OrderQty,
                    CurrentOrderVersion = o.Version,
                    MRPOrderVersion = p.OrdersVersion,
                    ShoeName = o.ShoeName,
                    IsApproved = o.IsApproved,
                    ARLocaleId = o.ARLocaleId,
                    BrandCodeId = o.BrandCodeId,
                    Season = o.Season,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    FinishTime = ql.FinishTime, // performance issue
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(g => new {
                OrdersId = g.OrdersId,
                OrderNo = g.OrderNo,
                MRPType = g.MRPType,
                ArticleId = g.ArticleId,
                ArticleNo = g.ArticleNo,
                StyleId = g.StyleId,
                StyleNo = g.StyleNo,
                OrderType = g.OrderType,
                ProductType = g.ProductType,
                LocaleId = g.LocaleId,
                CompanyId = g.CompanyId,
                Status = g.Status,
                doMRP = g.doMRP,
                CSD = g.CSD,
                LCSD = g.LCSD,
                OWD = g.OWD,
                ETC = g.ETC,
                OrderQty = g.OrderQty,
                CurrentOrderVersion = g.CurrentOrderVersion,
                MRPOrderVersion = g.MRPOrderVersion,
                ShoeName = g.ShoeName,
                IsApproved = g.IsApproved,
                ARLocaleId = g.ARLocaleId,
                BrandCodeId = g.BrandCodeId,
                Season = g.Season,
                ModifyUserName = g.ModifyUserName,
                LastUpdateTime = g.LastUpdateTime,
            })
            .Select(g => new Models.Views.OrdersDoMRP {
                OrdersId = g.Key.OrdersId,
                OrderNo = g.Key.OrderNo,
                MRPType = g.Key.MRPType,
                ArticleId = g.Key.ArticleId,
                ArticleNo = g.Key.ArticleNo,
                StyleId = g.Key.StyleId,
                StyleNo = g.Key.StyleNo,
                OrderType = g.Key.OrderType,
                ProductType = g.Key.ProductType,
                LocaleId = g.Key.LocaleId,
                CompanyId = g.Key.CompanyId,
                Status = g.Key.Status,
                doMRP = g.Key.doMRP,
                CSD = g.Key.CSD,
                LCSD = g.Key.LCSD,
                OWD = g.Key.OWD,
                ETC = g.Key.ETC,
                OrderQty = g.Key.OrderQty,
                CurrentOrderVersion = g.Key.CurrentOrderVersion,
                MRPOrderVersion = g.Key.MRPOrderVersion,
                ShoeName = g.Key.ShoeName,
                IsApproved = g.Key.IsApproved,
                ARLocaleId = g.Key.ARLocaleId,
                BrandCodeId = g.Key.BrandCodeId,
                Season = g.Key.Season,
                ModifyUserName = g.Key.ModifyUserName,
                LastUpdateTime = g.Key.LastUpdateTime,
                FinishTime = g.Max( gg => gg.FinishTime)// performance issue
            });

            orders = sort.Length > 0 ? orders.OrderBy(sort) : orders;
            orders = take != null ? orders.Take((int)take) : orders;
            return orders.ToList().AsQueryable();
        }
        
        private class PCLHead
        {
            public decimal? OrdersId { get; set; }
            public decimal? LocaleId { get; set; }
            public int? OrdersVersion { get; set; }
        }    
        private class QueueDoMRPLogHead
        {
            public decimal? OrdersId { get; set; }
            public decimal? LocaleId { get; set; }
            public DateTime? FinishTime { get; set; }
            public DateTime? SubmitTime { get; set; }
        }  
    }
}