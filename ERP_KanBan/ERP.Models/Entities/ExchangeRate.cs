using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ExchangeRate
    {
        public decimal Id { get; set; }
        public DateTime ExchDate { get; set; }
        public decimal CodeId { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public decimal BankingRate { get; set; }
        public decimal CustomRate { get; set; }
        public decimal TransCodeId { get; set; }
        public string TransNameTw { get; set; }
        public string TransNameEn { get; set; }
        public decimal ReversedBankingRate { get; set; }
        public decimal ReversedCustomRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
