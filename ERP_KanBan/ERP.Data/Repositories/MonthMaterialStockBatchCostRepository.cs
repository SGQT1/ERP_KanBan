using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MonthMaterialStockBatchCostRepository : Bases.BaseRepository<MonthMaterialStockBatchCost>
    {
        public MonthMaterialStockBatchCostRepository(DbContext db) : base(db)
        {
        }
    }
}