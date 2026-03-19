using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MonthMaterialStockIOInRepository : Bases.BaseRepository<MonthMaterialStockIOIn>
    {
        public MonthMaterialStockIOInRepository(DbContext db) : base(db)
        {
        }
    }
}