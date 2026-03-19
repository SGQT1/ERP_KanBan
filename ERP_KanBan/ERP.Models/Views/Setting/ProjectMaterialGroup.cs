using ERP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views.Setting
{
    public class ProjectMaterialProfileGroup : ProjectMaterialProfile
    {
        public IEnumerable<ProjectMaterial> ProjectMaterials { get; set; }
    }
}
