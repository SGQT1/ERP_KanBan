using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Code
    {
        public decimal Id { get; set; }
        public string CodeType { get; set; }
        public string TypeName { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
    }
    //Id(PK)
    //CodeType 類別代號
    //TypeName 類別名稱
    //ModifyUserName 修改人
    //LastUpdateTime 修改日期
    //LocaleId 歸屬分公司Id(ref. Company.Id)(PK)
    //msrepl_tran_version
}
