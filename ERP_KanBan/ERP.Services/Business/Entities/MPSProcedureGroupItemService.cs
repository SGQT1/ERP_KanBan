using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MPSProcedureGroupItemService : BusinessService
    {
        private Services.Entities.MpsProcedureGroupItemService MPSProcedureGroupItem { get; }
        private Services.Entities.MpsProcedureGroupService MPSProcedureGroup { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public MPSProcedureGroupItemService(
            Services.Entities.MpsProcedureGroupItemService mpsProcedureGroupItemService,
            Services.Entities.MpsProcedureGroupService mpsProcedureGroupService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedureGroupItem = mpsProcedureGroupItemService;
            MPSProcedureGroup = mpsProcedureGroupService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.MPSProcedureGroupItem> Get()
        {
            return MPSProcedureGroupItem.Get().Select(i => new Models.Views.MPSProcedureGroupItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MpsProcedureGroupId = i.MpsProcedureGroupId,
                ProcedureNameTw = i.ProcedureNameTw,
                PairsStandardTime = i.PairsStandardTime,
                PairsStandardPrice = i.PairsStandardPrice,
            });
        }
        public IQueryable<Models.Views.MPSProcedureGroupItem> GetWhitStyle()
        {
            var result = (
                from p in MPSProcedureGroup.Get()
                join pi in MPSProcedureGroupItem.Get() on new { Id = p.Id, LocaleId = p.LocaleId } equals new { Id = pi.MpsProcedureGroupId, LocaleId = pi.LocaleId }
                select new Models.Views.MPSProcedureGroupItem
                {
                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    MpsProcedureGroupId = pi.MpsProcedureGroupId,
                    ProcedureNameTw = pi.ProcedureNameTw,
                    PairsStandardTime = pi.PairsStandardTime,
                    PairsStandardPrice = pi.PairsStandardPrice,
                    StyleNo = p.StyleNo,
                    OutsourceProcess = p.GroupNameLocal,
                    GroupNameLocal = p.GroupNameLocal,

                });
            return result;
            // return MPSProcedureGroupItem.Get().Select(i => new Models.Views.MPSProcedureGroupItem
            // {
            //     Id = i.Id,
            //     LocaleId = i.LocaleId,
            //     MpsProcedureGroupId = i.MpsProcedureGroupId,
            //     ProcedureNameTw = i.ProcedureNameTw,
            //     PairsStandardTime = i.PairsStandardTime,
            //     PairsStandardPrice = i.PairsStandardPrice,
            // });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSProcedureGroupItem> items)
        {
            MPSProcedureGroupItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsProcedureGroupItem, bool>> predicate)
        {
            MPSProcedureGroupItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsProcedureGroupItem> BuildRange(IEnumerable<Models.Views.MPSProcedureGroupItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcedureGroupItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsProcedureGroupId = item.MpsProcedureGroupId,
                ProcedureNameTw = item.ProcedureNameTw,
                PairsStandardTime = item.PairsStandardTime,
                PairsStandardPrice = item.PairsStandardPrice,
            });
        }
    }
}