using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SysGroupService : EntityService<SysGroup>
    {
        protected new SysGroupRepository Repository { get { return base.Repository as SysGroupRepository; } }

        public SysGroupService(SysGroupRepository repository) : base(repository)
        {
        }
    }
}