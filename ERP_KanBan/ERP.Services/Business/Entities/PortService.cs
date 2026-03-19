using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PortService : BusinessService
    {
        private Services.Entities.PortService Port { get; }

        public PortService(Services.Entities.PortService portService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.Port = portService;
        }
        public IQueryable<Models.Views.Port> Get()
        {
            return Port.Get().Select(i => new Models.Views.Port
            {
                Id = i.Id,
                PortNo = Convert.ToInt32(i.PortNo),
                PortName = i.PortName,
                PortVarietyCodeId = i.PortVarietyCodeId,
                PortNameEng = i.PortNameEng,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
            });
        }
        public Models.Views.Port Create(Models.Views.Port item)
        {
            var _item = Port.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Port Update(Models.Views.Port item)
        {
            var _item = Port.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Port item)
        {
            Port.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Port Build(Models.Views.Port item)
        {
            return new Models.Entities.Port()
            {
                Id = item.Id,
                PortNo = item.PortNo.ToString(),
                PortName = item.PortName,
                PortVarietyCodeId = item.PortVarietyCodeId,
                PortNameEng = item.PortNameEng,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            };
        }
        
    }
}