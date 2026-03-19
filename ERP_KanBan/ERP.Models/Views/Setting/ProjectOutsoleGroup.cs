using System.Collections.Generic;

namespace ERP.Models.Views.Setting
{
    public class ProjectOutsoleGroup : ProjectOutsole
    {
        public string OwnerCompany { get; set; }
        public string OwnerCustomer { get; set; }
        public string MoneyCode { get; set; }
        public string Locale { get; set; }
        public string Vendor { get; set; }
        public string MDVendor { get; set; }
        public string EVAVendor { get; set; }
        public IEnumerable<ProjectOutsoleItem> Items { get; set; }
    }
}