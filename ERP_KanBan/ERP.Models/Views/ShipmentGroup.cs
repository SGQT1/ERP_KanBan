using System;
using System.Collections.Generic;
using System.Text;
using ERP.Models.Views;

namespace ERP.Models.Views {
    public class ShipmentGroup {
        public Shipment Shipment { get; set; }
        public IEnumerable<ShipmentItem> ShipmentItem { get; set; }
        public Orders Orders { get; set; }
        public ShipmentSummary ShipmentSummary { get; set; }

        public IEnumerable<Quotation> Quotation { get; set;} 
    }
}