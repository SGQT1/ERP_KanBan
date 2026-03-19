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
    public class BondProductChinaService : BusinessService
    {
        private Services.Entities.BondProductChinaService BondProductChina { get; }

        public BondProductChinaService(
            Services.Entities.BondProductChinaService BondProductChinaService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            BondProductChina = BondProductChinaService;
        }
        public IQueryable<Models.Views.BondProductChina> Get()
        {
            return BondProductChina.Get().Select(i => new Models.Views.BondProductChina
            {
                Id = i.Id,
                BondProductName = i.BondProductName,
                BondProductNo = i.BondProductNo,
                BondUnitName = i.BondUnitName,
                BondWeightEachUnit = i.BondWeightEachUnit,
                BondUnitPrice = i.BondUnitPrice,
                BondDollarName = i.BondDollarName,
                BondTaxRate = i.BondTaxRate,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,

            });
        }
        public Models.Views.BondProductChina Create(Models.Views.BondProductChina item)
        {
            var _item = BondProductChina.Create(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public Models.Views.BondProductChina Update(Models.Views.BondProductChina item)
        {
            var _item = BondProductChina.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.BondProductChina item)
        {
            BondProductChina.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.BondProductChina Build(Models.Views.BondProductChina item)
        {
            return new Models.Entities.BondProductChina()
            {
                Id = item.Id,
                BondProductName = item.BondProductName,
                BondProductNo = item.BondProductNo,
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