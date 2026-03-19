using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ShellService : EntityService<Shell>
    {
        protected new ShellRepository Repository { get { return base.Repository as ShellRepository; } }

        public ShellService(ShellRepository repository) : base(repository)
        {
        }
    }
}