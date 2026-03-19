using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CTNWeightManagement
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal InRatio { get; set; }
        public decimal? InWeight { get; set; }
        public int? InPriority { get; set; }
        public int? InDelayHour { get; set; }
        public int? InIgnore { get; set; }
        public decimal OutRatio { get; set; }
        public decimal? OutWeight { get; set; }
        public int? OutPriority { get; set; }
        public int? OutDelayHour { get; set; }
        public int? OutIgnore { get; set; }
        public int? DefaultDevice { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string DefaultPath { get; set; }
    }
}
