using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OutsourcePOItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POId { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public string PONo { get; set; }
        public string DisplayItem { get; set; }
        public string DisplayItemEng { get; set; }
        public decimal Qty { get; set; }
    }
}
