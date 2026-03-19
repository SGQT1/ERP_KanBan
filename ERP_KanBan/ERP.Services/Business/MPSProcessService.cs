using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSProcessService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcessService MPSProcess { get; set; }

        public MPSProcessService(
            ERP.Services.Business.Entities.MPSProcessService mpsProcessService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcess = mpsProcessService;
        }

        public ERP.Models.Views.MPSProcess Get(int id, int localeId)
        {
            return MPSProcess.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.MPSProcess Save(MPSProcess item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = MPSProcess.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = MPSProcess.Update(item);
                }
                else
                {
                    item = MPSProcess.Create(item);
                }

                UnitOfWork.Commit();
                return Get((int)item.Id, (int)item.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(MPSProcess item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                MPSProcess.Remove(item);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
