using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MaterialStockBatchCostRepository : Bases.BaseRepository<MaterialStockBatchCost>
    {
        public MaterialStockBatchCostRepository(DbContext db) : base(db)
        {
        }
    }
}