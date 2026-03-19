using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersPackRepository : Bases.BaseRepository<OrdersPack>
    {
        public OrdersPackRepository(DbContext db) : base(db) { }
    }
}