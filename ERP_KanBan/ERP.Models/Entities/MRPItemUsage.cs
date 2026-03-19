using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class MRPItemUsage
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public decimal PartId { get; set; }
        public decimal MaterialId { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string SizeUsage { get; set; }
        public long? MRPVersion { get; set; }
    }

    public class MRPItemSizeUsage
    {
        public int A { get; set; }
        public decimal B { get; set; }
        public string C { get; set; }
        public decimal D { get; set; }
        public string E { get; set; }
        public decimal F { get; set; }
        public decimal G { get; set; }
    }
}
