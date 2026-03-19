using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersItem
    {
        public decimal Id { get; set; }
        public decimal? OrdersId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal Qty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? TransferUnitPrice { get; set; }
        public decimal TransferQty { get; set; }
        public decimal? ToolingFund { get; set; }
        public decimal? ToolingCost { get; set; }

        public decimal OrderSize{ get; set; }
        public string OrderSizeSuffix{ get; set; }
        public string ArticleSizeCountry{ get; set; }
        public string OrderSizeCountry{ get; set; }
    }
    //Id
    //OrdersId    訂單ID(ref. Orders.Id)
    //ArticleSize 型體的尺寸
    //ArticleSizeSuffix 型體的尺寸後置字元(J)
    //ArticleInnerSize 型體的內部尺寸
    //DisplaySize 尺寸顯示內容(預設值為ArticleSize+ArticleSizeSuffix)
    //UnitPrice 單價
    //Qty 訂單數量
    //ModifyUserName 異動者
    //LastUpdateTime 異動時間
    //LocaleId
    //MixedQty1   混裝雙數1
    //MixedQty2   混裝雙數2
    //MixedQty3   混裝雙數3
    //MixedQty4   混裝雙數4
    //MixedQty5   混裝雙數5
    //TransferUnitPrice   轉撥生產單價
    //TransferQty 轉撥生產數量
    //ToolingFund
    //ToolingCost
    //msrepl_tran_version
}
