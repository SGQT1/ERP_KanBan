using System;

namespace ERP.Models.Views
{
    public class ProjectMaterialProfile
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public string Thickness { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public decimal LocaleId { get; set; }
        public int BrandId { get; set; }
    }
}