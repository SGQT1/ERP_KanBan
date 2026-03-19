using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MaterialStockRepository : Bases.BaseRepository<MaterialStock>
    {
        public MaterialStockRepository(DbContext db) : base(db)
        {
        }
    }
}