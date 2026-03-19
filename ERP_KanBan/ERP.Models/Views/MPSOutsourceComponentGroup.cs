using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MPSOutsourceComponentGroup
    {
        public MPSStyle MPSStyle { get; set; }
        public IEnumerable<MPSStyleItem> MPSStyleItem { get; set; }
        public IEnumerable<MPSProcedure> MPSProcedure { get; set; }
    }
}
