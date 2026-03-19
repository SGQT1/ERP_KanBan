using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class OutsoleTC
    {
        public decimal Id { get; set; }
        public string OutsoleNo { get; set; }
        public int? TCType { get; set; }
        public decimal? CompanyId { get; set; }
        public string Company { get; set; }
        public DateTime? Date { get; set; }
        public string VendorShortNameTw { get; set; }
        public string OrderNo { get; set; }
        public int Qty { get; set; }
        public string DollarName { get; set; }
        public decimal TC { get; set; }
        public decimal SubTC { get; set; }
        public decimal LocaleId { get; set; }
        public bool IsClose { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Confirmer { get; set; }
        public DateTime? ConfirmDate { get; set; }
    }
}
