using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using FastReport.Barcode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSDailyService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSDailyService MPSDaily { get; set; }
        private ERP.Services.Business.Entities.MPSDailyMaterialService MPSDailyMaterial { get; set; }
        private ERP.Services.Business.Entities.MPSDailyMaterialItemService MPSDailyMaterialItem { get; set; }
        private ERP.Services.Business.Entities.MPSStyleItemUsageService _MPSStyleItemUsage { get; set; }

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
        private ERP.Services.Entities.StockIOService StockIO { get; set; }
        private ERP.Services.Entities.MpsDailyService MpsDaily { get; set; }
        private ERP.Services.Entities.MpsDailyMaterialService MpsDailyMaterial { get; set; }
        private ERP.Services.Entities.MpsDailyMaterialItemService MpsDailyMaterialItem { get; set; }

        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }

        public MPSDailyService(
            ERP.Services.Business.Entities.MPSDailyService mpsDailyService,
            ERP.Services.Business.Entities.MPSDailyMaterialService mpsDailyMaterialService,
            ERP.Services.Business.Entities.MPSDailyMaterialItemService mpsDailyMaterialItemService,
            ERP.Services.Business.Entities.MPSStyleItemUsageService _mpsStyleItemUsageService,
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
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.MpsDailyService _mpsDailyService,
            ERP.Services.Entities.MpsDailyMaterialService _mpsDailyMaterialService,
            ERP.Services.Entities.MpsDailyMaterialItemService _mpsDailyMaterialItemService,
            ERP.Services.Business.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            MPSDaily = mpsDailyService;
            MPSDailyMaterial = mpsDailyMaterialService;
            MPSDailyMaterialItem = mpsDailyMaterialItemService;
            _MPSStyleItemUsage = _mpsStyleItemUsageService;

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
            StockIO = stockIOService;

            MpsDaily = _mpsDailyService;
            MpsDailyMaterial = _mpsDailyMaterialService;
            MpsDailyMaterialItem = _mpsDailyMaterialItemService;

            MRPItemOrders = mrpItemOrdersService;
        }

        public ERP.Models.Views.MPSDailyGroup Get(int id, int localeId)
        {
            var group = new MPSDailyGroup();

            var mpsDaily = MPSDaily.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (mpsDaily != null)
            {
                var orderNo = mpsDaily.OrderNo;
                var styleNo = mpsDaily.StyleNo;
                var dispatchDailyQtys = GetDispatchQty((int)mpsDaily.Id, (int)mpsDaily.LocaleId);

                // get Dispatch Qty(Size Run)
                var mpsMateraial = MPSDailyMaterial.Get().Where(i => i.MPSDailyId == mpsDaily.Id && i.LocaleId == mpsDaily.LocaleId).ToList();
                var ids = mpsMateraial.Select(i => i.Id).ToArray();
                var mpsMateraialItem = MPSDailyMaterialItem.Get().Where(i => i.LocaleId == mpsDaily.LocaleId && ids.Contains(i.MpsDailyMaterialId)).ToList();

                // 取得最新的MPSStyleItemUsage
                var mpsStyleItemIds = mpsMateraialItem.Select(i => i.MpsStyleItemId);
                var newStyleItemUsage = MPSStyleItemUsage.Get().Where(i => i.LocaleId == localeId && mpsStyleItemIds.Contains(i.MpsStyleItemId));
                var newPlanItemSize = MPSLiveItemSize.Get().Where(i => i.MpsLiveItemId == mpsDaily.MpsLiveItemId && i.LocaleId == localeId);
                var newMateraialItem = (
                    from s in newPlanItemSize
                    join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                    join u in newStyleItemUsage on new { ArticleInnerSize = o.ArticleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = (decimal)u.ArticleInnerSize, LocaleId = u.LocaleId }
                    select new Models.Views.MPSDailyMaterialItem
                    {
                        Id = 0,
                        LocaleId = localeId,
                        MpsDailyMaterialId = 0,
                        SubQty = s.SubQty,
                        UnitUsage = u.UnitUsage,
                        SubUsage = s.SubQty * u.UnitUsage,
                        PreSubUsage = 0,
                        MpsOrdersItemId = o.Id,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        ArticleInnerSize = o.ArticleInnerSize,
                        ArticeSize = o.DisplaySize,
                        MpsStyleItemId = u.MpsStyleItemId,
                    }
                ).ToList();

                mpsMateraialItem.ForEach(i =>
                {
                    var dispatchMaterilItem = dispatchDailyQtys.Where(d => d.MPSDailyMaterialId == i.MpsDailyMaterialId && d.ArticleInnerSize == i.ArticleInnerSize).FirstOrDefault();
                    i.TotalSubQty = dispatchMaterilItem == null ? 0 : dispatchMaterilItem.TotalSubQty;

                    var planSizeItem = newMateraialItem.Where(d => d.ArticleInnerSize == i.ArticleInnerSize).FirstOrDefault();
                    i.PlanSubQty = planSizeItem == null ? 0 : planSizeItem.SubQty;
                });
                newMateraialItem.ForEach(i =>
                {
                    i.MpsDailyMaterialId = mpsMateraial.Where(m => m.MpsStyleItemId == i.MpsStyleItemId).Max(m => m.Id);

                    var dispatchMaterilItem = dispatchDailyQtys.Where(d => d.MPSDailyMaterialId == i.MpsDailyMaterialId && d.ArticleInnerSize == i.ArticleInnerSize).FirstOrDefault();
                    i.TotalSubQty = dispatchMaterilItem == null ? 0 : dispatchMaterilItem.TotalSubQty;
                    i.PlanSubQty = i.SubQty;
                });

                group.MPSDaily = mpsDaily;
                group.MPSDailyMaterial = mpsMateraial;
                group.MPSDailyMaterialItem = mpsMateraialItem;
                group.NewDailyMaterialItem = newMateraialItem;
            }
            return group;
        }

        public ERP.Models.Views.MPSDailyPartGroup GetMPSMaterial(string predicate, string[] filters)
        {
            var group = new MPSDailyPartGroup();
            var withoutDaily = false;
            var material = "";

            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                withoutDaily = (Boolean)extenFilters.Field9;
                material = (string)extenFilters.Field5;
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
                    .OrderBy(i => i.OrderNo)
                    .ToList();

                //如果這個訂單沒有做計畫就沒資料
                if (dailyPlan.Any())
                {
                    var orderNo = dailyPlan[0].OrderNo;
                    var styleNo = dailyPlan[0].StyleNo;
                    var localeId = dailyPlan[0].LocaleId;

                    // 取部位的材料篩選是把正個部位材料取回後再篩選，取得所有部位
                    var dailyPlanItem = (
                        from s in MPSStyle.Get()
                        join si in MPSStyleItem.Get() on new { MPSStyleId = s.Id, LocaleId = s.LocaleId } equals new { MPSStyleId = si.MpsStyleId, LocaleId = si.LocaleId }
                        join m in Material.Get() on new { Material = si.MaterialNameTw, LocaleId = si.LocaleId } equals new { Material = m.MaterialName, LocaleId = m.LocaleId } into mGRP
                        from m in mGRP.DefaultIfEmpty()
                        where s.LocaleId == localeId && s.StyleNo == styleNo
                        select new MPSDailyPlanItem
                        {
                            Id = si.Id,
                            MpsStyleId = si.MpsStyleId,
                            PartNo = Part.Get().Where(i => i.PartNameTw == si.PartNameTw && i.LocaleId == si.LocaleId).Max(i => i.PartNo),
                            PartNameTw = si.PartNameTw,
                            MaterialNameTw = si.MaterialNameTw,
                            UnitNameTw = si.UnitNameTw,
                            AlternateType = si.AlternateType,
                            ModifyUserName = si.ModifyUserName,
                            LastUpdateTime = si.LastUpdateTime,
                            LocaleId = si.LocaleId,
                            MaterialId = m == null ? 0 : m.Id,
                            HasDailyMaterial = MPSDailyMaterial.Get().Where(i => hasMPSDailyIds.Contains(i.MPSDailyId) && i.MpsStyleItemId == si.Id && i.LocaleId == localeId).Count(),
                            PartNameEn = si.PartNameEn,
                            MaterialNameEn = si.MaterialNameEn,
                            UnitNameEn = si.UnitNameEn,
                            RefKnifeNo = si.RefKnifeNo,
                            PieceOfPair = si.PieceOfPair,
                            UnitCodeId = CodeItem.Get().Where(i => i.CodeType == "21" && i.NameTW == si.UnitNameTw).Max(i => i.Id),
                            IsForOrders = 0,
                        }
                    )
                    .ToList();

                    // 如果有材料條件的篩選
                    if (material != null && material.Length > 0)
                    {
                        dailyPlanItem = dailyPlanItem.Where(i => i.MaterialNameTw.Contains(material)).ToList();
                    }

                    // 排除派工單
                    if (withoutDaily)
                    {
                        // dailyPlan = dailyPlan.Where(i => i.HasDaily == 0).ToList();
                        dailyPlanItem = dailyPlanItem.Where(i => i.HasDailyMaterial == 0).OrderBy(i => i.PartNameTw).ToList();
                    }

                    // 排除訂單專用，因為訂單專用有特別處理
                    dailyPlanItem = dailyPlanItem.Where(i => i.PartNameTw != "訂單專用").ToList();

                    group.MPSDailyPlan = dailyPlan.Where(i => i.OrderNo == orderNo).OrderBy(i => i.MPSProcess).ToList();
                    group.MPSDailyPlanItem = dailyPlanItem.OrderBy(i => i.MaterialNameTw).ThenBy(i => i.PartNameTw).ToList();
                }
            }

            return group;
        }
        public ERP.Models.Views.MPSDailyGroup BuildMPSDaily(int mpsDailyPlanId, List<decimal> mpsDailyPlanItemIds, int localeId)
        {
            var group = new MPSDailyGroup();

            var mpsPlanItem = (
                from mli in MPSLiveItem.Get()
                join ml in MPSLive.Get() on new { MPSLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId } equals new { MPSLiveId = ml.Id, LocaleId = ml.LocaleId }
                join mo in MPSOrders.Get() on new { MPOrdersId = ml.MpsOrdersId, LocaleId = ml.LocaleId } equals new { MPOrdersId = mo.Id, LocaleId = mo.LocaleId }
                join mp in MPSProcess.Get() on new { MPSProcessId = ml.ProcessId, LocaleId = ml.LocaleId } equals new { MPSProcessId = mp.Id, LocaleId = mp.LocaleId } into mpGrp
                from mp in mpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = mo.OrderNo } equals new { OrderNo = o.OrderNo } into oGrp
                from o in oGrp.DefaultIfEmpty()
                where mli.Id == mpsDailyPlanId && mli.LocaleId == localeId
                select new
                {
                    Id = mli.Id,
                    OrderNo = mo.OrderNo,
                    Qty = mli.PlanQty,
                    MpsLiveItemId = mli.Id,
                    StyleNo = mo.StyleNo,
                    SizeCountryCodeId = mo.SizeCountryCodeId,

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

            var mpsDailyMaterial = (
                    from si in MPSStyleItem.Get()
                    join s in MPSStyle.Get() on new { MPSStyleId = si.MpsStyleId, LocaleId = si.LocaleId } equals new { MPSStyleId = s.Id, LocaleId = s.LocaleId }
                    join m in Material.Get() on new { Material = si.MaterialNameTw, LocaleId = si.LocaleId } equals new { Material = m.MaterialName, LocaleId = m.LocaleId }
                    join p in Part.Get() on new { Part = si.PartNameTw, LocaleId = si.LocaleId } equals new { Part = p.PartNameTw, LocaleId = p.LocaleId } into pGRP
                    from p in pGRP.DefaultIfEmpty()
                    where s.LocaleId == localeId && mpsDailyPlanItemIds.Contains(si.Id)
                    select new Models.Views.MPSDailyMaterial
                    {
                        Id = 0,
                        LocaleId = m.LocaleId,
                        MPSDailyId = 0,
                        TotalUsage = 0,
                        PreTotalUsage = 0,
                        MpsStyleItemId = si.Id,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        PartNameTw = si.PartNameTw,
                        PartNameEn = si.PartNameEn,
                        PartNo = p.PartNo,
                        PieceOfPair = si.PieceOfPair,
                        AlternateType = si.AlternateType,
                        RefKnifeNo = si.RefKnifeNo,
                        MaterialNameTw = si.MaterialNameTw,
                        MaterialNameEn = si.MaterialNameEn,
                        MaterialId = m.Id,
                        UnitNameTw = si.UnitNameTw,
                        UnitNameEn = si.UnitNameEn,
                        UnitCodeId = CodeItem.Get().Where(i => i.CodeType == "21" && i.NameTW == si.UnitNameTw).Max(i => i.Id),
                    }
                )
                .OrderBy(i => i.PartNameTw)
                .ToList();

            var mpsStyleItemUsage = MPSStyleItemUsage.Get().Where(i => i.LocaleId == localeId && mpsDailyPlanItemIds.Contains(i.MpsStyleItemId));
            var mpsPlanItemSize = MPSLiveItemSize.Get().Where(i => i.MpsLiveItemId == mpsDailyPlanId && i.LocaleId == localeId);

            var mpsMateraialItem = (
                from s in mpsPlanItemSize
                join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                join u in mpsStyleItemUsage on new { ArticleInnerSize = o.ArticleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = (decimal)u.ArticleInnerSize, LocaleId = u.LocaleId }
                select new Models.Views.MPSDailyMaterialItem
                {
                    Id = 0,
                    LocaleId = localeId,
                    MpsDailyMaterialId = 0,
                    SubQty = s.SubQty,
                    UnitUsage = u.UnitUsage,
                    SubUsage = s.SubQty * u.UnitUsage,
                    PreSubUsage = 0,
                    MpsOrdersItemId = o.Id,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    ArticleInnerSize = o.ArticleInnerSize,
                    ArticeSize = o.DisplaySize,
                    MpsStyleItemId = u.MpsStyleItemId,
                    PlanSubQty = s.SubQty,             // 檢查用
                }
            )
            .Where(i => i.SubQty > 0)
            .ToList();

            if (mpsPlanItem != null)
            {
                var mpsStyleItem = mpsDailyMaterial[0]; //只為了取材料名稱，不要做其他用途

                // MPSDaily
                var mpsDaily = new ERP.Models.Views.MPSDaily
                {
                    Id = 0,
                    LocaleId = localeId,
                    DailyNo = "",
                    PreDailyNo = "",
                    OpenDate = DateTime.Today,
                    DailyDate = DateTime.Today,
                    FinishedDate = DateTime.Today,
                    OrderNo = mpsPlanItem.OrderNo,
                    MaterialNameTw = mpsStyleItem.MaterialNameTw,
                    MaterialNameEn = mpsStyleItem.MaterialNameEn,
                    MaterialId = mpsStyleItem.MaterialId,
                    UnitNameTw = mpsStyleItem.UnitNameTw,
                    UnitNameEn = mpsStyleItem.UnitNameTw,
                    UnitCodeId = mpsStyleItem.UnitCodeId,
                    OrgUnitId = mpsPlanItem.MPSProcessOrgId,
                    OrgUnitNameCn = "",
                    OrgUnitNameEn = "",
                    OrgUnitNameTw = "",
                    OrgUnitNameVn = "",
                    Qty = mpsPlanItem.Qty,
                    DailyMode = 1,
                    DailyType = 1,
                    DoDaily = 1,
                    // DoDate = m.DoDate,
                    MpsLiveItemId = mpsPlanItem.Id,
                    // Multi = m.Multi,
                    Remark = "",
                    // ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = DateTime.Now,
                    IsForOrders = 0, // 區分是否是訂單專用的
                    MPSDailyId = 0,
                    TotalUsage = 0,
                    StyleNo = mpsPlanItem.StyleNo,
                    ShoeName = mpsPlanItem.ShoeName,
                    ArticleNo = mpsPlanItem.ArticleNo,
                    CSD = mpsPlanItem.CSD,
                    CompanyId = mpsPlanItem.CompanyId,
                    CompanyNo = mpsPlanItem.CompanyNo,
                    SizeCountryCodeId = mpsPlanItem.SizeCountryCodeId,
                    MPSProcessId = mpsPlanItem.MPSProcessId,
                    MPSProcessNameEn = mpsPlanItem.MPSProcessNameEn,
                    MPSProcessNameTw = mpsPlanItem.MPSProcessNameTw,
                };

                // MPSDailyMaterial
                mpsDailyMaterial.ForEach(i =>
                {
                    i.TotalUsage = mpsMateraialItem.Where(mi => mi.MpsStyleItemId == i.MpsStyleItemId).Sum(mi => mi.SubUsage);
                });
                group.MPSDaily = mpsDaily;
                group.MPSDailyMaterial = mpsDailyMaterial;
                group.MPSDailyMaterialItem = mpsMateraialItem;
                group.NewDailyMaterialItem = mpsMateraialItem;
            }
            return group;
        }

        // 訂單專用
        public ERP.Models.Views.MPSDailyPartGroup GetMPSMaterialForOrdersOnly(string predicate, string[] filters)
        {
            var group = new MPSDailyPartGroup();
            var withoutDaily = false;
            var material = "";

            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                withoutDaily = (Boolean)extenFilters.Field9;
                material = (string)extenFilters.Field5;
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
                    .OrderBy(i => i.OrderNo)
                    .ToList();

                //如果這個訂單沒有做計畫就沒資料
                if (dailyPlan.Any())
                {
                    var orderNo = dailyPlan[0].OrderNo;
                    var styleNo = dailyPlan[0].StyleNo;
                    var localeId = dailyPlan[0].LocaleId;

                    // 取部位的材料篩選是把正個部位材料取回後再篩選，取得所有部位
                    var dailyPlanItem = (
                        from o in Orders.Get()
                        join mo in MRPItemOrders.Get() on new { OrdersId = o.Id, o.LocaleId } equals new { OrdersId = mo.OrdersId, mo.LocaleId }
                        where o.LocaleId == localeId && o.OrderNo == orderNo
                        select new MPSDailyPlanItem
                        {
                            Id = mo.Id,
                            MpsStyleId = 0,
                            PartNo = Part.Get().Where(i => i.PartNameTw == mo.PartNameTw && i.LocaleId == mo.LocaleId).Max(i => i.PartNo),
                            PartNameTw = mo.PartNameTw,
                            MaterialNameTw = mo.MaterialNameTw,
                            UnitNameTw = mo.UnitNameTw,
                            AlternateType = 0,
                            ModifyUserName = mo.ModifyUserName,
                            LastUpdateTime = DateTime.Now,
                            LocaleId = mo.LocaleId,
                            MaterialId = mo.MaterialId,
                            HasDailyMaterial = MPSDaily.Get().Where(i => hasMPSDailyIds.Contains(i.Id) && i.MaterialId == mo.MaterialId && i.LocaleId == localeId).Count(),
                            PartNameEn = mo.PartNameEn,
                            MaterialNameEn = mo.MaterialNameEn,
                            UnitNameEn = mo.UnitNameEn,
                            RefKnifeNo = "",
                            PieceOfPair = 0,
                            UnitCodeId = CodeItem.Get().Where(i => i.CodeType == "21" && i.NameTW == mo.UnitNameTw).Max(i => i.Id),
                            IsForOrders = 1,
                        }
                    )
                    .ToList();

                    // 如果有材料條件的篩選
                    if (material != null && material.Length > 0)
                    {
                        dailyPlanItem = dailyPlanItem.Where(i => i.MaterialNameTw.Contains(material)).ToList();
                    }

                    // 排除派工單
                    if (withoutDaily)
                    {
                        dailyPlanItem = dailyPlanItem.Where(i => i.HasDailyMaterial == 0).OrderBy(i => i.PartNameTw).ToList();
                    }

                    group.MPSDailyPlan = dailyPlan.Where(i => i.OrderNo == orderNo).OrderBy(i => i.MPSProcess).ToList();
                    group.MPSDailyPlanItem = dailyPlanItem.OrderBy(i => i.MaterialNameTw).ThenBy(i => i.PartNameTw).ToList();
                }
            }

            return group;
        }
        public ERP.Models.Views.MPSDailyGroup BuildMPSDailyOrdersOnly(MPSDailyBuildGroup buildGroup)
        {
            // 訂單專用，訂單專用的MPSDailyMaterial只有有一筆，所以都是用只有一筆的概念設計
            var group = new MPSDailyGroup();
            var mpsPlanItemId = buildGroup.MPSDailyPlanId;
            var localeId = buildGroup.LocaleId;
            var mpsDailayPlanItems = buildGroup.MPSDailyPlanItem.ToList();

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

                    CSD = mo.CSD,
                    ShoeName = o.ShoeName,
                    ArticleNo = o.ArticleNo,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    OrdersId = o.Id,
                    RefLocaleId = o.LocaleId,

                    MPSProcessId = ml.ProcessId,
                    MPSProcessNameEn = mp.ProcessNameEn,
                    MPSProcessNameTw = mp.ProcessNameTw,
                    MPSProcessOrgId = MPSProcessOrg.Get().Where(i => i.ProcessId == ml.ProcessId && i.LocaleId == ml.LocaleId).Select(i => i.OrgUnitId).FirstOrDefault(),
                }
            )
            .FirstOrDefault();

            // var mpsDailayPlanItem = mpsDailayPlanItems[0];
            var mrpMaterialIds = mpsDailayPlanItems.Select(i => i.MaterialId).ToList();
            // var mrpItemOrders = MRPItemOrders.Get().Where(i => i.OrdersId == mpsPlanItem.OrdersId && i.LocaleId == mpsPlanItem.RefLocaleId && mrpMaterialIds.Contains(i.MaterialId)).ToList();
            var mrpItemOrder = MRPItemOrders.Get().Where(i => i.OrdersId == mpsPlanItem.OrdersId && i.LocaleId == mpsPlanItem.RefLocaleId && mrpMaterialIds.Contains(i.MaterialId)).FirstOrDefault();

            var mpsDailyMaterial = mpsDailayPlanItems.Select(i => new Models.Views.MPSDailyMaterial
            {
                Id = 0,
                LocaleId = localeId,
                MPSDailyId = 0,
                TotalUsage = 0,
                PreTotalUsage = 0,
                MpsStyleItemId = i.Id,
                ModifyUserName = "",
                LastUpdateTime = DateTime.Now,
                PartNameTw = i.PartNameTw,
                PartNameEn = i.PartNameEn,
                PartNo = i.PartNo,
                PieceOfPair = i.PieceOfPair,
                AlternateType = i.AlternateType,
                RefKnifeNo = i.RefKnifeNo,
                MaterialNameTw = i.MaterialNameTw,
                MaterialNameEn = i.MaterialNameEn,
                UnitNameTw = i.UnitNameTw,
                UnitNameEn = i.UnitNameEn,
                UnitCodeId = i.UnitCodeId ?? 0,
                MaterialId = i.MaterialId ?? 0,
            })
            .ToList();

            var mpsPlanItemSize = MPSLiveItemSize.Get().Where(i => i.MpsLiveItemId == mpsPlanItemId && i.LocaleId == localeId);

            
            var mpsMateraialItem = (
                from s in mpsPlanItemSize
                join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                select new Models.Views.MPSDailyMaterialItem
                {
                    Id = 0,
                    LocaleId = localeId,
                    MpsDailyMaterialId = 0,
                    SubQty = s.SubQty,
                    UnitUsage = mrpItemOrder.UnitTotal,
                    SubUsage = s.SubQty * mrpItemOrder.UnitTotal,
                    PreSubUsage = 0,
                    MpsOrdersItemId = o.Id,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    ArticleInnerSize = o.ArticleInnerSize,
                    ArticeSize = o.DisplaySize,
                    MpsStyleItemId = mrpItemOrder.Id,
                    PlanSubQty = s.SubQty,             // 檢查用
                }
            )
            .Where(i => i.SubQty > 0)
            .ToList();
            if (mpsPlanItem != null)
            {
                var _mpsMaterial = mpsDailyMaterial[0];
                // MPSDaily
                var mpsDaily = new ERP.Models.Views.MPSDaily
                {
                    Id = 0,
                    LocaleId = localeId,
                    DailyNo = "",
                    PreDailyNo = "",
                    OpenDate = DateTime.Today,
                    DailyDate = DateTime.Today,
                    FinishedDate = DateTime.Today,
                    OrderNo = mpsPlanItem.OrderNo,
                    MaterialNameTw = _mpsMaterial.MaterialNameTw,
                    MaterialNameEn = _mpsMaterial.MaterialNameEn,
                    MaterialId = _mpsMaterial.MaterialId,
                    UnitNameTw = _mpsMaterial.UnitNameTw,
                    UnitNameEn = _mpsMaterial.UnitNameTw,
                    UnitCodeId = _mpsMaterial.UnitCodeId,
                    OrgUnitId = mpsPlanItem.MPSProcessOrgId,
                    OrgUnitNameCn = "",
                    OrgUnitNameEn = "",
                    OrgUnitNameTw = "",
                    OrgUnitNameVn = "",
                    Qty = mpsPlanItem.Qty,
                    DailyMode = 1,
                    DailyType = 1,
                    DoDaily = 1,
                    // DoDate = m.DoDate,
                    MpsLiveItemId = mpsPlanItem.Id,
                    // Multi = m.Multi,
                    Remark = "",
                    // ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = DateTime.Now,
                    IsForOrders = 1, // 區分是否是訂單專用的
                    MPSDailyId = 0,
                    TotalUsage = 0,
                    StyleNo = mpsPlanItem.StyleNo,
                    ShoeName = mpsPlanItem.ShoeName,
                    ArticleNo = mpsPlanItem.ArticleNo,
                    CSD = mpsPlanItem.CSD,
                    CompanyId = mpsPlanItem.CompanyId,
                    CompanyNo = mpsPlanItem.CompanyNo,
                    SizeCountryCodeId = mpsPlanItem.SizeCountryCodeId,
                    MPSProcessId = mpsPlanItem.MPSProcessId,
                    MPSProcessNameEn = mpsPlanItem.MPSProcessNameEn,
                    MPSProcessNameTw = mpsPlanItem.MPSProcessNameTw,
                };

                // MPSDailyMaterial
                mpsDailyMaterial.ForEach(i =>
                {
                    i.TotalUsage = mrpItemOrder.Total;
                });

                group.MPSDaily = mpsDaily;
                group.MPSDailyMaterial = mpsDailyMaterial;
                group.MPSDailyMaterialItem = mpsMateraialItem;
                group.NewDailyMaterialItem = mpsMateraialItem;
            }
            return group;
        }

        public ERP.Models.Views.MPSDailyGroup SaveMPSDailyGroup(MPSDailyGroup group)
        {
            var daily = group.MPSDaily;
            var dailyMaterial = group.MPSDailyMaterial.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (daily != null)
                {
                    //Plan
                    {
                        var _daily = MPSDaily.Get().Where(i => i.LocaleId == daily.LocaleId && i.Id == daily.Id).FirstOrDefault();
                        if (_daily == null)
                        {
                            // 取出這個管制表裡這個材料的所有派工單號
                            var candidateNos = MPSDaily.Get()
                                .Where(i => i.LocaleId == daily.LocaleId && i.OrderNo == daily.OrderNo && i.DailyNo != null)
                                .Select(i => i.DailyNo)
                                .ToList();
                            // 判斷最大的單號是哪一個，考量到有舊系統資料，只針對的新系統資料做自動單號
                            int lastSeq = candidateNos
                                .Select(dn => int.TryParse(dn.Split('-').Last(), out int seq) ? seq : -1)
                                .Where(seq => seq >= 0)
                                .DefaultIfEmpty(0)
                                .Max();
                            int nextSeq = lastSeq + 1;
                            // 設動回自動單號
                            daily.DailyNo = $"{daily.OrderNo}-{nextSeq:000}";
                            daily = MPSDaily.Create(daily);
                        }
                        else
                        {
                            daily.Id = _daily.Id;
                            daily.LocaleId = _daily.LocaleId;
                            daily = MPSDaily.Update(daily);
                        }
                    }
                    //items
                    {
                        if (daily.Id != 0)
                        {
                            var materialIds = MPSDailyMaterial.Get().Where(i => i.MPSDailyId == daily.Id && i.LocaleId == daily.LocaleId).Select(i => i.Id).ToArray();
                            MPSDailyMaterial.Remove((int)daily.MPSDailyId, (int)daily.LocaleId);
                            MPSDailyMaterialItem.RemoveRange(i => materialIds.Contains(i.MpsDailyMaterialId) && i.LocaleId == daily.LocaleId);

                            dailyMaterial.ForEach(m =>
                            {
                                m.MPSDailyId = daily.Id;
                                m.ModifyUserName = daily.ModifyUserName;
                                m.LastUpdateTime = daily.LastUpdateTime;
                                m.LocaleId = daily.LocaleId;

                                var _material = MPSDailyMaterial.Create(m);

                                var _materialItem = group.MPSDailyMaterialItem
                                    .Where(i => i.MpsStyleItemId == _material.MpsStyleItemId)
                                    .Select(i => new ERP.Models.Views.MPSDailyMaterialItem
                                    {
                                        Id = i.Id,
                                        LocaleId = i.LocaleId,
                                        MpsDailyMaterialId = _material.Id,
                                        SubQty = i.SubQty,
                                        UnitUsage = i.UnitUsage,
                                        SubUsage = i.SubUsage,
                                        PreSubUsage = i.PreSubUsage,
                                        MpsOrdersItemId = i.MpsOrdersItemId,
                                        ModifyUserName = m.ModifyUserName,
                                        LastUpdateTime = m.LastUpdateTime,

                                        ArticleInnerSize = i.ArticleInnerSize,
                                        ArticeSize = i.ArticeSize,
                                        MpsStyleItemId = i.MpsStyleItemId,
                                        PlanSubQty = i.PlanSubQty,

                                    }).ToList();

                                MPSDailyMaterialItem.CreateRange(_materialItem);
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
            return Get((int)daily.Id, (int)daily.LocaleId);
        }
        public void RemoveMPSDailyGroup(MPSDailyGroup group)
        {
            var daily = group.MPSDaily;
            var dailyMaterialIds = group.MPSDailyMaterial.Select(i => i.Id);
            try
            {
                UnitOfWork.BeginTransaction();
                if (daily != null)
                {
                    MPSDailyMaterialItem.RemoveRange(i => dailyMaterialIds.Contains(i.MpsDailyMaterialId) && i.LocaleId == daily.LocaleId);
                    MPSDailyMaterial.Remove((int)daily.MPSDailyId, (int)daily.LocaleId);
                    MPSDaily.Remove(daily);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public ERP.Models.Views.MPSDailyGroup BuildMPSDailyDiff(string mpsDailyNo, int localeId)
        {
            var group = new MPSDailyGroup();
            var mpsDaily = MPSDaily.Get().Where(i => i.LocaleId == localeId && i.DailyNo == mpsDailyNo).FirstOrDefault();

            if (mpsDaily != null)
            {
                var hasStockIO = StockIO.Get().Where(i => i.LocaleId == localeId && i.PCLIOQty < 0 && i.RefNo == mpsDailyNo).Any();

                var mpsMateraial = MPSDailyMaterial.Get().Where(i => i.MPSDailyId == mpsDaily.Id && i.LocaleId == mpsDaily.LocaleId).ToList();
                var ids = mpsMateraial.Select(i => i.Id).ToArray();
                var mpsStyleItemIds = mpsMateraial.Select(i => i.MpsStyleItemId).ToArray();

                var mpsMateraialItem = MPSDailyMaterialItem.Get().Where(i => i.LocaleId == mpsDaily.LocaleId && ids.Contains(i.MpsDailyMaterialId)).ToList();

                var mpsStyleItemUsage = MPSStyleItemUsage.Get().Where(i => i.LocaleId == localeId && mpsStyleItemIds.Contains(i.MpsStyleItemId));
                var mpsPlanItemSize = MPSLiveItemSize.Get().Where(i => i.MpsLiveItemId == mpsDaily.MpsLiveItemId && i.LocaleId == localeId);
                var newMPSMateraialItem = (
                    from s in mpsPlanItemSize
                    join o in MPSOrdersItem.Get() on new { MpsOrdersItemId = s.MpsOrdersItemId, LocaleId = s.LocaleId } equals new { MpsOrdersItemId = o.Id, LocaleId = o.LocaleId }
                    join u in mpsStyleItemUsage on new { ArticleInnerSize = o.ArticleInnerSize, LocaleId = o.LocaleId } equals new { ArticleInnerSize = (decimal)u.ArticleInnerSize, LocaleId = u.LocaleId }
                    select new Models.Views.MPSDailyMaterialItem
                    {
                        Id = 0,
                        LocaleId = localeId,
                        MpsDailyMaterialId = 0,
                        SubQty = s.SubQty,
                        UnitUsage = u.UnitUsage,
                        SubUsage = s.SubQty * u.UnitUsage,
                        PreSubUsage = 0,
                        MpsOrdersItemId = o.Id,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        ArticleInnerSize = o.ArticleInnerSize,
                        ArticeSize = o.DisplaySize,
                        MpsStyleItemId = u.MpsStyleItemId,
                    }
                ).ToList();

                group.MPSDaily = new ERP.Models.Views.MPSDaily
                {
                    Id = 0,
                    LocaleId = mpsDaily.LocaleId,
                    DailyNo = "",
                    PreDailyNo = mpsDaily.DailyNo,
                    OpenDate = DateTime.Today,
                    DailyDate = DateTime.Today,
                    FinishedDate = DateTime.Today,
                    OrderNo = mpsDaily.OrderNo,
                    MaterialNameTw = mpsDaily.MaterialNameTw,
                    MaterialNameEn = mpsDaily.MaterialNameEn,
                    MaterialId = mpsDaily.MaterialId,
                    UnitNameTw = mpsDaily.UnitNameTw,
                    UnitNameEn = mpsDaily.UnitNameEn,
                    UnitCodeId = mpsDaily.UnitCodeId,
                    OrgUnitId = mpsDaily.OrgUnitId,
                    // OrgUnitNameCn = mpsDaily.UnitNameCn,
                    OrgUnitNameEn = mpsDaily.UnitNameEn,
                    OrgUnitNameTw = mpsDaily.UnitNameTw,
                    // OrgUnitNameVn = mpsDaily.UnitNameVn,
                    Qty = mpsDaily.Qty,
                    DailyMode = 1,
                    DailyType = 2,
                    DoDaily = 1,
                    // DoDate = m.DoDate,
                    MpsLiveItemId = mpsDaily.MpsLiveItemId,
                    Multi = 0,
                    Remark = "",
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    // IsForOrders = m.IsForOrders,
                    MPSDailyId = 0,
                    TotalUsage = 0,
                    StyleNo = mpsDaily.StyleNo,
                    ShoeName = mpsDaily.ShoeName,
                    ArticleNo = mpsDaily.ArticleNo,
                    CSD = mpsDaily.CSD,
                    CompanyId = mpsDaily.CompanyId,
                    CompanyNo = mpsDaily.CompanyNo,
                    SizeCountryCodeId = mpsDaily.SizeCountryCodeId,
                    MPSProcessId = mpsDaily.MPSProcessId,
                    MPSProcessNameEn = mpsDaily.MPSProcessNameEn,
                    MPSProcessNameTw = mpsDaily.MPSProcessNameTw,

                    HasStockOut = hasStockIO,
                };
                group.MPSDailyMaterial = mpsMateraial.Select(i => new ERP.Models.Views.MPSDailyMaterial
                {
                    Id = 0,
                    LocaleId = i.LocaleId,
                    MPSDailyId = 0,
                    TotalUsage = mpsMateraialItem.Where(mi => mi.MpsStyleItemId == i.MpsStyleItemId).Sum(mi => mi.SubUsage),
                    PreTotalUsage = mpsMateraial.Where(m => m.MpsStyleItemId == i.MpsStyleItemId).Max(m => m.TotalUsage),
                    MpsStyleItemId = i.MpsStyleItemId,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    PartNameTw = i.PartNameTw,
                    PartNameEn = i.PartNameEn,
                    PartNo = i.PartNo,
                    PieceOfPair = i.PieceOfPair,
                    AlternateType = i.AlternateType,
                    RefKnifeNo = i.RefKnifeNo,
                    MaterialNameTw = i.MaterialNameTw,
                    MaterialNameEn = i.MaterialNameEn,
                    MaterialId = i.Id,
                    UnitNameTw = i.UnitNameTw,
                    UnitCodeId = i.UnitCodeId,
                }).ToList();
                group.MPSDailyMaterialItem = newMPSMateraialItem.Select(i => new ERP.Models.Views.MPSDailyMaterialItem
                {
                    Id = 0,
                    LocaleId = localeId,
                    MpsDailyMaterialId = 0,
                    SubQty = i.SubQty,
                    UnitUsage = i.UnitUsage,
                    SubUsage = i.SubUsage,
                    PreSubUsage = mpsMateraialItem.Where(m => m.MpsStyleItemId == i.MpsStyleItemId && m.MpsOrdersItemId == i.MpsOrdersItemId).Max(m => m.SubUsage),
                    MpsOrdersItemId = i.MpsOrdersItemId,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    ArticleInnerSize = i.ArticleInnerSize,
                    ArticeSize = i.ArticeSize,
                    MpsStyleItemId = i.MpsStyleItemId,
                }).ToList();
            }
            return group;
        }
        public ERP.Models.Views.MPSDailyGroup SaveMPSDailyDiffGroup(MPSDailyGroup group)
        {
            var daily = group.MPSDaily;
            var dailyMaterial = group.MPSDailyMaterial.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (daily != null)
                {
                    //Plan
                    {
                        var _daily = MPSDaily.Get().Where(i => i.LocaleId == daily.LocaleId && i.Id == daily.Id).FirstOrDefault();
                        if (_daily == null)
                        {
                            // 取出這個管制表裡這個材料的所有派工單號
                            var candidateNos = MPSDaily.Get()
                                .Where(i => i.LocaleId == daily.LocaleId && i.OrderNo == daily.OrderNo && i.DailyNo != null)
                                .Select(i => i.DailyNo)
                                .ToList();
                            // 判斷最大的單號是哪一個，考量到有舊系統資料，只針對的新系統資料做自動單號
                            int lastSeq = candidateNos
                                .Select(dn =>
                                {
                                    var parts = dn.Split("-R");
                                    return (parts.Length >= 2 && int.TryParse(parts[^1], out int seq)) ? seq : -1;
                                })
                                .Where(seq => seq >= 0)
                                .DefaultIfEmpty(0)
                                .Max();

                            int nextSeq = lastSeq + 1;
                            // 設動回自動單號
                            daily.DailyNo = $"{daily.PreDailyNo}-R{nextSeq:D3}";
                            daily = MPSDaily.Create(daily);
                        }
                        else
                        {
                            daily.Id = _daily.Id;
                            daily.LocaleId = _daily.LocaleId;
                            daily = MPSDaily.Update(daily);
                        }
                    }
                    //items
                    {
                        if (daily.Id != 0)
                        {
                            var materialIds = MPSDailyMaterial.Get().Where(i => i.MPSDailyId == daily.Id && i.LocaleId == daily.LocaleId).Select(i => i.Id).ToArray();
                            // MPSDailyMaterial.RemoveRange(i => i.MpsDailyId == daily.Id && i.LocaleId == daily.LocaleId);
                            MPSDailyMaterial.Remove((int)daily.MPSDailyId, (int)daily.LocaleId);
                            MPSDailyMaterialItem.RemoveRange(i => materialIds.Contains(i.MpsDailyMaterialId) && i.LocaleId == daily.LocaleId);

                            dailyMaterial.ForEach(m =>
                            {
                                m.MPSDailyId = daily.Id;
                                m.ModifyUserName = daily.ModifyUserName;
                                m.LastUpdateTime = daily.LastUpdateTime;
                                m.LocaleId = daily.LocaleId;

                                var _material = MPSDailyMaterial.Create(m);

                                var _materialItem = group.MPSDailyMaterialItem
                                    .Where(i => i.MpsStyleItemId == _material.MpsStyleItemId)
                                    .Select(i => new ERP.Models.Views.MPSDailyMaterialItem
                                    {
                                        Id = i.Id,
                                        LocaleId = i.LocaleId,
                                        MpsDailyMaterialId = _material.Id,
                                        SubQty = i.SubQty,
                                        UnitUsage = i.UnitUsage,
                                        SubUsage = i.SubUsage,
                                        PreSubUsage = i.PreSubUsage,
                                        MpsOrdersItemId = i.MpsOrdersItemId,
                                        ModifyUserName = m.ModifyUserName,
                                        LastUpdateTime = m.LastUpdateTime,

                                        ArticleInnerSize = i.ArticleInnerSize,
                                        ArticeSize = i.ArticeSize,
                                        MpsStyleItemId = i.MpsStyleItemId,

                                    }).ToList();

                                MPSDailyMaterialItem.CreateRange(_materialItem);
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
            return Get((int)daily.Id, (int)daily.LocaleId);
        }
        public void RemoveMPSDailyDiffGroup(MPSDailyGroup group)
        {
            var daily = group.MPSDaily;
            var dailyMaterialIds = group.MPSDailyMaterial.Select(i => i.Id);
            try
            {
                UnitOfWork.BeginTransaction();
                if (daily != null)
                {
                    MPSDailyMaterialItem.RemoveRange(i => dailyMaterialIds.Contains(i.MpsDailyMaterialId) && i.LocaleId == daily.LocaleId);
                    // MPSDailyMaterial.RemoveRange(i => i.MpsDailyId == daily.Id && i.LocaleId == daily.LocaleId);
                    MPSDailyMaterial.Remove((int)daily.MPSDailyId, (int)daily.LocaleId);
                    MPSDaily.Remove(daily);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        private List<Models.Views.MPSDailyMaterialSizeItem> GetDispatchQty(int mpsDailyId, int mpsLocaleId)
        {
            var result = (
                from ml in MPSLive.Get()
                join mli in MPSLiveItem.Get() on new { MpsLiveId = ml.Id, LocaleId = ml.LocaleId } equals new { MpsLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId }
                join mlis in MPSLiveItemSize.Get() on new { MpsLiveItemId = mli.Id, LocaleId = mli.LocaleId } equals new { MpsLiveItemId = mlis.MpsLiveItemId, LocaleId = mlis.LocaleId }
                join moi in MPSOrdersItem.Get() on new { MpsOrdersItemId = mlis.MpsOrdersItemId, LocaleId = mlis.LocaleId } equals new { MpsOrdersItemId = moi.Id, LocaleId = moi.LocaleId }
                join md in MpsDaily.Get() on new { MpsLiveItemId = mli.Id, LocaleId = mli.LocaleId } equals new { MpsLiveItemId = md.MpsLiveItemId, LocaleId = md.LocaleId }
                join mdm in MpsDailyMaterial.Get() on new { MPSDailyId = md.Id, LocaleId = md.LocaleId } equals new { MPSDailyId = mdm.MpsDailyId, LocaleId = mdm.LocaleId }
                join mdmi in MpsDailyMaterialItem.Get() on new { MpsDailyMaterialId = mdm.Id, MpsOrdersItemId = moi.Id, LocaleId = mdm.LocaleId } equals new { MpsDailyMaterialId = mdmi.MpsDailyMaterialId, MpsOrdersItemId = mdmi.MpsOrdersItemId, LocaleId = mdmi.LocaleId }
                where md.Id == mpsDailyId && md.LocaleId == mpsLocaleId
                select new
                {
                    MPSOrdersId = ml.MpsOrdersId,
                    MPSLiveItemId = mli.Id,
                    MPSLiveItemSizeId = mlis.Id,
                    MPSLiveItemQty = mli.PlanQty,
                    MPSLiveItemSizeQty = mlis.SubQty,
                    ArticleInnerSize = moi.ArticleInnerSize,
                    DisplaySize = moi.DisplaySize,
                    MPSOrderSizeQty = moi.Qty,
                    MPSOrderQty = moi.OrderQty,
                    MPSDailyId = md.Id,
                    MPSDailyMaterialId = mdm.Id,
                    MPSDailyNo = md.DailyNo,
                    MPSOrderNo = md.OrderNo,
                    MaterialId = md.MaterialId,
                    MaterialName = md.MaterialNameTw,
                    MPSOrdersItemId = moi.Id,
                    MPSDailyMaterialSizeQty = mdmi.SubQty,
                    MPSStyleItemId = mdm.MpsStyleItemId,

                }
            )
            .GroupBy(g => new { g.MPSOrdersId, g.MPSLiveItemId, g.MPSLiveItemSizeId, g.MPSLiveItemQty, g.MPSLiveItemSizeQty, g.ArticleInnerSize, g.DisplaySize, g.MPSOrderSizeQty, g.MPSOrderQty, g.MPSDailyId, g.MPSDailyMaterialId, g.MPSDailyNo, g.MPSOrderNo, g.MaterialId, g.MaterialName, g.MPSOrdersItemId, g.MPSStyleItemId })
            .Select(i => new Models.Views.MPSDailyMaterialSizeItem
            {
                MPSOrdersId = i.Key.MPSOrdersId,
                MPSLiveItemId = i.Key.MPSLiveItemId,
                MPSLiveItemSizeId = i.Key.MPSLiveItemSizeId,
                MPSLiveItemQty = i.Key.MPSLiveItemQty,
                MPSLiveItemSizeQty = i.Key.MPSLiveItemSizeQty,
                ArticleInnerSize = i.Key.ArticleInnerSize,
                DisplaySize = i.Key.DisplaySize,
                MPSOrderSizeQty = i.Key.MPSOrderSizeQty,
                MPSOrderQty = i.Key.MPSOrderQty,
                MPSDailyId = i.Key.MPSDailyId,
                MPSDailyMaterialId = i.Key.MPSDailyMaterialId,
                MPSDailyNo = i.Key.MPSDailyNo,
                MPSOrderNo = i.Key.MPSOrderNo,
                MaterialId = i.Key.MaterialId,
                MaterialName = i.Key.MaterialName,
                MPSOrdersItemId = i.Key.MPSOrdersItemId,
                TotalSubQty = i.Sum(g => g.MPSDailyMaterialSizeQty),
                MPSStyleItemId = i.Key.MPSStyleItemId,
            })
            .ToList();

            return result;
        }
        private List<Models.Views.MPSDailyMaterialSizeItem> GetDispatchQty(int mpsPlanItemId, List<decimal> mpsStyleItemIds, int localeId)
        {
            var result = (
                from md in MpsDaily.Get()
                join mdm in MpsDailyMaterial.Get() on new { MPSDailyId = md.Id, LocaleId = md.LocaleId } equals new { MPSDailyId = mdm.MpsDailyId, LocaleId = mdm.LocaleId }
                join mdmi in MpsDailyMaterialItem.Get() on new { MpsDailyMaterialId = mdm.Id, LocaleId = mdm.LocaleId } equals new { MpsDailyMaterialId = mdmi.MpsDailyMaterialId, LocaleId = mdmi.LocaleId }
                join mlis in MPSLiveItemSize.Get() on new { MpsLiveItemId = md.MpsLiveItemId, MpsOrdersItemId = mdmi.MpsOrdersItemId, LocaleId = md.LocaleId } equals new { MpsLiveItemId = mlis.MpsLiveItemId, MpsOrdersItemId = mlis.MpsOrdersItemId, LocaleId = mlis.LocaleId }
                join mli in MPSLiveItem.Get() on new { MpsLiveItemId = mlis.MpsLiveItemId, LocaleId = mlis.LocaleId } equals new { MpsLiveItemId = mli.Id, LocaleId = mli.LocaleId }
                join moi in MPSOrdersItem.Get() on new { MpsOrdersItemId = mlis.MpsOrdersItemId, LocaleId = mlis.LocaleId } equals new { MpsOrdersItemId = moi.Id, LocaleId = moi.LocaleId }
                where md.MpsLiveItemId == mpsPlanItemId && md.LocaleId == localeId && mpsStyleItemIds.Contains(mdm.MpsStyleItemId)
                select new
                {
                    MPSOrdersId = moi.MpsOrdersId,
                    MPSLiveItemId = md.MpsLiveItemId,
                    MPSLiveItemSizeId = mlis.Id,
                    MPSLiveItemQty = mli.PlanQty,
                    MPSLiveItemSizeQty = mlis.SubQty,
                    ArticleInnerSize = moi.ArticleInnerSize,
                    DisplaySize = moi.DisplaySize,
                    MPSOrderSizeQty = moi.Qty,
                    MPSOrderQty = moi.OrderQty,
                    MPSDailyId = md.Id,
                    MPSDailyMaterialId = mdm.Id,
                    MPSDailyNo = md.DailyNo,
                    MPSOrderNo = md.OrderNo,
                    MaterialId = md.MaterialId,
                    MaterialName = md.MaterialNameTw,
                    MPSOrdersItemId = moi.Id,
                    MPSDailyMaterialSizeQty = mdmi.SubQty,
                    MPSStyleItemId = mdm.MpsStyleItemId,
                }
            )
            .GroupBy(g => new { g.MPSOrdersId, g.MPSLiveItemId, g.MPSLiveItemSizeId, g.MPSLiveItemQty, g.MPSLiveItemSizeQty, g.ArticleInnerSize, g.DisplaySize, g.MPSOrderSizeQty, g.MPSOrderQty, g.MPSDailyId, g.MPSDailyMaterialId, g.MPSDailyNo, g.MPSOrderNo, g.MaterialId, g.MaterialName, g.MPSOrdersItemId, g.MPSStyleItemId })
            .Select(i => new Models.Views.MPSDailyMaterialSizeItem
            {
                MPSOrdersId = i.Key.MPSOrdersId,
                MPSLiveItemId = i.Key.MPSLiveItemId,
                MPSLiveItemSizeId = i.Key.MPSLiveItemSizeId,
                MPSLiveItemQty = i.Key.MPSLiveItemQty,
                MPSLiveItemSizeQty = i.Key.MPSLiveItemSizeQty,
                ArticleInnerSize = i.Key.ArticleInnerSize,
                DisplaySize = i.Key.DisplaySize,
                MPSOrderSizeQty = i.Key.MPSOrderSizeQty,
                MPSOrderQty = i.Key.MPSOrderQty,
                MPSDailyId = i.Key.MPSDailyId,
                MPSDailyMaterialId = i.Key.MPSDailyMaterialId,
                MPSDailyNo = i.Key.MPSDailyNo,
                MPSOrderNo = i.Key.MPSOrderNo,
                MaterialId = i.Key.MaterialId,
                MaterialName = i.Key.MaterialName,
                MPSOrdersItemId = i.Key.MPSOrdersItemId,
                TotalSubQty = i.Sum(g => g.MPSDailyMaterialSizeQty),
                MPSStyleItemId = i.Key.MPSStyleItemId,
            })
            .ToList();

            return result;
        }
    }
}
