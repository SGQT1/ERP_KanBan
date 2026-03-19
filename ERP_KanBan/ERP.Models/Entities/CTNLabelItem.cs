using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class CTNLabelItem
    {
        public CTNLabelItem()
        {
            CTNLabelStockIn = new HashSet<CTNLabelStockIn>();
            CTNLabelStockReject = new HashSet<CTNLabelStockReject>();
            CTNLabelStockout = new HashSet<CTNLabelStockout>();
        }

        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal CTNLabelId { get; set; }
        public int GroupBy { get; set; }
        public int PackingType { get; set; }
        public string LabelCode { get; set; }
        public string MinRefDisplaySize { get; set; }
        public string MaxRefDisplaySize { get; set; }
        public decimal SubPackingQty { get; set; }
        public decimal? SubNetWeight { get; set; }
        public decimal? SubGrossWeight { get; set; }
        public decimal? SubMEAS { get; set; }
        public decimal? SubCBM { get; set; }
        public int SeqNo { get; set; }
        public string SubLabelCode { get; set; }
        public string DeptNo { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual CTNLabel CTNLabel { get; set; }
        public virtual ICollection<CTNLabelStockIn> CTNLabelStockIn { get; set; }
        public virtual ICollection<CTNLabelStockReject> CTNLabelStockReject { get; set; }
        public virtual ICollection<CTNLabelStockout> CTNLabelStockout { get; set; }
    }
}
