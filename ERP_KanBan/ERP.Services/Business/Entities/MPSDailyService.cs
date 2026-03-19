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

    public class MPSDailyService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyService MPSDaily { get; set; }
        private ERP.Services.Entities.MpsProcessUnitService MPSProcessUnit { get; set; }
        private ERP.Services.Entities.MpsDailyMaterialService MPSDailyMaterial { get; set; }
        private ERP.Services.Entities.MpsLiveService MPSLive { get; set; }
        private ERP.Services.Entities.MpsLiveItemService MPSLiveItem { get; set; }
        private ERP.Services.Entities.MpsProcessService MPSProcess { get; set; }

        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.StockIOService StockIO { get; set; }
        private ERP.Services.Entities.MpsDailyPrintLogService MPSDailyPrintLog { get; set; }

        public MPSDailyService(
            ERP.Services.Entities.MpsDailyService mpsDailyService,
            ERP.Services.Entities.MpsProcessUnitService mpsProcessUnitService,
            ERP.Services.Entities.MpsDailyMaterialService mpsDailyMaterialService,
            ERP.Services.Entities.MpsLiveService mpsLiveService,
            ERP.Services.Entities.MpsLiveItemService mpsLiveItemService,
            ERP.Services.Entities.MpsProcessService mpsProcessService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.MpsDailyPrintLogService mpsDailyPrintLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDaily = mpsDailyService;
            MPSProcessUnit = mpsProcessUnitService;
            MPSDailyMaterial = mpsDailyMaterialService;

            MPSLive = mpsLiveService;
            MPSLiveItem = mpsLiveItemService;
            MPSProcess = mpsProcessService;
            Orders = ordersService;
            StockIO = stockIOService;
            MPSDailyPrintLog = mpsDailyPrintLogService;
        }

        public IQueryable<Models.Views.MPSDaily> Get()
        {
            var items = (
                from m in MPSDaily.Get()
                join u in MPSProcessUnit.Get() on new { OrgUnitId = m.OrgUnitId, LocaleId = m.LocaleId } equals new { OrgUnitId = u.Id, LocaleId = u.LocaleId } into uGrp
                from u in uGrp.DefaultIfEmpty()
                join mli in MPSLiveItem.Get() on new { MPSLiveItemId = m.MpsLiveItemId, LocaleId = m.LocaleId } equals new { MPSLiveItemId = mli.Id, LocaleId = mli.LocaleId } into mliGrp
                from mli in mliGrp.DefaultIfEmpty()
                join ml in MPSLive.Get() on new { MPSLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId } equals new { MPSLiveId = ml.Id, LocaleId = ml.LocaleId } into mlGrp
                from ml in mlGrp.DefaultIfEmpty()
                join mp in MPSProcess.Get() on new { MPSProcessId = ml.ProcessId, LocaleId = ml.LocaleId } equals new { MPSProcessId = mp.Id, LocaleId = mp.LocaleId } into mpGrp
                from mp in mpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = o.OrderNo } into oGrp
                from o in oGrp.DefaultIfEmpty()
                select new Models.Views.MPSDaily
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    DailyNo = m.DailyNo,
                    PreDailyNo = m.PreDailyNo,
                    OpenDate = m.OpenDate,
                    DailyDate = m.DailyDate,
                    FinishedDate = m.FinishedDate,
                    OrderNo = m.OrderNo,
                    MaterialNameTw = m.MaterialNameTw,
                    MaterialNameEn = m.MaterialNameEn,
                    MaterialId = m.MaterialId,
                    UnitNameTw = m.UnitNameTw,
                    UnitNameEn = m.UnitNameEn,
                    UnitCodeId = m.UnitCodeId,
                    OrgUnitId = m.OrgUnitId,
                    OrgUnitNameCn = u.UnitNameCn,
                    OrgUnitNameEn = u.UnitNameEn,
                    OrgUnitNameTw = u.UnitNameTw,
                    OrgUnitNameVn = u.UnitNameVn,
                    Qty = m.Qty,
                    DailyMode = m.DailyMode,
                    DailyType = m.DailyType,
                    DoDaily = m.DoDaily,
                    DoDate = m.DoDate,
                    MpsLiveItemId = m.MpsLiveItemId,
                    Multi = m.Multi,
                    Remark = m.Remark,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                    IsForOrders = m.IsForOrders,
                    MPSDailyId = m.Id,
                    TotalUsage = MPSDailyMaterial.Get().Where(i => i.MpsDailyId == m.Id && i.LocaleId == m.LocaleId).Sum(i => i.TotalUsage),
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    ArticleNo = o.ArticleNo,
                    CSD = o.CSD,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    SizeCountryCodeId = o.SizeCountryCodeId,
                    MPSProcessId = ml.ProcessId,
                    MPSProcessNameEn = mp.ProcessNameEn,
                    MPSProcessNameTw = mp.ProcessNameTw,
                });
            return items;
        }
        public IQueryable<Models.Views.MPSDaily> GetHead()
        {
            var items = (
                from m in MPSDaily.Get()
                join u in MPSProcessUnit.Get() on new { OrgUnitId = m.OrgUnitId, LocaleId = m.LocaleId } equals new { OrgUnitId = u.Id, LocaleId = u.LocaleId } into uGrp
                from u in uGrp.DefaultIfEmpty()
                join mli in MPSLiveItem.Get() on new { MPSLiveItemId = m.MpsLiveItemId, LocaleId = m.LocaleId } equals new { MPSLiveItemId = mli.Id, LocaleId = mli.LocaleId } into mliGrp
                from mli in mliGrp.DefaultIfEmpty()
                join ml in MPSLive.Get() on new { MPSLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId } equals new { MPSLiveId = ml.Id, LocaleId = ml.LocaleId } into mlGrp
                from ml in mlGrp.DefaultIfEmpty()
                join mp in MPSProcess.Get() on new { MPSProcessId = ml.ProcessId, LocaleId = ml.LocaleId } equals new { MPSProcessId = mp.Id, LocaleId = mp.LocaleId } into mpGrp
                from mp in mpGrp.DefaultIfEmpty()
                select new Models.Views.MPSDaily
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    DailyNo = m.DailyNo,
                    OrderNo = m.OrderNo,
                    MaterialNameTw = m.MaterialNameTw,
                    MaterialId = m.MaterialId,
                    OrgUnitId = m.OrgUnitId,
                    OrgUnitNameCn = u.UnitNameCn,
                    OrgUnitNameEn = u.UnitNameEn,
                    OrgUnitNameTw = u.UnitNameTw,
                    OrgUnitNameVn = u.UnitNameVn,
                    // MPSDailyId = m.Id,
                    // TotalUsage = MPSDailyMaterial.Get().Where(i => i.MpsDailyId == m.Id && i.LocaleId == m.LocaleId).Sum(i => i.TotalUsage),
                    MPSProcessId = ml.ProcessId,
                    MPSProcessNameEn = mp.ProcessNameEn,
                    MPSProcessNameTw = mp.ProcessNameTw,
                }
            )
            .GroupBy(i => new { i.Id, i.LocaleId, i.DailyNo, i.OrderNo, i.MaterialNameTw, i.MaterialId, i.OrgUnitId, i.OrgUnitNameCn, i.OrgUnitNameEn, i.OrgUnitNameTw, i.OrgUnitNameVn, i.MPSProcessId, i.MPSProcessNameTw, i.MPSProcessNameEn })
            .Select(g => new Models.Views.MPSDaily
            {
                Id = g.Key.Id,
                LocaleId = g.Key.LocaleId,
                DailyNo = g.Key.DailyNo,
                OrderNo = g.Key.OrderNo,
                MaterialNameTw = g.Key.MaterialNameTw,
                MaterialId = g.Key.MaterialId,
                OrgUnitId = g.Key.OrgUnitId,
                OrgUnitNameCn = g.Key.OrgUnitNameCn,
                OrgUnitNameEn = g.Key.OrgUnitNameEn,
                OrgUnitNameTw = g.Key.OrgUnitNameTw,
                OrgUnitNameVn = g.Key.OrgUnitNameVn,
                // MPSDailyId = m.Id,
                // TotalUsage = MPSDailyMaterial.Get().Where(i => i.MpsDailyId == m.Id && i.LocaleId == m.LocaleId).Sum(i => i.TotalUsage),
                // TotalUsage = g.Sum(i => i.TotalUsage),
                MPSProcessId = g.Key.MPSProcessId,
                MPSProcessNameEn = g.Key.MPSProcessNameEn,
                MPSProcessNameTw = g.Key.MPSProcessNameTw,
            });
            return items;
        }

        public IQueryable<Models.Views.MPSDailyForPrint> GetPrint()
        {
            var items = (
                from m in MPSDaily.Get()
                join u in MPSProcessUnit.Get() on new { OrgUnitId = m.OrgUnitId, LocaleId = m.LocaleId } equals new { OrgUnitId = u.Id, LocaleId = u.LocaleId } into uGrp
                from u in uGrp.DefaultIfEmpty()
                join mli in MPSLiveItem.Get() on new { MPSLiveItemId = m.MpsLiveItemId, LocaleId = m.LocaleId } equals new { MPSLiveItemId = mli.Id, LocaleId = mli.LocaleId } into mliGrp
                from mli in mliGrp.DefaultIfEmpty()
                join ml in MPSLive.Get() on new { MPSLiveId = mli.MpsLiveId, LocaleId = mli.LocaleId } equals new { MPSLiveId = ml.Id, LocaleId = ml.LocaleId } into mlGrp
                from ml in mlGrp.DefaultIfEmpty()
                join mp in MPSProcess.Get() on new { MPSProcessId = ml.ProcessId, LocaleId = ml.LocaleId } equals new { MPSProcessId = mp.Id, LocaleId = mp.LocaleId } into mpGrp
                from mp in mpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = o.OrderNo } into oGrp
                from o in oGrp.DefaultIfEmpty()
                select new Models.Views.MPSDailyForPrint
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    DailyNo = m.DailyNo,
                    PreDailyNo = m.PreDailyNo,
                    OpenDate = m.OpenDate,
                    DailyDate = m.DailyDate,
                    FinishedDate = m.FinishedDate,
                    OrderNo = m.OrderNo,
                    MaterialNameTw = m.MaterialNameTw,
                    MaterialNameEn = m.MaterialNameEn,
                    MaterialId = m.MaterialId,
                    UnitNameTw = m.UnitNameTw,
                    UnitNameEn = m.UnitNameEn,
                    UnitCodeId = m.UnitCodeId,
                    OrgUnitId = m.OrgUnitId,
                    OrgUnitNameCn = u.UnitNameCn,
                    OrgUnitNameEn = u.UnitNameEn,
                    OrgUnitNameTw = u.UnitNameTw,
                    OrgUnitNameVn = u.UnitNameVn,
                    Qty = m.Qty,
                    DailyMode = m.DailyMode,
                    DailyType = m.DailyType,
                    DoDaily = m.DoDaily,
                    DoDate = m.DoDate,
                    MpsLiveItemId = m.MpsLiveItemId,
                    Multi = m.Multi,
                    Remark = m.Remark,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                    IsForOrders = m.IsForOrders,
                    MPSDailyId = m.Id,
                    TotalUsage = MPSDailyMaterial.Get().Where(i => i.MpsDailyId == m.Id && i.LocaleId == m.LocaleId).Sum(i => i.TotalUsage),
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    ArticleNo = o.ArticleNo,
                    CSD = o.CSD,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    SizeCountryCodeId = o.SizeCountryCodeId,
                    MPSProcessId = ml.ProcessId,
                    MPSProcessNameEn = mp.ProcessNameEn,
                    MPSProcessNameTw = mp.ProcessNameTw,
                    PrintCount = MPSDailyPrintLog.Get().Where(i => i.LocaleId == m.LocaleId && i.RefDailyNo == m.DailyNo).Count()
                });
            return items;
        }
        public Models.Views.MPSDaily Create(Models.Views.MPSDaily item)
        {
            var _item = MPSDaily.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSDaily Update(Models.Views.MPSDaily item)
        {
            var _item = MPSDaily.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSDaily item)
        {
            MPSDaily.Remove(Build(item));
        }
        private Models.Entities.MpsDaily Build(Models.Views.MPSDaily item)
        {
            return new Models.Entities.MpsDaily()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                DailyNo = item.DailyNo,
                PreDailyNo = item.PreDailyNo,
                OpenDate = item.OpenDate,
                DailyDate = item.DailyDate,
                FinishedDate = item.FinishedDate,
                OrderNo = item.OrderNo,
                MaterialNameTw = item.MaterialNameTw,
                MaterialNameEn = item.MaterialNameEn,
                MaterialId = item.MaterialId,
                UnitNameTw = item.UnitNameTw,
                UnitNameEn = item.UnitNameEn,
                UnitCodeId = item.UnitCodeId,
                OrgUnitId = item.OrgUnitId,
                Qty = item.Qty,
                DailyMode = item.DailyMode,
                DailyType = item.DailyType,
                DoDaily = item.DoDaily,
                DoDate = item.DoDate,
                MpsLiveItemId = item.MpsLiveItemId,
                Multi = item.Multi,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                IsForOrders = item.IsForOrders,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.MPSDaily> items)
        {
            MPSDaily.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsDaily, bool>> predicate)
        {
            MPSDaily.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsDaily> BuildRange(IEnumerable<Models.Views.MPSDaily> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDaily
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                DailyNo = item.DailyNo,
                PreDailyNo = item.PreDailyNo,
                OpenDate = item.OpenDate,
                DailyDate = item.DailyDate,
                FinishedDate = item.FinishedDate,
                OrderNo = item.OrderNo,
                MaterialNameTw = item.MaterialNameTw,
                MaterialNameEn = item.MaterialNameEn,
                MaterialId = item.MaterialId,
                UnitNameTw = item.UnitNameTw,
                UnitNameEn = item.UnitNameEn,
                UnitCodeId = item.UnitCodeId,
                OrgUnitId = item.OrgUnitId,
                Qty = item.Qty,
                DailyMode = item.DailyMode,
                DailyType = item.DailyType,
                DoDaily = item.DoDaily,
                DoDate = item.DoDate,
                MpsLiveItemId = item.MpsLiveItemId,
                Multi = item.Multi,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                IsForOrders = item.IsForOrders,
            });
        }

    }
}
