using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersPLService : BusinessService
    {
        private Services.Entities.OrdersPLService OrdersPL { get; }

        public OrdersPLService(Services.Entities.OrdersPLService ordersPLService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.OrdersPL = ordersPLService;
        }
        public IQueryable<Models.Views.OrdersPL> Get()
        {
            return OrdersPL.Get().Select(i => new Models.Views.OrdersPL
            {   
                Id =i.Id,
                LocaleId =i.LocaleId,
                RefLocaleId =i.RefLocaleId,
                RefOrdersId =i.RefOrdersId,
                OrderNo =i.OrderNo,
                Edition =i.Edition,
                SizeCountryNameTw =i.SizeCountryNameTw,
                MappingSizeCountryNameTw =i.MappingSizeCountryNameTw,
                PackingQty =i.PackingQty,
                PackingType =i.PackingType,
                PackingTypeDesc =i.PackingTypeDesc,
                ModifyUserName =i.ModifyUserName,
                LastUpdateTime =i.LastUpdateTime,
                CNoFrom =i.CNoFrom,
            });
        }
    }
}