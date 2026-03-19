using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_SALE
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal OrdersId { get; set; }
        public decimal SaleQty { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal ToolingCost { get; set; }
        public decimal OtherCharge { get; set; }
        public string OtherChargeDesc { get; set; }
        public decimal? ARId { get; set; }
        public DateTime ARDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal FeedbackFund { get; set; }
        public decimal ShippingId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? APTFundId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string CloseDateYM { get; set; }
        public string CloseDateYear { get; set; }
        public int? CloseDateWeek { get; set; }
        public string DollarNameTw { get; set; }
        public decimal? ARTotal { get; set; }
        public decimal ShareOtherCost { get; set; }
        public string CompanyNo { get; set; }
        public string BrandNameTw { get; set; }
    }
}
