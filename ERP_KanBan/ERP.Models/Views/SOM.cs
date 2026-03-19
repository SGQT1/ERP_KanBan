using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class SOM
    {
        public decimal Id { get; set; }
        public decimal ParentId { get; set; }
        public string ParentMaterialName { get; set; }
        public decimal ChildId { get; set; }
        public string ChildMaterialName { get; set; }
        public string ChildMaterialNameEng { get; set; }
        public int SeqNo { get; set; }
        public decimal? Qty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public string ItemGroupCode { get; set; }
        public string ParentGroupCode { get; set; }
    }
}
