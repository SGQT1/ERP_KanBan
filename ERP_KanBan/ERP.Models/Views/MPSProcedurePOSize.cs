using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSProcedurePOSize
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal MpsProcedurePOId { get; set; }
        public string DisplaySize { get; set; }
        public int SeqId { get; set; }
        public decimal SubQty { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int? PayType { get; set; }
        public decimal ArticleInnerSize { get; set; }
    }
}
