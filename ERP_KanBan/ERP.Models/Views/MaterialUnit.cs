using System;

namespace ERP.Models.Views
{
    public class MaterialUnit
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public int LocaleId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}