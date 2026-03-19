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
    public class ReceivedStandardService : BusinessService
    {
        private Services.Entities.ReceivedRejectStandardService ReceivedStandard { get; }
        private Services.Entities.MaterialService Material { get; }
        public ReceivedStandardService(
            Services.Entities.ReceivedRejectStandardService receivedStandardService,
            Services.Entities.MaterialService materialService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            ReceivedStandard = receivedStandardService;
            Material = materialService;
        }
        public IQueryable<Models.Views.ReceivedStandard> Get()
        {
            var result = (
                from r in ReceivedStandard.Get()
                join m in Material.Get() on new { MaterialId = r.MaterialId, LocaleId = r.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGPR
                from m in mGPR.DefaultIfEmpty()
                select new Models.Views.ReceivedStandard
                {
                    Id = r.Id,
                    LocaleId = r.LocaleId,
                    Priority = r.Priority,
                    CategoryCodeId = r.CategoryCodeId,
                    MaterialId = r.MaterialId,
                    VendorId = r.VendorId,
                    AbovePurQty = r.AbovePurQty,
                    WarningRate = r.WarningRate,
                    RejectRate = r.RejectRate,
                    ModifyUserName = r.ModifyUserName,
                    LastUpdateTime = r.LastUpdateTime,
                    MaterialName = m.MaterialName,
                    MaterialNameEn = m.MaterialNameEng,
                }
            );
            return result;
        }
        public Models.Views.ReceivedStandard Create(Models.Views.ReceivedStandard item)
        {
            var _item = ReceivedStandard.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public Models.Views.ReceivedStandard Update(Models.Views.ReceivedStandard item)
        {
            var _item = ReceivedStandard.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.ReceivedStandard item)
        {
            ReceivedStandard.Remove(Build(item));
        }
        private ERP.Models.Entities.ReceivedRejectStandard Build(Models.Views.ReceivedStandard item)
        {
            return new ERP.Models.Entities.ReceivedRejectStandard
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                Priority = item.Priority,
                CategoryCodeId = item.CategoryCodeId,
                MaterialId = item.MaterialId,
                VendorId = item.VendorId,
                AbovePurQty = item.AbovePurQty,
                WarningRate = item.WarningRate,
                RejectRate = item.RejectRate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    }
}