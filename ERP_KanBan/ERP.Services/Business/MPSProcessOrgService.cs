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
    public class MPSProcessOrgService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcessService MPSProcess { get; set; }
        private ERP.Services.Business.Entities.MPSProcessOrgService MPSProcessOrg { get; set; }

        public MPSProcessOrgService(
            ERP.Services.Business.Entities.MPSProcessService mpsProcessService,
            ERP.Services.Business.Entities.MPSProcessOrgService mpsProcessOrgService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcess = mpsProcessService;
            MPSProcessOrg = mpsProcessOrgService;
        }
        public IQueryable<Models.Views.MPSProcess> GetMPSProcess()
        {
            return MPSProcess.Get();
        }
        public ERP.Models.Views.MPSProcessOrgGroup Get(int id, int localeId)
        {
            var group = new MPSProcessOrgGroup();
            var process = MPSProcess.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();

            if (process != null)
            {
                group.MPSProcess = process;
                group.MPSProcessOrg = MPSProcessOrg.Get().Where(i => i.ProcessId == id && i.LocaleId == localeId).ToList();
            }

            return group;
        }
        public ERP.Models.Views.MPSProcessOrgGroup Save(MPSProcessOrgGroup item)
        {
            var process = item.MPSProcess;
            var orgItems = item.MPSProcessOrg;
            UnitOfWork.BeginTransaction();
            try
            {
                MPSProcessOrg.RemoveRange(i => i.ProcessId == process.Id && i.LocaleId == process.LocaleId);
                MPSProcessOrg.CreateRange(orgItems);
                UnitOfWork.Commit();

                return Get((int)process.Id, (int)process.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(MPSProcessOrgGroup item)
        {
            var process = item.MPSProcess;
            var orgItems = item.MPSProcessOrg;
            UnitOfWork.BeginTransaction();
            try
            {
                MPSProcessOrg.RemoveRange(i => i.ProcessId == process.Id && i.LocaleId == process.LocaleId);
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
