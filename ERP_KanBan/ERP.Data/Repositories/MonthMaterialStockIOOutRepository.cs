using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MonthMaterialStockIOOutRepository : Bases.BaseRepository<MonthMaterialStockIOOut>
    {
        public MonthMaterialStockIOOutRepository(DbContext db) : base(db)
        {
        }
    }
}