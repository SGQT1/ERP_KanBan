using System;
using System.Collections.Generic;


namespace ERP.Models.Entities
{
    public partial class LabelArticle
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ArticleId { get; set; }
        public string ArticleNo { get; set; }
        public string ArticleName { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string LabelArticle01 { get; set; }
        public string LabelArticle01PhotoURL { get; set; }
        public string LabelArticle02 { get; set; }
        public string LabelArticle02PhotoURL { get; set; }
        public string LabelArticle03 { get; set; }
        public string LabelArticle03PhotoURL { get; set; }
        public string LabelArticle04 { get; set; }
        public string LabelArticle04PhotoURL { get; set; }
        public string LabelArticle05 { get; set; }
        public string LabelArticle05PhotoURL { get; set; }
        public string LabelArticle06 { get; set; }
        public string LabelArticle06PhotoURL { get; set; }
        public string LabelArticle07 { get; set; }
        public string LabelArticle07PhotoURL { get; set; }
        public string LabelArticle08 { get; set; }
        public string LabelArticle08PhotoURL { get; set; }
        public string LabelArticle09 { get; set; }
        public string LabelArticle09PhotoURL { get; set; }
    }
}