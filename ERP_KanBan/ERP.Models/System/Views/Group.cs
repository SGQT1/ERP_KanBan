using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Group
    {

        public int Id { get; set; }
        public int? ParentGroupId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool Validate { get; set; }
        public string GroupCode { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
    }
}
