using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class Transfer
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ContainerNo { get; set; }
        public string ShipmentNo { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Vessel { get; set; }
        public DateTime OBDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public decimal ShippingPortId { get; set; }
        public string ShippingPortName { get; set; }
        public decimal TargetPortId { get; set; }
        public string TargetPortName { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? PaymentLocaleId { get; set; }
        public decimal? UnitPricePercent { get; set; }
        public string Remark { get; set; }
    }
}
