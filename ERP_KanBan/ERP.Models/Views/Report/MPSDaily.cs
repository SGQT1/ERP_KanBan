using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Views.Report
{
    public partial class MPSDaily
    {
        public decimal Id { get; set;}
        public decimal LocaleId { get; set;}
        public string Company { get; set; }
        public string DailyMode { get; set; }
        public string DailyNo { get; set; }
        public string OrgUnit { get; set; }
        public string PrintTime { get; set; }
        public string PrintBy { get; set; }
        public string DailyDate { get; set; }
        public string FinishedDate { get; set; }
        public string Unit { get; set; }
        public string MaterialNameTw { get; set; }
        public string OrderNo { get; set; }
        public string CSD { get; set; }
        public string StyleNo { get; set; }
        public string DailyType { get; set; }
        public string DailyTimes { get; set; }
        public int PrintCount { get; set; }
        public decimal OrderQty { get; set; }
        public decimal Mulit { get; set; }
        public decimal SubUsage { get; set; }
        public decimal SubQty { get; set; }
        public List<MPSDailyItem> MPSDailyItem { get; set; }
        public List<MPSDailyPart> MPSDailyPart { get; set; }
    }
    public partial class MPSDailyPart
    {
        public string PartNameTw { get; set; }
        public int? PieceOfPair { get; set; }
        public string RefKnifeNo { get; set; }
    }
    public partial class MPSDailyItem
    {
        public string Size { get; set; }
        public decimal Qty { get; set; }
        public decimal Usage { get; set; }
    }
}
