using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class CodeItem
    {
        public decimal Id { get; set; }
        public string CodeType { get; set; }
        public int CodeNo { get; set; }
        public string NameTW { get; set; }
        public string NameEng { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ReferenceCodeNo { get; set; }
        public decimal LocaleId { get; set; }
        public bool Disable { get; set; }
    }
    //Id
    //CodeType    類別代號
    //CodeNo  號碼
    //NameTW  中文名稱
    //NameEng 英文名稱
    //ModifyUserName  修改人
    //LastUpdateTime  修改日期
    //ReferenceCodeNo 顯示名稱(部分使用, 如幣別)...20150729
    //LocaleId 歸屬分公司Id(ref. Company.Id)(PK)
}
