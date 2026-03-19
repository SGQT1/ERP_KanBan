using System.Threading.Tasks;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class ProjectRepository : BaseRepository<Project>
    {
        public ProjectRepository(DbContext db) : base(db)
        {
        }       
    }
}