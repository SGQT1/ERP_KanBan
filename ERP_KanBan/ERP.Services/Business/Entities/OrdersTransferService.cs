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
    public class OrdersTransferService : BusinessService
    {
        private Services.Entities.OrdersTransferService OrdersTransfer { get; }

        public OrdersTransferService(Services.Entities.OrdersTransferService ordersTransferService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.OrdersTransfer = ordersTransferService;
        }
        public IQueryable<Models.Views.OrdersTransfer> Get()
        {
            return OrdersTransfer.Get().Select(i => new Models.Views.OrdersTransfer
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrdersId = i.OrdersId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.OrdersTransfer> ordersItems)
        {
            OrdersTransfer.CreateRange(BuildRange(ordersItems));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.OrdersTransfer, bool>> predicate)
        {
            OrdersTransfer.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.OrdersTransfer> BuildRange(IEnumerable<Models.Views.OrdersTransfer> items)
        {
            return items.Select(item => new ERP.Models.Entities.OrdersTransfer
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                OrdersId = item.OrdersId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}