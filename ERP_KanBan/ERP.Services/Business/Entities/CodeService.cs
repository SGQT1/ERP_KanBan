using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class CodeService : BusinessService
    {
        private Services.Entities.CodeService Code { get; }

        public CodeService(Services.Entities.CodeService codeService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.Code = codeService;
        }
        public IQueryable<Models.Views.Code> Get()
        {
            return Code.Get().Select(i => new Models.Views.Code
            {
                Id = i.Id,
                CodeType = i.CodeType,
                TypeName = i.TypeName,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId
            });
        }
    }
}