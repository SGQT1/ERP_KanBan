using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CBSMaterialQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string Type { get; set; }
        public string NameNo { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public string SpecDesc { get; set; }
        public string ColorDesc { get; set; }
        public string ProcessDesc { get; set; }
        public string Season { get; set; }
        public string BasicQtyDesc { get; set; }
        public string Unit { get; set; }
        public decimal? QuotPriceNTD { get; set; }
        public decimal? QuotPriceUSD { get; set; }
        public string Vendor { get; set; }
        public decimal? PurPrice { get; set; }
        public string PurDollarName { get; set; }
        public DateTime? QuotDate { get; set; }
        public DateTime? TillDate { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
