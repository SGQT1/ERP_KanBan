using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class RelOrdersRepository : BaseRepository<REP_ORDERS>
    {
        public RelOrdersRepository(DbContext db) : base(db) { }
    }
}
