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
    public class BOMService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.PCLService PCL { get; }
        private Services.Entities.PCLBKService PCLBK { get; }
        private Services.Business.Entities.TypeService Type { get; }
        public BOMService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.PCLService pclService,
            Services.Entities.PCLBKService pclBKService,
            Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            CodeItem = codeItemService;
            PCL = pclService;
            PCLBK = pclBKService;
            Type = typeService;
        }
        public IQueryable<Models.Views.BOM> Get(string predicate, string sort, int? take)
        {
            var closed = GetClosed().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);
            closed = sort.Length > 0 ? closed.OrderBy(sort) : closed;
            closed = take != 0 ? closed.Take((int)take) : closed;

            var unclosed = GetUnClosed().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);
            unclosed = sort.Length > 0 ? unclosed.OrderBy(sort) : unclosed;
            unclosed = take != 0 ? unclosed.Take((int)take) : unclosed;


            var t1 = closed.ToList();
            var t2 = unclosed.ToList();
            
            return unclosed.ToList().Union(closed.ToList()).AsQueryable();
        }
        public IQueryable<Models.Views.BOM> GetUnClosed()
        {
            var boms = (
                from o in Orders.Get()
                join c in CodeItem.Get() on new { BrandCodeId = o.BrandCodeId, LocaleId = o.LocaleId } equals new { BrandCodeId = (decimal?)c.Id, LocaleId = c.LocaleId } into cGrp
                from c in cGrp.DefaultIfEmpty()
                join m in (
                    from pcl in PCL.Get()
                    group pcl by new { pcl.OrdersId, pcl.LocaleId, pcl.OrdersVersion, pcl.ModifyUserName } into g
                    select new
                    {
                        OrdersId = g.Key.OrdersId,
                        LocaleId = g.Key.LocaleId,
                        OrdersVersion = g.Key.OrdersVersion,
                        ModifyUserName = g.Key.ModifyUserName,
                        LastUpdateTime = g.Max(i => i.LastUpdateTime)
                    }
                ) on new { OrderId = o.Id, LocaleId = o.LocaleId } equals new { OrderId = m.OrdersId, LocaleId = m.LocaleId }
                select new Models.Views.BOM
                {
                    OrderId = o.Id,
                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    ArticleId = o.ArticleId,
                    StyleId = o.StyleId,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    CompanyId = o.CompanyId,
                    LocaleId = o.LocaleId,
                    Status = o.Status,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    BrandCodeId = o.BrandCodeId,
                    RefBrand = c.NameTW,
                    Season = o.Season,
                    BOMType = 0,
                    OrdersVersion = m.OrdersVersion,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                }
            );
            return boms;
        }
        public IQueryable<Models.Views.BOM> GetClosed()
        {
            var boms = (
                from o in Orders.Get()
                join c in CodeItem.Get() on new { BrandCodeId = o.BrandCodeId, LocaleId = o.LocaleId } equals new { BrandCodeId = (decimal?)c.Id, LocaleId = c.LocaleId } into cGrp
                from c in cGrp.DefaultIfEmpty()
                join m in (
                    from pclBK in PCLBK.Get()
                    group pclBK by new { pclBK.OrdersId, pclBK.LocaleId, pclBK.OrdersVersion, pclBK.ModifyUserName } into g
                    select new
                    {
                        BOMType = 1,
                        OrdersId = g.Key.OrdersId,
                        LocaleId = g.Key.LocaleId,
                        OrdersVersion = g.Key.OrdersVersion,
                        ModifyUserName = g.Key.ModifyUserName,
                        LastUpdateTime = g.Max(i => i.LastUpdateTime)
                    }
                ) on new { OrderId = o.Id, LocaleId = o.LocaleId } equals new { OrderId = m.OrdersId, LocaleId = m.LocaleId }
                select new Models.Views.BOM
                {
                    OrderId = o.Id,
                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    ArticleId = o.ArticleId,
                    StyleId = o.StyleId,
                    OrderType = o.OrderType,
                    ProductType = o.ProductType,
                    CompanyId = o.CompanyId,
                    LocaleId = o.LocaleId,
                    Status = o.Status,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    BrandCodeId = o.BrandCodeId,
                    RefBrand = c.NameTW,
                    Season = o.Season,
                    BOMType = 1,
                    OrdersVersion = m.OrdersVersion,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                }
            );
            return boms;
        }
    }
}