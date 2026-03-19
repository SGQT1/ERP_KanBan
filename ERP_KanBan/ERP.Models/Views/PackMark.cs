using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackMark
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public string Edition { get; set; }
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
        public string LacosteTitle { get; set; }

        public string MarkPhoto { get; set; }
        public string SideMarkPhoto { get; set; }
        public string Add1Photo { get; set; }
        public string RefOrderNo { get; set; }
        public decimal RefCompanyId { get; set; }
        public string RefCompany { get; set; }
        public decimal RefCustomerId { get; set; }
        public string RefCustomer { get; set; }
        public decimal RefBrandCodeId { get; set; }
        public string RefBrand { get; set; }
        public string RefStyleNo { get; set; }
        public string RefShoeName { get; set; }
        public decimal RefQty { get; set; }
    }
}
