using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ShipmentLogService : BusinessService
    {
        private Services.Entities.ShipmentLogService ShipmentLog { get; }
        public ShipmentLogService(
            Services.Entities.ShipmentLogService shipmentLogService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            ShipmentLog = shipmentLogService;
        }
        public IQueryable<Models.Views.ShipmentLog> Get()
        {
            return (
                from i in ShipmentLog.Get()
                select new Models.Views.ShipmentLog
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    SaleDate = i.SaleDate,
                    OrdersId = i.OrdersId,
                    SaleQty = i.SaleQty,
                    CloseDate = i.CloseDate,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    Season = i.Season,
                    CustomerId = i.CustomerId,
                    Customer = i.Customer,
                    OrderNo = i.OrderNo,
                    CompanyNo = i.CompanyNo,
                    CompanyId = i.CompanyId,
                    EffectiveDate = i.EffectiveDate,
                }
            );
        }

    }
}