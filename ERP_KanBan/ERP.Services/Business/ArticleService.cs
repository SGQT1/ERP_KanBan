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

namespace ERP.Services.Business
{
    public class ArticleService : BusinessService
    {
        private ERP.Services.Business.Entities.ArticleService Article { get; set; }
        private ERP.Services.Business.Entities.StyleService Style { get; set; }

        public ArticleService(
            ERP.Services.Business.Entities.ArticleService articleService,
            ERP.Services.Business.Entities.StyleService styleService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            Style = styleService;
        }

        public ERP.Models.Views.Article Get(int id, int localeId)
        {
            var hasStyle = Style.Get().Where(i => i.LocaleId == localeId && i.ArticleId == id).Any();
            var article = Article.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();

            // article was be used
            article.IsUse = hasStyle ? 1 : 0;
            return article;
        }
        public ERP.Models.Views.Article Save(ERP.Models.Views.Article item)
        {
            if (item != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Knife
                    {
                        var _article = Article.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                        if (_article != null)
                        {
                            item.Id = _article.Id;
                            item.LocaleId = _article.LocaleId;
                            item = Article.Update(item);
                        }
                        else
                        {
                            item = Article.Create(item);
                        }
                    }

                    UnitOfWork.Commit();

                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }

            return Get((int)item.Id, (int)item.LocaleId);
        }

        public void Remove(ERP.Models.Views.Article item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Article.Remove(item);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public IEnumerable<ERP.Models.Views.Article> GetArticleNo(int brandId, string articleNo, int localeId)
        {
            return Article.Get().Where(i => i.BrandCodeId == brandId && i.ArticleNo.ToLower() == articleNo.ToLower() && i.LocaleId == localeId).ToList();
        }
    }
}
