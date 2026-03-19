using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersPLRepository : Bases.BaseRepository<OrdersPL>
    {
        public OrdersPLRepository(DbContext db) : base(db) { }
    }
}