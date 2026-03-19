using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class WarehouseService : BusinessService
    {
        private Services.Entities.WarehouseService Warehouse { get; }
        public WarehouseService(
            Services.Entities.WarehouseService warehouseService, 
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.Warehouse = warehouseService;
        }
        public IQueryable<Models.Views.Warehouse> Get()
        {
            return Warehouse.Get().Select(i => new Models.Views.Warehouse
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                WarehouseNo = i.WarehouseNo,
                LocationDesc = i.LocationDesc,
                OrgUnitId = i.OrgUnitId,
                TypeCode = i.TypeCode,
                CloseOff = i.CloseOff,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                IsSluggish = i.IsSluggish,
                BelongCompanyId  = i.BelongCompanyId,
            });
        }
        public IQueryable<Models.Views.WarehouseCache> GetCache()
        {
            return Warehouse.Get().Select(i => new WarehouseCache {
                    Id = i.Id,
                    WarehouseNo = i.WarehouseNo,
                    Type = i.TypeCode,
                    LocaleId = i.LocaleId
                });
        }
        public Models.Views.Warehouse Create(Models.Views.Warehouse item)
        {
            var _item = Warehouse.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Warehouse Update(Models.Views.Warehouse item)
        {
            var _item = Warehouse.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Warehouse item)
        {
            Warehouse.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Warehouse Build(Models.Views.Warehouse item)
        {
            return new Models.Entities.Warehouse()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                WarehouseNo = item.WarehouseNo,
                LocationDesc = item.LocationDesc,
                OrgUnitId = item.OrgUnitId,
                TypeCode = item.TypeCode,
                CloseOff = item.CloseOff,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                IsSluggish = item.IsSluggish,
                BelongCompanyId = item.BelongCompanyId,
            };
        }
        
    }
}