using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class PUMAOrdersRepository : Bases.BaseRepository<PUMAOrders>
    {
        public PUMAOrdersRepository(DbContext db) : base(db) { }
    }
}