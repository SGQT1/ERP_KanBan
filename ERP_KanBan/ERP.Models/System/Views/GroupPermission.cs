using System;
using System.Collections.Generic;

namespace ERP.Models.Views {
    public class GroupPermission {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string FunctionId { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
    }

    public class GroupPermissions {
        public int GroupId {get;set;}
        public IEnumerable<string> Permission {get;set;}

    }
}