using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views
{
    public partial class MPSPlanItemSize
    {
        public decimal Id { get; set; }
        public decimal MPSLiveItemId { get; set; }
        public decimal MPSOrdersItemId { get; set; }
        public decimal SubQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? SeqId { get; set; }

        public decimal MPSOrdersId { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public string DisplaySize { get; set; }
        public string KnifeDisplaySize { get; set; }
        public string OutsoleDisplaySize { get; set; }
        public string LastDisplaySize { get; set; }
        public string ShellDisplaySize { get; set; }
        public string Other1SizeDesc { get; set; }
        public string Other2SizeDesc { get; set; }
        public decimal SumQty { get; set; }
    }
}
