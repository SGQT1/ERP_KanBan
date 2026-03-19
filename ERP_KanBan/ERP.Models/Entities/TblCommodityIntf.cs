using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class TblCommodityIntf
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public string fkey { get; set; }
        public string fid { get; set; }
        public string fdesc { get; set; }
        public string fversion { get; set; }
        public string fuom { get; set; }
        public string ftype { get; set; }
        public decimal fcomType { get; set; }
        public string fmajorComGroup { get; set; }
        public decimal fisPrimCom { get; set; }
        public string fbuilder { get; set; }
        public string fbDate { get; set; }
        public string freviser { get; set; }
        public string frDate { get; set; }
    }
}
