using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class UserPermissionService : BusinessService
    {
        private Services.Entities.SysUserFunctionService UserFunction { get; }
        public UserPermissionService(
            Services.Entities.SysUserFunctionService userFunctionService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.UserFunction = userFunctionService;
        }
        public IQueryable<Models.Views.UserPermission> Get()
        {
            return UserFunction.Get().Select(i => new Models.Views.UserPermission
            {
                Id = i.Id,
                UserId = i.UserId,
                FunctionId = i.FunctionId,
                LastUpdateTime = i.LastUpdateTime,
                ModifyUserName = i.ModifyUserName,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.UserPermission> item)
        {
            UserFunction.CreateRange(BuildRange(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.SysUserFunction, bool>> predicate)
        {
            UserFunction.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.SysUserFunction> BuildRange(IEnumerable<Models.Views.UserPermission> items)
        {
            return items.Select(item => new ERP.Models.Entities.SysUserFunction
            {
                Id = item.Id,
                UserId = item.UserId,
                FunctionId = item.FunctionId,
                LastUpdateTime = item.LastUpdateTime,
                ModifyUserName = item.ModifyUserName,
            });
        }
    }
}