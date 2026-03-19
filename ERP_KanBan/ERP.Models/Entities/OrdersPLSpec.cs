using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersPLSpec
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public string Edition { get; set; }
        public int Type { get; set; }
        public string RefDisplaySizeBegin { get; set; }
        public string RefDisplaySizeEnd { get; set; }
        public string Spec { get; set; }
        public decimal MEAS { get; set; }
        public decimal Qty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
