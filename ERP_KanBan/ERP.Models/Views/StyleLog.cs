using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class StyleLog
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal StyleId { get; set; }
        public decimal PartId { get; set; }
        public decimal ArticlePartId { get; set; }
        public decimal MaterialId { get; set; }
        public string TransDesc { get; set; }
        public string Remark { get; set; }
        public string MaterialNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string ArticleNo { get; set; }
        public string StyleNo { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string PartNameEn { get; set; }
        public string MaterialNameEn { get; set; }
        public string Brand { get; set; }
    }
}
