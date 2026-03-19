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
    public class MPSProcessSetService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcessSetService MPSProcessSet { get; set; }

        public MPSProcessSetService(
            ERP.Services.Business.Entities.MPSProcessSetService mpsProcessSetService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcessSet = mpsProcessSetService;
        }
        public IQueryable<Models.Views.MPSProcessSet> GetMPSProcessSet()
        {
            return MPSProcessSet.Get();
        }
        public ERP.Models.Views.MPSProcessSet Get(int id, int localeId)
        {
            return MPSProcessSet.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();;
        }
        public ERP.Models.Views.MPSProcessSet Save(MPSProcessSet item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = MPSProcessSet.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = MPSProcessSet.Update(item);
                }
                else
                {
                    item = MPSProcessSet.Create(item);
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

        public void Remove(MPSProcessSet item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                MPSProcessSet.Remove(item);
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
