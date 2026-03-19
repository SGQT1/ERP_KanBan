using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class ReceivedLogAdd
    {
        public decimal ReceivedLogId { get; set; }
        public decimal LocaleId { get; set; }
        public string RefPONo { get; set; }
        public int Type { get; set; }
        public decimal MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public decimal? ParentMaterialId { get; set; }
        public string ParentMaterialNameTw { get; set; }
        public decimal PCLUnitCodeId { get; set; }
        public string PCLUnitNameTw { get; set; }
        public decimal PurUnitCodeId { get; set; }
        public string PurUnitNameTw { get; set; }
        public decimal PayQty { get; set; }
        public decimal FreeQty { get; set; }
        public decimal PurDollarCodeId { get; set; }
        public string PurDollarNameTw { get; set; }
        public decimal? StockDollarCodeId { get; set; }
        public string StockDollarNameTw { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string CloseMonth { get; set; }
    }
}
