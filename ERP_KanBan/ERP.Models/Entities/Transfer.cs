using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Transfer
    {
        public Transfer()
        {
            TransferItem = new HashSet<TransferItem>();
        }

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
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<TransferItem> TransferItem { get; set; }
    }
}
