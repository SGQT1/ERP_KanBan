using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSOutsourcePOBatchGroup
    {
        public IEnumerable<MPSProcedurePO>MPSProcedurePO  { get; set; }
        public IEnumerable<MPSProcedureQuot> MPSProcedureQuot{get;set;}
    }
}
