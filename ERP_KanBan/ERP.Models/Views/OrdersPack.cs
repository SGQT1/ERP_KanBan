using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersPack
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public string Edition { get; set; }
        public decimal ItemInnerSize { get; set; }
        public string RefDisplaySize { get; set; }
        public decimal PairOfCTN { get; set; }
        public int CTNS { get; set; }
        public int GroupBy { get; set; }
        public decimal NWOfCTN { get; set; }
        public decimal GWOfCTN { get; set; }
        public decimal MEAS { get; set; }
        public decimal CBM { get; set; }
        public decimal AdjQty { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid MsreplTranVersion { get; set; }
        public string PLId { get; set; }
    }
}
