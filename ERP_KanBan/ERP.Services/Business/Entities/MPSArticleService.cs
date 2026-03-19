using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSArticleService : BusinessService
    {
        private ERP.Services.Entities.MpsArticleService MPSArticle { get; set; }

        public MPSArticleService(
            ERP.Services.Entities.MpsArticleService mpsArticleService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSArticle = mpsArticleService;
        }

        public IQueryable<Models.Views.MPSArticle> Get()
        {
            return this.MPSArticle.Get().Select(i => new Models.Views.MPSArticle
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                BrandTw = i.BrandTw,
                ArticleNo = i.ArticleNo,
                ShoeName = i.ShoeName,
                OutsoleNo = i.OutsoleNo,
                LastNo = i.LastNo,
                KnifeNo = i.KnifeNo,
                ShellNo = i.ShellNo,
                DayCapacity = i.DayCapacity,
                LastTurnover = i.LastTurnover,
            });
        }
        public Models.Views.MPSArticle Create(Models.Views.MPSArticle item)
        {
            var _item = MPSArticle.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSArticle Update(Models.Views.MPSArticle item)
        {
            var _item = MPSArticle.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSArticle item)
        {
            MPSArticle.Remove(Build(item));
        }
        private Models.Entities.MpsArticle Build(Models.Views.MPSArticle item)
        {
            return new Models.Entities.MpsArticle()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                BrandTw = item.BrandTw,
                ArticleNo = item.ArticleNo,
                ShoeName = item.ShoeName,
                OutsoleNo = item.OutsoleNo,
                LastNo = item.LastNo,
                KnifeNo = item.KnifeNo,
                ShellNo = item.ShellNo,
                DayCapacity = item.DayCapacity,
                LastTurnover = item.LastTurnover,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.MPSArticle> items)
        {
            MPSArticle.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.MPSArticle> items)
        {
            MPSArticle.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsArticle, bool>> predicate)
        {
            MPSArticle.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsArticle> BuildRange(IEnumerable<Models.Views.MPSArticle> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsArticle
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                BrandTw = item.BrandTw,
                ArticleNo = item.ArticleNo,
                ShoeName = item.ShoeName,
                OutsoleNo = item.OutsoleNo,
                LastNo = item.LastNo,
                KnifeNo = item.KnifeNo,
                ShellNo = item.ShellNo,
                DayCapacity = item.DayCapacity,
                LastTurnover = item.LastTurnover,
            });
        }

    }
}
