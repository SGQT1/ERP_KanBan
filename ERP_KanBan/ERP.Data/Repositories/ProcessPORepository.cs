using System.Threading.Tasks;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class ProcessPORepository : BaseRepository<ProcessPO>
    {
        public ProcessPORepository(DbContext db) : base(db)
        {
        }       
    }
}