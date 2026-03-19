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
    public class MPSOutsourceQuotationService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcedureGroupService MPSProcedureGroup { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureGroupItemService MPSProcedureGroupItem { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureQuotService MPSProcedureQuot { get; set; }
        
        public MPSOutsourceQuotationService(
            ERP.Services.Business.Entities.MPSProcedureQuotService mpsProcedureQuotService,
            ERP.Services.Business.Entities.MPSProcedureGroupService mpsProcedureGroupService,
            ERP.Services.Business.Entities.MPSProcedureGroupItemService mpsProcedureGroupItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedureGroup = mpsProcedureGroupService;
            MPSProcedureGroupItem = mpsProcedureGroupItemService;
            MPSProcedureQuot = mpsProcedureQuotService;
        }

        public ERP.Models.Views.MPSOutsourceQuotationGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.MPSOutsourceQuotationGroup();

            var procedure = MPSProcedureGroup.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if(procedure != null) 
            {
                group.MPSProcedureGroup = procedure;
                group.MPSProcedureGroupItem = MPSProcedureGroupItem.Get().Where(i => i.MpsProcedureGroupId == procedure.Id && i.LocaleId == procedure.LocaleId).ToList();
                group.MPSProcedureQuot = MPSProcedureQuot.Get().Where(i => i.MpsProcedureGroupId == procedure.Id && i.LocaleId == procedure.LocaleId)
                    .OrderBy(i => i.MpsProcedureVendorId).ThenBy(i => i.MpsProcedureVendorId).ThenBy(i => i.QuotDate).ToList();
            }
            return group;
        }
        
        public ERP.Models.Views.MPSOutsourceQuotationGroup Save(MPSOutsourceQuotationGroup group)
        {
            var procedure = group.MPSProcedureGroup;
            var procedureItems = group.MPSProcedureGroupItem.ToList();
            var quots = group.MPSProcedureQuot.ToList();
            try
            {
                UnitOfWork.BeginTransaction();

                if (quots.Count() > 0)
                {
                    //MaterialQuot Item
                    {
                        MPSProcedureQuot.RemoveRange(i => i.MpsProcedureGroupId == procedure.Id && i.LocaleId == procedure.LocaleId);
                        MPSProcedureQuot.CreateRange(quots);
                    }
                }
                UnitOfWork.Commit();
                return Get((int)procedure.Id, (int)procedure.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(MPSOutsourceQuotationGroup group)
        {
            var procedure = group.MPSProcedureGroup;
            try
            {
                UnitOfWork.BeginTransaction();
                MPSProcedureQuot.RemoveRange(i => i.MpsProcedureGroupId == procedure.Id && i.LocaleId == procedure.LocaleId);
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
