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
    public class PackSpecService : BusinessService
    {
        private ERP.Services.Business.Entities.PackSpecService PackSpec { get; set; }
        public PackSpecService(
            ERP.Services.Business.Entities.PackSpecService packSpecService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackSpec = packSpecService;
        }

        public IQueryable<ERP.Models.Views.PackSpec> Get()
        {   
            return PackSpec.Get();
        }
        public ERP.Models.Views.PackSpec Create(PackSpec packSpec)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                var item = PackSpec.Create(packSpec);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.PackSpec Update(PackSpec packSpec)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                var item = PackSpec.Update(packSpec);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void UpdateSpec()
        {   
            PackSpec.UpdateSpec();
        }
        public void Remove(PackSpec packSpec)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                PackSpec.Remove(packSpec);
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
