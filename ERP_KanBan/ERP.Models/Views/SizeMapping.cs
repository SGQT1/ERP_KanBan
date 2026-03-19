using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class SizeMapping
    {
        public decimal Id { get; set; }
        public string CodeType { get; set; }
        public int CodeNo { get; set; }
        public string NameTW { get; set; }
        public string NameEng { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ReferenceCodeNo { get; set; }
        public decimal LocaleId { get; set; }
    }
}
