using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class TblDemandSizeElemIntf
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string fdemand { get; set; }
        public string fsize { get; set; }
        public decimal fqty { get; set; }
        public string fcomKey { get; set; }
        public string fcolor { get; set; }
        public string fshoewidth { get; set; }
        public string fLSize { get; set; }
        public string fCustBarCode { get; set; }
        public string fbuilder { get; set; }
        public string fbDate { get; set; }
        public string freviser { get; set; }
        public string frDate { get; set; }
    }
}
