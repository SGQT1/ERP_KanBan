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
    public class ArticleSizeRunService : BusinessService
    {
        private ERP.Services.Business.Entities.ArticleService Article { get; set; }
        private ERP.Services.Business.Entities.ArticleSizeRunService ArticleSizeRun { get; set; }

        public ArticleSizeRunService(
            ERP.Services.Business.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Business.Entities.ArticleService articleService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            ArticleSizeRun = articleSizeRunService;
        }

        public IEnumerable<ERP.Models.Views.ArticleSizeRun> GetByArticle(int articleId, int localeId)
        {
            return ArticleSizeRun.Get().Where(i => i.ArticleId == articleId && i.LocaleId == localeId).OrderBy(i => i.ArticleInnerSize).ToList();
        }
        public ERP.Models.Views.ArticleSizeRunGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.ArticleSizeRunGroup();

            var article = Article.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (article != null)
            {
                group.Article = article;
                group.ArticleSizeRun = ArticleSizeRun.Get().Where(i => i.ArticleId == id && i.LocaleId == localeId).ToList();
            }
            return group;
        }
        public IEnumerable<ERP.Models.Views.ArticleSizeRun> BuildArticleSizeRun(int articleId, int localeId)
        {
            var result = ArticleSizeRun.Get().Where(i => i.ArticleId == articleId && i.LocaleId == localeId)
                .Select(i => new ERP.Models.Views.ArticleSizeRun
                {
                    Id = 0,
                    ArticleId = 0,
                    ArticleSize = i.ArticleSize,
                    ArticleSizeSuffix = i.ArticleSizeSuffix,
                    ArticleInnerSize = i.ArticleInnerSize,
                    KnifeSize = i.KnifeSize,
                    KnifeSizeSuffix = i.KnifeSizeSuffix,
                    KnifeInnerSize = i.KnifeInnerSize,
                    KnifeDisplaySize = i.KnifeDisplaySize,
                    OutsoleSize = i.OutsoleSize,
                    OutsoleSizeSuffix = i.OutsoleSizeSuffix,
                    OutsoleInnerSize = i.OutsoleInnerSize,
                    OutsoleDisplaySize = i.OutsoleDisplaySize,
                    LastSize = i.LastSize,
                    LastSizeSuffix = i.LastSizeSuffix,
                    LastInnerSize = i.LastInnerSize,
                    LastDisplaySize = i.LastDisplaySize,
                    ShellSize = i.ShellSize,
                    ShellSizeSuffix = i.ShellSizeSuffix,
                    ShellInnerSize = i.ShellInnerSize,
                    Other1Size = i.Other1Size,
                    Other1SizeSuffix = i.Other1SizeSuffix,
                    Other1InnerSize = i.Other1InnerSize,
                    Other1Desc = i.Other1Desc,
                    Other2Size = i.Other2Size,
                    Other2SizeSuffix = i.Other2SizeSuffix,
                    Other2InnerSize = i.Other2InnerSize,
                    Other2SizeDesc = i.Other2SizeDesc,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    LocaleId = 0,
                    ArticleDisplaySize = i.ArticleDisplaySize,
                    ShellDisplaySize = i.ShellDisplaySize,
                })
                .OrderBy(i => i.ArticleInnerSize)
                .ToList();

            return result;
        }
        public ERP.Models.Views.ArticleSizeRunGroup Save(ArticleSizeRunGroup group)
        {
            var article = group.Article;
            var articleSizeRun = group.ArticleSizeRun.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (article != null)
                {
                    var _article = Article.Get().Where(i => i.LocaleId == article.LocaleId && i.Id == article.Id).FirstOrDefault();

                    articleSizeRun.ForEach(i =>
                    {
                        i.ArticleId = _article.Id;
                        i.LocaleId = _article.LocaleId;
                    });
                    ArticleSizeRun.RemoveRange(i => i.ArticleId == _article.Id && i.LocaleId == _article.LocaleId);
                    ArticleSizeRun.CreateRange(articleSizeRun);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)article.Id, (int)article.LocaleId);
        }
        public void Remove(ArticleSizeRunGroup group)
        {
            var article = group.Article;
            try
            {
                UnitOfWork.BeginTransaction();
                if (article != null)
                {
                    ArticleSizeRun.RemoveRange(i => i.ArticleId == article.Id && i.LocaleId == article.LocaleId);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

    }
}
