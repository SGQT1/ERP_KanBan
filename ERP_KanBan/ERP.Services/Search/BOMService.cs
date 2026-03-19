using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;
using Newtonsoft.Json;

namespace ERP.Services.Search
{
    public class BOMService : SearchService
    {

        private ERP.Services.Business.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        public BOMService(
            ERP.Services.Business.Entities.MRPItemService mrpItemService,
            ERP.Services.Business.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
            Orders = ordersService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.View.BOMItem> GetBOMMaterialUsage(string predicate, string[] filters)
        {
            bool combindPart = false;

            // extend condition, obj by ExtentionItem
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);

                combindPart = extenFilters.Field9;
            }

            var mrpItems = (
                    from mrp in MRPItem.Get()
                    join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId }
                    join c in CodeItem.Get() on new { CategoryCodeId = mrp.CategoryCodeId, LocaleId = mrp.LocaleId } equals new { CategoryCodeId = (decimal?)c.Id, LocaleId = c.LocaleId } into cGRP
                    from c in cGRP.DefaultIfEmpty()
                    select new
                    {
                        Id = mrp.Id,
                        LocaleId = mrp.LocaleId,
                        OrdersId = mrp.OrdersId,
                        PartId = mrp.PartId,
                        PartNo = mrp.PartNo,
                        PartNameTw = mrp.PartNameTw,
                        PartNameEn = mrp.PartNameEn,
                        MaterialId = mrp.MaterialId,
                        MaterialNameTw = mrp.MaterialNameTw,
                        MaterialNameEn = mrp.MaterialNameEn,
                        UnitCodeId = mrp.UnitCodeId,
                        UnitNameTw = mrp.UnitNameTw,
                        UnitNameEn = mrp.UnitNameEn,
                        UnitTotal = mrp.UnitTotal,
                        Total = mrp.Total,
                        SizeDivision = mrp.SizeDivision,
                        SizeDivisionDescTw = mrp.SizeDivisionDescTw,
                        SizeDivisionDescEn = mrp.SizeDivisionDescEn,
                        Version = mrp.StyleVersion,
                        ParentMaterialId = mrp.ParentMaterialId,
                        ModifyUserName = mrp.ModifyUserName,
                        LastUpdateTime = mrp.LastUpdateTime,
                        SemiGoods = mrp.SemiGoods,
                        CategoryCodeId = mrp.CategoryCodeId,
                        Category = c.NameTW,
                        OrderNo = o.OrderNo,
                        StyleNo = o.StyleNo,
                        OrderQty = o.OrderQty,
                        Brand = o.Brand,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new ERP.Models.Views.View.BOMItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    OrdersId = i.OrdersId,
                    PartId = i.PartId,
                    PartNo = i.PartNo,
                    PartNameTw = i.PartNameTw,
                    PartNameEn = i.PartNameEn,
                    MaterialId = i.MaterialId,
                    MaterialNameTw = i.MaterialNameTw,
                    MaterialNameEn = i.MaterialNameEn,
                    UnitCodeId = i.UnitCodeId,
                    UnitNameTw = i.UnitNameTw,
                    UnitNameEn = i.UnitNameEn,
                    UnitTotal = i.UnitTotal,
                    Total = i.Total,
                    SizeDivision = i.SizeDivision,
                    SizeDivisionDescTw = i.SizeDivisionDescTw,
                    SizeDivisionDescEn = i.SizeDivisionDescEn,
                    Version = i.Version,
                    ParentMaterialId = i.ParentMaterialId,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    SemiGoods = i.SemiGoods,
                    CategoryCodeId = i.CategoryCodeId,
                    OrderNo = i.OrderNo,
                    StyleNo = i.StyleNo,
                    OrderQty = i.OrderQty,
                    Category = i.Category,
                    Brand = i.Brand,
                })
                .OrderBy(i => i.OrderNo)
                .Take(3000)
                .ToList();

            var mrpItemOrders = (
                    from mrp in MRPItemOrders.Get()
                    join o in Orders.Get() on new { OrdersId = mrp.OrdersId, LocaleId = mrp.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId }
                    join c in CodeItem.Get() on new { CategoryCodeId = mrp.CategoryCodeId, LocaleId = mrp.LocaleId } equals new { CategoryCodeId = (decimal?)c.Id, LocaleId = c.LocaleId } into cGRP
                    from c in cGRP.DefaultIfEmpty()
                    select new
                    {
                        Id = mrp.Id,
                        LocaleId = mrp.LocaleId,
                        OrdersId = mrp.OrdersId,
                        PartId = mrp.PartId,
                        PartNo = mrp.PartNo,
                        PartNameTw = mrp.PartNameTw,
                        PartNameEn = mrp.PartNameEn,
                        MaterialId = mrp.MaterialId,
                        MaterialNameTw = mrp.MaterialNameTw,
                        MaterialNameEn = mrp.MaterialNameEn,
                        UnitCodeId = mrp.UnitCodeId,
                        UnitNameTw = mrp.UnitNameTw,
                        UnitNameEn = mrp.UnitNameEn,
                        UnitTotal = mrp.UnitTotal,
                        Total = mrp.Total,
                        SizeDivision = mrp.SizeDivision,
                        SizeDivisionDescTw = mrp.SizeDivisionDescTw,
                        SizeDivisionDescEn = mrp.SizeDivisionDescEn,
                        Version = mrp.OrderVersion,
                        ParentMaterialId = mrp.ParentMaterialId,
                        ModifyUserName = mrp.ModifyUserName,
                        LastUpdateTime = mrp.LastUpdateTime,
                        SemiGoods = mrp.SemiGoods,
                        CategoryCodeId = mrp.CategoryCodeId,
                        Category = c.NameTW,
                        OrderNo = o.OrderNo,
                        StyleNo = o.StyleNo,
                        OrderQty = o.OrderQty,
                        Brand = o.Brand,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new ERP.Models.Views.View.BOMItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    OrdersId = i.OrdersId,
                    PartId = i.PartId,
                    PartNo = i.PartNo,
                    PartNameTw = i.PartNameTw,
                    PartNameEn = i.PartNameEn,
                    MaterialId = i.MaterialId,
                    MaterialNameTw = i.MaterialNameTw,
                    MaterialNameEn = i.MaterialNameEn,
                    UnitCodeId = i.UnitCodeId,
                    UnitNameTw = i.UnitNameTw,
                    UnitNameEn = i.UnitNameEn,
                    UnitTotal = i.UnitTotal,
                    Total = i.Total,
                    SizeDivision = i.SizeDivision,
                    SizeDivisionDescTw = i.SizeDivisionDescTw,
                    SizeDivisionDescEn = i.SizeDivisionDescEn,
                    Version = i.Version,
                    ParentMaterialId = i.ParentMaterialId,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    SemiGoods = i.SemiGoods,
                    CategoryCodeId = i.CategoryCodeId,
                    OrderNo = i.OrderNo,
                    StyleNo = i.StyleNo,
                    OrderQty = i.OrderQty,
                    Category = i.Category,
                    Brand = i.Brand,
                })
                .OrderBy(i => i.OrderNo)
                .Take(3000)
                .ToList();

            var bomItems = mrpItems.Union(mrpItemOrders).OrderBy(i => i.PartNo).ToList();

            if (combindPart)
            {
                bomItems = bomItems
                .GroupBy(g => new
                {
                    g.LocaleId,
                    g.OrdersId,
                    g.MaterialId,
                    g.MaterialNameTw,
                    g.MaterialNameEn,
                    g.UnitCodeId,
                    g.UnitNameTw,
                    g.UnitNameEn,
                    g.ParentMaterialId,
                    g.OrderNo,
                    g.StyleNo,
                    g.OrderQty,
                    g.Category,
                    g.Brand,
                })
                .Select(i => new ERP.Models.Views.View.BOMItem
                {
                    LocaleId = i.Key.LocaleId,
                    OrdersId = i.Key.OrdersId,
                    MaterialId = i.Key.MaterialId,
                    MaterialNameTw = i.Key.MaterialNameTw,
                    MaterialNameEn = i.Key.MaterialNameEn,
                    UnitCodeId = i.Key.UnitCodeId,
                    UnitNameTw = i.Key.UnitNameTw,
                    UnitNameEn = i.Key.UnitNameEn,
                    OrderNo = i.Key.OrderNo,
                    StyleNo = i.Key.StyleNo,
                    OrderQty = i.Key.OrderQty,
                    Total = i.Sum(g => g.Total),
                    ParentMaterialId = i.Key.ParentMaterialId,
                    Category = i.Key.Category,
                    Brand = i.Key.Brand,
                }).ToList();

                var serialNo = 1;
                bomItems.ForEach(i =>
                {
                    i.Id = serialNo;
                    serialNo++;
                });
            }
            return bomItems.AsQueryable();
        }
    }
}