using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class StockIOMonthInRepository : Bases.BaseRepository<StockIOMonthIn>
    {
        public StockIOMonthInRepository(DbContext db) : base(db)
        {
        }
    }
}