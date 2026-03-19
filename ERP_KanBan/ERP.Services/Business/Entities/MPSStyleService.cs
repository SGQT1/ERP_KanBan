using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSStyleService : BusinessService
    {
        private Services.Entities.MpsArticleService MPSArticle { get; }
        private ERP.Services.Entities.MpsStyleService MPSStyle { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }

        public MPSStyleService(
            Services.Entities.MpsArticleService mpsArticleService,
            ERP.Services.Entities.MpsStyleService mpsStyleService,
            ERP.Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSArticle = mpsArticleService;
            MPSStyle = mpsStyleService;
            Orders = ordersService;
        }

        public IQueryable<Models.Views.MPSStyle> Get()
        {
            var result = (
               from s in MPSStyle.Get()
               join a in MPSArticle.Get() on new { MpsArticleId = s.MpsArticleId, LocaleId = s.LocaleId } equals new { MpsArticleId = a.Id, LocaleId = a.LocaleId }
               join o in Orders.Get() on new { OrderNo = s.RefOrderNo } equals new { OrderNo = o.OrderNo } into oGRP
               from o in oGRP.DefaultIfEmpty()
               select new ERP.Models.Views.MPSStyle
               {
                   Id = s.Id,
                   MpsArticleId = s.MpsArticleId,
                   StyleNo = s.StyleNo,
                   ColorDesc = s.ColorDesc,
                   LocaleId = s.LocaleId,
                   SizeCountryCodeId = s.SizeCountryCodeId,
                   ModifyUserName = s.ModifyUserName,
                   LastUpdateTime = s.LastUpdateTime,
                   RefOrderNo = s.RefOrderNo,
                   DoUsage = s.DoUsage,
                   DollarNameTw = s.DollarNameTw,
                   UnitRelaxTime = s.UnitRelaxTime,
                   UnitStandardTime = s.UnitStandardTime,
                   UnitLaborCost = s.UnitLaborCost,
                   RefOrderNoOfMaterial = s.RefOrderNoOfMaterial,
                   HasProcedure = 0,
                   ArticleNo = a.ArticleNo,
                   Brand = (string?)o.Brand ?? "",
                   ArticleId = (decimal?)o.ArticleId ?? 0,
                   StyleId = (decimal?)o.StyleId ?? 0,
               }
           );

            return result;
        }
        public Models.Views.MPSStyle Create(Models.Views.MPSStyle item)
        {
            var _item = MPSStyle.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSStyle Update(Models.Views.MPSStyle item)
        {
            var _item = MPSStyle.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void UpdateCost(Models.Views.MPSStyle item)
        {
            MPSStyle.UpdateRange(
                i => i.Id == item.Id && i.LocaleId == item.LocaleId,
                // u => new Models.Entities.MpsStyle { UnitStandardTime = item.UnitStandardTime, UnitLaborCost = item.UnitLaborCost, DollarNameTw = item.DollarNameTw }
                u => u.SetProperty(p => p.UnitStandardTime, v => item.UnitStandardTime).SetProperty(p => p.UnitLaborCost, v => item.UnitLaborCost).SetProperty(p => p.DollarNameTw, v => item.DollarNameTw)
            );
        }

        public void Remove(Models.Views.MPSStyle item)
        {
            MPSStyle.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.MpsStyle Build(Models.Views.MPSStyle item)
        {
            return new Models.Entities.MpsStyle()
            {
                Id = item.Id,
                MpsArticleId = item.MpsArticleId,
                StyleNo = item.StyleNo,
                ColorDesc = item.ColorDesc,
                LocaleId = item.LocaleId,
                SizeCountryCodeId = item.SizeCountryCodeId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                RefOrderNo = item.RefOrderNo,
                DoUsage = item.DoUsage,
                DollarNameTw = item.DollarNameTw,
                UnitRelaxTime = item.UnitRelaxTime,
                UnitStandardTime = item.UnitStandardTime,
                UnitLaborCost = item.UnitLaborCost,
                RefOrderNoOfMaterial = item.RefOrderNoOfMaterial,
            };
        }
    }
}
