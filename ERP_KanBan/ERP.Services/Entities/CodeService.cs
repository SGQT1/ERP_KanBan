using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class CodeService : EntityService<Code>
    {
        protected new CodeRepository Repository { get { return base.Repository as CodeRepository; } }

        public CodeService(CodeRepository repository) : base(repository)
        {
        }
    }
}