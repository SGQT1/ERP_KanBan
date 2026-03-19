using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSProcedureGroupService : BusinessService
    {
        private ERP.Services.Entities.MpsStyleService MPSStyle { get; set; }
        private ERP.Services.Entities.MpsProcedureGroupService MPSProcedureGroup { get; set; }

        public MPSProcedureGroupService(
            ERP.Services.Entities.MpsStyleService mpsStyleService,
            ERP.Services.Entities.MpsProcedureGroupService mpsProcedureGroupService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSStyle = mpsStyleService;
            MPSProcedureGroup = mpsProcedureGroupService;
        }

        public IQueryable<Models.Views.MPSProcedureGroup> Get()
        {
            return MPSProcedureGroup.Get().Select(i => new Models.Views.MPSProcedureGroup
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                StyleNo = i.StyleNo,
                GroupNameTw = i.GroupNameTw,
                GroupNameLocal = i.GroupNameLocal,
                GroupNameEn = i.GroupNameEn,
                DollarNameTw = i.DollarNameTw,
                UnitStandardTime = i.UnitStandardTime,
                UnitLaborCost = i.UnitLaborCost,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public List<string> GetLocal(int locale)
        {
            return MPSProcedureGroup.Get().Where(i => i.LocaleId == locale).Select(i => i.GroupNameLocal).Distinct().OrderBy(i => i).ToList();
        }
    
        public Models.Views.MPSProcedureGroup Create(Models.Views.MPSProcedureGroup item)
        {
            var _item = MPSProcedureGroup.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcedureGroup Update(Models.Views.MPSProcedureGroup item)
        {
            var _item = MPSProcedureGroup.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcedureGroup item)
        {
            MPSProcedureGroup.Remove(Build(item));
        }
        private Models.Entities.MpsProcedureGroup Build(Models.Views.MPSProcedureGroup item)
        {
            return new Models.Entities.MpsProcedureGroup()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                StyleNo = item.StyleNo,
                GroupNameTw = item.GroupNameTw,
                GroupNameLocal = item.GroupNameLocal,
                GroupNameEn = item.GroupNameEn,
                DollarNameTw = item.DollarNameTw,
                UnitStandardTime = item.UnitStandardTime,
                UnitLaborCost = item.UnitLaborCost,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    }
}
