using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class MRPService : SearchService
    {
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }

        public MRPService(
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.MRPItemService mrpItemService,
            ERP.Services.Business.Entities.MRPItemOrdersService mrpItemOrdersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
        }

        public IQueryable<Models.Views.MRPItemSummary> GetPackingMaterial() {

            var mrpItemOrders = (
                from o in Orders.Get()
                join mi in MRPItemOrders.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId  } equals new { OrdersId = mi.OrdersId, LocaleId = mi.LocaleId } 
                select new Models.Views.MRPItemSummary
                {
                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    Customer = o.Customer,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OWD = o.OWD,
                    OrderDate = o.OrderDate,
                    LocaleNo = o.LocaleNo,
                    CompanyNo = o.CompanyNo,
                    CompanyId = o.CompanyId,
                    Brand = o.Brand,
                    BrandCodeId = o.BrandCodeId,
                    
                    Id = mi.Id,
                    LocaleId = mi.LocaleId,
                    OrdersId = mi.OrdersId,
                    PartId = mi.PartId,
                    PartNo = mi.PartNo,
                    PartNameTw = mi.PartNameTw,
                    PartNameEn = mi.PartNameEn,
                    MaterialId = mi.MaterialId,
                    MaterialNameTw = mi.MaterialNameTw,
                    MaterialNameEn = mi.MaterialNameEn,
                    UnitCodeId = mi.UnitCodeId,
                    UnitNameTw = mi.UnitNameTw,
                    UnitNameEn = mi.UnitNameEn,
                    UnitTotal = mi.UnitTotal,
                    Total = mi.Total,
                    SizeDivision = mi.SizeDivision,
                    SizeDivisionDescTw = mi.SizeDivisionDescTw,
                    SizeDivisionDescEn = mi.SizeDivisionDescEn,
                    Version = mi.OrderVersion,
                    ParentMaterialId = mi.ParentMaterialId,
                    ModifyUserName = mi.ModifyUserName,
                    LastUpdateTime = mi.LastUpdateTime,
                }
            );

            return mrpItemOrders;
        }
        
    }
}