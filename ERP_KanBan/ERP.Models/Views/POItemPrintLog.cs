using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class POItemPrintLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefPOItemId { get; set; }
        public decimal RefLocaleId { get; set; }
        public string PrintUserName { get; set; }
        public DateTime PrintTime { get; set; }
    }
}
