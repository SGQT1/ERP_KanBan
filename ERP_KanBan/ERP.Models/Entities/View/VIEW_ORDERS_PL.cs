using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_ORDERS_PL
    {
        public decimal Id { get; set; }
        public string OrderNo { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrderQty { get; set; }
        public decimal CompanyId { get; set; }
        public decimal StyleId { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public DateTime CSD { get; set; }
        public DateTime ETD { get; set; }
        public int FilterId { get; set; }
        public int Status { get; set; }
        public int ProductType { get; set; }
        public int OrderType { get; set; }
        public int OrderVersion { get; set; }
        public string Edition { get; set; }
        public decimal PLLocaleId { get; set; }
        public string ArticleNo { get; set; }
        public string MarkPhotoURL { get; set; }
        public string MarkDesc { get; set; }
        public string SideMarkPhotoURL { get; set; }
        public string SideMarkDesc { get; set; }
        public string Add1PhotoURL { get; set; }
        public string Add1Desc { get; set; }
        public string Remark { get; set; }
        public string MarkTitle { get; set; }
        public string SubMarkTitle { get; set; }
        public string Add1Title { get; set; }
        public string DeliveryAddress { get; set; }
        public string CustomerOrderNo { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string PackingDescTW { get; set; }
        public string PackingDescEng { get; set; }
        public string ColorDesc { get; set; }
        public int PackingType { get; set; }
        public string InsockLabel { get; set; }
        public string SafeCode { get; set; }
        public string GBSPOReferenceNo { get; set; }
        public int? TransitType { get; set; }
        public decimal CustomerId { get; set; }
        public string Mark { get; set; }
        public string SideMark { get; set; }
        public string TransitTypeDescTw { get; set; }
        public string TransitTypeDescEn { get; set; }
        public string LacosteTitle { get; set; }
        public string PLId { get; set; }
    }
}
