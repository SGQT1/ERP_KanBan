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
    public class MPSOutsourceReceivedLogService : BusinessService
    {

        private ERP.Services.Business.Entities.MPSReceivedLogService MPSReceivedLog { get; set; }
        private ERP.Services.Business.Entities.MPSReceivedLogSizeItemService MPSReceivedLogSizeItem { get; set; }

        private ERP.Services.Business.Entities.MPSProcedurePOService MPSProcedurePO { get; set; }
        private ERP.Services.Business.Entities.MPSProcedurePOSizeService MPSProcedurePOSize { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureVendorService MPSProcedureVendor { get; set; }

        public MPSOutsourceReceivedLogService(
            ERP.Services.Business.Entities.MPSProcedurePOService mpsProcedurePOService,
            ERP.Services.Business.Entities.MPSProcedurePOSizeService mpsProcedurePOSizeService,
            ERP.Services.Business.Entities.MPSReceivedLogService mpsReceivedLogService,
            ERP.Services.Business.Entities.MPSReceivedLogSizeItemService mpsReceivedLogSizeItemService,
            ERP.Services.Business.Entities.MPSProcedureVendorService mpsProcedureVendorService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePO = mpsProcedurePOService;
            MPSProcedurePOSize = mpsProcedurePOSizeService;
            MPSReceivedLog = mpsReceivedLogService;
            MPSReceivedLogSizeItem = mpsReceivedLogSizeItemService;
            MPSProcedureVendor = mpsProcedureVendorService;
        }

        public IQueryable<ERP.Models.Views.MPSProcedurePOForReceived> GetMPSOutsourcePOForReceivedLog(string predicate, string[] filters)
        {
            var receivedStatus = 0;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                receivedStatus = (int)extenFilters.Field1;
            }

            var result = (
                from mp in MPSProcedurePO.Get()
                join mpr in MPSReceivedLog.Get() on new { MPSProcedurePOId = mp.Id, LocaleId = mp.LocaleId } equals
                                                    new { MPSProcedurePOId = mpr.MpsProcedurePOId, LocaleId = mpr.LocaleId } into rGRP
                from mpr in rGRP.DefaultIfEmpty()
                select new Models.Views.MPSProcedurePOForReceived
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
                    SumQty = MPSReceivedLog.Get().Where(i => i.MpsProcedurePOId == mp.Id && i.LocaleId == mp.LocaleId).Sum(i => i.ReceivedQty),
                    IsReceived = mpr.Id == null ? 0 : 1,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate);

            var items = new List<Models.Views.MPSProcedurePOForReceived>();

            if (receivedStatus == 0)
            {
                // 全部
                var receiveds = result.ToList();

                // 未完全收的要再新增一筆剩餘的資料
                var helfReceived = receiveds.Where(i => i.SumQty.GetValueOrDefault() > 0 && i.SumQty.GetValueOrDefault() < i.Qty)
                    .Select(i => new Models.Views.MPSProcedurePOForReceived
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        MpsProcedureVendorId = i.MpsProcedureVendorId,
                        OrderNo = i.OrderNo,
                        PONo = i.PONo,
                        StyleNo = i.StyleNo,
                        MpsProcedureGroupNameTw = i.MpsProcedureGroupNameTw,
                        Qty = i.Qty,
                        PurUnitName = i.PurUnitName,
                        MPSVendor = i.MPSVendor,
                        Status = i.Status,
                        ReceivedId = 0,
                        ReceivedDate = DateTime.Today,
                        ReceivedQty = i.Qty - i.SumQty,
                        MPSVendorShortName = i.MPSVendorShortName,
                        MPSReceivedLogId = 0,
                        DayOfMonth = i.DayOfMonth,
                        WarehouseNo = i.WarehouseNo,
                        SumQty = i.SumQty,
                        IsReceived = 0,
                    })
                    .GroupBy(x => new { x.Id, x.LocaleId, x.PONo }) // 群組條件
                    .Select(g => g.First()) // 取每組的第一筆
                    .ToList();

                items = receiveds.Union(helfReceived)
                                .Select(i => new Models.Views.MPSProcedurePOForReceived
                                {
                                    Id = i.Id,
                                    LocaleId = i.LocaleId,
                                    MpsProcedureVendorId = i.MpsProcedureVendorId,
                                    OrderNo = i.OrderNo,
                                    PONo = i.PONo,
                                    StyleNo = i.StyleNo,
                                    MpsProcedureGroupNameTw = i.MpsProcedureGroupNameTw,
                                    Qty = i.Qty,
                                    PurUnitName = i.PurUnitName,
                                    MPSVendor = i.MPSVendor,
                                    Status = i.Status,
                                    ReceivedId = i.ReceivedId,
                                    ReceivedDate = i.ReceivedDate == null ? DateTime.Now : i.ReceivedDate,
                                    ReceivedQty = i.ReceivedQty == null ? (i.Qty - i.SumQty.GetValueOrDefault()) : i.ReceivedQty,
                                    MPSVendorShortName = i.MPSVendorShortName,
                                    MPSReceivedLogId = i.MPSReceivedLogId,
                                    DayOfMonth = i.DayOfMonth,
                                    WarehouseNo = i.WarehouseNo,
                                    SumQty = i.SumQty.GetValueOrDefault(),
                                    IsReceived = i.IsReceived,
                                }).ToList();   // 全部.ToList();

            }
            if (receivedStatus == 1)
            {
                items = result.Where(i => i.IsReceived == 0)
                              .Select(i => new Models.Views.MPSProcedurePOForReceived
                              {
                                  Id = i.Id,
                                  LocaleId = i.LocaleId,
                                  MpsProcedureVendorId = i.MpsProcedureVendorId,
                                  OrderNo = i.OrderNo,
                                  PONo = i.PONo,
                                  StyleNo = i.StyleNo,
                                  MpsProcedureGroupNameTw = i.MpsProcedureGroupNameTw,
                                  Qty = i.Qty,
                                  PurUnitName = i.PurUnitName,
                                  MPSVendor = i.MPSVendor,
                                  Status = i.Status,
                                  ReceivedId = i.ReceivedId,
                                  ReceivedDate = i.ReceivedDate == null ? DateTime.Now : i.ReceivedDate,
                                  ReceivedQty = i.ReceivedQty == null ? (i.Qty - i.SumQty.GetValueOrDefault()) : i.ReceivedQty,
                                  MPSVendorShortName = i.MPSVendorShortName,
                                  MPSReceivedLogId = i.MPSReceivedLogId,
                                  DayOfMonth = i.DayOfMonth,
                                  WarehouseNo = i.WarehouseNo,
                                  SumQty = i.SumQty.GetValueOrDefault(),
                                  IsReceived = i.IsReceived,
                              })
                              //   .GroupBy(x => new { x.Id, x.LocaleId, x.PONo, x.ReceivedId }) // 群組條件
                              //   .Select(g => g.First()) // 取每組的第一筆
                              .ToList();
            }
            else if (receivedStatus == 2)
            {
                // 未收+未收完
                var receiveds = result.Where(i => i.SumQty.GetValueOrDefault() < i.Qty)
                                      .Select(i => new Models.Views.MPSProcedurePOForReceived
                                      {
                                          Id = i.Id,
                                          LocaleId = i.LocaleId,
                                          MpsProcedureVendorId = i.MpsProcedureVendorId,
                                          OrderNo = i.OrderNo,
                                          PONo = i.PONo,
                                          StyleNo = i.StyleNo,
                                          MpsProcedureGroupNameTw = i.MpsProcedureGroupNameTw,
                                          Qty = i.Qty,
                                          PurUnitName = i.PurUnitName,
                                          MPSVendor = i.MPSVendor,
                                          Status = i.Status,
                                          ReceivedId = i.ReceivedId,
                                          ReceivedDate = i.ReceivedDate == null ? DateTime.Now : i.ReceivedDate,
                                          ReceivedQty = i.ReceivedQty == null ? (i.Qty - i.SumQty.GetValueOrDefault()) : i.ReceivedQty,
                                          MPSVendorShortName = i.MPSVendorShortName,
                                          MPSReceivedLogId = i.MPSReceivedLogId,
                                          DayOfMonth = i.DayOfMonth,
                                          WarehouseNo = i.WarehouseNo,
                                          SumQty = i.SumQty.GetValueOrDefault(),
                                          IsReceived = i.IsReceived,
                                      })
                                      .ToList();   // 全部

                // 未完全收的要再新增一筆剩餘的資料
                var helfReceived = receiveds.Where(i => i.SumQty.GetValueOrDefault() > 0 && i.SumQty.GetValueOrDefault() < i.Qty)
                    .Select(i => new Models.Views.MPSProcedurePOForReceived
                    {
                        Id = i.Id,
                        LocaleId = i.LocaleId,
                        MpsProcedureVendorId = i.MpsProcedureVendorId,
                        OrderNo = i.OrderNo,
                        PONo = i.PONo,
                        StyleNo = i.StyleNo,
                        MpsProcedureGroupNameTw = i.MpsProcedureGroupNameTw,
                        Qty = i.Qty,
                        PurUnitName = i.PurUnitName,
                        MPSVendor = i.MPSVendor,
                        Status = i.Status,
                        ReceivedId = 0,
                        ReceivedDate = DateTime.Now,
                        ReceivedQty = i.Qty - i.SumQty,
                        MPSVendorShortName = i.MPSVendorShortName,
                        MPSReceivedLogId = 0,
                        DayOfMonth = i.DayOfMonth,
                        WarehouseNo = i.WarehouseNo,
                        SumQty = i.SumQty,
                        IsReceived = 0,
                    })
                    .GroupBy(x => new { x.Id, x.LocaleId, x.PONo }) // 群組條件
                    .Select(g => g.First()) // 取每組的第一筆
                    .ToList();
                // 未收
                var noReceived = receiveds.Where(i => i.IsReceived == 0 && i.SumQty.GetValueOrDefault() == 0).ToList();
                items = helfReceived.Union(noReceived)
                                    .Select(i => new Models.Views.MPSProcedurePOForReceived
                                    {
                                        Id = i.Id,
                                        LocaleId = i.LocaleId,
                                        MpsProcedureVendorId = i.MpsProcedureVendorId,
                                        OrderNo = i.OrderNo,
                                        PONo = i.PONo,
                                        StyleNo = i.StyleNo,
                                        MpsProcedureGroupNameTw = i.MpsProcedureGroupNameTw,
                                        Qty = i.Qty,
                                        PurUnitName = i.PurUnitName,
                                        MPSVendor = i.MPSVendor,
                                        Status = i.Status,
                                        ReceivedId = i.ReceivedId,
                                        ReceivedDate = i.ReceivedDate == null ? DateTime.Today : i.ReceivedDate,
                                        ReceivedQty = i.ReceivedQty == null ? (i.Qty - i.SumQty.GetValueOrDefault()) : i.ReceivedQty,
                                        MPSVendorShortName = i.MPSVendorShortName,
                                        MPSReceivedLogId = i.MPSReceivedLogId,
                                        DayOfMonth = i.DayOfMonth,
                                        WarehouseNo = i.WarehouseNo,
                                        SumQty = i.SumQty.GetValueOrDefault(),
                                        IsReceived = i.IsReceived,
                                    })
                                    // .GroupBy(x => new { x.Id, x.LocaleId, x.PONo, x.ReceivedId }) // 群組條件
                                    // .Select(g => g.First()) // 取每組的第一筆
                                    .ToList();

            }
            else if (receivedStatus == 3)
            {   // 已收
                items = result.Where(i => i.IsReceived == 1).ToList();
            }

            return items.AsQueryable();
        }
        public IQueryable<ERP.Models.Views.MPSReceivedLog> GetMPSOutsourceReceivedLog(string predicate, string[] filters)
        {
            var result = (
                from mp in MPSProcedurePO.Get()
                join mpr in MPSReceivedLog.Get() on new { MPSProcedurePOId = mp.Id, LocaleId = mp.LocaleId } equals new { MPSProcedurePOId = mpr.MpsProcedurePOId, LocaleId = mpr.LocaleId } 
                select new Models.Views.MPSReceivedLog
                {
                    Id = mpr.Id,
                    LocaleId = mpr.LocaleId,
                    MpsProcedurePOId = mpr.MpsProcedurePOId,
                    RefLocaleId = mpr.RefLocaleId,
                    ReceivedDate = mpr.ReceivedDate,
                    ReceivedQty = mpr.ReceivedQty,
                    ChargeQty = mpr.ChargeQty,
                    FreeQty = mpr.FreeQty,
                    QCBackQty = mpr.QCBackQty,
                    doPay = mpr.doPay,
                    ModifyUserName = mpr.ModifyUserName,
                    LastUpdateTime = mpr.LastUpdateTime,
                    Remark = mpr.Remark,
                    PayMonth = mpr.PayMonth,
                    AddFreeQty = mpr.AddFreeQty,
                    DiscountRate = mpr.DiscountRate,
                    WarehouseNo = mpr.WarehouseNo,
       
                    MpsProcedureVendorId = mp.MpsProcedureVendorId,
                    MPSVendorShortName = mp.MPSVendorShortName,
                    OrderNo = mp.OrderNo,
                    PONo = mp.PONo,
                    StyleNo = mp.StyleNo,
                    MpsProcedureGroupNameTw = mp.MpsProcedureGroupNameTw,
                    Qty = mp.Qty,
                    PurUnitName = mp.PurUnitName,
                    MPSVendor = mp.MPSVendor,
                    Status = mp.Status,
                    VendorETD = mp.VendorETD,
                    PaymentLocaleId = mp.PaymentLocaleId,
                    SumQty = MPSReceivedLog.Get().Where(i => i.MpsProcedurePOId == mp.Id && i.LocaleId == mp.LocaleId).Sum(i => i.ReceivedQty),
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList();

            return result.AsQueryable();
        }

        public ERP.Models.Views.MPSOutsourceReceivedLogGroup Get(int poId, int receivedId, int localeId)
        {
            var group = new MPSOutsourceReceivedLogGroup();

            var po = MPSProcedurePO.Get().Where(i => i.Id == poId && i.LocaleId == localeId).FirstOrDefault();
            var received = MPSReceivedLog.Get().Where(i => i.Id == receivedId && i.LocaleId == localeId).FirstOrDefault();
            if (po != null)
            {
                //Step1:取得這張PO的Size
                var poSize = MPSProcedurePOSize.Get().Where(i => i.MpsProcedurePOId == poId && i.LocaleId == localeId).OrderBy(i => i.SeqId).ToList();
                //Step2:取得這張PO的總收貨數
                var rSize = (
                    from rs in MPSReceivedLogSizeItem.Get()
                    join r in MPSReceivedLog.Get() on new { MpsReceivedLogId = rs.MpsReceivedLogId, LocaleId = rs.LocaleId } equals new { MpsReceivedLogId = r.Id, LocaleId = r.LocaleId }
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
                    var receivedSize = received == null ? null : MPSReceivedLogSizeItem.Get().Where(i => i.MpsReceivedLogId == received.Id && i.LocaleId == localeId).OrderBy(i => i.Id).ToList();
                    receivedSize.ForEach(i =>
                    {
                        var item = poSize.Where(p => p.DisplaySize == i.DisplaySize).FirstOrDefault();
                        i.POQty = item.SubQty;
                        i.MPSPOSizeItemId = item.Id;
                        i.MaxQCQty = item.SubQty - rSize.Where(s => s.DisplaySize == i.DisplaySize).Sum(s => s.SumQty) + i.QCQty;
                    });
                    // 設定回傳值-收貨資料
                    group.MPSReceivedLog = received;
                    group.MPSReceivedLogSizeItem = receivedSize;
                }
                else
                {
                    // 設定回傳值-收貨資料
                    //如果是沒有收貨資料，就根據PO建立一筆新的收貨資料
                    group.MPSReceivedLog = new MPSReceivedLog
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
                        doPay = 0,
                        // ModifyUserName = "",
                        // LastUpdateTime = i.LastUpdateTime,
                        // Remark = i.Remark,
                        // PayMonth = po.PayMonth,
                        AddFreeQty = 0,
                        DiscountRate = 1,
                        WarehouseNo = po.WarehouseNo,
                        DayOfMonth = po.DayOfMonth,
                    };
                    group.MPSReceivedLogSizeItem = poSize.Select(i => new MPSReceivedLogSizeItem
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
        public ERP.Models.Views.MPSOutsourceReceivedLogGroup Save(MPSOutsourceReceivedLogGroup group)
        {
            var receivedLog = group.MPSReceivedLog;
            var receivedLogSize = group.MPSReceivedLogSizeItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();

                if (receivedLog != null)
                {
                    //po
                    {
                        var _receivedLog = MPSReceivedLog.Get().Where(i => i.LocaleId == receivedLog.LocaleId && i.Id == receivedLog.Id).FirstOrDefault();

                        if (_receivedLog == null)
                        {
                            receivedLog = MPSReceivedLog.Create(receivedLog);
                        }
                        else
                        {
                            receivedLog.Id = _receivedLog.Id;
                            receivedLog.LocaleId = _receivedLog.LocaleId;
                            receivedLog = MPSReceivedLog.Update(receivedLog);
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
                            MPSReceivedLogSizeItem.RemoveRange(i => i.MpsReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId);
                            MPSReceivedLogSizeItem.CreateRange(receivedLogSize);
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
        public void Remove(MPSOutsourceReceivedLogGroup group)
        {
            var receivedLog = group.MPSReceivedLog;
            try
            {
                UnitOfWork.BeginTransaction();
                if (receivedLog != null)
                {
                    MPSReceivedLogSizeItem.RemoveRange(i => i.MpsReceivedLogId == receivedLog.Id && i.LocaleId == receivedLog.LocaleId);
                    MPSReceivedLog.Remove(receivedLog);
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
