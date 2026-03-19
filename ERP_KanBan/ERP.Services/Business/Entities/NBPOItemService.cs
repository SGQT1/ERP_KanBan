using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;

namespace ERP.Services.Business.Entities
{
    public class NBPOItemService : BusinessService
    {
        private Services.Entities.NBPOItemService NBPOItem { get; }

        public NBPOItemService(
            Services.Entities.NBPOItemService nbPOItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            NBPOItem = nbPOItemService;
        }
        public IQueryable<Models.Views.NBPOItem> Get()
        {
            return NBPOItem.Get().Select(i => new Models.Views.NBPOItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                POItemId = i.POItemId,
                POItemLocaleId = i.POItemLocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                PONo = i.PONo,
                Status = i.Status,
                POStatus = i.POStatus,
                POType = i.POType,
                OrderType = i.OrderType,
                PODate = i.PODate,
                Season = i.Season,
                FreightTerms = i.FreightTerms,
                PaymentTerms = i.PaymentTerms,
                PaymentMethod = i.PaymentMethod,
                Via = i.Via,
                BuyerCode = i.BuyerCode,
                BuyerName = i.BuyerName,
                VendorCode = i.VendorCode,
                VendorName = i.VendorName,
                FactoryCode = i.FactoryCode,
                FactoryAddress1 = i.FactoryAddress1,
                FactoryAddress2 = i.FactoryAddress2,
                FactoryAddress3 = i.FactoryAddress3,
                FactoryAddress4 = i.FactoryAddress4,
                FactoryPhone = i.FactoryPhone,
                FactoryFax = i.FactoryFax,
                BillToCompanyName = i.BillToCompanyName,
                BillToAddress1 = i.BillToAddress1,
                BillToAddress2 = i.BillToAddress2,
                BillToAddress3 = i.BillToAddress3,
                BillToAddress4 = i.BillToAddress4,
                BillToPhone = i.BillToPhone,
                BillToFax = i.BillToFax,
                ConsigneeCompanyName = i.ConsigneeCompanyName,
                ConsigneeAddress1 = i.ConsigneeAddress1,
                ConsigneeAddress2 = i.ConsigneeAddress2,
                ConsigneeAddress3 = i.ConsigneeAddress3,
                ConsigneeAddress4 = i.ConsigneeAddress4,
                ConsigneePhone = i.ConsigneePhone,
                ConsigneeFax = i.ConsigneeFax,
                AgentCompanyName = i.AgentCompanyName,
                AgentAddress1 = i.AgentAddress1,
                AgentAddress2 = i.AgentAddress2,
                AgentAddress3 = i.AgentAddress3,
                AgentAddress4 = i.AgentAddress4,
                AgentPhone = i.AgentPhone,
                AgentFax = i.AgentFax,
                AgentAttn = i.AgentAttn,
                ShipToCompanyName = i.ShipToCompanyName,
                ShipToAddress1 = i.ShipToAddress1,
                ShipToAddress2 = i.ShipToAddress2,
                ShipToAddress3 = i.ShipToAddress3,
                ShipToAddress4 = i.ShipToAddress4,
                ShipToPhone = i.ShipToPhone,
                ShipToFax = i.ShipToFax,
                ShipFromLocation = i.ShipFromLocation,
                NotifyPartyCompanyName = i.NotifyPartyCompanyName,
                NotifyPartyAddress1 = i.NotifyPartyAddress1,
                NotifyPartyAddress2 = i.NotifyPartyAddress2,
                NotifyPartyAddress3 = i.NotifyPartyAddress3,
                NotifyPartyAddress4 = i.NotifyPartyAddress4,
                NotifyPartyPhone = i.NotifyPartyPhone,
                NotifyPartyFax = i.NotifyPartyFax,
                NotifyPartyAttn = i.NotifyPartyAttn,
                NBPartNo = i.NBPartNo,
                NBColorKey = i.NBColorKey,
                SupplierLineItemNo = i.SupplierLineItemNo,
                SupplierMaterialNo = i.SupplierMaterialNo,
                NBMaterialDescription = i.NBMaterialDescription,
                SupplierMaterialDescription = i.SupplierMaterialDescription,
                UOM = i.UOM,
                Quantity = i.Quantity,
                Length = i.Length,
                RXFD = i.RXFD,
                RETA = i.RETA,
                ShipMode = i.ShipMode,
                PackType = i.PackType,
                SpecialTreatment = i.SpecialTreatment,
                AdditionInstruction = i.AdditionInstruction,
                StyleNo = i.StyleNo,
                CustomerPONo = i.CustomerPONo,
                ReceiptQuantity = i.ReceiptQuantity,
                ReceiptDate = i.ReceiptDate,
                ReceiptQuantityQC = i.ReceiptQuantityQC,
                RejectedQuantity = i.RejectedQuantity,
                RejectQuantityForColorIssues = i.RejectQuantityForColorIssues,
                RejectQuantityForCosmeticIssues = i.RejectQuantityForCosmeticIssues,
                ResolvedQuantity = i.ResolvedQuantity,
                ResolvedDate = i.ResolvedDate,
                FactoryPOPrintFileName = i.FactoryPOPrintFileName,
                SupplieRemark = i.SupplieRemark,
                FactoryRemark = i.FactoryRemark,
            });
        }

        public void CreateRange(IEnumerable<Models.Views.NBPOItem> items)
        {
            NBPOItem.CreateRange(StockOutBuildRange(items));
        }
        public void RemoveRange(List<decimal> items)
        {
            NBPOItem.RemoveRange(i => items.Contains(i.Id));
        }
        private IEnumerable<Models.Entities.NBPOItem> StockOutBuildRange(IEnumerable<Models.Views.NBPOItem> items)
        {
            return items.Select(item => new Models.Entities.NBPOItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                POItemId = item.POItemId,
                POItemLocaleId = item.POItemLocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PONo = item.PONo,
                Status = item.Status,
                POStatus = item.POStatus,
                POType = item.POType,
                OrderType = item.OrderType,
                PODate = item.PODate,
                Season = item.Season,
                FreightTerms = item.FreightTerms,
                PaymentTerms = item.PaymentTerms,
                PaymentMethod = item.PaymentMethod,
                Via = item.Via,
                BuyerCode = item.BuyerCode,
                BuyerName = item.BuyerName,
                VendorCode = item.VendorCode,
                VendorName = item.VendorName,
                FactoryCode = item.FactoryCode,
                FactoryAddress1 = item.FactoryAddress1,
                FactoryAddress2 = item.FactoryAddress2,
                FactoryAddress3 = item.FactoryAddress3,
                FactoryAddress4 = item.FactoryAddress4,
                FactoryPhone = item.FactoryPhone,
                FactoryFax = item.FactoryFax,
                BillToCompanyName = item.BillToCompanyName,
                BillToAddress1 = item.BillToAddress1,
                BillToAddress2 = item.BillToAddress2,
                BillToAddress3 = item.BillToAddress3,
                BillToAddress4 = item.BillToAddress4,
                BillToPhone = item.BillToPhone,
                BillToFax = item.BillToFax,
                ConsigneeCompanyName = item.ConsigneeCompanyName,
                ConsigneeAddress1 = item.ConsigneeAddress1,
                ConsigneeAddress2 = item.ConsigneeAddress2,
                ConsigneeAddress3 = item.ConsigneeAddress3,
                ConsigneeAddress4 = item.ConsigneeAddress4,
                ConsigneePhone = item.ConsigneePhone,
                ConsigneeFax = item.ConsigneeFax,
                AgentCompanyName = item.AgentCompanyName,
                AgentAddress1 = item.AgentAddress1,
                AgentAddress2 = item.AgentAddress2,
                AgentAddress3 = item.AgentAddress3,
                AgentAddress4 = item.AgentAddress4,
                AgentPhone = item.AgentPhone,
                AgentFax = item.AgentFax,
                AgentAttn = item.AgentAttn,
                ShipToCompanyName = item.ShipToCompanyName,
                ShipToAddress1 = item.ShipToAddress1,
                ShipToAddress2 = item.ShipToAddress2,
                ShipToAddress3 = item.ShipToAddress3,
                ShipToAddress4 = item.ShipToAddress4,
                ShipToPhone = item.ShipToPhone,
                ShipToFax = item.ShipToFax,
                ShipFromLocation = item.ShipFromLocation,
                NotifyPartyCompanyName = item.NotifyPartyCompanyName,
                NotifyPartyAddress1 = item.NotifyPartyAddress1,
                NotifyPartyAddress2 = item.NotifyPartyAddress2,
                NotifyPartyAddress3 = item.NotifyPartyAddress3,
                NotifyPartyAddress4 = item.NotifyPartyAddress4,
                NotifyPartyPhone = item.NotifyPartyPhone,
                NotifyPartyFax = item.NotifyPartyFax,
                NotifyPartyAttn = item.NotifyPartyAttn,
                NBPartNo = item.NBPartNo,
                NBColorKey = item.NBColorKey,
                SupplierLineItemNo = item.SupplierLineItemNo,
                SupplierMaterialNo = item.SupplierMaterialNo,
                NBMaterialDescription = item.NBMaterialDescription,
                SupplierMaterialDescription = item.SupplierMaterialDescription,
                UOM = item.UOM,
                Quantity = item.Quantity,
                Length = item.Length,
                RXFD = item.RXFD,
                RETA = item.RETA,
                ShipMode = item.ShipMode,
                PackType = item.PackType,
                SpecialTreatment = item.SpecialTreatment,
                AdditionInstruction = item.AdditionInstruction,
                StyleNo = item.StyleNo,
                CustomerPONo = item.CustomerPONo,
                ReceiptQuantity = item.ReceiptQuantity,
                ReceiptDate = item.ReceiptDate,
                ReceiptQuantityQC = item.ReceiptQuantityQC,
                RejectedQuantity = item.RejectedQuantity,
                RejectQuantityForColorIssues = item.RejectQuantityForColorIssues,
                RejectQuantityForCosmeticIssues = item.RejectQuantityForCosmeticIssues,
                ResolvedQuantity = item.ResolvedQuantity,
                ResolvedDate = item.ResolvedDate,
                FactoryPOPrintFileName = item.FactoryPOPrintFileName,
                SupplieRemark = item.SupplieRemark,
                FactoryRemark = item.FactoryRemark,
            });
        }

    }
}