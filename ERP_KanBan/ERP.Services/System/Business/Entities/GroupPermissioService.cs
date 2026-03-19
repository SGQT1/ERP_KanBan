using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class GroupPermissionService : BusinessService
    {
        private Services.Entities.SysGroupFunctionService GroupFunction { get; }

        public GroupPermissionService(
            Services.Entities.SysGroupFunctionService groupFunctionService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.GroupFunction = groupFunctionService;
        }
        public IQueryable<Models.Views.GroupPermission> Get()
        {
            return GroupFunction.Get().Select(i => new Models.Views.GroupPermission
            {
                Id = i.Id,
                GroupId = i.GroupId,
                FunctionId = i.FunctionId,
                LastUpdateTime = i.LastUpdateTime,
                ModifyUserName = i.ModifyUserName,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.GroupPermission> item)
        {
            GroupFunction.CreateRange(BuildRange(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.SysGroupFunction, bool>> predicate)
        {
            GroupFunction.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.SysGroupFunction> BuildRange(IEnumerable<Models.Views.GroupPermission> items)
        {
            return items.Select(item => new ERP.Models.Entities.SysGroupFunction
            {
                Id = item.Id,
                GroupId = item.GroupId,
                FunctionId = item.FunctionId,
                LastUpdateTime = item.LastUpdateTime,
                ModifyUserName = item.ModifyUserName,
            });
        }
        
    }
}