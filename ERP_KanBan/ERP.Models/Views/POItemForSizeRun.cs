using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class POItemForSizeRun
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
        public string OutsoleNo { get; set; }
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public Decimal? OrderQty { get; set; }
        public Decimal? PurchaseQty { get; set; }
        public DateTime? CSD { get; set; }
        public DateTime? LCSD { get; set; }
        public DateTime? PODate { get; set; }
        public string Material { get; set; }
        public string MaterialEng { get; set; }
        public int? MaterialId { get; set; }
        public Decimal? Price { get; set; }
        public Decimal? Amount { get; set; }
        public string Purchaser { get; set; }
        public string Barcode { get; set; }
        public string Company { get; set; }
        public string Brand { get; set; }
        public Decimal? CompanyId { get; set; }
        public string Unit { get; set; }
        public decimal? UnitCodeId { get; set; }

        public string LastSizeRun { get; set; }
        public string LastHead { get; set; }

        public string ArticleSizeRun { get; set; }
        public string ArticleHead { get; set; }

        // public string KnifeSizeRun { get; set; }
        // public string KnifeHead { get; set; }

        // public string OutsoleSizeRun { get; set; }
        // public string OutsoleHead { get; set; }

        // public string ShellSizeRun { get; set; }
        // public string ShellHead { get; set; }

        // public string Other1SizeRun { get; set; }
        // public string Other1Head { get; set; }

        // public string Other2SizeRun { get; set; }
        // public string Other2Head { get; set; }
    }
}