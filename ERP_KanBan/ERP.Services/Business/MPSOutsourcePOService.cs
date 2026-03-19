using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Entities;
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
    public class MPSOutsourcePOService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSProcedurePOService MPSProcedurePO { get; set; }
        private ERP.Services.Business.Entities.MPSProcedurePOSizeService MPSProcedurePOSize { get; set; }
        private ERP.Services.Business.Entities.MPSProcedurePOItemService MPSProcedurePOItem { get; set; }

        private ERP.Services.Business.Entities.MPSProcedureGroupService MPSProcedureGroup { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureGroupItemService MPSProcedureGroupItem { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureQuotService MPSProcedureQuot { get; set; }

        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.OrdersItemService OrdersItem { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }


        public MPSOutsourcePOService(
            ERP.Services.Business.Entities.MPSProcedurePOService mpsProcedurePOService,
            ERP.Services.Business.Entities.MPSProcedurePOSizeService mpsProcedurePOSizeService,
            ERP.Services.Business.Entities.MPSProcedurePOItemService mpsProcedurePOItemService,
            ERP.Services.Business.Entities.MPSProcedureGroupService mpsProcedureGroupService,
            ERP.Services.Business.Entities.MPSProcedureGroupItemService mpsProcedureGroupItemService,
            ERP.Services.Business.Entities.MPSProcedureQuotService mpsProcedureQuotService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.OrdersItemService ordersItemService,
            ERP.Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePO = mpsProcedurePOService;
            MPSProcedurePOSize = mpsProcedurePOSizeService;
            MPSProcedurePOItem = mpsProcedurePOItemService;

            MPSProcedureGroup = mpsProcedureGroupService;
            MPSProcedureGroupItem = mpsProcedureGroupItemService;
            MPSProcedureQuot = mpsProcedureQuotService;

            Orders = ordersService;
            OrdersItem = ordersItemService;
            Company = companyService;
        }

        public ERP.Models.Views.MPSOutsourcePOGroup Get(int id, int localeId)
        {
            var group = new MPSOutsourcePOGroup();

            var po = MPSProcedurePO.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (po != null)
            {
                group.MPSProcedurePO = po;
                group.MPSProcedurePOSize = MPSProcedurePOSize.Get().Where(i => i.MpsProcedurePOId == id && i.LocaleId == localeId).OrderBy(i => i.SeqId).ToList();
                group.MPSProcedurePOItem = MPSProcedurePOItem.Get().Where(i => i.MpsProcedurePOId == id && i.LocaleId == localeId).OrderBy(i => i.ProcedureNameTw).ToList();
                group.MPSProcedureGroup = MPSProcedureGroup.Get().Where(i => i.StyleNo == po.StyleNo && i.LocaleId == po.LocaleId).OrderBy(i => i.GroupNameLocal).ToList();
                group.MPSProcedureGroupItem = MPSProcedureGroupItem.GetWhitStyle().Where(i => i.StyleNo == po.StyleNo && i.LocaleId == po.LocaleId).OrderBy(i => i.ProcedureNameTw).ToList();
                group.MPSProcedureQuot = MPSProcedureQuot.Get().Where(i => i.StyleNo == po.StyleNo && i.LocaleId == po.LocaleId).OrderBy(i => i.VendorShortNameTw).ToList();

                var mpsPOs = MPSProcedurePO.Get().Where(i => i.OrderNo == po.OrderNo && i.LocaleId == localeId && i.Type == 1).GroupBy(g => new { g.OrderNo, g.MpsProcedureGroupNameTw }).Select(i => new { i.Key.OrderNo, i.Key.MpsProcedureGroupNameTw, TotalQty = i.Sum(g => g.Qty) }).ToList();
                group.MPSProcedureGroup.ToList().ForEach(g =>
                {
                    var mpsPO = mpsPOs.Where(i => i.MpsProcedureGroupNameTw == g.GroupNameLocal).FirstOrDefault();
                    g.TotalOutsouceQty = mpsPO == null ? 0 : mpsPO.TotalQty;
                });
            }
            return group;
        }

        public ERP.Models.Views.MPSOutsourcePOGroup Save(MPSOutsourcePOGroup group)
        {
            var po = group.MPSProcedurePO;
            var poSize = group.MPSProcedurePOSize.ToList();
            var poItem = group.MPSProcedurePOItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();

                if (po != null)
                {
                    //po
                    {
                        // 部分欄位資料處理
                        {
                            po.IsShowSizeRun = poSize.Count() > 1 ? 1 : 0;

                        }
                        var _po = MPSProcedurePO.Get().Where(i => i.LocaleId == po.LocaleId && i.Id == po.Id).FirstOrDefault();
                        if (_po == null)
                        {
                            // var _items = MPSProcedurePO.GetEntity().Where(i => i.OrderNo == po.OrderNo && i.LocaleId == po.LocaleId).Select(i => i.SeqId).ToList();
                            // var seq = _items.Any() ? _items.Max() : 0;
                            var seq = MPSProcedurePO.GetEntity().Where(i => i.OrderNo == po.OrderNo && i.LocaleId == po.LocaleId).OrderByDescending(i => i.SeqId).Select(i => i.SeqId).FirstOrDefault();

                            var companies = Company.Get().ToList();
                            var remark = "1.交貨之品質不符合或數量不足交貨期延誤，因此而影響本公司生產售方應負擔一切損失。\n2.交貨時請於簽單及材料包裝上註明訂單號碼，請款時務必列明並附上訂購單。\n3.本月份貨款於隔月25號日領取化款\n4.請嚴格遵守上關材料品質入期交貨。\n5.禁含AZO偶氮染料。\n※送貨時間請於上班時間內(週一到週五，上午8點~12點與下午1點~5點。\n※禁含致癌AZO偶氮染料.鉛等八大重金屬致癌物質。\n※提前交貨應事先徵得我方之書面同意，否則將拒絕收貨。";

                            po.SeqId = seq + 1;
                            po.PONo = po.Type == 1 ? po.PODate.ToString("yyyyMMdd") + "-" + po.OrderNo + "-" + po.SeqId.ToString("00") + "-" + po.MpsProcedureGroupNameTw
                                                   : "RE" + "-" + po.PODate.ToString("yyyyMMdd") + "-" + po.OrderNo + "-" + po.SeqId.ToString("00") + "-" + po.MpsProcedureGroupNameTw;
                            po.Remark = remark;
                            po = MPSProcedurePO.Create(po);
                        }
                        else
                        {
                            po.Id = _po.Id;
                            po.LocaleId = _po.LocaleId;
                            po = MPSProcedurePO.Update(po);
                        }
                    }
                    //poSize
                    {
                        if (po.Id != 0)
                        {
                            var sid = 0;
                            poSize.ForEach(i =>
                            {
                                sid += 1;

                                i.MpsProcedurePOId = po.Id;
                                i.LocaleId = po.LocaleId;
                                i.SeqId = sid;
                            });
                            MPSProcedurePOSize.RemoveRange(i => i.MpsProcedurePOId == po.Id && i.LocaleId == po.LocaleId);
                            MPSProcedurePOSize.CreateRange(poSize);

                            poItem.ForEach(i =>
                            {
                                i.MpsProcedurePOId = po.Id;
                                i.LocaleId = po.LocaleId;
                            });
                            MPSProcedurePOItem.RemoveRange(i => i.MpsProcedurePOId == po.Id && i.LocaleId == po.LocaleId);
                            MPSProcedurePOItem.CreateRange(poItem);
                        }
                    }
                }
                UnitOfWork.Commit();
                return Get((int)po.Id, (int)po.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(MPSOutsourcePOGroup group)
        {
            var po = group.MPSProcedurePO;
            try
            {
                UnitOfWork.BeginTransaction();
                if (po != null)
                {
                    MPSProcedurePOSize.RemoveRange(i => i.MpsProcedurePOId == po.Id && i.LocaleId == po.LocaleId);
                    MPSProcedurePOItem.RemoveRange(i => i.MpsProcedurePOId == po.Id && i.LocaleId == po.LocaleId);
                    MPSProcedurePO.Remove(po);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.MPSOutsourcePOGroup BuildMPSOutsourcePOGroup(string orderNo, int localeId)
        {
            var group = new MPSOutsourcePOGroup();
            var order = Orders.Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId).FirstOrDefault();

            if (order != null)
            {
                var mpsGroup = MPSProcedureGroup.Get().Where(i => i.StyleNo == order.StyleNo && i.LocaleId == order.LocaleId).OrderBy(i => i.GroupNameLocal).ToList();
                var mpsGroupItem = MPSProcedureGroupItem.GetWhitStyle().Where(i => i.StyleNo == order.StyleNo && i.LocaleId == order.LocaleId).OrderBy(i => i.ProcedureNameTw).ToList();
                var mpsQot = MPSProcedureQuot.Get().Where(i => i.StyleNo == order.StyleNo && i.LocaleId == order.LocaleId).OrderBy(i => i.VendorShortNameTw).ToList();
                var mpsPOs = MPSProcedurePO.Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId && i.Type == 1).GroupBy(g => new { g.OrderNo, g.MpsProcedureGroupNameTw }).Select(i => new { i.Key.OrderNo, i.Key.MpsProcedureGroupNameTw, TotalQty = i.Sum(g => g.Qty) }).ToList();

                mpsGroup.ForEach(g =>
                {
                    var mpsPO = mpsPOs.Where(i => i.MpsProcedureGroupNameTw == g.GroupNameLocal).FirstOrDefault();
                    g.TotalOutsouceQty = mpsPO == null ? 0 : mpsPO.TotalQty;
                });
                group.MPSProcedureGroup = mpsGroup;
                group.MPSProcedureGroupItem = mpsGroupItem;
                group.MPSProcedureQuot = mpsQot;

                group.MPSProcedurePO = new MPSProcedurePO
                {
                    Id = 0,
                    LocaleId = order.LocaleId,
                    Type = 1,
                    MpsProcedureVendorId = 0,
                    OrderNo = orderNo,
                    SeqId = 0,
                    PONo = "",
                    StyleNo = order.StyleNo,
                    MpsProcedureGroupNameTw = "",
                    PODate = DateTime.Today,
                    UnitPrice = 0,
                    DollarNameTw = "",
                    Qty = 0,
                    PurUnitName = "PR",
                    VendorETD = DateTime.Today.AddDays(7),
                    PayCodeId = 0,
                    ReceivingLocaleId = localeId,
                    PaymentLocaleId = localeId,
                    PaymentCodeId = 0,
                    PaymentPoint = 0,
                    IsOverQty = 1,
                    IsAllowPartial = 1,
                    IsShowSizeRun = 1,
                    SamplingMethod = 0,
                    Status = 0, // 沒作用，但默認要為0
                    SpecDesc = "",
                    Remark = "",
                    PhotoURL = "",
                    PhotoURLDescTw = "",
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    PayQty = 0,
                    WarehouseNo = "",
                    PriceType = 1,

                    MPSVendor = "",
                    Vendor = "",
                    Amount = 0,
                    TotalOutsouceQty = 0,
                    OrderQty = order.OrderQty,
                };
                group.MPSProcedurePOSize = OrdersItem.Get()
                    .Where(i => i.OrdersId == order.Id && i.LocaleId == order.LocaleId)
                    .Select(i => new MPSProcedurePOSize
                    {
                        Id = 0,
                        LocaleId = localeId,
                        MpsProcedurePOId = 0,
                        DisplaySize = i.DisplaySize,
                        SubQty = i.Qty,
                        PayType = null,
                        ArticleInnerSize = i.ArticleInnerSize,
                        SeqId = 0,
                    })
                    .OrderBy(i => i.ArticleInnerSize)
                    .ToList();
            }

            return group;
        }
        public ERP.Models.Views.MPSOutsourcePOBatchGroup BuildMPSOutsourcePOBatchGroup(string predicate, string[] filters)
        {
            var group = new ERP.Models.Views.MPSOutsourcePOBatchGroup();
            try
            {
                var status = 0;
                if (filters != null && filters.Length > 0)
                {
                    var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                    status = (int)extenFilters.Field1;
                }

                // var poQtys = MPSProcedurePO.Get().GroupBy(g => new { g.OrderNo, g.LocaleId, g.MpsProcedureGroupNameTw }).Select(i => new { i.Key.OrderNo, i.Key.LocaleId, i.Key.MpsProcedureGroupNameTw, TotalQty = i.Sum(g => g.Qty) });
                var items = (
                    from o in Orders.Get()
                    join g in MPSProcedureGroup.Get() on new { o.StyleNo, o.LocaleId } equals new { g.StyleNo, g.LocaleId }
                    join p in MPSProcedurePO.Get() on new { o.OrderNo, o.LocaleId, MpsProcedureGroupNameTw = g.GroupNameLocal } equals new { p.OrderNo, p.LocaleId, MpsProcedureGroupNameTw = p.MpsProcedureGroupNameTw } into pGRP
                    from p in pGRP.DefaultIfEmpty()
                    select new
                    {
                        OrderNo = o.OrderNo,
                        LocaleId = o.LocaleId,
                        StyleNo = o.StyleNo,
                        OrderQty = o.OrderQty,
                        LCSD = o.LCSD,
                        CSD = o.CSD,
                        CompanyNo = o.CompanyNo,
                        MpsProcedureGroupNameTw = g.GroupNameLocal,
                        MpsProcedureGroupId = g.Id,
                        OrderStatus = o.Status,

                        Id = (decimal?)p.Id,
                        Type = (int?)p.Type,
                        MpsProcedureVendorId = (decimal?)p.MpsProcedureVendorId,
                        SeqId = (int?)p.SeqId,
                        PONo = (string?)p.PONo,
                        PODate = (DateTime?)p.PODate,
                        UnitPrice = (decimal?)p.UnitPrice,
                        DollarNameTw = (string?)p.DollarNameTw,
                        Qty = (decimal?)p.Qty,
                        PurUnitName = (string?)p.PurUnitName,
                        VendorETD = (DateTime?)p.VendorETD,
                        PayCodeId = (int?)p.PayCodeId,
                        ReceivingLocaleId = (decimal?)p.ReceivingLocaleId,
                        PaymentLocaleId = (decimal?)p.PaymentLocaleId,
                        PaymentCodeId = (decimal?)p.PaymentCodeId,
                        PaymentPoint = (int?)p.PaymentPoint,
                        IsOverQty = (int?)p.IsOverQty,
                        IsAllowPartial = (int?)p.IsAllowPartial,
                        IsShowSizeRun = (int?)p.IsShowSizeRun,
                        SamplingMethod = (int?)p.SamplingMethod,
                        Status = (int?)p.Status,
                        SpecDesc = (string?)p.SpecDesc,
                        Remark = (string?)p.Remark,
                        PhotoURL = (string?)p.PhotoURL,
                        PhotoURLDescTw = (string?)p.PhotoURLDescTw,
                        ModifyUserName = (string?)p.ModifyUserName,
                        LastUpdateTime = (DateTime?)p.LastUpdateTime,
                        PayQty = (decimal?)p.PayQty,
                        WarehouseNo = (string?)p.WarehouseNo,
                        PriceType = (int?)p.PriceType,
                        MPSVendor = (string?)p.MPSVendor,
                        Vendor = (string?)p.Vendor,
                        MPSVendorShortName = p.MPSVendorShortName,
                        // Amount = p.UnitPrice * p.PayQty,
                        DayOfMonth = (decimal?)p.DayOfMonth,
                        TotalOutsouceQty = (decimal?)p.TotalOutsouceQty,

                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Distinct()
                .ToList();

                var result = items.Select(i => new ERP.Models.Views.MPSProcedurePO
                {
                    OrderNo = i.OrderNo,
                    OrderQty = i.OrderQty,
                    LocaleId = i.LocaleId,
                    StyleNo = i.StyleNo,
                    MpsProcedureGroupNameTw = i.MpsProcedureGroupNameTw,
                    MpsProcedureGroupId = i.MpsProcedureGroupId,

                    Id = i.Id ?? 0,
                    Type = i.Type ?? 1,
                    MpsProcedureVendorId = i.MpsProcedureVendorId ?? 0,
                    SeqId = i.SeqId ?? 0,
                    PONo = i.PONo ?? "",

                    PODate = i.PODate ?? DateTime.Today,
                    UnitPrice = i.UnitPrice ?? 0,
                    DollarNameTw = i.DollarNameTw ?? "",
                    Qty = i.Qty ?? 0,
                    PurUnitName = i.PurUnitName ?? "PR",
                    VendorETD = i.VendorETD ?? DateTime.Today.AddDays(7),
                    PayCodeId = i.PayCodeId ?? 0,
                    ReceivingLocaleId = i.ReceivingLocaleId ?? i.LocaleId,
                    PaymentLocaleId = i.PaymentLocaleId ?? i.LocaleId,
                    PaymentCodeId = i.PaymentCodeId ?? 0,
                    PaymentPoint = i.PaymentPoint ?? 0,
                    IsOverQty = i.IsOverQty ?? 1,
                    IsAllowPartial = i.IsAllowPartial ?? 1,
                    IsShowSizeRun = i.IsShowSizeRun ?? 1,
                    SamplingMethod = i.SamplingMethod ?? 0,
                    Status = i.Status ?? 0, // 沒作用，但默認要為0
                    SpecDesc = i.SpecDesc ?? "",
                    Remark = i.Remark ?? "",
                    PhotoURL = i.PhotoURL ?? "",
                    PhotoURLDescTw = i.PhotoURLDescTw ?? "",
                    ModifyUserName = i.ModifyUserName ?? "",
                    LastUpdateTime = i.LastUpdateTime ?? DateTime.Now,
                    PayQty = i.PayQty ?? 0,
                    WarehouseNo = i.WarehouseNo ?? "",
                    PriceType = i.PriceType ?? 1,

                    MPSVendor = i.MPSVendor ?? "",
                    Vendor = i.Vendor ?? "",
                    Amount = 0,
                    TotalOutsouceQty = i.TotalOutsouceQty ?? 0,
                })
                .OrderBy(i => i.OrderNo)
                .ThenBy(i => i.MpsProcedureGroupNameTw)
                .ToList();

                if (status == 1)
                {
                    result = result.Where(i => i.Id == 0).ToList();
                }
                else if (status == 2)
                {
                    result = result.Where(i => i.Id > 0).ToList();
                }

                var styleNos = result.Select(i => i.StyleNo).Distinct().ToList();
                var localeId = result.Select(i => i.LocaleId).Distinct().FirstOrDefault();
                var process = result.Select(i => i.MpsProcedureGroupNameTw).Distinct().ToList();
                var mpsQot = MPSProcedureQuot.Get().Where(i => styleNos.Contains(i.StyleNo) && i.LocaleId == localeId && process.Contains(i.MpsProcedureGroupName)).OrderBy(i => i.VendorShortNameTw).ToList();

                result.ForEach(i =>
                {
                    if (i.Id == 0)
                    {
                        var quot = mpsQot.Where(q => q.MpsProcedureGroupName == i.MpsProcedureGroupNameTw).FirstOrDefault();
                        if (quot != null)
                        {
                            i.MpsProcedureVendorId = quot.MpsProcedureVendorId;
                            i.MPSVendor = quot.VendorNameTw;
                            i.MPSVendorShortName = quot.VendorShortNameTw;
                            i.DollarNameTw = quot.DollarNameTw;
                            i.UnitPrice = quot.UnitPrice;
                            i.PaymentCodeId = quot.PaymentCodeId;
                        }
                    }
                });
                group.MPSProcedurePO = result;
                group.MPSProcedureQuot = mpsQot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return group;
        }

        public void SaveBatch(List<ERP.Models.Views.MPSProcedurePO> items)
        {
            var groupIds = items.Select(i => i.MpsProcedureGroupId).Distinct().ToList();
            var styleNos = items.Select(i => i.StyleNo).Distinct().ToList();
            var orderNos = items.Select(i => i.OrderNo).Distinct().ToList();
            var localeIds = items.Select(i => i.LocaleId).Distinct().ToList();

            try
            {
                UnitOfWork.BeginTransaction();
                var mpsGroupItems = MPSProcedureGroupItem.GetWhitStyle().Where(i => styleNos.Contains(i.StyleNo) && localeIds.Contains(i.LocaleId)).ToList();
                var orderItems = (
                    from o in Orders.Get()
                    join oi in OrdersItem.Get() on new { OrdersId = (decimal?)o.Id, LocaleId = o.LocaleId } equals new { OrdersId = oi.OrdersId, LocaleId = oi.LocaleId }
                    where orderNos.Contains(o.OrderNo) && localeIds.Contains(o.LocaleId)
                    select new
                    {
                        Id = 0,
                        DisplaySize = oi.DisplaySize,
                        Qty = oi.Qty,
                        ArticleInnerSize = oi.ArticleInnerSize,
                        OrderNo = o.OrderNo,
                        OrdersId = o.Id,
                        LocaleId = o.LocaleId,
                    })
                    .ToList();

                var poIds = new List<decimal>();

                items.ForEach(po =>
                {
                    // createPO
                    var seq = MPSProcedurePO.GetEntity().Where(i => i.OrderNo == po.OrderNo && i.LocaleId == po.LocaleId).OrderByDescending(i => i.SeqId).Select(i => i.SeqId).FirstOrDefault();

                    var companies = Company.Get().ToList();
                    var remark = "1.交貨之品質不符合或數量不足交貨期延誤，因此而影響本公司生產售方應負擔一切損失。\n2.交貨時請於簽單及材料包裝上註明訂單號碼，請款時務必列明並附上訂購單。\n3.本月份貨款於隔月25號日領取化款\n4.請嚴格遵守上關材料品質入期交貨。\n5.禁含AZO偶氮染料。\n※送貨時間請於上班時間內(週一到週五，上午8點~12點與下午1點~5點。\n※禁含致癌AZO偶氮染料.鉛等八大重金屬致癌物質。\n※提前交貨應事先徵得我方之書面同意，否則將拒絕收貨。";

                    po.SeqId = seq + 1;
                    po.PONo = po.Type == 1 ? po.PODate.ToString("yyyyMMdd") + "-" + po.OrderNo + "-" + po.SeqId.ToString("00") + "-" + po.MpsProcedureGroupNameTw
                                           : "RE" + "-" + po.PODate.ToString("yyyyMMdd") + "-" + po.OrderNo + "-" + po.SeqId.ToString("00") + "-" + po.MpsProcedureGroupNameTw;
                    po.Remark = remark;
                    po.SpecDesc = po.MpsProcedureGroupNameTw;
                    po.Qty = po.OrderQty ?? 0;
                    po.PayQty = po.OrderQty ?? 0;
                    var _po = MPSProcedurePO.Create(po);

                    var poItem = mpsGroupItems.Where(i => i.StyleNo == po.StyleNo && i.MpsProcedureGroupId == po.MpsProcedureGroupId)
                        .Select(i => new Models.Views.MPSProcedurePOItem
                        {
                            Id = 0,
                            LocaleId = _po.LocaleId,
                            MpsProcedurePOId = _po.Id,
                            ProcedureNameTw = i.ProcedureNameTw,
                            PairsStandardTime = i.PairsStandardTime,
                            PairsStandardPrice = i.PairsStandardPrice

                        }).ToList();

                    MPSProcedurePOItem.RemoveRange(i => i.MpsProcedurePOId == _po.Id && i.LocaleId == _po.LocaleId);
                    MPSProcedurePOItem.CreateRange(poItem);

                    var poSize = orderItems.Where(i => i.OrderNo == _po.OrderNo && i.LocaleId == _po.LocaleId)
                        .Select(i => new MPSProcedurePOSize
                        {
                            Id = 0,
                            LocaleId = _po.LocaleId,
                            MpsProcedurePOId = _po.Id,
                            DisplaySize = i.DisplaySize,
                            SubQty = i.Qty,
                            PayType = po.PayType,
                            ArticleInnerSize = i.ArticleInnerSize,
                            SeqId = 0,
                        })
                        .OrderBy(i => i.ArticleInnerSize)
                        .ToList();

                    var sid = 0;
                    poSize.ForEach(i =>
                    {
                        sid += 1;
                        i.SeqId = sid;
                    });
                    MPSProcedurePOSize.RemoveRange(i => i.MpsProcedurePOId == _po.Id && i.LocaleId == _po.LocaleId);
                    MPSProcedurePOSize.CreateRange(poSize);
                });

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
