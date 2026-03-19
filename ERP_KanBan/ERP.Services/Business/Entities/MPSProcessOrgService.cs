using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSProcessOrgService : BusinessService
    {
        private ERP.Services.Entities.MpsProcessOrgService MPSProcessOrg { get; set; }

        public MPSProcessOrgService(
            ERP.Services.Entities.MpsProcessOrgService mpsProcessOrgService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcessOrg = mpsProcessOrgService;
        }

        public IQueryable<Models.Views.MPSProcessOrg> Get()
        {
            return this.MPSProcessOrg.Get().Select(i => new Models.Views.MPSProcessOrg
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ProcessId = i.ProcessId,
                OrgUnitId = i.OrgUnitId,
                DayCapacity = i.DayCapacity,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSProcessOrg> items)
        {
            MPSProcessOrg.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsProcessOrg, bool>> predicate)
        {
            MPSProcessOrg.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsProcessOrg> BuildRange(IEnumerable<Models.Views.MPSProcessOrg> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcessOrg
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ProcessId = item.ProcessId,
                OrgUnitId = item.OrgUnitId,
                DayCapacity = item.DayCapacity,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

    }
}
