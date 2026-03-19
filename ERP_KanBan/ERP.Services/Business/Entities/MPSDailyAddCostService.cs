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

    public class MPSDailyAddCostService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyAddCostService MPSDailyAddCost { get; set; }
        private ERP.Services.Entities.MpsStyleItemService MPSStyleItem { get; set; }

        public MPSDailyAddCostService(
            ERP.Services.Entities.MpsDailyAddCostService mpsDailyMaterialAddService,
            ERP.Services.Entities.MpsStyleItemService mpsStyleItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyAddCost = mpsDailyMaterialAddService;
            MPSStyleItem = mpsStyleItemService;
        }

        public IQueryable<Models.Views.MPSDailyAddCost> Get()
        {
            // 暫時取用 MPSStyleItem
            var items = (
                from m in MPSDailyAddCost.Get()
                select new Models.Views.MPSDailyAddCost
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MpsDailyAddId = m.MpsDailyAddId,
                    MaterialNameTw = m.MaterialNameTw,
                    UnitNameTw = m.UnitNameTw,
                    PlanUsage = m.PlanUsage,
                    IOUsage = m.IOUsage,
                    UnitPrice = m.UnitPrice,
                    DollarNameTw = m.DollarNameTw,
                    ExchangeRate = m.ExchangeRate,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                }
            );
            return items;
        }
        public void CreateRange(IEnumerable<Models.Views.MPSDailyAddCost> items)
        {
            MPSDailyAddCost.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsDailyAddCost, bool>> predicate)
        {
            MPSDailyAddCost.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyAddCost> BuildRange(IEnumerable<Models.Views.MPSDailyAddCost> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyAddCost
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsDailyAddId = item.MpsDailyAddId,
                MaterialNameTw = item.MaterialNameTw,
                UnitNameTw = item.UnitNameTw,
                PlanUsage = item.PlanUsage,
                IOUsage = item.IOUsage,
                UnitPrice = item.UnitPrice,
                DollarNameTw = item.DollarNameTw,
                ExchangeRate = item.ExchangeRate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}
