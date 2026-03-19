using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class UserGroup
    {
        public User User {get;set;}
        public IEnumerable<string> Permission {get;set;}
        public IEnumerable<int> Role {get;set;}
    }
}
