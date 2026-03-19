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
    public class UserRoleService : BusinessService
    {
        private Services.Entities.SysUserGroupService UserRole { get; }

        public UserRoleService(
            Services.Entities.SysUserGroupService userRoleService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.UserRole = userRoleService;
        }
        public IQueryable<Models.Views.UserRole> Get()
        {
            return UserRole.Get().Select(i => new Models.Views.UserRole
            {
                Id = i.Id,
                UserId = i.UserId,
                GroupId = i.GroupId,
                LastUpdateTime = i.LastUpdateTime,
                ModifyUserName = i.ModifyUserName,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.UserRole> item)
        {
            UserRole.CreateRange(BuildRange(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.SysUserGroup, bool>> predicate)
        {
            UserRole.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.SysUserGroup> BuildRange(IEnumerable<Models.Views.UserRole> items)
        {
            return items.Select(item => new ERP.Models.Entities.SysUserGroup
            {
                Id = item.Id,
                UserId = item.UserId,
                GroupId = item.GroupId,
                LastUpdateTime = item.LastUpdateTime,
                ModifyUserName = item.ModifyUserName,
            });
        }
        
    }
}