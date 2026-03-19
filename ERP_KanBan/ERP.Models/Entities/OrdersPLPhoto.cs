using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersPLPhoto
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
        public Guid msrepl_tran_version { get; set; }
    }
}
