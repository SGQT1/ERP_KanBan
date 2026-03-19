using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OutsoleTC
    {
        public decimal Id { get; set; }
        public string OutsoleNo { get; set; }
        public int? TCType { get; set; }
        public decimal? CompanyId { get; set; }
        public DateTime? Date { get; set; }
        public string VendorShortNameTw { get; set; }
        public string OrderNo { get; set; }
        public int Qty { get; set; }
        public string DollarName { get; set; }
        public decimal TC { get; set; }
        public decimal SubTC { get; set; }
        public decimal LocaleId { get; set; }
        public int IsClose { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}
