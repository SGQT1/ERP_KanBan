using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Customer
    {
        public decimal Id { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string ChineseShortName { get; set; }
        public string OwnerName { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string FaxNo1 { get; set; }
        public string FaxNo2 { get; set; }
        public decimal? CountryCodeId { get; set; }
        public string RefCountry {get;set;}
        public decimal? AreaCodeId { get; set; }
         public string RefArea {get;set;}
        public decimal? BrandCodeId { get; set; }
        public string RefBrand {get;set;}
        public decimal? DollarCodeId { get; set; }
        public string RefCurrency {get;set;}
        public string UnifiedInvoiceNo { get; set; }
        public string CompanyAddressZip { get; set; }
        public string CompanyAddress { get; set; }
        public string InvoiceAddressZip { get; set; }
        public string InvoiceAddress { get; set; }
        public string Contact { get; set; }
        public string ContactMobileNo { get; set; }
        public string ContactEmail { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? CreditOverRate { get; set; }
        public string FirstTradeDate { get; set; }
        public string LastTradeDate { get; set; }
        public decimal? InvoiceCodeId { get; set; }
        public string RefInvoice {get;set;}
        public decimal? TaxCodeId { get; set; }
        public string RefTax {get;set;}
        public decimal? PaymentCodeId { get; set; }
        public string RefPayment {get;set;}
        public int? PaymentDays { get; set; }
        public string PackingDescTW { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public string SafeCode { get; set; }
        public decimal? BarcodeCodeId { get; set; }
        public string RefBarcode { get; set; }
        public int? PriceType { get; set; }
        public string RefPrice { get;set; }
        public decimal? ExportPortId { get; set; }
        public string RefPort { get; set; }
        public string PackingDescEng { get; set; }
        public decimal LocaleId { get; set; }
        public string DefaultPhotoURL1 { get; set; }
        public string DefaultPhotoURL2 { get; set; }
        public string SpecialNoteTw { get; set; }
        public string SpecialNoteEn { get; set; }
        public int? PayType { get; set; }
        public string RefPay { get;set; }
        public string DeliveryTerms { get; set; }
        public int? DayOfMonth { get; set; }
        public int? ShipmentType { get; set; }
        public string RefShipment {get;set;}
        public decimal? TaxRate { get; set; }
        public int? IsTaxAdded { get; set; }
        public int IsSpecial { get; set; }
        public int HasOrders { get; set; }
    }
    //Id(PK)
    //ChineseName 客戶中文名稱(U1)
    //EnglishName 客戶英文名稱
    //ChineseShortName 客戶中文簡稱(U1)
    //OwnerName 負責人
    //TelNo1 電話1
    //TelNo2 電話2
    //FaxNo1 傳真1
    //FaxNo2 傳真2
    //CountryCodeId 國家別設定Id(ref. CodeItem.Id, CodeItem.CodeType= '01 )(FK1)
    //AreaCodeId 地區別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '07)(FK2)
    //BrandCodeId 品牌別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '25')(FK3)
    //DollarCodeId    幣別的設定(ref. CodeItem.Id, CodeItem.CodeType= '02')(FK4)
    //UnifiedInvoiceNo    統一編號
    //CompanyAddressZip   公司地址郵遞區號
    //CompanyAddress  公司地址
    //InvoiceAddressZip   發票地址郵遞區號
    //InvoiceAddress  發票地址
    //Contact 聯絡人
    //ContactMobileNo 聯絡人手機
    //ContactEmail    聯絡人電子郵件
    //CreditAmount    信用額度(-1=不限)
    //CreditOverRate 信用額度超出率
    //FirstTradeDate 首次交易日
    //LastTradeDate 最近一次交易日
    //InvoiceCodeId 發票聯式別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '08')(FK5)
    //TaxCodeId   稅別的設定Id((ref. CodeItem.Id, CodeItem.CodeType= '09')(FK6)
    //PaymentCodeId   付款方式別的設定類別(ref. CodeItem.Id, CodeItem.CodeType= '10')(FK7)
    //PaymentDays 付款天數(-1=不限)
    //PackingDescTW 中文包裝說明
    //ModifyUserName 修改者
    //LastUpdateTime 最後修改時間
    //Mark 正嘜資料
    //SideMark 側嘜資料
    //SafeCode 安全碼
    //BarcodeCodeId 條碼別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '33')(FK8)
    //PriceType   產品取價別設定Id(1=INTEL, 2=SUB)
    //ExportPortId 出口港口ID(ref Port.ID)(FK10)
    //PackingDescEng  英文包裝說明
    //LocaleId    歸屬分公司Id(ref. Company.Id)(PK) (U1) (U2)
    //DefaultPhotoURL1    客戶預設圖檔路徑1
    //DefaultPhotoURL2    客戶預設圖檔路徑2
    //DefaultPhotoURL3    客戶預設圖檔路徑3
    //DefaultPhotoURL4    客戶預設圖檔路徑4
    //PayType 客戶付款方式(0=月結, 1=出貨前T/T, 2=出貨後T/T, 3=L/C))(來自MaterialQuot)
    //DeliveryTerms   交貨條件(FOB, CIF, FCA......)
    //DayOfMonth 月結日
    //ShipmentType 銷貨類別(0=出口,1=內銷)
    //TaxRate 稅率
    //IsTaxAdded 稅別(0=免稅,1=應稅,2=零稅率)
    //msrepl_tran_version

}
