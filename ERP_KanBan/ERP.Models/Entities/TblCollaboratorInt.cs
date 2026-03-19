using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class TblCollaboratorInt
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string fid { get; set; }
        public string fname { get; set; }
        public string fowner { get; set; }
        public string finvoice { get; set; }
        public string fwww { get; set; }
        public decimal fisVendor { get; set; }
        public decimal fisCustomer { get; set; }
        public decimal fsSubcontractor { get; set; }
        public string fbuilder { get; set; }
        public string fbDate { get; set; }
        public string freviser { get; set; }
        public string frDate { get; set; }
    }
}
