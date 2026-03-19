using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class OrgUnitService : BusinessService
    {
        private ERP.Services.Business.Entities.OrgUnitService OrgUnit { get; set; }
        public OrgUnitService(
            ERP.Services.Business.Entities.OrgUnitService orgUnitService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OrgUnit = orgUnitService;
        }

        public ERP.Models.Views.OrgUnit Get(int id, int localeId)
        {   
            return OrgUnit.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.OrgUnit Save(OrgUnit item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = OrgUnit.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = OrgUnit.Update(item);
                }
                else
                {
                    item = OrgUnit.Create(item);
                }

                UnitOfWork.Commit();
                return Get((int)item.Id, (int)item.LocaleId);
            }
            catch(Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(OrgUnit item)
        {   
            UnitOfWork.BeginTransaction();
            try
            { 
                OrgUnit.Remove(item);
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
