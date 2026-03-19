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
    public class RDPOItemLogService : BusinessService
    {
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.ProjectPOService ProjectPO { get; set; }
        private ERP.Services.Entities.ProjectPOItemService ProjectPOItem { get; set; }
        private ERP.Services.Entities.ProjectPOItemLogService ProjectPOItemLog { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }

        public RDPOItemLogService(
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.ProjectPOService projectPOService,
            ERP.Services.Entities.ProjectPOItemService projectPOItemService,
            ERP.Services.Entities.ProjectPOItemLogService projectPOItemLogService,
            ERP.Services.Entities.VendorService vendorService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Company = companyService;
            ProjectPO = projectPOService;
            ProjectPOItem = projectPOItemService;
            ProjectPOItemLog = projectPOItemLogService;
            Vendor = vendorService;
        }
        public IQueryable<Models.Views.RDPOItemLog> Get()
        {
            var result = ProjectPOItemLog.Get().Select(i => new Models.Views.RDPOItemLog
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                ProjectPOId = i.ProjectPOId,
                ProjectPOItemId = i.ProjectPOItemId,
                ReceivedDate = i.ReceivedDate,
                VendorId = i.VendorId,
                MaterialName = i.MaterialName,
                UnitNameTw = i.UnitNameTw,
                ReceivedQty = i.ReceivedQty,
                PayUnitPrice = i.PayUnitPrice,
                DollarNameTw = i.DollarNameTw,
                DeliveryOrderNo = i.DeliveryOrderNo,
                WarehouseNo = i.WarehouseNo,
                LocationDesc = i.LocationDesc,
                BankingRate = i.BankingRate,
                APMonth = i.APMonth,
                Barcode = i.Barcode,
            });
            return result;
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