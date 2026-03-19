using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class APTCost
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BrandTw { get; set; }
        public int MoldType { get; set; }
        public string MoldNo { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal PerFee { get; set; }
        public string DollarNameTw { get; set; }
        public int Qty { get; set; }
        public decimal PayExchangeRate { get; set; }
        public decimal DividePairs { get; set; }
        public decimal PerPairFee { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
