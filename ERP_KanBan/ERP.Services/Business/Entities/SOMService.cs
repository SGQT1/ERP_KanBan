using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class SOMService : BusinessService
    {
        private Services.Entities.SOMService SOM { get; }
        private Services.Entities.MaterialService Material { get; }

        public SOMService(
            Services.Entities.SOMService somService,
            Services.Entities.MaterialService materialService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            SOM = somService;
            Material = materialService;
        }
        public IQueryable<Models.Views.SOM> Get()
        {
            return SOM.Get().Select(i => new Models.Views.SOM
            {
                Id = i.Id,
                ParentId = i.ParentId,
                ParentMaterialName = Material.Get().Where(m => m.Id == i.ParentId && m.LocaleId == i.LocaleId).Max(m => m.MaterialName),
                ChildId = i.ChildId,
                ChildMaterialName = Material.Get().Where(m => m.Id == i.ChildId && m.LocaleId == i.LocaleId).Max(m => m.MaterialName),
                ChildMaterialNameEng = Material.Get().Where(m => m.Id == i.ChildId && m.LocaleId == i.LocaleId).Max(m => m.MaterialNameEng),
                SeqNo = i.SeqNo,
                Qty = i.Qty,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                ItemGroupCode = i.ItemGroupCode,
                ParentGroupCode = i.ParentGroupCode

            });
        }
        public void CreateRange(IEnumerable<Models.Views.SOM> item)
        {
            SOM.CreateRange(BuildRange(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.SOM, bool>> predicate)
        {
            SOM.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.SOM> BuildRange(IEnumerable<Models.Views.SOM> items)
        {
            return items.Select(item => new ERP.Models.Entities.SOM
            {
                Id = item.Id,
                ParentId = item.ParentId,
                ChildId = item.ChildId,
                SeqNo = item.SeqNo,
                Qty = item.Qty,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
                ItemGroupCode = item.ItemGroupCode,
                ParentGroupCode = item.ParentGroupCode
            });
        }
       
    }
}