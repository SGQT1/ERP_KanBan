using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class TblDemandIntf
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string fid { get; set; }
        public string fitemNo { get; set; }
        public decimal fversion { get; set; }
        public string fdesc { get; set; }
        public string fcomKey { get; set; }
        public string flocation { get; set; }
        public string fcollaborator { get; set; }
        public decimal ftype { get; set; }
        public decimal fpriority { get; set; }
        public string frequestDD { get; set; }
        public decimal fqty { get; set; }
        public string fcommitDD { get; set; }
        public string fcustOrder { get; set; }
        public string fcustDemand { get; set; }
        public string fuom { get; set; }
        public string fbuilder { get; set; }
        public string fbDate { get; set; }
        public string freviser { get; set; }
        public string frDate { get; set; }
        public decimal FBOMTYPE { get; set; }
        public string FBOMKEY { get; set; }
    }
}
