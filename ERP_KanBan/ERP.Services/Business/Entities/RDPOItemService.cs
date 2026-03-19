using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class RDPOItemService : BusinessService
    {
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.ProjectPOService ProjectPO { get; set; }
        private ERP.Services.Entities.ProjectPOItemService ProjectPOItem { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }

        public RDPOItemService(
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.ProjectPOService projectPOService,
            ERP.Services.Entities.ProjectPOItemService projectPOItemService,
            ERP.Services.Entities.VendorService vendorService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Company = companyService;
            ProjectPO = projectPOService;
            ProjectPOItem = projectPOItemService;
            Vendor = vendorService;
        }
        public IQueryable<Models.Views.RDPOItem> Get()
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
                    VendorNameTw = v.NameTw,
                    Brand = poi.Brand,
                    CloseMonth = poi.CloseMonth,
                }
            );
            return result;
        }

        public IQueryable<Models.Views.RDPOItem> GetRDPOForPrint(string predicate, string[] filters)
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
                    Brand = poi.Brand,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();
            return result.AsQueryable();
        }

        public void CreateRange(IEnumerable<Models.Views.RDPOItem> items)
        {
            ProjectPOItem.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.ProjectPOItem, bool>> predicate)
        {
            ProjectPOItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.ProjectPOItem> BuildRange(IEnumerable<Models.Views.RDPOItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.ProjectPOItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ProjectPOId = item.ProjectPOId,
                SeqNo = item.SeqNo,
                RefLocaleId = item.RefLocaleId,
                WorkOrderNo = item.WorkOrderNo,
                StyleNo = item.StyleNo,
                DevPairs = item.DevPairs,
                PlanPairs = item.PlanPairs,
                MaterialNameTw = item.MaterialNameTw,
                VendorId = item.VendorId,
                PlanQty = item.PlanQty,
                UnitNameTw = item.UnitNameTw,
                PayCodeId = item.PayCodeId,
                PaymentLocaleId = item.PaymentLocaleId,
                Remark = item.Remark,
                FirstProjectPODate = item.FirstProjectPODate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                VendorETD = item.VendorETD,
                ReceivedLocaleId = item.ReceivedLocaleId,
                ReceivedDate = item.ReceivedDate,
                ReceivedQty = item.ReceivedQty,
                ReceivedConfirmed = item.ReceivedConfirmed,
                RvdUserName = item.RvdUserName,
                RvdUpdateTime = item.RvdUpdateTime,
                QuotUnitPrice = item.QuotUnitPrice,
                PayUnitPrice = item.PayUnitPrice,
                ExtraAmount = item.ExtraAmount,
                DollarNameTw = item.DollarNameTw,
                Amount = item.Amount,
                PayQty = item.PayQty,
                Discount = item.Discount,
                APAmount = item.APAmount,
                DoAP = item.DoAP,
                APMonth = item.APMonth,
                ShoeName = item.ShoeName,
                CFMUserName = item.CFMUserName,
                CFMTime = item.CFMTime,
                IsCFM = item.IsCFM,
                DiscountRate = item.DiscountRate,

                ProjectPONo = item.ProjectPONo,
                Brand = item.Brand,
                CloseMonth = item.CloseMonth,
            });
        }

    }
}