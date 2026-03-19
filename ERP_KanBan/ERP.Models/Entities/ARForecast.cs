using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class ARForecast
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BrandTw { get; set; }
        public string CompanyNo { get; set; }
        public DateTime CSDFrom { get; set; }
        public DateTime CSDEnd { get; set; }
        public string DollarNameTw { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal FactoryAmount { get; set; }
        public decimal Qty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
