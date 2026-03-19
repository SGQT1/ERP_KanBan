using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class UserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GroupId { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
    }
}
