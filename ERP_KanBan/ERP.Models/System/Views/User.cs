using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Agent { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string POTeam { get; set; }
        public string Sign { get; set; }
        public string SignPhoto { get; set; }
        public bool? Validate { get; set; }
    }
}
