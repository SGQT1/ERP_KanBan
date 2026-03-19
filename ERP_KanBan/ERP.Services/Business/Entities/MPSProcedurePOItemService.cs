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
    public class MPSProcedurePOItemService : BusinessService
    {
        private Services.Entities.MpsProcedurePOItemService MPSProcedurePOItem { get; }

        public MPSProcedurePOItemService(
            Services.Entities.MpsProcedurePOItemService mpsProcedurePOItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePOItem = mpsProcedurePOItemService;
        }
        public IQueryable<Models.Views.MPSProcedurePOItem> Get()
        {
            return MPSProcedurePOItem.Get().Select(i => new Models.Views.MPSProcedurePOItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MpsProcedurePOId = i.MpsProcedurePOId,
                ProcedureNameTw = i.ProcedureNameTw,
                PairsStandardTime = i.PairsStandardTime,
                PairsStandardPrice = i.PairsStandardPrice
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSProcedurePOItem> items)
        {
            MPSProcedurePOItem.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsProcedurePOItem, bool>> predicate)
        {
            MPSProcedurePOItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsProcedurePOItem> BuildRange(IEnumerable<Models.Views.MPSProcedurePOItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcedurePOItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsProcedurePOId = item.MpsProcedurePOId,
                ProcedureNameTw = item.ProcedureNameTw,
                PairsStandardTime = item.PairsStandardTime,
                PairsStandardPrice = item.PairsStandardPrice
            });
        }
    }
}