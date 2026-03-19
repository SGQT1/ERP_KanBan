using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using Newtonsoft.Json;
using ERP.Models.Views.Common;

namespace ERP.Services.Business
{
    public class MPSOutsourceInspectLogService : BusinessService
    {

        private ERP.Services.Business.Entities.MPSInspectLogService MPSInspectLog { get; set; }
        private ERP.Services.Business.Entities.MPSInspectLogSizeItemService MPSInspectLogSizeItem { get; set; }

        private ERP.Services.Business.Entities.MPSProcedurePOService MPSProcedurePO { get; set; }
        private ERP.Services.Business.Entities.MPSProcedurePOSizeService MPSProcedurePOSize { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureVendorService MPSProcedureVendor { get; set; }

        public MPSOutsourceInspectLogService(
            ERP.Services.Business.Entities.MPSProcedurePOService mpsProcedurePOService,
            ERP.Services.Business.Entities.MPSProcedurePOSizeService mpsProcedurePOSizeService,
            ERP.Services.Business.Entities.MPSInspectLogService mpsInspectLogService,
            ERP.Services.Business.Entities.MPSInspectLogSizeItemService mpsInspectLogSizeItemService,
            ERP.Services.Business.Entities.MPSProcedureVendorService mpsProcedureVendorService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePO = mpsProcedurePOService;
            MPSProcedurePOSize = mpsProcedurePOSizeService;
            MPSInspectLog = mpsInspectLogService;
            MPSInspectLogSizeItem = mpsInspectLogSizeItemService;
            MPSProcedureVendor = mpsProcedureVendorService;
        }

        public IQueryable<ERP.Models.Views.MPSReceivedLogForInspect> GetMPSProcedureReceivedForInspect(string predicate)
        {
            var result = (
                from mp in MPSProcedurePO.Get()
                // join mpv in MPSProcedureVendor.Get() on new { MpsProcedureVendorId = mp.MpsProcedureVendorId, LocaleId = mp.LocaleId } equals
                //                                         new { MpsProcedureVendorId = mpv.Id, LocaleId = mpv.LocaleId } 
                join mpr in MPSInspectLog.Get() on new { MPSProcedurePOId = mp.Id, LocaleId = mp.LocaleId } equals
                                                    new { MPSProcedurePOId = mpr.MpsProcedurePOId, LocaleId = mpr.LocaleId }
                select new Models.Views.MPSReceivedLogForInspect
                {
                    Id = mp.Id,
                    LocaleId = mp.LocaleId,
                    MpsProcedureVendorId = mp.MpsProcedureVendorId,
                    OrderNo = mp.OrderNo,
                    PONo = mp.PONo,
                    StyleNo = mp.StyleNo,
                    MpsProcedureGroupNameTw = mp.MpsProcedureGroupNameTw,
                    Qty = mp.Qty,
                    PurUnitName = mp.PurUnitName,
                    MPSVendor = mp.MPSVendor,
                    Status = mp.Status,
                    ReceivedId = mpr.Id,
                    ReceivedDate = mpr.ReceivedDate,
                    ReceivedQty = mpr.ReceivedQty,
                    MPSVendorShortName = mp.MPSVendorShortName,
                    MPSReceivedLogId = mpr.Id,
                    DayOfMonth = mp.DayOfMonth,
                    WarehouseNo = mp.WarehouseNo,
                    SumQty = MPSInspectLog.Get().Where(i => i.MpsProcedurePOId == mp.Id && i.LocaleId == mp.LocaleId).Sum(i => i.ReceivedQty),
                    IsReceived = 1,
                    DoPay = mpr.DoPay,
                    QCBackQty = mpr.QCBackQty,
                    ChargeQty = mpr.ChargeQty,
                    FreeQty = mpr.FreeQty,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList();

            return result.AsQueryable();
        }

        public ERP.Models.Views.MPSOutsourceInspectLogGroup Get(int poId, int inspectId, int localeId)
        {
            var group = new MPSOutsourceInspectLogGroup();

            var po = MPSProcedurePO.Get().Where(i => i.Id == poId && i.LocaleId == localeId).FirstOrDefault();
            var received = MPSInspectLog.Get().Where(i => i.Id == inspectId && i.LocaleId == localeId).FirstOrDefault();
            if (po != null)
            {
                //Step1:取得這張PO的Size
                var poSize = MPSProcedurePOSize.Get().Where(i => i.MpsProcedurePOId == poId && i.LocaleId == localeId).OrderBy(i => i.SeqId).ToList();
                //Step2:取得這張PO的總收貨數
                var rSize = (
                    from rs in MPSInspectLogSizeItem.Get()
                    join r in MPSInspectLog.Get() on new { MpsReceivedLogId = rs.MpsReceivedLogId, LocaleId = rs.LocaleId } equals new { MpsReceivedLogId = r.Id, LocaleId = r.LocaleId }
                    where r.MpsProcedurePOId == po.Id && r.LocaleId == po.LocaleId
                    select new { MpsProcedurePOId = r.MpsProcedurePOId, MpsReceivedLogId = r.Id, DisplaySize = rs.DisplaySize, QCQty = rs.QCQty, LocaleId = rs.LocaleId }
                )
                .GroupBy(g => new { g.MpsProcedurePOId, g.MpsReceivedLogId, g.LocaleId, g.DisplaySize })
                .Select(g => new
                {
                    MpsProcedurePOId = g.Key.MpsProcedurePOId,
                    MpsReceivedLogId = g.Key.MpsReceivedLogId,
                    LocaleId = g.Key.LocaleId,
                    DisplaySize = g.Key.DisplaySize,
                    SumQty = g.Sum(i => i.QCQty)
                })
                .ToList();

                // 設定回傳值-訂單資料
                group.MPSProcedurePO = po;
                group.MPSProcedurePOSize = poSize;

                //如果是以收貨的資料呈現
                if (received != null)
                {
                    // 取結帳日
                    received.DayOfMonth = po.DayOfMonth;
                    // 生成收貨Size
                    var receivedSize = received == null ? null : MPSInspectLogSizeItem.Get().Where(i => i.MpsReceivedLogId == received.Id && i.LocaleId == localeId).OrderBy(i => i.Id).ToList();
                    receivedSize.ForEach(i =>
                    {
                        var item = poSize.Where(p => p.DisplaySize == i.DisplaySize).FirstOrDefault();
                        i.POQty = item.SubQty;
                        i.MPSPOSizeItemId = item.Id;
                        i.MaxQCQty = item.SubQty - rSize.Where(s => s.DisplaySize == i.DisplaySize).Sum(s => s.SumQty) + i.QCQty;
                    });
                    // 設定回傳值-收貨資料
                    group.MPSInspectLog = received;
                    group.MPSInspectLogSizeItem = receivedSize;
                }
                else
                {
                    // 設定回傳值-收貨資料
                    //如果是沒有收貨資料，就根據PO建立一筆新的收貨資料
                    group.MPSInspectLog = new MPSInspectLog
                    {
                        Id = 0,
                        LocaleId = po.LocaleId,
                        MpsProcedurePOId = po.Id,
                        RefLocaleId = po.LocaleId,
                        // ReceivedDate = null,
                        ReceivedQty = 0,
                        ChargeQty = 0,
                        FreeQty = 0,
                        QCBackQty = 0,
                        DoPay = 0,
                        // ModifyUserName = "",
                        // LastUpdateTime = i.LastUpdateTime,
                        // Remark = i.Remark,
                        // PayMonth = po.PayMonth,
                        AddFreeQty = 0,
                        DiscountRate = 1,
                        WarehouseNo = po.WarehouseNo,
                        DayOfMonth = po.DayOfMonth,
                    };
                    group.MPSInspectLogSizeItem = poSize.Select(i => new MPSInspectLogSizeItem
                    {
                        Id = 0,
                        LocaleId = i.LocaleId,
                        MpsReceivedLogId = 0,
                        DisplaySize = i.DisplaySize,
                        QCQty = i.SubQty - rSize.Where(s => s.DisplaySize == i.DisplaySize).Sum(s => s.SumQty),
                        FreeQty = 0,
                        SubQty = rSize.Where(s => s.DisplaySize == i.DisplaySize).Sum(s => s.SumQty),
                        POQty = i.SubQty,
                        MaxQCQty = i.SubQty - rSize.Where(s => s.DisplaySize == i.DisplaySize).Sum(s => s.SumQty),
                    });
                }
            }
            return group;
        }
        public ERP.Models.Views.MPSOutsourceInspectLogGroup Save(MPSOutsourceInspectLogGroup group)
        {
            var receivedLog = group.MPSInspectLog;
            var receivedLogSize = group.MPSInspectLogSizeItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();

                if (receivedLog != null)
                {
                    //po
                    {
                        var _receivedLog = MPSInspectLog.Get().Where(i => i.LocaleId == receivedLog.LocaleId && i.Id == receivedLog.Id).FirstOrDefault();

                        if (_receivedLog == null)
                        {
                            receivedLog = MPSInspectLog.Create(receivedLog);
                        }
                        else
                        {
                            receivedLog.Id = _receivedLog.Id;
                            receivedLog.LocaleId = _receivedLog.LocaleId;
                            receivedLog = MPSInspectLog.Update(receivedLog);
                        }
                    }
                    //poSize
                    {
                        if (receivedLog.Id != 0)
                        {
                            receivedLogSize.ForEach(i =>
                            {
                                i.MpsReceivedLogId = receivedLog.Id;
                                i.LocaleId = receivedLog.LocaleId;
                            });
                            MPSInspectLogSizeItem.RemoveRange(i => i.MpsReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId);
                            MPSInspectLogSizeItem.CreateRange(receivedLogSize);
                        }
                    }
                }
                UnitOfWork.Commit();
                return Get((int)receivedLog.MpsProcedurePOId, (int)receivedLog.Id, (int)receivedLog.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(MPSOutsourceInspectLogGroup group)
        {
            var receivedLog = group.MPSInspectLog;
            try
            {
                UnitOfWork.BeginTransaction();
                if (receivedLog != null)
                {
                    MPSInspectLogSizeItem.RemoveRange(i => i.MpsReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId);
                    MPSInspectLog.Remove(receivedLog);
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
