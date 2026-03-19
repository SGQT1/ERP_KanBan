using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class InvoiceGroup
    {
        public Invoice Invoice { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItem { get; set; }
        public IEnumerable<Payment> Payment { get; set; }
    }
}
