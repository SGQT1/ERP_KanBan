using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class Menu
    {
        public string Id { get; set; }
        public string ParentMenuId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int? ItemSort { get; set; }
        public bool? Validate { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public string MenuCode { get; set; }
    }
}
