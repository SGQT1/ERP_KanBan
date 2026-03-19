using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class ChemPOSizeRun
    {
        // FOR IDM CHEM
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string Customer { get; set; }
        public string Vendor { get; set; }
        public int VendorId { get; set; }
        public string OrderNo { get; set; }
        public string BatchNo { get; set; }
        public int BatchSeq { get; set; }
        public string PONo { get; set; }
        public int? POTypeId { get; set; }
        public string POType { get; set; }
        public string Component { get; set; }
        public string OutsoleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public string Color { get; set; }
        public Decimal? OrderQty { get; set; }
        public Decimal? PurchaseQty { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? PODate { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Unit { get; set; }
        public string Material { get; set; }
        public string MaterialEng { get; set; }
        public int? MaterialId { get; set; }
        public Decimal? Price { get; set; }
        public Decimal? Amount { get; set; }
        public string Currency { get; set; }
        public string Purchaser { get; set; }
        public string Barcode { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string Company { get; set; }
        public string Brand { get; set; }
        public Decimal? CompanyId { get; set; }
        public Decimal? SJ190 { get; set; }
        public Decimal? SJ200 { get; set; }
        public Decimal? SJ210 { get; set; }
        public Decimal? SJ220 { get; set; }
        public Decimal? SJ230 { get; set; }
        public Decimal? SJ240 { get; set; }
        public Decimal? SJ250 { get; set; }
        public Decimal? SJ260 { get; set; }
        public Decimal? SJ270 { get; set; }
        public Decimal? SJ010 { get; set; }
        public Decimal? SJ015 { get; set; }
        public Decimal? SJ020 { get; set; }
        public Decimal? SJ025 { get; set; }
        public Decimal? SJ030 { get; set; }
        public Decimal? SJ035 { get; set; }
        public Decimal? SJ040 { get; set; }
        public Decimal? SJ045 { get; set; }
        public Decimal? SJ050 { get; set; }
        public Decimal? SJ055 { get; set; }
        public Decimal? SJ060 { get; set; }
        public Decimal? SJ065 { get; set; }
        public Decimal? SJ070 { get; set; }
        public Decimal? SJ075 { get; set; }
        public Decimal? SJ080 { get; set; }
        public Decimal? SJ085 { get; set; }
        public Decimal? SJ090 { get; set; }
        public Decimal? SJ095 { get; set; }
        public Decimal? SJ100 { get; set; }
        public Decimal? SJ105 { get; set; }
        public Decimal? SJ110 { get; set; }
        public Decimal? SJ115 { get; set; }
        public Decimal? SJ120 { get; set; }
        public Decimal? SJ125 { get; set; }
        public Decimal? SJ130 { get; set; }
        public Decimal? SJ135 { get; set; }
        public Decimal? SJ140 { get; set; }
        public Decimal? SJ145 { get; set; }
        public Decimal? SJ150 { get; set; }
        public Decimal? SJ155 { get; set; }
        public Decimal? S010 { get; set; }
        public Decimal? S015 { get; set; }
        public Decimal? S020 { get; set; }
        public Decimal? S025 { get; set; }
        public Decimal? S030 { get; set; }
        public Decimal? S035 { get; set; }
        public Decimal? S040 { get; set; }
        public Decimal? S045 { get; set; }
        public Decimal? S050 { get; set; }
        public Decimal? S055 { get; set; }
        public Decimal? S060 { get; set; }
        public Decimal? S065 { get; set; }
        public Decimal? S070 { get; set; }
        public Decimal? S075 { get; set; }
        public Decimal? S080 { get; set; }
        public Decimal? S085 { get; set; }
        public Decimal? S090 { get; set; }
        public Decimal? S095 { get; set; }
        public Decimal? S100 { get; set; }
        public Decimal? S105 { get; set; }
        public Decimal? S110 { get; set; }
        public Decimal? S115 { get; set; }
        public Decimal? S120 { get; set; }
        public Decimal? S125 { get; set; }
        public Decimal? S130 { get; set; }
        public Decimal? S135 { get; set; }
        public Decimal? S140 { get; set; }
        public Decimal? S145 { get; set; }
        public Decimal? S150 { get; set; }
        public Decimal? S155 { get; set; }
        public Decimal? S160 { get; set; }
        public Decimal? S165 { get; set; }
        public Decimal? S170 { get; set; }
        public Decimal? S175 { get; set; }
        public Decimal? S180 { get; set; }
        public Decimal? S185 { get; set; }
 
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateUser { get; set; }

        // public Decimal? SJ010 { get; set; }
        // public Decimal? SJ015 { get; set; }
        // public Decimal? SJ020 { get; set; }
        // public Decimal? SJ025 { get; set; }
        // public Decimal? SJ030 { get; set; }
        // public Decimal? SJ035 { get; set; }
        // public Decimal? SJ040 { get; set; }
        // public Decimal? SJ045 { get; set; }
        // public Decimal? SJ050 { get; set; }
        // public Decimal? SJ055 { get; set; }
        // public Decimal? SJ060 { get; set; }
        // public Decimal? SJ065 { get; set; }
        // public Decimal? SJ070 { get; set; }
        // public Decimal? SJ075 { get; set; }
        // public Decimal? SJ080 { get; set; }
        // public Decimal? SJ085 { get; set; }
        // public Decimal? SJ090 { get; set; }
        // public Decimal? SJ095 { get; set; }
        // public Decimal? SJ100 { get; set; }
        // public Decimal? SJ105 { get; set; }
        // public Decimal? SJ110 { get; set; }
        // public Decimal? SJ115 { get; set; }
        // public Decimal? SJ120 { get; set; }
        // public Decimal? SJ125 { get; set; }
        // public Decimal? SJ130 { get; set; }
        // public Decimal? SJ135 { get; set; }
        // public Decimal? SJ140 { get; set; }
        // public Decimal? SJ145 { get; set; }
        // public Decimal? SJ150 { get; set; }
        // public Decimal? SJ155 { get; set; }
        // public Decimal? S010 { get; set; }
        // public Decimal? S015 { get; set; }
        // public Decimal? S020 { get; set; }
        // public Decimal? S025 { get; set; }
        // public Decimal? S125 { get; set; }
        // public string S010J { get; set; }
        // public string S015J { get; set; }
        // public string S020J { get; set; }
        // public string S025J { get; set; }
        // public string S030J { get; set; }
        // public string S035J { get; set; }
        // public string S040J { get; set; }
        // public string S045J { get; set; }
        // public string S050J { get; set; }
        // public string S055J { get; set; }
        // public string S060J { get; set; }
        // public string S065J { get; set; }
        // public string S070J { get; set; }
        // public string S075J { get; set; }
        // public string S080J { get; set; }
        // public string S085J { get; set; }
        // public string S090J { get; set; }
        // public string S095J { get; set; }
        // public string S100J { get; set; }
        // public string S105J { get; set; }
        // public string S110J { get; set; }
        // public string S115J { get; set; }
        // public string S120J { get; set; }
        // public string S125J { get; set; }
        // public string S130J { get; set; }
        // public string S135J { get; set; }
        // public string S140J { get; set; }
        // public string S145J { get; set; }
        // public string S150J { get; set; }
        // public string S155J { get; set; }
        // public string S010 { get; set; }
        // public string S015 { get; set; }
        // public string S020 { get; set; }
        // public string S025 { get; set; }
        // public string S030 { get; set; }
        // public string S035 { get; set; }
        // public string S040 { get; set; }
        // public string S045 { get; set; }
        // public string S050 { get; set; }
        // public string S055 { get; set; }
        // public string S060 { get; set; }
        // public string S065 { get; set; }
        // public string S070 { get; set; }
        // public string S075 { get; set; }
        // public string S080 { get; set; }
        // public string S085 { get; set; }
        // public string S090 { get; set; }
        // public string S095 { get; set; }
        // public string S100 { get; set; }
        // public string S105 { get; set; }
        // public string S110 { get; set; }
        // public string S115 { get; set; }
        // public string S120 { get; set; }
        // public string S125 { get; set; }
        // public string S130 { get; set; }
        // public string S135 { get; set; }
        // public string S140 { get; set; }
        // public string S145 { get; set; }
        // public string S150 { get; set; }
        // public string S155 { get; set; }
        // public Decimal? S135 { get; set; }
        // public Decimal? S140 { get; set; }
        // public Decimal? S145 { get; set; }
        // public Decimal? S150 { get; set; }
        // public Decimal? S155 { get; set; }
        // public Decimal? S160 { get; set; }
        // public Decimal? S165 { get; set; }
        // public Decimal? S170 { get; set; }
        // public Decimal? S175 { get; set; }
        // public Decimal? S180 { get; set; }
        // public Decimal? S185 { get; set; }
        // public Decimal? S190 { get; set; }
        // public Decimal? S195 { get; set; }
        // public Decimal? S200 { get; set; }
        // public Decimal? S205 { get; set; }
        // public Decimal? S210 { get; set; }
        // public Decimal? S215 { get; set; }
        // public Decimal? S220 { get; set; }
        // public Decimal? S225 { get; set; }
        // public Decimal? S230 { get; set; }
        // public Decimal? S235 { get; set; }
        // public Decimal? S240 { get; set; }
        // public Decimal? S245 { get; set; }
        // public Decimal? S250 { get; set; }
        // public Decimal? S255 { get; set; }
        // public Decimal? S260 { get; set; }
        // public Decimal? S265 { get; set; }
        // public Decimal? S270 { get; set; }
        // public Decimal? S275 { get; set; }
        // public Decimal? S280 { get; set; }
        // public Decimal? S285 { get; set; }
        // public Decimal? S290 { get; set; }
        // public Decimal? S295 { get; set; }
        // public Decimal? S300 { get; set; }
        // public Decimal? S305 { get; set; }
        // public Decimal? S310 { get; set; }
        // public Decimal? S315 { get; set; }
        // public Decimal? S320 { get; set; }
        // public Decimal? S325 { get; set; }
        // public Decimal? S330 { get; set; }
        // public Decimal? S335 { get; set; }
        // public Decimal? S340 { get; set; }
        // public Decimal? S345 { get; set; }
        // public Decimal? S350 { get; set; }
        // public Decimal? S355 { get; set; }
        // public Decimal? S360 { get; set; }
        // public Decimal? S365 { get; set; }
        // public Decimal? S370 { get; set; }
        // public Decimal? S375 { get; set; }
        // public Decimal? S380 { get; set; }
        // public Decimal? S385 { get; set; }
        // public Decimal? S390 { get; set; }
        // public Decimal? S395 { get; set; }
        // public Decimal? S400 { get; set; }
        // public Decimal? S405 { get; set; }
    }
}