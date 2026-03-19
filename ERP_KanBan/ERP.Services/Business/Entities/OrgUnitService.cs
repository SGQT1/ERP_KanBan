using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrgUnitService : BusinessService
    {
        private Services.Entities.OrgUnitService OrgUnit { get; }

        public OrgUnitService(
            Services.Entities.OrgUnitService orgUnitService, 
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.OrgUnit = orgUnitService;
        }
        public IQueryable<Models.Views.OrgUnit> Get()
        {
            return OrgUnit.Get().Select(i => new Models.Views.OrgUnit
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                UnitNo = i.UnitNo,
                UnitNameTw = i.UnitNameTw,
                UnitNameEn = i.UnitNameEn,
                FoundingDate = i.FoundingDate,
                ManagerStaffId = i.ManagerStaffId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                ParentId = i.ParentId,
            });
        }
        public Models.Views.OrgUnit Create(Models.Views.OrgUnit item)
        {
            var _item = OrgUnit.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.OrgUnit Update(Models.Views.OrgUnit item)
        {
            var _item = OrgUnit.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.OrgUnit item)
        {
            OrgUnit.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.OrgUnit Build(Models.Views.OrgUnit item)
        {
            return new Models.Entities.OrgUnit()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                UnitNo = item.UnitNo,
                UnitNameTw = item.UnitNameTw,
                UnitNameEn = item.UnitNameEn,
                FoundingDate = item.FoundingDate,
                ManagerStaffId = item.ManagerStaffId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                ParentId = item.ParentId,
            };
        }
        
    }
}