using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackMappingItem1Service : BusinessService
    {
        private Services.Entities.PackMappingItem1Service PackMappingItem1 { get; }

        public PackMappingItem1Service(Services.Entities.PackMappingItem1Service packMappingItem1Service, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.PackMappingItem1 = packMappingItem1Service;
        }
        public IQueryable<Models.Views.PackMappingItem1> Get()
        {
            return PackMappingItem1.Get().Select(i => new Models.Views.PackMappingItem1
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                PackMappingId = i.PackMappingId,
                ArticleSize = i.ArticleSize,
                ArticleSizeSuffix = i.ArticleSizeSuffix,
                ArticleInnerSize = i.ArticleInnerSize,
                GWOfCTN = i.GWOfCTN,
                NWOfCTN = i.NWOfCTN,
                MEAS = i.MEAS,
                GWOfCTNCLB = i.GWOfCTNCLB,
                MEASCLB = i.MEASCLB,
            });
        }
    }
}