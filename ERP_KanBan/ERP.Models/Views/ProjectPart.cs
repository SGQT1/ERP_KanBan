using System;

namespace ERP.Models.Views
{
    public  class ProjectPart
    {
        public int Id { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int BrandId { get; set; }
    }
}