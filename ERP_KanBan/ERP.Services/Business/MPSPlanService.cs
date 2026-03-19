using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSPlanService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSOrdersService MPSOrders { get; set; }
        private ERP.Services.Business.Entities.MPSOrdersItemService MPSOrdersItem { get; set; }
        private ERP.Services.Business.Entities.MPSLiveService MPSLive { get; set; }
        private ERP.Services.Business.Entities.MPSLiveItemService MPSLiveItem { get; set; }
        private ERP.Services.Business.Entities.MPSLiveItemSizeService MPSLiveItemSize { get; set; }
        private ERP.Services.Business.Entities.MPSProcessSetService MPSProcessSet { get; set; }
        private ERP.Services.Entities.MpsLiveService MPSPaln { get; set; }

        public MPSPlanService(
            ERP.Services.Business.Entities.MPSOrdersService mpsOrdersService,
            ERP.Services.Business.Entities.MPSOrdersItemService mpsOrdersItemService,
            ERP.Services.Business.Entities.MPSLiveService mpsLiveService,
            ERP.Services.Business.Entities.MPSLiveItemService mpsLiveItemService,
            ERP.Services.Business.Entities.MPSLiveItemSizeService mpsLiveItemSizeService,
            ERP.Services.Business.Entities.MPSProcessSetService mpsProcessSetService,
            ERP.Services.Entities.MpsLiveService mpsPlanService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSOrders = mpsOrdersService;
            MPSOrdersItem = mpsOrdersItemService;
            MPSLive = mpsLiveService;
            MPSLiveItem = mpsLiveItemService;
            MPSLiveItemSize = mpsLiveItemSizeService;
            MPSProcessSet = mpsProcessSetService;
            MPSPaln = mpsPlanService;
        }
        public IQueryable<ERP.Models.Views.MPSOrders> GetMPSOrdersForPlan()
        {
            var result = (
                from o in MPSOrders.Get()
                join p in MPSPaln.Get() on new { MpsOrdersId = o.Id, LocaleId = o.LocaleId } equals new { MpsOrdersId = p.MpsOrdersId, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new ERP.Models.Views.MPSOrders
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    ModifyUserName = o.ModifyUserName,
                    LastUpdateTime = o.LastUpdateTime,
                    OrderNo = o.OrderNo,
                    OrderQty = o.OrderQty,
                    Qty = o.Qty,
                    MpsArticleId = o.MpsArticleId,
                    ProcessSetId = o.ProcessSetId,
                    StyleNo = o.StyleNo,
                    SizeCountryCodeId = o.SizeCountryCodeId,
                    IncreaseRate = o.IncreaseRate,
                    ETD = o.ETD,
                    CSD = o.CSD,
                    BaseOn = o.BaseOn,
                    CustomerNameTw = o.CustomerNameTw,
                    ProcessType = o.ProcessType,
                    HasPlan = p == null ? 0 : 1,
                }
            );
            return result;
            // return MPSOrders.Get();
        }
        public ERP.Models.Views.MPSPlanGroup BuildMPSPlan(string orderNo, int localeId)
        {
            var group = new ERP.Models.Views.MPSPlanGroup();

            var plan = MPSLive.Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId).FirstOrDefault();
            if (plan != null)
            {
                return Get((int)plan.Id, (int)plan.LocaleId);
            }

            var mpsOrders = MPSOrders.Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId).FirstOrDefault();
            if (mpsOrders != null)
            {
                group.MPSPlan = new ERP.Models.Views.MPSPlan
                {
                    Id = 0,
                    LocaleId = mpsOrders.LocaleId,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    MPSOrdersId = mpsOrders.Id,
                    ProcessId = 0,
                    // Process = MPSProcess.Get().Where(i => i.Id == p.ProcessId && i.LocaleId == p.LocaleId).Max(i => i.ProcessNameTw),

                    OrderNo = mpsOrders.OrderNo,
                    OrderQty = mpsOrders.OrderQty,
                    PlanQty = mpsOrders.Qty,
                    MPSArticleId = mpsOrders.MpsArticleId,
                    ProcessSetId = mpsOrders.ProcessSetId,
                    ProcessSet = MPSProcessSet.Get().Where(i => i.Id == mpsOrders.ProcessSetId && i.LocaleId == mpsOrders.LocaleId).Max(i => i.ProcessSetName),
                    StyleNo = mpsOrders.StyleNo,
                    CSD = mpsOrders.CSD,
                    ETD = mpsOrders.ETD,
                };
                group.MPSOrdersItem = MPSOrdersItem.Get().Where(i => i.MpsOrdersId == mpsOrders.Id && i.LocaleId == localeId).ToList();
                group.MPSPlanItem = new List<ERP.Models.Views.MPSPlanItem> { };
                group.MPSPlanItemSize = new List<ERP.Models.Views.MPSPlanItemSize> { };
                group.HasMPSPlan = new List<ERP.Models.Views.MPSPlan> { };
            }
            return group;
        }
        public ERP.Models.Views.MPSPlanGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.MPSPlanGroup();

            var plan = MPSLive.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (plan != null)
            {
                group.MPSPlan = plan;
                group.MPSPlanItem = MPSLiveItem.Get().Where(i => i.MPSLiveId == id && i.LocaleId == localeId).ToList();
                group.HasMPSPlan = MPSLive.Get().Where(i => i.MPSOrdersId == plan.MPSOrdersId && i.LocaleId == plan.LocaleId).ToList();
                group.MPSOrdersItem = MPSOrdersItem.Get().Where(i => i.MpsOrdersId == plan.MPSOrdersId && i.LocaleId == plan.LocaleId).ToList();

                if (group.MPSPlanItem.Any())
                {
                    var ids = group.MPSPlanItem.Select(i => i.Id).ToList();
                    var size = MPSLiveItemSize.Get().Where(i => ids.Contains(i.MPSLiveItemId) && i.LocaleId == localeId).ToList();
                    size.ForEach(i =>
                    {
                        i.SumQty = size.Where(s => s.ArticleInnerSize == i.ArticleInnerSize).Sum(s => s.SubQty);
                    });

                    group.MPSPlanItemSize = size;
                }
            }
            return group;
        }
        public ERP.Models.Views.MPSPlanGroup Save(MPSPlanGroup group)
        {
            var plan = group.MPSPlan;
            var planItem = group.MPSPlanItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (plan != null)
                {
                    //Plan
                    {
                        var _procedure = MPSLive.Get().Where(i => i.LocaleId == plan.LocaleId && i.Id == plan.Id).FirstOrDefault();
                        if (_procedure == null)
                        {
                            plan = MPSLive.Create(plan);
                        }
                        else
                        {
                            plan.Id = _procedure.Id;
                            plan.LocaleId = _procedure.LocaleId;
                            plan.MPSOrdersId = _procedure.MPSOrdersId;
                            plan = MPSLive.Update(plan);
                        }
                    }
                    //items
                    {
                        if (plan.Id != 0)
                        {
                            var planItemIds = planItem.Select(i => i.Id).ToArray();
                            MPSLiveItemSize.RemoveRange(i => planItemIds.Contains(i.MpsLiveItemId) && i.LocaleId == plan.LocaleId);
                            MPSLiveItem.RemoveRange(i => i.MpsLiveId == plan.Id && i.LocaleId == plan.LocaleId);

                            planItem.ForEach(pi =>
                            {
                                //先把前端當做識別的Id改掉。
                                pi.Id = pi.MPSLiveId == 0 ? 0 : pi.Id; // Id的欄位有放了一個識別 liveItem 跟 liveItemSize的關聯欄位，所以當新資料的時候，就要把LiveItemId 設定為0

                                pi.MPSLiveId = plan.Id;
                                pi.ModifyUserName = plan.ModifyUserName;
                                pi.LocaleId = plan.LocaleId;

                                var _planItem = MPSLiveItem.Create(pi);

                                var _planItemSize = group.MPSPlanItemSize
                                    .Where(i => i.SeqId == pi.SeqId)
                                    .Select(pis => new ERP.Models.Views.MPSPlanItemSize
                                    {
                                        Id = pis.Id,
                                        ModifyUserName = _planItem.ModifyUserName,
                                        LastUpdateTime = _planItem.LastUpdateTime,
                                        LocaleId = _planItem.LocaleId,
                                        MPSLiveItemId = _planItem.Id,
                                        MPSOrdersItemId = pis.MPSOrdersItemId,
                                        SubQty = pis.SubQty,
                                    }).ToList();

                                MPSLiveItemSize.CreateRange(_planItemSize);
                            });

                        }
                    }
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)plan.Id, (int)plan.LocaleId);
        }
        public void Remove(MPSPlanGroup group)
        {
            var plan = group.MPSPlan;
            var planItemIs = group.MPSPlanItem.Select(i => i.Id);
            try
            {
                UnitOfWork.BeginTransaction();
                if (plan != null)
                {
                    MPSLiveItemSize.RemoveRange(i => planItemIs.Contains(i.MpsLiveItemId) && i.LocaleId == plan.LocaleId);
                    MPSLiveItem.RemoveRange(i => i.MpsLiveId == plan.Id && i.LocaleId == plan.LocaleId);
                    MPSLive.Remove(plan);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
