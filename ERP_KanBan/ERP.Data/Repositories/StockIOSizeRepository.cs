using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class StockIOSizeRepository : Bases.BaseRepository<StockIOSize>
    {
        public StockIOSizeRepository(DbContext db) : base(db)
        {
        }
    }
}