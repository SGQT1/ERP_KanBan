using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackMappingService : BusinessService
    {
        private Services.Entities.PackMappingService PackMapping { get; }

        public PackMappingService(Services.Entities.PackMappingService packMappingService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.PackMapping = packMappingService;
        }
        public IQueryable<Models.Views.PackMapping> Get()
        {
            return PackMapping.Get().Select(i => new Models.Views.PackMapping
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ArticleNo = i.ArticleNo,
                ShoeName = i.ShoeName,
                SizeCountryNameTw = i.SizeCountryNameTw,
                WeightUnitNameTw = i.WeightUnitNameTw,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
    }
}