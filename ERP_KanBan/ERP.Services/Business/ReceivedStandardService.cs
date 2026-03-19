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
    public class ReceivedStandardService : BusinessService
    {
        private ERP.Services.Business.Entities.ReceivedStandardService ReceivedStandard { get; set; }
        public ReceivedStandardService(
            ERP.Services.Business.Entities.ReceivedStandardService receivedStandardService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            ReceivedStandard = receivedStandardService;
        }

        public IQueryable<ERP.Models.Views.ReceivedStandard> Get()
        {   
            return ReceivedStandard.Get();
        }
        public ERP.Models.Views.ReceivedStandard Create(ReceivedStandard item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                item = ReceivedStandard.Create(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.ReceivedStandard Update(ReceivedStandard item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                item = ReceivedStandard.Update(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(ReceivedStandard item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                ReceivedStandard.Remove(item);
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
