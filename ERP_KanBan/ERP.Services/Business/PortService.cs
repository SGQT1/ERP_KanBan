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
    public class PortService : BusinessService
    {
        private ERP.Services.Business.Entities.PortService Port { get; set; }
        public PortService(
            ERP.Services.Business.Entities.PortService portService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Port = portService;
        }

        public IQueryable<ERP.Models.Views.Port> GetPort()
        {   
            return Port.Get();
        }
        public ERP.Models.Views.Port CreatePort(Port item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                item = Port.Create(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.Port UpdatePort(Port item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                item = Port.Update(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void RemovePort(Port item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                Port.Remove(item);
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
