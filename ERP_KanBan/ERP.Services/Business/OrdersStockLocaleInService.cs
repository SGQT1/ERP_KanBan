using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using KellermanSoftware.CompareNetObjects;

namespace ERP.Services.Business
{
    public class OrdersStockLocaleInService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersStockLocaleService OrdersStockLocale { get; set; }
        private ERP.Services.Business.Entities.OrdersStockLocaleInService OrdersStockLocaleIn { get; set; }
        private ERP.Services.Business.Entities.OrdersStockItemService OrdersStockItem { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; }
        public OrdersStockLocaleInService(
            ERP.Services.Business.Entities.OrdersStockLocaleService ordersStockLocaleService,
            ERP.Services.Business.Entities.OrdersStockLocaleInService ordersStockLocaleInService,
            ERP.Services.Business.Entities.OrdersStockItemService ordersStockItemService,
            ERP.Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OrdersStockLocale = ordersStockLocaleService;
            OrdersStockLocaleIn = ordersStockLocaleInService;
            OrdersStockItem = ordersStockItemService;
            Orders = ordersService;
        }

        public ERP.Models.Views.OrdersStockLocaleGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.OrdersStockLocaleGroup();

            group.OrdersStockLocale = OrdersStockLocale.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            group.OrdersStockLocaleIn = OrdersStockLocaleIn.Get().Where(i => i.OrdersStockLocaleId == id && i.LocaleId == localeId).ToList();

            return group;
        }

        public ERP.Models.Views.OrdersStockItem GetBarcode(string barcode, int localeId)
        {
            var item = OrdersStockItem.Get()
                .Where(i => i.LabelCode == barcode && i.LocaleId == localeId)
                .Select(i => new ERP.Models.Views.OrdersStockItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    OrderId = i.OrderId,
                    CTNLabelId = i.CTNLabelId,
                    CompanyId = i.CompanyId,
                    Company = i.Company,
                    BrandCodeId = i.BrandCodeId,
                    BrandCode = i.BrandCode,
                    OrderNo = i.OrderNo,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    Customer = i.Customer,
                    SeqNo = i.SeqNo,
                    CartonCount = i.CartonCount,
                    LabelCode = i.LabelCode,
                    MinOrderSize = i.MinOrderSize,
                    MaxOrderSize = i.MaxOrderSize,
                    OrderQty = i.OrderQty,
                    PackingQty = i.PackingQty,
                    SubPackingQty = i.SubPackingQty,
                    SubLabelCode = i.SubLabelCode,

                    StockInTime = i.StockInTime,
                    StockOutTime = i.StockOutTime,

                    GBSPOReferenceNo = i.GBSPOReferenceNo,
                })
                .FirstOrDefault();

            return item;
        }

        public ERP.Models.Views.OrdersStockItem GetCustomerBarcode(string barcode, int localeId)
        {
            var item = OrdersStockItem.Get()
                .Where(i => i.SubLabelCode == barcode && i.LocaleId == localeId)
                .Select(i => new ERP.Models.Views.OrdersStockItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    OrderId = i.OrderId,
                    CTNLabelId = i.CTNLabelId,
                    CompanyId = i.CompanyId,
                    Company = i.Company,
                    BrandCodeId = i.BrandCodeId,
                    BrandCode = i.BrandCode,
                    OrderNo = i.OrderNo,
                    CSD = i.CSD,
                    LCSD = i.LCSD,
                    StyleNo = i.StyleNo,
                    ShoeName = i.ShoeName,
                    Customer = i.Customer,
                    SeqNo = i.SeqNo,
                    CartonCount = i.CartonCount,
                    LabelCode = i.LabelCode,
                    MinOrderSize = i.MinOrderSize,
                    MaxOrderSize = i.MaxOrderSize,
                    OrderQty = i.OrderQty,
                    PackingQty = i.PackingQty,
                    SubPackingQty = i.SubPackingQty,
                    SubLabelCode = i.SubLabelCode,

                    StockInTime = i.StockInTime,
                    StockOutTime = i.StockOutTime,

                    GBSPOReferenceNo = i.GBSPOReferenceNo,
                })
                .FirstOrDefault();

            return item;
        }
        public ERP.Models.Views.OrdersStockLocaleGroup Save(ERP.Models.Views.OrdersStockLocaleGroup group)
        {
            var ordersStockLocale = group.OrdersStockLocale;
            var ordersStockLocaleIn = group.OrdersStockLocaleIn.DistinctBy(x => new
            {
                LocaleId = x.LocaleId,
                OrdersStockLocaleId = x.OrdersStockLocaleId,
                OrdersStockLocaleCode = x.OrdersStockLocaleCode ?? "",
                CTNLabelCode = x.CTNLabelCode ?? "",
                CTNLabelId = x.CTNLabelId,
                CTNLabelItemId = x.CTNLabelItemId,
                OrderNo = x.OrderNo ?? "",
                SeqNo = x.SeqNo,
                StockInTime = x.StockInTime.ToUniversalTime(),
                Remark = x.Remark ?? "",
                Version = x.Version,
            });

            UnitOfWork.BeginTransaction();
            try
            {
                if (ordersStockLocaleIn.Any())
                {
                    var labelCodes = ordersStockLocaleIn.Select(i => i.CTNLabelCode).Distinct();
                    OrdersStockLocaleIn.RemoveRange(i => i.LocaleId == ordersStockLocale.LocaleId && labelCodes.Contains(i.CTNLabelCode));
                    OrdersStockLocaleIn.CreateRange(ordersStockLocaleIn);
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)ordersStockLocale.Id, (int)ordersStockLocale.LocaleId);
        }

        public ERP.Models.Views.OrdersStockLocaleGroup Remove(ERP.Models.Views.OrdersStockLocaleGroup group)
        {
            var ordersStockLocale = group.OrdersStockLocale;
            var ordersStockLocaleIn = group.OrdersStockLocaleIn;

            UnitOfWork.BeginTransaction();
            try
            {
                if (ordersStockLocaleIn.Any())
                {
                    var labelCodes = ordersStockLocaleIn.Select(i => i.CTNLabelCode);
                    OrdersStockLocaleIn.RemoveRange(i => i.LocaleId == ordersStockLocale.LocaleId && labelCodes.Contains(i.CTNLabelCode));
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)ordersStockLocale.Id, (int)ordersStockLocale.LocaleId);
        }

    }
}