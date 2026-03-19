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

    public class MPSLiveService : BusinessService
    {
        private ERP.Services.Entities.MpsLiveService MPSPaln { get; set; }
        private ERP.Services.Entities.MpsOrdersService MPSOrders { get; set; }
        private ERP.Services.Entities.MpsProcessSetService MPSProcessSet { get; set; }
        private ERP.Services.Entities.MpsProcessService MPSProcess { get; set; }


        public MPSLiveService(
            ERP.Services.Entities.MpsLiveService mpsLiveService,
            ERP.Services.Entities.MpsOrdersService mpsOrdersService,
            ERP.Services.Entities.MpsProcessSetService mpsProcessSetService,
            ERP.Services.Entities.MpsProcessService mpsProcessService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSPaln = mpsLiveService;
            MPSOrders = mpsOrdersService;
            MPSProcessSet = mpsProcessSetService;
            MPSProcess = mpsProcessService;
        }

        public IQueryable<Models.Views.MPSPlan> Get()
        {
            var result = (
                from o in MPSOrders.Get()
                join p in MPSPaln.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = p.MpsOrdersId, LocaleId = p.LocaleId }
                select new Models.Views.MPSPlan
                {
                    Id = p.Id,
                    LocaleId = p.LocaleId,
                    ModifyUserName = p.ModifyUserName,
                    LastUpdateTime = p.LastUpdateTime,
                    MPSOrdersId = p.MpsOrdersId,
                    ProcessId = p.ProcessId,
                    Process = MPSProcess.Get().Where(i => i.Id == p.ProcessId && i.LocaleId == p.LocaleId).Max(i => i.ProcessNameTw),

                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    PlanQty = o.Qty,
                    MPSArticleId = o.MpsArticleId,
                    ProcessSetId = o.ProcessSetId,
                    ProcessSet = MPSProcessSet.Get().Where(i => i.Id == o.ProcessSetId && i.LocaleId == o.LocaleId).Max(i => i.ProcessSetName),
                    StyleNo = o.StyleNo,
                    CSD = o.CSD,
                    ETD = o.ETD,
                }
            );
            return result;
        }
        
        public Models.Views.MPSPlan Create(Models.Views.MPSPlan item)
        {
            var _item = MPSPaln.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSPlan Update(Models.Views.MPSPlan item)
        {
            var _item = MPSPaln.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSPlan item)
        {
            MPSPaln.Remove(Build(item));
        }
        private Models.Entities.MpsLive Build(Models.Views.MPSPlan item)
        {
            return new Models.Entities.MpsLive()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                MpsOrdersId = item.MPSOrdersId,
                ProcessId = item.ProcessId,
            };
        }
    }
}
