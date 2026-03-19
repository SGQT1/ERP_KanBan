using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ProjectService : EntityService<Project>
    {
        protected new ProjectRepository Repository { get { return base.Repository as ProjectRepository; } }

        public ProjectService(ProjectRepository repository) : base(repository)
        {
        }       
    }
}