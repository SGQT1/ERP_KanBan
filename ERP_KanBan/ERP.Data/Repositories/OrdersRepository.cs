using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersRepository : Bases.BaseRepository<Orders>
    {
        public OrdersRepository(DbContext db) : base(db) { }
    }
}