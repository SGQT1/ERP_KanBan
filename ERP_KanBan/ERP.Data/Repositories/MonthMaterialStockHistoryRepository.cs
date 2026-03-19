using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MonthMaterialStockHistoryRepository : Bases.BaseRepository<MonthMaterialStockHistory>
    {
        public MonthMaterialStockHistoryRepository(DbContext db) : base(db)
        {
        }
    }
}