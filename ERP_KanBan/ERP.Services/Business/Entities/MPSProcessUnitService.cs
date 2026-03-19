using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSProcessUnitService : BusinessService
    {
        private ERP.Services.Entities.MpsProcessUnitService MPSProcessUnit { get; set; }

        public MPSProcessUnitService(
            ERP.Services.Entities.MpsProcessUnitService mpsProcessUnitService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcessUnit = mpsProcessUnitService;
        }

        public IQueryable<Models.Views.MPSProcessUnit> Get()
        {
            return this.MPSProcessUnit.Get().Select(i => new Models.Views.MPSProcessUnit {
                Id = i.Id,
                LocaleId = i.LocaleId,
                UnitNo = i.UnitNo,
                UnitNameTw = i.UnitNameTw,
                UnitNameEn = i.UnitNameEn,
                UnitNameCn = i.UnitNameCn,
                UnitNameVn = i.UnitNameVn,
                UnitType = i.UnitType,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime
            });
        }
        public Models.Views.MPSProcessUnit Create(Models.Views.MPSProcessUnit item)
        {
            var _item = MPSProcessUnit.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcessUnit Update(Models.Views.MPSProcessUnit item)
        {
            var _item = MPSProcessUnit.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcessUnit item)
        {
            MPSProcessUnit.Remove(Build(item));
        }
        private Models.Entities.MpsProcessUnit Build(Models.Views.MPSProcessUnit item)
        {
            return new Models.Entities.MpsProcessUnit()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                UnitNo = item.UnitNo,
                UnitNameTw = item.UnitNameTw,
                UnitNameEn = item.UnitNameEn,
                UnitNameCn = item.UnitNameCn,
                UnitNameVn = item.UnitNameVn,
                UnitType = item.UnitType,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    
    }
}
