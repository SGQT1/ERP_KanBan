using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class StockIOMonthSchRepository : Bases.BaseRepository<StockIOMonthSch>
    {
        public StockIOMonthSchRepository(DbContext db) : base(db)
        {
        }
    }
}