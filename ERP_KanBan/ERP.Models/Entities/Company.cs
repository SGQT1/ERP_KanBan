using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Company
    {
        public Company()
        {
            Code = new HashSet<Code>();
            ControlItem = new HashSet<ControlItem>();
            LastLocale = new HashSet<Last>();
            LastOwnerCompany = new HashSet<Last>();
            Material = new HashSet<Material>();
            MpsBOP = new HashSet<MpsBOP>();
            OutsoleItem = new HashSet<OutsoleItem>();
            OutsoleLocale = new HashSet<Outsole>();
            OutsoleOwnerCompany = new HashSet<Outsole>();
            Part = new HashSet<Part>();
            Port = new HashSet<Port>();
            Project = new HashSet<Project>();
            ProjectArticleItem = new HashSet<ProjectArticleItem>();
            ProjectArticlePart = new HashSet<ProjectArticlePart>();
            ProjectArticleSizeUsage = new HashSet<ProjectArticleSizeUsage>();
            SOM = new HashSet<SOM>();
            ShellItem = new HashSet<ShellItem>();
            ShellLocale = new HashSet<Shell>();
            ShellOwnerCompany = new HashSet<Shell>();
            Vendor = new HashSet<Vendor>();
            VendorItem = new HashSet<VendorItem>();
        }

        public decimal Id { get; set; }
        public string CompanyNo { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string ChineseShortName { get; set; }
        public string EnglishShortName { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string ChineseAddress { get; set; }
        public string EnglishAddress { get; set; }
        public string OfficialAddress { get; set; }
        public string UnifiedNo { get; set; }
        public string TaxNo { get; set; }
        public string Owner { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceTitle { get; set; }
        public string WebsiteURL { get; set; }
        public short StockClosedMonth { get; set; }
        public short StockClosedDay { get; set; }
        public short AccountClosedMonth { get; set; }
        public short AccountClosedDay { get; set; }
        public decimal BusinessTaxRate { get; set; }
        public string Remark { get; set; }
        public string AccountDollarNameTw { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public int IgnoreSunday { get; set; }
        public int IgnoreHoliday { get; set; }
        public string Contact { get; set; }
        public string ContactMobileNo { get; set; }
        public string ContactEmail { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public int? Enable { get; set; }
        public int? IsVirtual { get; set; }

        public virtual ICollection<Code> Code { get; set; }
        public virtual ICollection<ControlItem> ControlItem { get; set; }
        public virtual ICollection<Last> LastLocale { get; set; }
        public virtual ICollection<Last> LastOwnerCompany { get; set; }
        public virtual ICollection<Material> Material { get; set; }
        public virtual ICollection<MpsBOP> MpsBOP { get; set; }
        public virtual ICollection<OutsoleItem> OutsoleItem { get; set; }
        public virtual ICollection<Outsole> OutsoleLocale { get; set; }
        public virtual ICollection<Outsole> OutsoleOwnerCompany { get; set; }
        public virtual ICollection<Part> Part { get; set; }
        public virtual ICollection<Port> Port { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ProjectArticleItem> ProjectArticleItem { get; set; }
        public virtual ICollection<ProjectArticlePart> ProjectArticlePart { get; set; }
        public virtual ICollection<ProjectArticleSizeUsage> ProjectArticleSizeUsage { get; set; }
        public virtual ICollection<SOM> SOM { get; set; }
        public virtual ICollection<ShellItem> ShellItem { get; set; }
        public virtual ICollection<Shell> ShellLocale { get; set; }
        public virtual ICollection<Shell> ShellOwnerCompany { get; set; }
        public virtual ICollection<Vendor> Vendor { get; set; }
        public virtual ICollection<VendorItem> VendorItem { get; set; }
    }
}
