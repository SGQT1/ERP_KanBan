using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class OrgUnit
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string UnitNo { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public DateTime FoundingDate { get; set; }
        public decimal ManagerStaffId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal ParentId { get; set; }
    }
}
