using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MaterialStockSizeRepository : Bases.BaseRepository<MaterialStockSize>
    {
        public MaterialStockSizeRepository(DbContext db) : base(db)
        {
        }
    }
}