using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class OrdersMaterialCost
    {

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CompanyId { get; set; }
        public string Company { get; set; }
        public bool IsClosed { get; set; }
        public string Brand { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string Customer { get; set; }
        public decimal OrderQty { get; set; }
        public DateTime ETD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime CSD { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string OrderNo { get; set; }
        public decimal? IOMonth { get; set; }
        public decimal? IOAmount { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int CostType { get; set; }
    }
}