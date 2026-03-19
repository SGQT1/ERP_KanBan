using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSDailyAddService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.MPSDailyAddService MPSDailyAdd { get; set; }
        private ERP.Services.Business.Entities.MPSDailyMaterialAddService MPSDailyMaterialAdd { get; set; }
        private ERP.Services.Business.Entities.MPSDailyMaterialItemAddService MPSDailyMaterialItemAdd { get; set; }

        private ERP.Services.Business.Entities.MPSDailyService MPSDaily { get; set; }
        private ERP.Services.Business.Entities.MPSDailyMaterialService MPSDailyMaterial { get; set; }
        private ERP.Services.Business.Entities.MPSDailyMaterialItemService MPSDailyMaterialItem { get; set; }

        private ERP.Services.Entities.MpsOrdersService MPSOrders { get; set; }
        private ERP.Services.Entities.MpsOrdersItemService MPSOrdersItem { get; set; }
        private ERP.Services.Entities.MpsLiveService MPSLive { get; set; }
        private ERP.Services.Entities.MpsLiveItemService MPSLiveItem { get; set; }
        private ERP.Services.Entities.MpsLiveItemSizeService MPSLiveItemSize { get; set; }
        private ERP.Services.Entities.MpsStyleService MPSStyle { get; set; }
        private ERP.Services.Entities.MpsStyleItemService MPSStyleItem { get; set; }
        private ERP.Services.Entities.MpsStyleItemUsageService MPSStyleItemUsage { get; set; }
        private ERP.Services.Entities.MpsProcessService MPSProcess { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.PartService Part { get; set; }
        private ERP.Services.Entities.MpsProcessOrgService MPSProcessOrg { get; set; }
        private ERP.Services.Entities.MaterialStockService MaterialStock { get; set; }
        public MPSDailyAddService(
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.MPSDailyAddService mpsDailyAddService,
            ERP.Services.Business.Entities.MPSDailyMaterialAddService mpsDailyMaterialAddService,
            ERP.Services.Business.Entities.MPSDailyMaterialItemAddService mpsDailyMaterialItemAddService,

            ERP.Services.Business.Entities.MPSDailyService mpsDailyService,
            ERP.Services.Business.Entities.MPSDailyMaterialService mpsDailyMaterialService,
            ERP.Services.Business.Entities.MPSDailyMaterialItemService mpsDailyMaterialItemService,
            ERP.Services.Entities.MpsOrdersService mpsOrdersService,
            ERP.Services.Entities.MpsOrdersItemService mpsOrdersItemService,
            ERP.Services.Entities.MpsLiveService mpsLiveService,
            ERP.Services.Entities.MpsLiveItemService mpsLiveItemService,
            ERP.Services.Entities.MpsLiveItemSizeService mpsLiveItemSizeService,
            ERP.Services.Entities.MpsStyleService mpsStyleService,
            ERP.Services.Entities.MpsStyleItemService mpsStyleItemService,
            ERP.Services.Entities.MpsStyleItemUsageService mpsStyleItemUsageService,
            ERP.Services.Entities.MpsProcessService mpsProcessService,
            ERP.Services.Entities.MpsProcessOrgService mpsProcessOrgService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.PartService partService,
            ERP.Services.Entities.MaterialStockService materialStockService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            MPSDailyAdd = mpsDailyAddService;
            MPSDailyMaterialAdd = mpsDailyMaterialAddService;
            MPSDailyMaterialItemAdd = mpsDailyMaterialItemAddService;

            MPSDaily = mpsDailyService;
            MPSDailyMaterial = mpsDailyMaterialService;
            MPSDailyMaterialItem = mpsDailyMaterialItemService;

            MPSOrders = mpsOrdersService;
            MPSOrdersItem = mpsOrdersItemService;
            MPSLive = mpsLiveService;
            MPSLiveItem = mpsLiveItemService;
            MPSLiveItemSize = mpsLiveItemSizeService;
            MPSStyle = mpsStyleService;
            MPSStyleItem = mpsStyleItemService;
            MPSStyleItemUsage = mpsStyleItemUsageService;
            MPSProcess = mpsProcessService;
            MPSProcessOrg = mpsProcessOrgService;
            Material = materialService;
            CodeItem = codeItemService;
            Part = partService;
            MaterialStock = materialStockService;
        }

        public ERP.Models.Views.MPSDailyAddGroup Get(int id, int localeId)
        {
            var group = new MPSDailyAddGroup();
            // var partsGroup = new List<MPSDailyMaterialGroup>();

            var mpsDailyAdd = MPSDailyAdd.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (mpsDailyAdd != null)
            {
                // get Dispatch Qty(Size Run)
                var mpsMateraialAdd = MPSDailyMaterialAdd.Get().Where(i => i.MpsDailyAddId == mpsDailyAdd.Id && i.LocaleId == mpsDailyAdd.LocaleId).ToList();
                var ids = mpsMateraialAdd.Select(i => i.Id).ToArray();
                var mpsMateraialItemAdd = MPSDailyMaterialItemAdd.Get().Where(i => i.LocaleId == mpsDailyAdd.LocaleId && ids.Contains(i.MpsDailyMaterialAddId)).ToList();


                // 取得最新的MPSStyleItemUsage
                var mpsStyleItemIds = mpsMateraialAdd.Select(i => i.MpsStyleItemId);
                var mpsStyleItemUsage = MPSStyleItemUsage.Get().Where(i => i.LocaleId == localeId && mpsStyleItemIds.Contains(i.MpsStyleItemId));

                var newMateraialItemAdd = new List<Models.Views.MPSDailyMaterialItemAdd>();
                mpsMateraialAdd.ForEach(i =>
                {
                    var items = new List<Models.Views.MPSDailyMaterialItemAdd>();

                    if (i.AlternateType == 0)
                    {
                        items = (
                            from s in MPSOrders.Get().Where(i => i.OrderNo == mpsDailyAdd.OrderNo && i.LocaleId == mpsDailyAdd.LocaleId)
                            join o in MPSOrdersItem.Get() on new { MpsOrdersId = s.Id, LocaleId = s.LocaleId } equals new { MpsOrdersId = o.MpsOrdersId, LocaleId = o.LocaleId }
                            join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.ArticleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = (decimal)u.ArticleInnerSize, LocaleId = u.LocaleId }
                            select new Models.Views.MPSDailyMaterialItemAdd
                            {
                                Id = 0,
                                LocaleId = localeId,
                                MpsDailyMaterialAddId = 0,
                                SubQty = 0,
                                UnitUsage = u.UnitUsage,
                                SubUsage = 0,
                                PreSubUsage = 0,
                                ModifyUserName = "",
                                LastUpdateTime = DateTime.Now,
                                ArticleInnerSize = o.ArticleInnerSize,
                                DisplaySize = o.DisplaySize,
                                MpsStyleItemId = u.MpsStyleItemId,

                                LSubQty = 0,
                                RSubQty = 0,
                            }
                        )
                        .ToList();
                    }
                    else if (i.AlternateType == 1)
                    {
                        items = (
                            from s in MPSOrders.Get().Where(i => i.OrderNo == mpsDailyAdd.OrderNo && i.LocaleId == mpsDailyAdd.LocaleId)
                            join o in MPSOrdersItem.Get() on new { MpsOrdersId = s.Id, LocaleId = s.LocaleId } equals new { MpsOrdersId = o.MpsOrdersId, LocaleId = o.LocaleId }
                            join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.KnifeInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                            select new Models.Views.MPSDailyMaterialItemAdd
                            {
                                Id = 0,
                                LocaleId = localeId,
                                MpsDailyMaterialAddId = 0,
                                SubQty = 0,
                                UnitUsage = u.UnitUsage,
                                SubUsage = 0,
                                PreSubUsage = 0,
                                ModifyUserName = "",
                                LastUpdateTime = DateTime.Now,
                                ArticleInnerSize = o.ArticleInnerSize,
                                DisplaySize = o.DisplaySize,
                                MpsStyleItemId = u.MpsStyleItemId,

                                LSubQty = 0,
                                RSubQty = 0,
                            }
                        )
                        .ToList();
                    }
                    else if (i.AlternateType == 2)
                    {
                        items = (
                            from s in MPSOrders.Get().Where(i => i.OrderNo == mpsDailyAdd.OrderNo && i.LocaleId == mpsDailyAdd.LocaleId)
                            join o in MPSOrdersItem.Get() on new { MpsOrdersId = s.Id, LocaleId = s.LocaleId } equals new { MpsOrdersId = o.MpsOrdersId, LocaleId = o.LocaleId }
                            join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.OutsoleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                            select new Models.Views.MPSDailyMaterialItemAdd
                            {
                                Id = 0,
                                LocaleId = localeId,
                                MpsDailyMaterialAddId = 0,
                                SubQty = 0,
                                UnitUsage = u.UnitUsage,
                                SubUsage = 0,
                                PreSubUsage = 0,
                                ModifyUserName = "",
                                LastUpdateTime = DateTime.Now,
                                ArticleInnerSize = o.ArticleInnerSize,
                                DisplaySize = o.DisplaySize,
                                MpsStyleItemId = u.MpsStyleItemId,

                                LSubQty = 0,
                                RSubQty = 0,
                            }
                        )
                        .ToList();
                    }
                    else if (i.AlternateType == 3)
                    {
                        items = (
                            from s in MPSOrders.Get().Where(i => i.OrderNo == mpsDailyAdd.OrderNo && i.LocaleId == mpsDailyAdd.LocaleId)
                            join o in MPSOrdersItem.Get() on new { MpsOrdersId = s.Id, LocaleId = s.LocaleId } equals new { MpsOrdersId = o.MpsOrdersId, LocaleId = o.LocaleId }
                            join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.LastInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                            select new Models.Views.MPSDailyMaterialItemAdd
                            {
                                Id = 0,
                                LocaleId = localeId,
                                MpsDailyMaterialAddId = 0,
                                SubQty = 0,
                                UnitUsage = u.UnitUsage,
                                SubUsage = 0,
                                PreSubUsage = 0,
                                ModifyUserName = "",
                                LastUpdateTime = DateTime.Now,
                                ArticleInnerSize = o.ArticleInnerSize,
                                DisplaySize = o.DisplaySize,
                                MpsStyleItemId = u.MpsStyleItemId,

                                LSubQty = 0,
                                RSubQty = 0,
                            }
                        )
                        .ToList();
                    }
                    else if (i.AlternateType == 4)
                    {
                        items = (
                           from s in MPSOrders.Get().Where(i => i.OrderNo == mpsDailyAdd.OrderNo && i.LocaleId == mpsDailyAdd.LocaleId)
                           join o in MPSOrdersItem.Get() on new { MpsOrdersId = s.Id, LocaleId = s.LocaleId } equals new { MpsOrdersId = o.MpsOrdersId, LocaleId = o.LocaleId }
                           join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.ShellInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                           select new Models.Views.MPSDailyMaterialItemAdd
                           {
                               Id = 0,
                               LocaleId = localeId,
                               MpsDailyMaterialAddId = 0,
                               SubQty = 0,
                               UnitUsage = u.UnitUsage,
                               SubUsage = 0,
                               PreSubUsage = 0,
                               ModifyUserName = "",
                               LastUpdateTime = DateTime.Now,
                               ArticleInnerSize = o.ArticleInnerSize,
                               DisplaySize = o.DisplaySize,
                               MpsStyleItemId = u.MpsStyleItemId,

                               LSubQty = 0,
                               RSubQty = 0,
                           }
                        )
                        .ToList();
                    }
                    else if (i.AlternateType == 5)
                    {
                        items = (
                            from s in MPSOrders.Get().Where(i => i.OrderNo == mpsDailyAdd.OrderNo && i.LocaleId == mpsDailyAdd.LocaleId)
                            join o in MPSOrdersItem.Get() on new { MpsOrdersId = s.Id, LocaleId = s.LocaleId } equals new { MpsOrdersId = o.MpsOrdersId, LocaleId = o.LocaleId }
                            join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.Other1InnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                            select new Models.Views.MPSDailyMaterialItemAdd
                            {
                                Id = 0,
                                LocaleId = localeId,
                                MpsDailyMaterialAddId = 0,
                                SubQty = 0,
                                UnitUsage = u.UnitUsage,
                                SubUsage = 0,
                                PreSubUsage = 0,
                                ModifyUserName = "",
                                LastUpdateTime = DateTime.Now,
                                ArticleInnerSize = o.ArticleInnerSize,
                                DisplaySize = o.DisplaySize,
                                MpsStyleItemId = u.MpsStyleItemId,

                                LSubQty = 0,
                                RSubQty = 0,
                            }
                        )
                        .ToList();
                    }
                    else if (i.AlternateType == 6)
                    {
                        items = (
                            from s in MPSOrders.Get().Where(i => i.OrderNo == mpsDailyAdd.OrderNo && i.LocaleId == mpsDailyAdd.LocaleId)
                            join o in MPSOrdersItem.Get() on new { MpsOrdersId = s.Id, LocaleId = s.LocaleId } equals new { MpsOrdersId = o.MpsOrdersId, LocaleId = o.LocaleId }
                            join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.Other2InnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                            select new Models.Views.MPSDailyMaterialItemAdd
                            {
                                Id = 0,
                                LocaleId = localeId,
                                MpsDailyMaterialAddId = 0,
                                SubQty = 0,
                                UnitUsage = u.UnitUsage,
                                SubUsage = 0,
                                PreSubUsage = 0,
                                ModifyUserName = "",
                                LastUpdateTime = DateTime.Now,
                                ArticleInnerSize = o.ArticleInnerSize,
                                DisplaySize = o.DisplaySize,
                                MpsStyleItemId = u.MpsStyleItemId,

                                LSubQty = 0,
                                RSubQty = 0,
                            }
                        )
                        .ToList();
                    }

                    if (items.Any())
                    {
                        newMateraialItemAdd.AddRange(items);
                    }
                });

                newMateraialItemAdd.ForEach(i =>
                {
                    i.MpsDailyMaterialAddId = mpsMateraialAdd.Where(m => m.MpsStyleItemId == i.MpsStyleItemId).Max(m => m.Id);
                });

                group.MPSDailyAdd = mpsDailyAdd;
                group.MPSDailyMaterialAdd = mpsMateraialAdd;
                group.MPSDailyMaterialItemAdd = mpsMateraialItemAdd;
                group.NewDailyMaterialItemAdd = newMateraialItemAdd;
            }
            return group;
        }
        public ERP.Models.Views.MPSDailyPartGroup GetMPSMaterial(string predicate, string[] filters)
        {
            var group = new MPSDailyPartGroup();
            var withDaily = false;
            var material = "";
            // var orderNo = "";

            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                withDaily = (Boolean)extenFilters.Field9;
                material = (string)extenFilters.Field5;
                // orderNo = (string)extenFilters.Field6;
            }

            var items = (
               from mli in MPSLiveItem.Get()
               join ml in MPSLive.Get() on new { MPSLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId } equals new { MPSLiveId = ml.Id, LocaleId = ml.LocaleId }
               join o in MPSOrders.Get() on new { MPSOrdersId = ml.MpsOrdersId, LocaleId = ml.LocaleId } equals new { MPSOrdersId = o.Id, LocaleId = o.LocaleId }
               join md in MPSDaily.Get() on new { MPSLiveItemId = mli.Id, LocaleId = mli.LocaleId } equals new { MPSLiveItemId = md.MpsLiveItemId, LocaleId = md.LocaleId } into mdGRP
               from md in mdGRP.DefaultIfEmpty()
               join p in MPSProcess.Get() on new { MPSProcessId = ml.ProcessId, LocaleId = ml.LocaleId } equals new { MPSProcessId = p.Id, LocaleId = p.LocaleId } into pGRP
               from p in pGRP.DefaultIfEmpty()
               select new
               {
                   Id = mli.Id,
                   MPSLiveId = mli.MpsLiveId,
                   PlanDate = mli.PlanDate,
                   PlanQty = mli.PlanQty,
                   ModifyUserName = mli.ModifyUserName,
                   LastUpdateTime = mli.LastUpdateTime,
                   LocaleId = mli.LocaleId,
                   OrderNo = o.OrderNo,
                   MPSProcessId = ml.ProcessId,
                   MPSProcessNo = p.ProcessNo,
                   MPSProcess = p.ProcessNameTw,
                   StyleNo = o.StyleNo,
                   MPSDailyId = (decimal?)md.Id,
               })
               .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
               .ToList();

            if (items.Any())
            {
                // 取得所有派工單ID,
                var hasMPSDailyIds = items.Where(i => i.MPSDailyId > 0).Select(i => i.MPSDailyId).Distinct().ToList();

                // group每個工序下有多少派工單
                var dailyPlan = items.GroupBy(g => new { g.Id, g.MPSLiveId, g.PlanDate, g.PlanQty, g.LocaleId, g.OrderNo, g.StyleNo, g.MPSProcess, g.MPSProcessId })
                    .Select(g => new ERP.Models.Views.MPSDailyPlan
                    {
                        Id = g.Key.Id,
                        MPSLiveId = g.Key.MPSLiveId,
                        PlanDate = g.Key.PlanDate,
                        PlanQty = g.Key.PlanQty,
                        LocaleId = g.Key.LocaleId,
                        OrderNo = g.Key.OrderNo,
                        MPSProcessId = g.Key.MPSProcessId,
                        MPSProcess = g.Key.MPSProcess,
                        StyleNo = g.Key.StyleNo,
                        HasDaily = g.Count(i => i.MPSDailyId > 0),
                    })
                    .Distinct()
                    .ToList();

                var orderNo = dailyPlan[0].OrderNo;
                var styleNo = dailyPlan[0].StyleNo;
                var localeId = dailyPlan[0].LocaleId;


                // 取部位的材料篩選是把整個部位材料取回後再篩選，取得所有部位
                var dailyPlanItem = (
                    from s in MPSStyle.Get()
                    join si in MPSStyleItem.Get() on new { MPSStyleId = s.Id, LocaleId = s.LocaleId } equals new { MPSStyleId = si.MpsStyleId, LocaleId = si.LocaleId }
                    join m in Material.Get() on new { Material = si.MaterialNameTw, LocaleId = si.LocaleId } equals new { Material = m.MaterialName, LocaleId = m.LocaleId } into mGRP
                    from m in mGRP.DefaultIfEmpty()
                    join p in Part.Get() on new { Part = si.PartNameTw, LocaleId = si.LocaleId } equals new { Part = p.PartNameTw, LocaleId = p.LocaleId } into pGRP
                    from p in pGRP.DefaultIfEmpty()
                    where s.LocaleId == localeId && s.StyleNo == styleNo
                    select new MPSDailyPlanItem
                    {
                        Id = si.Id,
                        MpsStyleId = si.MpsStyleId,
                        PartNameTw = si.PartNameTw,
                        MaterialNameTw = si.MaterialNameTw,
                        UnitNameTw = si.UnitNameTw,
                        AlternateType = si.AlternateType,
                        ModifyUserName = si.ModifyUserName,
                        LastUpdateTime = si.LastUpdateTime,
                        LocaleId = si.LocaleId,
                        MaterialId = m == null ? 0 : m.Id,
                        HasDailyMaterial = MPSDailyMaterial.Get().Where(i => hasMPSDailyIds.Contains(i.MPSDailyId) && i.MpsStyleItemId == si.Id && i.LocaleId == localeId).Count(),
                        PartNo = p.PartNo
                    }
                )
                .ToList();

                // 如果有材料條件的篩選
                if (material != null && material.Length > 0)
                {
                    dailyPlanItem = dailyPlanItem.Where(i => i.MaterialNameTw.Contains(material)).ToList();
                }

                // 只選有派工單
                if (withDaily)
                {
                    dailyPlan = dailyPlan.Where(i => i.HasDaily > 0).ToList();
                    dailyPlanItem = dailyPlanItem.Where(i => i.HasDailyMaterial > 0).OrderBy(i => i.PartNameTw).ToList();
                }
                // 排除訂單專用，因為訂單專用有特別處理
                dailyPlanItem = dailyPlanItem.Where(i => i.PartNameTw != "訂單專用").ToList();

                group.MPSDailyPlan = dailyPlan.OrderBy(i => i.MPSProcess).ToList();
                group.MPSDailyPlanItem = dailyPlanItem.OrderBy(i => i.MaterialNameTw).ThenBy(i => i.PartNameTw).ToList();
            }

            return group;
        }
        public ERP.Models.Views.MPSDailyAddGroup BuildMPSDailyAdd(int mpsPlanItemId, List<decimal> mpsStyleItemIds, int localeId)
        {
            var group = new MPSDailyAddGroup();

            var mpsPlanItem = (
                from mli in MPSLiveItem.Get()
                join ml in MPSLive.Get() on new { MPSLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId } equals new { MPSLiveId = ml.Id, LocaleId = ml.LocaleId }
                join mo in MPSOrders.Get() on new { MPOrdersId = ml.MpsOrdersId, LocaleId = ml.LocaleId } equals new { MPOrdersId = mo.Id, LocaleId = mo.LocaleId }
                join mp in MPSProcess.Get() on new { MPSProcessId = ml.ProcessId, LocaleId = ml.LocaleId } equals new { MPSProcessId = mp.Id, LocaleId = mp.LocaleId } into mpGrp
                from mp in mpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = mo.OrderNo } equals new { OrderNo = o.OrderNo } into oGrp
                from o in oGrp.DefaultIfEmpty()
                where mli.Id == mpsPlanItemId && mli.LocaleId == localeId
                select new
                {
                    Id = mli.Id,
                    OrderNo = mo.OrderNo,
                    Qty = mli.PlanQty,
                    MpsLiveItemId = mli.Id,
                    StyleNo = mo.StyleNo,
                    SizeCountryCodeId = mo.SizeCountryCodeId,
                    SizeCountryCode = CodeItem.Get().Where(i => i.Id == mo.SizeCountryCodeId && i.LocaleId == mo.LocaleId).Select(i => i.NameTW).FirstOrDefault(),

                    CSD = mo.CSD,
                    ShoeName = o.ShoeName,
                    ArticleNo = o.ArticleNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,

                    MPSProcessId = ml.ProcessId,
                    MPSProcessNameEn = mp.ProcessNameEn,
                    MPSProcessNameTw = mp.ProcessNameTw,
                    MPSProcessOrgId = MPSProcessOrg.Get().Where(i => i.ProcessId == ml.ProcessId && i.LocaleId == ml.LocaleId).Select(i => i.OrgUnitId).FirstOrDefault(),
                }
            )
            .FirstOrDefault();

            var mpsDailyMaterialAdd = (
                    from si in MPSStyleItem.Get()
                    join s in MPSStyle.Get() on new { MPSStyleId = si.MpsStyleId, LocaleId = si.LocaleId } equals new { MPSStyleId = s.Id, LocaleId = s.LocaleId }
                    join m in Material.Get() on new { Material = si.MaterialNameTw, LocaleId = si.LocaleId } equals new { Material = m.MaterialName, LocaleId = m.LocaleId }
                    join p in Part.Get() on new { Part = si.PartNameTw, LocaleId = si.LocaleId } equals new { Part = p.PartNameTw, LocaleId = p.LocaleId } into pGRP
                    from p in pGRP.DefaultIfEmpty()
                    where s.LocaleId == localeId && mpsStyleItemIds.Contains(si.Id)
                    select new Models.Views.MPSDailyMaterialAdd
                    {
                        Id = 0,
                        LocaleId = m.LocaleId,
                        MpsDailyAddId = 0,
                        PartNo = p.PartNo,
                        PartNameTw = si.PartNameTw,
                        MaterialNameTw = m.MaterialName,
                        UnitNameTw = si.UnitNameTw,
                        AlternateType = si.AlternateType,
                        PreTotalUsage = 0,
                        SubMulti = 0,
                        TotalUsage = 0,
                        ProceduresId = 0,
                        MpsStyleItemId = si.Id,
                        WarehouseNo = MaterialStock.Get().Where(i => i.LocaleId == m.LocaleId && i.MaterialId == m.Id && i.OrderNo == mpsPlanItem.OrderNo).Select(i => i.WarehouseNo).FirstOrDefault(),
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        MpsStyleId = s.Id,
                    }
                )
                .OrderBy(i => i.PartNameTw)
                .ToList();

            var mpsStyleItemUsage = MPSStyleItemUsage.Get().Where(i => i.LocaleId == localeId && mpsStyleItemIds.Contains(i.MpsStyleItemId));
            var mpsPlanItemSize = MPSLiveItemSize.Get().Where(i => i.MpsLiveItemId == mpsPlanItemId && i.LocaleId == localeId);

            var mpsMaterialItemAdd = new List<Models.Views.MPSDailyMaterialItemAdd>();
            mpsDailyMaterialAdd.ForEach(i =>
            {
                var items = new List<Models.Views.MPSDailyMaterialItemAdd>();

                if (i.AlternateType == 0)
                {
                    items = (
                        from s in mpsPlanItemSize
                        join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                        join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.ArticleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = (decimal)u.ArticleInnerSize, LocaleId = u.LocaleId }
                        select new Models.Views.MPSDailyMaterialItemAdd
                        {
                            Id = 0,
                            LocaleId = localeId,
                            MpsDailyMaterialAddId = 0,
                            SubQty = 0,
                            UnitUsage = u.UnitUsage,
                            SubUsage = 0,
                            PreSubUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ArticleInnerSize = o.ArticleInnerSize,
                            DisplaySize = o.DisplaySize,
                            MpsStyleItemId = u.MpsStyleItemId,

                            LSubQty = 0,
                            RSubQty = 0,
                        }
                    )
                    .ToList();
                }
                else if (i.AlternateType == 1)
                {
                    items = (
                        from s in mpsPlanItemSize
                        join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                        join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.KnifeInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                        select new Models.Views.MPSDailyMaterialItemAdd
                        {
                            Id = 0,
                            LocaleId = localeId,
                            MpsDailyMaterialAddId = 0,
                            SubQty = 0,
                            UnitUsage = u.UnitUsage,
                            SubUsage = 0,
                            PreSubUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ArticleInnerSize = o.ArticleInnerSize,
                            DisplaySize = o.DisplaySize,
                            MpsStyleItemId = u.MpsStyleItemId,

                            LSubQty = 0,
                            RSubQty = 0,
                        }
                    )
                    .ToList();
                }
                else if (i.AlternateType == 2)
                {
                    items = (
                        from s in mpsPlanItemSize
                        join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                        join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.OutsoleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                        select new Models.Views.MPSDailyMaterialItemAdd
                        {
                            Id = 0,
                            LocaleId = localeId,
                            MpsDailyMaterialAddId = 0,
                            SubQty = 0,
                            UnitUsage = u.UnitUsage,
                            SubUsage = 0,
                            PreSubUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ArticleInnerSize = o.ArticleInnerSize,
                            DisplaySize = o.DisplaySize,
                            MpsStyleItemId = u.MpsStyleItemId,

                            LSubQty = 0,
                            RSubQty = 0,
                        }
                    )
                    .ToList();
                }
                else if (i.AlternateType == 3)
                {
                    items = (
                        from s in mpsPlanItemSize
                        join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                        join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.LastInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                        select new Models.Views.MPSDailyMaterialItemAdd
                        {
                            Id = 0,
                            LocaleId = localeId,
                            MpsDailyMaterialAddId = 0,
                            SubQty = 0,
                            UnitUsage = u.UnitUsage,
                            SubUsage = 0,
                            PreSubUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ArticleInnerSize = o.ArticleInnerSize,
                            DisplaySize = o.DisplaySize,
                            MpsStyleItemId = u.MpsStyleItemId,

                            LSubQty = 0,
                            RSubQty = 0,
                        }
                    )
                    .ToList();
                }
                else if (i.AlternateType == 4)
                {
                    items = (
                        from s in mpsPlanItemSize
                        join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                        join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.ShellInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                        select new Models.Views.MPSDailyMaterialItemAdd
                        {
                            Id = 0,
                            LocaleId = localeId,
                            MpsDailyMaterialAddId = 0,
                            SubQty = 0,
                            UnitUsage = u.UnitUsage,
                            SubUsage = 0,
                            PreSubUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ArticleInnerSize = o.ArticleInnerSize,
                            DisplaySize = o.DisplaySize,
                            MpsStyleItemId = u.MpsStyleItemId,

                            LSubQty = 0,
                            RSubQty = 0,
                        }
                    )
                    .ToList();
                }
                else if (i.AlternateType == 5)
                {
                    items = (
                        from s in mpsPlanItemSize
                        join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                        join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.Other1InnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                        select new Models.Views.MPSDailyMaterialItemAdd
                        {
                            Id = 0,
                            LocaleId = localeId,
                            MpsDailyMaterialAddId = 0,
                            SubQty = 0,
                            UnitUsage = u.UnitUsage,
                            SubUsage = 0,
                            PreSubUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ArticleInnerSize = o.ArticleInnerSize,
                            DisplaySize = o.DisplaySize,
                            MpsStyleItemId = u.MpsStyleItemId,

                            LSubQty = 0,
                            RSubQty = 0,
                        }
                    )
                    .ToList();
                }
                else if (i.AlternateType == 6)
                {
                    items = (
                        from s in mpsPlanItemSize
                        join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                        join u in mpsStyleItemUsage.Where(m => m.MpsStyleItemId == i.MpsStyleItemId) on new { ArticleInnerSize = o.Other2InnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = u.ArticleInnerSize, LocaleId = u.LocaleId }
                        select new Models.Views.MPSDailyMaterialItemAdd
                        {
                            Id = 0,
                            LocaleId = localeId,
                            MpsDailyMaterialAddId = 0,
                            SubQty = 0,
                            UnitUsage = u.UnitUsage,
                            SubUsage = 0,
                            PreSubUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            ArticleInnerSize = o.ArticleInnerSize,
                            DisplaySize = o.DisplaySize,
                            MpsStyleItemId = u.MpsStyleItemId,

                            LSubQty = 0,
                            RSubQty = 0,
                        }
                    )
                    .ToList();
                }

                if (items.Any())
                {
                    mpsMaterialItemAdd.AddRange(items);
                }
            });

            if (mpsPlanItem != null)
            {
                var mpsStyleItem = mpsDailyMaterialAdd[0]; //只為了取材料名稱，不要做其他用途

                // MPSDaily
                var mpsDailyAdd = new ERP.Models.Views.MPSDailyAdd
                {
                    Id = 0,
                    LocaleId = localeId,
                    DailyNo = "",
                    PreDailyNo = "",
                    OpenDate = DateTime.Today,
                    DailyDate = DateTime.Today,
                    FinishedDate = DateTime.Today,
                    ProcessId = mpsPlanItem.MPSProcessId, // ProcessId
                    ProcessUnitId = mpsPlanItem.MPSProcessOrgId,
                    OrderNo = mpsPlanItem.OrderNo,
                    MpsStyleId = mpsStyleItem.MpsStyleId,
                    OrderQty = mpsPlanItem.Qty,
                    Qty = 0,
                    SizeCountryNameTw = mpsPlanItem.SizeCountryCode,
                    DailyMode = 1,
                    DailyType = 3,
                    DoDaily = 1,
                    // DoDate = m.DoDate,
                    // Multi = m.Multi,
                    Remark = "",
                    SeqId = 0,
                    MaterialCost = 0,
                    DollarNameTw = "NTD",
                    CostBalance = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,

                    ProcessUnitTw = mpsStyleItem.UnitNameTw,
                    StyleNo = mpsPlanItem.StyleNo,
                    ShoeName = mpsPlanItem.ShoeName,
                    ArticleNo = mpsPlanItem.ArticleNo,
                    CSD = mpsPlanItem.CSD,
                    CompanyId = mpsPlanItem.CompanyId,
                    CompanyNo = mpsPlanItem.CompanyNo,
                    SizeCountryCodeId = mpsPlanItem.SizeCountryCodeId,
                    MPSProcessNameTw = mpsPlanItem.MPSProcessNameTw,
                    MPSProcessNameEn = mpsPlanItem.MPSProcessNameEn,
                };

                // MPSDailyMaterial
                mpsDailyMaterialAdd.ForEach(i =>
                {
                    i.TotalUsage = mpsMaterialItemAdd.Where(mi => mi.MpsStyleItemId == i.MpsStyleItemId).Sum(mi => mi.SubUsage);
                });
                group.MPSDailyAdd = mpsDailyAdd;
                group.MPSDailyMaterialAdd = mpsDailyMaterialAdd;
                group.MPSDailyMaterialItemAdd = mpsMaterialItemAdd.Distinct();
                group.NewDailyMaterialItemAdd = mpsMaterialItemAdd.Distinct(); ;
            }
            return group;
        }
        public ERP.Models.Views.MPSDailyAddGroup BuildMPSDailyAdd1(int mpsPlanItemId, List<decimal> mpsStyleItemIds, int localeId)
        {
            var group = new MPSDailyAddGroup();

            var mpsPlanItem = (
                from mli in MPSLiveItem.Get()
                join ml in MPSLive.Get() on new { MPSLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId } equals new { MPSLiveId = ml.Id, LocaleId = ml.LocaleId }
                join mo in MPSOrders.Get() on new { MPOrdersId = ml.MpsOrdersId, LocaleId = ml.LocaleId } equals new { MPOrdersId = mo.Id, LocaleId = mo.LocaleId }
                join mp in MPSProcess.Get() on new { MPSProcessId = ml.ProcessId, LocaleId = ml.LocaleId } equals new { MPSProcessId = mp.Id, LocaleId = mp.LocaleId } into mpGrp
                from mp in mpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = mo.OrderNo } equals new { OrderNo = o.OrderNo } into oGrp
                from o in oGrp.DefaultIfEmpty()
                where mli.Id == mpsPlanItemId && mli.LocaleId == localeId
                select new
                {
                    Id = mli.Id,
                    OrderNo = mo.OrderNo,
                    Qty = mli.PlanQty,
                    MpsLiveItemId = mli.Id,
                    StyleNo = mo.StyleNo,
                    SizeCountryCodeId = mo.SizeCountryCodeId,
                    SizeCountryCode = CodeItem.Get().Where(i => i.Id == mo.SizeCountryCodeId && i.LocaleId == mo.LocaleId).Select(i => i.NameTW).FirstOrDefault(),

                    CSD = mo.CSD,
                    ShoeName = o.ShoeName,
                    ArticleNo = o.ArticleNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,

                    MPSProcessId = ml.ProcessId,
                    MPSProcessNameEn = mp.ProcessNameEn,
                    MPSProcessNameTw = mp.ProcessNameTw,
                    MPSProcessOrgId = MPSProcessOrg.Get().Where(i => i.ProcessId == ml.ProcessId && i.LocaleId == ml.LocaleId).Select(i => i.OrgUnitId).FirstOrDefault(),
                }
            )
            .FirstOrDefault();

            var mpsDailyMaterialAdd = (
                    from si in MPSStyleItem.Get()
                    join s in MPSStyle.Get() on new { MPSStyleId = si.MpsStyleId, LocaleId = si.LocaleId } equals new { MPSStyleId = s.Id, LocaleId = s.LocaleId }
                    join m in Material.Get() on new { Material = si.MaterialNameTw, LocaleId = si.LocaleId } equals new { Material = m.MaterialName, LocaleId = m.LocaleId }
                    join p in Part.Get() on new { Part = si.PartNameTw, LocaleId = si.LocaleId } equals new { Part = p.PartNameTw, LocaleId = p.LocaleId } into pGRP
                    from p in pGRP.DefaultIfEmpty()
                    where s.LocaleId == localeId && mpsStyleItemIds.Contains(si.Id)
                    select new Models.Views.MPSDailyMaterialAdd
                    {
                        Id = 0,
                        LocaleId = m.LocaleId,
                        MpsDailyAddId = 0,
                        PartNo = p.PartNo,
                        PartNameTw = si.PartNameTw,
                        MaterialNameTw = m.MaterialName,
                        UnitNameTw = si.UnitNameTw,
                        AlternateType = si.AlternateType,
                        PreTotalUsage = 0,
                        SubMulti = 0,
                        TotalUsage = 0,
                        ProceduresId = 0,
                        MpsStyleItemId = si.Id,
                        WarehouseNo = MaterialStock.Get().Where(i => i.LocaleId == m.LocaleId && i.MaterialId == m.Id && i.OrderNo == mpsPlanItem.OrderNo).Select(i => i.WarehouseNo).FirstOrDefault(),
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        MpsStyleId = s.Id,
                    }
                )
                .OrderBy(i => i.PartNameTw)
                .ToList();

            var mpsStyleItemUsage = MPSStyleItemUsage.Get().Where(i => i.LocaleId == localeId && mpsStyleItemIds.Contains(i.MpsStyleItemId));
            var mpsPlanItemSize = MPSLiveItemSize.Get().Where(i => i.MpsLiveItemId == mpsPlanItemId && i.LocaleId == localeId);

            var mpsMateraialItemAdd = (
                from s in mpsPlanItemSize
                join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                join u in mpsStyleItemUsage on new { ArticleInnerSize = o.ArticleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = (decimal)u.ArticleInnerSize, LocaleId = u.LocaleId }
                select new Models.Views.MPSDailyMaterialItemAdd
                {
                    Id = 0,
                    LocaleId = localeId,
                    MpsDailyMaterialAddId = 0,
                    SubQty = 0,
                    UnitUsage = u.UnitUsage,
                    SubUsage = 0,
                    PreSubUsage = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    ArticleInnerSize = o.ArticleInnerSize,
                    DisplaySize = o.DisplaySize,
                    MpsStyleItemId = u.MpsStyleItemId,

                    LSubQty = 0,
                    RSubQty = 0,
                }
            ).ToList();

            if (mpsPlanItem != null)
            {
                var mpsStyleItem = mpsDailyMaterialAdd[0]; //只為了取材料名稱，不要做其他用途

                // MPSDaily
                var mpsDailyAdd = new ERP.Models.Views.MPSDailyAdd
                {
                    Id = 0,
                    LocaleId = localeId,
                    DailyNo = "",
                    PreDailyNo = "",
                    OpenDate = DateTime.Today,
                    DailyDate = DateTime.Today,
                    FinishedDate = DateTime.Today,
                    ProcessId = mpsPlanItem.MPSProcessId, // ProcessId
                    ProcessUnitId = mpsPlanItem.MPSProcessOrgId,
                    OrderNo = mpsPlanItem.OrderNo,
                    MpsStyleId = mpsStyleItem.MpsStyleId,
                    OrderQty = mpsPlanItem.Qty,
                    Qty = 0,
                    SizeCountryNameTw = mpsPlanItem.SizeCountryCode,
                    DailyMode = 1,
                    DailyType = 3,
                    DoDaily = 1,
                    // DoDate = m.DoDate,
                    // Multi = m.Multi,
                    Remark = "",
                    SeqId = 0,
                    MaterialCost = 0,
                    DollarNameTw = "NTD",
                    CostBalance = 0,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,

                    ProcessUnitTw = mpsStyleItem.UnitNameTw,
                    StyleNo = mpsPlanItem.StyleNo,
                    ShoeName = mpsPlanItem.ShoeName,
                    ArticleNo = mpsPlanItem.ArticleNo,
                    CSD = mpsPlanItem.CSD,
                    CompanyId = mpsPlanItem.CompanyId,
                    CompanyNo = mpsPlanItem.CompanyNo,
                    SizeCountryCodeId = mpsPlanItem.SizeCountryCodeId,
                    MPSProcessNameTw = mpsPlanItem.MPSProcessNameTw,
                    MPSProcessNameEn = mpsPlanItem.MPSProcessNameEn,
                };

                // MPSDailyMaterial
                mpsDailyMaterialAdd.ForEach(i =>
                {
                    i.TotalUsage = mpsMateraialItemAdd.Where(mi => mi.MpsStyleItemId == i.MpsStyleItemId).Sum(mi => mi.SubUsage);
                });
                group.MPSDailyAdd = mpsDailyAdd;
                group.MPSDailyMaterialAdd = mpsDailyMaterialAdd;
                group.MPSDailyMaterialItemAdd = mpsMateraialItemAdd;
                group.NewDailyMaterialItemAdd = mpsMateraialItemAdd;
            }
            return group;
        }

        public ERP.Models.Views.MPSDailyAddGroup SaveMPSDailyAddGroup(MPSDailyAddGroup group)
        {
            var dailyAdd = group.MPSDailyAdd;
            var dailyMaterialAdd = group.MPSDailyMaterialAdd.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (dailyAdd != null)
                {
                    //Plan
                    {
                        var _dailyAdd = MPSDailyAdd.Get().Where(i => i.LocaleId == dailyAdd.LocaleId && i.Id == dailyAdd.Id).FirstOrDefault();

                        if (_dailyAdd == null)
                        {
                            // 從所有符合條件中，篩出合法格式並取最大 SeqId
                            var candidateNos = MPSDailyAdd.Get()
                                .Where(i => i.LocaleId == dailyAdd.LocaleId && i.OrderNo == dailyAdd.OrderNo && i.DailyNo != null)
                                .Select(i => i.SeqId)
                                .ToList();

                            int lastSeq = candidateNos.Any() ? candidateNos.Max() : 0;
                            int nextSeq = lastSeq + 1;

                            dailyAdd.DailyNo = $"RE-{dailyAdd.OrderNo}-{nextSeq:000}";
                            dailyAdd.SeqId = nextSeq;

                            dailyAdd = MPSDailyAdd.Create(dailyAdd);
                        }
                        else
                        {
                            dailyAdd.Id = _dailyAdd.Id;
                            dailyAdd.LocaleId = _dailyAdd.LocaleId;
                            dailyAdd = MPSDailyAdd.Update(dailyAdd);
                        }
                    }
                    //items
                    {
                        if (dailyAdd.Id != 0)
                        {
                            var materialAddIds = MPSDailyMaterialAdd.Get().Where(i => i.MpsDailyAddId == dailyAdd.Id && i.LocaleId == dailyAdd.LocaleId).Select(i => i.Id).ToArray();
                            // MPSDailyMaterialAdd.RemoveRange(i => i.MpsDailyAddId == dailyAdd.Id && i.LocaleId == dailyAdd.LocaleId);
                            MPSDailyMaterialAdd.Remove((int)dailyAdd.Id, (int)dailyAdd.LocaleId);
                            MPSDailyMaterialItemAdd.RemoveRange(i => materialAddIds.Contains(i.MpsDailyMaterialAddId) && i.LocaleId == dailyAdd.LocaleId);

                            dailyMaterialAdd.ForEach(m =>
                            {
                                m.MpsDailyAddId = dailyAdd.Id;
                                m.ModifyUserName = dailyAdd.ModifyUserName;
                                m.LastUpdateTime = dailyAdd.LastUpdateTime;
                                m.LocaleId = dailyAdd.LocaleId;

                                var _materialAdd = MPSDailyMaterialAdd.Create(m);

                                var _materialItemAdd = group.MPSDailyMaterialItemAdd
                                    .Where(i => i.MpsStyleItemId == _materialAdd.MpsStyleItemId)
                                    .Select(i => new ERP.Models.Views.MPSDailyMaterialItemAdd
                                    {
                                        Id = i.Id,
                                        LocaleId = i.LocaleId,
                                        MpsDailyMaterialAddId = _materialAdd.Id,
                                        ArticleInnerSize = i.ArticleInnerSize,
                                        DisplaySize = i.DisplaySize,
                                        LSubQty = i.LSubQty,
                                        RSubQty = i.RSubQty,
                                        SubQty = i.SubQty,
                                        UnitUsage = i.UnitUsage,
                                        SubUsage = i.SubUsage,
                                        PreSubUsage = i.PreSubUsage,
                                        ModifyUserName = m.ModifyUserName,
                                        LastUpdateTime = m.LastUpdateTime,

                                        MpsStyleItemId = i.MpsStyleItemId,

                                    }).ToList();

                                MPSDailyMaterialItemAdd.CreateRange(_materialItemAdd);
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
            return Get((int)dailyAdd.Id, (int)dailyAdd.LocaleId);
        }
        public void RemoveMPSDailyAddGroup(MPSDailyAddGroup group)
        {
            var dailyAdd = group.MPSDailyAdd;
            var dailyMaterialAddIds = group.MPSDailyMaterialAdd.Select(i => i.Id);
            try
            {
                UnitOfWork.BeginTransaction();
                if (dailyAdd != null)
                {
                    MPSDailyMaterialItemAdd.RemoveRange(i => dailyMaterialAddIds.Contains(i.MpsDailyMaterialAddId) && i.LocaleId == dailyAdd.LocaleId);
                    // MPSDailyMaterialAdd.RemoveRange(i => i.MpsDailyAddId == dailyAdd.Id && i.LocaleId == dailyAdd.LocaleId);
                    MPSDailyMaterialAdd.Remove((int)dailyAdd.Id, (int)dailyAdd.LocaleId);
                    MPSDailyAdd.Remove(dailyAdd);
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
