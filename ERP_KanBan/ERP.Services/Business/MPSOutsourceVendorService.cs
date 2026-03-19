using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSOutsourceVendorService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcedureVendorService MPSProcedureVendor { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureVendorItemService MPSProcedureVendorItem { get; set; }
        
        public MPSOutsourceVendorService(
            ERP.Services.Business.Entities.MPSProcedureVendorService mpsProcedureVendorService,
            ERP.Services.Business.Entities.MPSProcedureVendorItemService mpsProcedureVendorItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedureVendor = mpsProcedureVendorService;
            MPSProcedureVendorItem = mpsProcedureVendorItemService;
        }

        public ERP.Models.Views.MPSOutsourceVendorGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.MPSOutsourceVendorGroup();

            var vendor = MPSProcedureVendor.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if(vendor != null) 
            {
                group.MPSProcedureVendor = vendor;
                group.MPSProcedureVendorItem = MPSProcedureVendorItem.Get().Where(i => i.MPSProcedureVendorId == vendor.Id && i.LocaleId == vendor.LocaleId).ToList();
            }
            return group;
        }
        
        public ERP.Models.Views.MPSOutsourceVendorGroup Save(MPSOutsourceVendorGroup group)
        {
            var vendror = group.MPSProcedureVendor;
            var vendrorItems = group.MPSProcedureVendorItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (vendror != null)
                {
                    //vendor
                    {
                        var _vendor = MPSProcedureVendor.Get().Where(i => i.LocaleId == vendror.LocaleId && i.Id == vendror.Id).FirstOrDefault();
                        if (_vendor == null)
                        {
                            vendror = MPSProcedureVendor.Create(vendror);
                        }
                        else
                        {
                            vendror.Id = _vendor.Id;
                            vendror.LocaleId = _vendor.LocaleId;
                            vendror = MPSProcedureVendor.Update(vendror);
                        }
                    }
                    //items
                    {
                        if (vendror.Id != 0)
                        {
                            vendrorItems.ForEach(i =>
                            {
                                i.MPSProcedureVendorId = vendror.Id;
                                i.LocaleId = vendror.LocaleId;
                            });
                            MPSProcedureVendorItem.RemoveRange(i => i.MpsProcedureVendorId == vendror.Id && i.LocaleId == vendror.LocaleId);
                            MPSProcedureVendorItem.CreateRange(vendrorItems);
                        }
                    }
                }
                UnitOfWork.Commit();
                return Get((int)vendror.Id, (int)vendror.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(MPSOutsourceVendorGroup group)
        {
            var vendor = group.MPSProcedureVendor;
            // var venodrItems = group.MPSProcedureVendorItem;
            try
            {
                UnitOfWork.BeginTransaction();
                if (vendor != null)
                {
                    MPSProcedureVendorItem.RemoveRange(i => i.MpsProcedureVendorId == vendor.Id && i.LocaleId == vendor.LocaleId);
                    MPSProcedureVendor.Remove(vendor);
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
