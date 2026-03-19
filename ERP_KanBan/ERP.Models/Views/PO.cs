using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public DateTime PODate { get; set; }
        public string BatchNo { get; set; }
        public int SeqId { get; set; }
        public decimal VendorId { get; set; }
        public int IsShowSizeRun { get; set; }
        public int ShowAlternateType { get; set; }
        public DateTime VendorETD { get; set; }
        public int IsAllowPartial { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string PhotoURLDescTw { get; set; }
        public string PhotoURL { get; set; }
        public string POTeam { get; set; }
        public string Vendor { get; set; }
        public string PONo { get; set; }
    }
}