using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackMapping
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string WeightUnitNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
