using System.Collections.Generic;

namespace ERP.Models.Views.Projects
{
    public class ProjectProfileGroup : ProjectProfile
    {
        public IEnumerable<ProjectGroup> Projects { get; set; }

        public IEnumerable<ProjectItemProfile> Items { get; set; }
    }
}