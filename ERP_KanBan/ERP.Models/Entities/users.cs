using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class users
    {
        public string ID { get; set; }
        public string PWD { get; set; }
        public string AGENT { get; set; }
        public string EMAIL { get; set; }
        public string TEMP1 { get; set; }
        public string NameTw { get; set; }
        public string NameEn { get; set; }
        public string POTeam { get; set; }
        public string SIGN { get; set; }
        public bool? Validate { get; set; }
    }
}
