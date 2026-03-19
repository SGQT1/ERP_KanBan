using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class BondIO
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BondId { get; set; }
        public int IOType { get; set; }
        public int ImportType { get; set; }
        public string IOPortName { get; set; }
        public DateTime IODate { get; set; }
        public DateTime RequestDate { get; set; }
        public string TransportType { get; set; }
        public string TransportName { get; set; }
        public string PreCodingNo { get; set; }
        public string CustomNo { get; set; }
        public string BillNo { get; set; }
        public string TradeTypeName { get; set; }
        public string TaxationType { get; set; }
        public decimal? TaxRate { get; set; }
        public string LicenseNo { get; set; }
        public string IORegion { get; set; }
        public string IOPort { get; set; }
        public string IOPlace { get; set; }
        public string ApprovedNo { get; set; }
        public string TradeTerm { get; set; }
        public decimal Freight { get; set; }
        public decimal Insurance { get; set; }
        public decimal? OtherFee { get; set; }
        public int CartonCount { get; set; }
        public string PackingType { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public string MassPackNo { get; set; }
        public string AttachedPapers { get; set; }
        public string ProductOrgName { get; set; }
        public string Mark { get; set; }
        public string Remark { get; set; }
        public string TaxationRemark { get; set; }
        public int TaxationCount { get; set; }
        public decimal TaxationAmount { get; set; }
        public string CustomOfficer { get; set; }
        public string CustomUnitName { get; set; }
        public string Declarer { get; set; }
        public string OrgAddress { get; set; }
        public string OrgZipCode { get; set; }
        public string OrgTelNo { get; set; }
        public DateTime? FillOutDate { get; set; }
        public DateTime? CustomReleaseDate { get; set; }
        public decimal? VendorId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid msrepl_tran_version { get; set; }
    }
}
