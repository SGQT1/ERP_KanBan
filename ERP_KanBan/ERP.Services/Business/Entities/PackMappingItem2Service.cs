using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackMappingItem2Service : BusinessService
    {
        private Services.Entities.PackMappingItem2Service PackMappingItem2 { get; }

        public PackMappingItem2Service(Services.Entities.PackMappingItem2Service packMappingItem2Service, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.PackMappingItem2 = packMappingItem2Service;
        }
        public IQueryable<Models.Views.PackMappingItem2> Get()
        {
            return PackMappingItem2.Get().Select(i => new Models.Views.PackMappingItem2
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                PackMappingId = i.PackMappingId,
                Type = i.Type,
                BeginArticleSize = i.BeginArticleSize,
                BeginArticleSizeSuffix = i.BeginArticleSizeSuffix,
                BeginArticleInnerSize = i.BeginArticleInnerSize,
                EndArticleSize = i.EndArticleSize,
                EndArticleSizeSuffix = i.EndArticleSizeSuffix,
                EndArticleInnerSize = i.EndArticleInnerSize,
                Spec = i.Spec,
                SpecCLB = i.SpecCLB,
            });
        }
    }
}