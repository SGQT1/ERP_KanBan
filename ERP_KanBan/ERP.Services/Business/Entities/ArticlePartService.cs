using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ArticlePartService : BusinessService
    {
        private Services.Entities.ArticlePartService ArticlePart { get; }

        public ArticlePartService(
            Services.Entities.ArticlePartService articlePartService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.ArticlePart = articlePartService;
        }
        public IQueryable<Models.Views.ArticlePart> Get()
        {
            return ArticlePart.Get().Select(i => new Models.Views.ArticlePart
            {
                Id = i.Id,
                ArticleId = i.ArticleId,
                Division = i.Division,
                DivisionOther = i.DivisionOther,
                PartId = i.PartId,
                StandardUsage = i.StandardUsage,
                UnitCodeId = i.UnitCodeId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                AlternateType = i.AlternateType,
                PartPhotoURL = i.PartPhotoURL,
                KnifeNo = i.KnifeNo,
                PieceOfPair = i.PieceOfPair,
            });
        }
    }
}