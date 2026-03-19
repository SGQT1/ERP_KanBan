using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Business.Entities
{
    public class NBOrdersService : BusinessService
    {
        private Services.Entities.NBOrdersService NBOrders { get; }
        public NBOrdersService(
            Services.Entities.NBOrdersService nbOrdersService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            NBOrders = nbOrdersService;
        }
        public IQueryable<ERP.Models.Views.NBOrders> Get()
        {
            return NBOrders.Get().Select(i => new ERP.Models.Views.NBOrders
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

        public ERP.Models.Views.NBOrders Get(decimal id, decimal localeId)
        {
            return Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }

        public void CreateRange(List<ERP.Models.Views.NBOrders> nbOrders)
        {
            NBOrders.CreateRange(BuildRange(nbOrders));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.NBOrders, bool>> predicate)
        {
            NBOrders.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.NBOrders> BuildRange(List<ERP.Models.Views.NBOrders> items)
        {
            return items.Select(item => new ERP.Models.Entities.NBOrders
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CustomerName = item.CustomerName,
                CustomerPONo = item.PONo,
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