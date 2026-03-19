using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MPSOutsourceProcessGroup
    {
        public MPSProcedureGroup MPSProcedureGroup { get; set; }
        public IEnumerable<MPSProcedureGroupItem> MPSProcedureGroupItem { get; set; }
        public IEnumerable<MPSProcedure> MPSProcedure { get; set; }
    }
}
