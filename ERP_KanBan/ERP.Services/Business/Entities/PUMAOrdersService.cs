using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Business.Entities
{
    public class PUMAOrdersService : BusinessService
    {
        private Services.Entities.PUMAOrdersService PUMAOrders { get; }
        public PUMAOrdersService(
            Services.Entities.PUMAOrdersService pumaOrdersService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            PUMAOrders = pumaOrdersService;
        }
        public IQueryable<PUMAOrders> Get()
        {
            return PUMAOrders.Get().Select(i => new PUMAOrders
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                CustomerName = i.CustomerName,
                StyleNo = i.StyleNo,
                OrderNo = i.OrderNo,
                CSD = i.CSD,
                ETD = i.ETD,
                OrderDate = i.OrderDate,
                LCSD = i.LCSD,
                Season = i.Season,
            });
        }

        public PUMAOrders Get(decimal id, decimal localeId)
        {
            return Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }

        public void CreateRange(List<PUMAOrders> pumaOrders)
        {
            PUMAOrders.CreateRange(BuildRange(pumaOrders));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.PUMAOrders, bool>> predicate)
        {
            PUMAOrders.RemoveRange(predicate);
        }
        private ERP.Models.Entities.PUMAOrders Build(PUMAOrders item)
        {
            return new ERP.Models.Entities.PUMAOrders
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CustomerName = item.CustomerName,
                StyleNo = item.StyleNo,
                OrderNo = item.OrderNo,
                CSD = item.CSD,
                ETD = item.ETD,
                OrderDate = item.OrderDate,
                LCSD = item.LCSD,
                Season = item.Season,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
            };
        }
        private IEnumerable<ERP.Models.Entities.PUMAOrders> BuildRange(List<PUMAOrders> items)
        {
            return items.Select(item => new ERP.Models.Entities.PUMAOrders
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CustomerName = item.CustomerName,
                StyleNo = item.StyleNo,
                OrderNo = item.OrderNo,
                CSD = item.CSD,
                ETD = item.ETD,
                OrderDate = item.OrderDate,
                LCSD = item.LCSD,
                Season = item.Season,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
            });
        }
    }
}