using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using Newtonsoft.Json;

namespace ERP.Services.Search
{
    public class RDPOService : SearchService
    {
        private ERP.Services.Entities.ProjectPOService ProjectPO { get; set; }
        private ERP.Services.Entities.ProjectPOItemService ProjectPOItem { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }

        public RDPOService(
            ERP.Services.Entities.ProjectPOService projectPOService,
            ERP.Services.Entities.ProjectPOItemService projectPOItemService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.ArticleSizeRunService articleSizeRunService,
            ERP.Services.Entities.CodeItemService codeItemService,
            ERP.Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            ProjectPO = projectPOService;
            ProjectPOItem = projectPOItemService;

            Material = materialService;
            Vendor = vendorService;
            CodeItem = codeItemService;
            Company = companyService;
        }

        public IQueryable<Models.Views.RDPOForVendor> GetRDPOForVendor(string predicate, string[] filters)
        {
            var result = (
                from poi in ProjectPOItem.Get()
                join p in ProjectPO.Get() on new { ProjectPOId = poi.ProjectPOId, LocaleId = poi.LocaleId } equals new { ProjectPOId = p.Id, LocaleId = p.LocaleId }
                join c in Company.Get() on new { LocaleId = poi.LocaleId } equals new { LocaleId = c.Id }
                join v in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new
                {
                    Id = p.Id,
                    LocaleId = poi.LocaleId,
                    ProjectPOId = poi.ProjectPOId,
                    SeqNo = poi.SeqNo,
                    RefLocaleId = poi.RefLocaleId,
                    WorkOrderNo = poi.WorkOrderNo,
                    StyleNo = poi.StyleNo,
                    MaterialNameTw = poi.MaterialNameTw,
                    VendorId = poi.VendorId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    ReceivedLocaleId = poi.ReceivedLocaleId,
                    ReceivedConfirmed = poi.ReceivedConfirmed,
                    DollarNameTw = poi.DollarNameTw,
                    DoAP = poi.DoAP,
                    APMonth = poi.APMonth,
                    IsCFM = poi.IsCFM,

                    ProjectPODate = p.ProjectPODate,
                    Type = p.Type,
                    ProjectPONo = poi.ProjectPONo,
                    VendorNameTw = v.ShortNameTw,
                    ModifyUserName = poi.ModifyUserName,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.RDPOForVendor
            {
                // Id = i.Id,
                LocaleId = i.LocaleId,
                Type = i.Type,
                VendorId = i.VendorId,
                VendorNameTw = i.VendorNameTw,
            })
            .Distinct()
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.RDPOItem> GetRDPOItem(string predicate)
        {
            var result = (
                from poi in ProjectPOItem.Get()
                join p in ProjectPO.Get() on new { ProjectPOId = poi.ProjectPOId, LocaleId = poi.LocaleId } equals new { ProjectPOId = p.Id, LocaleId = p.LocaleId }
                join c in Company.Get() on new { LocaleId = poi.LocaleId } equals new { LocaleId = c.Id }
                join v in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.RDPOItem
                {
                    Id = poi.Id,
                    LocaleId = poi.LocaleId,
                    ProjectPOId = poi.ProjectPOId,
                    SeqNo = poi.SeqNo,
                    RefLocaleId = poi.RefLocaleId,
                    WorkOrderNo = poi.WorkOrderNo,
                    StyleNo = poi.StyleNo,
                    DevPairs = poi.DevPairs,
                    PlanPairs = poi.PlanPairs,
                    MaterialNameTw = poi.MaterialNameTw,
                    VendorId = poi.VendorId,
                    PlanQty = poi.PlanQty,
                    UnitNameTw = poi.UnitNameTw,
                    PayCodeId = poi.PayCodeId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    Remark = poi.Remark,
                    FirstProjectPODate = poi.FirstProjectPODate,
                    ModifyUserName = poi.ModifyUserName,
                    LastUpdateTime = poi.LastUpdateTime,
                    VendorETD = poi.VendorETD,
                    ReceivedLocaleId = poi.ReceivedLocaleId,
                    ReceivedDate = poi.ReceivedDate,
                    ReceivedQty = poi.ReceivedQty,
                    ReceivedConfirmed = poi.ReceivedConfirmed,
                    RvdUserName = poi.RvdUserName,
                    RvdUpdateTime = poi.RvdUpdateTime,
                    QuotUnitPrice = poi.QuotUnitPrice,
                    PayUnitPrice = poi.PayUnitPrice,
                    ExtraAmount = poi.ExtraAmount,
                    DollarNameTw = poi.DollarNameTw,
                    Amount = poi.Amount,
                    PayQty = poi.PayQty,
                    Discount = poi.Discount,
                    APAmount = poi.APAmount,
                    DoAP = poi.DoAP,
                    APMonth = poi.APMonth,
                    ShoeName = poi.ShoeName,
                    CFMUserName = poi.CFMUserName,
                    CFMTime = poi.CFMTime,
                    IsCFM = poi.IsCFM,
                    DiscountRate = poi.DiscountRate,

                    ProjectPODate = p.ProjectPODate,
                    Type = p.Type,
                    // ProjectPONo = c.CompanyNo + p.ProjectPODate.ToString("yyyyMMdd") + "-" + p.Id.ToString().PadLeft(2, '0').Substring(p.Id.ToString().PadLeft(2, '0').Length - 2) + "-" + poi.SeqNo,
                    ProjectPONo = poi.ProjectPONo,
                    VendorNameTw = v.ShortNameTw,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.RDPayableForVendor> GetRDMonthAPForRecd(string predicate, string[] filters)
        {
            var result = (
                from poi in ProjectPOItem.Get()
                join p in ProjectPO.Get() on new { ProjectPOId = poi.ProjectPOId, LocaleId = poi.LocaleId } equals new { ProjectPOId = p.Id, LocaleId = p.LocaleId }
                join c in Company.Get() on new { LocaleId = poi.LocaleId } equals new { LocaleId = c.Id }
                join v in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new
                {
                    Id = p.Id,
                    LocaleId = poi.LocaleId,
                    ProjectPOId = poi.ProjectPOId,
                    SeqNo = poi.SeqNo,
                    RefLocaleId = poi.RefLocaleId,
                    WorkOrderNo = poi.WorkOrderNo,
                    StyleNo = poi.StyleNo,
                    MaterialNameTw = poi.MaterialNameTw,
                    VendorId = poi.VendorId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    ReceivedLocaleId = poi.ReceivedLocaleId,
                    ReceivedConfirmed = poi.ReceivedConfirmed,
                    ReceivedDate = poi.ReceivedDate,
                    DollarNameTw = poi.DollarNameTw,
                    DoAP = poi.DoAP,
                    APMonth = poi.APMonth,
                    IsCFM = poi.IsCFM,

                    ProjectPODate = p.ProjectPODate,
                    Type = p.Type,
                    ProjectPONo = poi.ProjectPONo,
                    VendorNameTw = v.ShortNameTw,
                    ModifyUserName = poi.ModifyUserName,
                    Discount = poi.Discount,
                    Amount = poi.Amount,
                    APAmount = poi.APAmount,
                    CloseMonth = poi.APMonth,
                }
            )
            .Where(i => i.ReceivedConfirmed > 0)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(g => new { g.VendorId, g.VendorNameTw, g.LocaleId, g.PaymentLocaleId, g.Type, g.DollarNameTw })
            .Select(i => new Models.Views.RDPayableForVendor
            {
                // Id = i.Id,
                LocaleId = i.Key.LocaleId,
                Type = i.Key.Type,
                VendorId = i.Key.VendorId,
                VendorNameTw = i.Key.VendorNameTw,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                SubAmount = i.Sum(g => g.Amount),
                SubAPAmount = i.Sum(g => g.APAmount),
                DollarNameTw = i.Key.DollarNameTw,
                SubDiscount = i.Sum(g => g.Discount),
                CloseMonthFrom = i.Min(g => g.CloseMonth),
                CloseMonthTo = i.Max(g => g.CloseMonth)
            })
            .Distinct()
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.RDPayableItemForVendor> GetRDMonthAPItemForRecd(string predicate)
        {
            var result = (
                from poi in ProjectPOItem.Get()
                join p in ProjectPO.Get() on new { ProjectPOId = poi.ProjectPOId, LocaleId = poi.LocaleId } equals new { ProjectPOId = p.Id, LocaleId = p.LocaleId }
                join c in Company.Get() on new { LocaleId = poi.LocaleId } equals new { LocaleId = c.Id }
                join v in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.RDPayableItemForVendor
                {
                    Id = poi.Id,
                    LocaleId = poi.LocaleId,
                    ProjectPOId = poi.ProjectPOId,
                    SeqNo = poi.SeqNo,
                    RefLocaleId = poi.RefLocaleId,
                    WorkOrderNo = poi.WorkOrderNo,
                    StyleNo = poi.StyleNo,
                    DevPairs = poi.DevPairs,
                    PlanPairs = poi.PlanPairs,
                    MaterialNameTw = poi.MaterialNameTw,
                    VendorId = poi.VendorId,
                    PlanQty = poi.PlanQty,
                    UnitNameTw = poi.UnitNameTw,
                    PayCodeId = poi.PayCodeId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    Remark = poi.Remark,
                    FirstProjectPODate = poi.FirstProjectPODate,
                    ModifyUserName = poi.ModifyUserName,
                    LastUpdateTime = poi.LastUpdateTime,
                    VendorETD = poi.VendorETD,
                    ReceivedLocaleId = poi.ReceivedLocaleId,
                    ReceivedDate = poi.ReceivedDate,
                    ReceivedQty = poi.ReceivedQty,
                    ReceivedConfirmed = poi.ReceivedConfirmed,
                    RvdUserName = poi.RvdUserName,
                    RvdUpdateTime = poi.RvdUpdateTime,
                    QuotUnitPrice = poi.QuotUnitPrice,
                    PayUnitPrice = poi.PayUnitPrice,
                    ExtraAmount = poi.ExtraAmount,
                    DollarNameTw = poi.DollarNameTw,
                    Amount = poi.Amount,
                    PayQty = poi.PayQty,
                    Discount = poi.Discount,
                    APAmount = poi.APAmount,
                    DoAP = poi.DoAP,
                    APMonth = poi.APMonth,
                    ShoeName = poi.ShoeName,
                    CFMUserName = poi.CFMUserName,
                    CFMTime = poi.CFMTime,
                    IsCFM = poi.IsCFM,
                    DiscountRate = poi.DiscountRate,

                    ProjectPODate = p.ProjectPODate,
                    Type = p.Type,

                    ProjectPONo = poi.ProjectPONo,
                    VendorNameTw = v.ShortNameTw,
                    CloseMonth = poi.APMonth,
                }
            )
            .Where(i => i.ReceivedConfirmed > 0)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.RDPayableForVendor> GetRDMonthAPForPay(string predicate, string[] filters)
        {
            var result = (
                from poi in ProjectPOItem.Get()
                join p in ProjectPO.Get() on new { ProjectPOId = poi.ProjectPOId, LocaleId = poi.LocaleId } equals new { ProjectPOId = p.Id, LocaleId = p.LocaleId }
                join c in Company.Get() on new { LocaleId = poi.LocaleId } equals new { LocaleId = c.Id }
                join v in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new
                {
                    Id = p.Id,
                    LocaleId = poi.LocaleId,
                    ProjectPOId = poi.ProjectPOId,
                    SeqNo = poi.SeqNo,
                    RefLocaleId = poi.RefLocaleId,
                    WorkOrderNo = poi.WorkOrderNo,
                    StyleNo = poi.StyleNo,
                    MaterialNameTw = poi.MaterialNameTw,
                    VendorId = poi.VendorId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    ReceivedLocaleId = poi.ReceivedLocaleId,
                    ReceivedConfirmed = poi.ReceivedConfirmed,
                    ReceivedDate = poi.ReceivedDate,
                    DollarNameTw = poi.DollarNameTw,
                    DoAP = poi.DoAP,
                    APMonth = poi.APMonth,
                    IsCFM = poi.IsCFM,

                    ProjectPODate = p.ProjectPODate,
                    Type = p.Type,
                    ProjectPONo = poi.ProjectPONo,
                    VendorNameTw = v.ShortNameTw,
                    ModifyUserName = poi.ModifyUserName,
                    Discount = poi.Discount,
                    Amount = poi.Amount,
                    APAmount = poi.APAmount,
                    CloseMonth = poi.APMonth,
                }
            )
            .Where(i => i.ReceivedConfirmed > 0 && i.DoAP == 1)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(g => new { g.VendorId, g.VendorNameTw, g.LocaleId, g.PaymentLocaleId, g.Type, g.DollarNameTw })
            .Select(i => new Models.Views.RDPayableForVendor
            {
                // Id = i.Id,
                LocaleId = i.Key.LocaleId,
                Type = i.Key.Type,
                VendorId = i.Key.VendorId,
                VendorNameTw = i.Key.VendorNameTw,
                PaymentLocaleId = i.Key.PaymentLocaleId,
                SubAmount = i.Sum(g => g.Amount),
                SubAPAmount = i.Sum(g => g.APAmount),
                DollarNameTw = i.Key.DollarNameTw,
                SubDiscount = i.Sum(g => g.Discount),
                CloseMonthFrom = i.Min(g => g.CloseMonth),
                CloseMonthTo = i.Max(g => g.CloseMonth)
            })
            .Distinct()
            .ToList();

            return result.AsQueryable();
        }

        public IQueryable<Models.Views.RDPayableItemForVendor> GetRDMonthAPItemForPay(string predicate)
        {
            var result = (
                from poi in ProjectPOItem.Get()
                join p in ProjectPO.Get() on new { ProjectPOId = poi.ProjectPOId, LocaleId = poi.LocaleId } equals new { ProjectPOId = p.Id, LocaleId = p.LocaleId }
                join c in Company.Get() on new { LocaleId = poi.LocaleId } equals new { LocaleId = c.Id }
                join v in Vendor.Get() on new { VendorId = poi.VendorId, LocaleId = poi.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.RDPayableItemForVendor
                {
                    Id = poi.Id,
                    LocaleId = poi.LocaleId,
                    ProjectPOId = poi.ProjectPOId,
                    SeqNo = poi.SeqNo,
                    RefLocaleId = poi.RefLocaleId,
                    WorkOrderNo = poi.WorkOrderNo,
                    StyleNo = poi.StyleNo,
                    DevPairs = poi.DevPairs,
                    PlanPairs = poi.PlanPairs,
                    MaterialNameTw = poi.MaterialNameTw,
                    VendorId = poi.VendorId,
                    PlanQty = poi.PlanQty,
                    UnitNameTw = poi.UnitNameTw,
                    PayCodeId = poi.PayCodeId,
                    PaymentLocaleId = poi.PaymentLocaleId,
                    Remark = poi.Remark,
                    FirstProjectPODate = poi.FirstProjectPODate,
                    ModifyUserName = poi.ModifyUserName,
                    LastUpdateTime = poi.LastUpdateTime,
                    VendorETD = poi.VendorETD,
                    ReceivedLocaleId = poi.ReceivedLocaleId,
                    ReceivedDate = poi.ReceivedDate,
                    ReceivedQty = poi.ReceivedQty,
                    ReceivedConfirmed = poi.ReceivedConfirmed,
                    RvdUserName = poi.RvdUserName,
                    RvdUpdateTime = poi.RvdUpdateTime,
                    QuotUnitPrice = poi.QuotUnitPrice,
                    PayUnitPrice = poi.PayUnitPrice,
                    ExtraAmount = poi.ExtraAmount,
                    DollarNameTw = poi.DollarNameTw,
                    Amount = poi.Amount,
                    PayQty = poi.PayQty,
                    Discount = poi.Discount,
                    APAmount = poi.APAmount,
                    DoAP = poi.DoAP,
                    APMonth = poi.APMonth,
                    ShoeName = poi.ShoeName,
                    CFMUserName = poi.CFMUserName,
                    CFMTime = poi.CFMTime,
                    IsCFM = poi.IsCFM,
                    DiscountRate = poi.DiscountRate,

                    ProjectPODate = p.ProjectPODate,
                    Type = p.Type,

                    ProjectPONo = poi.ProjectPONo,
                    VendorNameTw = v.ShortNameTw,
                    CloseMonth = poi.APMonth,
                }
            )
            .Where(i => i.ReceivedConfirmed > 0 && i.DoAP == 1)
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();

            return result.AsQueryable();
        }
    }
}