using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business
{
    public class PermissionService : BusinessService
    {
        private ERP.Services.Business.Entities.UserPermissionService UserPermission { get; set; }

        public PermissionService(
            ERP.Services.Business.Entities.UserPermissionService userPermissionService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            UserPermission = userPermissionService;
        }

        public IQueryable<ERP.Models.Views.UserPermission> GetUserByFunction(string FunctionId)
        {
            return UserPermission.Get().Where(i => i.FunctionId == FunctionId);
        }

        public IQueryable<ERP.Models.Views.UserPermission> SaveUserByFunction(List<UserPermission> items)
        {
            var funcId = items.Max(i => i.FunctionId);
            try
            {
                UserPermission.RemoveRange(i => i.FunctionId == funcId);
                UserPermission.CreateRange(items);

                UnitOfWork.Commit();
                return GetUserByFunction(funcId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }


    }
}