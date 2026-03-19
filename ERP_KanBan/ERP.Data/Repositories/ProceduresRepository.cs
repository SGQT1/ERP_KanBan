using System.Threading.Tasks;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class ProceduresRepository : BaseRepository<Procedures>
    {
        public ProceduresRepository(DbContext db) : base(db)
        {
        }       
    }
}