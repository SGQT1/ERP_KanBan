using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
// using FastReport.Data;
// using FastReport.Export.PdfSimple;

namespace ERP.Services.Report
{
    public class MPSDailyPrintService : ReportService
    {
        ERP.Services.Business.Entities.CompanyService Company;
        ERP.Services.Business.Entities.MPSDailyService MPSDaily;
        ERP.Services.Business.Entities.MPSDailyMaterialService MPSDailyMaterial;
        ERP.Services.Business.Entities.MPSDailyMaterialItemService MPSDailyMaterialItem;
        ERP.Services.Business.Entities.MPSOrdersItemService MPSOrdersItem;
        ERP.Services.Business.TranslateService Translate { get; set; }

        ERP.Services.Business.Entities.MPSDailyPrintLogService MPSDailyPrintLog;
        ERP.Services.Business.Entities.MPSDailyAddPrintLogService MPSDailyAddPrintLog;

        public MPSDailyPrintService(
            ERP.Services.Business.Entities.CompanyService companyService,
            ERP.Services.Business.Entities.MPSDailyService mpsDailyService,
            ERP.Services.Business.Entities.MPSDailyMaterialService mpsDailyMaterialService,
            ERP.Services.Business.Entities.MPSDailyMaterialItemService mpsDailyMaterialItemService,
            ERP.Services.Business.Entities.MPSOrdersItemService mpsOrdersItemService,
            ERP.Services.Business.TranslateService translateService,
            ERP.Services.Business.Entities.MPSDailyPrintLogService mpsDailyPrintLogService,
            ERP.Services.Business.Entities.MPSDailyAddPrintLogService mpsDailyAddPrintLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Company = companyService;
            MPSDaily = mpsDailyService;
            MPSDailyMaterial = mpsDailyMaterialService;
            MPSDailyMaterialItem = mpsDailyMaterialItemService;
            MPSOrdersItem = mpsOrdersItemService;
            Translate = translateService;

            MPSDailyPrintLog = mpsDailyPrintLogService;
            MPSDailyAddPrintLog = mpsDailyAddPrintLogService;
        }

        public byte[] GetReportFile(string ids, int localeId, string printBy, int edition)
        {
            try
            {
                List<ERP.Models.Views.Report.MPSDaily> datas = new List<ERP.Models.Views.Report.MPSDaily>();

                var items = ids.Replace("'", "").Split(',');
                var company = Company.Get().Where(i => i.Id == localeId).Select(i => i.ChineseName).First();
                var dailys = MPSDaily.Get().Where(i => items.Contains(i.DailyNo))
                .Select(i => new Models.Views.Report.MPSDaily
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    Company = company,
                    DailyMode = i.DailyMode == 1 ? "計畫派工" : "半成品派工",
                    DailyNo = i.DailyNo,
                    OrgUnit = i.OrgUnitNameTw,
                    PrintTime = DateTime.Now.ToString("yyyy-MM-dd"),
                    PrintBy = printBy,
                    DailyDate = i.DailyDate.ToString("yyyy-MM-dd"),
                    FinishedDate = ((DateTime)i.FinishedDate).ToString("yyyy-MM-dd"),
                    Unit = i.UnitNameTw,
                    MaterialNameTw = i.MaterialNameTw,
                    OrderNo = i.OrderNo,
                    CSD = i.CSD.ToString("yyyy-MM-dd"),
                    StyleNo = i.StyleNo,
                    DailyType = i.DailyType == 1 ? "計畫派工" : i.DailyType == 2 ? "差異派工" : i.DailyType == 3 ? "補料派工" : i.DailyType == 4 ? "其他派工" : "",
                    DailyTimes = "1",
                    PrintCount = 1,
                    OrderQty = i.Qty,
                    SubUsage = (decimal)i.TotalUsage,
                    Mulit = i.Multi,
                }).ToList();

                foreach (var daily in dailys)
                {
                    var parts = MPSDailyMaterial.Get().Where(i => i.MPSDailyId == daily.Id)
                        .Select(i => new Models.Views.Report.MPSDailyPart
                        {
                            PartNameTw = i.PartNameTw,
                            PieceOfPair = i.PieceOfPair,
                            RefKnifeNo = i.RefKnifeNo
                        }).ToList();
                        
                    var item = (
                        from m in MPSDailyMaterial.Get()
                        join mi in MPSDailyMaterialItem.Get() on new { MpsMaterialId = m.Id, LocaleId = m.LocaleId } equals new { MpsMaterialId = mi.MpsDailyMaterialId, LocaleId = mi.LocaleId } into miGrp
                        from mi in miGrp.DefaultIfEmpty()
                        join oi in MPSOrdersItem.Get() on new { MpsOrdersItemId = mi.MpsOrdersItemId, LocaleId = mi.LocaleId } equals new { MpsOrdersItemId = oi.Id, LocaleId = oi.LocaleId }
                        where m.MPSDailyId == daily.Id
                        select new
                        {
                            Size = oi.DisplaySize,
                            Qty = mi.SubQty,
                            Usage = ((mi.SubUsage - mi.PreSubUsage) * (1 + daily.Mulit))
                        }
                    ).ToList();

                    var usage = item.GroupBy(i => new { i.Size, i.Qty }).Select(i => new Models.Views.Report.MPSDailyItem
                    {
                        Size = i.Key.Size,
                        Qty = i.Key.Qty,
                        Usage = i.Sum(g => g.Usage)
                    }).ToList();

                    daily.SubQty = usage.Sum(i => i.Qty);
                    daily.MPSDailyPart = parts;
                    daily.MPSDailyItem = usage;
                }

                string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ERP.Services.dll", string.Empty);
                string frxFilePath = string.Format("{0}Template/ProductionPlan/MPSDaily.frx", fileDirPath);
                var fields = GetFieldItems(edition);
                // var fields = GetFieldItems("vi");

                MemoryStream stream = new MemoryStream();
                // using (FastReport.Report report = new FastReport.Report())
                // {
                //     report.Load(frxFilePath); //Load report
                //     report.RegisterData(fields, "FieldItem");
                //     report.RegisterData(dailys, "MPSDaily"); //Register Data in report
                //     report.Prepare();

                //     PDFSimpleExport pdfExport = new PDFSimpleExport();
                //     report.Export(pdfExport, stream);
                // }

                byte[] file = stream.ToArray();
                return file;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Models.Views.Report.FieldItem> GetFieldItems(int edtion)
        {
            var lang = edtion == 1 ? "zh" : edtion == 2 ? "cn" : edtion == 3 ? "en" : edtion == 4 ? "vi" : edtion == 5 ? "id" : edtion == 6 ? "kh" : "zh";


            List<Models.Views.Report.FieldItem> items = new List<Models.Views.Report.FieldItem>();
            List<Models.Views.Report.FieldItem> secItems = new List<Models.Views.Report.FieldItem>();

            // master lang is tw or cn or en


            Translate.SetLang(lang);
            if (edtion <= 3)
            {
                Translate.SetLang(lang);
                items.Add(new Models.Views.Report.FieldItem
                {
                    F901 = Translate.Get("Report.Page"),
                    F902 = Translate.Get("Report.PrintDate"),
                    F903 = Translate.Get("Report.PringBy"),
                    F001 = Translate.Get("Report.CSD"),
                    F002 = Translate.Get("Report.DailyNo"),
                    F003 = Translate.Get("Report.DailyDate"),
                    F004 = Translate.Get("Report.CompletionDate"),
                    F005 = Translate.Get("Report.Unit"),
                    F006 = Translate.Get("Report.OrderNo"),
                    F007 = Translate.Get("Report.StyleNo"),
                    F008 = Translate.Get("Report.DailyType"),
                    F009 = Translate.Get("Report.PrintCount"),
                    F010 = Translate.Get("Report.Material"),
                    F011 = Translate.Get("Report.Part"),
                    F012 = Translate.Get("Report.Pieces"),
                    F013 = Translate.Get("Report.KnifeNo"),
                    F014 = Translate.Get("Report.SizeRun"),
                    F015 = Translate.Get("Report.DispatchQty"),
                    F016 = Translate.Get("Report.AcceptQty"),
                    F017 = Translate.Get("Report.TotalDispatchQty"),
                    F018 = Translate.Get("Report.TotalAcceptQty"),
                    F019 = Translate.Get("Report.Confirmor"),
                    F020 = Translate.Get("Report.Cutting"),
                    F021 = Translate.Get("Report.Counter"),
                    F022 = Translate.Get("Report.AcceptDispatchQty"),
                    F023 = Translate.Get("Report.AcceptUsage"),
                    F024 = Translate.Get("Report.BalanceUsage"),
                    F025 = Translate.Get("Report.QC"),
                    F026 = Translate.Get("Report.Warehouse"),
                    F027 = Translate.Get("Report.Requester"),
                    // F901 = "頁次",
                    // F902 = "打印日期",
                    // F903 = "打印人",
                    // F001 = "CSD",
                    // F002 = "派工編號\nngười đặt",
                    // F003 = "派工日\nngười đặt",
                    // F004 = "預計完成日\nngười đặt",
                    // F005 = "計量單位\nngười đặt",
                    // F006 = "管制表編號\nngười đặt",
                    // F007 = "鞋型編號\nngười đặt",
                    // F008 = "派工類別\nngười đặt",
                    // F009 = "印次\nngười đặt",
                    // F010 = "材料\nngười đặt",
                    // F011 = "部位名稱\nngười đặt",
                    // F012 = "片數\nngười đặt",
                    // F013 = "斬刀編號\nngười đặt",
                    // F014 = "Size",
                    // F015 = "雙數(nngười)",
                    // F016 = "核發量(nngười)",
                    // F017 = "總雙數\nngười đặt hàng",
                    // F018 = "總核發量\nngười đặt hàng",
                    // F019 = "完成確認\nចំនួនជាក់ស្ដែ",
                    // F020 = "裁斷員\nចំនួនជាក់ស្ដែ",
                    // F021 = "點料員\nngười đặt hàng",
                    // F022 = "雙數\nngười đặt hàng",
                    // F023 = "實耗量\nngười đặt hàng",
                    // F024 = "退料量\nngười đặt hàng",
                    // F025 = "品管\nngười đặt hàng",
                    // F026 = "倉庫\nngười đặt hàng",
                    // F027 = "領料員\nngười đặt hàng",
                });

            }

            // second lang append master lang
            if (edtion > 3)
            {
                Translate.SetLang("zh");
                items.Add(new Models.Views.Report.FieldItem
                {
                    F901 = Translate.Get("Report.Page"),
                    F902 = Translate.Get("Report.PrintDate"),
                    F903 = Translate.Get("Report.PringBy"),
                    F001 = Translate.Get("Report.CSD"),
                    F002 = Translate.Get("Report.DailyNo"),
                    F003 = Translate.Get("Report.DailyDate"),
                    F004 = Translate.Get("Report.CompletionDate"),
                    F005 = Translate.Get("Report.Unit"),
                    F006 = Translate.Get("Report.OrderNo"),
                    F007 = Translate.Get("Report.StyleNo"),
                    F008 = Translate.Get("Report.DailyType"),
                    F009 = Translate.Get("Report.PrintCount"),
                    F010 = Translate.Get("Report.Material"),
                    F011 = Translate.Get("Report.Part"),
                    F012 = Translate.Get("Report.Pieces"),
                    F013 = Translate.Get("Report.KnifeNo"),
                    F014 = Translate.Get("Report.SizeRun"),
                    F015 = Translate.Get("Report.DispatchQty"),
                    F016 = Translate.Get("Report.AcceptQty"),
                    F017 = Translate.Get("Report.TotalDispatchQty"),
                    F018 = Translate.Get("Report.TotalAcceptQty"),
                    F019 = Translate.Get("Report.Confirmor"),
                    F020 = Translate.Get("Report.Cutting"),
                    F021 = Translate.Get("Report.Counter"),
                    F022 = Translate.Get("Report.AcceptDispatchQty"),
                    F023 = Translate.Get("Report.AcceptUsage"),
                    F024 = Translate.Get("Report.BalanceUsage"),
                    F025 = Translate.Get("Report.QC"),
                    F026 = Translate.Get("Report.Warehouse"),
                    F027 = Translate.Get("Report.Requester"),
                });
                Translate.SetLang(lang);
                // items[0].F901 += "\n" + Translate.Get("Report.Page");
                // items[0].F902 += "\n" + Translate.Get("Report.PrintDate");
                // items[0].F903 += "\n" + Translate.Get("Report.PringBy");
                // items[0].F001 += "\n" + Translate.Get("Report.CSD");
                items[0].F002 += "\n" + Translate.Get("Report.DailyNo");
                items[0].F003 += "\n" + Translate.Get("Report.DailyDate");
                items[0].F004 += "\n" + Translate.Get("Report.CompletionDate");
                items[0].F005 += "\n" + Translate.Get("Report.Unit");
                items[0].F006 += "\n" + Translate.Get("Report.OrderNo");
                items[0].F007 += "\n" + Translate.Get("Report.StyleNo");
                items[0].F008 += "\n" + Translate.Get("Report.DailyType");
                items[0].F009 += "\n" + Translate.Get("Report.PrintCount");
                items[0].F010 += "\n" + Translate.Get("Report.Material");
                items[0].F011 += "\n" + Translate.Get("Report.Part");
                items[0].F012 += "\n" + Translate.Get("Report.Pieces");
                items[0].F013 += "\n" + Translate.Get("Report.KnifeNo");
                items[0].F014 += " " + Translate.Get("Report.SizeRun");
                items[0].F015 += " " + Translate.Get("Report.DispatchQty");
                items[0].F016 += " " + Translate.Get("Report.AcceptQty");
                items[0].F017 += "\n" + Translate.Get("Report.TotalDispatchQty");
                items[0].F018 += "\n" + Translate.Get("Report.TotalAcceptQty");
                items[0].F019 += "\n" + Translate.Get("Report.Confirmor");
                items[0].F020 += "\n" + Translate.Get("Report.Cutting");
                items[0].F021 += "\n" + Translate.Get("Report.Counter");
                items[0].F022 += "\n" + Translate.Get("Report.AcceptDispatchQty");
                items[0].F023 += "\n" + Translate.Get("Report.AcceptUsage");
                items[0].F024 += "\n" + Translate.Get("Report.BalanceUsage");
                items[0].F025 += "\n" + Translate.Get("Report.QC");
                items[0].F026 += "\n" + Translate.Get("Report.Warehouse");
                items[0].F027 += "\n" + Translate.Get("Report.Requester");
            }
            return items;
        }

        public void MPSDailyPringLog(IEnumerable<Models.Views.MPSDailyPrintLog> items) {
            MPSDailyPrintLog.CreateRange(items);
        }
        public void MPSDailyAddPringLog(IEnumerable<Models.Views.MPSDailyAddPrintLog> items) {
            MPSDailyAddPrintLog.CreateRange(items);
        }
    }
}