using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class StyleMaterial
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public string ArticleNo { get; set; }
        public string ArticleName { get; set; }
        public string StyleNo { get; set; }

        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string KnifeNo { get; set; }
        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        public bool IsSpecial { get; set; }
        public int EnableMaterial { get; set; }
        public string Season { get; set; }
        public string Brand { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public string CategoryCode { get; set; }
    }

}
