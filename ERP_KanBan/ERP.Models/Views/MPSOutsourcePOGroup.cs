using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSOutsourcePOGroup
    {
        public MPSProcedurePO MPSProcedurePO { get; set; }
        public IEnumerable<MPSProcedurePOSize> MPSProcedurePOSize { get; set; }
        public IEnumerable<MPSProcedurePOItem> MPSProcedurePOItem { get; set; }

        public IEnumerable<MPSProcedureGroup> MPSProcedureGroup { get; set; }
        public IEnumerable<MPSProcedureGroupItem> MPSProcedureGroupItem { get; set; }
        public IEnumerable<MPSProcedureQuot> MPSProcedureQuot{get;set;}
    }
}
