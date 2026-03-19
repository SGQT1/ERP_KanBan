using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersPLForUSARepository : Bases.BaseRepository<OrdersPLForUSA>
    {
        public OrdersPLForUSARepository(DbContext db) : base(db) { }
    }
}