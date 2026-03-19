using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class OrdersTD
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal OrdersId { get; set; }
        public string NOrderNo { get; set; }
        public decimal? Qty { get; set; }
    }
    //Id
    //LocaleId
    //msrepl_tran_version
    //OrdersId    訂單PK
    //NOrderNo    新單管制編號
    //Qty 訂單數
}
