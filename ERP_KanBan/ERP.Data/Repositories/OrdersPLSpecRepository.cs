using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersPLSpecRepository : Bases.BaseRepository<OrdersPLSpec>
    {
        public OrdersPLSpecRepository(DbContext db) : base(db) { }
    }
}