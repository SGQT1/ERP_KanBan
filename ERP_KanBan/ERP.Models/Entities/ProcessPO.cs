using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ProcessPO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal POId { get; set; }
        public decimal POItemId { get; set; }
        public decimal StockIOId { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal StockDollarCodeId { get; set; }
        public string OrderNo { get; set; }
        public decimal? OPCount { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual PO PO { get; set; }
        public virtual POItem POItem { get; set; }
        public virtual StockIO StockIO { get; set; }
    }
}
