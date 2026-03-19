using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using Newtonsoft.Json;

namespace ERP.Services.Search
{
    public class NBPPMService : SearchService
    {
        private Services.Entities.NBMaterialService NBMaterial { get; }
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.StyleItemService StyleItem { get; }
        private Services.Entities.MaterialService Material { get; }

        private Services.Entities.NBSCCReportService NBSCCReport { get; }
        private Services.Entities.NBPOItemService NBPOItem { get; }
        private Services.Entities.POItemService POItem { get; }
        private Services.Entities.POService PO { get; }
        private Services.Entities.CodeItemService CodeItem { get; set; }
        private Services.Entities.VendorService Vendor { get; set; }
        private Services.Entities.ReceivedLogService ReceivedLog1 { get; set; }
        private Services.Entities.ReceivedLogService ReceivedLog2 { get; set; }
        private Services.Entities.ReceivedLogService ReceivedLog3 { get; set; }
        private Services.Entities.TransferService Transfer { get; set; }
        private Services.Entities.TransferItemService TransferItem { get; set; }

        public NBPPMService(
            Services.Entities.NBMaterialService nbMaterialService,
            Services.Entities.OrdersService ordersService,
            Services.Entities.StyleItemService styleItemService,
            Services.Entities.MaterialService materialService,
            Services.Entities.NBSCCReportService nbSCCReportService,
            Services.Entities.NBPOItemService nbPOItemservice,
            Services.Entities.POItemService poItemService,
            Services.Entities.POService poService,
            Services.Entities.CodeItemService codeItemServcie,
            Services.Entities.VendorService vendorService,
            Services.Entities.ReceivedLogService receivedLogService1,
            Services.Entities.ReceivedLogService receivedLogService2,
            Services.Entities.ReceivedLogService receivedLogService3,
            Services.Entities.TransferService transferService,
            Services.Entities.TransferItemService transferItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            NBMaterial = nbMaterialService;
            Orders = ordersService;
            StyleItem = styleItemService;
            Material = materialService;

            NBSCCReport = nbSCCReportService;
            NBPOItem = nbPOItemservice;
            POItem = poItemService;
            PO = poService;
            CodeItem = codeItemServcie;
            Vendor = vendorService;

            ReceivedLog1 = receivedLogService1;
            ReceivedLog2 = receivedLogService2;
            ReceivedLog3 = receivedLogService3;
            Transfer = transferService;
            TransferItem = transferItemService;
        }

        public IQueryable<Models.Views.NBMaterial> GetWithOrders(string predicate)
        {
            var result = (
                from o in Orders.Get()
                join si in StyleItem.Get() on new { StyleId = o.StyleId, LocaleId = o.LocaleId } equals new { StyleId = si.StyleId, LocaleId = si.LocaleId }
                join m in Material.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                join n in NBMaterial.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = n.MaterialId, LocaleId = n.LocaleId } into nGRP
                from n in nGRP.DefaultIfEmpty()
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    CSD = o.CSD,
                    BrandCodeId = o.BarcodeCodeId,
                    MaterialId = m.Id,
                    MaterialName = m.MaterialName,
                    MaterialNameEng = m.MaterialNameEng,
                    MaterialCode = n.MaterialCode,
                    ColorKey = n.ColorKey,
                    Description = n.Description,
                    VendorCode = n.VendorCode,
                    VendorName = n.VendorName,
                    UOM = n.UOM,
                    CommodityType = n.CommodityType,
                    NBColorName = n.NBColorName,
                    ColorFamily = n.ColorFamily,
                    ModifyUserName = n.ModifyUserName,
                    LastUpdateTime = n.LastUpdateTime,
                    Brand = o.Brand,
                    NBMaterialId = n.Id,
                    StyleNo = o.StyleNo,
                    OrderNo = o.OrderNo,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.NBMaterial
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                MaterialId = i.MaterialId,
                MaterialCode = i.MaterialCode,
                CommodityType = i.CommodityType,
                Description = i.Description,
                UOM = i.UOM,
                ColorKey = i.ColorKey,
                NBColorName = i.NBColorName,
                ColorFamily = i.ColorFamily,
                VendorName = i.VendorName,
                VendorCode = i.VendorCode,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
            })
            .Distinct();
            return result;
        }

        public IQueryable<Models.Views.NBSCCReport> GetNBSCCReport(string predicate, string[] filters)
        {
            var groupBy = false;
            if (filters != null && filters.Length > 0)
            {
                var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                groupBy = (bool)extenFilters.Field9;
            }

            var result = (
                from r in NBPOItem.Get()
                join pi in POItem.Get().Where(p => p.Status != 2) on new { POItemId = r.POItemId, POItemLocaleId = r.POItemLocaleId } equals new { POItemId = (decimal?)pi.Id, POItemLocaleId = (decimal?)pi.LocaleId }
                join po in PO.Get() on new { POId = pi.POId, LocaleId = pi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                join m in Material.Get() on new { MId = pi.MaterialId, LocaleId = pi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId }
                select new
                {
                    Id = r.Id,
                    LocaleId = r.LocaleId,
                    POItemId = r.POItemId,
                    POItemLocaleId = r.POItemLocaleId,
                    ModifyUserName = r.ModifyUserName,
                    LastUpdateTime = r.LastUpdateTime,
                    PONo = r.PONo,
                    Status = r.Status,
                    POStatus = r.POStatus,
                    POType = r.POType,
                    OrderType = r.OrderType,
                    PDate = Convert.ToInt32(r.PODate.Substring(6, 4) + r.PODate.Substring(3, 2) + r.PODate.Substring(0, 2)),
                    PODate = r.PODate,
                    Season = r.Season,
                    FreightTerms = r.FreightTerms,
                    PaymentTerms = r.PaymentTerms,
                    PaymentMethod = r.PaymentMethod,
                    Via = r.Via,
                    BuyerCode = r.BuyerCode,
                    BuyerName = r.BuyerName,
                    VendorCode = r.VendorCode,
                    VendorName = r.VendorName,
                    FactoryCode = r.FactoryCode,
                    FactoryAddress1 = r.FactoryAddress1,
                    FactoryAddress2 = r.FactoryAddress2,
                    FactoryAddress3 = r.FactoryAddress3,
                    FactoryAddress4 = r.FactoryAddress4,
                    FactoryPhone = r.FactoryPhone,
                    FactoryFax = r.FactoryFax,
                    BillToCompanyName = r.BillToCompanyName,
                    BillToAddress1 = r.BillToAddress1,
                    BillToAddress2 = r.BillToAddress2,
                    BillToAddress3 = r.BillToAddress3,
                    BillToAddress4 = r.BillToAddress4,
                    BillToPhone = r.BillToPhone,
                    BillToFax = r.BillToFax,
                    ConsigneeCompanyName = r.ConsigneeCompanyName,
                    ConsigneeAddress1 = r.ConsigneeAddress1,
                    ConsigneeAddress2 = r.ConsigneeAddress2,
                    ConsigneeAddress3 = r.ConsigneeAddress3,
                    ConsigneeAddress4 = r.ConsigneeAddress4,
                    ConsigneePhone = r.ConsigneePhone,
                    ConsigneeFax = r.ConsigneeFax,
                    AgentCompanyName = r.AgentCompanyName,
                    AgentAddress1 = r.AgentAddress1,
                    AgentAddress2 = r.AgentAddress2,
                    AgentAddress3 = r.AgentAddress3,
                    AgentAddress4 = r.AgentAddress4,
                    AgentPhone = r.AgentPhone,
                    AgentFax = r.AgentFax,
                    AgentAttn = r.AgentAttn,
                    ShipToCompanyName = r.ShipToCompanyName,
                    ShipToAddress1 = r.ShipToAddress1,
                    ShipToAddress2 = r.ShipToAddress2,
                    ShipToAddress3 = r.ShipToAddress3,
                    ShipToAddress4 = r.ShipToAddress4,
                    ShipToPhone = r.ShipToPhone,
                    ShipToFax = r.ShipToFax,
                    ShipFromLocation = r.ShipFromLocation,
                    NotifyPartyCompanyName = r.NotifyPartyCompanyName,
                    NotifyPartyAddress1 = r.NotifyPartyAddress1,
                    NotifyPartyAddress2 = r.NotifyPartyAddress2,
                    NotifyPartyAddress3 = r.NotifyPartyAddress3,
                    NotifyPartyAddress4 = r.NotifyPartyAddress4,
                    NotifyPartyPhone = r.NotifyPartyPhone,
                    NotifyPartyFax = r.NotifyPartyFax,
                    NotifyPartyAttn = r.NotifyPartyAttn,
                    NBPartNo = r.NBPartNo,
                    NBColorKey = r.NBColorKey,
                    SupplierLineItemNo = r.SupplierLineItemNo,
                    SupplierMaterialNo = r.SupplierMaterialNo,
                    NBMaterialDescription = r.NBMaterialDescription,
                    SupplierMaterialDescription = r.SupplierMaterialDescription,
                    UOM = r.UOM,
                    Quantity = r.Quantity,
                    Length = r.Length,
                    RXFD = r.RXFD,
                    RETA = r.RETA,
                    ShipMode = r.ShipMode,
                    PackType = r.PackType,
                    SpecialTreatment = r.SpecialTreatment,
                    AdditionInstruction = r.AdditionInstruction,
                    StyleNo = r.StyleNo,
                    CustomerPONo = r.CustomerPONo,
                    ReceiptQuantity = r.ReceiptQuantity,
                    ReceiptDate = r.ReceiptDate,
                    ReceiptQuantityQC = r.ReceiptQuantityQC,
                    RejectedQuantity = r.RejectedQuantity,
                    RejectQuantityForColorIssues = r.RejectQuantityForColorIssues,
                    RejectQuantityForCosmeticIssues = r.RejectQuantityForCosmeticIssues,
                    ResolvedQuantity = r.ResolvedQuantity,
                    ResolvedDate = r.ResolvedDate,
                    FactoryPOPrintFileName = r.FactoryPOPrintFileName,
                    SupplieRemark = r.SupplieRemark,
                    FactoryRemark = r.FactoryRemark,
                    VendorId = po.VendorId,
                    MaterialName = m.MaterialNameEng,
                    MaterialId = pi.MaterialId
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.NBSCCReport
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
            })
            .Distinct()
            .ToList();

            result.ForEach(i =>
            {
                i.ReceiptQuantity = i.ReceiptQuantity == 0 ? null : i.ReceiptQuantity;
                i.ReceiptQuantityQC = i.ReceiptQuantityQC == 0 ? null : i.ReceiptQuantityQC;
                i.RejectedQuantity = i.RejectedQuantity == 0 ? null : i.RejectedQuantity;
                i.RejectQuantityForColorIssues = i.RejectQuantityForColorIssues == 0 ? null : i.RejectQuantityForColorIssues;
                i.RejectQuantityForCosmeticIssues = i.RejectQuantityForCosmeticIssues == 0 ? null : i.RejectQuantityForCosmeticIssues;
                i.ResolvedQuantity = i.ResolvedQuantity == 0 ? null : i.ResolvedQuantity;
            });

            if (groupBy)
            {
                result = result
                .GroupBy(g => new
                {
                    // g.Id,
                    g.LocaleId,
                    // g.POItemId,
                    g.POItemLocaleId,
                    // g.ModifyUserName,
                    // g.LastUpdateTime,
                    g.PONo,
                    g.Status,
                    g.POStatus,
                    g.POType,
                    g.OrderType,
                    g.PODate,
                    g.Season,
                    g.FreightTerms,
                    g.PaymentTerms,
                    g.PaymentMethod,
                    g.Via,
                    g.BuyerCode,
                    g.BuyerName,
                    g.VendorCode,
                    g.VendorName,
                    g.FactoryCode,
                    g.FactoryAddress1,
                    g.FactoryAddress2,
                    g.FactoryAddress3,
                    g.FactoryAddress4,
                    g.FactoryPhone,
                    g.FactoryFax,
                    g.BillToCompanyName,
                    g.BillToAddress1,
                    g.BillToAddress2,
                    g.BillToAddress3,
                    g.BillToAddress4,
                    g.BillToPhone,
                    g.BillToFax,
                    g.ConsigneeCompanyName,
                    g.ConsigneeAddress1,
                    g.ConsigneeAddress2,
                    g.ConsigneeAddress3,
                    g.ConsigneeAddress4,
                    g.ConsigneePhone,
                    g.ConsigneeFax,
                    g.AgentCompanyName,
                    g.AgentAddress1,
                    g.AgentAddress2,
                    g.AgentAddress3,
                    g.AgentAddress4,
                    g.AgentPhone,
                    g.AgentFax,
                    g.AgentAttn,
                    g.ShipToCompanyName,
                    g.ShipToAddress1,
                    g.ShipToAddress2,
                    g.ShipToAddress3,
                    g.ShipToAddress4,
                    g.ShipToPhone,
                    g.ShipToFax,
                    g.ShipFromLocation,
                    g.NotifyPartyCompanyName,
                    g.NotifyPartyAddress1,
                    g.NotifyPartyAddress2,
                    g.NotifyPartyAddress3,
                    g.NotifyPartyAddress4,
                    g.NotifyPartyPhone,
                    g.NotifyPartyFax,
                    g.NotifyPartyAttn,
                    g.NBPartNo,
                    g.NBColorKey,
                    g.SupplierLineItemNo,
                    g.SupplierMaterialNo,
                    g.NBMaterialDescription,
                    g.SupplierMaterialDescription,
                    g.UOM,
                    // g.Quantity,
                    g.Length,
                    g.RXFD,
                    g.RETA,
                    g.ShipMode,
                    g.PackType,
                    g.SpecialTreatment,
                    g.AdditionInstruction,
                    g.StyleNo,
                    g.CustomerPONo,
                    g.ReceiptQuantity,
                    g.ReceiptDate,
                    g.ReceiptQuantityQC,
                    g.RejectedQuantity,
                    g.RejectQuantityForColorIssues,
                    g.RejectQuantityForCosmeticIssues,
                    g.ResolvedQuantity,
                    g.ResolvedDate,
                    g.FactoryPOPrintFileName,
                    g.SupplieRemark,
                    g.FactoryRemark,
                })
                .Select(i => new ERP.Models.Views.NBSCCReport
                {
                    Id = i.Max(g => g.Id),
                    LocaleId = i.Key.LocaleId,
                    POItemId = i.Max(g => g.POItemId),
                    POItemLocaleId = i.Key.POItemLocaleId,
                    ModifyUserName = i.Max(g => g.ModifyUserName), //i.Key.ModifyUserName,
                    LastUpdateTime = i.Max(g => g.LastUpdateTime), //i.Key.LastUpdateTime,
                    PONo = i.Key.PONo,
                    Status = i.Key.Status,
                    POStatus = i.Key.POStatus,
                    POType = i.Key.POType,
                    OrderType = i.Key.OrderType,
                    PODate = i.Key.PODate,
                    Season = i.Key.Season,
                    FreightTerms = i.Key.FreightTerms,
                    PaymentTerms = i.Key.PaymentTerms,
                    PaymentMethod = i.Key.PaymentMethod,
                    Via = i.Key.Via,
                    BuyerCode = i.Key.BuyerCode,
                    BuyerName = i.Key.BuyerName,
                    VendorCode = i.Key.VendorCode,
                    VendorName = i.Key.VendorName,
                    FactoryCode = i.Key.FactoryCode,
                    FactoryAddress1 = i.Key.FactoryAddress1,
                    FactoryAddress2 = i.Key.FactoryAddress2,
                    FactoryAddress3 = i.Key.FactoryAddress3,
                    FactoryAddress4 = i.Key.FactoryAddress4,
                    FactoryPhone = i.Key.FactoryPhone,
                    FactoryFax = i.Key.FactoryFax,
                    BillToCompanyName = i.Key.BillToCompanyName,
                    BillToAddress1 = i.Key.BillToAddress1,
                    BillToAddress2 = i.Key.BillToAddress2,
                    BillToAddress3 = i.Key.BillToAddress3,
                    BillToAddress4 = i.Key.BillToAddress4,
                    BillToPhone = i.Key.BillToPhone,
                    BillToFax = i.Key.BillToFax,
                    ConsigneeCompanyName = i.Key.ConsigneeCompanyName,
                    ConsigneeAddress1 = i.Key.ConsigneeAddress1,
                    ConsigneeAddress2 = i.Key.ConsigneeAddress2,
                    ConsigneeAddress3 = i.Key.ConsigneeAddress3,
                    ConsigneeAddress4 = i.Key.ConsigneeAddress4,
                    ConsigneePhone = i.Key.ConsigneePhone,
                    ConsigneeFax = i.Key.ConsigneeFax,
                    AgentCompanyName = i.Key.AgentCompanyName,
                    AgentAddress1 = i.Key.AgentAddress1,
                    AgentAddress2 = i.Key.AgentAddress2,
                    AgentAddress3 = i.Key.AgentAddress3,
                    AgentAddress4 = i.Key.AgentAddress4,
                    AgentPhone = i.Key.AgentPhone,
                    AgentFax = i.Key.AgentFax,
                    AgentAttn = i.Key.AgentAttn,
                    ShipToCompanyName = i.Key.ShipToCompanyName,
                    ShipToAddress1 = i.Key.ShipToAddress1,
                    ShipToAddress2 = i.Key.ShipToAddress2,
                    ShipToAddress3 = i.Key.ShipToAddress3,
                    ShipToAddress4 = i.Key.ShipToAddress4,
                    ShipToPhone = i.Key.ShipToPhone,
                    ShipToFax = i.Key.ShipToFax,
                    ShipFromLocation = i.Key.ShipFromLocation,
                    NotifyPartyCompanyName = i.Key.NotifyPartyCompanyName,
                    NotifyPartyAddress1 = i.Key.NotifyPartyAddress1,
                    NotifyPartyAddress2 = i.Key.NotifyPartyAddress2,
                    NotifyPartyAddress3 = i.Key.NotifyPartyAddress3,
                    NotifyPartyAddress4 = i.Key.NotifyPartyAddress4,
                    NotifyPartyPhone = i.Key.NotifyPartyPhone,
                    NotifyPartyFax = i.Key.NotifyPartyFax,
                    NotifyPartyAttn = i.Key.NotifyPartyAttn,
                    NBPartNo = i.Key.NBPartNo,
                    NBColorKey = i.Key.NBColorKey,
                    SupplierLineItemNo = i.Key.SupplierLineItemNo,
                    SupplierMaterialNo = i.Key.SupplierMaterialNo,
                    NBMaterialDescription = i.Key.NBMaterialDescription,
                    SupplierMaterialDescription = i.Key.SupplierMaterialDescription,
                    UOM = i.Key.UOM,
                    Quantity = i.Sum(g => g.Quantity),
                    Length = i.Key.Length,
                    RXFD = i.Key.RXFD,
                    RETA = i.Key.RETA,
                    ShipMode = i.Key.ShipMode,
                    PackType = i.Key.PackType,
                    SpecialTreatment = i.Key.SpecialTreatment,
                    AdditionInstruction = i.Key.AdditionInstruction,
                    StyleNo = i.Key.StyleNo,
                    CustomerPONo = i.Key.CustomerPONo,
                    ReceiptQuantity = i.Key.ReceiptQuantity,
                    ReceiptDate = i.Key.ReceiptDate,
                    ReceiptQuantityQC = i.Key.ReceiptQuantityQC,
                    RejectedQuantity = i.Key.RejectedQuantity,
                    RejectQuantityForColorIssues = i.Key.RejectQuantityForColorIssues,
                    RejectQuantityForCosmeticIssues = i.Key.RejectQuantityForCosmeticIssues,
                    ResolvedQuantity = i.Key.ResolvedQuantity,
                    ResolvedDate = i.Key.ResolvedDate,
                    FactoryPOPrintFileName = i.Key.FactoryPOPrintFileName,
                    SupplieRemark = i.Key.SupplieRemark,
                    FactoryRemark = i.Key.FactoryRemark,
                })
                .Distinct()
                .ToList();
            }
            return result.AsQueryable();
        }

        public IQueryable<Models.Views.NBOSRReport> GetNBOSRReport(string predicate)
        {
            var rlog1 = ReceivedLog1.Get().Where(i => i.TransferInId == 0)
                .GroupBy(g => new { g.POItemId, g.RefLocaleId, g.LocaleId })
                .Select(i => new
                {
                    MinReceivedDate = i.Min(g => g.ReceivedDate),
                    ReceivedQty = i.Sum(g => g.ReceivedQty),
                    POItemId = i.Key.POItemId,
                    RefLocaleId = i.Key.RefLocaleId,
                    LocaleId = i.Key.LocaleId
                });
            var rlog2 = ReceivedLog2.Get().Where(i => i.TransferInId > 0)
                .GroupBy(g => new { g.POItemId, g.RefLocaleId, g.LocaleId })
                .Select(i => new
                {
                    MinReceivedDate = i.Min(g => g.ReceivedDate),
                    ReceivedQty = i.Sum(g => g.ReceivedQty),
                    POItemId = i.Key.POItemId,
                    RefLocaleId = i.Key.RefLocaleId,
                    LocaleId = i.Key.LocaleId
                });
            var transfer = (
                from t in Transfer.Get()
                join ti in TransferItem.Get() on new { TransferId = t.Id, LocaleId = t.LocaleId } equals new { TransferId = ti.TransferId, LocaleId = ti.LocaleId }
                join r in ReceivedLog3.Get().Where(i => i.TransferInId == 0) on new { ReceivedLogId = ti.ReceivedLogId, LocaleId = ti.LocaleId } equals new { ReceivedLogId = r.Id, LocaleId = r.LocaleId }
                select new { t.OBDate, r.POItemId, r.RefLocaleId }
            )
            .GroupBy(g => new { g.POItemId, g.RefLocaleId })
            .Select(i => new { OBDate = i.Min(g => g.OBDate), POItemId = i.Key.POItemId, RefLocaleId = i.Key.RefLocaleId });

            var osrItem = (
                from poi in POItem.Get().Where(i => i.Status != 2)
                join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                join m in Material.Get() on new { MId = poi.MaterialId, LocaleId = poi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId }
                join v in Vendor.Get() on new { VendorId = po.VendorId, LocaleId = po.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                join o in Orders.Get() on new { OrderNo = poi.OrderNo } equals new { OrderNo = o.OrderNo } into oGRP
                from o in oGRP.DefaultIfEmpty()
                join r1 in rlog1 on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = r1.POItemId, LocaleId = r1.RefLocaleId } into r1GRP
                from r1 in r1GRP.DefaultIfEmpty()
                join r2 in rlog2 on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = r2.POItemId, LocaleId = r2.RefLocaleId } into r2GRP
                from r2 in r2GRP.DefaultIfEmpty()
                join ob in transfer on new { POItemId = poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = ob.POItemId, LocaleId = ob.RefLocaleId } into obGRP
                from ob in obGRP.DefaultIfEmpty()
                select new Models.Views.NBOSRReport
                {
                    Id = poi.Id,
                    LocaleId = poi.LocaleId,
                    PONo = po.BatchNo + '-' + po.SeqId.ToString(),
                    PODate = po.PODate,
                    VendorId = po.VendorId,
                    VendorETD = po.VendorETD,
                    Qty = poi.Qty,
                    MaterialId = poi.MaterialId,
                    SupplierCode = NBMaterial.Get().Where(i => i.MaterialId == poi.MaterialId && i.LocaleId == poi.LocaleId).Select(i => i.VendorCode).FirstOrDefault(),
                    MaterialCode = NBMaterial.Get().Where(i => i.MaterialId == poi.MaterialId && i.LocaleId == poi.LocaleId).Select(i => i.MaterialCode).FirstOrDefault(),
                    PurUnitNameTw = CodeItem.Get().Where(c => c.Id == poi.UnitCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "21").Max(c => c.NameTW),
                    MaterialColor = NBMaterial.Get().Where(i => i.MaterialId == poi.MaterialId && i.LocaleId == poi.LocaleId).Select(i => i.ColorKey).FirstOrDefault(),

                    Vendor = v.NameTw,
                    MaterialNameEng = m.MaterialNameEng,

                    CompanyId = o.CompanyId,
                    OrderNo = o.OrderNo,
                    CustomerOrderNo = o.CustomerOrderNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,

                    ReceivedDate = r1.MinReceivedDate,
                    OBDate = ob.OBDate,
                    ArrivalDate = r2.MinReceivedDate,
                    ReceivedQty = r1.ReceivedQty,
                    // LeadTime
                    // FactoryShipTime
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Distinct()
            .ToList();

            osrItem.ForEach(i =>
            {
                if (i.ReceivedDate != null)
                {
                    TimeSpan diffDays = (TimeSpan)(i.ReceivedDate - i.VendorETD);
                    i.LeadTime = diffDays.Days + 30;
                }

                if (i.OBDate != null && i.ReceivedDate != null)
                {
                    TimeSpan diffDays = (TimeSpan)(i.ReceivedDate - i.OBDate);
                    i.FactoryShipTime = diffDays.Days;
                }

            });

            return osrItem.AsQueryable();
        }
    }
}