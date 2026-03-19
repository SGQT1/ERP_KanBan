using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ArticleService : BusinessService
    {
        private Services.Entities.ArticleService Article { get; }
        private Services.Entities.OutsoleService Outsole { get; }
        private Services.Entities.LastService Last { get; }
        private Services.Entities.KnifeService Knife { get; }
        private Services.Entities.ShellService Shell { get; }

        private Services.Entities.CodeItemService CodeItem { get; }
        public ArticleService(
            Services.Entities.ArticleService articleService,
            Services.Entities.OutsoleService outsoleService,
            Services.Entities.LastService lastService,
            Services.Entities.KnifeService knifeService,
            Services.Entities.ShellService shellService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            Outsole = outsoleService;
            Last = lastService;
            Knife = knifeService;
            Shell = shellService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.Article> Get()
        {
            return Article.Get().Select(i => new Models.Views.Article
            {
                Id = i.Id,
                ArticleNo = i.ArticleNo,
                ArticleName = i.ArticleName,
                SizeRange = i.SizeRange,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                ProjectType = i.ProjectType,
                BrandCodeId = i.BrandCodeId,
                OutsoleId = i.OutsoleId,
                KnifeId = i.KnifeId,
                LastId = i.LastId,
                Gender = i.Gender,
                forCBD = i.forCBD,
                IsAlternate = i.IsAlternate,
                ShellId = i.ShellId,
                DayCapacity = i.DayCapacity,
                LastTurnover = i.LastTurnover,

                Brand = CodeItem.Get().Where(c => c.Id == i.BrandCodeId && c.LocaleId == i.LocaleId).Max(c => c.NameTW),
                OutsoleNo = Outsole.Get().Where(o => o.Id == i.OutsoleId && o.LocaleId == i.LocaleId).Max(o => o.OutsoleNo),
                KnifeNo = Knife.Get().Where(o => o.Id == i.KnifeId && o.LocaleId == i.LocaleId).Max(o => o.KnifeNo),
                LastNo = Last.Get().Where(o => o.Id == i.LastId && o.LocaleId == i.LocaleId).Max(o => o.LastNo),
                ShellNo = Shell.Get().Where(o => o.Id == i.ShellId && o.LocaleId == i.LocaleId).Max(o => o.ShellNo),

            });
        }
        public Models.Views.Article Create(Models.Views.Article item)
        {
            var _item = Article.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Article Update(Models.Views.Article item)
        {
            var _item = Article.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Article item)
        {
            Article.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Article Build(Models.Views.Article item)
        {
            return new Models.Entities.Article()
            {
                Id = item.Id,
                ArticleNo = item.ArticleNo,
                ArticleName = item.ArticleName,
                SizeRange = item.SizeRange,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
                ProjectType = item.ProjectType,
                BrandCodeId = item.BrandCodeId,
                OutsoleId = item.OutsoleId,
                KnifeId = item.KnifeId,
                LastId = item.LastId,
                Gender = item.Gender,
                forCBD = item.forCBD,
                IsAlternate = item.IsAlternate,
                ShellId = item.ShellId,
                DayCapacity = item.DayCapacity,
                LastTurnover = item.LastTurnover,
            };
        }

    }
}