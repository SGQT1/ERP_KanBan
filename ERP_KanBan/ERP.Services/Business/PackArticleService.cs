using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    /*
     * PackArticle is article pack infor consolidate multi tabele : packMapping, packMappingItem1, packMappingItem2
     */
    public class PackArticleService : BusinessService
    {
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.PackArticleService PackArticle { get; set; }
        private ERP.Services.Business.Entities.PackArticleItemService PackArticleItem { get; set; }
        private ERP.Services.Business.Entities.PackMappingItem1Service PackMappingItem1 { get; set; }
        private ERP.Services.Business.Entities.PackMappingItem2Service PackMappingItem2 { get; set; }
        public PackArticleService(
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.PackArticleService packArticleService,
            ERP.Services.Business.Entities.PackArticleItemService packArticleItemService,
            ERP.Services.Business.Entities.PackMappingItem1Service packMappingItem1Service,
            ERP.Services.Business.Entities.PackMappingItem2Service packMappingItem2Service,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            CodeItem = codeItemService;
            PackArticle = packArticleService;
            PackArticleItem = packArticleItemService;
            PackMappingItem1 = packMappingItem1Service;
            PackMappingItem2 = packMappingItem2Service;
        }

        public Models.Views.PackArticleGroup GetPackArticleGroup(int articleId, int localeId)
        {
            var packArticle = PackArticle.Get()
                .Where(i => i.ArticleId == articleId && i.LocaleId == localeId)
                .FirstOrDefault();
            if (packArticle != null && packArticle.Id != 0)
            {
                var packArticlItems = PackArticleItem.Get((int)packArticle.Id, localeId);
                return new PackArticleGroup
                {
                    PackArticle = Build(packArticle),
                    PackArticleItem = packArticlItems,
                };
            }
            else
            {
                return new PackArticleGroup();
            }
        }
        public Models.Views.PackArticleGroup SavePackArticleGroup(PackArticleGroup packArticleGroup)
        {
            var packArticle = packArticleGroup.PackArticle;
            var packArticlItems = packArticleGroup.PackArticleItem.ToList();

            try
            {
                UnitOfWork.BeginTransaction();
                if (packArticle != null)
                {

                    //PackMapping
                    var _packArticle = PackArticle.Get().Where(i => i.ArticleNo == packArticle.ArticleNo && i.LocaleId == packArticle.LocaleId).FirstOrDefault();
                    if (_packArticle == null)
                    {
                        packArticle = PackArticle.Create(packArticle);
                    }
                    else
                    {
                        packArticle.Id = _packArticle.Id;
                        packArticle = PackArticle.Update(packArticle);
                    }

                    //PackMappingItem1 & 2
                    if (packArticle.Id != 0)
                    {
                        packArticlItems.ForEach(i => i.PackMappingId = packArticle.Id);
                        //PackMappingItem1 & 24(create,update) is remove OrdersItem and Insert.
                        PackArticleItem.RemoveRange((int)packArticle.Id, (int)packArticle.LocaleId);
                        PackArticleItem.CreateRange(packArticlItems);
                    }
                }
                UnitOfWork.Commit();
                return this.GetPackArticleGroup((int)packArticle.ArticleId, (int)packArticle.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        private Models.Views.PackArticle Build(Models.Views.PackArticle packArticle)
        {

            packArticle.SizeCountryCodeId = CodeItem.Get().Where(i =>
                i.CodeType.CompareTo("35") == 0 &&
                i.NameTW.CompareTo(packArticle.SizeCountryNameTw) == 0 &&
                i.LocaleId == packArticle.LocaleId
            ).FirstOrDefault().Id;

            packArticle.WeightUnitCodeId = Convert.ToDecimal(packArticle.WeightUnitNameTw);
            packArticle.WeightUnitNameTw = CodeItem.Get().Where(i =>
                i.CodeType.CompareTo("21") == 0 &&
                i.Id == packArticle.WeightUnitCodeId &&
                i.LocaleId == packArticle.LocaleId
            ).FirstOrDefault().NameTW;

            return packArticle;
        }
    }
}
