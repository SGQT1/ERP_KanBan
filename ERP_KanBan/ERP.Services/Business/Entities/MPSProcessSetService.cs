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

    public class MPSProcessSetService : BusinessService
    {
        private ERP.Services.Entities.MpsProcessSetService MPSProcessSet { get; set; }

        public MPSProcessSetService(
            ERP.Services.Entities.MpsProcessSetService mpsMpsProcessSetService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcessSet = mpsMpsProcessSetService;
        }

        public IQueryable<Models.Views.MPSProcessSet> Get()
        {
            return this.MPSProcessSet.Get().Select(i => new Models.Views.MPSProcessSet {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ProcessSetName = i.ProcessSetName,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime
            });
        }
        public Models.Views.MPSProcessSet Create(Models.Views.MPSProcessSet item)
        {
            var _item = MPSProcessSet.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcessSet Update(Models.Views.MPSProcessSet item)
        {
            var _item = MPSProcessSet.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcessSet item)
        {
            MPSProcessSet.Remove(Build(item));
        }
        private Models.Entities.MpsProcessSet Build(Models.Views.MPSProcessSet item)
        {
            return new Models.Entities.MpsProcessSet()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ProcessSetName = item.ProcessSetName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    
    }
}
