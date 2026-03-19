using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class MPSOutsourceQuotationGroup
    {
        public MPSProcedureGroup MPSProcedureGroup { get; set; }
        public IEnumerable<MPSProcedureGroupItem> MPSProcedureGroupItem { get; set; }
        public IEnumerable<MPSProcedureQuot> MPSProcedureQuot { get; set; }
    }
}
