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

    public class MPSProceduresService : BusinessService
    {
        private ERP.Services.Entities.ProceduresService MPSProcedures { get; set; }

        public MPSProceduresService(
            ERP.Services.Entities.ProceduresService mpsProceduresService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedures = mpsProceduresService;
        }

        public IQueryable<Models.Views.MPSProcedures> Get()
        {
            return this.MPSProcedures.Get().Select(i => new Models.Views.MPSProcedures
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ProcedureNo = i.ProcedureNo,
                ProcedureName = i.ProcedureName,
                ProcedureNameTw = i.ProcedureNameTw,
                ProcedureNameEn = i.ProcedureNameEn,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,

            });
        }
        public Models.Views.MPSProcedures Create(Models.Views.MPSProcedures item)
        {
            var _item = MPSProcedures.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcedures Update(Models.Views.MPSProcedures item)
        {
            var _item = MPSProcedures.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcedures item)
        {
            MPSProcedures.Remove(Build(item));
        }
        private Models.Entities.Procedures Build(Models.Views.MPSProcedures item)
        {
            return new Models.Entities.Procedures()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ProcedureNo = item.ProcedureNo,
                ProcedureName = item.ProcedureName,
                ProcedureNameTw = item.ProcedureNameTw,
                ProcedureNameEn = item.ProcedureNameEn,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    }
}
