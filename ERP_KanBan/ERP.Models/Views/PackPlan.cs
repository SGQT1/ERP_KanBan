using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    /*
     * replace OrdersPL
     */
    public class PackPlan
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string OrderNo { get; set; }
        public string Edition { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public decimal? PackingQty { get; set; }
        public int PackingTypeId { get; set; }
        public string PackingType { get; set; }
        public string PackingTypeDesc { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int CNoFrom { get; set; }
        public decimal? PackingCTNS { get; set; }
        public decimal? PackingNW { get; set; }
        public decimal? PackingGW { get; set; }
        public decimal? PackingMEAS { get; set; }
        public decimal? PackingCBM { get; set; }
        
        public decimal RefOrderQty { get; set; }
        public decimal RefCompanyId { get; set; }
        public string RefCompany { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public decimal RefBrandCodeId { get; set; }
        public string RefBrand { get; set; }
        public decimal RefArticleId { get; set; }
        public string RefArticleNo { get; set; }
        public decimal RefStyleId { get; set; }
        public string RefStyleNo { get; set; }
        public decimal RefProductTypeId { get; set; }
        public string RefProductType { get; set; }
        public DateTime RefCSD { get; set; }
        public DateTime RefLCSD { get; set; }
        public string RefShoeName { get; set; }
        public int RefOrderTypeId { get; set; }
        public string RefOrderType { get; set; }
        public int RefSizeCountryId { get; set; }
        public string RefOrdersStatus { get; set; }
        public string RefPackingType { get; set; }
        public string RefOutsole { get; set; }
        public string RefLast { get; set; }
        public string RefCustomer { get; set; }
        public string RefSeason { get; set; }
        public int RefTransitTypeId { get; set; }
        public string RefTransitType { get; set; }
        public string RefCustomerOrderNo { get; set; }
        public DateTime RefOrderDate { get; set; }
        public string InvoiceNo { get; set; }
    }
}
