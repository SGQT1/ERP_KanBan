using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Company
    {
        public decimal Id { get; set; }
        public string CompanyNo { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string ChineseShortName { get; set; }
        public string EnglishShortName { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string ChineseAddress { get; set; }
        public string EnglishAddress { get; set; }
        public string OfficialAddress { get; set; }
        public string UnifiedNo { get; set; }
        public string TaxNo { get; set; }
        public string Owner { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceTitle { get; set; }
        public string WebsiteUrl { get; set; }
        public short StockClosedMonth { get; set; }
        public short StockClosedDay { get; set; }
        public short AccountClosedMonth { get; set; }
        public short AccountClosedDay { get; set; }
        public decimal BusinessTaxRate { get; set; }
        public string Remark { get; set; }
        public string AccountDollarNameTw { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public int IgnoreSunday { get; set; }
        public int IgnoreHoliday { get; set; }
        public string Contact { get; set; }
        public string ContactMobileNo { get; set; }
        public string ContactEmail { get; set; }
        public int? Enable { get; set; }
        public int? IsVirtual { get; set; }
    }
    //Id
    //CompanyNo   公司代號
    //ChineseName 公司中文名稱
    //EnglishName 公司英文名稱
    //ChineseShortName    公司中文簡稱
    //EnglishShortName    公司英文簡稱
    //TelNo   聯絡電話
    //FaxNo   傳真電話
    //ChineseAddress  中文地址
    //EnglishAddress  英文地址
    //OfficialAddress 官方語言地址
    //UnifiedNo   統一編號
    //TaxNo   稅藉資料
    //Owner   負責人
    //InvoiceAddress  發票地址
    //InvoiceTitle    發票抬頭
    //WebsiteURL  公司網址
    //StockClosedMonth    庫存關帳月(每年)
    //StockClosedDay 庫存關帳日(每月)
    //AccountClosedMonth 會計關帳月(每年)
    //AccountClosedDay 會計關帳日(每月)
    //BusinessTaxRate 營業稅率
    //Remark 備註說明
    //AccountDollarNameTw 帳務幣別
    //LastUpdateTime 最後異動時間
    //ModifyUserName 最後異動者
    //IgnoreSunday 忽略星期日為假日(0=否,1=是)
    //IgnoreHoliday 忽略國定假日(0=否,1=是)
    //Contact 連絡人
    //ContactMobileNo 聯絡人電話
    //ContactEmail 連絡人EMAIL
}
