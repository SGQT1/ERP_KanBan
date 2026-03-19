using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SysUserFunctionService : EntityService<SysUserFunction>
    {
        protected new SysUserFunctionRepository Repository { get { return base.Repository as SysUserFunctionRepository; } }

        public SysUserFunctionService(SysUserFunctionRepository repository) : base(repository)
        {
        }
    }
}