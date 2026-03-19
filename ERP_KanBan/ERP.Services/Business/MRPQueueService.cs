using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    /*
     * MRPQueue is QueueDoMRP, SubmitTime is Server Time, order by SubmitTime,OrderNo
     * SaveMRPQueue : remove OrdersId from client side, and create MRPQueue
     * RemoveMRPQueue : remove OrdersId from client side
     * RankUpageMRPQueue: update items SumitTime and before Min(SubmitTime) in Queue, remove exit items and create
     */
    public class MRPQueueService : BusinessService
    {
        private ERP.Services.Business.Entities.MRPQueueService MRPQueue { get; set; }

        public MRPQueueService(
            ERP.Services.Business.Entities.MRPQueueService mrpQueueService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MRPQueue = mrpQueueService;
        }

        public IEnumerable<Models.Views.MRPQueue> SaveMRPQueue(IEnumerable<MRPQueue> mrpQueues)
        {
            var ids = mrpQueues.Select(i => i.OrdersId).ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (ids.Count() > 0)
                {
                    //MRPQueue(create,update) is remove mrpItemOrders and Insert.
                    MRPQueue.RemoveRange(i => ids.Contains(i.OrdersId) && i.LocaleId == mrpQueues.First().LocaleId);
                    mrpQueues.ToList().ForEach(i => { i.SubmitTime = DateTime.Now; });
                    MRPQueue.CreateRange(mrpQueues);
                }
                UnitOfWork.Commit();
                return MRPQueue.Get().OrderBy(i => i.SubmitTime).ThenBy(i => i.OrderNo).ToList();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public IEnumerable<Models.Views.MRPQueue> RemoveMRPQueue(IEnumerable<MRPQueue> mrpQueues)
        {
            var ids = mrpQueues.Select(i => i.OrdersId).ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (ids.Count() > 0)
                {
                    //MRPQueue(create,update) is remove mrpItemOrders and Insert.
                    MRPQueue.RemoveRange(i => ids.Contains(i.OrdersId) && i.LocaleId == mrpQueues.First().LocaleId);
                }
                UnitOfWork.Commit();
                return MRPQueue.Get().OrderBy(i => i.SubmitTime).ThenBy(i => i.OrderNo).ToList();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public IEnumerable<Models.Views.MRPQueue> RankUpageMRPQueue(IEnumerable<MRPQueue> mrpQueues)
        {
            var ids = mrpQueues.Select(i => i.OrdersId).ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (ids.Count() > 0)
                {
                    // step 1: remove task
                    MRPQueue.RemoveRange(i => ids.Contains(i.OrdersId) && i.LocaleId == mrpQueues.First().LocaleId);

                    // step 2: change submitTime and create
                    var submitTime = DateTime.Now;
                    var firstItem = MRPQueue.Get().OrderBy(i => i.SubmitTime).FirstOrDefault();
                    if (firstItem != null)
                    {
                        submitTime = firstItem.SubmitTime.AddSeconds(-1);
                    }
                    mrpQueues.ToList().ForEach(i =>
                    {
                        i.Id = 0;
                        i.SubmitTime = submitTime.AddSeconds(-1);
                    });
                    MRPQueue.CreateRange(mrpQueues);
                }
                UnitOfWork.Commit();
                return MRPQueue.Get().OrderBy(i => i.SubmitTime).ThenBy(i => i.OrderNo).ToList();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
