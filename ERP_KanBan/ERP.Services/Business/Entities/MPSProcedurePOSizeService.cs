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
    public class MPSProcedurePOSizeService : BusinessService
    {
        private Services.Entities.MpsProcedurePOSizeService MPSProcedurePOSize { get; }

        public MPSProcedurePOSizeService(
            Services.Entities.MpsProcedurePOSizeService mpsProcedurePOSizeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePOSize = mpsProcedurePOSizeService;
        }
        public IQueryable<Models.Views.MPSProcedurePOSize> Get()
        {
            return MPSProcedurePOSize.Get().Select(i => new Models.Views.MPSProcedurePOSize
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MpsProcedurePOId = i.MpsProcedurePOId,
                DisplaySize = i.DisplaySize,
                SeqId = i.SeqId,
                SubQty = i.SubQty,
                PayType = i.PayType,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSProcedurePOSize> items)
        {
            MPSProcedurePOSize.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsProcedurePOSize, bool>> predicate)
        {
            MPSProcedurePOSize.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsProcedurePOSize> BuildRange(IEnumerable<Models.Views.MPSProcedurePOSize> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcedurePOSize
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsProcedurePOId = item.MpsProcedurePOId,
                DisplaySize = item.DisplaySize,
                SeqId = item.SeqId,
                SubQty = item.SubQty,
                PayType = item.PayType,
            });
        }
    }
}