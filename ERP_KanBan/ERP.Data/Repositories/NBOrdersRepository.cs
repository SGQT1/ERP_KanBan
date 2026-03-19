using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class NBOrdersRepository : Bases.BaseRepository<NBOrders>
    {
        public NBOrdersRepository(DbContext db) : base(db) { }
    }
}