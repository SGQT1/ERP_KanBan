using System;
using System.Collections.Generic;
using System.Text;
using ERP.Models.Views.View;

namespace ERP.Models.Views
{
    public class BondMRPGroup
    {
        public BondMRP BondMRP { get;set; }
        public IEnumerable<BondMRPItem> BondMRPItem { get;set; }
    }
}