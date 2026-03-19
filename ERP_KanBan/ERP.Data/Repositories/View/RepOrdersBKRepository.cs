using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class RelOrdersBKRepository : BaseRepository<REP_ORDERSBK>
    {
        public RelOrdersBKRepository(DbContext db) : base(db) { }
    }
}
