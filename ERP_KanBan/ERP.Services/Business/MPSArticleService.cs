using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using Newtonsoft.Json;
using ERP.Models.Views.Common;

namespace ERP.Services.Business
{
    public class MPSArticleService : BusinessService
    {
        private ERP.Services.Business.Entities.ArticleService Article { get; set; }
        private ERP.Services.Business.Entities.MPSArticleService MPSArticle { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }

        public MPSArticleService(
            ERP.Services.Business.Entities.ArticleService articleService,
            ERP.Services.Business.Entities.MPSArticleService mpsArticleService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Article = articleService;
            MPSArticle = mpsArticleService;
            CodeItem = codeItemService;
        }

        public IQueryable<ERP.Models.Views.MPSArticle> BuildMPSArticle(string predicate)
        {
            var result = (
                    from a in Article.Get()
                    join c in CodeItem.Get() on new { BrandCodeId = a.BrandCodeId, LocaleId = a.LocaleId } equals new { BrandCodeId = c.Id, LocaleId = c.LocaleId } into cGRP
                    from c in cGRP.DefaultIfEmpty()
                    join ma in MPSArticle.Get() on new { BrandCode = c.NameTW, ArticleNo = a.ArticleNo, LocaleId = c.LocaleId } equals new { BrandCode = ma.BrandTw, ArticleNo = ma.ArticleNo, LocaleId = ma.LocaleId } into maGRP
                    from ma in maGRP.DefaultIfEmpty()
                    select new
                    {
                        LocaleId = a.LocaleId,
                        ArticleNo = a.ArticleNo,
                        ShoeName = a.ArticleName,
                        OutsoleNo = a.OutsoleNo,
                        LastNo = a.LastNo,
                        KnifeNo = a.KnifeNo,
                        ShellNo = a.ShellNo,
                        DayCapacity = a.DayCapacity,
                        LastTurnover = a.LastTurnover,
                        ArticleId = a.Id,

                        Id = (decimal?)ma.Id,
                        BrandTw = (string?)c.NameTW,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Select(i => new ERP.Models.Views.MPSArticle
                {
                    Id = i.Id ?? 0,
                    LocaleId = i.LocaleId,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    BrandTw = i.BrandTw ?? "",
                    ArticleNo = i.ArticleNo,
                    ShoeName = i.ShoeName,
                    OutsoleNo = i.OutsoleNo,
                    LastNo = i.LastNo,
                    KnifeNo = i.KnifeNo,
                    ShellNo = i.ShellNo,
                    DayCapacity = i.DayCapacity ?? 0,
                    LastTurnover = i.LastTurnover ?? 0,
                    ArticleId = i.ArticleId,
                })
                .ToList();

            return result.AsQueryable();
        }

        public List<ERP.Models.Views.MPSArticle> Save(List<MPSArticle> items)
        {
            try
            {
                UnitOfWork.BeginTransaction();

                var localeId = items[0].LocaleId;
                var userName = items[0].ModifyUserName;

                var articls = items.Select(i => i.ArticleNo).ToList();
                var updateItems = items.Where(i => i.Id > 0).ToList();
                var addItems = items.Where(i => i.Id == 0 || i.Id == null).ToList();

                if (updateItems.Count() > 0)
                {
                    MPSArticle.UpdateRange(updateItems);
                }
                if (addItems.Count() > 0)
                {
                    MPSArticle.CreateRange(addItems);
                }
                UnitOfWork.Commit();

                return MPSArticle.Get().Where(i => articls.Contains(i.ArticleNo) && i.LocaleId == localeId).ToList();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public List<MPSArticle> Remove(List<MPSArticle> items)
        {
            try
            {
                UnitOfWork.BeginTransaction();

                var localeId = items[0].LocaleId;
                var Ids = items.Select(i => i.Id).ToList();

                if (Ids.Any())
                {
                    MPSArticle.RemoveRange(i => i.LocaleId == localeId && Ids.Contains(i.Id));
                }

                UnitOfWork.Commit();
                return items;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
