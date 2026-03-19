using System;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class BondMaterialChinaService : BusinessService
    {
        private Services.Entities.BondMaterialChinaService BondMaterialChina { get; }

        public BondMaterialChinaService(
            Services.Entities.BondMaterialChinaService BondMaterialChinaService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            BondMaterialChina = BondMaterialChinaService;
        }
        public IQueryable<Models.Views.BondMaterialChina> Get()
        {
            return BondMaterialChina.Get().Select(i => new Models.Views.BondMaterialChina
            {
                Id = i.Id,
                BondMaterialName = i.BondMaterialName,
                BondMaterialNo = i.BondMaterialNo,
                BondUnitName = i.BondUnitName,
                BondWeightEachUnit = i.BondWeightEachUnit,
                BondUnitPrice = i.BondUnitPrice,
                BondDollarName = i.BondDollarName,
                BondTaxRate = i.BondTaxRate,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,

            });
        }
        public Models.Views.BondMaterialChina Create(Models.Views.BondMaterialChina item)
        {
            var _item = BondMaterialChina.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public Models.Views.BondMaterialChina Update(Models.Views.BondMaterialChina item)
        {
            var _item = BondMaterialChina.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.BondMaterialChina item)
        {
            BondMaterialChina.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.BondMaterialChina Build(Models.Views.BondMaterialChina item)
        {
            return new Models.Entities.BondMaterialChina()
            {
                Id = item.Id,
                BondMaterialName = item.BondMaterialName,
                BondMaterialNo = item.BondMaterialNo,
                BondUnitName = item.BondUnitName,
                BondWeightEachUnit = item.BondWeightEachUnit,
                BondUnitPrice = item.BondUnitPrice,
                BondDollarName = item.BondDollarName,
                BondTaxRate = item.BondTaxRate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    }
}