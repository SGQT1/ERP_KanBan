using System;
using System.Collections.Generic;
using System.Text;

using ERP.Models.Views;

namespace ERP.Models.Views
{
    public class OutsoleTCGroup
    {
        public OutsoleTC OutsoleTC { get; set; }
        public IEnumerable<OutsoleTC> OutsoleTCHistory { get; set; }
    }
}