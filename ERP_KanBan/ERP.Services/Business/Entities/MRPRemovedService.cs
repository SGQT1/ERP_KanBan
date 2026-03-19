using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MRPRemovedService : BusinessService
    {
        private Services.Entities.MRPRemovedService MRPRemoved { get; }

        public MRPRemovedService(Services.Entities.MRPRemovedService mrpRemovedService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.MRPRemoved = mrpRemovedService;
        }
        public IQueryable<Models.Views.MRPRemoved> Get()
        {
            return MRPRemoved.Get().Select(i => new Models.Views.MRPRemoved
            {
                LocaleId = i.LocaleId,
                OrdersId = i.OrdersId
            });
        }
    }
}