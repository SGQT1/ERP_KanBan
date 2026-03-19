using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class REP_ORDERS
    {
        public string ColorCode { get; set; }
        public string StyleNo { get; set; }
        public string ProcessNoteTW { get; set; }
        public string ColorDesc { get; set; }
        public string OutsoleColorDescTW { get; set; }
        public string OutsoleColorDescEN { get; set; }
        public decimal? OutsoleId { get; set; }
        public decimal? ShellId { get; set; }
        public string FinishGoodsPhotoURL { get; set; }
        public string MoldNo { get; set; }
        public decimal? LastId { get; set; }
        public decimal? KnifeId { get; set; }
        public string KnifeNo { get; set; }
        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        public string ShellNo { get; set; }
        public decimal BrandCodeId { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public decimal Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; }
        public decimal CustomerId { get; set; }
        public decimal ArticleId { get; set; }
        public decimal StyleId { get; set; }
        public string OrderType { get; set; }
        public string OrderTypeEn { get; set; }
        public string ProductType { get; set; }
        public string ProductTypeEn { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? ReferUnitPrice { get; set; }
        public DateTime ETD { get; set; }
        public DateTime? ShippingDate { get; set; }
        public decimal CompanyId { get; set; }
        public string CompanyNo { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public decimal? OrderSizeCountryCodeId { get; set; }
        public string PackingDescTW { get; set; }
        public string PackingDescEng { get; set; }
        public string SafeCode { get; set; }
        public decimal? BarcodeCodeId { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public string CustomerOrderNo { get; set; }
        public decimal LocaleId { get; set; }
        public int Status { get; set; }
        public DateTime CSD { get; set; }
        public decimal OrderQty { get; set; }
        public decimal? PCLQty { get; set; }
        public string PackingType { get; set; }
        public string PackingTypeEn { get; set; }
        public string Mark1Desc { get; set; }
        public string Mark1PhotoURL { get; set; }
        public string Mark2Desc { get; set; }
        public string Mark2PhotoURL { get; set; }
        public string Mark3Desc { get; set; }
        public string Mark3PhotoURL { get; set; }
        public string Mark4Desc { get; set; }
        public string Mark4PhotoURL { get; set; }
        public string Mark5Desc { get; set; }
        public string Mark5PhotoURL { get; set; }
        public decimal? DollarCodeId { get; set; }
        public int doMRP { get; set; }
        public int Version { get; set; }
        public decimal? ProcessSetId { get; set; }
        public decimal ExportPortId { get; set; }
        public string InsockLabel { get; set; }
        public string PackingTypeDesc { get; set; }
        public string CustomerStyleNo { get; set; }
        public string ShoeName { get; set; }
        public int IsApproved { get; set; }
        public string ProcessNoteEng { get; set; }
        public string BrandName { get; set; }
        public string MappingSizeCountry { get; set; }
        public string OrdersCompanyNo { get; set; }
        public string PortName { get; set; }
        public string PortNameEng { get; set; }
        public string StyleSizeCountry { get; set; }
        public string SpecialNote { get; set; }
        public string CustomerNameTw { get; set; }
        public string CustomerNameEn { get; set; }
        public string BarcodeNameTw { get; set; }
        public string BarcodeNameEn { get; set; }
        public string DeliveryTerms { get; set; }
        public string TransitTypeDesc { get; set; }
        public string TransitTypeDescEn { get; set; }
        public int? OrdersVersion { get; set; }
        public int? StyleVersion { get; set; }
        public DateTime? PCLLastUpdateTime { get; set; }
        public string PCLModifyUserName { get; set; }
        public string Gender { get; set; }
        public string GBSPOReferenceNo { get; set; }
    }
}
