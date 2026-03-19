using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MaterialSimpleQuot
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal VendorId { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DollarCodeId { get; set; }
        public decimal PayCodeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int QuotType { get; set; }
        public int Enable { get; set; }
        
    }
}
