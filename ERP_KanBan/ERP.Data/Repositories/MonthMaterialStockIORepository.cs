using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MonthMaterialStockIORepository : Bases.BaseRepository<MonthMaterialStockIO>
    {
        public MonthMaterialStockIORepository(DbContext db) : base(db)
        {
        }
    }
}