using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CBDSeasonStyleItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string Season { get; set; }
        public string StyleNo { get; set; }
        public string PartNo { get; set; }
        public string PartNameTw { get; set; }
        public string SampleMaterialNameTw { get; set; }
        public decimal? SampleUsage { get; set; }
        public decimal? SamplePrice { get; set; }
        public decimal? SampleUSDPrice { get; set; }
        public decimal? MaterialId { get; set; }
        public string MaterialNameTw { get; set; }
        public string UnitNameTw { get; set; }
        public int? PieceOfPair { get; set; }
        public decimal? UnitUsage { get; set; }
        public decimal? ProductionPrice { get; set; }
        public string ProductionDollar { get; set; }
        public decimal? USDExchangeRate { get; set; }
        public decimal? ProductionUSDPrice { get; set; }
    }
}
