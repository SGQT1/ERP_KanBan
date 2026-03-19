using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackMappingItem1
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal PackMappingId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal GWOfCTN { get; set; }
        public decimal NWOfCTN { get; set; }
        public decimal MEAS { get; set; }
        public decimal GWOfCTNCLB { get; set; }
        public decimal MEASCLB { get; set; }
    }
}

// Id	PK
// LocaleId	公司PK
// RefLocaleId	來源地公司別PK
// RefOrdersId	來源地訂單PK(ref. Orders.Id)
// Edition	版本
// ItemInnerSize	訂單SIZE內碼(ref. OrdersItem.ArticleInnerSize)
// RefDisplaySize	對照尺寸顯示內容
// PairOfCTN	每件外箱雙數
// CTNS	外箱件數
// GroupBy	群組
// NWOfCTN	每件外箱淨重
// GWOfCTN	每件外箱毛重
// MEAS	每件外箱才數
// CBM	總外箱才積(=MEAS*CTNS/35.315)
// AdjQty	調整雙數(總雙數=PairOfCTN*CTNS-AdjQty)
// ModifyUserName	異動者
// LastUpdateTime	異動時間