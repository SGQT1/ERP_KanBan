using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSOrdersService : BusinessService
    {
        private ERP.Services.Entities.MpsOrdersService MPSOrders { get; set; }
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }

        public MPSOrdersService(
            ERP.Services.Entities.MpsOrdersService mpsOrdersService,
            ERP.Services.Business.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSOrders = mpsOrdersService;
            Orders = ordersService;
        }

        public IQueryable<Models.Views.MPSOrders> Get()
        {
            return MPSOrders.Get().Select(i => new Models.Views.MPSOrders
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                OrderNo = i.OrderNo,
                OrderQty = i.OrderQty,
                Qty = i.Qty,
                MpsArticleId = i.MpsArticleId,
                ProcessSetId = i.ProcessSetId,
                StyleNo = i.StyleNo,
                SizeCountryCodeId = i.SizeCountryCodeId,
                IncreaseRate = i.IncreaseRate,
                ETD = i.ETD,
                CSD = i.CSD,
                BaseOn = i.BaseOn,
                CustomerNameTw = i.CustomerNameTw,
                ProcessType = i.ProcessType,
            });
        }
        public IQueryable<Models.Views.MPSOrders> GetCache()
        {
            return MPSOrders.Get().Select(i => new Models.Views.MPSOrders
            {
                Id = i.Id,
                OrderNo = i.OrderNo,
                StyleNo = i.StyleNo,
                LocaleId = i.LocaleId,
                CSD = i.CSD,
                ProcessType = i.ProcessType,
            });
        }

        public Models.Views.MPSOrders Create(Models.Views.MPSOrders item)
        {
            var _item = MPSOrders.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSOrders Update(Models.Views.MPSOrders item)
        {
            var _item = MPSOrders.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSOrders item)
        {
            MPSOrders.Remove(Build(item));
        }
        private Models.Entities.MpsOrders Build(Models.Views.MPSOrders item)
        {
            return new Models.Entities.MpsOrders()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                OrderNo = item.OrderNo,
                OrderQty = item.OrderQty,
                Qty = item.Qty,
                MpsArticleId = item.MpsArticleId,
                ProcessSetId = item.ProcessSetId,
                StyleNo = item.StyleNo,
                SizeCountryCodeId = item.SizeCountryCodeId,
                IncreaseRate = item.IncreaseRate,
                ETD = item.ETD,
                CSD = item.CSD,
                BaseOn = item.BaseOn,
                CustomerNameTw = item.CustomerNameTw,
                ProcessType = item.ProcessType,
            };
        }


        public void CreateRange(IEnumerable<Models.Views.MPSOrders> items)
        {
            MPSOrders.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsOrders, bool>> predicate)
        {
            MPSOrders.RemoveRange(predicate);
        }

        private IEnumerable<ERP.Models.Entities.MpsOrders> BuildRange(IEnumerable<Models.Views.MPSOrders> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsOrders
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                OrderNo = item.OrderNo,
                OrderQty = item.OrderQty,
                Qty = item.Qty,
                MpsArticleId = item.MpsArticleId,
                ProcessSetId = item.ProcessSetId,
                StyleNo = item.StyleNo,
                SizeCountryCodeId = item.SizeCountryCodeId,
                IncreaseRate = item.IncreaseRate,
                ETD = item.ETD,
                CSD = item.CSD,
                BaseOn = item.BaseOn,
                CustomerNameTw = item.CustomerNameTw,
                ProcessType = item.ProcessType,
            });
        }

    }
}
