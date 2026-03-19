using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business
{
    public class UserService : BusinessService
    {
        private ERP.Services.Business.Entities.UserService User { get; set; }
        private ERP.Services.Business.Entities.UserPermissionService UserPermission { get; set; }
        private ERP.Services.Business.Entities.UserRoleService UserRole { get; set; }

        public UserService(
            ERP.Services.Business.Entities.UserService userService,
            ERP.Services.Business.Entities.UserPermissionService userPermissionService,
            ERP.Services.Business.Entities.UserRoleService userRoleServicer,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            User = userService;
            UserPermission = userPermissionService;
            UserRole = userRoleServicer;
        }

        public ERP.Models.Views.UserGroup Get(string id)
        {
            var group = new ERP.Models.Views.UserGroup();

            var user = User.Get().Where(i => i.Id == id).FirstOrDefault();
            if(user != null) {
                user.SignPhoto = User.GetPhoto(user.Sign);

                var permission = UserPermission.Get().Where(i => i.UserId == id).Select(i => i.FunctionId);
                var rose = UserRole.Get().Where(i => i.UserId == id).Select(i => i.GroupId);

                group.User = user;
                group.Permission = permission;
                group.Role = rose;
            }
             
            return group;
        }
        public ERP.Models.Views.UserGroup Save(UserGroup item)
        {
            var user = item.User;
            var permission = item.Permission;
            var role = item.Role;
            try
            {
                UnitOfWork.BeginTransaction();

                var _user = User.Get().Where(i => i.Id == user.Id).ToList();
                if (_user.Any())
                {
                    user = User.Update(user);
                }
                else
                {
                    user = User.Create(user);
                }
                UserPermission.RemoveRange(i => i.UserId == user.Id);
                UserPermission.CreateRange(permission.Select(i => new ERP.Models.Views.UserPermission
                {
                    UserId = user.Id,
                    FunctionId = i,
                    LastUpdateTime = DateTime.Now,
                    ModifyUserName = "Admin",
                }));

                UserRole.RemoveRange(i => i.UserId == user.Id);
                UserRole.CreateRange(role.Select(i => new ERP.Models.Views.UserRole
                {
                    UserId = user.Id,
                    GroupId = i,
                    LastUpdateTime = DateTime.Now,
                    ModifyUserName = "Admin",
                }));

                UnitOfWork.Commit();
                return Get(user.Id);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(UserGroup item)
        {
            var user = item.User;
            UnitOfWork.BeginTransaction();
            try
            {
                if (user != null && user.Id.Length > 0)
                {
                    UserPermission.RemoveRange(i => i.UserId == user.Id);
                    UserRole.RemoveRange(i => i.UserId == user.Id);

                    User.Remove(user);
                    UnitOfWork.Commit();
                }
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public IEnumerable<string> GetUserPermission(string id)
        {
            return UserPermission.Get().Where(i => i.UserId == id).Select(i => i.FunctionId);
        }
    }
}