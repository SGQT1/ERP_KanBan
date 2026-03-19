using System;
using System.Collections.Generic;

namespace ERP.Models.Entities
{
    public partial class MonthMaterialStockIOSch
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal IOMonth { get; set; }
        public DateTime? CalTime { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal Records { get; set; }
    }
}
