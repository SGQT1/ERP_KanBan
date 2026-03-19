using System.Collections.Generic;

namespace ERP.Models.Views.Projects
{
    public class ProjectGroup : Project
    {
        public IEnumerable<ProjectItem> Items { get; set; }
    }
}