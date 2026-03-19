using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CBS
    {
        public CBS()
        {
            CBSItemCompare = new HashSet<CBSItemCompare>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string BrandTw { get; set; }
        public DateTime MadeDate { get; set; }
        public string ArticleNo { get; set; }
        public string ColorDesc { get; set; }
        public string WorkOrderNo { get; set; }
        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        public string Factory { get; set; }
        public string SizeRunDesc { get; set; }
        public string SampleSizeDesc { get; set; }
        public string MoldAmortiseOnDesc { get; set; }
        public decimal MoldAmortization { get; set; }
        public string TargetPriceDesc { get; set; }
        public string CFMPriceDesc { get; set; }
        public decimal StandardQuote { get; set; }
        public decimal OurStandardQuote { get; set; }
        public string DollarNameTw { get; set; }
        public decimal ExchangeRate { get; set; }
        public string ChinaProduction { get; set; }
        public string Comments { get; set; }
        public string HeightDesc { get; set; }
        public string Remark { get; set; }
        public string PhotoURL { get; set; }
        public int Status { get; set; }
        public string CompareDesc { get; set; }
        public string RefArticleNo { get; set; }
        public string RefColorDesc { get; set; }
        public string RefSizeRunDesc { get; set; }
        public string RefSampleSizeDesc { get; set; }
        public string RefOutsoleNo { get; set; }
        public string RefLastNo { get; set; }
        public string RefPriceDesc { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual ICollection<CBSItemCompare> CBSItemCompare { get; set; }
    }
}
