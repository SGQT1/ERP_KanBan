using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class BondMRPService : BusinessService
    {
        private Services.Entities.BondMRPService BondMRP { get; }
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.SimpleSaleService SimpleSale { get; }

        private Services.Entities.BondProductChinaContrastService BondProductChinaContrast { get; }

        public BondMRPService(
            Services.Entities.BondMRPService bondMRPService,
            Services.Entities.OrdersService ordersService,
            Services.Entities.SimpleSaleService simpleSale,
            Services.Entities.CustomerService customerService,
            Services.Entities.BondProductChinaContrastService bondProductChinaContrastService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            BondMRP = bondMRPService;
            Orders = ordersService;
            SimpleSale = simpleSale;

            BondProductChinaContrast = bondProductChinaContrastService;
        }
        public IQueryable<Models.Views.BondMRP> Get()
        {
            return BondMRP.Get().Select(i => new Models.Views.BondMRP
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                IsClose = i.IsClose,
                OrderNo = i.OrderNo,
                BOMLocaleId = i.BOMLocaleId,
                StyleNo = i.StyleNo,
                BondProductName = i.BondProductName,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                SalesDate = i.SalesDate,
                BondNo = i.BondNo,
            });
        }
        public IQueryable<Models.Views.BondMRP> GetWithItem()
        {
            var result = (
                from o in Orders.Get().Where(i => i.Status != 3 && i.OrderType == 2)
                join b in BondMRP.Get() on new { Orders = o.OrderNo } equals new { Orders = b.OrderNo } into bGRP
                from b in bGRP.DefaultIfEmpty()
                join p in BondProductChinaContrast.Get() on new { StyleNo = o.StyleNo } equals new { StyleNo = p.StyleNo } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new Models.Views.BondMRP
                {
                    LocaleId = o.LocaleId,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    OrderNo = o.OrderNo,
                    CustomerName = o.Customer,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    OrderQty = o.OrderQty,
                    // ShipQty = SimpleSale.Get().Where(i => i.OrdersId == o.Id && i.RefLocaleId == o.LocaleId).Sum(i =>i.SaleQty),
                    // ShortQty = o.OrderQty - SimpleSale.Get().Where(i => i.OrdersId == o.Id && i.RefLocaleId == o.LocaleId).Sum(i => i.SaleQty),
                    ShipQty = SimpleSale.Get().Where(i => i.OrdersId == o.Id && i.RefLocaleId == o.LocaleId).Select(i => (int?)i.SaleQty).Sum() ?? 0,
                    ShortQty = o.OrderQty - (SimpleSale.Get().Where(i => i.OrdersId == o.Id && i.RefLocaleId == o.LocaleId).Select(i => (int?)i.SaleQty).Sum() ?? 0),
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    LastNo = o.LastNo,
                    OutsoleNo = o.OutsoleNo,
                    CSDYM = "",

                    Id = b.Id,
                    IsClose = b.IsClose,
                    BondOrderNo = b.OrderNo,
                    BOMLocaleId = b == null ? o.LocaleId : b.BOMLocaleId,
                    BondStyleNo = b.StyleNo,
                    BondProductName = b.BondProductName,
                    ModifyUserName = b.ModifyUserName,
                    LastUpdateTime = b.LastUpdateTime,
                    SalesDate = b.SalesDate,
                    BondNo = b.BondNo,
                    RefBondProductName = p.BondProductName,
                });

            return result.AsQueryable();
        }

        public Models.Views.BondMRP Create(Models.Views.BondMRP item)
        {
            var _item = BondMRP.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.BondMRP Update(Models.Views.BondMRP item)
        {
            var _item = BondMRP.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }

        public void CreateRange(IEnumerable<Models.Views.BondMRP> items)
        {
            BondMRP.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.BondMRP> items)
        {
            BondMRP.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.BondMRP, bool>> predicate)
        {
            BondMRP.RemoveRange(predicate);
        }
        private Models.Entities.BondMRP Build(Models.Views.BondMRP item)
        {
            return new Models.Entities.BondMRP()
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId,
                IsClose = item.IsClose ?? 0,
                OrderNo = item.OrderNo,
                BOMLocaleId = item.BOMLocaleId ?? 0,
                StyleNo = item.StyleNo,
                BondProductName = item.BondProductName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime ?? DateTime.Now,
                SalesDate = item.SalesDate,
                BondNo = item.BondNo,
            };
        }
        private IEnumerable<Models.Entities.BondMRP> BuildRange(IEnumerable<Models.Views.BondMRP> items)
        {
            return items.Select(item => new Models.Entities.BondMRP()
            {
                Id = item.Id ?? 0,
                LocaleId = item.LocaleId,
                IsClose = item.IsClose ?? 0,
                OrderNo = item.OrderNo,
                BOMLocaleId = item.BOMLocaleId ?? 0,
                StyleNo = item.StyleNo,
                BondProductName = item.BondProductName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime ?? DateTime.Now,
                SalesDate = item.SalesDate,
                BondNo = item.BondNo,
            });
        }
    }
}