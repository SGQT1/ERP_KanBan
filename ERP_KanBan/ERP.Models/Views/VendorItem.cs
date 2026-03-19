using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class VendorItem
    {
        public decimal Id { get; set; }
        public decimal VendorId { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankAddress { get; set; }
        public decimal MoneyCodeId { get; set; }
        public string MoneyCode { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
    }
}
