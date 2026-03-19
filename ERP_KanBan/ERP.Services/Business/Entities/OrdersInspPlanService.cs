using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersInspPlanService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.OrdersInspPlanService OrdersInspPlan { get; }

        public OrdersInspPlanService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.OrdersInspPlanService ordersInspPlanService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersInspPlan = ordersInspPlanService;
        }
        public IQueryable<Models.Views.OrdersInspPlan> Get()
        {
            return OrdersInspPlan.Get().Select(i => new Models.Views.OrdersInspPlan
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                LastUpdateTime = i.LastUpdateTime,
                ModifyUserName = i.ModifyUserName,
                OrdersId = i.OrdersId,
                OrderNo = i.OrderNo,
                STILineCode = i.STILineCode,
                ASSLineCode = i.ASSLineCode,
                InspPlanDate = i.InspPlanDate,
                WeeklyInspPlanDate = i.WeeklyInspPlanDate,
                CustomerOrderNo = i.CustomerOrderNo,
                CSD = i.CSD,
                LCSD = i.LCSD,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.OrdersInspPlan> items)
        {
            OrdersInspPlan.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.OrdersInspPlan, bool>> predicate)
        {
            OrdersInspPlan.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.OrdersInspPlan> BuildRange(IEnumerable<Models.Views.OrdersInspPlan> items)
        {
            return items.Select(item => new ERP.Models.Entities.OrdersInspPlan
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                LastUpdateTime = (DateTime)item.LastUpdateTime,
                ModifyUserName = item.ModifyUserName,
                OrdersId = item.OrdersId,
                OrderNo = item.OrderNo,
                STILineCode = item.STILineCode,
                ASSLineCode = item.ASSLineCode,
                InspPlanDate = item.InspPlanDate,
                WeeklyInspPlanDate = item.WeeklyInspPlanDate,
                CustomerOrderNo = item.CustomerOrderNo,
                CSD = (DateTime)item.CSD,
                LCSD = item.LCSD,
            });
        }
    }
}