using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersBKRepository : Bases.BaseRepository<OrdersBK>
    {
        public OrdersBKRepository(DbContext db) : base(db) { }
    }
}