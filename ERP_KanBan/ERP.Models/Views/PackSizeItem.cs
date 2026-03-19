using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    /*
     * replace PackPlanItem
     */
    public class PackSizeItem
    {
        public decimal Id { get; set; }             //OrdersItemId
        public decimal LocaleId { get; set; }       //資料所屬廠別
        public decimal RefLocalId {get;set;}
        public string SizeCountryNameTw { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public decimal OrdersId { get; set; }
        public string OrdersNo { get; set; }
        public decimal Qty { get; set; }
        public decimal AvailableQty { get; set; }
        public decimal AdjQty { get; set; }
        public string CustomerOrderNo { get; set; }
        public string RefCustomer { get; set; }
        public string StyleNo { get; set; }
        public DateTime CSD { get; set; }
        public string ArticleSize { get; set; }
        public string OrderSize { get; set; }
        public decimal ItemInnerSize { get; set; }
        public decimal? PairsOfCTN { get; set; }
        public decimal GWOfCTN { get; set; }
        public decimal NWOfCTN { get; set; }
        public decimal MEAS { get; set; }
        public string CTNSpec { get; set; }
        public string CTNL { get; set; }
        public string CTNW { get; set; }
        public string CTNH { get; set; }
        public string BOXSpec { get; set; }
        public string BOXL { get; set; }
        public string BOXW { get; set; }
        public string BOXH { get; set; }
    }
}
