using System.Linq;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class BOMRepOrdersBKService : BusinessService
    {
        private Services.Entities.BOMRepOrdersBKService BOMRepOrdersBK { get; }
        public BOMRepOrdersBKService(Services.Entities.BOMRepOrdersBKService bomRepOrdersBKService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.BOMRepOrdersBK = bomRepOrdersBKService;
        }
        public IQueryable<ERP.Models.Views.View.BOMOrders> Get()
        {
            return BOMRepOrdersBK.Get().Select(i => new ERP.Models.Views.View.BOMOrders
            {
                ColorCode = i.ColorCode,
                StyleNo = i.StyleNo,
                ProcessNoteTW = i.ProcessNoteTW,
                ColorDesc = i.ColorDesc,
                OutsoleColorDescTW = i.OutsoleColorDescTW,
                OutsoleColorDescEN = i.OutsoleColorDescEN,
                OutsoleId = i.OutsoleId,
                ShellId = i.ShellId,
                FinishGoodsPhotoURL = i.FinishGoodsPhotoURL,
                MoldNo = i.MoldNo,
                LastId = i.LastId,
                KnifeId = i.KnifeId,
                KnifeNo = i.KnifeNo,
                OutsoleNo = i.OutsoleNo,
                LastNo = i.LastNo,
                ShellNo = i.ShellNo,
                BrandCodeId = i.BrandCodeId,
                SizeCountryCodeId = i.SizeCountryCodeId,
                Id = i.Id,
                OrderDate = i.OrderDate,
                OrderNo = i.OrderNo,
                CustomerId = i.CustomerId,
                ArticleId = i.ArticleId,
                StyleId = i.StyleId,
                OrderType = i.OrderType,
                OrderTypeEn = i.OrderTypeEn,
                ProductType = i.ProductType,
                ProductTypeEn = i.ProductTypeEn,
                UnitPrice = i.UnitPrice,
                ReferUnitPrice = i.ReferUnitPrice,
                ETD = i.ETD,
                ShippingDate = i.ShippingDate,
                CompanyId = i.CompanyId,
                CompanyNo = i.CompanyNo,
                ChineseName = i.ChineseName,
                EnglishName = i.EnglishName,
                OrderSizeCountryCodeId = i.OrderSizeCountryCodeId,
                PackingDescTW = i.PackingDescTW,
                PackingDescEng = i.PackingDescEng,
                SafeCode = i.SafeCode,
                BarcodeCodeId = i.BarcodeCodeId,
                Mark = i.Mark,
                SideMark = i.SideMark,
                CustomerOrderNo = i.CustomerOrderNo,
                LocaleId = i.LocaleId,
                Status = i.Status,
                CSD = i.CSD,
                OrderQty = i.OrderQty,
                // PCLQty = i.PCLQty,
                PackingType = i.PackingType,
                PackingTypeEn = i.PackingTypeEn,
                Mark1Desc = i.Mark1Desc,
                Mark1PhotoURL = i.Mark1PhotoURL,
                Mark2Desc = i.Mark2Desc,
                Mark2PhotoURL = i.Mark2PhotoURL,
                Mark3Desc = i.Mark3Desc,
                Mark3PhotoURL = i.Mark3PhotoURL,
                Mark4Desc = i.Mark4Desc,
                Mark4PhotoURL = i.Mark4PhotoURL,
                Mark5Desc = i.Mark5Desc,
                Mark5PhotoURL = i.Mark5PhotoURL,
                DollarCodeId = i.DollarCodeId,
                doMRP = i.doMRP,
                Version = i.Version,
                ProcessSetId = i.ProcessSetId,
                ExportPortId = i.ExportPortId,
                InsockLabel = i.InsockLabel,
                PackingTypeDesc = i.PackingTypeDesc,
                CustomerStyleNo = i.CustomerStyleNo,
                ShoeName = i.ShoeName,
                IsApproved = i.IsApproved,
                ProcessNoteEng = i.ProcessNoteEng,
                BrandName = i.BrandName,
                MappingSizeCountry = i.MappingSizeCountry,
                OrdersCompanyNo = i.OrdersCompanyNo,
                PortName = i.PortName,
                PortNameEng = i.PortNameEng,
                StyleSizeCountry = i.StyleSizeCountry,
                SpecialNote = i.SpecialNote,
                CustomerNameTw = i.CustomerNameTw,
                CustomerNameEn = i.CustomerNameEn,
                BarcodeNameTw = i.BarcodeNameTw,
                BarcodeNameEn = i.BarcodeNameEn,
                DeliveryTerms = i.DeliveryTerms,
                TransitTypeDesc = i.TransitTypeDesc,
                TransitTypeDescEn = i.TransitTypeDescEn,
                OrdersVersion = i.OrdersVersion,
                StyleVersion = i.StyleVersion,
                PCLLastUpdateTime = i.PCLLastUpdateTime,
                PCLModifyUserName = i.PCLModifyUserName,
                // Gender = i.Gender,
                GBSPOReferenceNo = i.GBSPOReferenceNo
            });
        }
    }
}