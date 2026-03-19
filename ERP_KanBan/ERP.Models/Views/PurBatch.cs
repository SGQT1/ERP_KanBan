using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class PurBatch
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BatchNo { get; set; }
        public DateTime BatchDate { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal RefLocaleId { get; set; }
    }
}
