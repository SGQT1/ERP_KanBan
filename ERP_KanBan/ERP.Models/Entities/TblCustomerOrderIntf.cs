using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class TblCustomerOrderIntf
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string fid { get; set; }
        public string fcollaborator { get; set; }
        public string fdesc { get; set; }
        public string fcustDemand { get; set; }
        public decimal fstatus { get; set; }
        public string forderDate { get; set; }
        public decimal type { get; set; }
        public string fbuilder { get; set; }
        public string fbDate { get; set; }
        public string freviser { get; set; }
        public string frDate { get; set; }
        public string FQTY { get; set; }
        public string Fcolor { get; set; }
        public string Fcomkey { get; set; }
        public string Ftype { get; set; }
    }
}
