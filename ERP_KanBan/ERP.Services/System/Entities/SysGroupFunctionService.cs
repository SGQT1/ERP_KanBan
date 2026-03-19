using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class SysGroupFunctionService : EntityService<SysGroupFunction>
    {
        protected new SysGroupFunctionRepository Repository { get { return base.Repository as SysGroupFunctionRepository; } }

        public SysGroupFunctionService(SysGroupFunctionRepository repository) : base(repository)
        {
        }
    }
}