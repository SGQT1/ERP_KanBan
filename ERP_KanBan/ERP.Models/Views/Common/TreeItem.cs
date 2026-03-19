using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views.Common {
    public class TreeItem {
        public string Id { get; set; }
        public string ParentMenuId { get; set; }
        public string Name { get; set; }
        public List<TreeItem> Items { get; set; }
        public string URL { get; set; }
        public bool Validate { get; set; }
        public string Code { get; set; }
        public bool IsExpand { get; set; }
        public bool IsCheck { get; set; }
    }
}