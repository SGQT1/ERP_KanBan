using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersPLForUSAService : BusinessService
    {
        private Services.Entities.OrdersPLForUSAService OrdersPLForUSA { get; }

        public OrdersPLForUSAService(Services.Entities.OrdersPLForUSAService ordersPLForUSAService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.OrdersPLForUSA = ordersPLForUSAService;
        }
        public IQueryable<Models.Views.OrdersPLForUSA> Get()
        {
            return OrdersPLForUSA.Get().Select(i => new Models.Views.OrdersPLForUSA
            {   
                Id = i.Id,
                LocaleId = i.LocaleId,
                RefLocaleId = i.RefLocaleId,
                RefOrdersId = i.RefOrdersId,
                Edition = i.Edition,
                MinCNo = i.MinCNo,
                MaxCNo = i.MaxCNo,
                CTNL = i.CTNL,
                CTNW = i.CTNW,
                CTNH = i.CTNH,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
    }
}