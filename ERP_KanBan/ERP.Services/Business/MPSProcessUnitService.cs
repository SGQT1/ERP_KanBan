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
    public class MPSProcessUnitService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcessUnitService MPSProcessUnit { get; set; }

        public MPSProcessUnitService(
            ERP.Services.Business.Entities.MPSProcessUnitService mpsProcessUnitService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcessUnit = mpsProcessUnitService;
        }
        public IQueryable<Models.Views.MPSProcessUnit> GetMPSProcessUnit()
        {
            return MPSProcessUnit.Get();
        }
        public ERP.Models.Views.MPSProcessUnit Get(int id, int localeId)
        {
            return MPSProcessUnit.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();;
        }
        public ERP.Models.Views.MPSProcessUnit Save(MPSProcessUnit item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = MPSProcessUnit.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = MPSProcessUnit.Update(item);
                }
                else
                {
                    item = MPSProcessUnit.Create(item);
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

        public void Remove(MPSProcessUnit item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                MPSProcessUnit.Remove(item);
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
