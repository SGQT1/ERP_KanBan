using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CTNLabelStockReject
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string LabelCode { get; set; }
        public decimal? SubGrossWeight { get; set; }
        public DateTime StockRejectTime { get; set; }
        public DateTime? StockRejectAdjTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual CTNLabelItem L { get; set; }
    }
}
