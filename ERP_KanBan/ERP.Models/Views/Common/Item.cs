using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views.Common
{
    public class Item
    {
        public int? Id { get; set; }
        public string ItemNo { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public decimal LocaleId { get; set; }
        public decimal Type { get; set; }
    }
    public class ExtentionItem
    {
        public int? Field1 { get; set; }
        public int? Field2 { get; set; }
        public int[] Field3 { get; set; }
        public int[] Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string[] Field7 { get; set; }
        public string[] Field8 { get; set; }
        public bool Field9 { get; set; }
        public bool Field10 { get; set; }
    }
}
