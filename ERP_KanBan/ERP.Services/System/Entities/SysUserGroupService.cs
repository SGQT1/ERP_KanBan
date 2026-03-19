using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SysUserGroupService : EntityService<SysUserGroup>
    {
        protected new SysUserGroupRepository Repository { get { return base.Repository as SysUserGroupRepository; } }

        public SysUserGroupService(SysUserGroupRepository repository) : base(repository)
        {
        }
    }
}