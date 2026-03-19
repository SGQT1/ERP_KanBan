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

    public class MPSProcessService : BusinessService
    {
        private ERP.Services.Entities.MpsProcessService MPSProcess { get; set; }

        public MPSProcessService(
            ERP.Services.Entities.MpsProcessService mpsProcessService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcess = mpsProcessService;
        }

        public IQueryable<Models.Views.MPSProcess> Get()
        {
            return this.MPSProcess.Get().Select(i => new Models.Views.MPSProcess
            {
                Id = i.Id,
                LocaleId = i.LocaleId,

                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                ProcessNo = i.ProcessNo,
                ProcessNameTw = i.ProcessNameTw,
                ProcessNameEn = i.ProcessNameEn,
                LeadInDays = i.LeadInDays,
                SortKey = i.SortKey,
                RelateCapacity = i.RelateCapacity,
                RelateMaterial = i.RelateMaterial,
                DisplayProcess = i.ProcessNo + "-" + i.ProcessNameTw,
            });
        }
        public Models.Views.MPSProcess Create(Models.Views.MPSProcess item)
        {
            var _item = MPSProcess.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcess Update(Models.Views.MPSProcess item)
        {
            var _item = MPSProcess.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcess item)
        {
            MPSProcess.Remove(Build(item));
        }
        private Models.Entities.MpsProcess Build(Models.Views.MPSProcess item)
        {
            return new Models.Entities.MpsProcess()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                ProcessNo = item.ProcessNo,
                ProcessNameTw = item.ProcessNameTw,
                ProcessNameEn = item.ProcessNameEn,
                LeadInDays = item.LeadInDays,
                SortKey = item.SortKey,
                RelateCapacity = item.RelateCapacity,
                RelateMaterial = item.RelateMaterial,
            };
        }
    }
}
