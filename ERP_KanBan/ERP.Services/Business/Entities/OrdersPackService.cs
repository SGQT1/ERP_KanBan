using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersPackService : BusinessService
    {
        private Services.Entities.OrdersPackService OrdersPack { get; }

        public OrdersPackService(Services.Entities.OrdersPackService ordersPackService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.OrdersPack = ordersPackService;
        }
        public IQueryable<Models.Views.OrdersPack> Get()
        {
            return OrdersPack.Get().Select(i => new Models.Views.OrdersPack
            {   
                Id =i.Id,
                LocaleId =i.LocaleId,
                RefLocaleId =i.RefLocaleId,
                RefOrdersId =i.RefOrdersId,
                Edition =i.Edition,
                ItemInnerSize =i.ItemInnerSize,
                RefDisplaySize =i.RefDisplaySize,
                PairOfCTN =i.PairOfCTN,
                CTNS =i.CTNS,
                GroupBy =i.GroupBy,
                NWOfCTN =i.NWOfCTN,
                GWOfCTN =i.GWOfCTN,
                MEAS =i.MEAS,
                CBM =i.CBM,
                AdjQty =i.AdjQty,
                ModifyUserName =i.ModifyUserName,
                LastUpdateTime =i.LastUpdateTime,
                PLId = i.PLId
            });
        }
    }
}