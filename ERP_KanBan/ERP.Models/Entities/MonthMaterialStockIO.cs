using System;
using System.Collections.Generic;

namespace ERP.Models.Entities
{
    public partial class MonthMaterialStockIO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public DateTime MaxUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
