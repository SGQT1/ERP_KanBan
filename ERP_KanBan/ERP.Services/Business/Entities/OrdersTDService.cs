using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersTDService : BusinessService
    {
        private Services.Entities.OrdersTDService OrdersTD { get; }

        public OrdersTDService(Services.Entities.OrdersTDService ordersTDService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.OrdersTD = ordersTDService;
        }
        public IQueryable<Models.Views.OrdersTD> Get()
        {
            return OrdersTD.Get().Select(i => new Models.Views.OrdersTD
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrdersId = i.OrdersId,
                NOrderNo = i.NOrderNo,
                Qty = i.Qty,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.OrdersTD> ordersTD)
        {
            OrdersTD.CreateRange(BuildRange(ordersTD));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.OrdersTD, bool>> predicate)
        {
            OrdersTD.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.OrdersTD> BuildRange(IEnumerable<Models.Views.OrdersTD> items)
        {
            return items.Select(item => new ERP.Models.Entities.OrdersTD
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                OrdersId = item.OrdersId,
                NOrderNo = item.NOrderNo,
                Qty = item.Qty,
            });
        }
    }
}