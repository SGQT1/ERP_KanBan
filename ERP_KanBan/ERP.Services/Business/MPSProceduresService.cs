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
    public class MPSProceduresService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProceduresService Procedures { get; set; }

        public MPSProceduresService(
            ERP.Services.Business.Entities.MPSProceduresService proceduresService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Procedures = proceduresService;
        }

        public ERP.Models.Views.MPSProcedures Get(int id, int localeId)
        {
            return Procedures.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.MPSProcedures Save(MPSProcedures item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = Procedures.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = Procedures.Update(item);
                }
                else
                {
                    item = Procedures.Create(item);
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

        public void Remove(MPSProcedures item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Procedures.Remove(item);
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
