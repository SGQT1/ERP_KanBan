using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackLabelEdition
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal PackLabelId { get; set; }
        public string OrderNo { get; set; }
        public string Edition { get; set; }
        public bool IsEdition { get; set; }
    }
}
