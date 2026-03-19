using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class QuotationGroup
    {
        public Quotation Quotation { get; set; }
        public IEnumerable<Quotation> QuotationHistory { get; set; }
    }
}