using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class ExchangeRate
    {
        public decimal Id { get; set; }
        public DateTime ExchDate { get; set; }
        public decimal CodeId { get; set; }
        public string CurrencyTw { get; set; }
        public string CurrencyEn { get; set; }
        public decimal BankingRate { get; set; }
        public decimal CustomRate { get; set; }
        public decimal TransCodeId { get; set; }
        public string TransCurrencyTw { get; set; }
        public string TransCurrencyEn { get; set; }
        public decimal ReversedBankingRate { get; set; }
        public decimal ReversedCustomRate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
