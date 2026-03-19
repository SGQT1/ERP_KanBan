using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class LabelArticleService : BusinessService
    {
        private ERP.Services.Business.Entities.ArticleService Article { get; set; }
        private ERP.Services.Business.Entities.LabelArticleService LabelArticle { get; set; }

        public LabelArticleService(
            ERP.Services.Business.Entities.ArticleService articleService,
            ERP.Services.Business.Entities.LabelArticleService labelStyleService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            LabelArticle = labelStyleService;
        }

        public ERP.Models.Views.LabelArticleGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.LabelArticleGroup { };
            var article = Article.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            var labelArticle = LabelArticle.Get().Where(i => i.ArticleId == id && i.LocaleId == localeId).FirstOrDefault();
            if (labelArticle != null)
            {
                labelArticle.LabelArticle01Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle01PhotoURL);
                labelArticle.LabelArticle02Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle02PhotoURL);
                labelArticle.LabelArticle03Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle03PhotoURL);
                labelArticle.LabelArticle04Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle04PhotoURL);
                labelArticle.LabelArticle05Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle05PhotoURL);
                labelArticle.LabelArticle06Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle06PhotoURL);
                labelArticle.LabelArticle07Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle07PhotoURL);
                labelArticle.LabelArticle08Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle08PhotoURL);
                labelArticle.LabelArticle09Photo = LabelArticle.GetPhoto(labelArticle.LabelArticle09PhotoURL);
            }

            if (article != null)
            {
                group.Article = article;
                group.LabelArticle = labelArticle == null ? new Models.Views.LabelArticle() : labelArticle;
            }
            return group;
        }

        public ERP.Models.Views.LabelArticleGroup Save(ERP.Models.Views.LabelArticleGroup item)
        {
            var article = item.Article;
            var labelArticle = item.LabelArticle;

            UnitOfWork.BeginTransaction();
            try
            {

                // Id >> exist, ChineseName >> duplicate
                var _item = LabelArticle.Get().Where(i => i.LocaleId == labelArticle.LocaleId && i.Id == labelArticle.Id && i.ArticleId == article.Id).FirstOrDefault();

                if (_item != null)
                {
                    labelArticle.Id = _item.Id;
                    labelArticle.ArticleId = _item.ArticleId;
                    labelArticle.LocaleId = _item.LocaleId;

                    labelArticle = LabelArticle.Update(labelArticle);
                }
                else
                {
                    labelArticle = LabelArticle.Create(labelArticle);
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                return item;
            }
            return Get((int)article.Id, (int)article.LocaleId);
        }
        public void Remove(ERP.Models.Views.LabelArticleGroup item)
        {
            var article = item.Article;

            UnitOfWork.BeginTransaction();
            try
            {
                LabelArticle.RemoveRange(i => i.ArticleId == article.Id && i.LocaleId == article.LocaleId);
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }
    }
}