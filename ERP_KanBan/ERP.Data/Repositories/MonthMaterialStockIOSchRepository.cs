using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MonthMaterialStockIOSchRepository : Bases.BaseRepository<MonthMaterialStockIOSch>
    {
        public MonthMaterialStockIOSchRepository(DbContext db) : base(db)
        {
        }
    }
}