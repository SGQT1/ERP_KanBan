using System;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackArticleService : BusinessService
    {
        private Services.Entities.ArticleService Article { get; }

        private Services.Entities.PackMappingService PackMapping { get; }

        public PackArticleService(
            Services.Entities.ArticleService articleService,
            Services.Entities.PackMappingService packMappingService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            PackMapping = packMappingService;
        }
        public IQueryable<Models.Views.PackArticle> Get()
        {
            var parkArticle = (
                from p in PackMapping.Get()
                join a in Article.Get() on new { ArticleNo = p.ArticleNo.Trim(), LocaleId = p.LocaleId } equals new { ArticleNo = a.ArticleNo.Trim(), LocaleId = a.LocaleId }
                select new Models.Views.PackArticle
                {
                    Id = p.Id,
                    LocaleId = p.LocaleId,
                    ArticleNo = p.ArticleNo,
                    ShoeName = p.ShoeName,
                    SizeCountryNameTw = p.SizeCountryNameTw,
                    WeightUnitNameTw = p.WeightUnitNameTw,
                    ModifyUserName = p.ModifyUserName,
                    LastUpdateTime = p.LastUpdateTime,

                    ArticleId = a.Id,
                    BrandCodeId = a.BrandCodeId,
                }
            );
            return parkArticle;
        }

        public Models.Views.PackArticle Create(Models.Views.PackArticle packArticle)
        {
            var _packArticle = PackMapping.Create(Build(packArticle));
            return Get().Where(i => i.Id == _packArticle.Id && i.LocaleId == _packArticle.LocaleId).FirstOrDefault();
        }
        public Models.Views.PackArticle Update(Models.Views.PackArticle packArticle)
        {
            var _packArticle = PackMapping.Update(Build(packArticle));
            return Get().Where(i => i.Id == _packArticle.Id && i.LocaleId == _packArticle.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.PackArticle packArticle)
        {
            PackMapping.Remove(Build(packArticle));
        }
        public Models.Entities.PackMapping Build(Models.Views.PackArticle item)
        {
            return new Models.Entities.PackMapping()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ArticleNo = item.ArticleNo,
                ShoeName = item.ShoeName,
                SizeCountryNameTw = item.SizeCountryNameTw,
                WeightUnitNameTw = item.WeightUnitCodeId.ToString(),
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
            };
        }
    }
}