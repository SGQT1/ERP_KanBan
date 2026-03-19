using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business {
    public class SizeMappingService : BusinessService {
        private ERP.Services.Business.Entities.SizeMappingService SizeMapping { get; set; }
        private ERP.Services.Business.Entities.SizeMappingItemService SizeMappingItem { get; set; }
        public SizeMappingService (
            ERP.Services.Business.Entities.SizeMappingService sizeMappingService,
            ERP.Services.Business.Entities.SizeMappingItemService sizeMappingItemService,
            UnitOfWork unitOfWork
        ) : base (unitOfWork) {
            SizeMapping = sizeMappingService;
            SizeMappingItem = sizeMappingItemService;
        }

        public ERP.Models.Views.SizeMappingGroup GetSizeMappingGroup (int id, int localeId) {
            return new Models.Views.SizeMappingGroup {
                SizeMapping = SizeMapping.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault(),
                SizeMappingItem = SizeMappingItem.Get().Where(i => i.SizeCountryCodeId == id && i.LocaleId == localeId),
            };
        }

        public ERP.Models.Views.SizeMappingGroup SaveSizeMappingGroup (ERP.Models.Views.SizeMappingGroup item) {
            UnitOfWork.BeginTransaction ();
            try {
                var sizeMapping =  item.SizeMapping;
        
                // var _item = Customer.Get ().Where (i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault ();
                // if (_item != null) {
                //     item = Customer.Update (item);
                // } else {
                //     item = Customer.Create (item);
                // }
                // UnitOfWork.Commit ();
                return GetSizeMappingGroup ((int) sizeMapping.Id, (int) sizeMapping.LocaleId);
            } catch {
                UnitOfWork.Rollback ();
                return item;
            }
        }
        public void RemoveSizeMappingGroup (ERP.Models.Views.SizeMappingGroup item) {
            UnitOfWork.BeginTransaction ();
            try {
                // Customer.Remove (item);
                UnitOfWork.Commit();
            }
            catch {
                UnitOfWork.Rollback ();
            }
        }

        public IQueryable<ERP.Models.Views.SizeMappingItem> GetSizeMappingItem()
        {   
            return SizeMappingItem.Get();
        }
        public ERP.Models.Views.SizeMappingItem CreateSizeMappingItem(SizeMappingItem item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                item = SizeMappingItem.Create(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.SizeMappingItem UpdateSizeMappingItem(SizeMappingItem item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                item = SizeMappingItem.Update(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void RemoveSizeMappingItem(SizeMappingItem item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                SizeMappingItem.Remove(item);
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