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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class WarehouseService : BusinessService
    {
        private ERP.Services.Business.Entities.WarehouseService Warehouse { get; set; }
        public WarehouseService(
            ERP.Services.Business.Entities.WarehouseService warehouseService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Warehouse = warehouseService;
        }

        public ERP.Models.Views.Warehouse Get(int id, int localeId)
        {   
            return Warehouse.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.Warehouse Save(Warehouse item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = Warehouse.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = Warehouse.Update(item);
                }
                else
                {
                    item = Warehouse.Create(item);
                }

                UnitOfWork.Commit();
                return Get((int)item.Id, (int)item.LocaleId);
            }
            catch(Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(Warehouse item)
        {   
            UnitOfWork.BeginTransaction();
            try
            { 
                Warehouse.Remove(item);
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
