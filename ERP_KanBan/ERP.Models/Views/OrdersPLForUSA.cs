using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersPLForUSA
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal RefLocaleId { get; set; }
        public decimal RefOrdersId { get; set; }
        public string Edition { get; set; }
        public string MinCNo { get; set; }
        public string MaxCNo { get; set; }
        public string CTNL { get; set; }
        public string CTNW { get; set; }
        public string CTNH { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
