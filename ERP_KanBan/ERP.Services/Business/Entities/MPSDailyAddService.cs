using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSDailyAddService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyAddService MPSDailyAdd { get; set; }
        private ERP.Services.Entities.MpsProcessUnitService MPSProcessUnit { get; set; }
        private ERP.Services.Entities.MpsDailyMaterialAddService MPSDailyMaterialAdd { get; set; }
        private ERP.Services.Entities.MpsLiveService MPSLive { get; set; }
        private ERP.Services.Entities.MpsLiveItemService MPSLiveItem { get; set; }
        private ERP.Services.Entities.MpsProcessService MPSProcess { get; set; }

        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.WarehouseService Warehouse { get; set; }
        private ERP.Services.Business.Entities.MPSDailyAddPrintLogService MPSDailyAddPrintLog;


        public MPSDailyAddService(
            ERP.Services.Entities.MpsDailyAddService mpsDailyAddService,
            ERP.Services.Entities.MpsProcessUnitService mpsProcessUnitService,
            ERP.Services.Entities.MpsDailyMaterialAddService mpsDailyMaterialAddService,
            ERP.Services.Entities.MpsLiveService mpsLiveService,
            ERP.Services.Entities.MpsLiveItemService mpsLiveItemService,
            ERP.Services.Entities.MpsProcessService mpsProcessService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.WarehouseService warehouseService,
            ERP.Services.Business.Entities.MPSDailyAddPrintLogService mpsDailyAddPrintLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyAdd = mpsDailyAddService;
            MPSProcessUnit = mpsProcessUnitService;
            MPSDailyMaterialAdd = mpsDailyMaterialAddService;

            MPSLive = mpsLiveService;
            MPSLiveItem = mpsLiveItemService;
            MPSProcess = mpsProcessService;
            Orders = ordersService;
            Material = materialService;
            Warehouse = warehouseService;
            MPSDailyAddPrintLog = mpsDailyAddPrintLogService;
        }

        public IQueryable<Models.Views.MPSDailyAdd> Get()
        {
            var items = (
                from m in MPSDailyAdd.Get()
                join u in MPSProcessUnit.Get() on new { OrgUnitId = m.ProcessUnitId, LocaleId = m.LocaleId } equals new { OrgUnitId = u.Id, LocaleId = u.LocaleId } into uGrp
                from u in uGrp.DefaultIfEmpty()
                join mp in MPSProcess.Get() on new { MPSProcessId = m.ProcessId, LocaleId = m.LocaleId } equals new { MPSProcessId = mp.Id, LocaleId = mp.LocaleId } into mpGrp
                from mp in mpGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = o.OrderNo } into oGrp
                from o in oGrp.DefaultIfEmpty()
                select new Models.Views.MPSDailyAdd
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    DailyNo = m.DailyNo,
                    PreDailyNo = m.PreDailyNo,
                    OpenDate = m.OpenDate,
                    DailyDate = m.DailyDate,
                    FinishedDate = m.FinishedDate,
                    ProcessId = m.ProcessId,
                    ProcessUnitId = m.ProcessUnitId,
                    OrderNo = m.OrderNo,
                    MpsStyleId = m.MpsStyleId,
                    OrderQty = m.OrderQty,
                    Qty = m.Qty,
                    SizeCountryNameTw = m.SizeCountryNameTw,
                    DailyMode = m.DailyMode,
                    DailyType = m.DailyType,
                    DoDaily = m.DoDaily,
                    DoDate = m.DoDate,
                    Multi = m.Multi,
                    Remark = m.Remark,
                    SeqId = m.SeqId,
                    MaterialCost = m.MaterialCost,
                    DollarNameTw = m.DollarNameTw,
                    CostBalance = m.CostBalance,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,

                    ProcessUnitTw = u.UnitNameTw ?? "",
                    StyleNo = o.StyleNo ?? "",
                    ShoeName = o.ShoeName ?? "",
                    ArticleNo = o.ArticleNo ?? "",
                    CSD = o.CSD,
                    CompanyId = (decimal?)o.CompanyId ?? 0,
                    CompanyNo = o.CompanyNo ?? "",
                    SizeCountryCodeId = (decimal?)o.SizeCountryCodeId ?? 0,
                    MPSProcessNameTw = mp.ProcessNameTw ?? "",
                    MPSProcessNameEn = mp.ProcessNameEn ?? "",
                });
            return items;
        }

        public IQueryable<Models.Views.MPSDailyAddPrint> GetMPSDailyAddPrint(string predicate)
        {
            var items = (
                from m in MPSDailyAdd.Get()
                join mm in MPSDailyMaterialAdd.Get() on new { MpsDailyAddId = m.Id, LocaleId = m.LocaleId } equals new { MpsDailyAddId = mm.MpsDailyAddId, LocaleId = mm.LocaleId } into mmGrp
                from mm in mmGrp.DefaultIfEmpty()
                join o in Orders.Get() on new { OrderNo = m.OrderNo } equals new { OrderNo = o.OrderNo } into oGrp
                from o in oGrp.DefaultIfEmpty()
                join ml in Material.Get() on new { Material = mm.MaterialNameTw, LocaleId = mm.LocaleId } equals new { Material = ml.MaterialName, LocaleId = ml.LocaleId } into mlGrp
                from ml in mlGrp.DefaultIfEmpty()
                join w in Warehouse.Get() on new { Material = mm.WarehouseNo, LocaleId = mm.LocaleId } equals new { Material = w.WarehouseNo, LocaleId = w.LocaleId } into wGrp
                from w in wGrp.DefaultIfEmpty()
                join u in MPSProcessUnit.Get() on new { OrgUnitId = m.ProcessUnitId, LocaleId = m.LocaleId } equals new { OrgUnitId = u.Id, LocaleId = u.LocaleId } into uGrp
                from u in uGrp.DefaultIfEmpty()
                select new Models.Views.MPSDailyAddPrint
                {
                    DailyDate = m.DailyDate,
                    DailyNo = m.DailyNo,
                    UnitNameTw = mm.UnitNameTw,
                    ProcessUnitTw = u.UnitNameTw,
                    OrderNo = m.OrderNo,
                    TotalUsage = mm.TotalUsage,
                    WarehouseNo = mm.WarehouseNo,
                    MaterialNameTw = mm.MaterialNameTw,
                    DailyType = m.DailyType,
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MaterialId = ml.Id,
                    WarehouseId = w.Id,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    OrderQty = m.OrderQty,
                    StyleNo = o.StyleNo,

                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .GroupBy(g => new
                {
                    g.DailyDate,
                    g.DailyNo,
                    g.OrderNo,
                    g.OrderQty,
                    g.MaterialNameTw,
                    g.WarehouseNo,
                    g.DailyType,
                    g.Id,
                    g.LocaleId,
                    g.MaterialId,
                    g.WarehouseId,
                    g.CompanyId,
                    g.CompanyNo,
                    g.UnitNameTw,
                    g.ProcessUnitTw,
                    g.StyleNo,
                })
                .Select(g => new Models.Views.MPSDailyAddPrint
                {
                    DailyDate = g.Key.DailyDate,
                    DailyNo = g.Key.DailyNo,
                    UnitNameTw = g.Key.UnitNameTw,
                    ProcessUnitTw = g.Key.ProcessUnitTw,
                    OrderNo = g.Key.OrderNo,
                    TotalUsage = g.Sum(g => g.TotalUsage),
                    WarehouseNo = g.Key.WarehouseNo,
                    MaterialNameTw = g.Key.MaterialNameTw,
                    DailyType = g.Key.DailyType,
                    Id = g.Key.Id,
                    LocaleId = g.Key.LocaleId,
                    MaterialId = g.Key.MaterialId,
                    WarehouseId = g.Key.WarehouseId,
                    CompanyId = g.Key.CompanyId,
                    CompanyNo = g.Key.CompanyNo,
                    OrderQty = g.Key.OrderQty,
                    PrintCount = MPSDailyAddPrintLog.Get().Where(i => i.LocaleId == g.Key.LocaleId && i.RefDailyNo == g.Key.DailyNo).Count(),
                    StyleNo = g.Key.StyleNo,
                })
                .ToList();

            return items.AsQueryable();
        }
        public Models.Views.MPSDailyAdd Create(Models.Views.MPSDailyAdd item)
        {
            var _item = MPSDailyAdd.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSDailyAdd Update(Models.Views.MPSDailyAdd item)
        {
            var _item = MPSDailyAdd.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSDailyAdd item)
        {
            MPSDailyAdd.Remove(Build(item));
        }
        private Models.Entities.MpsDailyAdd Build(Models.Views.MPSDailyAdd item)
        {
            return new Models.Entities.MpsDailyAdd()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                DailyNo = item.DailyNo,
                PreDailyNo = item.PreDailyNo,
                OpenDate = item.OpenDate,
                DailyDate = item.DailyDate,
                FinishedDate = item.FinishedDate,
                ProcessId = item.ProcessId,
                ProcessUnitId = item.ProcessUnitId,
                OrderNo = item.OrderNo,
                MpsStyleId = item.MpsStyleId,
                OrderQty = item.OrderQty,
                Qty = item.Qty,
                SizeCountryNameTw = item.SizeCountryNameTw,
                DailyMode = item.DailyMode,
                DailyType = item.DailyType,
                DoDaily = item.DoDaily,
                DoDate = item.DoDate,
                Multi = item.Multi,
                Remark = item.Remark,
                SeqId = item.SeqId,
                MaterialCost = item.MaterialCost,
                DollarNameTw = item.DollarNameTw,
                CostBalance = item.CostBalance,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }


        public void CreateRange(IEnumerable<Models.Views.MPSDailyAdd> items)
        {
            MPSDailyAdd.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsDailyAdd, bool>> predicate)
        {
            MPSDailyAdd.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyAdd> BuildRange(IEnumerable<Models.Views.MPSDailyAdd> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyAdd
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                DailyNo = item.DailyNo,
                PreDailyNo = item.PreDailyNo,
                OpenDate = item.OpenDate,
                DailyDate = item.DailyDate,
                FinishedDate = item.FinishedDate,
                ProcessId = item.ProcessId,
                ProcessUnitId = item.ProcessUnitId,
                OrderNo = item.OrderNo,
                MpsStyleId = item.MpsStyleId,
                OrderQty = item.OrderQty,
                Qty = item.Qty,
                SizeCountryNameTw = item.SizeCountryNameTw,
                DailyMode = item.DailyMode,
                DailyType = item.DailyType,
                DoDaily = item.DoDaily,
                DoDate = item.DoDate,
                Multi = item.Multi,
                Remark = item.Remark,
                SeqId = item.SeqId,
                MaterialCost = item.MaterialCost,
                DollarNameTw = item.DollarNameTw,
                CostBalance = item.CostBalance,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

    }
}
