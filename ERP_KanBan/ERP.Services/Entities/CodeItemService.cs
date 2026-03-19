using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class CodeItemService : EntityService<CodeItem>
    {
        protected new CodeItemRepository Repository { get { return base.Repository as CodeItemRepository; } }

        public CodeItemService(CodeItemRepository repository) : base(repository)
        {
        }
    }
}