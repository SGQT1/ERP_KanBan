using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class OrdersPL
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
        public Guid msrepl_tran_version { get; set; }
        public int CNoFrom { get; set; }
        public decimal? PackingCTNS { get; set; }
        public decimal? PackingNW { get; set; }
        public decimal? PackingGW { get; set; }
        public decimal? PackingMEAS { get; set; }
        public decimal? PackingCBM { get; set; }
    }
}
