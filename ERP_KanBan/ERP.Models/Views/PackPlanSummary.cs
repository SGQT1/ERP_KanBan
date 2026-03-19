using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    /*
     * replace OrdersPL
     */
    public class PackPlanSummary
    {
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public string OrderNo { get; set; }
         public decimal OrderQty { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public decimal? PackingQtyTotal { get; set; }
    }
}
