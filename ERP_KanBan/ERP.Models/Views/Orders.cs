using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class Orders
    {
        public decimal Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; }
        public decimal CustomerId { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public int OrderType { get; set; }
        public int ProductType { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? ReferUnitPrice { get; set; }
        public DateTime ETD { get; set; }
        public DateTime? ShippingDate { get; set; }
        public decimal CompanyId { get; set; }
        public decimal? OrderSizeCountryCodeId { get; set; }
        public string PackingDescTW { get; set; }
        public string PackingDescEng { get; set; }
        public string SafeCode { get; set; }
        public decimal? BarcodeCodeId { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public string CustomerOrderNo { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int Status { get; set; }
        public DateTime CSD { get; set; }
        public decimal OrderQty { get; set; }
        public int PackingType { get; set; }
        public string LabelDesc { get; set; }
        // public string Mark1PhotoURL { get; set; }
        // public string Mark2Desc { get; set; }
        // public string Mark2PhotoURL { get; set; }
        // public string Mark3Desc { get; set; }
        // public string Mark3PhotoURL { get; set; }
        // public string Mark4Desc { get; set; }
        // public string Mark4PhotoURL { get; set; }
        
        public string CustomerVendorCode { get; set; }
        public string SpecialDesc { get; set; }
        // public string Mark5PhotoURL { get; set; }
        //public decimal MixedBoxes1 { get; set; }
        //public decimal MixedBoxes2 { get; set; }
        //public decimal MixedBoxes3 { get; set; }
        //public decimal MixedBoxes4 { get; set; }
        //public decimal MixedBoxes5 { get; set; }
        
        public decimal? DollarCodeId { get; set; }
        public int doMRP { get; set; }
        public int Version { get; set; }
        public decimal? ProcessSetId { get; set; }
        public decimal ExportPortId { get; set; }
        public string InsockLabel { get; set; }
        public string PackingTypeDesc { get; set; }
        public string CustomerStyleNo { get; set; }
        public string ShoeName { get; set; }
        public string SpecialNote { get; set; }
        public int? PayType { get; set; }
        public string DeliveryTerms { get; set; }
        public int? TransitType { get; set; }
        public decimal? ToolingFund { get; set; }
        public int SpecialPackingStatus { get; set; }
        public decimal ARCustomerId { get; set; }
        public int IsApproved { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? ARLocaleId { get; set; }
        public decimal? ParentOrdersId { get; set; }
        public decimal? RefOrdersLocaleId { get; set; }
        public DateTime? LCSD { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public DateTime? KeyInDate { get; set; }
        public DateTime? OWD { get; set; }
        public DateTime? OWRD { get; set; }
        public DateTime? RSD { get; set; }
        public DateTime? GBSCD { get; set; }
        public DateTime? GBSPUD { get; set; }
        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string Season { get; set; }

        public string LocaleNo { get; set; }
        public string CompanyNo { get; set; } 
        public string Customer { get; set; }
        public string LastNo {get;set;}
        public string OutsoleNo {get;set;}
        public string ArticleName { get; set; }
        public string Dollar { get; set; }
        public string Brand { get; set; }
        public string OrderSizeCountryCode { get; set; }
        public decimal? ArticleSizeCountryCodeId { get; set; }
        public string ArticleSizeCountryCode{ get; set; }
        public string Port { get; set; }
        public string ARLocaleNo { get; set; }
        public string OrdersLocaleNo { get; set; }

        public string RefARCustomer { get; set; }
        public string RefArticleNo { get; set; }
        public string RefStyleNo { get; set; }
        public int RefStyleState { get; set; }
        public string RefColorDesc { get; set; }
        public string RefOutsoleColorDesc {get;set;}
        public string RefOrderType { get; set; }
        public string RefProductType { get; set; }
        public string RefBarcode { get; set; }
        public string RefOrdersStatus { get; set; }
        public string RefPackingType { get; set; }
        public string RefdoMRP { get; set; }
        public string RefPayType { get; set; }
        public string RefDeliveryTerm { get; set; }
        public string RefTransitType { get; set; }
        public string RefApproved { get; set; }

        public bool SpecialStyle { get; set; }
        public bool SpecialCustomer { get; set; }
        public decimal? HasBOM { get; set; }
        public bool IsClosed { get; set; }
    }
    //Id 訂單的識別ID
    //OrderDate 訂單日期
    //OrderNo 訂單編號
    //CustomerId 客戶的ID(ref. Customer.Id)
    //ArticleId 型體的ID(ref. Article.Id)
    //StyleId 鞋型的ID(ref. Style.Id)
    //OrderType 訂單類別(1=預告訂單,2=正式訂單)
    //ProductType 產品屬性(1=銷樣, 2=量產, 3=半成品)
    //UnitPrice 單價(加權平均)
    //ReferUnitPrice ToolingCost(加權平均)
    //ETD 交貨日期
    //ShippingDate 實際出貨日
    //CompanyId 生產的公司ID(ref. Company.Id)
    //SizeCountryCodeId 出貨尺寸國家設定Id(ref. CodeItem.Id, CodeItem.CodeType= '35')
    //PackingDescTW 中文包裝說明
    //PackingDescEng 英文包裝說明
    //SafeCode 安全碼
    //BarcodeCodeId 條碼別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '33')
    //Mark 正嘜
    //SideMark 側嘜
    //CustomerOrderNo 客戶的訂單號碼
    //ModifyUserName 異動者
    //LastUpdateTime 異動時間
    //LocaleId
    //Status  狀態(0=準備中;1=生產中;2=結案;3=作廢)
    //CSD 客戶期望交期
    //OrderQty 訂單數量
    //PackingType(0=單號裝;1=混號裝)

    //Mark1Desc 標誌1說明
    //Mark1PhotoURL 標誌1圖檔路徑
    //Mark2Desc 標誌2說明
    //Mark2PhotoURL 標誌2圖檔路徑
    //Mark3Desc 標誌3說明
    //Mark3PhotoURL 標誌3圖檔路徑
    //Mark4Desc 標誌4說明
    //Mark4PhotoURL 標誌4圖檔路徑
    //Mark5Desc 標誌5說明
    //Mark5PhotoURL 標誌5圖檔路徑
    //MixedBoxes1 混裝箱數1
    //MixedBoxes2 混裝箱數2
    //MixedBoxes3 混裝箱數3
    //MixedBoxes4 混裝箱數4
    //MixedBoxes5 混裝箱數5
    //DollarCodeId 幣別設定的Id(ref. CodeItem.Id, CodeItem.CodeType= '02')
    //doMRP 可否展開(0=否, 1=是)
    //Version 版本
    //ProcessSetId 製程組ID(ref. MpsProcessSetId)
    //ExportPortId 出口港口ID(ref Port.ID)
    //InsockLabel 鞋墊標內容
    //PackingTypeDesc 內箱數
    //CustomerStyleNo 客戶外盒鞋型編號
    //ShoeName
    //SpecialNote 特別説明
    //PayType 客戶付款方式(0=月結, 1=出貨前T/T, 2=出貨後T/T, 3=L/C))(來自MaterialQuot)
    //DeliveryTerms   交貨條件(FOB, CIF, FCA......)
    //TransitType 運輸別(0=海運,1=空運;2=陸運;3=其它)
    //ToolingFund 每雙模具退佣(加權平均)
    //SpecialPackingStatus 特殊包裝(0=不需要,1=需要/未送達,2=需要/已送達
    //ARCustomerId    應付帳款歸屬的客戶PK
    //IsApproved  是否核准過(0=否,1=是)
    //PaymentDate 收款日
    //ARLocaleId 收款地(出貨地)(ref. Company.Id)
    //ParentOrdersId 母訂單(合併訂單Id)
    //RefOrdersLocaleId 外製訂單來源地(ref. Company.Id)
    //LCSD 客戶提前交期
    //GBSPOReferenceNo GBS訂單編號
    //KeyInDate 資料建立日期
    //OWD
    //OWRD
    //RSD
    //GBSCD
    //GBSPUD
    //msrepl_tran_version
    //ArticleNo 型體編號...20110422
    //StyleNo 鞋型編號...20110422

    //select":[{"field":"Id"},{"field":"OrderNo"},{"field":"StyleNo"},{"field":"ShoeName"},{"field":"CompanyId"},{"field":"LocaleId"}]
    public class OrdersCache {
        public decimal Id { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public decimal CompanyId { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public string Brand { get; set; }
    }
}
