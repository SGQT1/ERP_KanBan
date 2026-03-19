using System;

namespace ERP.Models.Views.Projects
{
    public class ProjectItem
    {
        public int Id { get; set; }
        public decimal ProjectId { get; set; }
        public int Division { get; set; }
        public int ProjectPartId { get; set; }
        public int ProjectMaterialId { get; set; }
        public int? MaterialUnitId { get; set; }
        public string ProjectMaterial { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public int BrandId { get; set; }

        public string ProjectPartNo { get; set; }
        public string ProjectPartName { get; set; }
        public string ProjectMaterialName { get; set; }
        public string Supplier { get; set; }
        public string Thickness { get; set; }
        public string Color { get; set; }
        public string MaterialUnit { get; set; }
    }
}
