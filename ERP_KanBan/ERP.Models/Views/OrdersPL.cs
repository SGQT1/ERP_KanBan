using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersPL
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public decimal? RefOrdersId { get; set; }
        public string OrderNo { get; set; }
        public string Edition { get; set; }
        public string SizeCountryNameTw { get; set; }
        public string MappingSizeCountryNameTw { get; set; }
        public decimal? PackingQty { get; set; }
        public int PackingType { get; set; }
        public string PackingTypeDesc { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid MsreplTranVersion { get; set; }
        public int CNoFrom { get; set; }
    }
}
