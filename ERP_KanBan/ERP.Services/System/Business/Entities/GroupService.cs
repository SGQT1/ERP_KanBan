using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities {
    public class GroupService : BusinessService {
        private Services.Entities.SysGroupService SysGroup { get; }
        private Services.Entities.SysUserGroupService SysUserGroup { get; }

        public GroupService(
            Services.Entities.SysGroupService sysGroupService,
            Services.Entities.SysUserGroupService sysUserGroupService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork) {
            SysGroup = sysGroupService;
            SysUserGroup = sysUserGroupService;
        }
        public IQueryable<Models.Views.Group> Get() {
            return SysGroup.Get().Select(i => new Models.Views.Group {
                Id = i.Id,
                ParentGroupId = i.ParentGroupId,
                Name = i.Name,
                Validate = i.Validate,
                ShortName = i.ShortName,
                LastUpdateTime = i.LastUpdateTime,
                ModifyUserName = i.ModifyUserName,
            });
        }
        public List<Models.Views.Group> GetByUser(string userId) {
            var groups = (
                from g in SysGroup.Get() 
                join ug in SysUserGroup.Get() on g.Id equals ug.GroupId 
                where ug.UserId == userId && g.Validate == 
                true select new Models.Views.Group {
                    Id = g.Id,
                    ParentGroupId = g.ParentGroupId,
                    Name = g.Name,
                    ShortName = g.ShortName,
                    Validate = g.Validate,
                    GroupCode = g.GroupCode,
                    LastUpdateTime = g.LastUpdateTime,
                    ModifyUserName = g.ModifyUserName,
                }
            ).ToList();
            return groups;
        }
        public Models.Views.Group Create(Models.Views.Group item) {
            var _item = SysGroup.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public Models.Views.Group Update(Models.Views.Group item) {
            var _item = SysGroup.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.Group item) {
            SysGroup.Remove(Build(item));
        }
        private Models.Entities.SysGroup Build(Models.Views.Group item) {
            return new Models.Entities.SysGroup() {
                Id = item.Id,
                ParentGroupId = item.ParentGroupId,
                Name = item.Name,
                ShortName = item.ShortName,
                Validate = item.Validate,
                GroupCode = item.GroupCode,
                LastUpdateTime = item.LastUpdateTime,
                ModifyUserName = item.ModifyUserName,
            };
        }
    }
}