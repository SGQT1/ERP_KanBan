using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSOutsourceProcessService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcedureGroupService MPSProcedureGroup { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureGroupItemService MPSProcedureGroupItem { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureService MPSProcedure { get; set; }

        public MPSOutsourceProcessService(
            ERP.Services.Business.Entities.MPSProcedureService mpsProcedureService,
            ERP.Services.Business.Entities.MPSProcedureGroupService mpsProcedureGroupService,
            ERP.Services.Business.Entities.MPSProcedureGroupItemService mpsProcedureGroupItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedure = mpsProcedureService;
            MPSProcedureGroup = mpsProcedureGroupService;
            MPSProcedureGroupItem = mpsProcedureGroupItemService;
        }

        public ERP.Models.Views.MPSOutsourceProcessGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.MPSOutsourceProcessGroup();

            var mpsProcedureGroup = MPSProcedureGroup.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (mpsProcedureGroup != null)
            {
                group.MPSProcedureGroup = mpsProcedureGroup;
                group.MPSProcedureGroupItem = MPSProcedureGroupItem.Get().Where(i => i.MpsProcedureGroupId == mpsProcedureGroup.Id && i.LocaleId == mpsProcedureGroup.LocaleId).ToList();
                group.MPSProcedure = MPSProcedure.GetWithItem().Where(i => i.MpsStyleNo == mpsProcedureGroup.StyleNo && i.LocaleId == mpsProcedureGroup.LocaleId).ToList();
            }
            return group;
        }
        public ERP.Models.Views.MPSProcedureGroup GetProcedureGroup(string style, string process, int localeId)
        {
            var mpsProcedureGroup = MPSProcedureGroup.Get().Where(i => i.StyleNo == style && i.GroupNameLocal == process && i.LocaleId == localeId).FirstOrDefault();
           
            return mpsProcedureGroup;
        }
        public ERP.Models.Views.MPSOutsourceProcessGroup Save(MPSOutsourceProcessGroup group)
        {
            var mpsProcedureGroup = group.MPSProcedureGroup;
            var mpsProcedureGroupItem = group.MPSProcedureGroupItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();

                if (mpsProcedureGroup != null)
                {
                    //mpsProcedureGroup
                    {
                        var _mpsProcedureGroup = MPSProcedureGroup.Get().Where(i => i.LocaleId == mpsProcedureGroup.LocaleId && i.Id == mpsProcedureGroup.Id).FirstOrDefault();
                        if (_mpsProcedureGroup == null)
                        {
                            mpsProcedureGroup = MPSProcedureGroup.Create(mpsProcedureGroup);
                        }
                        else
                        {
                            mpsProcedureGroup.Id = _mpsProcedureGroup.Id;
                            mpsProcedureGroup.LocaleId = _mpsProcedureGroup.LocaleId;
                            mpsProcedureGroup = MPSProcedureGroup.Update(mpsProcedureGroup);
                        }
                    }
                    //mpsProcedureGroupItems
                    {
                        if (mpsProcedureGroup.Id != 0)
                        {
                            mpsProcedureGroupItem.ForEach(i =>
                            {
                                i.MpsProcedureGroupId = mpsProcedureGroup.Id;
                                i.LocaleId = mpsProcedureGroup.LocaleId;
                            });
                            MPSProcedureGroupItem.RemoveRange(i => i.MpsProcedureGroupId == mpsProcedureGroup.Id && i.LocaleId == mpsProcedureGroup.LocaleId);
                            MPSProcedureGroupItem.CreateRange(mpsProcedureGroupItem);
                        }
                    }
                }
                UnitOfWork.Commit();
                return Get((int)mpsProcedureGroup.Id, (int)mpsProcedureGroup.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(MPSOutsourceProcessGroup group)
        {
            var mpsProcedureGroup = group.MPSProcedureGroup;
            try
            {
                UnitOfWork.BeginTransaction();
                if (mpsProcedureGroup != null)
                {
                    MPSProcedureGroupItem.RemoveRange(i => i.MpsProcedureGroupId == mpsProcedureGroup.Id && i.LocaleId == mpsProcedureGroup.LocaleId);
                    MPSProcedureGroup.Remove(mpsProcedureGroup);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public IEnumerable<ERP.Models.Views.MPSProcedure> BuildMPSProcedure(string styleNo, int localeId) {
            var items = MPSProcedure.GetWithItem().Where(i => i.MpsStyleNo == styleNo && i.LocaleId == localeId).ToList();

            return items;
        }
    }
}
