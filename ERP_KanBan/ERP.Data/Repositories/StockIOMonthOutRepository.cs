using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class StockIOMonthOutRepository : Bases.BaseRepository<StockIOMonthOut>
    {
        public StockIOMonthOutRepository(DbContext db) : base(db)
        {
        }
    }
}