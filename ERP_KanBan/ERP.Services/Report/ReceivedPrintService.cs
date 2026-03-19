using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

using FastReport.Data;
using FastReport.Export.PdfSimple;
using System.Reflection;
using System.IO;
using ERP.Models.Views.Common;
using Newtonsoft.Json;
// using FastReport.Export.Pdf;

namespace ERP.Services.Report
{
    public class ReceivedPrintService : ReportService
    {
        private Services.Entities.ReceivedLogService ReceivedLog { get; }
        private Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; }
        private Services.Entities.TransferItemService TransferItem { get; }
        private Services.Entities.APMonthItemService APMonthItem { get; }

        private Services.Entities.VendorService Vendor { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.POItemService POItem { get; }
        private Services.Entities.WarehouseService Warehouse { get; }

        private Services.Entities.StockIOService StockIO { get; }

        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; set; }

        public ReceivedPrintService(
            Services.Entities.ReceivedLogService receivedLogService,
            Services.Entities.ReceivedLogAddService receivedLogAddService,
            Services.Entities.APMonthItemService apMonthItemService,

            Services.Entities.TransferItemService transferItemService,
            Services.Entities.VendorService vendorService,
            Services.Entities.CompanyService companyService,
            Services.Entities.MaterialService materialService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.POItemService poItemService,
            Services.Business.Entities.MaterialStockItemService materialStockItemService,
            Services.Entities.WarehouseService warehouseService,
            Services.Entities.StockIOService stockIOService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.ReceivedLog = receivedLogService;
            this.ReceivedLogAdd = receivedLogAddService;
            this.APMonthItem = apMonthItemService;

            this.TransferItem = transferItemService;
            this.Vendor = vendorService;
            this.Company = companyService;
            this.Material = materialService;
            this.CodeItem = codeItemService;
            this.POItem = poItemService;
            this.MaterialStockItem = materialStockItemService;
            this.Warehouse = warehouseService;
            this.StockIO = stockIOService;
        }
        public List<Models.Views.Report.Acceptance> Get(int locale, string ids)
        {
            var receiveIds = ids.Split(",").Select(i => Convert.ToDecimal(i));

            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.Report.Acceptance
                {
                    Id = rl.Id,
                    LocaleId = rl.LocaleId,
                    ReceivedDate = rl.ReceivedDate,
                    Vendor = v.ShortNameTw,
                    IQCGetQty = rl.IQCGetQty,
                    IQCTestQty = rl.IQCTestQty,
                    IQCPassQty = rl.IQCPassQty,
                    StockQty = rl.StockQty,
                    OrderNo = rl.OrderNo,
                    NetWeight = rl.NetWeight,
                    PONo = rla.RefPONo,
                    MaterialNameTw = rla.MaterialNameTw,
                    UnitName = rla.PCLUnitNameTw,
                    // MaterialNameEng = Material.Get().Where(i => i.Id == rla.MaterialId && i.LocaleId == rla.LocaleId).Max(i => i.MaterialNameEng),
                    WarehouseNo = Warehouse.Get().Where(i => i.Id == rl.WarehouseId && i.LocaleId == rl.LocaleId).Max(i => i.WarehouseNo),
                    ETD = pi.FactoryETD,
                    TransferInId = rl.TransferInId,
                })
                .Where(i => i.LocaleId == locale && receiveIds.Contains(i.Id))
                .ToList();
            return result;
        }

        public List<Models.Views.Report.Acceptance> Get(string options)
        {
            // var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(options);
            var result = (
                from rl in ReceivedLog.Get()
                join rla in ReceivedLogAdd.Get() on new { ReceivedLogId = rl.Id, LocaleId = rl.LocaleId } equals new { ReceivedLogId = rla.ReceivedLogId, LocaleId = rla.LocaleId } into rlaGRP
                from rla in rlaGRP.DefaultIfEmpty()
                join pi in POItem.Get() on new { POItem = rl.POItemId, LocaleId = rl.RefLocaleId } equals new { POItem = pi.Id, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                join v in Vendor.Get() on new { VendorId = rl.ShippingListVendorId, LocaleId = rl.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.Report.Acceptance
                {
                    Id = rl.Id,
                    LocaleId = rl.LocaleId,
                    ReceivedDate = rl.ReceivedDate,
                    Vendor = v.ShortNameTw,
                    IQCGetQty = rl.IQCGetQty,
                    IQCTestQty = rl.IQCTestQty,
                    IQCPassQty = rl.IQCPassQty,
                    StockQty = rl.StockQty,
                    OrderNo = rl.OrderNo,
                    NetWeight = rl.NetWeight,
                    PONo = rla.RefPONo,
                    MaterialNameTw = rla.MaterialNameTw,
                    UnitName = rla.PCLUnitNameTw,
                    // MaterialNameEng = Material.Get().Where(i => i.Id == rla.MaterialId && i.LocaleId == rla.LocaleId).Max(i => i.MaterialNameEng),
                    WarehouseNo = Warehouse.Get().Where(i => i.Id == rl.WarehouseId && i.LocaleId == rl.LocaleId).Max(i => i.WarehouseNo),
                    ETD = pi.FactoryETD,
                    TransferInId = rl.TransferInId,
                    PaymentLocaleId = pi.PaymentLocaleId,
                })
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, options)
                .ToList();
            return result;
        }
        public byte[] GetReportFile(List<Models.Views.Report.Acceptance> items)
        {
            try
            {
                if (items.Any())
                {
                    var company = Company.Get().Where(i => i.Id == items[0].PaymentLocaleId).First();

                    var formItems = items
                        .OrderBy(i => i.ReceivedDate)
                        .ThenBy(i => i.Vendor)
                        .ThenBy(i => i.MaterialNameTw)
                        .Select((i, index) => new Models.Views.Report.AcceptanceForm
                        {
                            No = (index + 1).ToString(),  // 使用 index 作為遞增的序號，+1 是因為索引從 0 開始
                            Id = i.Id.ToString(),
                            LocaleId = i.LocaleId.ToString(),
                            ReceivedDate = i.ReceivedDate.ToString("yyyyMMdd"),
                            Vendor = i.Vendor,
                            IQCGetQty = Math.Round(i.IQCGetQty, 2).ToString(),
                            IQCTestQty = Math.Round((decimal)i.IQCTestQty, 2).ToString(),
                            IQCPassQty = Math.Round(i.IQCPassQty, 2).ToString(),
                            StockQty = Math.Round(i.StockQty, 2).ToString(),
                            OrderNo = i.OrderNo,
                            NetWeight = Math.Round(i.NetWeight, 2).ToString(),
                            PONo = i.PONo,
                            MaterialNameTw = i.MaterialNameTw,
                            UnitName = i.UnitName,
                            WarehouseNo = i.WarehouseNo,
                            ETD = ((DateTime)i.ETD).ToString("yyyyMMdd"),
                            TransferInId = i.TransferInId.ToString(),
                        })
                        .ToList();

                    var fieldItems = new List<Models.Views.Report.FieldItem>();
                    fieldItems.Add(new Models.Views.Report.FieldItem() { F902 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), F001 = company.ChineseName });

                    string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ERP.Services.dll", string.Empty);
                    string frxFilePath = string.Format("{0}Template/MaterialRecevie/Acceptance.frx", fileDirPath);

                    MemoryStream stream = new MemoryStream();
                    using (FastReport.Report report = new FastReport.Report())
                    {
                        report.Load(frxFilePath);

                        report.RegisterData(fieldItems, "FieldItem");
                        report.RegisterData(formItems, "Acceptance"); //Register Data in report
                        report.GetDataSource("Acceptance").Enabled = true;
                        report.GetDataSource("FieldItem").Enabled = true;

                        report.Prepare();

                        PDFSimpleExport pdfExport = new PDFSimpleExport();
                        // PDFExport pdfExport = new PDFExport
                        // {
                        //     Compressed = true,
                        //     JpegCompression = true,
                        //     JpegQuality = 70,
                        //     EmbeddingFonts = true,
                        //     TextInCurves = false   // 關鍵：把文字畫成向量路徑
                        // };
                        report.Export(pdfExport, stream);
                        report.Dispose();
                    }

                    byte[] file = stream.ToArray();
                    return file;
                }
                return new byte[] { };

            }
            catch (Exception e)
            {
                return new byte[] { };
            }
        }
        public byte[] GetReportFile1(List<Models.Views.Report.Acceptance> items)
        {
            try
            {
                if (items.Any())
                {
                    var company = Company.Get().Where(i => i.Id == items[0].PaymentLocaleId).First();

                    var formItems = items
                        .OrderBy(i => i.ReceivedDate)
                        .ThenBy(i => i.Vendor)
                        .ThenBy(i => i.MaterialNameTw)
                        .Select((i, index) => new Models.Views.Report.AcceptanceForm
                        {
                            No = (index + 1).ToString(),  // 使用 index 作為遞增的序號，+1 是因為索引從 0 開始
                            Id = i.Id.ToString(),
                            LocaleId = i.LocaleId.ToString(),
                            ReceivedDate = i.ReceivedDate.ToString("yyyyMMdd"),
                            Vendor = i.Vendor,
                            IQCGetQty = Math.Round(i.IQCGetQty, 2).ToString(),
                            IQCTestQty = Math.Round((decimal)i.IQCTestQty, 2).ToString(),
                            IQCPassQty = Math.Round(i.IQCPassQty, 2).ToString(),
                            StockQty = Math.Round(i.StockQty, 2).ToString(),
                            OrderNo = i.OrderNo,
                            NetWeight = Math.Round(i.NetWeight, 2).ToString(),
                            PONo = i.PONo,
                            MaterialNameTw = i.MaterialNameTw,
                            UnitName = i.UnitName,
                            WarehouseNo = i.WarehouseNo,
                            ETD = ((DateTime)i.ETD).ToString("yyyyMMdd"),
                            TransferInId = i.TransferInId.ToString(),
                        })
                        .ToList();

                    var fieldItems = new List<Models.Views.Report.FieldItem>();
                    fieldItems.Add(new Models.Views.Report.FieldItem() { F902 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), F001 = company.ChineseName });

                    string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ERP.Services.dll", string.Empty);
                    string frxFilePath = string.Format("{0}Template/MaterialRecevie/Acceptance.frx", fileDirPath);

                    MemoryStream stream = new MemoryStream();
                    using (FastReport.Report report = new FastReport.Report())
                    {
                        // report.Load(frxFilePath);

                        // report.RegisterData(fieldItems, "FieldItem");
                        // report.RegisterData(formItems, "Acceptance"); //Register Data in report
                        // report.GetDataSource("Acceptance").Enabled = true;
                        // report.GetDataSource("FieldItem").Enabled = true;

                        // report.Prepare();

                        // PDFSimpleExport pdfExport = new PDFSimpleExport();
                        // report.Export(pdfExport, stream);
                        // report.Dispose();
                    }

                    byte[] file = stream.ToArray();
                    return file;
                }
                return new byte[] { };

            }
            catch (Exception e)
            {
                return new byte[] { };
            }
        }

        public List<Models.Views.Report.FieldItem> GetFieldItems(int edtion)
        {
            // var lang = edtion == 1 ? "zh" : edtion == 2 ? "cn" : edtion == 3 ? "en" : edtion == 4 ? "vi" : edtion == 5 ? "id" : edtion == 6 ? "kh" : "zh";


            List<Models.Views.Report.FieldItem> items = new List<Models.Views.Report.FieldItem>();
            // List<Models.Views.Report.FieldItem> secItems = new List<Models.Views.Report.FieldItem>();

            // // master lang is tw or cn or en


            // Translate.SetLang(lang);
            // if (edtion <= 3)
            // {
            //     Translate.SetLang(lang);
            //     items.Add(new Models.Views.Report.FieldItem
            //     {
            //         F901 = Translate.Get("Report.Page"),
            //         F902 = Translate.Get("Report.PrintDate"),
            //         F903 = Translate.Get("Report.PringBy"),
            //         F001 = Translate.Get("Report.CSD"),
            //         F002 = Translate.Get("Report.DailyNo"),
            //         F003 = Translate.Get("Report.DailyDate"),
            //         F004 = Translate.Get("Report.CompletionDate"),
            //         F005 = Translate.Get("Report.Unit"),
            //         F006 = Translate.Get("Report.OrderNo"),
            //         F007 = Translate.Get("Report.StyleNo"),
            //         F008 = Translate.Get("Report.DailyType"),
            //         F009 = Translate.Get("Report.PrintCount"),
            //         F010 = Translate.Get("Report.Material"),
            //         F011 = Translate.Get("Report.Part"),
            //         F012 = Translate.Get("Report.Pieces"),
            //         F013 = Translate.Get("Report.KnifeNo"),
            //         F014 = Translate.Get("Report.SizeRun"),
            //         F015 = Translate.Get("Report.DispatchQty"),
            //         F016 = Translate.Get("Report.AcceptQty"),
            //         F017 = Translate.Get("Report.TotalDispatchQty"),
            //         F018 = Translate.Get("Report.TotalAcceptQty"),
            //         F019 = Translate.Get("Report.Confirmor"),
            //         F020 = Translate.Get("Report.Cutting"),
            //         F021 = Translate.Get("Report.Counter"),
            //         F022 = Translate.Get("Report.AcceptDispatchQty"),
            //         F023 = Translate.Get("Report.AcceptUsage"),
            //         F024 = Translate.Get("Report.BalanceUsage"),
            //         F025 = Translate.Get("Report.QC"),
            //         F026 = Translate.Get("Report.Warehouse"),
            //         F027 = Translate.Get("Report.Requester"),
            //     });

            // }

            // // second lang append master lang
            // if (edtion > 3)
            // {
            //     Translate.SetLang("zh");
            //     items.Add(new Models.Views.Report.FieldItem
            //     {
            //         F901 = Translate.Get("Report.Page"),
            //         F902 = Translate.Get("Report.PrintDate"),
            //         F903 = Translate.Get("Report.PringBy"),
            //         F001 = Translate.Get("Report.CSD"),
            //         F002 = Translate.Get("Report.DailyNo"),
            //         F003 = Translate.Get("Report.DailyDate"),
            //         F004 = Translate.Get("Report.CompletionDate"),
            //         F005 = Translate.Get("Report.Unit"),
            //         F006 = Translate.Get("Report.OrderNo"),
            //         F007 = Translate.Get("Report.StyleNo"),
            //         F008 = Translate.Get("Report.DailyType"),
            //         F009 = Translate.Get("Report.PrintCount"),
            //         F010 = Translate.Get("Report.Material"),
            //         F011 = Translate.Get("Report.Part"),
            //         F012 = Translate.Get("Report.Pieces"),
            //         F013 = Translate.Get("Report.KnifeNo"),
            //         F014 = Translate.Get("Report.SizeRun"),
            //         F015 = Translate.Get("Report.DispatchQty"),
            //         F016 = Translate.Get("Report.AcceptQty"),
            //         F017 = Translate.Get("Report.TotalDispatchQty"),
            //         F018 = Translate.Get("Report.TotalAcceptQty"),
            //         F019 = Translate.Get("Report.Confirmor"),
            //         F020 = Translate.Get("Report.Cutting"),
            //         F021 = Translate.Get("Report.Counter"),
            //         F022 = Translate.Get("Report.AcceptDispatchQty"),
            //         F023 = Translate.Get("Report.AcceptUsage"),
            //         F024 = Translate.Get("Report.BalanceUsage"),
            //         F025 = Translate.Get("Report.QC"),
            //         F026 = Translate.Get("Report.Warehouse"),
            //         F027 = Translate.Get("Report.Requester"),
            //     });
            //     Translate.SetLang(lang);
            //     // items[0].F901 += "\n" + Translate.Get("Report.Page");
            //     // items[0].F902 += "\n" + Translate.Get("Report.PrintDate");
            //     // items[0].F903 += "\n" + Translate.Get("Report.PringBy");
            //     // items[0].F001 += "\n" + Translate.Get("Report.CSD");
            //     items[0].F002 += "\n" + Translate.Get("Report.DailyNo");
            //     items[0].F003 += "\n" + Translate.Get("Report.DailyDate");
            //     items[0].F004 += "\n" + Translate.Get("Report.CompletionDate");
            //     items[0].F005 += "\n" + Translate.Get("Report.Unit");
            //     items[0].F006 += "\n" + Translate.Get("Report.OrderNo");
            //     items[0].F007 += "\n" + Translate.Get("Report.StyleNo");
            //     items[0].F008 += "\n" + Translate.Get("Report.DailyType");
            //     items[0].F009 += "\n" + Translate.Get("Report.PrintCount");
            //     items[0].F010 += "\n" + Translate.Get("Report.Material");
            //     items[0].F011 += "\n" + Translate.Get("Report.Part");
            //     items[0].F012 += "\n" + Translate.Get("Report.Pieces");
            //     items[0].F013 += "\n" + Translate.Get("Report.KnifeNo");
            //     items[0].F014 += " " + Translate.Get("Report.SizeRun");
            //     items[0].F015 += " " + Translate.Get("Report.DispatchQty");
            //     items[0].F016 += " " + Translate.Get("Report.AcceptQty");
            //     items[0].F017 += "\n" + Translate.Get("Report.TotalDispatchQty");
            //     items[0].F018 += "\n" + Translate.Get("Report.TotalAcceptQty");
            //     items[0].F019 += "\n" + Translate.Get("Report.Confirmor");
            //     items[0].F020 += "\n" + Translate.Get("Report.Cutting");
            //     items[0].F021 += "\n" + Translate.Get("Report.Counter");
            //     items[0].F022 += "\n" + Translate.Get("Report.AcceptDispatchQty");
            //     items[0].F023 += "\n" + Translate.Get("Report.AcceptUsage");
            //     items[0].F024 += "\n" + Translate.Get("Report.BalanceUsage");
            //     items[0].F025 += "\n" + Translate.Get("Report.QC");
            //     items[0].F026 += "\n" + Translate.Get("Report.Warehouse");
            //     items[0].F027 += "\n" + Translate.Get("Report.Requester");
            // }

            return items;
        }

    }
}
