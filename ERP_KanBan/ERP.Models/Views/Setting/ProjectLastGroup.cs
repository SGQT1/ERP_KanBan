using System;
using System.Collections.Generic;

namespace ERP.Models.Views.Setting
{
    public class ProjectLastGroup : ProjectLast
    {
        public string OwnerCompany { get; set; }
        public string OwnerCustomer { get; set; }
        public string MoneyCode { get; set; }
        public string Locale { get; set; }
        public IEnumerable<ProjectLastItem> Items { get; set; }
    }
}