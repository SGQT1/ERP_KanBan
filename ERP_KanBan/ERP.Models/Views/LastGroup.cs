using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class LastGroup
    {
        public Last Last { get; set; }
        public IEnumerable<LastItem> LastItem { get; set; }
    }
}
